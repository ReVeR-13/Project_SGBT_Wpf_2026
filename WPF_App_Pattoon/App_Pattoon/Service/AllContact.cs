
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllContacts
    {
        private static  Dictionary<string, Contact> _lesContacts;
        private static int _numContact;
        static AllContacts()
        {
            _lesContacts = new Dictionary<string, Contact>();
            _numContact = 0;
        }

        public static int NumContact
        {
            get
            {
                if (Count > 0)
                {
                    _numContact = Forma.LastNumero(_lesContacts);
                }
                return _numContact;
            }
        }
        public static int Count
        {
            get
            {
                return _lesContacts.Count;
            }
        }
        public static string LesContacts
        {
            get
            {
                int i = 0;
                string retVal =
                    Forma.Text("N°", "Id", "Date Crea.", "Nom", "Prenom", "Dem.");
                foreach (Contact a in _lesContacts.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Prenom}",
                    $"{AllDemande.GetAllDemandeByContact(a).Count}");
                }
                return Forma.Center($"Liste des Contacts [{i}/{Count}]\n\n") + retVal;
            }
        }
        public static Dictionary<string, Contact> LesStocks
        {
            get { DB_Sync(); return _lesContacts; }
        }

        public static IEnumerable<Contact> Get()
        {
            foreach (Contact cont in _lesContacts.Values)
            {
                yield return cont;
            }
        }
        public static Contact? Find(string idcontact)
        {
            Contact? retVal = null;
            string f_id = Forma.TrimUpper(idcontact);
            if (_lesContacts.TryGetValue(f_id, out Contact? value))
            {
                retVal = value;
            }
            return retVal;
        }
        public static Contact? FindByGsm(string gsm)
        {

            Contact? retVal = null;
            if (Forma.IsNum(gsm))
            {
                string f_gsm = Forma.TrimUpper(gsm);
                foreach (Contact c in _lesContacts.Values)
                {
                    if (c.Gsm == f_gsm)
                    {
                        retVal = c;
                    }
                }
            }
            return retVal;
        }
        public static void Add(Contact contact)
        {
            if (Find(contact.Id) != null)
            {
                throw new Exception($"[ Groupe Animaux] Cet identifiant existe deja :{contact.Id}");
            }
            _numContact++;
            _lesContacts.Add(contact.Id, contact);
        }
        public static void Remove(Contact contact)
        {
            if (Find(contact.Id) == null)
            {
                throw new Exception($"[ Groupe Animaux] Cet ContactSelectionne est deja supprimé :{contact.Id}");
            }
            _lesContacts.Remove(contact.Id);
        }

        public static int DB_Add(Contact contact)
        {
            int retVal = 0;
            if (DB_Contact.UnContactById(contact.Id) == null)
            {
                retVal = DB_Contact.Add(contact);
            }
            return retVal;
        }
        public static int DB_Update(Contact contact)
        {
            int retVal = 0;
            //les donnees locaux doivent etre à jour d'abord
            if (DB_Contact.UnContactById(contact.Id) != null)
            {
                retVal = DB_Contact.Update(contact);
            }
            return retVal;
        }
        public static int DB_Delete(Contact contact)
        {
            int retVal = 0;
            //les donnees doivent etre supprimer en locale
            if (DB_Contact.UnContactById(contact.Id) != null)
            {
                retVal = DB_Contact.Delete(contact.Id);
            }
            return retVal;
        }

        public static void DB_Sync()
        {
            _lesContacts = new Dictionary<string, Contact>(DB_Contact.All_From_Db());
        }

    }
}
