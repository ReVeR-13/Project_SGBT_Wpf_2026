using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    class DemandeViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Demande> _lesDemandes;

        public DemandeViewModel()
        {
            this._lesDemandes = [];
        }

        public ObservableCollection<Demande> LesDemandes { get => _lesDemandes; }

        public string Message => "Demande";
    }
}
