using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Ks.Common.MVVM
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
        public class NavigationFrame : IEnumerable<ViewModel>
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        {
            public NavigationFrame(IEnumerable<ViewModel> List)
            {
                this.List = List.ToArray();
            }

            public IEnumerator<ViewModel> GetEnumerator()
            {
                return ((IEnumerable<ViewModel>)this.List).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public NavigationFrame AddViewModel(ViewModel ViewModel)
            {
                Verify.True(this.IsOpenEnded, "Cannot add a view-model to a non-open-ended frame.");
                return new NavigationFrame(this.List.Append(ViewModel));
            }

            public NavigationFrame SubFrame(int Length)
            {
                return new NavigationFrame(this.List.Take(Length));
            }

            public int IndexOf(ViewModel ViewModel)
            {
                for (var I = 0; I < this.Count; I++)
                {
                    if (this[I] == ViewModel)
                        return I;
                }
                return -1;
            }

            public override bool Equals(object obj)
            {
                var Frame = obj as NavigationFrame;
                if (obj == null)
                    return false;
                return this == Frame;
            }

            public static bool operator ==(NavigationFrame Left, NavigationFrame Right)
            {
                if (Left == null)
                    return Right == null;
                if (Right == null)
                    return false;

                if (Left.List.Length != Right.List.Length)
                    return false;
                for (var I = 0; I < Left.List.Length; I++)
                {
                    if (Left.List[I] != Right.List[I])
                        return false;
                }

                return true;
            }

            public static bool operator !=(NavigationFrame Left, NavigationFrame Right)
            {
                return !(Left == Right);
            }

            public bool IsOpenEnded
            {
                get
                {
                    return this.Tip.IsNavigation();
                }
            }

            public ViewModel Tip
            {
                get
                {
                    return this[this.Count - 1];
                }
            }

            public NavigationViewModel GetParent(int Index)
            {
                return (NavigationViewModel)this[this.Count - 2 - Index];
            }

            public ViewModel this[int Index]
            {
                get
                {
                    return this.List[Index];
                }
            }

            public int Count
            {
                get
                {
                    return this.List.Length;
                }
            }

            private readonly ViewModel[] List;
        }
    }
