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
    public class AdoptionViewModel:BaseViewModel
    {
        private Adoption? _adoptionSelectionne;
        private readonly ObservableCollection<Adoption> _lesAdoptions;
        private readonly IWindowService _windowService;

        public ICommand NouveauCommand {  get; }
        public ICommand OuvrirDetailCommand { get; }

        public AdoptionViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _adoptionSelectionne =null;
            _lesAdoptions = new ObservableCollection<Adoption>(AllAdoption.ListAllAdoptions.Values);

            NouveauCommand = new RelayCommand(_=> NouvelAdoption() ,_=> true);
            OuvrirDetailCommand = new RelayCommand(_ => DetailsAdoption(), _ => this._adoptionSelectionne != null);
        }

        public ObservableCollection<Adoption> LesAdoptions
        {
            get { return _lesAdoptions; }
        }
        public Adoption? AdoptionSelectionne
        {
            get { return _adoptionSelectionne; }
            set { _adoptionSelectionne=value; OnPropertyChanged(); }
        }

        private void NouvelAdoption()
        {
            this._adoptionSelectionne = null;
            DetailsAdoption();
        }
        private void DetailsAdoption()
        {
            var vm = new AdoptionDetailViewModel(this.AdoptionSelectionne, this._adoptionSelectionne?.Demande,_windowService);
            _windowService.OuvrirDetail(vm);
        }


    }
}
