using System;
using System.Windows.Input;

namespace Ks.Common.MVVM
{
    public class DelegateCommand : ICommand
    {
        public DelegateCommand(Action ExecuteFunc)
        {
            this._ExecuteFunc = O => ExecuteFunc.Invoke();
            this._CanExecuteFunc = null;
        }

        public DelegateCommand(Action ExecuteFunc, Func<bool> CanExecuteFunc)
        {
            this._ExecuteFunc = O => ExecuteFunc.Invoke();
            this._CanExecuteFunc = O => CanExecuteFunc.Invoke();
        }

        public DelegateCommand(Action<object> ExecuteFunc)
        {
            this._ExecuteFunc = ExecuteFunc;
            this._CanExecuteFunc = null;
        }

        public DelegateCommand(Action<object> ExecuteFunc, Func<object, bool> CanExecuteFunc)
        {
            this._ExecuteFunc = ExecuteFunc;
            this._CanExecuteFunc = CanExecuteFunc;
        }

#pragma warning disable CS0067 // The event is never used
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event is never used

        public void Execute(object parameter)
        {
            this._ExecuteFunc.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return this._CanExecuteFunc?.Invoke(parameter) ?? true;
        }

        private readonly Action<object> _ExecuteFunc;
        private readonly Func<object, bool> _CanExecuteFunc;
    }
}
