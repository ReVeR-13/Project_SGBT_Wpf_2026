using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllAdoption
    {
        private static readonly Dictionary<string, Adoption> _lesAdoptions;

        static AllAdoption()
        {
            _lesAdoptions = new Dictionary<string, Adoption>();
        }

        public static int Count
        {
            get { return _lesAdoptions.Count; }
        }
        public static string Listes
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Adoptions [{Count}]\n" +
                    string.Format($"{"{0,-4} {1,-19} {2,-11} {3,-11} {4,-16} {5,-20} {6,-10} {7,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Statut", "Id Demande", "Contact", "Date Fin", "Raison Refus");
                foreach (Adoption vc in _lesAdoptions.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-19} {2,-11} {3,-11} {4,-16} {5,-20} {6,-11} {7,-11}\n"}",
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{vc.Statut}",
                             $"{vc.Demande.Id}",
                             $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                             vc.DateF == null ? "--" : vc.DateF?.ToString("dd-MM-yyyy"),
                             vc.DateF == null ? "--" : vc.RaisonRefus);
                }

                return retVal;
            }
        }

        public static IEnumerable<Adoption> Get()
        {
            foreach (Adoption dem in _lesAdoptions.Values)
            {
                yield return dem;
            }
        }
        public static IEnumerable<Adoption> Get(Contact contact)
        {
            foreach (Adoption dem in _lesAdoptions.Values)
            {
                if (dem.Demande.Contact == contact)
                {
                    yield return dem;
                }

            }
        }
        public static IEnumerable<Adoption> Get(Animal animal)
        {
            foreach (Adoption dem in Get().Where(a => a.Demande.Animal == animal))
            {
                yield return dem;
            }
        }
        public static IEnumerable<Adoption> Get(EStatutValidation validation)
        {
            foreach (Adoption dem in Get().Where(a => a.Statut == validation))
            {
                yield return dem;
            }
        }
        public static IEnumerable<Adoption> Get(Contact contact, EStatutValidation eStatut)
        {
            foreach (Adoption dema in Get().Where(a => a.Demande.Contact == contact && a.Statut == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Adoption> Get(Animal animal, EStatutValidation eStatut)
        {
            foreach (Adoption dema in Get().Where(a => a.Demande.Animal == animal && a.Statut == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }

        public static Dictionary<string, Adoption> GetAllAdoption(EStatutValidation eStatut)
        {
            Dictionary<string, Adoption> retVal = [];

            foreach (Adoption dm in Get(eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Adoption> GetAllAdoption(Animal animal, EStatutValidation eStatut)
        {
            Dictionary<string, Adoption> retVal = [];

            foreach (Adoption dm in Get(animal, eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }

        public static Adoption Find(string id)
        {
            Adoption ado = null;
            string fid = Forma.TrimUpper(id);
            if (_lesAdoptions.ContainsKey(fid))
            {
                ado = _lesAdoptions[fid];
            }
            return ado;
        }
        public static Adoption Find(Demande demande)
        {
            Adoption ado = null;
            foreach (Adoption a in _lesAdoptions.Values)
            {
                if (a.Demande == demande)
                {
                    ado = a;
                    break;
                }
            }

            return ado;
        }
        public static void Add(Adoption ado)
        {
            if (Find(ado.Id) != null)
            {
                ExceptionLauncher.New("Liste Adoptions", "Cette adoption existe deja");
            }
            _lesAdoptions.Add(ado.Id, ado);
        }
        public static void Remove(string id)
        {
            string fid = Forma.TrimUpper(id);
            if (Find(fid) == null)
            {
                ExceptionLauncher.New("Liste Adoptions", "Cette adoption est deja supprimer");
            }
            _lesAdoptions.Remove(fid);
        }

        public static int DB_Add(Adoption ado)
        {
            int ret = 0;
            if (DB_Adoption.UnAdoptionById(ado) == null)
            {
                ret = DB_Adoption.Add(ado);
            }
            return ret;
        }
        public static int DB_Update(Adoption ado)
        {
            int ret = 0;
            if (DB_Adoption.UnAdoptionById(ado) != null)
            {
                ret = DB_Adoption.Update(ado);
            }
            return ret;
        }
        public static int DB_Delete(Adoption ado)
        {
            int ret = 0;
            if (DB_Adoption.UnAdoptionById(ado) != null)
            {
                ret = DB_Adoption.Delete(ado);
            }
            return ret;
        }

    }
}
