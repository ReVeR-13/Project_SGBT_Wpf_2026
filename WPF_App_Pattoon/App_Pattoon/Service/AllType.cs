using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllTypeContact
    {
        private static Dictionary<string, TypeContact> _lesTypesContacts;
        private static int _numType;
        static AllTypeContact()
        {
            _lesTypesContacts = new Dictionary<string, TypeContact>();
            _numType = 0;
        }

        public static int Count
        {
            get
            {
                return _lesTypesContacts.Count;
            }
        }
        public static int NumType
        {
            get
            {
                if (Count > 0)
                {
                    _numType = Forma.LastNumero(_lesTypesContacts);
                }
                return _numType;
            }
        }
        public static string LesTypesContacts
        {
            get
            {
                int i = 0;
                string retVal =
                    Forma.Text("N°", "Id", "Date Crea.", "Type", "Desciption");
                foreach (TypeContact a in _lesTypesContacts.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Description}");
                }
                return Forma.Center($"Liste des types Contacts [{i}/{Count}]\n\n") + retVal;
            }
        }
        public static Dictionary<string, TypeContact> LesStocks 
        {
            get { DB_Sync(); return _lesTypesContacts; }
        }



        public static void Add(TypeContact type)
        {
            if (FindByNom(type.Nom) != null)
            {
                ExceptionLauncher.New("Type ContactSelectionne", "Cet type existe deja...");
            }
            _lesTypesContacts.Add(type.Id, type);
            _numType++;
        }
        public static void Delete(TypeContact type)
        {
            if (FindById(type.Id) == null)
            {
                throw new Exception($"[Groupe Type ContactSelectionne] Ce type de contact n' existe plus : {type.Nom}");
            }
            _lesTypesContacts.Remove(type.Id);
        }
        public static TypeContact? FindByNom(string nom)
        {
            return _lesTypesContacts.Values.FirstOrDefault(a => a.Nom == Forma.TrimUpper(nom));
        }
        public static TypeContact? FindById(string id)
        {
            TypeContact? type = _lesTypesContacts.Values.Where(a => a.Id == Forma.TrimUpper(id)).FirstOrDefault();
            return type ?? null;
        }
        public static string LesTypesContactsManquant(Contact contact)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Desciption");
            foreach (TypeContact a in _lesTypesContacts.Values)
            {
                bool verif = false;
                foreach (TypeContact_Contact b in contact.EnumType())
                {
                    if (a == b.Type)
                    {
                        verif = true;
                    }
                }

                if (!verif)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Description}");
                }

            }
            return $"Liste des Types Contacts [{i}]\n\n" + retVal;
        }

        public static Dictionary<string, TypeContact> LesRolesManquant(Contact contact)
        {
            int i = 0;
            Dictionary<string, TypeContact> retVal = [];
            foreach (TypeContact a in _lesTypesContacts.Values)
            {
                bool verif = contact.GetRolesListe().ContainsKey(a.Id);
                
                if (!verif)
                {
                    retVal.Add(a.Id,a);
                }

            }
            return retVal;
        }


        public static int DB_Add(TypeContact type)
        {
            int retval = 0;
            if (DB_TypeContact.UnTypesContactById(type.Id) == null)
            {
                retval = DB_TypeContact.Add(type);

            }
            return retval;
        }
        public static int DB_Update(TypeContact type)
        {
            int retval = 0;
            if (DB_TypeContact.UnTypesContactById(type.Id) != null)
            {
                retval = DB_TypeContact.Update(type);

            }
            return retval;
        }
        public static int DB_Delete(TypeContact type)
        {
            int retval = 0;
            if (DB_TypeContact.UnTypesContactById(type.Id) != null)
            {
                DB_TypeContact.Delete(type.Id);
                retval = DB_TypeContact.UnTypesContactById(type.Id) == null ? 1 : 0;
            }
            return retval;
        }
        public static void DB_Sync()
        {
            _lesTypesContacts = new(DB_TypeContact.All_From_Db());
        }


    }
    public static class AllTypeAnimal
    {
        private static Dictionary<string, TypeAnimal> lesTypes;
        private static int _num;

        static AllTypeAnimal()
        {
            lesTypes = new Dictionary<string, TypeAnimal>();
            _num = 0;
        }
        public static string LesTypes
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Types Animals [{Count}]\n\n" +
                    string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-20}\n"}",
                    "N°", "Id", "Date Crea.", "Type", "Desciption");
                foreach (TypeAnimal a in lesTypes.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-20}\n"}",
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{a.Nom}",
                    $"{a.Description}");
                }
                return retVal;
            }
        }
        public static int Count
        {
            get
            {
                return lesTypes.Count;
            }
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(lesTypes);
                }
                return _num;
            }
        }
        public static Dictionary<string, TypeAnimal> LesStocks
        {
            get { DB_Sync(); return lesTypes; }
        }

        public static void Add(TypeAnimal type)
        {
            if (FindTypeByNom(type.Nom) != null)
            {
                ExceptionLauncher.New("Groupe Type Animal", $"Cet type d'animal existe deja : {type.Nom}");
            }
            _num++;
            lesTypes.Add(type.Id, type);
        }
        public static void Delete(string id)
        {
            if (!lesTypes.ContainsKey(id))
            {
                throw new Exception($"[Groupe Type Animal] Ce Animal n' existe plus : {id}");
            }
            lesTypes.Remove(id);
        }
        public static TypeAnimal? FindTypeByNom(string nom)
        {
            TypeAnimal? type = null;
            foreach (TypeAnimal item in lesTypes.Values)
            {
                if (item.Nom == Forma.TrimUpper(nom))
                {
                    type = item;
                    break;
                }
            }

            return type;
        }
        public static TypeAnimal? FindTypebyId(string id)
        {
            TypeAnimal? type = null;
            if (lesTypes.ContainsKey(Forma.TrimUpper(id)))
            {
                type = lesTypes[Forma.TrimUpper(id)];
            }
            return type;
        }

        public static int DB_Add(TypeAnimal type)
        {
            int retval = 0;
            if (DB_TypeAnimal.UnTypesAnimalByNom(type.Nom) == null)
            {
                retval = DB_TypeAnimal.Add(type);
            }

            return retval;
        }
        public static int DB_Update(TypeAnimal type)
        {
            int retval = 0;
            if (DB_TypeAnimal.UnTypesAnimalById(type.Id) != null)
            {
                retval = DB_TypeAnimal.Update(type);
            }

            return retval;
        }
        public static int DB_Delete(TypeAnimal type)
        {
            int retval = 0;
            if (DB_TypeAnimal.UnTypesAnimalById(type.Id) != null)
            {
                retval = DB_TypeAnimal.Delete(type);
            }

            return retval;
        }

        public static void DB_Sync()
        {
            lesTypes = new(DB_TypeAnimal.AllTypesAnimal());
        }

    }
}
