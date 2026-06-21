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
    public class EntreeDetailViewModel:BaseViewModel,ICloseable
    {

        private ObservableCollection<Demande> _lesDemandes;
        private ObservableCollection<MotifEntree> _lesMotifs;
        private Demande _demandeSelectionne;
        private MotifEntree? _motifsSelectionne;
        private Entree? _entree;

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

        public EntreeDetailViewModel(Demande? demande)
        {
            DemandeSelectionne = demande;
            _lesDemandes = [];
            _lesMotifs = new ObservableCollection<MotifEntree>(AllMotifsEntrees.LesStocks.Values);
            _entree = AllEntree.Find(demande);


            _id = _entree == null ? string.Empty : _entree.Id;
            _dateCreation = _entree == null ? DateTime.Now : _entree.DateCreation;
            _description = _entree == null ? string.Empty : _entree.Details;
            MotifSelectionne = _entree?.Motifs;

            if (demande != null)
            {
                _lesDemandes.Add(demande);

            }
            else
            {
                _lesDemandes = new ObservableCollection<Demande>(AllDemande.ListeAllDemande.Values.Where(a => a.Type == ETypeDemande.ENTREE));
            }

            NouveauCommand = new RelayCommand(_ => Nouveau(), _ => true);
            EnregistreCommand = new RelayCommand(_ => Enregistrer(), _ => DemandeSelectionne != null && MotifSelectionne != null && DemandeSelectionne.Statut == EStatutDemande.EN_COURS);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => _entree != null);

            _title = $"FICHE ENTREE DE [ {_id} ]";
            _message = string.Empty;
            _infoEtape = Forma.InfoDemande(DemandeSelectionne);
        }

        public ObservableCollection<Demande> LesDemandes
        {
            get { return _lesDemandes; }
            set { _lesDemandes = value; OnPropertyChanged(); }
        }
        public ObservableCollection<MotifEntree> LesMotifs
        {
            get { return _lesMotifs; }
        }
        public Demande? DemandeSelectionne
        {
            get => _demandeSelectionne;
            set { _demandeSelectionne = value; OnPropertyChanged(); InfoEtape = Forma.InfoDemande(value); }
        }
        public MotifEntree? MotifSelectionne
        {
            get { return _motifsSelectionne; }
            set { _motifsSelectionne = value; OnPropertyChanged(); }
        }
        public Entree EntreeSelectionne
        {
            get { return _entree; }
            set { _entree = value; OnPropertyChanged(); InfoEtape = Forma.InfoDemande(value?.Demande); }
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
            LesDemandes = new ObservableCollection<Demande>(AllDemande.ListeAllDemande.Values.Where(a => a.Type == ETypeDemande.ENTREE && a.Statut == EStatutDemande.EN_COURS));
            DemandeSelectionne = null;
            MotifSelectionne = null;
            EntreeSelectionne = null;
            Id = string.Empty;
            DateCreation = DateTime.Now;
            Description = string.Empty;
            Title = $"NOUVELLE ENTREE";
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
                Forma.MsgInfo("Details Entree", $"cette entrée ne peut etre créé: Decision Actuel [ {DemandeSelectionne?.Statut} ] ?");
                return;
            }

            try
            {

                Entree nouveau = Entree.Creer(DemandeSelectionne, MotifSelectionne, Description);
                if (Entree.Save(nouveau) == 1)
                {
                    Forma.SyncAllWithDB();

                    Forma.MsgInfo("Details Entree", $"Entree créé avec succes: [ {nouveau.Id} ] ?");
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
            if (EntreeSelectionne == null)
            {
                Message = $"Action impossible..";
                return;
            }
            MessageBoxResult msg = Forma.MsgValidation("Entrée", LesMessage.Q_Suppression);
            if (msg == MessageBoxResult.Yes)
            {
                Entree.Delete(EntreeSelectionne);
                Forma.SyncAllWithDB();

                Forma.MsgInfo("Entree", LesMessage.SuccesSuppression);
                this.CloseFenetre?.Invoke();
            }

        }
    }
}
