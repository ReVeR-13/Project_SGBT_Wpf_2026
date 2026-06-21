using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    public class SortieDetailViewModel : BaseViewModel,ICloseable
    {
        private ObservableCollection<Demande> _lesDemandes;
        private ObservableCollection<MotifSortie> _lesMotifs;
        private Demande _demandeSelectionne;
        private MotifSortie _motifsSelectionne;
        private Sortie? _sortie;

        private string? _id;
        private DateTime _dateCreation;
        private string? _description;
        private string? _title;
        private string? _message;
        private string? _infoEtape;

        public event Action CloseFenetre;

        public ICommand NouveauCommand { get; }
        public ICommand EnregistreCommand { get; }
        public ICommand DeleteCommand { get; }

        public SortieDetailViewModel(Demande? demande)
        {
            DemandeSelectionne = demande;
            _lesDemandes = new ObservableCollection<Demande>();
            _lesMotifs = new ObservableCollection<MotifSortie>(AllMotifsSortie.LesStocks.Values);
            _sortie = AllSortie.Find(demande);


            _id = _sortie == null ? string.Empty : _sortie.Id;
            _dateCreation = _sortie == null ? DateTime.Now : _sortie.DateCreation;
            _description = _sortie == null ? string.Empty : _sortie.Details;
            MotifSelectionne = _sortie?.Motifs;

            if (demande != null)
            {
                _lesDemandes.Add(demande);

            }
            else
            {
                _lesDemandes = new ObservableCollection<Demande>(AllDemande.ListeAllDemande.Values.Where(a => a.Type != ETypeDemande.ENTREE));
            }

            NouveauCommand = new RelayCommand(_ => Nouveau(), _ => true);
            EnregistreCommand = new RelayCommand(_ => Enregistrer(), _ => DemandeSelectionne != null && MotifSelectionne != null && DemandeSelectionne.Statut == EStatutDemande.EN_COURS);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => _sortie != null);

            _title = $"FICHE SORTIE DE [ {_id} ]";
            _message = string.Empty;
            _infoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        public ObservableCollection<Demande> LesDemandes
        {
            get { return _lesDemandes; }
            set { _lesDemandes = value; OnPropertyChanged(); }
        }
        public ObservableCollection<MotifSortie> LesMotifs
        {
            get { return _lesMotifs; }
        }
        public Demande? DemandeSelectionne
        {
            get => _demandeSelectionne;
            set { _demandeSelectionne = value; OnPropertyChanged(); InfoEtape = Forma.InfoDemande(value); }
        }
        public MotifSortie? MotifSelectionne
        {
            get { return _motifsSelectionne; }
            set { _motifsSelectionne = value; OnPropertyChanged(); }
        }
        public Sortie SortieSelectionne
        {
            get { return _sortie; }
            set { _sortie = value; OnPropertyChanged(); InfoEtape = Forma.InfoDemande(value?.Demande); }
        }
        public string? Id
        {
            get
            {
                return _id;
            }
            set { _id = value; OnPropertyChanged(); }
        }
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public string? InfoEtape
        {
            get
            {
                return _infoEtape;
            }
            set { _infoEtape = value; OnPropertyChanged(); }
        }


        private void Nouveau()
        {
            LesDemandes = new ObservableCollection<Demande>(AllDemande.ListeAllDemande.Values.Where(a => a.Type != ETypeDemande.ENTREE && a.Statut == EStatutDemande.EN_COURS));
            DemandeSelectionne = null;
            MotifSelectionne = null;
            SortieSelectionne = null;
            Id = string.Empty;
            DateCreation = DateTime.Now;
            Description = string.Empty;
            Title = $"NOUVELLE SORTIE";
            Message = string.Empty;

            if (LesDemandes.Count == 0)
            {
                Message = $"Aucune demande n'est à ce niveau de traitemant";
            }
            InfoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        private void Enregistrer()
        {
            if (DemandeSelectionne?.Statut != EStatutDemande.EN_COURS)
            {
                Forma.MsgInfo("Details Sortie", $"cette sortie ne peut etre créé: Decision Actuel [ {DemandeSelectionne?.Statut} ] ?");
                return;
            }

            try
            {

                Sortie nouveau = Sortie.Creer(DemandeSelectionne, MotifSelectionne, Description);
                if (Sortie.Save(nouveau) == 1)
                {
                    Forma.SyncAllWithDB();

                    SortieViewModel.LesSorties?.Add(nouveau);
                    Forma.MsgInfo("Details Sortie", $"Sortie créé avec succes: [ {nouveau.Id} ] ?");
                    this.CloseFenetre?.Invoke();
                }


            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private void Delete()
        {
            if (SortieSelectionne == null)
            {
                Message = $"Action impossible..";
                return;
            }
            MessageBoxResult msg = Forma.MsgValidation("Sortie", LesMessage.Q_Suppression);
            if (msg == MessageBoxResult.Yes)
            {
                Sortie.Delete(SortieSelectionne);
                Forma.SyncAllWithDB();
                SortieViewModel.LesSorties?.Remove(SortieSelectionne);

                Forma.MsgInfo("Sortie",LesMessage.SuccesSuppression);
                this.CloseFenetre?.Invoke();
            }
            
        }

        

    }
}
