using Wpf_App_Pattoon_Animalerie.Service;
namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class Compatibilite : ITable, IComparable<Compatibilite>
    {
        private string _id;
        private DateTime _date;
        private string _nom;
        private string _details;
        private Compatibilite(string nom, string details)
        {
            _id = Forma.SimpleId("CMP", AllCompatibilite.Num + 1);
            _date = DateTime.Now;
            Nom = nom;
            _details = details;
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if (value == null || value.Length != 15)
                {
                    ExceptionLauncher.New("Compatibilite ID", "L'id est invalide");
                }
                _id = Forma.TrimUpper(value);
            }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("Compatibilite Date Creation", "La date est invalide");
                }
                _date = value;
            }
        }
        public string Nom
        {
            get { return _nom; }
            set
            {
                if (value.Length < 2 && !Forma.IsCaractere(value))
                {
                    ExceptionLauncher.New("Compatibilité", "Le nom est invalide :" + value);
                }
                _nom = Forma.TrimUpper(value);
            }
        }
        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Id", Id) +
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Nom", Nom) +
                Forma.Texta2("Details", Details);

            return retVal;
        }
        public int CompareTo(Compatibilite comp)
        {
            return Nom.CompareTo(comp.Nom);
        }
        public IEnumerable<AnimalCompatibilité> GetAnimalCompatibilité()
        {
            foreach (AnimalCompatibilité comp in AnimalCompatibilitéService.FindAllByCompatibilite(this).Values)
            {
                yield return comp;
            }
        }

        public static Compatibilite Creer(string nom, string details)
        {
            Compatibilite retval = null;

            retval = new Compatibilite(nom, details);
            return retval;
        }
        public static int Save(Compatibilite compatibilite)
        {
            int retval = 0;
            if (AllCompatibilite.Find(compatibilite.Id) == null)
            {
                AllCompatibilite.Add(compatibilite);
                retval = AllCompatibilite.DB_Add(compatibilite);
            }
            return retval;
        }
        public int Update(string details, string nom)
        {
            int retval = 0;
            if (AllCompatibilite.FindByNom(Forma.TrimUpper(nom)) == null)
            {
                Nom = nom;
                Details = details;

                retval = AllCompatibilite.DB_Update(this);
            }
            return retval;
        }
        public static int Delete(Compatibilite compatibilite)
        {
            int retval = 0;
            if (AllCompatibilite.Find(compatibilite.Id) != null)
            {
                OnDelete(compatibilite);

                AllCompatibilite.Remove(compatibilite);
                retval = AllCompatibilite.DB_Delete(compatibilite);
            }
            return retval;
        }
        private static int OnDelete(Compatibilite compatibilite)
        {
            if (AnimalCompatibilitéService.FindAllByCompatibilite(compatibilite).Count > 0)
            {
                foreach (AnimalCompatibilité ac in compatibilite.GetAnimalCompatibilité())
                {
                    AnimalCompatibilité.Delete(ac);
                }
            }
            return 1;
        }

    }
}
