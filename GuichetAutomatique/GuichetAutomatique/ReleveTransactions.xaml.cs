using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour ReleveTransactions.xaml
    /// </summary>
    public partial class ReleveTransactions : Window
    {
        SqlConnection connexion;
        SqlDataAdapter da;
        DataSet dsTransactions;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif UtilisateurCourant;
        public ReleveTransactions(int IdUtilisateur)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            // Classe permettant de conserver le login de l'utiisateur.
            UtilisateurCourant = new UtilisateurActif { IdUtilisateur = IdUtilisateur };
            dsTransactions = new DataSet();
            AffichageTransactions();
        }

        private void AffichageTransactions()
        {
            try
            {
                // Établissement de la connexion.
                connexion.Open();

                // Création de la requête de sélection qui va chercher toutes les transactions du client.
                string trans = "SELECT * FROM Transactions WHERE IdCompte IN " +
                               "(SELECT IdCompte FROM Comptes WHERE IdUtilisateur = @IdUtilisateur)";
                SqlCommand commande = new SqlCommand(trans, connexion);
                commande.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);

                // Adapteur qui établissant la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(commande);
                da.FillSchema(dsTransactions, SchemaType.Mapped, "Transactions");
                da.Fill(dsTransactions, "Transactions");

                // Liaision de la DataTable au DataGrid.
                dgTransactions.ItemsSource = dsTransactions.Tables["Transactions"].DefaultView;
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

        private void btnRetourReleve_Click(object sender, RoutedEventArgs e)
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
