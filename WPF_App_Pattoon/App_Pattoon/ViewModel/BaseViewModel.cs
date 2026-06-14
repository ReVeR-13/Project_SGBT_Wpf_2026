using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.View.WindowView;
using Wpf_App_Pattoon_Animalerie.ViewModel.WindowViewModel;

namespace Wpf_App_Pattoon_Animalerie.ViewModel
{
    public interface IWindowService
    {
        void OuvrirDetail(object viewModel);
        T OuvrirPopup<T>(SelectionViewModel<T> viewModel);
    }

    public class BaseViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }

    public class WindowService : IWindowService
    {
        public void OuvrirDetail(object viewModel)
        {
            var windows = new DetailView();
            windows.DataContext = viewModel;
            windows.ShowDialog();
        }

        public T OuvrirPopup<T>(SelectionViewModel<T> viewModel)
        {
            var window = new PopupView();
            window.DataContext = viewModel;

            T? result = default;

            viewModel.CloseRequested += (s, item) =>
            {
                result = item;
                window.Close();
            };

            window.ShowDialog();
            return result;
        }
    }
}
