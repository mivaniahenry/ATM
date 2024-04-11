using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        SqlConnection connexion;
        // Nécessaire afin de conserver le login en vie durant la navigation.
        private UtilisateurActif utilisateurActif;
        SqlDataAdapter da;
        DataSet dsUtilisateurs;
        private DataRowView selectionClient;
        private DataRowView selectionCompte;
        // Vérifier si un compte ou un client est sélectionné.
        private bool isClientSelected = false;
        private bool isCompteSelected = false;
        public Admin()
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
            this.utilisateurActif = utilisateurActif;
            dsUtilisateurs = new DataSet();
            // Ajout de la table dans le dataset.
            dsUtilisateurs.Tables.Add("Transactions");
            // Désactivation du bouton "Annuler" au départ.
            btnAnnuler.IsEnabled = false;

            // Association des textbox avec leur événement.
            txtId.TextChanged += TextBox_TextChanged;
            txtPrenom.TextChanged += TextBox_TextChanged;
            txtNom.TextChanged += TextBox_TextChanged;
            txtTelephone.TextChanged += TextBox_TextChanged;
            txtCourriel.TextChanged += TextBox_TextChanged;
            txtNIP.TextChanged += TextBox_TextChanged;
            txtTypeUtilisateur.TextChanged += TextBox_TextChanged;

            txtIdCompte.TextChanged += TextBox_TextChanged;
            txtTypeCompte.TextChanged += TextBox_TextChanged;
            txtSolde.TextChanged += TextBox_TextChanged;
            txtLimiteCredit.TextChanged += TextBox_TextChanged;
            txtIdUtilisateur.TextChanged += TextBox_TextChanged;

            dgClients.SelectionChanged += dgClients_SelectionChanged;
            dgComptes.SelectionChanged += dgComptes_SelectionChanged;

            // Appels de fonction nécessaire à l'initialisation du programme.
            AffichageClientsDg();
            AffichageComptesDg();
            AfficherTransactions();
            cbCompteSelection();
            dgTransactions.ItemsSource = dsUtilisateurs.Tables["Transactions"].DefaultView;
            PopulerHyp();
        }

        // Affichage des clients dans la liste.
        private void AffichageClientsDg()
        {
            try
            {
                // Établissement de la connexion.
                connexion.Open();

                // Création de la requête de sélection qui va chercher toutes les clients.
                string clients = "SELECT * FROM Utilisateurs WHERE TypeUtilisateur = 'Client'";
                SqlCommand commande = new SqlCommand(clients, connexion);

                // Adapteur qui établissant la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(commande);
                da.FillSchema(dsUtilisateurs, SchemaType.Mapped, "Utilisateurs");
                da.Fill(dsUtilisateurs, "Utilisateurs");

                // Liaision de la DataTable au DataGrid.
                dgClients.ItemsSource = dsUtilisateurs.Tables["Utilisateurs"].DefaultView;
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

        private void btnAjouterClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Prélèvement des données inscrites dans les textbox.
                string id = txtId.Text;
                string prenom = txtPrenom.Text;
                string nom = txtNom.Text;
                string telephone = txtTelephone.Text;
                string courriel = txtCourriel.Text;
                string nip = txtNIP.Text;
                string typeUtilisateur = txtTypeUtilisateur.Text;
                bool bloque = checkboxBloque.IsChecked ?? false; 

                // S'assurer que tous les informations du nouveau client sont insérées.
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) ||
                    string.IsNullOrWhiteSpace(telephone) || string.IsNullOrWhiteSpace(courriel) || string.IsNullOrWhiteSpace(nip) ||
                    string.IsNullOrWhiteSpace(typeUtilisateur))
                {
                    MessageBox.Show("Veuillez entrer toutes les informations nécessaires.");
                    return;
                }

                // Ajout du nouveau client dans la liste.
                connexion.Open();
                string ajoutClient = "INSERT INTO Utilisateurs (IdUtilisateur, Prenom, Nom, Telephone, Courriel, NIP, TypeUtilisateur, Bloque, TentativeConn) " +
                                "VALUES (@Id, @Prenom, @Nom, @Telephone, @Courriel, @NIP, @TypeUtilisateur, @Bloque, 0)";

                SqlCommand commande = new SqlCommand(ajoutClient, connexion);
                commande.Parameters.AddWithValue("@Id", id);
                commande.Parameters.AddWithValue("@Prenom", prenom);
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Telephone", telephone);
                commande.Parameters.AddWithValue("@Courriel", courriel);
                commande.Parameters.AddWithValue("@NIP", nip);
                commande.Parameters.AddWithValue("@TypeUtilisateur", typeUtilisateur);
                commande.Parameters.AddWithValue("@Bloque", bloque);

                // Exécution de la commande
                commande.ExecuteNonQuery();

                // Rafraichir la liste des clients.
                majListeClients();

                // Vider les textbox après validation.
                ViderClientTextbox();

                MessageBox.Show("Client ajouté avec succès!");
            }
            catch (Exception ex)
            {
                // Si erreur.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connexion.Close();
            }
        }

        private void majListeClients()
        {
            try
            {
                // Supprimer les données courantes du dataset.
                dsUtilisateurs.Tables["Utilisateurs"].Clear();

                // Récupéré les données incluant la mise è jour.
                string clients = "SELECT * FROM Utilisateurs WHERE TypeUtilisateur = 'Client'";
                da.SelectCommand.CommandText = clients;
                da.Fill(dsUtilisateurs, "Utilisateurs");

                // Mettre à jour la vue du tableau.
                dgClients.ItemsSource = dsUtilisateurs.Tables["Utilisateurs"].DefaultView;
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

        private void ViderClientTextbox()
        {
            txtId.Clear();
            txtPrenom.Clear();
            txtNom.Clear();
            txtTelephone.Clear();
            txtCourriel.Clear();
            txtNIP.Clear();
            txtTypeUtilisateur.Clear();
            checkboxBloque.IsChecked = false;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Activer les boutons "Annuler" lorsque qu'une saisie est faite dans l'un des textbox.
            btnAnnuler.IsEnabled = true;
            btnAnnulerCpt.IsEnabled = true;
        }
        private void dgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Vérifier si une donnée est sélectionné.
            if (dgClients.SelectedItem != null)
            {
                // Populer les données du client sélectionné dans le datarow.
                selectionClient = dgClients.SelectedItem as DataRowView;

                // Affichage de la sélection dans les textbox.
                txtId.Text = selectionClient["IdUtilisateur"].ToString();
                txtPrenom.Text = selectionClient["Prenom"].ToString();
                txtNom.Text = selectionClient["Nom"].ToString();
                txtTelephone.Text = selectionClient["Telephone"].ToString();
                txtCourriel.Text = selectionClient["Courriel"].ToString();
                txtNIP.Text = selectionClient["NIP"].ToString();
                txtTypeUtilisateur.Text = selectionClient["TypeUtilisateur"].ToString();
                checkboxBloque.IsChecked = (bool)selectionClient["Bloque"];
                // Désactivation du bouton "Ajouter" et activation du bouton "Modifier".
                isClientSelected = true;
                btnAjouterClient.IsEnabled = false;
                btnModifierClient.IsEnabled = true;
            }
            else
            {   // Réactivation du bouton "Ajouter" et désactivation du bouton "Modifier".
                isClientSelected = false;
                btnAjouterClient.IsEnabled = true;
                btnModifierClient.IsEnabled = false;
                btnAnnuler.IsEnabled = true;
            }
        }
        private void btnModifierClient_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez d'abord un client à modifier.");
                return;
            }

            if (selectionClient != null)
            {
                try
                {
                    // Modifier l'information du client dans la table Utilisateurs.
                    connexion.Open();
                    string majClient = "UPDATE Utilisateurs SET Prenom = @Prenom, Nom = @Nom, Telephone = @Telephone, " +
                                          "Courriel = @Courriel, NIP = @NIP, TypeUtilisateur = @TypeUtilisateur, Bloque = @Bloque " +
                                         "WHERE IdUtilisateur = @Id";

                    SqlCommand commande = new SqlCommand(majClient, connexion);
                    commande.Parameters.AddWithValue("@Id", txtId.Text);
                    commande.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                    commande.Parameters.AddWithValue("@Nom", txtNom.Text);
                    commande.Parameters.AddWithValue("@Telephone", txtTelephone.Text);
                    commande.Parameters.AddWithValue("@Courriel", txtCourriel.Text);
                    commande.Parameters.AddWithValue("@NIP", txtNIP.Text);
                    commande.Parameters.AddWithValue("@TypeUtilisateur", txtTypeUtilisateur.Text);
                    commande.Parameters.AddWithValue("@Bloque", checkboxBloque.IsChecked ?? false);

                    commande.ExecuteNonQuery();

                    // Mettre à jour la liste client.
                    majListeClients();

                    MessageBox.Show("Client modifié avec succès!");
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
            else
            {
                MessageBox.Show("Sélectionnez un client à modifier.");
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // Vde les textbox.
            ViderClientTextbox();

            // Désactivation du bouton.
            btnAnnuler.IsEnabled = false;
            btnAjouterClient.IsEnabled = true;
        }

        private void AffichageComptesDg()
        {
            try
            {
                // Établissement de la connexion.
                connexion.Open();

                // Création de la requête de sélection qui va chercher toutes les copmtes clients.
                string comptes = "SELECT * FROM Comptes";
                SqlCommand commandeCPT = new SqlCommand(comptes, connexion);

                // Adapteur qui établissant la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(commandeCPT);
                da.FillSchema(dsUtilisateurs, SchemaType.Mapped, "Comptes");
                da.Fill(dsUtilisateurs, "Comptes");

                // Afin de pouvoir faire une mise à jour.
                SqlCommandBuilder cmdUpdate = new SqlCommandBuilder(da);

                // Liaision de la DataTable au DataGrid.
                dgComptes.ItemsSource = dsUtilisateurs.Tables["Comptes"].DefaultView;
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

        private void btnAjouterCpt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Prélèvement des données inscrites dans les textbox.
                string idCompte = txtIdCompte.Text;
                string typeCompte = txtTypeCompte.Text;

                // Valider la conversion pour les champs Solde et LimitCredit
                if (!decimal.TryParse(txtSolde.Text, out decimal solde) ||
                    !decimal.TryParse(txtLimiteCredit.Text, out decimal limiteCredit))
                {
                    MessageBox.Show("Veuillez entrer des valeurs numériques valides pour Solde et Limite de crédit.");
                    return;
                }

                string idUtilisateur = txtIdUtilisateur.Text;

                // S'assurer que toutes les informations du nouveau compte sont insérées.
                if (string.IsNullOrWhiteSpace(idCompte) || string.IsNullOrWhiteSpace(typeCompte) ||
                    string.IsNullOrWhiteSpace(txtSolde.Text) || string.IsNullOrWhiteSpace(txtLimiteCredit.Text) ||
                    string.IsNullOrWhiteSpace(idUtilisateur))
                {
                    MessageBox.Show("Veuillez entrer toutes les informations nécessaires.");
                    return;
                }

                // Ajout du nouveau compte dans la liste.
                connexion.Open();
                string ajoutCompte = "INSERT INTO Comptes (IdCompte, TypeCompte, Solde, LimiteCredit, IdUtilisateur) " +
                                    "VALUES (@IdCompte, @TypeCompte, @Solde, @LimiteCredit, @IdUtilisateur)";

                SqlCommand commande = new SqlCommand(ajoutCompte, connexion);
                commande.Parameters.AddWithValue("@IdCompte", idCompte);
                commande.Parameters.AddWithValue("@TypeCompte", typeCompte);
                commande.Parameters.Add("@Solde", SqlDbType.Decimal).Value = solde;
                commande.Parameters.Add("@LimiteCredit", SqlDbType.Decimal).Value = limiteCredit;
                commande.Parameters.AddWithValue("@IdUtilisateur", idUtilisateur);

                // Exécution de la commande
                commande.ExecuteNonQuery();

                // Rafraîchir la liste des comptes.
                majListeComptes();

                MessageBox.Show("Compte ajouté avec succès!");

                // Vider les textbox après validation.
                ViderCompteTextbox();
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

        private void majListeComptes()
        {
            try
            {
                // Supprimer les données courantes du dataset.
                dsUtilisateurs.Tables["Comptes"].Clear();

                // Récupéré les données incluant la mise è jour.
                string comptes = "SELECT * FROM Comptes";
                da.SelectCommand.CommandText = comptes;
                da.Fill(dsUtilisateurs, "Comptes");

                // Mettre à jour la vue du tableau.
                dgComptes.ItemsSource = dsUtilisateurs.Tables["Comptes"].DefaultView;
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
        private void ViderCompteTextbox()
        {
            txtIdCompte.Clear();
            txtTypeCompte.Clear();
            txtSolde.Clear();
            txtLimiteCredit.Clear();
            txtIdUtilisateur.Clear();
        }

        private void btnModifierCpt_Click(object sender, RoutedEventArgs e)
        {
            if (dgComptes.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez d'abord un compte à modifier.");
                return;
            }

            if (selectionCompte != null)
            {
                try
                {
                    // Valider la conversion pour les champs Solde et LimitCredit
                    if (!decimal.TryParse(txtSolde.Text, out decimal solde) ||
                        !decimal.TryParse(txtLimiteCredit.Text, out decimal limiteCredit))
                    {
                        MessageBox.Show("Veuillez entrer des valeurs numériques valides pour Solde et Limite de crédit.");
                        return;
                    }

                    // Modifier l'information du compte dans la table Comptes.
                    connexion.Open();
                    string updateCompte = "UPDATE Comptes SET TypeCompte = @TypeCompte, Solde = @Solde, " +
                                          "LimiteCredit = @LimiteCredit, IdUtilisateur = @IdUtilisateur " +
                                          "WHERE IdCompte = @Id";

                    SqlCommand commande = new SqlCommand(updateCompte, connexion);
                    commande.Parameters.AddWithValue("@Id", txtIdCompte.Text);
                    commande.Parameters.AddWithValue("@TypeCompte", txtTypeCompte.Text);
                    commande.Parameters.Add("@Solde", SqlDbType.Decimal).Value = solde;
                    commande.Parameters.Add("@LimiteCredit", SqlDbType.Decimal).Value = limiteCredit;
                    commande.Parameters.AddWithValue("@IdUtilisateur", txtIdUtilisateur.Text);

                    commande.ExecuteNonQuery();

                    // Mettre à jour la liste des comptes.
                    majListeComptes();

                    MessageBox.Show("Compte modifié avec succès!");

                    // Vider les textbox après validation.
                    ViderCompteTextbox();

                    btnAnnulerCpt.IsEnabled = false;
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
            else
            {
                MessageBox.Show("Sélectionnez un compte à modifier.");
            }
        }

        private void btnAnnulerCpt_Click(object sender, RoutedEventArgs e)
        {
            // Vider les textbox.
            ViderCompteTextbox();
            btnAnnulerCpt.IsEnabled = false;
            // Activation du bouton "Ajouter".
            btnAjouterCpt.IsEnabled = true;
        }

        private void dgComptes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Vérifier si un compte est sélectionné dans le tableau.
            if (dgComptes.SelectedItem != null)
            {
                // Populer le datarow view.
                selectionCompte = dgComptes.SelectedItem as DataRowView;

                // Populer les textbox avec la sélection.
                txtIdCompte.Text = selectionCompte["IdCompte"].ToString();
                txtTypeCompte.Text = selectionCompte["TypeCompte"].ToString();
                txtSolde.Text = selectionCompte["Solde"].ToString();
                txtLimiteCredit.Text = selectionCompte["LimiteCredit"].ToString();
                txtIdUtilisateur.Text = selectionCompte["IdUtilisateur"].ToString();

                // Activation des boutons nécessaires.
                isCompteSelected = true;
                btnAnnulerCpt.IsEnabled = true;
                btnModifierCpt.IsEnabled = true;
            }
            else
            {
                // Activation et désactivation des boutons nécessaires.
                isCompteSelected = false;
                btnAjouterCpt.IsEnabled = true;
                btnModifierCpt.IsEnabled = false;
                btnAnnulerCpt.IsEnabled = true;
            }
        }

        private void epargne1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifier si la table Comptes existe dans le dataset.
                if (dsUtilisateurs.Tables.Contains("Comptes"))
                {
                    // Boucle récupérant les données de type "Épargne".
                    foreach (DataRow row in dsUtilisateurs.Tables["Comptes"].Rows)
                    {
                        string typeCompte = row["TypeCompte"].ToString();

                        if (typeCompte == "Épargne")
                        {
                            // Calcul du taux d'intérêt de 1%.
                            decimal soldeCourant = Convert.ToDecimal(row["Solde"]);
                            decimal interet1 = soldeCourant * 0.01m;

                            // Mise à jour des comptes.
                            row["Solde"] = soldeCourant + interet1;
                        }
                    }

                    // Mise à jour de la BDD à l'aide de l'adapteur.
                    da.Update(dsUtilisateurs, "Comptes");

                    // Mise à jour du datagrid.
                    majListeComptes();

                    // Si succès
                    MessageBox.Show("Intérêts payés avec succès!");
                }
                else
                {
                    // Si échec.
                    MessageBox.Show("Le jeu de données des comptes n'est pas correctement initialisé.");
                }
            }
            catch (Exception ex)
            {
                // Si erreur.
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAugMarge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifier si la table Comptes existe dans le dataset.
                if (dsUtilisateurs.Tables.Contains("Comptes"))
                {
                    // Boucle récupérant les données de type "Marge de crédit".
                    foreach (DataRow row in dsUtilisateurs.Tables["Comptes"].Rows)
                    {
                        string typeCompte = row["TypeCompte"].ToString();

                        if (typeCompte == "Marge de crédit")
                        {
                            // Calcul du taux d'augmentation du solde (5%)
                            decimal margeSoldeCourant = Convert.ToDecimal(row["Solde"]);
                            decimal nouveauSolde = margeSoldeCourant * 1.05m;

                            // Mise à jour des soldes.
                            row["Solde"] = nouveauSolde;
                        }
                    }

                    // Mise à jour BDD.
                    da.Update(dsUtilisateurs, "Comptes");

                    // Afficher la mise à jour dans le datagrid.
                    majListeComptes();

                    MessageBox.Show("Soldes augmentée avec succès!");
                }
                else
                {
                    // Si échec.
                    MessageBox.Show("Le jeu de données des comptes n'est pas correctement initialisé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AfficherTransactions()
        {
            try
            {
                // Ouverture de la connexion.
                connexion.Open();

                // Récupération de la table Transaction.
                string requete = "SELECT * FROM Transactions";
                SqlCommand cmd = new SqlCommand(requete, connexion);

                // Adapteur qui établissant la liaison entre le dataset et la bdd.
                da = new SqlDataAdapter(cmd);
                // Nettoyer toutes données qui pourraient figurer dans le tableau.
                dsUtilisateurs.Tables["Transactions"].Clear();
                da.Fill(dsUtilisateurs, "Transactions");

                // Liaison.
                dgTransactions.ItemsSource = dsUtilisateurs.Tables["Transactions"].DefaultView;
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

        private void cbCompteSelection()
        {
            try
            {
                // Ouverture de la connexion.
                connexion.Open();

                // Récupérer les comptes pour affichage de transaction.
                string requete = "SELECT IdCompte FROM Comptes";
                SqlCommand cmd = new SqlCommand(requete, connexion);
                da = new SqlDataAdapter(cmd);
                dsUtilisateurs.Tables["Transactions"].Clear();
                da.Fill(dsUtilisateurs, "Transactions");

                // Liaison.
                cbCompte.ItemsSource = dsUtilisateurs.Tables["Transactions"].DefaultView;
                cbCompte.DisplayMemberPath = "IdCompte";
                cbCompte.SelectedIndex = -1;
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

        private void cbCompte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbCompte.SelectedItem != null)
                {
                    if (cbCompte.SelectedItem is DataRowView selectedRange)
                    {
                        if (selectedRange.Row["IdCompte"] is not DBNull)
                        {
                            string selectNumCpt = selectedRange["IdCompte"].ToString();

                            // Apply a filter to the data source based on the selected account.
                            DataView filtre = dsUtilisateurs.Tables["Transactions"].DefaultView;
                            filtre.RowFilter = $"IdCompte = '{selectNumCpt}'";
                            filtre.Sort = "IdCompte ASC";

                            dgTransactions.ItemsSource = filtre;
                        }
                        else
                        {
                            // If blank selection, show all transactions.
                            DataView filtre = dsUtilisateurs.Tables["Transactions"].DefaultView;
                            filtre.RowFilter = string.Empty;
                            filtre.Sort = "IdCompte ASC";

                            dgTransactions.ItemsSource = filtre;
                        }
                    }
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

        private void btnAjoutArgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // S'il y a ajout de billets. 
                if (!decimal.TryParse(txtArgent.Text, out decimal montantAjout) || montantAjout <= 0)
                {
                    MessageBox.Show("Veuillez entrer un montant valide à ajouter.");
                    return;
                }

                // Calcul du nouveau solde du GAB après dépôt.
                decimal soldeCourant = SoldeGAB();
                decimal nouvSolde = soldeCourant + montantAjout;

                // Vérifier si le dépôt dépasse la limite autorisée.
                if (nouvSolde > 20000.00m)
                {
                    MessageBox.Show("Le solde du guichet ne peut pas dépasser 20 000 $.");
                    return;
                }

                majSoldeGAB(nouvSolde);

                MessageBox.Show("Montant ajouté avec succès!");

                // Vider la boîte.
                txtArgent.Clear();
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

        private decimal SoldeGAB()
        {
            try
            {
                connexion.Open();

                // Récupération du solde du guichet dans la bdd.
                string solde = "SELECT ArgentPapier FROM GABArgentPapier WHERE IdArgent = 1";
                SqlCommand cmd = new SqlCommand(solde, connexion);
                // Calcul du solde du guichet.
                object resultat = cmd.ExecuteScalar();
                if (resultat != null && resultat != DBNull.Value)
                {
                    // Conversion en décimal.
                    return Convert.ToDecimal(resultat);
                }
                return 0.00m;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0.00m;
            }
            finally
            {
                // Close the connection.
                connexion.Close();
            }
        }

        private void majSoldeGAB(decimal montant)
        {
            try
            {
                connexion.Open();

                // Mise à jour de la table GABArgentPapier dans la bdd.
                string update = "UPDATE GABArgentPapier SET ArgentPapier = @montant WHERE IdArgent = 1";
                SqlCommand cmd = new SqlCommand(update, connexion);
                cmd.Parameters.AddWithValue("@montant", montant);
                cmd.ExecuteNonQuery();
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

        private void btnAnnulerAjoutPapier_Click(object sender, RoutedEventArgs e)
        {
            // Effacer les données dans le textbox.
            txtArgent.Clear();
        }

        // Insertion des données dans le combobox.
        private void PopulerHyp()
        {
            try
            {
                connexion.Open();

                string query = "SELECT * FROM Comptes WHERE TypeCompte = 'Hypothèque'";
                SqlCommand cmd = new SqlCommand(query, connexion);

                da = new SqlDataAdapter(cmd);
                dsUtilisateurs.Tables["Comptes"].Clear();
                da.Fill(dsUtilisateurs, "Comptes");

                listeHyp.ItemsSource = dsUtilisateurs.Tables["Comptes"].DefaultView;
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
 
        private void btnPrelever_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Si aucune sélection.
                if (listeHyp.SelectedItem == null)
                {
                    MessageBox.Show("Sélectionnez un compte à partir duquel prélever.");
                    return;
                }

                DataRowView select = listeHyp.SelectedItem as DataRowView;
                int hypothequeId = Convert.ToInt32(select["IdCompte"]);
                decimal soldeCompte = Convert.ToDecimal(select["Solde"]);
                string typeCompte = select["TypeCompte"].ToString();

                if (typeCompte != "Hypothèque")
                {
                    MessageBox.Show("Vous ne pouvez prélever que des comptes de type 'Hypothèque'.");
                    return;
                }

                // Si montant indiqué n'est pas une donnée valide.
                if (!decimal.TryParse(txtPrelevement.Text, out decimal montantPrelevement) || montantPrelevement <= 0)
                {
                    MessageBox.Show("Veuillez entrer un montant de retrait valide.");
                    return;
                }

                // Vérifier si le montant du solde est suffisant pour couvrir le prélèvement.
                if (montantPrelevement > soldeCompte)
                {
                    MessageBox.Show("Le montant de retrait dépasse le solde du compte.");
                    return;
                }

                // Update the account balance after withdrawal.
                decimal newSolde = soldeCompte - montantPrelevement;
                UpdateAccountBalance(hypothequeId, newSolde);

                MessageBox.Show("Retrait effectué avec succès!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                listeHyp.SelectedIndex = -1;
                txtPrelevement.Clear();
            }
        }

        private void UpdateAccountBalance(int hypothequeId, decimal newSolde)
        {
            try
            {
                connexion.Open();

                // Update the account balance in the database.
                string update = "UPDATE Comptes SET Solde = @newSolde WHERE IdCompte = @hypothequeId";
                SqlCommand cmd = new SqlCommand(update, connexion);
                cmd.Parameters.AddWithValue("@newSolde", newSolde);
                cmd.Parameters.AddWithValue("@hypothequeId", hypothequeId);
                cmd.ExecuteNonQuery();
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

        private void btnAnnulerPrel_Click(object sender, RoutedEventArgs e)
        {
            // Vider le textbox.
            txtPrelevement.Clear();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Sortie de l'application.
            MessageBox.Show("Vous avez quitter le volet administrateur. \n\nAu revoir !");
            Application.Current.Shutdown();
        }
    }


}
