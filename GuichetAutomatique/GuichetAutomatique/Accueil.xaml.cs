using System.Data.SqlClient;
using System.Windows;

namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        // Déclaration des variable nécessaires au programme.
        SqlConnection connexion;
        private UtilisateurActif UtilisateurCourant;
        public Accueil(UtilisateurActif utilisateurCourant)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            UtilisateurCourant = utilisateurCourant;
        }

        // Ponts permettant d'être rediriger aux pages désirées lorsque leur bouton respectif est cliqué.
        private void Retrait_Click(object sender, RoutedEventArgs e)
        {
            RetraitFonds retrait = new RetraitFonds(UtilisateurCourant.IdUtilisateur);
            retrait.Show();
            this.Close();
        }

        private void Depot_Click(object sender, RoutedEventArgs e)
        {
            DepotFonds depot = new DepotFonds(UtilisateurCourant.IdUtilisateur);
            depot.Show();
            this.Close();
        }

        private void Virement_Click(object sender, RoutedEventArgs e)
        {
            Virement virement = new Virement(UtilisateurCourant.IdUtilisateur);
            virement.Show();
            this.Close();
        }

        private void PaiementFacture_Click(object sender, RoutedEventArgs e)
        {
            PaiementFactures facture = new PaiementFactures(UtilisateurCourant.IdUtilisateur);
            facture.Show();
            this.Close();
        }

        private void InfoCompte_Click(object sender, RoutedEventArgs e)
        {
            InfoClient info = new InfoClient(UtilisateurCourant.IdUtilisateur);
            info.Show();
            this.Close();
        }

        private void ReleveTrans_Click(object sender, RoutedEventArgs e)
        {
            ReleveTransactions releve = new ReleveTransactions(UtilisateurCourant.IdUtilisateur);
            releve.Show();
            this.Close();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Fermeture complète de l'application.
            Application.Current.Shutdown();
        }
    }
}
