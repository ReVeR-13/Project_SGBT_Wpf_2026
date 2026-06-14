using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class User : ITable, IComparable<User>
    {
        private string _id;
        private DateTime _dateCreation;

        private string _nom;
        private string _password;
        private string _email;
        private string _description;

        private bool _admin;
        private bool _active;

        private DateTime _updated;
        private DateTime _lastConnection;

        private User(string nom, string password, string mail, string description)
        {
            Id = Forma.SimpleId("UTI", UserService.Num);
            DateCreation = DateTime.Now;

            Nom = nom;
            Password = password;
            Email = mail;
            Description = description;

            Admin = false;
            Active = false;

            Updated = DateTime.Now;
            LastConnection = DateTime.Now;

        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = Forma.Checked_DateCreation(value); }
        }
        public string Nom
        {
            get { return _nom; }
            set {
                if (value.Length < 4)
                {
                    ExceptionLauncher.New("User Nom", "Le nombre de caractere doit etre plus de 4");
                }
                _nom = value; 
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (value.Length < 8)
                {
                    ExceptionLauncher.New("User Password", "Le nombre de caractere doit etre au mininum de 8");
                }
                _password = value;
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (!Forma.IsMail(value))
                {
                    ExceptionLauncher.New("User Email", "Cet email n'est pas valide");
                }
                _email = value;
            }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public bool Admin
        {
            get { return _admin; }
            set { _admin = value; }
        }
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
        public DateTime Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }
        public DateTime LastConnection
        {
            get { return _lastConnection; }
            set { _lastConnection = value; }
        }

        public int CompareTo(User? other)
        {
            return Id.CompareTo(other.Id);
        }
        public override string ToString()
        {
            string retval = Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Username", $"{Nom}") +
                Forma.Texta2("Password", $"{Password}") +
                Forma.Texta2("Email", $"{Email}") +
                Forma.Texta2("isAdmin", Admin? "Oui":"Non") +
                Forma.Texta2("isActive", Active ? "Oui" : "Non") +
                Forma.Texta2("Updated", $"{Updated:dd-MM-yyyy}") +
                Forma.Texta2("Last Connection", $"{LastConnection:dd-MM-yyyy}") +
                Forma.Texta2("Description", $"{Description}"); ;
            return retval;
        }

        public static User Create(string username, string password,string mail,string description)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mail))
            {
                ExceptionLauncher.New("User Creer", "Parametres null");
            }

            return new User(username, password, mail ,description);
        }
        public User Update(string username, string password, string mail, string description)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mail))
            {
                ExceptionLauncher.New("User Updated", "Parametres null");
            }

            Nom = username;
            Password = password;
            Email = mail;
            Description = description;

            Updated = DateTime.Now;

            return this;
        }
        public User UpdateAdmin(bool admin)
        {
            Admin = admin;
            Updated = DateTime.Now;
            return this;
        }
        public User UpdateActive(bool active)
        {
            Active = active;
            Updated = DateTime.Now;
            return this;
        }
        public static User Save(User user)
        {
            UserService.Add(user);
            return user;
        }

    }
}
