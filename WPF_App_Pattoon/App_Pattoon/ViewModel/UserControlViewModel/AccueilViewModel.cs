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
    public class AccueilViewModel: BaseViewModel
    {
        private Accueil? _accueilSelectionne;
        private readonly ObservableCollection<Accueil> _lesAccueils;
        private readonly IWindowService _windowService;

        public ICommand NouveauCommand { get; }
        public ICommand OuvrirDetailCommand { get; }

        public AccueilViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _accueilSelectionne = null;
            _lesAccueils = new ObservableCollection<Accueil>(AllAccueil.Get());

            NouveauCommand = new RelayCommand(_ => NouvelAccueil(), _ => true);
            OuvrirDetailCommand = new RelayCommand(_ => DetailsAccueil(), _ => this._accueilSelectionne != null);
        }

        public ObservableCollection<Accueil> LesAccueils
        {
            get { return _lesAccueils; }
        }
        public Accueil? AccueilSelectionne
        {
            get { return _accueilSelectionne; }
            set { _accueilSelectionne = value; OnPropertyChanged(); }
        }

        private void NouvelAccueil()
        {
            this._accueilSelectionne = null;
            DetailsAccueil();
        }
        private void DetailsAccueil()
        {
            var vm = new AccueilDetailViewModel(this.AccueilSelectionne, this._accueilSelectionne?.Demande, _windowService);
            _windowService.OuvrirDetail(vm);
        }
    }
}
