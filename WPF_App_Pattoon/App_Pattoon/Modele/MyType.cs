using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{


    public abstract class MyType : ITable, IComparable<string>
    {
        private DateTime date;
        private string nom;
        private string? description;
        protected MyType(string nom, string description)
        {
            date = DateTime.Now;
            Nom = nom;
            Description = description;
        }
        public abstract string Id
        {
            get;
            set;
        }
        public virtual DateTime DateCreation
        {
            get { return date; }
            set { date = value; }
        }
        public virtual string Nom
        {
            get { return nom; }
            set
            {
                if (value.Trim().Length < 3 || string.IsNullOrEmpty(value))
                {
                    throw new Exception($"[Type Animal] Le nom n'est pas valide:{value} ");
                }
                nom = Forma.TrimUpper(value);
            }
        }
        public virtual string? Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }
        public int CompareTo(string? nom)
        {
            return Nom.CompareTo(nom);
        }

        public override string ToString()
        {
            string retVal = Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("Nom", $"{Nom}") +
                Forma.Texta2("Description", $"{Description}");

            return retVal;
        }

    }
    public class TypeAnimal : MyType
    {
        private string id;
        private TypeAnimal(string nom, string description) : base(nom, description)
        {
            id = Forma.SimpleId("TAN", AllTypeAnimal.Num + 1);
        }
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string ToString()
        {
            string retVal = this.Nom; /*
                Forma.Texta2("ID", $"{Id}\n") +
                base.ToString();*/


            return retVal;
        }

        public static TypeAnimal Creer(string nom, string description)
        {
            string f_nom = Forma.TrimUpper(nom);
            if (string.IsNullOrEmpty(f_nom))
            {
                throw new Exception($"[Type Animal] Ce nom est null");
            }
            TypeAnimal retVal = new(f_nom, description);
            
            return retVal;
        }
        public static int Save(TypeAnimal type)
        {
            int ret = 0;
            if (AllTypeAnimal.FindTypebyId(type.id) == null)
            {
                AllTypeAnimal.Add(type);
                ret = AllTypeAnimal.DB_Add(type);
            }
            return ret;
        }
        public int Update(string nom, string description)
        {
            int ret = 0;
            if (AllTypeAnimal.FindTypeByNom(Forma.TrimUpper(nom)) == null)
            {
                Nom = nom;
            }
            Description = description;
            ret = AllTypeAnimal.DB_Update(this);
            return ret;
        }
        public static int Delete(TypeAnimal type)
        {
            int ret = 0;
            if (AllTypeAnimal.FindTypebyId(type.Id) != null)
            {
                OnDelete(type);
                AllTypeAnimal.Delete(type.Id);
                ret = AllTypeAnimal.DB_Delete(type);
            }
            return ret;
        }

        private static int OnDelete(TypeAnimal type)
        {
            if (AllAnimal.Get(type).Any())
            {
                foreach (Animal ac in AllAnimal.Get(type))
                {
                    Animal.Delete(ac);
                }
            }
            return 1;
        }

    }
    public class TypeContact : MyType
    {
        private string id;
        private TypeContact(string nom, string description) : base(nom, description)
        {
            id = Forma.SimpleId("TCT", AllTypeContact.NumType + 9998);
        }
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string ToString()
        {
            string retVal =$"{this.Nom}";
                /*Forma.Center($"LES ROLES CONTACT - [ {Id} ] - \n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                Forma.Texta2("ID", $"{Id}") +
                base.ToString();*/

            return retVal;
        }

        public IEnumerable<TypeContact_Contact> EnumType()
        {
            foreach (TypeContact_Contact type in AllTypeContact_Contact.AllOfType(this).Values)
            {
                yield return type;
            }
        }

        public static TypeContact Creer(string nom, string description)
        {
            Forma.ParametreNullTesteur(nom);

            TypeContact retVal = new TypeContact(nom, description);
            return retVal;
        }
        public static int Save(TypeContact type)
        {
            if (AllTypeContact.FindByNom(type.Nom) != null)
            {
                throw new Exception($"[Type Animal] Ce Animal existe deja:\n{AllTypeContact.FindByNom(type.Nom)}");
            }
            AllTypeContact.Add(type);
            return AllTypeContact.DB_Add(type);
        }
        public int Update(string nom, string description)
        {
            int retVal = 0;
            if (AllTypeContact.FindByNom(Forma.TrimUpper(nom)) == null)
            {
                Nom = nom;
            }
            Description = description;
            retVal = AllTypeContact.DB_Update(this);

            return retVal;
        }
        public static int Delete(TypeContact type)
        {
            int retVal = 0;
            if (AllTypeContact.FindById(type.Id) != null)
            {
                OnDelete(type);
                AllTypeContact.Delete(type);
                retVal = AllTypeContact.DB_Delete(type);
            }

            return retVal;
            AllTypeContact.Delete(type);
        }

        private static int OnDelete(TypeContact type)
        {
            if (AllTypeContact_Contact.AllOfType(type).Count > 0)
            {
                foreach (TypeContact_Contact ac in type.EnumType())
                {
                    TypeContact_Contact.Delete(ac);
                }
            }
            return 1;
        }

    }
    
}



