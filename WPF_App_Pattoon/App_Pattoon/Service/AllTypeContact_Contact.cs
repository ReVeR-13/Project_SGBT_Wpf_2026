using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllTypeContact_Contact
    {
        private static readonly Dictionary<string, TypeContact_Contact> _lesTypeContact_Contact;
        private static int _numType;
        static AllTypeContact_Contact()
        {
            _lesTypeContact_Contact = new Dictionary<string, TypeContact_Contact>();
            _numType = 0;
        }
        public static void Add(TypeContact_Contact type)
        {
            if (Find(type.Id) != null)
            {
                ExceptionLauncher.New("GetUnType Contact", "Cet Contact a deja cet role...");
            }
            _lesTypeContact_Contact.Add(type.Id, type);
            _numType++;
        }
        public static void Delete(string id)
        {
            if (!_lesTypeContact_Contact.ContainsKey(id.Trim().ToUpper()))
            {
                throw new Exception($"[MyType Contact - Contact] Ce contact n'a pas ce type de contact : {id}");
            }
            _lesTypeContact_Contact.Remove(id.Trim().ToUpper());
        }

        public static TypeContact_Contact Find(string id)
        {
            TypeContact_Contact type = null;
            if (_lesTypeContact_Contact.ContainsKey(Forma.TrimUpper(id)))
            {
                type = _lesTypeContact_Contact[Forma.TrimUpper(id)];
            }
            return type;
        }
        public static TypeContact_Contact Find(Contact contacts, TypeContact type)
        {
            TypeContact_Contact tpe = null;
            foreach (TypeContact_Contact t in AllOfContact(contacts).Values)
            {
                if (t.Type == type)
                {
                    tpe = t;
                    break;
                }
            }

            return tpe;
        }
        public static Dictionary<string, TypeContact_Contact> AllOfContact(Contact contact)
        {
            Dictionary<string, TypeContact_Contact> retval = new();
            foreach (TypeContact_Contact t in _lesTypeContact_Contact.Values.Where(a => a.Contact == contact))
            {
                    retval.Add(t.Type.Id, t); 
            }
            return retval;
        }
        public static Dictionary<string, TypeContact_Contact> AllOfType(TypeContact type)
        {
            Dictionary<string, TypeContact_Contact> retval = new Dictionary<string, TypeContact_Contact>();
            foreach (TypeContact_Contact t in _lesTypeContact_Contact.Values)
            {
                if (t.Type == type)
                {
                    retval.Add(t.Id, t);
                }
            }
            return retval;
        }
        public static TypeContact_Contact? DB_Find(TypeContact_Contact type)
        {
            return DB_TypeCnt_Contact.UnRoles(type);
        }

        public static string LesTypes
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Types Contacts [{Count}]\n\n" +
                    string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-13} {4,-15}\n"}",
                    "N°", "Id", "Date Crea.", "Contact", "Roles");
                foreach (TypeContact_Contact a in _lesTypeContact_Contact.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-13} {4,-15}\n"}",
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{a.Contact.Nom}",
                    $"{a.Type.Nom}");
                }
                return retVal;
            }
        }
        public static int Count
        {
            get
            {
                return _lesTypeContact_Contact.Count;
            }
        }
        public static int NumType
        {
            get
            {
                return _numType;
            }
        }

        public static int DB_Add(TypeContact_Contact type)
        {
            int ret = 0;

            if (DB_TypeCnt_Contact.UnRoles(type) == null)
            {
                ret = DB_TypeCnt_Contact.Add(type); ;
            }
            return ret;
        }
        public static int DB_Delete(TypeContact_Contact type)
        {
            int ret = 0;

            if (DB_TypeCnt_Contact.UnRoles(type) != null)
            {
                ret = DB_TypeCnt_Contact.Delete(type); ;
            }
            return ret;
        }
    }
}
