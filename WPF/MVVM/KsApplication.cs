using System.Windows;
using System.Threading.Tasks;
using Mono;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using Ks.Common.Controls;

namespace Ks.Common.MVVM
{
    public abstract class KsApplication : BindableBase
    {

        // ToDo OnShuttingDown is only called when the ShutDown method is called, not when the application is shutting down.

        public KsApplication(string Name, Application Application)
        {
            this.NavigateBackCommand = new DelegateCommand(this.NavigateBack);

            this.Name = Name;
            this.Application = Application;

            this.Window = new WindowViewModel(this);
            this.EmptyNavigationFrame = new NavigationFrame(new[] { this.Window });
            this.Window.NavigationFrame = this.EmptyNavigationFrame;
            this.CurrentNavigationFrame = this.EmptyNavigationFrame;

            this.State = KsApplicationState.NotStarted;
        }

        public KsApplication()
        {
            Verify.True(KsApplication.IsInDesignMode, "Test constructor should not be called from runtime.");

            var TestData = this.OnTestConstruct();
            this.Name = TestData.Name;
            this.Settings = TestData.Settings;
        }

        protected virtual void OnInitialize()
        {
            this.Settings = new AutoStoreDictionary(System.IO.Path.Combine(".", "Settings.dat"));

            var Dir = System.IO.Path.Combine(".", "Languages");
            if (!System.IO.Directory.Exists(Dir))
            {
                System.IO.Directory.CreateDirectory(Dir);
            }

            var Languages = new OneToOneDictionary<string, KsLanguage>(new Dictionary<string, KsLanguage>(StringComparer.InvariantCultureIgnoreCase), L => L.Id);

            foreach (var F in System.IO.Directory.EnumerateFiles(Dir))
            {
                if (!F.ToLowerInvariant().EndsWith(".csv"))
                {
                    continue;
                }

                var File = System.IO.File.Open(F, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read);
                Languages.Add(new KsLanguage(File));
            }

            if (Languages.Count == 0)
            {
                var F = System.IO.Path.Combine(Dir, "Default.csv");
                var File = System.IO.File.Open(F, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read);
                Languages.Add(new KsLanguage(File));
            }

            this.Languages = Languages.AsReadOnly();

            if (!this.Settings.TryGetValue(nameof(this.Language), out var LangId))
            {
                LangId = "";
            }

            if (!this.Languages.TryGetValue(LangId, out this._Language))
            {
                this._Language = this.Languages.FirstOrDefault().Value;
                this.Settings[nameof(this.Language)] = this._Language?.Id;
            }
        }

        protected virtual KsApplicationTestData OnTestConstruct()
        {
            return new KsApplicationTestData() { Name = "", Settings = new Dictionary<string, string>() };
        }

        protected virtual void OnStart()
        {
            this.NavigateTo(this.DefaultNavigationFrame);
        }

        protected virtual void OnNavigated(NavigationFrame OldFrame, NavigationFrame NewFrame)
        {
        }

        protected virtual void OnShuttingDown()
        {
        }

        protected virtual void OnShutDown()
        {
            (this.Settings as IDisposable)?.Dispose();
            foreach (var L in this.Languages.Values)
            {
                L.Dispose();
            }
        }

