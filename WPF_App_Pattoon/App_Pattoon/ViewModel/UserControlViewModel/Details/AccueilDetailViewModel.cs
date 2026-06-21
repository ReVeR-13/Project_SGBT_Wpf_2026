using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    public class AccueilDetailViewModel : BaseViewModel, ICloseable
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
        private Accueil? _accueil;
        private string _infoEtape;

        public event Action CloseFenetre;
        private IWindowService _windowService;

        public ICommand NouveauCommande { get; }
        public ICommand EnregistreCommande { get; }
        public ICommand DeleteCommande { get; }
        public ICommand VersSortieCommande { get; }

        public AccueilDetailViewModel(Accueil? accueil, Demande? demande, IWindowService windowService)
        {

            _windowService = windowService;
            _lesdemandesDisponibles = new ObservableCollection<Demande>();

            AccueilSelectionne = accueil;
            _id = accueil != null ? accueil.Id : string.Empty;
            DemandeSelectionne = demande;
            StatutSelectionne = accueil != null ? accueil.Decision : EStatutValidation.EN_COURS;
            _date = accueil != null ? accueil.DateCreation : DateTime.Now;
            _dateD = accueil != null ? accueil.DateDebut : DateTime.Now;
            _dateF = accueil != null ? accueil.DateFin : DateTime.Now;
            _raisonRefus = accueil != null ?    accueil.RaisonAnullation : string.Empty;
            _infos = accueil != null ?  accueil.Info : string.Empty;
            _message = string.Empty;


            NouveauCommande = new RelayCommand(_ => NouvelAccueil(), _ => true);
            EnregistreCommande = new RelayCommand(_ => EnregistrerAccueil(), _ => this._demandeSelectionne != null);
            DeleteCommande = new RelayCommand(_ => SupprimerAccueil(), _ => this._accueil != null);
            VersSortieCommande = new RelayCommand(_ => VersSortie(), _ => this.StatutSelectionne == EStatutValidation.ACCEPTEE && _accueil != null);

            _title = $"DETAILS DE LA FICHE [ {_id} ]";
            InfoEtape = Forma.InfoDemande(_demandeSelectionne);
            this.Init();


        }

        public ObservableCollection<Demande> LesDemandes
        {
            get { return _lesdemandesDisponibles; }
            set { _lesdemandesDisponibles = value; OnPropertyChanged(); }
        }
        public Accueil? AccueilSelectionne
        {
            get { return _accueil; }
            set { _accueil = value; OnPropertyChanged(); }
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
                _statutSelectionne = AccueilSelectionne != null ? AccueilSelectionne.Decision : EStatutValidation.EN_COURS;

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

        private void NouvelAccueil()
        {
            DemandeSelectionne = null;
            StatutSelectionne = EStatutValidation.EN_COURS;
            AccueilSelectionne = null;

            Id = string.Empty;
            DateCreation = DateTime.Now;
            DateD = DateTime.Now;
            DateF = DateTime.Now;

            RaisonRefus = string.Empty;
            Infos = string.Empty;

            Message = string.Empty;
            LesDemandes = new ObservableCollection<Demande>(AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, ETypeDemande.ACCUEIL).Values);
            if (LesDemandes.Count == 0)
            {
                Message = $"Aucune de mande disponible";
            }

            Title = $"CREATION D'UNE NOUVELLE ADOPTION";
            InfoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        private void EnregistrerAccueil()
        {
            if (_demandeSelectionne == null || _demandeSelectionne.Statut > EStatutDemande.EN_COURS)
            {
                Message = $"La Demande est invalide";
                return;
            }

            try
            {
                Accueil? nouveau = Accueil.Creer(_demandeSelectionne, Infos);

                if (Accueil.Save(nouveau) == 1)
                {
                    AllAccueil.DB_Sync();
                    Init();
                    Title = $"DETAILS DE LA FICHE [ {nouveau.Id} ]";
                    NouvelAccueil();
                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }


        }

        private void SupprimerAccueil()
        {
            if (_accueil == null)
            {
                Message = $"Suppression impossible";
            }

            Accueil.Delete(_accueil);
            InfoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        private void Init()
        {

            if (DemandeSelectionne != null)
            {
                _lesdemandesDisponibles.Add(_demandeSelectionne);
            }
            else
            {
                _lesdemandesDisponibles = new ObservableCollection<Demande>(AllDemande.ListeByType(ETypeDemande.ACCUEIL).Values);
            }

            _message = string.Empty;
        }
        private void MsgLock()
        {
            MessageBoxResult msgBox = MessageBox.Show
                (
                $"Cette option n 'est pas disponible à cet stade de la demande: [ {AccueilSelectionne?.Decision} ] ?",
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
                        AccueilSelectionne?.Accepter();
                    }
                    break;
                case EStatutValidation.REFUSEE:
                    {
                        AccueilSelectionne?.Refuser(RaisonRefus);
                    }
                    break;
                case EStatutValidation.EN_COURS:
                    {
                        AccueilSelectionne?.Indecis();
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
