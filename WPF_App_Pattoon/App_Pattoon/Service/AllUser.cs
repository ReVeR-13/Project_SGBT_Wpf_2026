using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class UserService
    {
        private static readonly Dictionary<string, User> _lesUsers;
        private static int _num;
        static UserService()
        {
            _lesUsers = [];
            _num = 0;
        }

        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesUsers);
                }
                return _num;
            }
        }
        public static int Count
        {
            get
            {
                return _lesUsers.Count;
            }
        }
        public static string LesUsers
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Utilisateurs [{Count}]\n\n" +
                    Forma.Text("N°", "Id", "Date Crea.", "Username", "Password", "Email", "Admin", "Active", "Updated", "Dernier Conn.");
                foreach (User a in _lesUsers.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Password}",
                    $"{a.Email}",
                    a.Admin ? "Oui" : "Non",
                    a.Active ? "Oui" : "Non",
                    $"{a.Updated:dd-MM-yyyy}",
                    $"{a.LastConnection:dd-MM-yyyy}");
                }
                return retVal;
            }
        }

        public static IEnumerable<User> Get()
        {
            foreach (User cont in _lesUsers.Values)
            {
                yield return cont;
            }
        }
        public static User? Find(string id)
        {
            User? retVal = null;
            string f_id = Forma.TrimUpper(id);
            if (_lesUsers.TryGetValue(f_id, out User? value))
            {
                retVal = value;
            }
            return retVal;
        }
        public static User? FindByMail(string mail)
        {

            User? retVal = null;
            if (Forma.IsMail(mail))
            {
                foreach (User c in _lesUsers.Values)
                {
                    if (c.Email == mail)
                    {
                        retVal = c;
                        break;
                    }
                }
            }
            return retVal;
        }
        public static void Add(User user)
        {
            if (Find(user.Id) != null)
            {
                throw new Exception($"[ Groupe Users] Cet identifiant existe deja :{user.Id}");
            }
            _num++;
            _lesUsers.Add(user.Id, user);
        }
        public static void Remove(string idc)
        {
            if (Find(idc) == null)
            {
                ExceptionLauncher.New($"[ Groupe Users]", $" Cet Animal est deja supprimé :{idc}");
            }
            _lesUsers.Remove(idc);
        }

        public static int DB_Add(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) == null)
            {
                retVal = DB_User.Add(user);
            }
            return retVal;
        }
        public static User? DB_FindById(string id)
        {
            User? retVal = null;
            if (!string.IsNullOrEmpty(id))
            {
                retVal = DB_User.UnUserById(id);
            }
            return retVal;
        }
        public static User? DB_FindByEmail(string email)
        {
            User? retVal = null;
            if (Forma.IsMail(email))
            {
                retVal = DB_User.UnUserByEmail(email);
            }
            return retVal;
        }
        public static int DB_Update(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.Update(user);
            }
            return retVal;
        }
        public static int DB_UpdateAdmin(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.UpdateAdmin(user);
            }
            return retVal;
        }
        public static int DB_UpdateActive(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.UpdateActive(user);
            }
            return retVal;
        }
        public static int DB_Delete(User user)
        {
            int retVal = 0;
            //les donnees doivent etre supprimer en locale
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.Delete(user.Id);
            }
            return retVal;
        }

    }
}
