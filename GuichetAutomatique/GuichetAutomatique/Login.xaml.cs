using System;
using System.Data.SqlClient;
using System.Windows;


namespace GuichetAutomatique
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        SqlConnection connexion;

        public Login()
        {
            InitializeComponent();
            connexion = new SqlConnection("server =.; initial catalog=GuichetAutomatique; integrated security=true");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Création des variable associés aux contrôles qui récolteront les informations de login.
            string IdUtilisateur = txtUtilisateur.Text;
            string NIP = txtNIP.Password;
            try
            {
                // Ouverture de la connexion.
                connexion.Open();
                // Création de l'objet SqlCommand.
                SqlCommand commande = new SqlCommand("SELECT TypeUtilisateur, Prenom, Nom FROM Utilisateurs WHERE " +
                                                     "IdUtilisateur = @IdUtilisateur AND NIP = @NIP", connexion);
                // Création de ses paramètres.
                commande.Parameters.AddWithValue("@IdUtilisateur", IdUtilisateur);
                commande.Parameters.AddWithValue("@NIP", NIP);

                // Création du lecteur de données.
                SqlDataReader lecteur = commande.ExecuteReader();

                if (lecteur.Read())
                {
                    // Établissement d'un index pour aller chercher les données nécessaires pour la connexion.
                    string role = lecteur.GetString(0);
                    string prenom = lecteur.GetString(1);
                    string nom = lecteur.GetString(2);

                    // Si l'utilisateur se connecte en tant qu'Admin.
                    if (role == "Admin")
                    {
                        Admin connAdmin = new Admin();
                        connAdmin.Show();
                        this.Close();
                    }
                    // Si l'utilisateur se connecte en tant que client.
                    else if (role == "Client")
                    {
                        UtilisateurActif utilisateurActif = new UtilisateurActif
                        {
                            IdUtilisateur = int.Parse(IdUtilisateur),
                            Prenom = prenom,
                            Nom = nom
                        };

                        // Redirection vers la page d'accueil Client.
                        Accueil AccueilClient = new Accueil(utilisateurActif);
                        AccueilClient.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe Incorrecte. Veuillez essayer de nouveau");
                }
            }
            // Si une exception est attrapée lors de la rédaction du programme.
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de la connexion.
                connexion.Close();
                this.Close();
            }
        }
    }
}
