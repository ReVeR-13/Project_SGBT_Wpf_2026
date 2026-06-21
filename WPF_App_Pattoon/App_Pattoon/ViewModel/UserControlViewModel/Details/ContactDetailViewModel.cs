using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Wpf_App_Pattoon_Animalerie.ViewModel.WindowViewModel;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    class ContactDetailViewModel : BaseViewModel
    {
        public ICommand AddRoleCommand { get; }
        public ICommand DeleteRoleCommand { get; }
        public ICommand AnnulerCommand { get; }
        public ICommand EnregistrerCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand GotoRoleCommand { get; }

        private readonly IWindowService _windowService;
        private readonly TypeContactViewModel _type;

        private Contact _contact;
        private ObservableCollection<TypeContact_Contact> _mesTypes;
        private TypeContact_Contact _contactRoleSelected;

        private string? _id;
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

        private string _message;

        public ContactDetailViewModel(Contact contact, TypeContactViewModel typeContactViewModel, IWindowService windowService)
        {
            _windowService = windowService;
            _contact = contact;
            _type = typeContactViewModel;
            _contactRoleSelected = null;

            _id = _contact == null ? string.Empty : _contact.Id;
            _date = _contact == null ? DateTime.Now : _contact.DateCreation;
            _niss = _contact == null ? string.Empty : _contact.Niss;
            _nom = _contact == null ? string.Empty : _contact.Nom;
            _prenom = _contact == null ? string.Empty : _contact.Prenom;
            _datenais = _contact == null ? DateTime.Now : _contact.DateNaissance;
            _gsm = _contact == null ? string.Empty : _contact.Gsm;
            _phone = _contact == null ? string.Empty : _contact.Telephone;
            _mail = _contact == null ? string.Empty : _contact.Mail;
            _codePostal = _contact == null ? string.Empty : _contact.CodePostal;
            _localite = _contact == null ? string.Empty : _contact.Localite;
            _adresse = _contact == null ? string.Empty : _contact.Adresse;

            _mesTypes = _contact != null ? new ObservableCollection<TypeContact_Contact>(_contact.ListeRoles.Values) : [] ;

            AddRoleCommand = new RelayCommand(_ => AddRole(), _ => ContactSelectionne != null);
            DeleteRoleCommand = new RelayCommand(_ => DeleteRole(), _ => ContactRoleSelected != null);
            AnnulerCommand  = new RelayCommand(_ => Annuler(), _ => ContactSelectionne != null);
            EnregistrerCommand = new RelayCommand(_ => Enregistrer(), _ => peutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => DeleteContact(), _ => ContactSelectionne != null);
            GotoRoleCommand = new RelayCommand(_ => OuvrirRoleContact(), _ => true);
            _message = string.Empty;

        }

        public string? Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {
                _date = value; OnPropertyChanged();
            }
        }
        public DateTime DateNaissance
        {
            get { return _datenais; }
            set
            {

                _datenais = value;
                OnPropertyChanged();
            }
        }
        public string? Niss
        {
            get { return _niss; }
            set { _niss = value; OnPropertyChanged(); }
        }
        public string? Nom
        {
            get { return _nom; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim().Length < 3)
                {
                    Message = $"[Contacts] Le nom n'est pas valable: {value}";
                }
                
                    _nom = Forma.TrimUpper(value); OnPropertyChanged();
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
                        Message =$"[Contacts] Le Prenom n'est pas valable: {value}";
                    }
                    
                }
                _prenom= value.Trim(); OnPropertyChanged();
            }
        }
        public string? Gsm
        {
            get { return _gsm; }
            set
            {
                if (!Forma.IsNum(value))
                {
                    Message =$"[Contacts] Le numero de GSM n'est pas valable: {value}";
                }
                
                _gsm = value.Trim().ToUpper(); OnPropertyChanged();
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
                        Message = $"[Contacts] Le num de Telephone n'est pas valable: {value}";
                    }
                    
                }
                _phone = value.Trim().ToUpper(); OnPropertyChanged();
            }
        }
        public string? Mail
        {
            get { return _mail; }
            set
            {
                if (!Forma.IsMail(value))
                {
                    Message = $"[Contacts] Le mail n'est pas valable: {value}";
                }
                
                _mail = value; OnPropertyChanged();
            }
        }
        public string? CodePostal
        {
            get
            {
                return _codePostal;
            }
            set
            {
                _codePostal = Forma.TrimUpper(value); OnPropertyChanged();
            }
        }
        public string? Localite
        {
            get
            {
                return _localite;
            }
            set
            {
                _localite = value; OnPropertyChanged();
            }
        }
        public string? Adresse
        {
            get
            {
                return _adresse;
            }
            set
            {
                _adresse = value; OnPropertyChanged();
            }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TypeContact_Contact> MesRoles
        {
            get { return _mesTypes; }
            set { _mesTypes = value; OnPropertyChanged(); }
        }
        public TypeContact_Contact? ContactRoleSelected
        {
            get => _contactRoleSelected;
            set
            {
                _contactRoleSelected = value; OnPropertyChanged(); 
            }
        }
        public Contact? ContactSelectionne
        {
            get { return _contact; }
            set { _contact = value; OnPropertyChanged(); }
        }


        private void AddRole()
        {
            if (_contact == null)
            {
                Message = $"Ce contat n est pas encore créé...";
                return;
            }

            IEnumerable<TypeContact> manquants = AllTypeContact.LesRolesManquant(_contact).Values;

            if (!manquants.Any())
            {
                Message = $"Tous les roles sont deja assignés à {ContactSelectionne.Nom}";
                return;
            }

            var selector = new SelectionViewModel<TypeContact>(manquants);
            TypeContact selectedRole = _windowService.OuvrirPopup(selector);

            if (selectedRole != null)
            {
                MesRoles.Add(_contact.AddType(selectedRole));
                AllContacts.DB_Sync();
                Message = $"Role Ajouté : {selectedRole.Nom}";
            }
        }
        private void DeleteRole()
        {
            if (this.ContactRoleSelected == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show
                (
                $"Voulez-vous retirer cette element:\n [{ContactRoleSelected.Type.Nom}]\n de la liste des roles de:\n [{ContactSelectionne.Nom}]",
                "Confimation de suppresion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );

            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    ContactSelectionne.RemoveType(ContactRoleSelected.Type);
                    MesRoles.Remove(ContactRoleSelected);
                    AllContacts.DB_Sync();

                    Message = "Role Retiré...";

                    _contactRoleSelected = null;

                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
                

                
            }
            
        }

        private void Annuler()
        {
            ContactRoleSelected = null;
            ContactSelectionne = null;
            Id = string.Empty;
            DateCreation = DateTime.Now;
            DateNaissance = DateTime.Now;
            Niss = string.Empty ;
            Nom = string.Empty ;
            Prenom = string.Empty ;
            Gsm = string.Empty ;
            Telephone = string.Empty ;
            Mail = string.Empty ;
            CodePostal = string.Empty ;
            Localite = string.Empty ;
            Adresse = string.Empty ;
            MesRoles = [];

        }
        private bool peutEnregistrer()
        {
            return !string.IsNullOrEmpty(Nom) && (Nom.Length > 3) &&
                   DateNaissance < DateTime.Now &&
                   !string.IsNullOrEmpty(Niss) && Niss.Length == 11 &&
                   !string.IsNullOrEmpty(Gsm) && Gsm.Length > 8 && Gsm.Length < 15 &&
                   !string.IsNullOrEmpty(Mail) && Mail.Contains('@');
        }
        private void Enregistrer()
        {
            try
            {
                if (ContactSelectionne == null)
                {
                    Contact contact = Contact.Creer(Niss, DateNaissance, Nom, Prenom, Gsm, Telephone, Mail, CodePostal, Localite, Adresse);
                    if (Contact.Save(contact) == 1)
                    {
                        ContactSelectionne = contact;
                        Message = $"{contact.Nom} est créé";
                    }
                }else
                {
                    Message= ContactSelectionne.Modification(Niss, DateNaissance, Nom, Prenom, Gsm, Telephone, Mail, CodePostal, Localite, Adresse);
                }
                

            }catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        private void DeleteContact()
        {
            try
            {
                MessageBoxResult result = Forma.MsgValidation("Delete Contact", $"Voulez-Supprimer {ContactSelectionne.Nom}");
                if (result == MessageBoxResult.Yes)
                {
                    if (Contact.Delete(ContactSelectionne) == 1)
                    {
                        ContactSelectionne = null;
                        Message = "Contact supprimé";
                    }
                }
                
            }catch (Exception ex)
            {
                Message = ex.Message;
            }
        }


        private void OuvrirRoleContact()
        {
            var vm = new TypeContactViewModel(_windowService);
            _windowService.OuvrirDetail(vm);
        }
    }
}
