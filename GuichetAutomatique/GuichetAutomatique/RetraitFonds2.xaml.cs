using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace GuichetAutomatique
{
    public partial class RetraitFonds2 : Window
    {
        SqlConnection connexion;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif utilisateurActif;

        public RetraitFonds2(int IdCompte, UtilisateurActif utilisateurActif)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            this.utilisateurActif = utilisateurActif;
            string noCompte = GetNoCompte(IdCompte);
            LabelnoCPTRetrait.Content = noCompte;
        }

        private string GetNoCompte(int IdCompte)
        {
            string noCompte = string.Empty;
            try
            {
                // Ouverture de la connexion.
                connexion.Open();
                // Création de notre requête de sélection afin de pouvoir affiché le numéro de compte 
                // dans le label afin que l'utilisateur puisse suivre la transaction en direct.
                string recupCompte = "SELECT IdCompte FROM Comptes WHERE IdCompte = @IdCompte";
                // Création de l'objet SqlCommand et de ses paramètres.
                SqlCommand commande = new SqlCommand(recupCompte, connexion);
                commande.Parameters.AddWithValue("@IdCompte", IdCompte);
                object resultat = commande.ExecuteScalar();

                // Retour des données retrouvées.
                if (resultat != null)
                {
                    noCompte = resultat.ToString();
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
            return noCompte;
        }

        private void OKRetrait_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // S'il y a des données saisies dans le textbox.
                if (!string.IsNullOrEmpty(txtMontantRetrait.Text) && int.TryParse(txtMontantRetrait.Text, out int montant))
                {
                    // Si le montant validé est d'un maximum de 1000$.
                    if (montant <= 1000)
                    {
                        string noCompte = GetNoCompte(int.Parse(LabelnoCPTRetrait.Content.ToString()));
                        connexion.Open();
                        SqlCommand commande = new SqlCommand("sp_Retrait", connexion);
                        commande.CommandType = CommandType.StoredProcedure;
                        commande.Parameters.AddWithValue("@IdCompte", int.Parse(LabelnoCPTRetrait.Content.ToString()));
                        commande.Parameters.AddWithValue("@MontantTransaction", montant);
                        commande.ExecuteNonQuery();
                        // Affichage de la confirmation de la transaction et question demandant à l'utilisateur s'il
                        // désire retourner au menu ou quitter.
                        MessageBoxResult result = MessageBox.Show($"Retrait de {montant} $ effectué avec succès sur " +
                                                                  $"le compte {noCompte}.\n\nVoulez-vous retourner" +
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
                            // L'utilisateur quittera l'application.
                            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
                            Application.Current.Shutdown();
                        }
                    }
                    else
                    {
                        // Si le montant va au-delà de 1000$.
                        MessageBox.Show("Le montant maximum de retrait par transaction est de 1000$.");
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
                // S'il y a erreur.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Fermeture de la connexion.
                connexion.Close();
            }
        }

        private void AnnulerRetrait_Click(object sender, RoutedEventArgs e)
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
