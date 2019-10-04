﻿using System.Windows;
using System.Windows.Controls;

namespace Ks
{
    namespace Common.Controls
    {
        [System.ComponentModel.DesignTimeVisible(false)]
        public class Page : ContentControl
        {
            static Page()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(typeof(Page)));
            }

            public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Page), new PropertyMetadata(null));

            public string Title
            {
                get
                {
                    return (string)this.GetValue(TitleProperty);
                }
                set
                {
                    this.SetValue(TitleProperty, value);
                }
            }

            private MVVM.INavigationView _ParentView;

            internal MVVM.INavigationView ParentView
            {
                get
                {
                    return this._ParentView;
                }
                set
                {
                    this._ParentView = value;
                }
            }
        }
    }
}