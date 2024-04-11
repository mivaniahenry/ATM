using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour RetraitFonds.xaml
    /// </summary>
    public partial class RetraitFonds : Window
    {
        SqlConnection connexion;
        SqlDataAdapter da;
        DataSet dsGuichetAutomatique;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif UtilisateurCourant;
        private DataRowView selectionCompte;


        public RetraitFonds(int IdUtilisateur)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            dsGuichetAutomatique = new DataSet();
            UtilisateurCourant = new UtilisateurActif { IdUtilisateur = IdUtilisateur };
            DonneesRetrait();
        }

        private void DonneesRetrait()
        {
            try
            {
                // Établissement de la connexion.
                connexion.Open();

                // Création de la requête de sélection et de ses paramètres.
                string afficheCpt = "SELECT IdCompte, TypeCompte, Solde FROM Comptes WHERE IdUtilisateur = @IdUtilisateur";
                SqlCommand commande = new SqlCommand(afficheCpt, connexion);
                commande.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);

                // Création de l'adapteur qui établi la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(commande);
                da.FillSchema(dsGuichetAutomatique, SchemaType.Mapped, "Comptes");
                da.Fill(dsGuichetAutomatique, "Comptes");

                // Liaision de la DataTable au DataGrid.
                GridCompteRetrait.ItemsSource = dsGuichetAutomatique.Tables["Comptes"].DefaultView;

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

        private void GridCompteRetrait_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Repère de données par sélection.
            if (GridCompteRetrait.SelectedItem is DataRowView SelectionRange)
            {
                selectionCompte = SelectionRange;
            }
        }

        private void btnSuivantRetrait_Click(object sender, RoutedEventArgs e)
        {
            // Action s'il y a sélection d'un compte.
            if (selectionCompte != null)
            {
                int selectionIdCompte = (int)selectionCompte["IdCompte"];

                // Bascule vers la prochaine étape (page).
                RetraitFonds2 retrait2 = new RetraitFonds2(selectionIdCompte, UtilisateurCourant);
                retrait2.Show();
                this.Close();
            }
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Sortie complète de l'application.
            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
            Application.Current.Shutdown();
        }

        private void btnRetourRetrait_Click(object sender, RoutedEventArgs e)
        {
            // Retour au menu principal.
            Accueil accueil = new Accueil(UtilisateurCourant);
            accueil.Show();
            this.Close();
        }
    }
}
