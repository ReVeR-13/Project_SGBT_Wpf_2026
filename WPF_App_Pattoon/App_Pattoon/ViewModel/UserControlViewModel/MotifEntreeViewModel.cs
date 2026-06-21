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
    public class MotifEntreeViewModel : BaseViewModel
    {
        IWindowService _windowService;
        private MotifEntree _motifSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;

        private ObservableCollection<MotifEntree> _lesMotifs;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public ICommand OuvrirDetailCommand { get; }

        public MotifEntreeViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _lesMotifs = new ObservableCollection<MotifEntree>(AllMotifsEntrees.LesStocks.Values);
            _message = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;
            _description = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauMotifEntree(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerMotifEntree(), _ => this.MotifSelectionne != null);
            OuvrirDetailCommand = new RelayCommand(_ => OuvrirDetail(), _ => this.MotifSelectionne != null);

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
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public MotifEntree MotifSelectionne
        {
            get { return _motifSelectionne; }
            set
            {
                _motifSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    Nom = value.Libele;
                    Description = value.Details;
                    Message = string.Empty;
                }
            }
        }

        public ObservableCollection<MotifEntree> LesMotifs
        {
            get { return _lesMotifs; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauMotifEntree()
        {
            MotifSelectionne = null;
            Nom = string.Empty;
            Description = string.Empty;
            Id = string.Empty;
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(Nom)
                && !string.IsNullOrWhiteSpace(Description);
        }
        private void AjouteOuMettreAJour()
        {
            this.Message = LesMessage.ErrorAjout;

            try
            {
                if (this.MotifSelectionne == null)
                {
                    if (Nom != null)
                    {
                        MotifEntree nouveau = MotifEntree.Creer(Nom, Description);
                        if (MotifEntree.Save(nouveau) == 1)
                        {
                            AllMotifsEntrees.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                            LesMotifs.Add(nouveau);
                        }

                    }
                }
                else
                {
                    if (this.MotifSelectionne.Update(Nom, Description) == 1)
                    {
                        AllMotifsEntrees.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauMotifEntree();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerMotifEntree()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.MotifSelectionne != null)
            {
                try
                {
                    if (MotifEntree.Delete(this.MotifSelectionne) == 1)
                    {
                        AllMotifsEntrees.DB_Sync();
                        LesMotifs.Remove(MotifSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauMotifEntree();
                    }
                }
                catch (Exception ex)
                {
                    this.Message = "[Erreur] : " + ex.Message;
                }

            }
        }
        private void OuvrirDetail()
        {
            var vm = new MotifEntreeDetailViewModel(MotifSelectionne);
            _windowService.OuvrirDetail(vm);

        }
    }
}
