using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace GuichetAutomatique
{
    public partial class PaiementFactures : Window
    {
        SqlConnection connexion;
        SqlDataAdapter da;
        DataSet dsPaiement;
        private UtilisateurActif UtilisateurCourant;

        public PaiementFactures(int IdUtilisateur)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            UtilisateurCourant = new UtilisateurActif { IdUtilisateur = IdUtilisateur };
            dsPaiement = new DataSet();
            // Populate the ComboBox with checking accounts of the current user.
            cptProvenancePayComboBox();
            cptDestinatairePayComboBox();
        }

        private void cptProvenancePayComboBox()
        {
            try
            {
                // S'assurer que le comboBox soit vide de code au départ.
                cptProvPay.Items.Clear();
                // Ouverture de la connexion.
                connexion.Open();
                // Populer le comboBox avec le ou les comptes chèque détenus par le client.
                string cptSource = "SELECT IdCompte FROM Comptes WHERE TypeCompte = 'Chèque' AND " +
                                         "IdUtilisateur = @IdUtilisateur";
                // Création de l'adapteur et de ses paramètres.
                da = new SqlDataAdapter(cptSource, connexion); ;
                da.SelectCommand.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);
                // Populer le ComboBox.
                da.Fill(dsPaiement, "Comptes");
                cptProvPay.ItemsSource = dsPaiement.Tables["Comptes"].DefaultView;
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

        private void cptDestinatairePayComboBox()
        {
            try
            {
                // S'assurer que le comboBox soit vide de code au départ.
                cptDestPay.Items.Clear();
                // Ouverture de la connexion.
                connexion.Open();
                // Populer le comboBox avec le ou les comptes chèque détenus par le client.
                string cptFactures = "SELECT IdFacture FROM Factures WHERE IdUtilisateur = @IdUtilisateur";
                // Création de l'adapteur et de ses paramètres.
                da = new SqlDataAdapter(cptFactures, connexion);
                da.SelectCommand.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);
                // Populer le ComboBox des informations de la table Factures.
                da.Fill(dsPaiement, "Factures");
                cptDestPay.ItemsSource = dsPaiement.Tables["Factures"].DefaultView;
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

        private void OKPaiement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Si un montant est inséré dans le textbox pour effectuer le paiement.
                if (!string.IsNullOrEmpty(txtMontantFacture.Text) && decimal.TryParse(txtMontantFacture.Text, out decimal montantPaiement))
                {
                    int compteProvId = (int)cptProvPay.SelectedValue;

                    // Si une facture est sélectionnée dans le combobox.
                    if (cptDestPay.SelectedValue != null && int.TryParse(cptDestPay.SelectedValue.ToString(), out int idFacture))
                    {
                        // Ouverture de la connexion.
                        connexion.Open();

                        SqlCommand cmd = new SqlCommand("sp_Paiement", connexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdFacture", idFacture);
                        cmd.Parameters.AddWithValue("@IdCompteProv", compteProvId);
                        cmd.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);
                        cmd.Parameters.AddWithValue("@MontantTransaction", montantPaiement);

                        // Exécution de la commande.
                        cmd.ExecuteNonQuery();

                        // Confirmation de la transaction en cas de succès et demande à l'utilisateur courant s'il désire
                        // retourner au menu principal ou quitter l'application.
                        MessageBoxResult resultat = MessageBox.Show($"Paiement de {montantPaiement} $ de la facture {idFacture} effectué avec succès !\n\n" +
                                                                    $"Voulez-vous retourner au menu principal?", "Succès",
                                                                    MessageBoxButton.YesNo, MessageBoxImage.Information);

                        if (resultat == MessageBoxResult.Yes)
                        {
                            Accueil accueil = new Accueil(UtilisateurCourant);
                            accueil.Show();
                            this.Close();
                        }
                        else
                        {
                            // Fermeture de l'application si l'utilisateur courant décide de ne pas retourner au menu principal.
                            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
                            Application.Current.Shutdown();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Veuillez choisir une facture valide.");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez entrer un montant valide.");
                }
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

        private void AnnulerPaiement_Click(object sender, RoutedEventArgs e)
        {
            // Renvoi au menu principal.
            Accueil accueil = new Accueil(UtilisateurCourant);
            accueil.Show();
            this.Close();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Sortie de l'application.
            MessageBox.Show("Merci d'avoir utilisé le guichet de la Banque. \n\nAu revoir !");
            Application.Current.Shutdown();
        }
    }
}
