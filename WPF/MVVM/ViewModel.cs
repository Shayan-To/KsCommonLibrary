using System.Windows.Controls;
using System;

namespace Ks.Common.MVVM
{
    public abstract class ViewModel : BindableBase
    {
        public ViewModel(KsApplication KsApplication)
        {
            this.KsApplicationBase = KsApplication;
            this.Metadata = this.GetMetadata();
        }

        public ViewModel()
        {
            Verify.True(KsApplication.IsInDesignMode, "Test constructor should not be called from runtime.");

            this.Metadata = this.GetMetadata();
            if (this.Metadata.KsApplicationType != null)
                this.KsApplicationBase = (KsApplication) this.Metadata.KsApplicationType.CreateInstance();
        }

        private ViewModelMetadataAttribute GetMetadata()
        {
            var R = this.GetType().GetCustomAttribute<ViewModelMetadataAttribute>(false);
            Verify.False(R == null, "Every view-model that could be instantiated must have a ViewModelMetadata attribute set to it.");
            return R;
        }

        public event EventHandler<NavigationEventArgs> NavigatedTo;

        protected internal virtual void OnNavigatedTo(NavigationEventArgs E)
        {
            NavigatedTo?.Invoke(this, E);
        }

        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        protected internal virtual void OnNavigatedFrom(NavigationEventArgs E)
        {
            NavigatedFrom?.Invoke(this, E);
        }

        private Control _View = null;

        public Control View
        {
            get
            {
                if (this._View == null)
                {
                    this._View = (Control) this.Metadata.ViewType.CreateInstance();
                    this._View.DataContext = this;
                    Utils.SetViewModel(this._View, this);
                }
                return this._View;
            }
            internal set
            {
                Verify.True(this._View == null);
                this._View = value;
            }
        }

        public KsApplication KsApplicationBase { get; }

        private NavigationFrame _NavigationFrame;

        public NavigationFrame NavigationFrame
        {
            get
            {
                return this._NavigationFrame;
            }
            internal set
            {
                this.SetProperty(ref this._NavigationFrame, value);
            }
        }

        public ViewModelMetadataAttribute Metadata { get; }

        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }
    }
}
