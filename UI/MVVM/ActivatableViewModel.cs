using System;
using System.Collections.Generic;
using System.Text;

using ReactiveUI;

namespace Ks.Common.MVVM
{
    public class ActivatableViewModel : ReactiveObject, IActivatableViewModel
    {
        ViewModelActivator IActivatableViewModel.Activator { get; } = new();
    }
}
