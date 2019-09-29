using System.Windows;
using System;
using System.Windows.Input;

namespace Ks
{
    namespace Common.MVVM
    {
        public class NavigationCommand : DependencyObject, ICommand
        {
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
                    return (NavigationViewModel)this.GetKsApplication().GetViewModel(T2);
                return this.GetKsApplication().DefaultNavigationView;
            }

            private KsApplication GetKsApplication()
            {
                return this.KsApplication ?? KsApplication.Current;
            }

            public static readonly DependencyProperty AddToStackProperty = DependencyProperty.Register("AddToStack", typeof(bool), typeof(NavigationCommand), new PropertyMetadata(true));

            public bool AddToStack
            {
                get
                {
                    return (bool)this.GetValue(AddToStackProperty);
                }
                set
                {
                    this.SetValue(AddToStackProperty, value);
                }
            }

            public static readonly DependencyProperty ForceToStackProperty = DependencyProperty.Register("ForceToStack", typeof(bool), typeof(NavigationCommand), new PropertyMetadata(false));

            public bool ForceToStack
            {
                get
                {
                    return (bool)this.GetValue(ForceToStackProperty);
                }
                set
                {
                    this.SetValue(ForceToStackProperty, value);
                }
            }

            public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.Register("ViewType", typeof(Type), typeof(NavigationCommand), new PropertyMetadata(null, null, ViewType_Coerce));

            private static object ViewType_Coerce(DependencyObject D, object BaseValue)
            {
                var Value = BaseValue as Type;

                if (Value == null || !typeof(ViewModel).IsAssignableFrom(Value))
                    return null;

                return BaseValue;
            }

            public Type ViewType
            {
                get
                {
                    return (Type)this.GetValue(ViewTypeProperty);
                }
                set
                {
                    this.SetValue(ViewTypeProperty, value);
                }
            }

            public static readonly DependencyProperty ParentTypeProperty = DependencyProperty.Register("ParentType", typeof(Type), typeof(NavigationCommand), new PropertyMetadata(null, null, ParentType_Coerce));

            private static object ParentType_Coerce(DependencyObject D, object BaseValue)
            {
                var Self = (NavigationCommand)D;

                if (Self.Parent != null)
                    return null;

                var Value = BaseValue as Type;
                if (Value == null || !typeof(NavigationViewModel).IsAssignableFrom(Value))
                    return null;

                return BaseValue;
            }

            public Type ParentType
            {
                get
                {
                    return (Type)this.GetValue(ParentTypeProperty);
                }
                set
                {
                    this.SetValue(ParentTypeProperty, value);
                }
            }

            public static readonly DependencyProperty ParentProperty = DependencyProperty.Register("Parent", typeof(NavigationViewModel), typeof(NavigationCommand), new PropertyMetadata(null, null, Parent_Coerce));

            private static object Parent_Coerce(DependencyObject D, object BaseValue)
            {
                var Self = (NavigationCommand)D;

                if (Self.ParentType != null)
                    return null;

                return BaseValue;
            }

            public NavigationViewModel Parent
            {
                get
                {
                    return (NavigationViewModel)this.GetValue(ParentProperty);
                }
                set
                {
                    this.SetValue(ParentProperty, value);
                }
            }

            public static readonly DependencyProperty KsApplicationProperty = DependencyProperty.Register("KsApplication", typeof(KsApplication), typeof(NavigationCommand), new PropertyMetadata(null));

            public KsApplication KsApplication
            {
                get
                {
                    return (KsApplication)this.GetValue(KsApplicationProperty);
                }
                set
                {
                    this.SetValue(KsApplicationProperty, value);
                }
            }
        }
    }
}
