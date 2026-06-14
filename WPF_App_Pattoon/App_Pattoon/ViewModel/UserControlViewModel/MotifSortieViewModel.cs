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

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    public class MotifSortieViewModel : BaseViewModel
    {
        private MotifSortie _motifSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;

        private ObservableCollection<MotifSortie> _lesMotifs;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        public MotifSortieViewModel()
        {
            _lesMotifs = new ObservableCollection<MotifSortie>(AllMotifsSortie.LesStocks.Values);
            _message = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;
            _description = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauMotifSortie(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerMotifSortie(), _ => this.MotifSelectionne != null);


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
        public MotifSortie MotifSelectionne
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

        public ObservableCollection<MotifSortie> LesMotifs
        {
            get { return _lesMotifs; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauMotifSortie()
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
                        MotifSortie nouveau = MotifSortie.Creer(Nom, Description);
                        if (MotifSortie.Save(nouveau) == 1)
                        {
                            AllMotifsSortie.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                            LesMotifs.Add(nouveau);
                        }

                    }
                }
                else
                {
                    if (this.MotifSelectionne.Update(Nom, Description) == 1)
                    {
                        AllMotifsSortie.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauMotifSortie();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerMotifSortie()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.MotifSelectionne != null)
            {
                try
                {
                    if (MotifSortie.Delete(this.MotifSelectionne) == 1)
                    {
                        AllMotifsSortie.DB_Sync();
                        LesMotifs.Remove(MotifSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauMotifSortie();
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
