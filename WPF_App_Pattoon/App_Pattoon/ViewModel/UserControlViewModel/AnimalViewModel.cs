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
    class AnimalViewModel : BaseViewModel
    {
        private ObservableCollection<Animal> _lesAnimaux;
        private Animal? _animalSelectionne;
        IWindowService _windowService;

        public ICommand OuvrirDetailCommand { get; }
        public ICommand NouveauCommand { get; }

        public AnimalViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _animalSelectionne = null;
            this._lesAnimaux = new ObservableCollection<Animal>(AllAnimal.ListeAllAnimal.Values);

            OuvrirDetailCommand = new RelayCommand(_ => OuvrirDetail(), _ => AnimalSelectionne != null);
            NouveauCommand = new RelayCommand(_ => OuvrirDetail(), _ => true);
        }

        public ObservableCollection<Animal> LesAnimaux { get => _lesAnimaux; }
        public Animal? AnimalSelectionne
        {
            get => _animalSelectionne;
            set { _animalSelectionne = value; OnPropertyChanged(); }
        }

        public string Message => "Animal";

        private void OuvrirDetail()
        {
            var vm = new AnimalDetailViewModel(AnimalSelectionne,_windowService);
            _windowService.OuvrirDetail(vm);

        }
    }
}
