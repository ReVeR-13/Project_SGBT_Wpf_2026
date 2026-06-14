using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class Vaccination : ITable, IComparable<Vaccination>
    {
        private string _id;
        private DateTime _date;
        private Animal _animal;
        private Vaccin _vaccin;
        private string? _remaque;
        private Vaccination(Animal animal, Vaccin vaccin, string? remarque)
        {
            _id = animal.Id + vaccin.Id;
            _date = DateTime.Now;
            Animal = animal;
            Vaccin = vaccin;
            Remaque = remarque;

        }
        public string Id
        {
            get { return _id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ExceptionLauncher.New("Vaccination id", "L Id est incorrecte");
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
                    ExceptionLauncher.New("Vaccination Dade Creation", "La Date est incorrecte");
                }
                _date = value;
            }
        }
        public Animal Animal
        {
            get { return _animal; }
            set { _animal = value; }
        }
        public Vaccin Vaccin
        {
            get { return _vaccin; }
            set { _vaccin = value; }
        }
        public string? Remaque
        {
            get { return _remaque; }
            set { _remaque = value; }
        }

        public int CompareTo(Vaccination? other)
        {
            return Id.CompareTo(other?._id);
        }
        public string ToString()
        {
            string retVal =
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom Vaccin", Vaccin.Nom) +
                Forma.Texta2("Nom Animal", Animal.Nom) +
                Forma.Texta2("Remarque", Remaque == null ? "--" : Remaque);

            return retVal;
        }

        public static Dictionary<string, Vaccination> ByAnimal(Animal animal)
        {
            return AllVaccination.FindAllBy(animal);
        }
        public static Dictionary<string, Vaccination> ByVaccin(Vaccin vaccin)
        {
            return AllVaccination.FindAllBy(vaccin);
        }

        public static Vaccination Creer(Animal animal, Vaccin vaccin, string? remarque)
        {
            Vaccination animalVaccin = new(animal, vaccin, remarque);
            return animalVaccin;
        }
        public static int Save(Vaccination vaccination)
        {
            int ret = 0;
            if (AllVaccination.Find(vaccination.Id) == null)
            {
                AllVaccination.Add(vaccination);
                ret = AllVaccination.DB_Add(vaccination);
            }
            return ret;
        }
        public int Update(Animal animal, Vaccin vaccin, string? remarque)
        {
            int ret = 0;
            if (animal != null && vaccin != null)
            {
                Animal = animal;
                Vaccin = vaccin;
                Remaque = remarque;

                AllVaccination.DB_Update(this);
            }
            return ret;
        }
        public static int Delete(Vaccination vaccination)
        {
            int ret = 0;
            if (AllVaccination.Find(vaccination.Id) != null)
            {
                AllVaccination.Remove(vaccination.Id);
                ret = AllVaccination.DB_Delete(vaccination);
            }
            return ret;
        }
    }
}
