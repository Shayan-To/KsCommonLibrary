using System;
using System.Windows.Input;

namespace Ks.Common.MVVM
{
    public class Navigation : System.Windows.Markup.MarkupExtension, ICommand
    {
        public Navigation()
        {
        }

        public Navigation(Type ViewType)
        {
            this.ViewType = ViewType;
        }

#pragma warning disable CS0067 // The event is never used
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event is never used

        public void Execute(object parameter)
        {
            var KsApplication = this.GetKsApplication();
            KsApplication.NavigateTo(this.GetParent(), KsApplication.GetViewModel(this.ViewType), this.AddToStack, this.ForceToStack);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private NavigationViewModel GetParent()
        {
            var T = this.Parent;
            if (T != null)
                return T;
            var T2 = this.ParentType;
            if (T2 != null)
                return (NavigationViewModel) this.GetKsApplication().GetViewModel(T2);
            return this.GetKsApplication().DefaultNavigationView;
        }

        private KsApplication GetKsApplication()
        {
            return this.KsApplication ?? KsApplication.Current;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public bool AddToStack { get; set; } = true;

        public bool ForceToStack { get; set; } = false;

        private Type _ViewType;

        public Type ViewType
        {
            get
            {
                return this._ViewType;
            }
            set
            {
                if (value == null || !typeof(ViewModel).IsAssignableFrom(value))
                    value = null;

                this._ViewType = value;
            }
        }

        private Type _ParentType;

        public Type ParentType
        {
            get
            {
                return this._ParentType;
            }
            set
            {
                if (this.Parent != null)
                    value = null;

                if (value == null || !typeof(NavigationViewModel).IsAssignableFrom(value))
                    value = null;

                this._ParentType = value;
            }
        }

        private NavigationViewModel _Parent;

        public NavigationViewModel Parent
        {
            get
            {
                return this._Parent;
            }
            set
            {
                if (this.ParentType != null)
                    value = null;

                this._Parent = value;
            }
        }

        public KsApplication KsApplication { get; set; }
    }
}
