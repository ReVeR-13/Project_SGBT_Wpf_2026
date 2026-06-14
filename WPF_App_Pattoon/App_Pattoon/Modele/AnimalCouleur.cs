using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{

    public class AnimalCouleur: ITable, IComparable<AnimalCouleur>
    {
        private string _id;
        private DateTime _date;
        private Animal _animal;
        private Couleur _couleur;
        private AnimalCouleur( Animal animal, Couleur couleur)
        {
            Id = animal.Id + couleur.Id;
            DateCreation = DateTime.Now;
            _animal = animal;
            _couleur = couleur;
        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = Forma.Checked_DateCreation(value); }
        }
        public Animal Animal
        {
            get { return _animal; }
            set { _animal = value; }
        }
        public Couleur Couleur
        {
            get { return _couleur; }
            set { _couleur = value; }
        }

        public int CompareTo(AnimalCouleur? other)
        {
            return Id.CompareTo(other.Id);
        }
        public override string ToString()
        {
            string retval = 
                Forma.Center($"Fiche de Couleur de [ {Animal.Id} ]\n") +
                Forma.Center(new string('-',90) + "\n") +
                Forma.Texta2("DATE", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("ID", Id) +
                Forma.Texta2("ID ANIMAL", Animal.Id) +
                Forma.Texta2("ID COULEUL", Couleur.Id); ;
            return retval;
        }

        public static AnimalCouleur Creer(Animal animal, Couleur couleur)
        {
            Forma.ParametreNullTesteur(animal);
            Forma.ParametreNullTesteur(couleur);

            return new AnimalCouleur(animal, couleur);
        }
        public int Delete()
        {
            AllAnimalCouleur.Delete(Id);
            return AllAnimalCouleur.DB_Delete(this);
        }
        public static int Save(AnimalCouleur coloration)
        {
            Forma.ParametreNullTesteur(coloration);

            AllAnimalCouleur.Add(coloration);
            return AllAnimalCouleur.DB_Add(coloration);
        }
    }
}
