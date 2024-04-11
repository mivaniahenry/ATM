using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour DepotFonds2.xaml
    /// </summary>
    public partial class DepotFonds2 : Window
    {
        SqlConnection connexion;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif utilisateurActif;
        public DepotFonds2(int IdCompte, UtilisateurActif utilisateurActif)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            this.utilisateurActif = utilisateurActif;
            string noCompteDepot = GetNoCompteDepot(IdCompte);
            labelNoCPT.Content = noCompteDepot;
        }

        private string GetNoCompteDepot(int IdCompte)
        {
            string noCompteDepot = string.Empty;
            try
            {
                // Ouverture de la connexion.
                connexion.Open();
                // Création de notre requête de sélection afin de pouvoir affiché le numéro de compte 
                // dans le label afin que l'utilisateur puisse suivre la transaction en direct.
                string recupCompteDepot = "SELECT IdCompte FROM Comptes WHERE IdCompte = @IdCompte";
                // Création de l'objet SqlCommand et de ses paramètres.
                SqlCommand commande = new SqlCommand(recupCompteDepot, connexion);
                commande.Parameters.AddWithValue("@IdCompte", IdCompte);
                object resultat = commande.ExecuteScalar();

                // Retour des données retrouvées.
                if (resultat != null)
                {
                    noCompteDepot = resultat.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de la connexion.
                connexion.Close();
            }
            return noCompteDepot;
        }

        private void OKDepot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // S'il y a des données saisies dans le textbox.
                if (!string.IsNullOrEmpty(txtMontantDepot.Text) && int.TryParse(txtMontantDepot.Text, out int montantDepot))
                {

                    string noCompteDepot = GetNoCompteDepot(int.Parse(labelNoCPT.Content.ToString()));
                    connexion.Open();
                    SqlCommand commande = new SqlCommand("sp_Depot", connexion);
                    commande.CommandType = CommandType.StoredProcedure;
                    commande.Parameters.AddWithValue("@IdCompte", int.Parse(labelNoCPT.Content.ToString()));
                    commande.Parameters.AddWithValue("@MontantTransaction", montantDepot);
                    commande.ExecuteNonQuery();
                    // Affichage de la confirmation de la transaction et question demandant à l'utilisateur s'il
                    // désire retourner au menu ou quitter.
                    MessageBoxResult result = MessageBox.Show($"Depot de {montantDepot} $ effectué avec succès sur " +
                                                                  $"dans le compte {noCompteDepot}.\n\nVoulez-vous retourner" +
                                                                  $" au menu principal?", "Succès", MessageBoxButton.YesNo,
                                                                  MessageBoxImage.Information);

                    // Si l'utilisateur saisi "oui" il retournera à l'accueil.
                    if (result == MessageBoxResult.Yes)
                    {
                        Accueil accueil = new Accueil(utilisateurActif);
                        accueil.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
                        Application.Current.Shutdown();
                    }

                }
                else
                {
                    // Par exemple si le montant n'est pas un multiple de 10.
                    MessageBox.Show("Veuillez entrer un montant valide.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connexion.Close();
            }
        }

        private void AnnulerDepot_Click(object sender, RoutedEventArgs e)
        {
            // Si l'opération est annulée, l'utilisateur sera redirigé au menu principal.
            Accueil accueil = new Accueil(utilisateurActif);
            accueil.Show();
            this.Close();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Sortie complète de l'application.
            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
            Application.Current.Shutdown();
        }
    }
}
