﻿using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Ks.Common.MVVM
    {
        public class NavigationFrame : IEnumerable<ViewModel>
        {
            public NavigationFrame(IEnumerable<ViewModel> List)
            {
                this.List = List.ToArray();
            }

            public IEnumerator<ViewModel> GetEnumerator()
            {
                return ((IEnumerable<ViewModel>)this.List).GetEnumerator();
            }

            private IEnumerator IEnumerable_GetEnumerator()
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
                var loopTo = this.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (this.Item == ViewModel)
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
                var loopTo = Left.List.Length - 1;
                for (int I = 0; I <= loopTo; I++)
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
                    return this.Item;
                }
            }

            public NavigationViewModel Parents
            {
                get
                {
                    return (NavigationViewModel)this.Item;
                }
            }

            public ViewModel Item
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
}
