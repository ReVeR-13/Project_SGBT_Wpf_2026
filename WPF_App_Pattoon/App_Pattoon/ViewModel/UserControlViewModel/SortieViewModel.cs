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

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    class SortieViewModel : BaseViewModel
    {
        private Sortie _sortieSelectionne;
        private Demande? _demande;
        private string _id;
        private string _demandeId;
        private string _motifId;
        private string _description;
        private string _message;

        private static ObservableCollection<Sortie> _lesSorties;
        private IWindowService _windowService;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        public ICommand OuvrirDetailCommand { get; }

        public SortieViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            _lesSorties = new ObservableCollection<Sortie>(AllSortie.LesStocks.Values);
            _demande = null;
            _message = string.Empty;
            _id = string.Empty;
            _demandeId = string.Empty;
            _description = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauSortie(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerSortie(), _ => this.SortieSelectionne != null);
            OuvrirDetailCommand = new RelayCommand(_ => OuvrirDetail(), _ => this.SortieSelectionne != null);
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public Demande? DemandeSelectionne
        {
            get 
            { return _demande; }
            set { _demande = value; OnPropertyChanged(); }
        }
        public string DemandeId
        {
            get { return _demandeId; }
            set { _demandeId = value; OnPropertyChanged(); }
        }
        public string MotifId
        {
            get { return _motifId; }
            set { _motifId = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public Sortie SortieSelectionne
        {
            get { return _sortieSelectionne; }
            set
            {
                _sortieSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    DemandeId = value.Demande.Id;
                    MotifId = value.Motifs.Id;
                    Description = value.Details;
                    Message = string.Empty;
                }
            }
        }

        public static ObservableCollection<Sortie> LesSorties
        {
            get { return _lesSorties; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauSortie()
        {
            SortieSelectionne = null;
            DemandeId = string.Empty;
            MotifId = string.Empty;
            Description = string.Empty;
            Id = string.Empty;
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(DemandeId)
                && !string.IsNullOrWhiteSpace(MotifId);
        }
        private void AjouteOuMettreAJour()
        {
            this.Message = LesMessage.ErrorAjout;

            try
            {
                if (this.SortieSelectionne == null)
                {

                    if (!string.IsNullOrEmpty(DemandeId) && !string.IsNullOrEmpty(MotifId))
                    {
                        Demande? demande = AllDemande.Find(DemandeId);
                        MotifSortie? motif = AllMotifsSortie.FindById(MotifId);
                        if (demande != null && motif != null)
                        {
                            Sortie nouveau = Sortie.Creer(demande, motif, Description);
                            if (Sortie.Save(nouveau) == 1)
                            {
                                Forma.SyncAllWithDB();
                                this.Message = LesMessage.SuccesAjout;
                                LesSorties.Add(nouveau);
                            }
                        }

                    }
                }
                else
                {
                    MotifSortie? motif = AllMotifsSortie.FindById(MotifId);
                    if (motif != null)
                    {
                        if (this.SortieSelectionne.Update(motif, Description) == 1)
                        {
                            Forma.SyncAllWithDB();
                            this.Message = LesMessage.SuccesMaj;
                        }
                    }

                }
                NouveauSortie();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }

        private void OuvrirDetail()
        {
            var vm = new SortieDetailViewModel(SortieSelectionne.Demande);
            _windowService.OuvrirDetail(vm);

        }
        private void SupprimerSortie()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.SortieSelectionne != null)
            {
                try
                {
                    if (Sortie.Delete(this.SortieSelectionne) == 1)
                    {
                        AllSortie.DB_Sync();
                        LesSorties.Remove(SortieSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauSortie();
                    }
                }
                catch (Exception ex)
                {
                    this.Message = "[Erreur] : " + ex.Message;
                }

            }
        }

    }
}
