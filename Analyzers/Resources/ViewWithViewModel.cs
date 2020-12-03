using System;
using System.Reactive.Linq;

using Avalonia;

using ReactiveUI;

//[[_
namespace _
{
    class _
        //#[[ClassExtras
        : IViewFor</*{*/ViewModelType/*}*/>
        //#]]ClassExtras
    {
//]]_
//#!NamespaceClassStart
        public static readonly StyledProperty</*{*/ViewModelType/*}*/?> ViewModelProperty =
            AvaloniaProperty.Register</*{*/ClassName/*}*/, /*{*/ViewModelType/*}*/?>(nameof(ViewModel));

        private void InitializeViewModelProperty()
        {
            // This WhenActivated block calls ViewModel's WhenActivated
            // block if the ViewModel implements IActivatableViewModel.
            this.WhenActivated(disposables => { });

            this.ObservableForProperty(x => x.ViewModel)
                .Subscribe(p => this.DataContext = p.Value);
            this.ObservableForProperty(x => x.DataContext)
                .Subscribe(p => this.ViewModel = (/*{*/ViewModelType/*}*/?) p.Value);
        }

        public /*{*/ViewModelType/*}*/? ViewModel
        {
            get => this.GetValue(ViewModelProperty);
            set => this.SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => this.ViewModel;
            set => this.ViewModel = (/*{*/ViewModelType/*}*/?) value;
        }

//#!NamespaceClassEnd
//[[_
    }
}
//]]_
