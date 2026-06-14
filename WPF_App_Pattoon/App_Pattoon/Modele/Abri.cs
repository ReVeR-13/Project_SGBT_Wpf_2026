

using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class Abri:ITable
    {
        private string _id;
        private DateTime _date;
        private string _libelle;
        private EStatutAbri _statut;
        private string? _description;
        private Abri(string libelle, string description)
        {
            Id = Forma.SimpleId("ABR", AllAbri.Num +1);
            DateCreation = DateTime.Now;
            Libelle = libelle;
            Statut = EStatutAbri.DISPONIBLE;
            Description = description;
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = Forma.TrimUpper(value);
            }
        }
        public DateTime DateCreation
        {
            get
            {
                return _date;
            }
            set
            {
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("Abri", "La date est invalide");
                }
                _date = value;
            }
        }
        public string Libelle
        {
            get
            {
                return _libelle;
            }
            set
            { 
                if (value.Length < 2)
                {
                    ExceptionLauncher.New("Abri", "Le libellé est invalide");
                }
                _libelle = Forma.TrimUpper( value); 
            }
        }
        public EStatutAbri Statut
        {
            get
            {
                return _statut;
            }
            set
            {
                _statut = value;
            }
        }
        public string? Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public static Abri Creer(string libelle, string description)
        {
            Abri abri = null;
            if (!string.IsNullOrEmpty(libelle))
            {
                abri = new Abri(libelle, description);
            }
            return abri;
        }
        public static int Save(Abri abri)
        {
            int result = 0;
            if (AllAbri.Find(abri.Id) == null)
            {
                AllAbri.Add(abri);

                result = AllAbri.DB_Add(abri);
            }
            return result;
        }
        public int Update(string libelle, EStatutAbri? statutAbri, string description)
        {
            int modifier = 0;
            if (!string.IsNullOrEmpty(libelle) || statutAbri != null)
            {
                Libelle = libelle;
                Statut = (EStatutAbri)statutAbri;
                Description = description;

                AllAbri.DB_Update(this);

                modifier = 1;
            }
            return modifier;
        }
        public int Update(EStatutAbri? statutAbri)
        {
            int modifier = 0;
            if (statutAbri != null)
            {
                Statut = (EStatutAbri)statutAbri;
                AllAbri.DB_Update(this);

                modifier = 1;
            }
            return modifier;
        }
        public static int Delete(Abri abri)
        {
            int result = 0;
            if (true)
            {
                AllAbri.Remove(abri.Id);
                AllAbri.DB_Delete(abri);
                result = 1;
            }
            return result;
        }
        public override string ToString()
        {
            string retVal =
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom", Libelle) +
                Forma.Texta2("Statut", Statut.ToString());
                Forma.Texta2("Description", Description);

            return retVal;
        }

    }
}
