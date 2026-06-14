using Wpf_App_Pattoon_Animalerie.Service;
namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class AnimalCompatibilité
    {
        private string _id;
        private DateTime _date;
        private Animal _animal;
        private Compatibilite _compatibilite;
        private bool _compatible;
        private string? _remaque;

        private AnimalCompatibilité(Animal animal, Compatibilite compat, bool compatible, string? remarque)
        {
            _id = animal.Id + compat.Id;
            _date = DateTime.Now;
            Animal = animal;
            Compatibilite = compat;
            Compatible = compatible;
            Remaque = remarque;
        }

        public string Id
        {
            get
            { return _id; }
            set
            {
                if (string.IsNullOrEmpty(value) && value.Length != 30)
                {
                    ExceptionLauncher.New("AnimalCompatibilite Id", "L'id est invalide");
                }
                _id = value;
            }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("AnimalCompatibilite Date Creation", "La date de creation est invalide");
                }
                _date = value;
            }
        }
        public Animal Animal
        {
            get { return _animal; }
            set { _animal = value; }
        }
        public Compatibilite Compatibilite
        {
            get { return _compatibilite; }
            set { _compatibilite = value; }
        }
        public bool Compatible
        {
            get
            {
                return _compatible;
            }
            set { _compatible = value; }
        }
        public string? Remaque
        {
            get { return _remaque; }
            set { _remaque = value; }
        }

        public int CompareTo(AnimalCompatibilité? other)
        {
            return Id.CompareTo(other?._id);
        }
        public string ToString()
        {
            string retVal =
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom Animal", Animal.Nom) +
                Forma.Texta2("Nom Compat.", Compatibilite.Nom) +
                Forma.Texta2("Compatible", Compatible.ToString()) +
                Forma.Texta2("Remarque", Remaque == null ? "--" : Remaque);

            return retVal;
        }
        public static AnimalCompatibilité Creer(Animal animal, Compatibilite comp, bool compatible, string? remarque)
        {
            if (animal == null || comp == null)
            {
                ExceptionLauncher.New("Animal compatibilité", "Parametre invalide");
            }
            AnimalCompatibilité animalcomp = new(animal, comp, compatible, remarque);
            return animalcomp;
        }
        public static int Save(AnimalCompatibilité compatibilité)
        {
            if (compatibilité.Animal.Statut != EStatutAnimal.REFUGE)
            {
                ExceptionLauncher.New("AnimalCompatibilité Save", "L'animal n 'est pas au refuge");
            }

            int retVal = 0;
            if (AnimalCompatibilitéService.Find(compatibilité.Id) == null)
            {
                AnimalCompatibilitéService.Add(compatibilité);
                retVal = AnimalCompatibilitéService.DB_Add(compatibilité);
            }
            return retVal;
        }
        public int Update(bool compatible, string? remarque)
        {
            int ret = 0;
            if (AnimalCompatibilitéService.Find(Id) != null)
            {
                Compatible = compatible;
                Remaque = remarque;
                ret = AnimalCompatibilitéService.DB_Update(this);
            }
            return ret;
        }
        public static int Delete(AnimalCompatibilité compatibilité)
        {
            int ret = 0;
            if (AnimalCompatibilitéService.Find(compatibilité.Id) != null)
            {
                AnimalCompatibilitéService.Remove(compatibilité.Id);
                ret = AnimalCompatibilitéService.DB_Delete(compatibilité);
            }
            return ret;
        }
    }
}
