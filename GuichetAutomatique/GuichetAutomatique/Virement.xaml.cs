using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace GuichetAutomatique
{
    public partial class Virement : Window
    {
        SqlConnection connexion;
        SqlDataAdapter da;
        DataSet dsGABVir;
        private UtilisateurActif UtilisateurCourant;
        private decimal LimiteCreditProv;
        private decimal SoldeProv;

        public Virement(int IdUtilisateur)
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            dsGABVir = new DataSet();
            UtilisateurCourant = new UtilisateurActif { IdUtilisateur = IdUtilisateur };
            // Populer le comboBox uniquement du ou des comptes chèques de l'utilisateur courant.
            cptProvenanceComboBox();
        }

        private void cptProvenanceComboBox()
        {
            try
            {
                // S'assurer que le comboBox soit vide de code au départ.
                cptProv.Items.Clear();
                // Ouverture de la connexion.
                connexion.Open();
                // Requête de sélection afin de populer le comboBox avec les comptes chèques actifs de l'utilisateur
                // Courant.
                string selectionCompte = "SELECT IdCompte FROM Comptes WHERE TypeCompte = 'Chèque' AND " +
                                         "IdUtilisateur = @IdUtilisateur";
                // Création de l'adapteur et de ses paramètres.
                da = new SqlDataAdapter(selectionCompte, connexion);
                da.SelectCommand.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);
                // Populer le dataset.
                da.Fill(dsGABVir, "Comptes");
                cptProv.ItemsSource = dsGABVir.Tables["Comptes"].DefaultView;


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

        private void cptDestinataireComboBox()
        {
            try
            {
                // S'assurer que le comboBox soit vide de code au départ.
                cptDest.Items.Clear();
                // Ouverture de la connexion.
                connexion.Open();
                // Requête de sélection afin de populer le comboBox avec tous les comptes de l'utilisateur courant
                // sauf le compte en provenance.
                string selectionCompteDest = "SELECT IdCompte FROM Comptes WHERE IdUtilisateur = @IdUtilisateur " +
                    "AND IdCompte <> @IdCompteProv";
                da = new SqlDataAdapter(selectionCompteDest, connexion);
                da.SelectCommand.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);
                da.SelectCommand.Parameters.AddWithValue("@IdCompteProv", (int)cptProv.SelectedValue);
                // Populer le dataset.
                da.Fill(dsGABVir, "Comptes");
                // Liaison entre le dataset et la BDD.
                cptDest.ItemsSource = dsGABVir.Tables["Comptes"].DefaultView;
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

        private void cptProv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cptDestinataireComboBox();
        }

        private void cptDest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Sélection d'un compte dans le comboBox.
            if (cptDest.SelectedValue != null && int.TryParse(cptDest.SelectedValue.ToString(), out int cptProvSelect))
            {
                // Le bouton "OK" sera désactivé si le compte de provenance choisi est le même que celui choisi dans le
                // comboBox du compte destinataire.
                OKVirement.IsEnabled = (cptProvSelect != (int)cptProv.SelectedValue);
            }
        }

        private void OKVirement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMontantVir.Text) && decimal.TryParse(txtMontantVir.Text, out decimal montantVir))
                {
                    int compteProvId = (int)cptProv.SelectedValue;
                    int compteDestId = (int)cptDest.SelectedValue;

                    // Ouverture de la connexion.
                    connexion.Open();

                    // Création de l'objet command et de ses paramètres pour la procédure stockée.
                    SqlCommand cmd = new SqlCommand("sp_Virement", connexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCompteProv", compteProvId);
                    cmd.Parameters.AddWithValue("@IdCompteDest", compteDestId);
                    cmd.Parameters.AddWithValue("@IdUtilisateur", UtilisateurCourant.IdUtilisateur);
                    cmd.Parameters.AddWithValue("@MontantTransaction", montantVir);

                    // Exécution de la requête.
                    cmd.ExecuteNonQuery();

                    // Confirmation de la transaction en cas de succès et demande à l'utilisateur courant s'il désire
                    // retourner au menu principal ou quitter l'application.
                    MessageBoxResult resultat = MessageBox.Show($"Virement de {montantVir} $ effectué avec succès du " +
                                                                $"compte {compteProvId} au compte {compteDestId}.\n\n" +
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
                    MessageBox.Show("Veuillez entrer un montant valide.");
                }
            }
            catch (Exception ex)
            {
                //Si erreur
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de la connexion.
                connexion.Close();
            }
        }

        private void AnnulerVir_Click(object sender, RoutedEventArgs e)
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
