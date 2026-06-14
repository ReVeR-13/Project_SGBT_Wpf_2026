using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    class AnimalViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Demande> _lesAnimaux;

        public AnimalViewModel()
        {
            this._lesAnimaux = [];
        }

        public ObservableCollection<Demande> LesAnimaux { get => _lesAnimaux; }

        public string Message => "Animal";
    }
}
