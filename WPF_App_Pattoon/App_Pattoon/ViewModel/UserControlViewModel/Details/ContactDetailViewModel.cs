using System.Collections.ObjectModel;
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

        private readonly IWindowService _windowService;
        private readonly TypeContactViewModel _type;

        private Contact _contact;
        private ObservableCollection<TypeContact_Contact> _mesTypes;

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

        private string _message;

        public ContactDetailViewModel(Contact contact, TypeContactViewModel typeContactViewModel, IWindowService windowService)
        {
            _windowService = windowService;
            _contact = contact;
            _type = typeContactViewModel;

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

            AddRoleCommand = new RelayCommand(_ => AddRole(), _ => true);
            _message = string.Empty;

        }

        public string? Id
        {
            get { return _id; }
        }
        public DateTime? DateCreation
        {
            get { return _date; }
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
                _mail= value; OnPropertyChanged();
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

        public ObservableCollection<TypeContact_Contact> MesRoles
        {
            get { return _mesTypes; }
        }
        public Contact Contact
        {
            get { return _contact; }
            set { _contact = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        private void AddRole()
        {
            var selector = new SelectionViewModel<TypeContact>(_type.LesTypes);
            TypeContact selectedRole = _windowService.OuvrirPopup(selector);

            Console.WriteLine(selectedRole.ToString());

            if (selectedRole != null)
            {
                MesRoles.Add(_contact.AddType(selectedRole));
                AllContacts.DB_Sync();
            }
        }
    }
}
