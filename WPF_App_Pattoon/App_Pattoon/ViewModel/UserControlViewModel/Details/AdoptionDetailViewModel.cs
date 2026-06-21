using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    public class AdoptionDetailViewModel : BaseViewModel, ICloseable
    {
        private ObservableCollection<Demande> _lesdemandesDisponibles;
        private string? _id;
        private Demande? _demandeSelectionne;
        private EStatutValidation _statutSelectionne;
        private DateTime? _date;
        private DateTime? _dateD;
        private DateTime? _dateF;
        private string? _raisonRefus;
        private string? _infos;

        private string? _message;
        private string? _title;
        private Adoption? _adoption;
        private string _infoEtape;

        public event Action CloseFenetre;
        private IWindowService _windowService;

        public ICommand NouveauCommande { get; }
        public ICommand EnregistreCommande { get; }
        public ICommand DeleteCommande { get; }
        public ICommand VersSortieCommande { get; }

        public AdoptionDetailViewModel(Adoption? adoption, Demande? demande, IWindowService windowService)
        {

            _windowService = windowService;
            _lesdemandesDisponibles = new ObservableCollection<Demande>();

            AdoptionSelectionne = adoption;
            _id = adoption != null ? adoption.Id : string.Empty;
            DemandeSelectionne = demande;
            StatutSelectionne = adoption != null ? adoption.Decision : EStatutValidation.EN_COURS;
            _date = adoption != null ? adoption.DateCreation : DateTime.Now;
            _dateD = adoption != null ? adoption.DateD : DateTime.Now;
            _dateF = adoption != null ? adoption.DateF : DateTime.Now;
            _raisonRefus = adoption != null ? adoption.RaisonRefus : string.Empty;
            _infos = adoption != null ? adoption.Info : string.Empty;
            _message = string.Empty;


            NouveauCommande = new RelayCommand(_ => NouvelAdoption(), _ => true);
            EnregistreCommande = new RelayCommand(_ => EnregistrerAdoption(), _ => this._demandeSelectionne != null);
            DeleteCommande = new RelayCommand(_ => SupprimerAdoption(), _ => this._adoption != null);
            VersSortieCommande = new RelayCommand(_ => VersSortie(), _ => this.StatutSelectionne == EStatutValidation.ACCEPTEE && _adoption != null);

            _title = $"DETAILS DE LA FICHE [ {_id} ]";

            if (_adoption == null && DemandeSelectionne != null)
            {
                _message = $"Cette adoption doit etre enregistré";

            }

            if (StatutSelectionne == EStatutValidation.EN_COURS)
            {
                _message = $"Cette adoption est en attente de decision";
            }
            InfoEtape = Forma.InfoDemande(_demandeSelectionne);
            this.Init();


        }

        public ObservableCollection<Demande> LesDemandes
        {
            get { return _lesdemandesDisponibles; }
            set { _lesdemandesDisponibles = value; OnPropertyChanged(); }
        }
        public Adoption? AdoptionSelectionne
        {
            get { return _adoption; }
            set { _adoption = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public DateTime? DateCreation
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        public Demande DemandeSelectionne
        {
            get { return _demandeSelectionne; }
            set
            {

                _demandeSelectionne = value;
                InfoEtape = Forma.InfoDemande(value);
                OnPropertyChanged();

            }
        }
        public EStatutValidation StatutSelectionne
        {
            get
            {
                return _statutSelectionne;
            }
            set
            {
                _statutSelectionne = AdoptionSelectionne != null ? AdoptionSelectionne.Decision : EStatutValidation.EN_COURS;

                if (DemandeSelectionne != null)
                {
                    if (DemandeSelectionne.Statut < EStatutDemande.TERMINEE)
                    {
                        _statutSelectionne = value;
                        InfoEtape = Forma.InfoDemande(DemandeSelectionne);
                        OnPropertyChanged();
                        Decision();

                    }
                    else
                    {
                        Message = "cette element n 'est pas disponible";
                    }
                }

            }
        }

        public DateTime? DateD
        {
            get { return _dateD; }
            set { _dateD = value; OnPropertyChanged(); }
        }
        public DateTime? DateF
        {
            get { return _dateF; }
            set { _dateF = value; OnPropertyChanged(); }
        }

        public string RaisonRefus
        {
            get { return _raisonRefus; }
            set { _raisonRefus = value; OnPropertyChanged(); }
        }
        public string Infos
        {
            get { return _infos; }
            set { _infos = value; OnPropertyChanged(); }

        }
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public string InfoEtape
        {
            get
            {
                return _infoEtape;
            }
            set { _infoEtape = value; OnPropertyChanged(); }
        }

        private void NouvelAdoption()
        {
            DemandeSelectionne = null;
            StatutSelectionne = EStatutValidation.EN_COURS;
            AdoptionSelectionne = null;

            Id = string.Empty;
            DateCreation = DateTime.Now;
            DateD = DateTime.Now;
            DateF = DateTime.Now;

            RaisonRefus = string.Empty;
            Infos = string.Empty;

            Message = string.Empty;
            LesDemandes = new ObservableCollection<Demande>(AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, ETypeDemande.ADOPTION).Values);
            if (LesDemandes.Count == 0)
            {
                Message = $"Aucune de mande disponible";
            }

            Title = $"CREATION D'UNE NOUVELLE ADOPTION";
            InfoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        private void EnregistrerAdoption()
        {
            if (_demandeSelectionne == null || _demandeSelectionne.Statut > EStatutDemande.EN_COURS)
            {
                Message = $"La Demande est invalide";
                return;
            }

            try
            {
                Adoption nouveau = Adoption.Creer(_demandeSelectionne, Infos);

                if (Adoption.Save(nouveau) == 1)
                {
                    AllAdoption.DB_Sync();
                    Init();
                    Title = $"DETAILS DE LA FICHE [ {nouveau.Id} ]";
                    NouvelAdoption();
                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }


        }

        private void SupprimerAdoption()
        {
            try
            {
                if (AdoptionSelectionne == null)
                {
                    Message = $"Suppression impossible";
                }

                MessageBoxResult result = Forma.MsgValidation("Adoption", "Voulez vous supprimer cet element?");

                if (result == MessageBoxResult.Yes)
                {
                    Adoption.Delete(_adoption);
                    InfoEtape = Forma.InfoDemande(DemandeSelectionne);
                    NouvelAdoption();
                }
                
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            
        }

        private void Init()
        {

            if (DemandeSelectionne != null)
            {
                _lesdemandesDisponibles.Add(_demandeSelectionne);
            }
            else
            {
                _lesdemandesDisponibles = new ObservableCollection<Demande>(AllDemande.ListeByType(ETypeDemande.ADOPTION).Values);
            }

            _message = string.Empty;
        }
        private void MsgLock()
        {
            MessageBoxResult msgBox = MessageBox.Show
                (
                $"Cette option n 'est pas disponible à cet stade de la demande: [ {AdoptionSelectionne?.Decision} ] ?",
                "Details Adoption",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }

        private void Decision()
        {
            switch (_statutSelectionne)
            {
                case EStatutValidation.ACCEPTEE:
                    {
                        AdoptionSelectionne?.Accepter();
                    }
                    break;
                case EStatutValidation.REFUSEE:
                    {
                        AdoptionSelectionne?.Refuser(RaisonRefus);
                    }
                    break;
                case EStatutValidation.EN_COURS:
                    {
                        AdoptionSelectionne?.Indecis();
                    }
                    break;
            }
            AllDemande.DB_Sync();
            InfoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        private void VersSortie()
        {
            var vm = new SortieDetailViewModel(DemandeSelectionne);
            this.CloseFenetre?.Invoke();
            _windowService.OuvrirDetail(vm);
        }
    }
}
