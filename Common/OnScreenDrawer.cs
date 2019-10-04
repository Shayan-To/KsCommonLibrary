using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace Ks
{
    namespace Common
    {
        public class OnScreenDrawer
        {
            private OnScreenDrawer(Graphics Graphics)
            {
                this.Graphics = Graphics;
                this._IsDrawing = true;
            }

            public static OnScreenDrawer ForScreen()
            {
                return ForWindowHandle(IntPtr.Zero);
            }

            public static OnScreenDrawer ForWindowHandle(IntPtr Handle)
            {
                return ForGraphics(Graphics.FromHwnd(Handle));
            }

            public static OnScreenDrawer ForGraphics(Graphics Graphics)
            {
                return new OnScreenDrawer(Graphics);
            }

            private void AddToGroup(TimeSpan Interval, Drawing Drawing)
            {
                DrawingGroup Group = null;
                if (!this.Groups.TryGetValue(Interval, out Group))
                {
                    Group = new DrawingGroup(this, Interval);
                    this.Groups[Interval] = Group;

                    if (this.IsDrawing)
                        Group.StartDrawing();
                }

                Group.Drawings.Add(Drawing);
            }

            private void RemoveFromGroup(TimeSpan Interval, Drawing Drawing)
            {
                var Group = this.Groups[Interval];
                Assert.True(Group.Drawings.Remove(Drawing));
                if (Group.Drawings.Count == 0)
                {
                    Group.StopDrawing();
                    this.Groups.Remove(Interval);
                }
            }

            private void ReportIntervalChange(Drawing Drawing, TimeSpan OldInterval)
            {
                if (Drawing.IsVisible)
                {
                    this.RemoveFromGroup(OldInterval, Drawing);
                    this.AddToGroup(Drawing.Interval, Drawing);
                }
            }

            private void ReportIsVisibleChange(Drawing Drawing)
            {
                if (Drawing.IsVisible)
                    this.AddToGroup(Drawing.Interval, Drawing);
                else
                    this.RemoveFromGroup(Drawing.Interval, Drawing);
            }

            private void ReportDrawingGotIn(Drawing Drawing)
            {
                Drawing.SetParent(this);
                if (Drawing.IsVisible)
                    this.AddToGroup(Drawing.Interval, Drawing);
            }

            private void ReportDrawingGotOut(Drawing Drawing)
            {
                Drawing.SetParent(null);
                if (Drawing.IsVisible)
                    this.RemoveFromGroup(Drawing.Interval, Drawing);
            }

            private void StartDrawing()
            {
                Verify.False(this.IsDrawing, "Already drawing.");
                this._IsDrawing = true;
                foreach (var D in this.Groups.Values)
                    D.StartDrawing();
            }

            private void StopDrawing()
            {
                this._IsDrawing = false;
                foreach (var D in this.Groups.Values)
                    D.StopDrawing();
            }

            private readonly DrawingsCollection _Drawings = new DrawingsCollection(this);

            public IList<Drawing> Drawings
            {
                get
                {
                    return this._Drawings;
                }
            }

            private bool _IsDrawing;

            public bool IsDrawing
            {
                get
                {
                    return this._IsDrawing;
                }
                set
                {
                    if (value != this._IsDrawing)
                    {
                        if (value)
                            this.StartDrawing();
                        else
                            this.StopDrawing();
                    }
                }
            }

            private readonly Dictionary<TimeSpan, DrawingGroup> Groups = new Dictionary<TimeSpan, DrawingGroup>();
            private readonly Graphics Graphics;

            private class DrawingsCollection : BaseList<Drawing>
            {
                public DrawingsCollection(OnScreenDrawer Parent)
                {
                    this.Parent = Parent;
                }

                private void OnChanged()
                {
                }

                private void ItemGotIn(Drawing Item)
                {
                    this.Parent.ReportDrawingGotIn(Item);
                }

                private void ItemGotOut(Drawing Item)
                {
                    this.Parent.ReportDrawingGotOut(Item);
                }

                public override void Clear()
                {
                    foreach (var I in this)
                        this.ItemGotOut(I);
                    this.Base.Clear();
                    this.OnChanged();
                }

                public override void Insert(int index, Drawing item)
                {
                    this.ItemGotIn(item);
                    this.Base.Insert(index, item);
                    this.OnChanged();
                }

                public override void RemoveAt(int index)
                {
                    this.ItemGotOut(this.Base[index]);
                    this.Base.RemoveAt(index);
                    this.OnChanged();
                }

                protected override IEnumerator<Drawing> _GetEnumerator()
                {
                    return this.GetEnumerator();
                }

                public List<Drawing>.Enumerator GetEnumerator()
                {
                    return this.Base.GetEnumerator();
                }

                public override Drawing this[int index]
                {
                    get
                    {
                        return this.Base[index];
                    }
                    set
                    {
                        this.ItemGotOut(this.Base[index]);
                        this.ItemGotIn(value);
                        this.Base[index] = value;
                        this.OnChanged();
                    }
                }

                public override int Count
                {
                    get
                    {
                        return this.Base.Count;
                    }
                }

                private readonly List<Drawing> Base = new List<Drawing>();
                private readonly OnScreenDrawer Parent;
            }

            private class DrawingGroup
            {
                public DrawingGroup(OnScreenDrawer Parent, TimeSpan Interval)
                {
                    this.Parent = Parent;
                    this.Interval = Interval;
                }

                public async void StartDrawing()
                {
                    Assert.True(this.Parent.IsDrawing);

                    if (this.IsDrawing)
                        return;
                    this.IsDrawing = true;

                    var Graphics = this.Parent.Graphics;

                    do
                    {
                        await Task.Delay(this.Interval);
                        if (!this.IsDrawing)
                            break;
                        foreach (var D in this.Drawings)
                        {
                            Assert.True(D.Interval == this.Interval);
                            D.Draw(Graphics);
                        }
                    }
                    while (true);

                    this.IsDrawing = false;
                }

                public void StopDrawing()
                {
                    this.IsDrawing = false;
                }

                private readonly List<Drawing> _Drawings = new List<Drawing>();

                public List<Drawing> Drawings
                {
                    get
                    {
                        return this._Drawings;
                    }
                }

                private bool IsDrawing;
                private readonly OnScreenDrawer Parent;
                private readonly TimeSpan Interval;
            }

            public class Drawing
            {
                public Drawing(int IntervalMillis, int Width, int Height) : this(IntervalMillis, 0, 0, Width, Height)
                {
                }

                public Drawing(int IntervalMillis, int X, int Y, int Width, int Height) : this(TimeSpan.FromMilliseconds((double)IntervalMillis), new Rectangle(X, Y, Width, Height))
                {
                }

                public Drawing(TimeSpan Interval, Rectangle Bounds)
                {
                    this._Interval = Interval;
                    this._Bounds = Bounds;
                    this.Bitmap = new Bitmap(Bounds.Width, Bounds.Height);
                    this._Graphics = Graphics.FromImage(this.Bitmap);
                }

                internal void SetParent(OnScreenDrawer Parent)
                {
                    Verify.True(Parent == null ^ this.Parent == null, "Cannot add a drawing to two drawers.");
                    this.Parent = Parent;
                }

                private void RecreateBitmap()
                {
                    this._Graphics.Dispose();
                    this.Bitmap.Dispose();
                    this.Bitmap = new Bitmap(this.Bounds.Width, this.Bounds.Height);
                    this._Graphics = Graphics.FromImage(this.Bitmap);
                }

                public void Show()
                {
                    this._IsVisible = true;
                    this.Parent?.ReportIsVisibleChange(this);
                }

                public void Hide()
                {
                    this._IsVisible = false;
                    this.Parent?.ReportIsVisibleChange(this);
                }

                internal void Draw(Graphics Graphics)
                {
                    Graphics.DrawImageUnscaled(this.Bitmap, this.Bounds);
                }

                private TimeSpan _Interval;

                public TimeSpan Interval
                {
                    get
                    {
                        return this._Interval;
                    }
                    set
                    {
                        var Old = this._Interval;
                        this._Interval = value;
                        this.Parent?.ReportIntervalChange(this, Old);
                    }
                }

                private Rectangle _Bounds;

                public Rectangle Bounds
                {
                    get
                    {
                        return this._Bounds;
                    }
                    set
                    {
                        var ShouldRecreate = (this._Bounds.Width != value.Width) | (this._Bounds.Height != value.Height);
                        this._Bounds = value;
                        if (ShouldRecreate)
                            this.RecreateBitmap();
                    }
                }

                private Graphics _Graphics;

                public Graphics Graphics
                {
                    get
                    {
                        return this._Graphics;
                    }
                }

                private bool _IsVisible;

                public bool IsVisible
                {
                    get
                    {
                        return this._IsVisible;
                    }
                    set
                    {
                        if (this._IsVisible != value)
                        {
                            if (value)
                                this.Show();
                            else
                                this.Hide();
                        }
                    }
                }

                private OnScreenDrawer Parent;
                private Bitmap Bitmap;
            }
        }
    }
}
