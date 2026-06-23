using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details;
using Wpf_App_Pattoon_Animalerie.ViewModel.WindowViewModel;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    class ContactViewModel : BaseViewModel
    {

        private string _id;
        private string _nom;
        private string? _prenom;
        private string _message;

        private Contact? _contactSelectionne;
        public ICommand NouveauCommand { get; }
        public ICommand OuvrirDetailCommand { get; }
        public ICommand TypesCommand { get; }

        //--------------------------------------------------

        private readonly ObservableCollection<Contact> _lesContacts;
        private readonly IWindowService _windowService;
        private readonly TypeContactViewModel _type;

        //---------------------------------------------------


        public ContactViewModel(IWindowService windowService)
        {
            this._lesContacts = new ObservableCollection<Contact>(AllContacts.LesStocks.Values);
            this._type = new TypeContactViewModel(windowService);

            _contactSelectionne = null;
            _id = string.Empty;
            _nom = string.Empty;
            _prenom = string.Empty;
            _message = string.Empty;

            _windowService = windowService;

            NouveauCommand = new RelayCommand(_ => NouveauContact(null), _ => true);
            TypesCommand = new RelayCommand(_ => TypeContact(), _ => true);
            OuvrirDetailCommand = new RelayCommand(_ => NouveauContact(this.ContactSelectionne), _ => this.ContactSelectionne != null);

        }

        public ObservableCollection<Contact> LesContacts { get => _lesContacts; }
        public Contact? ContactSelectionne
        {
            get { return _contactSelectionne; }
            set
            {
                _contactSelectionne = value;
                OnPropertyChanged();

            }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged(); }
        }
        public string? Prenom
        {
            get { return _prenom; }
            set { _prenom = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }

        }

        private void NouveauContact(Contact? contact)
        {
            var vm = new ContactDetailViewModel(contact,_type,_windowService,
                onSaved: MiseajourContact,
                onCreated: AjoutContact,
                onDelete: RemoveContact);
            _windowService.OuvrirDetail(vm);
        }

        private void TypeContact()
        {
            var selector = new SelectionViewModel<TypeContact>(_type.LesTypes);
            var selected = _windowService.OuvrirPopup(selector);

            if (selected != null && this.ContactSelectionne != null)
            {
                this.ContactSelectionne.AddType(selected);
            }

        }

        private void MiseajourContact(Contact contactModifiee)
        {
            var exist = LesContacts.FirstOrDefault(d => d.Id == contactModifiee.Id);
            if (exist != null)
            {

                var idx = LesContacts.IndexOf(exist);
                LesContacts[idx] = contactModifiee;

            }
        }
        private void AjoutContact(Contact newContact)
        {
            LesContacts.Add(newContact);
        }
        private void RemoveContact(Contact remContact)
        {
            LesContacts.Remove(remContact);
        }

    }
}
