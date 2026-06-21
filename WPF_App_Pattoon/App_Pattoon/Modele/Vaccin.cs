using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{

    public class Vaccin : ITable
    {
        private string _id;
        private DateTime _date;
        private string _nom;
        private string _description;
        private Vaccin(string nom,string description)
        {
            _id = Forma.SimpleId("VCC", 9999);
            _date = DateTime.Now;
            Nom = nom;
            Description = description;
        }

        public string Id
        {
            get { return _id; }
            set 
            {
                _id = Forma.Checked_Id(value); 
            }
            
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {
                _date = Forma.Checked_DateCreation(value);
            }
        }
        public string Nom
        {
            get { return _nom; }
            set 
            { 
                string F_nom = Forma.TrimUpper(value);
                if (F_nom.Length < 3)
                {
                    ExceptionLauncher.New("Non Vaccin", "Le Nom du vaccin n est pas valable");
                }
                _nom = F_nom; 
            }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public override string ToString()
        {
            string retVal =
                Forma.Texta2("Date",DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom", Nom) +
                Forma.Texta2("Description", Description) ;

            return retVal;
        }
        public IEnumerable<Vaccination> GetVaccination()
        {
            foreach (Vaccination vaccination in Vaccination.ByVaccin(this).Values)
            {
                yield return vaccination;
            }
        }

        public static Vaccin Creer(string nom, string description)
        {
            Vaccin retval = null;
            string f_nom = Forma.TrimUpper(nom);
            if (string.IsNullOrEmpty(nom))
            {
                ExceptionLauncher.New("Creer Vaccin", $"Parametre invalide");
            }
            retval = new Vaccin(nom, description);
            
            return retval;
        }
        public int Modifier(string nom, string description)
        {
            if (AllVaccin.FindByNom(nom)== null)
            {
                Nom = nom;
            }
                Description = description;
            return  AllVaccin.DB_Update(this);
            
        }
        public static int Delete(Vaccin vaccin)
        {
            int retval = 0;
            if (AllVaccin.Find(vaccin.Id) != null)
            {
                OnDelete(vaccin);

                AllVaccin.Remove(vaccin.Id);
                retval = AllVaccin.DB_Delete(vaccin);
            }
            return retval;
        }
        public static int Save(Vaccin vaccin)
        {
            int retval = 0;
            if (AllVaccin.Find(vaccin.Id) == null)
            {
                AllVaccin.Add(vaccin);
                retval = AllVaccin.DB_Add(vaccin);
            }
            return retval;
        }
        private static int OnDelete(Vaccin vaccin)
        {
            if (AllVaccination.FindAllBy(vaccin).Count > 0)
            {
                foreach (Vaccination ac in vaccin.GetVaccination())
                {
                    Vaccination.Delete(ac);
                }
            }
            return 1;
        }

    }
}
