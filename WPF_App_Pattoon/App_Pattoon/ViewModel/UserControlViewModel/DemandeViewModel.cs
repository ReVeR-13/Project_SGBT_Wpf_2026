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
    class DemandeViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Demande> _lesDemandes;
        private Demande? _demandeSelectionne;
        private string _message;

        private readonly IWindowService _windowService;
        public ICommand NouveauCommand { get; }
        public ICommand EntreeCommand { get; }
        public ICommand SortieCommand { get; }
        public ICommand AccueilCommand { get; }
        public ICommand AdoptionCommand { get; }

        public ICommand OuvrirDetailCommand { get; }

        public DemandeViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            this._lesDemandes = new ObservableCollection<Demande>(AllDemande.ListeAllDemande.Values);
            this._demandeSelectionne = null;
            this._message = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouvelDemande(this._demandeSelectionne), _ => true);
            OuvrirDetailCommand = new RelayCommand(_ => NouvelDemande(this._demandeSelectionne),_=> this._demandeSelectionne != null);
        }

        public ObservableCollection<Demande> LesDemandes { get => _lesDemandes; }
        public Demande? DemandeSelectionne
        {
            get { return _demandeSelectionne; }
            set { _demandeSelectionne = value; OnPropertyChanged(); }

        }

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public void NouvelDemande(Demande? demande)
        {
            var vm = new DemandeDetailViewModel(demande,null,null, _windowService);
            _windowService.OuvrirDetail(vm);
        }
    }
}