        [DebuggerHidden()]
        public void Run()
        {
            if (this.State != KsApplicationState.NotStarted)
            {
                throw new InvalidOperationException("Cannot run an already run KsApplication.");
            }

            Current = this;

            this.State = KsApplicationState.Initializing;
            this.OnInitialize();

            this.Application.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.Application.MainWindow = null;
            // this.Application.StartupUri = null;

            var Window = (Controls.Window) this.Window.View;
            Window.Resources.MergedDictionaries.Add(this.Application.Resources);

            this.Application.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.OnStart();
                this.State = KsApplicationState.Started;
            }));

            this.Application.Run(Window);

            this.State = KsApplicationState.ShutDown;
            this.OnShutDown();
        }

        public void ShutDown(int ExitCode = 0)
        {
            Verify.True(this.State == KsApplicationState.Started, "The KsApplication has to be started to be able to be shut down.");

            if (Current == this)
            {
                Current = null;
            }

            this.State = KsApplicationState.ShuttingDown;
            this.OnShuttingDown();

            this.Application.Shutdown(ExitCode);
        }

        protected virtual bool CanNavigateBackToEmpty()
        {
            return false;
        }

        protected virtual NavigationData OnNavigateToEmpty()
        {
            return default;
        }

        public TViewModel GetViewModel<TViewModel>() where TViewModel : ViewModel
        {
            return (TViewModel) this.GetViewModel(typeof(TViewModel));
        }

        public ViewModel GetViewModel(Type ViewModelType)
        {
            var Metadata = ViewModelType.GetCustomAttribute<ViewModelMetadataAttribute>();
            Verify.False(Metadata == null, "No metadata available.");

            if (Metadata.IsSingleInstance)
            {
                if (this.SingleInstanceViewModels.TryGetValue(ViewModelType, out var T))
                {
                    return T;
                }
            }

            var ApplicationType = this.GetType();
            System.Reflection.ConstructorInfo Constructor = null;

            foreach (var C in ViewModelType.GetConstructors())
            {
                var Ps = C.GetParameters();
                if (Ps.Length == 1 && Ps[0].ParameterType.IsAssignableFrom(ApplicationType))
                {
                    Constructor = C;
                    break;
                }
            }

            Verify.False(Constructor == null, "No valid constructor available. Every view-model has to have a contructor getting one single argument, the KsApplication of that WPF application.");

            var R = (ViewModel) Constructor.Invoke(new[] { this });

            if (Metadata.IsSingleInstance)
            {
                this.SingleInstanceViewModels[ViewModelType] = R;
            }

            return R;
        }

        internal void NavigateTo(NavigationViewModel Parent, ViewModel ViewModel, bool AddToStack = true, bool ForceToStack = false)
        {
            Verify.False(Parent.NavigationFrame == null, "Cannot navigate. The navigation parent is not in the view.");

            this.NavigateTo(Parent.NavigationFrame.AddViewModel(ViewModel), AddToStack, ForceToStack);
        }

        public void NavigateTo(NavigationFrame Frame, bool AddToStack = true, bool ForceToStack = false)
        {
            var Tip = Frame.Tip;

            if (Tip.NavigationFrame != null)
            {
                this.NavigationFrames.Remove(Tip.NavigationFrame);
            }

            if (ForceToStack | (AddToStack & !Frame.IsOpenEnded))
            {
                this.NavigationFrames.Push(Frame);
            }

            this.DoNavigation(Frame, NavigationType.NewNavigation);
        }

        public void NavigateBack()
        {
            // See UpdateCanNavigateBack.
            if (!this.CanNavigateBack)
            {
                return;
            }

            if (this.NavigationFrames.PeekOrDefault() == this.CurrentNavigationFrame)
            {
                this.NavigationFrames.Pop();
            }

            if (!this.NavigationFrames.CanPop())
            {
                var Data = this.OnNavigateToEmpty();
                this.NavigateTo(Data.Frame, Data.AddToStack, Data.ForceToStack);
                return;
            }

            this.DoNavigation(this.NavigationFrames.Peek(), NavigationType.BackNavigation);
        }

        private void DoNavigation(NavigationFrame NewFrame, NavigationType NavigationType)
        {
            var OldFrame = this.CurrentNavigationFrame;
            this.CurrentNavigationFrame = NewFrame;

            this.UpdateCanNavigateBack();

            var I = 0;
            var count = Math.Min(NewFrame.Count, OldFrame.Count);
            for (; I < count; I++)
            {
                if (NewFrame[I] != OldFrame[I])
                {
                    break;
                }
            }

            var NavigationEventArgs = new NavigationEventArgs(NavigationType);
            for (var J = OldFrame.Count - 1; J >= I; J--)
            {
                var VM = OldFrame[J];

                VM.NavigationFrame = null;
                if (VM.IsNavigation())
                {
                    VM.SetView(null);
                }

                VM.OnNavigatedFrom(NavigationEventArgs);
            }

            for (var J = I; J <= NewFrame.Count; J++)
            {
                var Parent = NewFrame[J - 1] as NavigationViewModel;
                var Current = (J != NewFrame.Count) ? NewFrame[J] : null;

                if (Current != null)
                {
                    Current.NavigationFrame = NewFrame.SubFrame(J + 1);
                }

                Parent?.SetView(Current);
                Current?.OnNavigatedTo(NavigationEventArgs);
            }

            this.OnNavigated(OldFrame, NewFrame);
        }

        private void UpdateCanNavigateBack()
        {
            var Count = this.NavigationFrames.Count;

            // If the top of stack is not the current frame, we can go back from the current frame to it.
            Count += (this.NavigationFrames.PeekOrDefault() == this.CurrentNavigationFrame) ? 0 : 1;

            switch (Count)
            {
                case 0:
                    this.CanNavigateBack = false;
                    break;
                case 1:
                    this.CanNavigateBack = this.CanNavigateBackToEmpty();
                    break;
                default:
                    this.CanNavigateBack = true;
                    break;
            }
        }

        public DelegateCommand NavigateBackCommand { get; }

        private bool _CanNavigateBack;

        public bool CanNavigateBack
        {
            get
            {
                return this._CanNavigateBack;
            }
            set
            {
                this.SetProperty(ref this._CanNavigateBack, value);
            }
        }

        public virtual NavigationViewModel DefaultNavigationView
        {
            get
            {
                return this.Window;
            }
        }

        private NavigationFrame _DefaultNavigationFrame;

        public NavigationFrame DefaultNavigationFrame
        {
            get
            {
                if (this.DefaultNavigationView == this.Window)
                {
                    return this.EmptyNavigationFrame;
                }

                if (this._DefaultNavigationFrame?.Tip != this.DefaultNavigationView)
                {
                    this._DefaultNavigationFrame = this.EmptyNavigationFrame.AddViewModel(this.DefaultNavigationView);
                }

                return this._DefaultNavigationFrame;
            }
        }

        private NavigationFrame _CurrentNavigationFrame;

        public NavigationFrame CurrentNavigationFrame
        {
            get
            {
                return this._CurrentNavigationFrame;
            }
            private set
            {
                this.SetProperty(ref this._CurrentNavigationFrame, value);
            }
        }

        public PushPopList<NavigationFrame> NavigationFrames { get; } = new PushPopList<NavigationFrame>();

        public NavigationFrame EmptyNavigationFrame { get; }

        private readonly Dictionary<Type, ViewModel> SingleInstanceViewModels = new Dictionary<Type, ViewModel>();

        private static DependencyObject DepObj;

        public static bool IsInDesignMode
        {
            get
            {
#if SimulateDesign
                    return true;
#else
                if (DepObj == null)
                {
                    DepObj = new DependencyObject();
                }
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(DepObj);
#endif
            }
        }

        public string Name { get; }

