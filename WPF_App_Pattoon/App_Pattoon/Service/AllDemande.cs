using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllDemande
    {
        private static Dictionary<string, Demande> _lesDemandes;
        private static int _num;

        static AllDemande()
        {
            _num = 0;
            _lesDemandes = new Dictionary<string, Demande>();
        }

        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesDemandes);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return _lesDemandes.Count; }
        }
        public static Dictionary<string, Demande> ListeAllDemande
        {
            get { return _lesDemandes;}
        }
        public static string Listes
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Decision");

                foreach (Demande a in _lesDemandes.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Type}",
                    $"{a.Animal.Type.Nom}",
                    $"{a.Statut}");
                }
                return Forma.Center($"Liste de toutes les Demandes [{i}/{Count}]\n\n") + retVal;
            }
        }
        public static string ListesDernieres
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Decision");

                foreach (Demande a in _lesDemandes.Values.OrderByDescending(od => od.DateCreation).Take(7))
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Type}",
                    $"{a.Animal.Type.Nom}",
                    $"{a.Statut}");
                }
                return Forma.Center($"Liste des derniéres Demandes [{i}/{Count}]\n\n") + retVal;
            }
        }

        public static string ListesByAnimal(Animal animal)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Decision");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Animal.Id == animal.Id))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste de toutes les Demandes de Animal: {animal.Id} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListesByContact(Contact contact)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Decision");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Contact.Id == contact.Id))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes de ContactSelectionne: {contact.Id} [{i}/{Count}]\n\n") + retVal;

        }

        public static string ListeByStatut()
        {
            return Listes;
        }
        public static string ListeByStatut(EStatutDemande eStatut)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Decision");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes de Decision: {eStatut} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListeByStatut(EStatutDemande eStatut, EStatutDemande yStatut)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Decision");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut || a.Statut == yStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes: {eStatut} | {yStatut} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListeByTypeString(ETypeDemande eType)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Decision");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Type == eType))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes: {eType} [{i}/{Count}]\n\n") + retVal;

        }
        public static Dictionary<string, Demande> ListeByType(ETypeDemande eType)
        {

            Dictionary<string, Demande> retVal = new Dictionary<string, Demande>();

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Type == eType))
            {
                retVal.Add(a.Id, a);
            }
            return retVal;

        }
        public static string ListeByStatutString(EStatutDemande eStatut, ETypeDemande eType)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Decision");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut && a.Type == eType))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes: {eStatut} & {eType} [{i}/{Count}]\n\n") + retVal;

        }
        public static Dictionary<string, Demande> ListeByStatut(EStatutDemande eStatut, ETypeDemande eType)
        {

            int i = 0;
            Dictionary<string, Demande> retVal = new Dictionary<string, Demande>();

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut && a.Type == eType))
            {
                retVal.Add(a.Id, a);
            }
            return retVal;

        }

        public static IEnumerable<Demande> Get()
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                yield return dem;
            }
        }
        public static IEnumerable<Demande> Get(Contact contact)
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                if (dem.Contact.Id == contact.Id)
                {
                    yield return dem;
                }

            }
        }
        public static IEnumerable<Demande> Get(Animal animal)
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                if (dem.Animal.Id == animal.Id)
                {
                    yield return dem;
                }

            }
        }
        public static IEnumerable<Demande> Get(EStatutDemande eStatut)
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                if (dem.Statut == eStatut)
                {
                    yield return dem;
                }

            }
        }

        public static Dictionary<string, Demande> GetAllDemandeByContact(Contact contact)
        {
            Dictionary<string, Demande> retval = [];

            foreach (Demande av in Get(contact))
            {
                retval.Add(av.Id, av);
            }

            return retval;
        }
        public static Dictionary<string, Demande> GetAllDemandeByAnimal(Animal animal)
        {
            Dictionary<string, Demande> retval = [];

            foreach (Demande av in Get(animal))
            {
                retval.Add(av.Id, av);
            }

            return retval;
        }

        public static Demande? Find(string id)
        {
            Demande? ado = null;
            string fid = Forma.TrimUpper(id);
            if (_lesDemandes.TryGetValue(fid, out Demande? value))
            {
                ado = value;
            }
            return ado;
        }
        public static Demande? Find(Contact contact, Animal animal, EStatutDemande eStatut, ETypeDemande eType)
        {
            Demande? ado = null;
            if (Get().Where(a => a.Statut < eStatut && a.Contact.Id == contact.Id && a.Animal.Id == animal.Id && a.Type == eType).Any())
            {
                foreach (Demande d in Get().Where(a => a.Statut < eStatut &&
                                                  a.Contact == contact &&
                                                  a.Animal == animal &&
                                                  a.Type == eType))
                {
                    ado = d;
                }
            }
            return ado;
        }

        public static void Add(Demande demande)
        {
            if (Find(demande.Id) != null)
            {
                ExceptionLauncher.New("Liste Demandes", "Cette demande existe deja");
            }
            _num++;
            _lesDemandes.Add(demande.Id, demande);
        }
        public static void Remove(Demande demande)
        {

            if (Find(demande.Id) == null)
            {
                ExceptionLauncher.New("Liste Demandes", "Cette demande est deja supprimer");
            }
            _lesDemandes.Remove(demande.Id);
        }

        public static int DB_Add(Demande demande)
        {
            int retval = 0;
            if (DB_Demande.UnDemandeById(demande) == null)
            {
                retval = DB_Demande.Add(demande);
            }
            return retval;
        }
        public static int DB_Update(Demande demande)
        {
            int retval = 0;
            if (DB_Demande.UnDemandeById(demande) != null)
            {
                retval = DB_Demande.Update(demande);
            }
            return retval;
        }
        public static int DB_Delete(Demande demande)
        {
            int retval = 0;
            if (DB_Demande.UnDemandeById(demande) != null)
            {
                retval = DB_Demande.Delete(demande);
            }
            return retval;
        }

        public static void DB_Sync()
        {
            _lesDemandes = new Dictionary<string, Demande>(DB_Demande.All_From_Db());
        }
    }
}
