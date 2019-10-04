using System.Threading.Tasks;
using System;

namespace Ks
{
    namespace Ks.Common.MVVM
    {
        public abstract class SingleTaskViewModel : ViewModel
        {
            public SingleTaskViewModel(KsApplication KsApplication) : base(KsApplication)
            {
            }

            public SingleTaskViewModel() : base()
            {
            }

            protected internal override void OnNavigatedTo(NavigationEventArgs E)
            {
                if ((int)E.NavigationType == (int)NavigationType.NewNavigation)
                    this.IsWorkDone = false;
            }

            protected internal override void OnNavigatedFrom(NavigationEventArgs E)
            {
                if ((int)E.NavigationType == (int)NavigationType.NewNavigation)
                    this.IsWorkDone = true;
            }

            public Task WhenWorkDone()
            {
                if (this.WhenWorkDoneTaskSource == null)
                {
                    this.WhenWorkDoneTaskSource = new TaskCompletionSource<Void>();
                    if (this.IsWorkDone)
                        this.WhenWorkDoneTaskSource.SetResult(null);
                }

                return this.WhenWorkDoneTaskSource.Task;
            }

            private bool _IsWorkDone;

            public bool IsWorkDone
            {
                get
                {
                    return this._IsWorkDone;
                }
                protected set
                {
                    if (this.SetProperty(ref this._IsWorkDone, value))
                    {
                        if (value)
                            this.WhenWorkDoneTaskSource?.SetResult(null);
                        else
                            this.WhenWorkDoneTaskSource = null;
                    }
                }
            }

            private TaskCompletionSource<Void> WhenWorkDoneTaskSource;
        }
    }
}