#pragma warning disable CS0169 // The field is never used
        private readonly string _Version; // ToDo
#pragma warning restore CS0169 // The field is never used

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        public NavigationViewModel Window { get; }

        public Type HomePageType { get; }

        private KsApplicationState _State;

        public KsApplicationState State
        {
            get
            {
                return this._State;
            }
            private set
            {
                this.SetProperty(ref this._State, value);
            }
        }

        public IReadOnlyDictionary<string, KsLanguage> Languages { get; private set; }

        private KsLanguage _Language;

        public KsLanguage Language
        {
            get
            {
                return this._Language;
            }
            set
            {
                if (this.SetProperty(ref this._Language, value))
                {
                    this.Settings[nameof(this.Language)] = value.Id;
                    // ToDo A very bad workaround for updating the title of the window.
                    this.NotifyPropertyChanged(nameof(this.Name));
                }
            }
        }

        public IDictionary<string, string> Settings { get; private set; }

        public Application Application { get; }

        public static KsApplication Current { get; private set; }
    }

    public enum KsApplicationState
    {
        NotStarted,
        Initializing,
        Started,
        ShuttingDown,
        ShutDown
    }

    public struct KsApplicationTestData
    {
        public string Name { get; set; }

        public IDictionary<string, string> Settings { get; set; }
    }
}
