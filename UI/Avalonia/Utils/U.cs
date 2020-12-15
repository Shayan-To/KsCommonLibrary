using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;

using Ks.Common;

using ReactiveUI;

namespace Ks.Avalonia.Utils
{
    public class U : AvaloniaObject
    {
        static U()
        {
            HorizontalWheelScrollProperty.Changed.Subscribe(e => HorizontalWheelScrollChanged(e));
            DoubleClickCommandProperty.Changed.Subscribe(e => DoubleClickCommandChanged(e));
        }

        public static readonly AttachedProperty<bool> HorizontalWheelScrollProperty = AvaloniaProperty.RegisterAttached<ScrollViewer, bool>("HorizontalWheelScroll", typeof(U));

        public static bool GetHorizontalWheelScroll(ScrollViewer host)
        {
            return host.GetValue(HorizontalWheelScrollProperty);
        }

        public static void SetHorizontalWheelScroll(ScrollViewer host, bool value)
        {
            host.SetValue(HorizontalWheelScrollProperty, value);
        }

        private static void HorizontalWheelScrollChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            var element = (ScrollViewer) e.Sender;

            if (e.OldValue.GetValueOrDefault())
            {
                element.RemoveHandler(ScrollViewer.PointerWheelChangedEvent, Element_PointerWheelChanged);
            }

            if (e.NewValue.GetValueOrDefault())
            {
                element.AddHandler(ScrollViewer.PointerWheelChangedEvent, Element_PointerWheelChanged, handledEventsToo: true);
            }

            static void Element_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
            {
                if (sender == null)
                {
                    return;
                }

                var element = (ScrollViewer) sender;

                if (e.Delta.Y > 0)
                {
                    element.LineLeft();
                    element.LineLeft();
                    element.LineLeft();
                }
                else if (e.Delta.Y < 0)
                {
                    element.LineRight();
                    element.LineRight();
                    element.LineRight();
                }
            }
        }

        public static readonly AttachedProperty<ICommand> DoubleClickCommandProperty = AvaloniaProperty.RegisterAttached<Control, ICommand>("DoubleClickCommand", typeof(U));

        public static ICommand GetDoubleClickCommand(Control host)
        {
            return host.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommand(Control host, ICommand value)
        {
            host.SetValue(DoubleClickCommandProperty, value);
        }

        private static readonly ConditionalWeakTable<Control, IDisposable> _DoubleClickSubscriptions = new();

        private static void DoubleClickCommandChanged(AvaloniaPropertyChangedEventArgs<ICommand> e)
        {
            var element = (Control) e.Sender;
            var command = e.NewValue.GetValueOrDefault();

            if (_DoubleClickSubscriptions.TryGetValue(element, out var oldSubscription))
            {
                oldSubscription?.Dispose();
                _DoubleClickSubscriptions.Remove(element);
            }

            if (command == null)
            {
                return;
            }

            var settings = AvaloniaLocator.Current.GetService<IPlatformSettings>();
            var doubleClickTime = settings.DoubleClickTime;

            var subscription = element.GetObservable(Control.PointerReleasedEvent)
                .TimeInterval()
                .Skip(1)
                .Where(e => e.Interval <= doubleClickTime)
                .Subscribe(_ =>
                {
                    var parameter = GetDoubleClickCommandParameter(element);
                    if (command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }
                });

            _DoubleClickSubscriptions.Add(element, subscription);
        }

        public static readonly AttachedProperty<object?> DoubleClickCommandParameterProperty = AvaloniaProperty.RegisterAttached<Control, object?>("DoubleClickCommandParameter", typeof(U));

        public static object? GetDoubleClickCommandParameter(Control host)
        {
            return host.GetValue(DoubleClickCommandParameterProperty);
        }

        public static void SetDoubleClickCommandParameter(Control host, object? value)
        {
            host.SetValue(DoubleClickCommandParameterProperty, value);
        }

        public static readonly AttachedProperty<bool> Boolean1Property = AvaloniaProperty.RegisterAttached<Control, bool>("Boolean1", typeof(U));

        public static bool GetBoolean1(Control host)
        {
            return host.GetValue(Boolean1Property);
        }

        public static void SetBoolean1(Control host, bool value)
        {
            host.SetValue(Boolean1Property, value);
        }

    }
}
