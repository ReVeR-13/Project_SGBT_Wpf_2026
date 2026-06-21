using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    public class HistoriqueAnimalViewModel:BaseViewModel
    {
        private Animal _animal;
        private string _title;
        private string _title2;
        private ObservableCollection<object> _historiqueSources;

        public ICommand DemandeCommand { get; }
        public ICommand EntreeCommand { get; }
        public ICommand SortieCommand { get; }
        public ICommand AdoptionCommand { get; }
        public ICommand AccueilCommand { get; }
        public HistoriqueAnimalViewModel(Animal animal)
        {
            _animal = animal;
            _title = $"HISTORIQUE DE [ {animal.Nom} ]";
            _title2 = "Demandes";
            HistoriqueSources = new ObservableCollection<object>(animal.GetDemandes());

            DemandeCommand = new RelayCommand(_ => HistoriqueDemande(), _ => AnimalSelectionne != null);
            EntreeCommand = new RelayCommand(_ => HistoriqueEntree(), _ => true);
            SortieCommand = new RelayCommand(_ => HistoriqueSortie(), _ => true);
            AdoptionCommand = new RelayCommand(_ => HistoriqueAdoption(), _ => true);
            AccueilCommand = new RelayCommand(_ => HistoriqueAccueil(), _ => true);
        }

        public Animal AnimalSelectionne
        {
            get { return _animal; }
            set { _animal = value; OnPropertyChanged(); }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string Title2
        {
            get { return _title2; }
            set { _title2 = value; OnPropertyChanged(); }
        }

        public ObservableCollection<object> HistoriqueSources
        {
            get { return _historiqueSources; }
            set { _historiqueSources = value; OnPropertyChanged(); }
        }

        public void HistoriqueDemande()
        {
            HistoriqueSources = new ObservableCollection<object>(AnimalSelectionne.GetDemandes());
            Title2 = "Demandes";
        }

        public void HistoriqueEntree()
        {
            HistoriqueSources = new ObservableCollection<object>(AnimalSelectionne.GetEntree());
            Title2 = "Entrees";
        }

        public void HistoriqueSortie()
        {
            HistoriqueSources = new ObservableCollection<object>(AnimalSelectionne.GetSortie());
            Title2 = "Sorties";
        }

        public void HistoriqueAdoption()
        {
            HistoriqueSources = new ObservableCollection<object>(AnimalSelectionne.GetAdoption());
            Title2 = "Adoptions";
        }

        public void HistoriqueAccueil()
        {
            HistoriqueSources = new ObservableCollection<object>(AnimalSelectionne.GetAccueil());
            Title2 = "Accueils";
        }
    }
}
