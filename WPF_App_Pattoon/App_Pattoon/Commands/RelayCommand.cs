
using System.Windows.Input;

namespace Wpf_App_Pattoon_Animalerie.Commands
{
    public class RelayCommand: ICommand
    {

        private Action<object> _execute;
        private Func<object, bool> _canExecute;
        public RelayCommand(Action<object> execute, Func<object,bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) 
            => _canExecute==null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value;}
            remove { CommandManager.RequerySuggested -= value;}
        }

    }
}
