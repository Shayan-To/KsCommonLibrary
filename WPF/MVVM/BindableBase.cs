using System.ComponentModel;

namespace Ks
{
    namespace Common.MVVM
    {
        public class BindableBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected bool SetProperty<T>(ref T Source, T Value, [System.Runtime.CompilerServices.CallerMemberName()] string PropertyName = null)
            {
                if (!object.Equals(Source, Value))
                {
                    Source = Value;
                    this.NotifyPropertyChanged(PropertyName);
                    return true;
                }

                return false;
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs E)
            {
                PropertyChanged?.Invoke(this, E);
            }

            protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName()] string PropertyName = null)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
