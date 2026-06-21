using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllAccueil
    {
        private static Dictionary<string, Accueil> _lesAccueils;

        static AllAccueil()
        {
            _lesAccueils = new Dictionary<string, Accueil>();
        }

        public static int Count
        {
            get { return _lesAccueils.Count; }
        }
        public static string Liste
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Decision", "ContactSelectionne", "Date Annul.", "Raison");

                foreach (Accueil dm in _lesAccueils.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Decision}",
                    $"{dm.Demande.Contact.Nom} {dm.Demande.Contact.Prenom}",
                    dm.DateFin == null ? "--" : dm.DateFin?.ToString("dd-MM-yyyy"),
                    dm.DateFin == null ? "--" : dm.RaisonAnullation);
                }

                return $"Liste des Accueils[{Count}]\n\n" + retVal;
            }
        }

        public static string ListeAccueil(EStatutValidation eStatut)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Decision", "ContactSelectionne", "Date Annul.", "Raison");

            foreach (Accueil dm in _lesAccueils.Values)
            {
                if (dm.Decision == eStatut)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Decision}",
                    $"{dm.Demande.Contact.Nom} {dm.Demande.Contact.Prenom}",
                    dm.DateFin == null ? "--" : dm.DateFin?.ToString("dd-MM-yyyy"),
                    dm.DateFin == null ? "--" : dm.RaisonAnullation);
                }

            }

            return $"Liste des Accueils[{i}]\n\n" + retVal;

        }
        public static string ListeAccueil()
        {
            return Liste;
        }

        public static IEnumerable<Accueil> Get()
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                yield return dema;
            }
        }
        public static IEnumerable<Accueil> Get(Contact contact)
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                if (dema.Demande.Contact == contact)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Animal animal)
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                if (dema.Demande.Animal == animal)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Contact contact, Animal animal)
        {
            foreach (Accueil dema in Get(contact))
            {
                if (dema.Demande.Animal == animal)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(EStatutValidation eStatut)
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                if (dema.Decision == eStatut)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Contact contact, EStatutValidation eStatut)
        {
            foreach (Accueil dema in _lesAccueils.Values.Where(a => a.Demande.Contact == contact && a.Decision == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Decision == eStatut)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Animal animal, EStatutValidation eStatut)
        {
            foreach (Accueil dema in _lesAccueils.Values.Where(a => a.Demande.Animal == animal && a.Decision == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Decision == eStatut)
                {
                    yield return dema;
                }

            }
        }

        public static Dictionary<string, Accueil> GetAllAccueil(Contact contacts)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(contacts))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(Animal animal)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(animal))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(Animal animal, Contact contact)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(contact, animal))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(EStatutValidation eStatut)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(Animal animal, EStatutValidation eStatut)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(animal, eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Accueil Find(string id)
        {
            Accueil dem = null;
            string fid = Forma.TrimUpper(id);
            if (_lesAccueils.TryGetValue(fid, out Accueil? value))
            {
                dem = value;
            }
            return dem;
        }
        public static Accueil Find(Demande demande)
        {
            Accueil dem = null;
            foreach (Accueil a in _lesAccueils.Values)
            {
                if (a.Demande == demande)
                {
                    dem = a;
                    break;
                }
            }

            return dem;
        }

        public static void Add(Accueil dem)
        {
            if (Find(dem.Id) != null)
            {
                ExceptionLauncher.New("Liste Accueil", "Cette accueil existe deja");
            }
            _lesAccueils.Add(dem.Id, dem);
        }
        public static void Remove(string id)
        {
            string fid = Forma.TrimUpper(id);
            if (Find(fid) == null)
            {
                ExceptionLauncher.New("Liste accueil", "Cette accueil est deja supprimer");
            }
            _lesAccueils.Remove(fid);
        }

        public static int DB_Add(Accueil accueil)
        {
            int ret = 0;
            if (DB_Accueil.UnAccueilById(accueil) == null)
            {
                ret = DB_Accueil.Add(accueil);
            }
            return ret;
        }
        public static int DB_Update(Accueil accueil)
        {
            int ret = 0;
            if (DB_Accueil.UnAccueilById(accueil) != null)
            {
                ret = DB_Accueil.Update(accueil);
            }
            return ret;
        }
        public static int DB_Delete(Accueil accueil)
        {
            int ret = 0;
            if (DB_Accueil.UnAccueilById(accueil) != null)
            {
                ret = DB_Accueil.Delete(accueil);
            }
            return ret;
        }
        public static void DB_Sync()
        {
            _lesAccueils = new Dictionary<string, Accueil> (DB_Accueil.All_From_Db());
        }


    }

}
