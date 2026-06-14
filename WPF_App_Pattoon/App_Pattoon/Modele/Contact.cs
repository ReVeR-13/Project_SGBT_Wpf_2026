using Wpf_App_Pattoon_Animalerie.Service;
namespace Wpf_App_Pattoon_Animalerie.Modele
{


    public class Contact : ITable, IComparable<Contact>
    {
        private string _id;
        private DateTime _date;
        private string _niss;
        private string _nom;
        private string? _prenom;
        private DateTime _datenais;
        private string _gsm;
        private string? _phone;
        private string _mail;
        private string _codePostal;
        private string _localite;
        private string _adresse;

        private Dictionary<string, TypeContact_Contact> _roles;

        private Contact(string niss, DateTime datenaiss, string nom, string? prenom, string gsm, string? tel, string mail, string cp, string localite, string adresse)
        {
            _id = Forma.SimpleId("CNT", AllContacts.NumContact + 1);
            _date = DateTime.Now;
            Niss = niss;
            Nom = nom;
            Prenom = prenom ?? "--";
            Gsm = gsm;
            Telephone = tel ?? gsm;
            Mail = mail;
            Adresse = adresse;
            CodePostal = cp;
            Localite = localite;

            DateNaissance = datenaiss;

            _roles = new Dictionary<string, TypeContact_Contact>();

        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {

                _date = Forma.Checked_DateCreation(value);

            }
        }
        public DateTime DateNaissance
        {
            get { return _datenais; }
            set
            {

                _datenais = Forma.Checked_DateCreation(value);
            }
        }
        public string Niss
        {
            get { return _niss; }
            set { _niss = value; }
        }
        public string Nom
        {
            get { return _nom; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim().Length < 3)
                {
                    throw new Exception($"[Contacts] Le nom n'est pas valable: {value}");
                }
                _nom = Forma.TrimUpper(value);
            }
        }
        public string? Prenom
        {
            get { return _prenom; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Trim().Length < 2)
                    {
                        throw new Exception($"[Contacts] Le Prenom n'est pas valable: {value}");
                    }
                }
                _prenom = value.Trim();
            }
        }
        public string Gsm
        {
            get { return _gsm; }
            set
            {
                if (!Forma.IsNum(value))
                {
                    throw new Exception($"[Contacts] Le numero de GSM n'est pas valable: {value}");
                }
                _gsm = value.Trim().ToUpper();
            }
        }
        public string? Telephone
        {
            get { return _phone; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (!Forma.IsNum(value))
                    {
                        throw new Exception($"[Contacts] Le num de Telephone n'est pas valable: {value}");
                    }
                }
                _phone = value.Trim().ToUpper();
            }
        }
        public string Mail
        {
            get { return _mail; }
            set
            {
                if (!Forma.IsMail(value))
                {
                    throw new Exception($"[Contacts] Le mail n'est pas valable: {value}");
                }
                _mail = value;
            }
        }
        public string CodePostal
        {
            get
            {
                return _codePostal;
            }
            set
            {
                _codePostal = Forma.TrimUpper(value);
            }
        }
        public string Localite
        {
            get
            {
                return _localite;
            }
            set
            {
                _localite = value;
            }
        }
        public string Adresse
        {
            get
            {
                return _adresse;
            }
            set
            {
                _adresse = value;
            }
        }

        public Dictionary<string, TypeContact_Contact> ListeRoles
        {
            get { return this._roles; }
        }

        public int TypeCount
        {
            get
            {
                return AllTypeContact_Contact.AllOfContact(this).Count;
            }
        }


        public string LesTypesString()
        {
            int i = 0;
            string retVal = Forma.Center($"Liste des Types [{TypeCount}]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                Forma.Padding(Forma.Text("N°", "Id", "Date Crea.", "Nom", "Descrip."));
            foreach (TypeContact_Contact t in AllTypeContact_Contact.AllOfContact(this).Values)
            {
                i++;
                retVal += Forma.Padding(Forma.Text($"{i}", $"{t.Id}", $"{t.DateCreation:dd-MM-yyyy}", $"{t.Type.Nom}", $"{t.Type.Description}"));
            }

            return retVal;
        }
        public string LesTypesManquantsString()
        {

            return AllTypeContact.LesTypesContactsManquant(this);

        }
        public string ListeDemandesString()
        {
            int i = 0;
            string retVal =
                Forma.Text("N°", "Id", "Date Crea.", "Statut", "Remarque");

            if (GetAllDemandes().Count > 0)
            {
                foreach (Demande dm in GetAllDemandes().Values)
                {
                    i++;
                    retVal += Forma.Text($"{i}°", $"{dm.Id}", $"{dm.DateCreation:dd-MM-yyyy}", $"{dm.Statut}", $"{dm.Remarque}");
                }
            }
            else
            {
                retVal = "Pas de demande";
            }
            return $"Liste des Demande de {Nom} [{i}]\n\n" + retVal;
        }
        public Dictionary<string, Demande> GetAllDemandes()
        {
            return AllDemande.GetAllDemandeByContact(this);
        }
        public string ListeDemandesByStatutString(EStatutDemande eStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Statut", "Remarque");

            if (GetAllDemandes().Count > 0)
            {
                foreach (Demande dm in GetAllDemandes().Values)
                {
                    if (dm.Statut == eStatut)
                    {
                        i++;
                        retVal += Forma.Text($"{i}°", $"{dm.Id}", $"{dm.DateCreation:dd-MM-yyyy}", $"{dm.Statut}", $"{dm.Remarque}");
                    }

                }
            }
            else
            {
                retVal = "Pas de demande";
            }
            return $"Liste des Demande En Cours... de {Nom} [{i}]\n\n" + retVal;
        }

        public Dictionary<string, TypeContact_Contact> GetRolesListe()
        {
            return AllTypeContact_Contact.AllOfContact(this);
        }
        public TypeContact_Contact AddType(TypeContact typeContact)
        {
            if (this.GetRolesListe().ContainsKey(typeContact.Id))
            {
                ExceptionLauncher.New("Conctact", "Cet type existe deja dans la liste");
            }
            TypeContact_Contact lien =  TypeContact_Contact.Creer(this, typeContact);
            TypeContact_Contact.Save(lien);

            this._roles.Add(typeContact.Id, lien);
            return lien;
        }
        public int RemoveType(TypeContact type)
        {
            int ret = 0;
            TypeContact_Contact cible = AllTypeContact_Contact.Find(this, type);
            if (cible == null)
            {
                ExceptionLauncher.New("Conctact", "Cet type n' existe pas dans la liste");
            }
            this._roles.Remove(type.Id);
            return TypeContact_Contact.Delete(cible);
        }
        public IEnumerable<TypeContact_Contact> EnumType()
        {
           return _roles.Values;
        }
        public TypeContact_Contact? GetUnType(TypeContact type)
        {
            TypeContact_Contact? retval = null;
            if (type != null && this.GetRolesListe().ContainsKey(type.Id))
            {
                retval = this.GetRolesListe()[type.Id];
            }
            return retval;
        }

        public int CompareTo(Contact contact)
        {
            return this.Id.CompareTo(contact.Id);
        }
        public override string ToString()
        {
            string retVal = Id; /*

                Forma.Center($"CONTACT - [ {Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Niss", $"{Niss}") +
                Forma.Texta2("Date de Naiss.", $"{DateNaissance:dd-MM-yyyy}") +
                Forma.Texta2("Nom", $"{Nom}") +
                Forma.Texta2("Prenom", $"{Prenom}") +
                Forma.Texta2("Gsm", $"{Gsm}") +
                Forma.Texta2("Telephone", Telephone ?? "--") +
                Forma.Texta2("Email", Mail) +
                Forma.Texta2("Adresse", $"{Adresse} | {Localite} | {CodePostal}") +
                Forma.Texta2("Nbre Type", $"{TypeCount}");

            retVal += "\n\n" + LesTypesString();*/

            return retVal;
        }

        public static Contact Creer(string niss, DateTime datenaissance, string nom, string prenom, string gsm, string? tel, string mail, string cp, string localite, string adresse)
        {
            Contact retVal = new Contact(niss, datenaissance, nom, prenom, gsm, tel, mail, cp, localite, adresse);

            return retVal;
        }
        public string Modification(string? niss, DateTime? datenaissance, string? nom, string? prenom, string? gsm, string? tel, string? mail, string? cp, string? localite, string? adresse)
        {
            string retVal = $"Updated effectuée(s) sur --{Nom}--\n";
            int i = 0;
            if (!string.IsNullOrEmpty(nom))
            {
                Nom = nom;
                i++;
                retVal += $"Nom modifié: {Nom}\n";
            }

            if (!string.IsNullOrEmpty(prenom))
            {
                Prenom = prenom;
                i++;
                retVal += $"Prenom modifié: {Prenom}\n";
            }

            if (!string.IsNullOrEmpty(niss))
            {
                Niss = niss;
                i++;
                retVal += $"Niss modifié: {Niss}\n";
            }

            if (datenaissance != null)
            {
                DateNaissance = (DateTime)datenaissance;
                i++;
                retVal += $"Date de naissance modifié: {DateNaissance}\n";
            }

            if (!string.IsNullOrEmpty(gsm))
            {
                Gsm = gsm;
                i++;
                retVal += $"Gsm modifié: {Gsm}\n";
            }

            if (!string.IsNullOrEmpty(tel))
            {
                Telephone = tel;
                i++;
                retVal += $"Telephone modifié: {Telephone}\n";
            }

            if (!string.IsNullOrEmpty(mail))
            {
                Mail = mail;
                i++;
                retVal += $"Mail modifié: {Mail}\n";
            }

            if (!string.IsNullOrEmpty(cp))
            {
                CodePostal = cp;
                i++;
                retVal += $"Code postal modifié: {cp}\n";
            }

            if (!string.IsNullOrEmpty(localite))
            {
                Localite = localite;
                i++;
                retVal += $"Localite modifié: {localite}\n";
            }

            if (!string.IsNullOrEmpty(adresse))
            {
                Adresse = adresse;
                i++;
                retVal += $"Adresse modifié: {Adresse}\n";
            }
            if (i > 0)
            {
                AllContacts.DB_Update(this);
            }

            return retVal;


        }
        public static int Save(Contact contact)
        {
            int retVal = 0;
            if (AllContacts.Find(contact.Id) == null)
            {
                AllContacts.Add(contact);
                retVal = AllContacts.DB_Add(contact);
            }
            return retVal;
        }
        public static int Delete(Contact contact)
        {
            int retVal = 0;
            if (AllContacts.Find(contact.Id) != null)
            {
                OnDelete(contact);
                AllContacts.Remove(contact);
                retVal = AllContacts.DB_Delete(contact);
            }

            return retVal;
        }

        private static int OnDelete(Contact contact)
        {
            if (AllTypeContact_Contact.AllOfContact(contact).Count > 0)
            {
                foreach (TypeContact_Contact ac in contact.EnumType())
                {
                    TypeContact_Contact.Delete(ac);
                }
            }

            if (AllDemande.GetAllDemandeByContact(contact).Count > 0)
            {
                foreach (Demande d in contact.GetAllDemandes().Values)
                {
                    Demande.Delete(d);
                }
            }
            return 1;
        }

    }

}
