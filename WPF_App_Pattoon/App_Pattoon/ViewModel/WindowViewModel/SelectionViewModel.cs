using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.WindowViewModel
{
    public class SelectionViewModel<T> :BaseViewModel
    {
        private ObservableCollection<T> _items;
        public T SelectedItem { get; set; }

        public ICommand ConfirmCommand { get;}

        public event EventHandler<T> CloseRequested;

        public SelectionViewModel(IEnumerable<T> items)
        {
            _items = new ObservableCollection<T>(items);
            ConfirmCommand = new RelayCommand(_=> CloseRequested?.Invoke(this,SelectedItem),_=> SelectedItem != null);
        }

        public ObservableCollection<T> Items 
        {
            get { return _items; }
        }
    }
}
