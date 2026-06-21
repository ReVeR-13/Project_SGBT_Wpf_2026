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
    class EntreeViewModel : BaseViewModel
    {
        private Entree _entreeSelectionne;
        private string _id;
        private string _demandeId;
        private string _motifId;
        private string _description;
        private string _message;

        private ObservableCollection<Entree> _lesEntrees;

        private IWindowService _windowService;
        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public ICommand OuvrirDetailCommand { get; }

        public EntreeViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _lesEntrees = new ObservableCollection<Entree>(AllEntree.LesStocks.Values);
            _message = string.Empty;
            _id = string.Empty;
            _demandeId = string.Empty;
            _description = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauEntree(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerEntree(), _ => this.EntreeSelectionne != null);
            OuvrirDetailCommand = new RelayCommand(_ => OuvrirDetail(), _ => this.EntreeSelectionne != null);

        }

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
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
        public Entree EntreeSelectionne
        {
            get { return _entreeSelectionne; }
            set
            {
                _entreeSelectionne = value;
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

        public ObservableCollection<Entree> LesEntrees
        {
            get { return _lesEntrees; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauEntree()
        {
            EntreeSelectionne = null;
            DemandeId = string.Empty;
            MotifId = string.Empty;
            Description = string.Empty;
            Id = string.Empty;
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(DemandeId)
                && !string.IsNullOrWhiteSpace(Description);
        }
        private void AjouteOuMettreAJour()
        {
            this.Message = LesMessage.ErrorAjout;

            try
            {
                if (this.EntreeSelectionne == null)
                {

                    if (!string.IsNullOrEmpty(DemandeId) && !string.IsNullOrEmpty(MotifId))
                    {
                        Demande? demande = AllDemande.Find(DemandeId);
                        MotifEntree motif = AllMotifsEntrees.FindById(MotifId);
                        if (demande != null && motif != null)
                        {
                            Entree nouveau = Entree.Creer(demande, motif, Description);
                            if (Entree.Save(nouveau) == 1)
                            {
                                AllEntree.DB_Sync();
                                this.Message = LesMessage.SuccesAjout;
                                LesEntrees.Add(nouveau);
                            }
                        }

                    }
                }
                else
                {
                    MotifEntree motif = AllMotifsEntrees.FindById(MotifId);
                    if (motif != null)
                    {
                        if (this.EntreeSelectionne.Update(motif, Description) == 1)
                        {
                            AllEntree.DB_Sync();
                            this.Message = LesMessage.SuccesMaj;
                        }
                    }

                }
                NouveauEntree();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void OuvrirDetail()
        {
            var vm = new EntreeDetailViewModel(EntreeSelectionne.Demande);
            _windowService.OuvrirDetail(vm);

        }
        private void SupprimerEntree()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.EntreeSelectionne != null)
            {
                try
                {
                    if (Entree.Delete(this.EntreeSelectionne) == 1)
                    {
                        AllEntree.DB_Sync();
                        LesEntrees.Remove(EntreeSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauEntree();
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
