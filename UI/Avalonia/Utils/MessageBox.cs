using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

using ReactiveUI;

namespace Ks.Avalonia.Utils
{
    public static class MessageBox
    {
        public static IObservable<Unit> Show(string title, string message)
        {
            var command = ReactiveCommand.Create(() => { });

            var window = new Window
            {
                Title = title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                CanResize = false,
                Width = 300,
                Height = 120,
                Content = new DockPanel()
                {
                    Children =
                    {
                        new Panel()
                        {
                            Background = BrushCache.GetBrush(Color.Parse("#eeeeee")),
                            Children =
                            {
                                new Button()
                                {
                                    Command = command,
                                    Content = new TextBlock() { Text = "OK" },
                                    Width = 50,
                                    Margin = new(5),
                                    HorizontalAlignment = HorizontalAlignment.Right,
                                },
                            },
                            [DockPanel.DockProperty] = Dock.Bottom,
                        },
                        new TextBlock()
                        {
                            Text = message,
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new(15),
                            VerticalAlignment = VerticalAlignment.Center,
                            [DockPanel.DockProperty] = Dock.Top,
                        },
                    },
                },
                KeyBindings =
                {
                    new KeyBinding()
                    {
                        Gesture = new KeyGesture(Key.Enter),
                        Command = command
                    },
                    new KeyBinding()
                    {
                        Gesture = new KeyGesture(Key.Escape),
                        Command = command
                    },
                },
            };

            window.AttachDevTools();
            window.Show();

            return command.Do(_ =>
            {
                window.Close();
            });
        }
    }
}
