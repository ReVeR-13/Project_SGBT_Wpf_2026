using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    public class DemandeDetailViewModel : BaseViewModel,ICloseable
    {
        public event Action CloseFenetre;
        private readonly IWindowService _windowService;
        private ObservableCollection<Contact> _lesContacts;
        private ObservableCollection<Animal> _lesAnimaux;
        private ETypeDemande[] _lesTypes;

        private Demande? _demande;

        private DateTime _dateOuverture;
        private DateTime? _dateFermeture;
        private Contact? _contactSelectionne;
        private Animal? _animalSelectionne;
        private ETypeDemande? _typeSelectionne;
        private EStatutDemande? _statut;
        private string? _remarque;

        private string? _id;

        private string _message;
        private string _title;
        private string _infoEtape;
        private string? _infoSuivi;
        private string _label;

        public ICommand AnnulerCommand { get; }
        public ICommand EnregistreCommand { get; }
        public ICommand SupprimerCommand { get; }

        public ICommand NewContactCommand { get; }
        public ICommand NewAnimalCommand { get; }

        public ICommand SuivantCommand { get; }

        public DemandeDetailViewModel(Demande? demande,Contact? contact,Animal? animal,IWindowService windowService)
        {
            _lesContacts = [];
            _lesAnimaux = [];
            _lesTypes = Enum.GetValues<ETypeDemande>();
            _windowService = windowService;
            _demande = demande;
            _label = string.Empty;

            _id = demande?.Id;
            _dateOuverture = demande == null ? DateTime.Now : demande.DateCreation ;
            _dateFermeture = demande == null ? DateTime.Now : demande.DateFermeture;

            TypeSelectionne =demande?.Type;
            _statut = demande?.Statut;
            ContactSelectionne = demande?.Contact;
            AnimalSelectionne = demande?.Animal;

            _remarque = demande?.Remarque;

            AnnulerCommand = new RelayCommand(_ => AnnulerDemande(),_=> true);
            EnregistreCommand = new RelayCommand(_ => EnregistrerDemande(), _ => ContactSelectionne != null && AnimalSelectionne != null && TypeSelectionne != null);
            SuivantCommand = new RelayCommand(_ => DemandePhase2(_demande), _ => _demande != null);

            SupprimerCommand = new RelayCommand(_ => Delete(), _ => _demande != null);

            if (contact != null)
            {
                _lesContacts.Add(contact);
                ContactSelectionne = contact;
            }
            else
            {
                _lesContacts = new ObservableCollection<Contact>(AllContacts.LesStocks.Values);
            }

            if (animal != null)
            {
                _lesAnimaux.Add(animal);
                AnimalSelectionne = animal;
            }
            else
            {
                _lesAnimaux = new ObservableCollection<Animal>(AllAnimal.ListeAllAnimal.Values);
            }
             
            _title = $"FICHE DEMANDE N° [ {_id} ]";
            _infoSuivi = _demande?.InfosSuivi();
            _infoEtape = Forma.InfoDemande(DemandeSelectionne);


        }

        public ObservableCollection<Contact> LesContacts { get { return _lesContacts; } set { _lesContacts = value; OnPropertyChanged(); } }
        public ObservableCollection<Animal> LesAnimaux { get { return _lesAnimaux; } set { _lesAnimaux = value; OnPropertyChanged(); } }
        public ETypeDemande[] LesTypes { get { return _lesTypes; } }

        public Demande? DemandeSelectionne 
        { 
            get { return _demande; } 
            set 
            { 
                _demande = value;OnPropertyChanged(); 
                if (value == null)
                {
                    this.ContactSelectionne = null;
                    this.AnimalSelectionne = null;
                    this.TypeSelectionne = null;
                    this.Remarque = string.Empty;
                    this.DateFermeture = null;
                    this.Message = string.Empty;
                    _infoEtape = Forma.InfoDemande(value);
                }
            } 
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }    
        }
        public EStatutDemande? StatutDemande
        {
            get { return _statut; }
            set {_statut = value; OnPropertyChanged(); }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public DateTime DateOuverture
        {
            get { return _dateOuverture; }
            set { _dateOuverture = value; OnPropertyChanged(); }
        }
        public ETypeDemande? TypeSelectionne
        {
            get { return _typeSelectionne; }
            set 
            { 
                _typeSelectionne = value; OnPropertyChanged();
                Labeler = value.ToString();
                if (value == ETypeDemande.ENTREE)
                {
                    LesAnimaux = new ObservableCollection<Animal>(AllAnimal.ListeAllAnimal.Values.Where(a =>a.Statut != EStatutAnimal.REFUGE));
                }
                if (value != ETypeDemande.ENTREE)
                {
                    LesAnimaux = new ObservableCollection<Animal>(AllAnimal.ListeAllAnimal.Values.Where(a => a.Statut == EStatutAnimal.REFUGE));
                }
            }
        }
        public string? Remarque
        {
            get { return _remarque; }
            set { _remarque = value; OnPropertyChanged(); }
        }
        public DateTime? DateFermeture
        {
            get { return _dateFermeture; }
            set { _dateFermeture = value; OnPropertyChanged(); }
        }
        public string InfoEtape
        {
            get
            {
                return _infoEtape;
            }
            set { _infoEtape = value; OnPropertyChanged(); }
        }

        public Contact? ContactSelectionne
        {
            get { return _contactSelectionne; }
            set { _contactSelectionne = value; OnPropertyChanged(); }
        }
        public Animal? AnimalSelectionne
        {
            get { return _animalSelectionne; }
            set { _animalSelectionne = value;OnPropertyChanged(); }
        }
        public string? InfosSuivi
        {
            get
            {
                return _infoSuivi;
            }
            set { _infoSuivi = value; OnPropertyChanged(); }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }  
        public string Labeler
        {
            get { return _label; }
            set
            {
                if (value == "DECES" || value == "NAISSANCE")
                {
                    _label = "SORTIE";
                }
                else
                {
                    _label = value;
                }
                 OnPropertyChanged();
            }
        }

        public void AnnulerDemande()
        {
            this.DemandeSelectionne = null;
            Id = string.Empty;
            DateOuverture = DateTime.Now;
            TypeSelectionne = null;
            StatutDemande = EStatutDemande.EXAMINATION;
            Title = $"CREATION DE NOUVELLE DEMANDE";
            InfosSuivi = string.Empty;
            LesContacts = new ObservableCollection<Contact>(AllContacts.LesStocks.Values);
            LesAnimaux = new ObservableCollection<Animal>(AllAnimal.ListeAllAnimal.Values);
            InfoEtape = Forma.InfoDemande(DemandeSelectionne);

        }
        public void EnregistrerDemande()
        {
            if (DemandeSelectionne == null)
            {
                NouvelDemande();
            }
            else
            {
                Message = $"--";
                
            }
        }
        private void NouvelDemande()
        {
            if (ContactSelectionne == null || AnimalSelectionne == null || TypeSelectionne == null)
            {
                Message = $"Il manque des elements pour valider cette demande";
                return;
            }
            try
            {
                Demande demande = Demande.Creer(ContactSelectionne, AnimalSelectionne, TypeSelectionne, Remarque);

                if (demande == null)
                {
                    Message = $"La Demande n' a pu etre créé ...";
                    return;
                }

                if (Demande.Save(demande) == 1)
                {
                    Message = $"[Succes] Demande enregistré avec succes";

                    MessageBoxResult msgBox = MessageBox.Show
                    (
                    $"Voulez-vous poursuivre vers: [ {demande.Type} ] ?",
                    "Poursuivre La demande",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                    if (msgBox == MessageBoxResult.Yes)
                    {
                        DemandePhase2(demande);
                    }
                    AnnulerDemande();
                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            
        }
        private void DemandePhase2(Demande demande)
        {
            BaseViewModel vm = null ;
            switch (demande.Type)
            {
                case ETypeDemande.ADOPTION:
                    {
                        Adoption? adoption = AllAdoption.Find(demande);
                        if (adoption == null)
                        {
                            adoption = Adoption.Creer(demande, "");
                            if (Adoption.Save(adoption) == 1)
                            {
                                Forma.MsgInfo("Adoption Reussite", "Adoption Creer");
                            }
                        }
                        
                        vm = new AdoptionDetailViewModel(adoption,demande,_windowService); 
                    }
                    break;
                case ETypeDemande.ACCUEIL:
                    {
                        Accueil? accueil = AllAccueil.Find(demande);
                        if (accueil == null)
                        {
                            accueil = Accueil.Creer(demande, "");
                            if (Accueil.Save(accueil) == 1)
                            {
                                Forma.MsgInfo("Adoption Reussite", "Accueil Creer");
                            }
                        }
                        vm = new AccueilDetailViewModel(accueil,demande,_windowService);
                    }
                    break;
                case ETypeDemande.ENTREE:
                    {
                        vm = new EntreeDetailViewModel(demande);
                    }
                    break;
                case ETypeDemande.SORTIE:
                    {
                        Sortie sortie = AllSortie.Find(demande);
                        vm = new SortieDetailViewModel(demande);
                    }
                    break;
                case ETypeDemande.DECES:
                    {
                        vm = new SortieDetailViewModel(demande);
                    }
                    break;
                case ETypeDemande.NAISSANCE:
                    {
                        vm = new SortieDetailViewModel(demande);
                    }break;

                    
            }
            this.CloseFenetre?.Invoke();
            _windowService.OuvrirDetail(vm);
        }

        private void Delete()
        {
            try
            {
                MessageBoxResult result = Forma.MsgValidation("Delete demande", "Voulez-vous supprimer cette demande?");
                if (result == MessageBoxResult.Yes)
                {
                    if (Demande.Delete(DemandeSelectionne) == 1)
                    {
                        Message = "Demande supprimer...";
                        AnnulerDemande();
                    }
                    
                }
            }catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

    }
}
