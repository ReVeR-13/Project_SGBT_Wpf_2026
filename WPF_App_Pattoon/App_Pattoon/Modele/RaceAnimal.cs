using Wpf_App_Pattoon_Animalerie.Service;
namespace Wpf_App_Pattoon_Animalerie.Modele
{

    public class RaceAnimal : ITable, IComparable<RaceAnimal>
    {
        private string _id;
        private DateTime _dateCreation;
        private TypeAnimal _type;
        private string _nom;
        private string? _infos;

        private RaceAnimal(TypeAnimal type, string nom, string? infos)
        {
            _id = Forma.SimpleId("RAC", AllRaceAnimal.Num + 1);
            _dateCreation = DateTime.Now;
            Type = type;
            Nom = nom;
            Infos = infos;
        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = Forma.Checked_DateCreation(value); }
        }
        public string Nom
        {
            get { return _nom; }
            set { _nom = Forma.Checked_Id(value); }
        }
        public string? Infos
        {
            get { return _infos; }
            set { _infos = value; }
        }
        public TypeAnimal Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override string ToString()
        {
            string retval =
                Forma.Center($"Fiche de la race n°[ {Id} ]\n", 102) +
                Forma.Center(new string('-', 90) + $"\n") +

                Forma.Texta2("Date Creation", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Type", $"{Type.Nom}") +
                Forma.Texta2("Race", $"{Nom}") +
                Forma.Texta2("Infos utile", $"{Infos}");
            return retval;
        }
        public int CompareTo(RaceAnimal? other)
        {
            return Id.CompareTo(other.Id);
        }

        public RaceAnimal Update(TypeAnimal? type, string? nom, string? infos)
        {
            Type = type ?? Type;
            Nom = nom ?? Nom;
            Infos = infos ?? Infos;
            return this;
        }

        public static RaceAnimal Creer(TypeAnimal type, string nom, string? infos)
        {
            Forma.ParametreNullTesteur(type);
            Forma.ParametreNullTesteur(nom);
            if (nom.Length < 3)
            {
                ExceptionLauncher.New("RaceAnimal", "Nom Invalide");
            }
            return new RaceAnimal(type, nom, infos);
        }
        public static int Save(RaceAnimal race)
        {
            Forma.ParametreNullTesteur(race);

            AllRaceAnimal.Add(race);

            return 1;
        }
        public static int Delete(RaceAnimal race)
        {
            AllRaceAnimal.Delete(race);
            return 1;
        }

        
    }
}
