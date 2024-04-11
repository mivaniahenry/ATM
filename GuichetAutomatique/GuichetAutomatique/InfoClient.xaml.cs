using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour InfoClient.xaml
    /// </summary>
    public partial class InfoClient : Window
    {
        // Déclaration des variables nécessaires au programme.
        SqlConnection connexion;
        SqlDataAdapter da;
        DataSet dsInfoClient;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif UtilisateurCourant;

        public InfoClient(int IdUtilisateur)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            // Classe permettant de conserver le login de l'utiisateur.
            UtilisateurCourant = new UtilisateurActif { IdUtilisateur = IdUtilisateur };
            dsInfoClient = new DataSet();
            AffichageInfoClient();
        }
        private void AffichageInfoClient()
        {
            try
            {
                // Établissement de la connexion.
                connexion.Open();

                // Création de la requête de sélection et de ses paramètres.
                string affichage = "SELECT IdCompte, TypeCompte, Solde FROM Comptes WHERE " +
                                   "IdUtilisateur = @IdUtilisateur";
                SqlCommand commande = new SqlCommand(affichage, connexion);
                commande.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);

                // Adapteur qui établissant la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(commande);
                da.FillSchema(dsInfoClient, SchemaType.Mapped, "Comptes");
                da.Fill(dsInfoClient, "Comptes");

                // Liaision de la DataTable au DataGrid.
                dgInfoClient.ItemsSource = dsInfoClient.Tables["Comptes"].DefaultView;
            }
            catch (Exception ex)
            {
                // Si erreur.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de la connexion.
                connexion.Close();
            }
        }

        private void btnRetourInfoClient_Click(object sender, RoutedEventArgs e)
        {
            // Retour au menu principal.
            Accueil accueil = new Accueil(UtilisateurCourant);
            accueil.Show();
            this.Close();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Quitter l'application.
            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
            Application.Current.Shutdown();
        }
    }
}
