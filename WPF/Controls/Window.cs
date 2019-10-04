using System.Windows;
using System.Threading.Tasks;
using Mono;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using Ks.Common.MVVM;

namespace Ks
{
    namespace Ks.Common.Controls
    {

        // ToDo Add some context class that the main window can be get from it. (We kind of have it right now, the KsApplication class is our context class, we just have to make the main objects (like windows and pages and ...) have it. (And maybe it is not needed in this kind of design. Because a view is known only by its view model, and for a view by itself, there is no use.)

        [TemplatePart(Name = Window.PopupsPanelName, Type = typeof(PopupPanel))]
        [TemplatePart(Name = Window.ContentPresenterName, Type = typeof(ContentPresenter))]
        public class Window : System.Windows.Window, INavigationView
        {
            static Window()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
                ContentProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(null, null, (D, C) => C as UIElement));
            }

            internal void AddPopup(Popup Popup)
            {
                var LayerIndex = Popup.Layer;
                Popup NextLayerFirst = null;

                foreach (var KV in this.ShelterLayers)
                {
                    if (KV.Key > LayerIndex)
                    {
                        NextLayerFirst = KV.Value[0];
                        break;
                    }
                }

                var PanelChildren = this.PopupsPanel.Children;
                var LastIndex = -1;

                if (NextLayerFirst == null)
                    LastIndex = PanelChildren.Count;
                else
                    LastIndex = PanelChildren.IndexOf(NextLayerFirst);

                var Layer = this.ShelterLayers[LayerIndex];
                PopupShelter Shelter = null;

                if (Popup.HasShelter)
                {
                    Shelter = new PopupShelter() { DataContext = Popup, Style = this.ShelterStyle };
                    this.PopupShelterDic.Add(Popup, Shelter);
                    PanelChildren.Insert(LastIndex, Shelter);
                    LastIndex += 1;
                }

                PanelChildren.Insert(LastIndex, Popup);

                this.UpdateDims();
            }

            internal void RemovePopup(Popup Popup, int LayerIndex = -1)
            {
                if (LayerIndex == -1)
                    LayerIndex = Popup.Layer;

                var Shelter = this.PopupShelterDic[Popup];
                this.PopupShelterDic.Remove(Popup);

                var PanelChildren = this.PopupsPanel.Children;
                PanelChildren.Remove(Popup);
                PanelChildren.Remove(Shelter);

                this.UpdateDims();
            }

            internal void RefreshPopup(Popup Popup)
            {
                PopupShelter Shelter = null;

                if (Popup.HasShelter)
                {
                    if (!this.PopupShelterDic.ContainsKey(Popup))
                    {
                        Shelter = new PopupShelter() { DataContext = Popup };
                        this.PopupShelterDic.Add(Popup, Shelter);

                        var PanelChildren = this.PopupsPanel.Children;
                        var PopupIndex = PanelChildren.IndexOf(Popup);
                        PanelChildren.Insert(PopupIndex, Shelter);
                    }
                }
                else if (this.PopupShelterDic.TryGetValue(Popup, out Shelter))
                {
                    this.PopupsPanel.Children.Remove(Shelter);
                    this.PopupShelterDic.Remove(Popup);
                }

                this.UpdateDims();
            }

            internal void UpdateDims()
            {
                var DimmedSeen = false;
                foreach (int I in this.ShelterLayers.Keys.Reverse())
                {
                    var Layer = this.ShelterLayers[I];
                    for (int J = Layer.Count - 1; J >= 0; J += -1)
                    {
                        var Popup = Layer[J];
                        if (Popup.HasShelter)
                        {
                            var Shelter = this.PopupShelterDic[Popup];
                            if (DimmedSeen)
                                Shelter.IsShelterShown = false;
                            else
                            {
                                Shelter.IsShelterShown = true;
                                if (Popup.DimShelter)
                                    DimmedSeen = true;
                            }
                        }
                    }
                }
            }

            private void UpdateShelterStyle()
            {
                var ShelterStyle = this.ShelterStyle;
                foreach (var KV in this.PopupShelterDic)
                    KV.Value.Style = ShelterStyle;
            }

            private Page INavigationView_Content
            {
                get
                {
                    return this.Content as Page;
                }
                set
                {
                    this.Content = value;
                }
            }

            public static readonly DependencyProperty WindowStartupPositionProperty = DependencyProperty.Register("WindowStartupPosition", typeof(WindowStartupLocation), typeof(Window), new PropertyMetadata(WindowStartupLocation.Manual, WindowStartupPosition_Changed, WindowStartupPosition_Coerce));

            private static object WindowStartupPosition_Coerce(DependencyObject D, object BaseValue)
            {
                // Dim Self = DirectCast(D, Window)

                // Dim Value = DirectCast(BaseValue, WindowStartupLocation)

                return BaseValue;
            }

            private static void WindowStartupPosition_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
            {
                var Self = (Window)D;

                // Dim OldValue = DirectCast(E.OldValue, WindowStartupLocation)
                var NewValue = (WindowStartupLocation)E.NewValue;

                Self.WindowStartupLocation = NewValue;
            }

            public WindowStartupLocation WindowStartupPosition
            {
                get
                {
                    return (WindowStartupLocation)this.GetValue(WindowStartupPositionProperty);
                }
                set
                {
                    this.SetValue(WindowStartupPositionProperty, value);
                }
            }

            public static readonly DependencyProperty ShelterStyleProperty = DependencyProperty.Register("ShelterStyle", typeof(Style), typeof(Window), new PropertyMetadata(null, ShelterStyle_Changed));

            private static void ShelterStyle_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
            {
                var Self = (Window)D;

                // Dim OldValue = DirectCast(E.OldValue, Style)
                // Dim NewValue = DirectCast(E.NewValue, Style)

                Self.UpdateShelterStyle();
            }

            public Style ShelterStyle
            {
                get
                {
                    return (Style)this.GetValue(ShelterStyleProperty);
                }
                set
                {
                    this.SetValue(ShelterStyleProperty, value);
                }
            }

            public override void OnApplyTemplate()
            {
                this.PopupsPanel = this.Template.FindName(PopupsPanelName, this) as PopupPanel;
                this.ContentPresenter = this.Template.FindName(ContentPresenterName, this) as ContentPresenter;

                base.OnApplyTemplate();
            }

            internal const string PopupsPanelName = "PART_PopupsPanel";
            private PopupPanel _PopupsPanel;

            internal PopupPanel PopupsPanel
            {
                get
                {
                    return this._PopupsPanel;
                }
                private set
                {
                    this._PopupsPanel = value;
                }
            }

            internal const string ContentPresenterName = "PART_ContentPresenter";
            private ContentPresenter _ContentPresenter;

            private ContentPresenter ContentPresenter
            {
                get
                {
                    return this._ContentPresenter;
                }
                set
                {
                    this._ContentPresenter = value;
                }
            }

            private readonly CreateInstanceDictionary<int, List<Popup>> ShelterLayers = CreateInstanceDictionary.Create(new SortedDictionary<int, List<Popup>>());
            private readonly Dictionary<Popup, PopupShelter> PopupShelterDic = new Dictionary<Popup, PopupShelter>();
        }
    }
}
