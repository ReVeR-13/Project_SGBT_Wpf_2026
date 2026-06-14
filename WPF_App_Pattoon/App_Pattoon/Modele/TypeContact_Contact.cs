using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class TypeContact_Contact : ITable,IComparable<TypeContact_Contact>
    {
        private string _id;
        private DateTime _date;
        private Contact _contact;
        private TypeContact _type;

        private TypeContact_Contact(Contact contact, TypeContact type)
        {
            try
            {
                Id = $"{contact.Id}{type.Id}";
                DateCreation = DateTime.Now;
                Contact = contact;
                Type = type;
            }
            catch (Exception e)
            {
                ExceptionLauncher.New("TypeContact_Contact Instance", e.Message);
            }
            
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
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
                _date = Forma.Checked_DateCreation(value); 
            }
        }
        public Contact Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
            }
        }
        public TypeContact Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public int CompareTo(TypeContact_Contact? other)
        {
            return Id.CompareTo(other.Id);
        }
        public override string ToString()
        {
            string retVal = $"{this.Type.Nom}";/*string.Format($"{"{0,-8} : {1,-12}\n"}", "Date", $"{DateCreation.ToString("dd-MM-yyyy")}") +
                string.Format("{0,-8} : {1,-30}\n", "ID", $"{Id}") +
                string.Format("{0,-8} : {1,-12}\n", "Contact", $"{Contact.Nom} {Contact.Prenom}") +
                string.Format("{0,-8} : {1,-12}\n", "Roles", $"{Type.Nom}");*/

            return retVal;
        }


        public static TypeContact_Contact Creer(Contact contact, TypeContact type)
        {
            string id = $"{contact.Id}{type.Id}";
            if (contact == null || type == null)
            {
                ExceptionLauncher.New("GetUnType Contact - Contact", $"Parametre invalide");
            }

            return new TypeContact_Contact(contact, type);
        }
        public static int Save(TypeContact_Contact tpe)
        {
            AllTypeContact_Contact.Add(tpe);
            return AllTypeContact_Contact.DB_Add(tpe);
        }
        public static int Delete(TypeContact_Contact tpe)
        {
            int ret = 0;
            if (AllTypeContact_Contact.Find(tpe.Id) != null)
            {
                AllTypeContact_Contact.Delete(tpe.Id);
                ret = AllTypeContact_Contact.DB_Delete(tpe);
            }
            return ret;
            
        }


    }
}
