using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace GuichetAutomatique
{
    public partial class DepotFonds : Window
    {
        // Déclaration des variables nécessaires au programme.
        SqlConnection connexion;
        SqlDataAdapter da;
        DataSet dsGuichetAutomatiqueDepot;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif UtilisateurCourant;
        private DataRowView selectCompte;

        public DepotFonds(int IdUtilisateur)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            // Classe permettant de conserver le login de l'utiisateur.
            UtilisateurCourant = new UtilisateurActif { IdUtilisateur = IdUtilisateur };
            dsGuichetAutomatiqueDepot = new DataSet();
            DonneesDepot();
        }

        private void DonneesDepot()
        {
            try
            {
                // Établissement de la connexion.
                connexion.Open();

                // Création de la requête de sélection et de ses paramètres.
                string selectCPT = "SELECT IdCompte, TypeCompte, Solde FROM Comptes WHERE IdUtilisateur = @IdUtilisateur";
                SqlCommand commande = new SqlCommand(selectCPT, connexion);
                commande.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);

                // Création de l'adapteur qui établi la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(commande);
                da.FillSchema(dsGuichetAutomatiqueDepot, SchemaType.Mapped, "Comptes");
                da.Fill(dsGuichetAutomatiqueDepot, "Comptes");

                // Liaision de la DataTable au DataGrid.
                GridCompteDepot.ItemsSource = dsGuichetAutomatiqueDepot.Tables["Comptes"].DefaultView;
            }
            catch (Exception ex)
            {
                // Message d'erreur si la connexion ou une autre erreur survienne.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de la connexion.
                connexion.Close();
            }
        }

        private void GridCompteDepot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Repère de données par sélection.
            if (GridCompteDepot.SelectedItem is DataRowView selectRange)
            {
                selectCompte = selectRange;
            }
        }


        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Quitter l'application.
            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
            Application.Current.Shutdown();
        }
        private void btnSuivantDepot_Click(object sender, RoutedEventArgs e)
        {
            // Action s'il y a sélection d'un compte.
            if (selectCompte != null)
            {
                int selectIdCompte = (int)selectCompte["IdCompte"];

                // Bascule vers la prochaine étape (page).
                DepotFonds2 depot2 = new DepotFonds2(selectIdCompte, UtilisateurCourant);
                depot2.Show();
                this.Close();
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            // Retour au menu principal.
            Accueil accueil = new Accueil(UtilisateurCourant);
            accueil.Show();
            this.Close();
        }
    }
}
