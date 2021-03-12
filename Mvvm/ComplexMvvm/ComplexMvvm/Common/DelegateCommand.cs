using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComplexMvvm.Common
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteAction == null)
            {
                return true;
            }
            return this.CanExecuteAction(parameter);
        }
        public void Execute(object parameter)
        {
            if (this.ExecuteAction == null)
            {
                return;
            }
            this.ExecuteAction(parameter);
        }

        public Action<Object> ExecuteAction { get; set; }
        public Func<Object, bool> CanExecuteAction { get; set; }
    }
}
