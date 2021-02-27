using System;
using Terminal.Gui;

namespace CSLibreCell
{
    class Program
    {
        private static uint counter = 0; //todo: remove this when no longer used
        private static Label theLogin;

        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var login = new Label($"Counter: {counter}") { X = 2, Y = 1 };
            login.Width = Dim.Fill();
            login.Height = Dim.Fill();
            theLogin = login;

            // Creates the top-level window to show
            var win = new Window(Localization.WindowTitle)
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem (Localization.MenuBar.Game, new MenuItem [] {
                new MenuItem (Localization.MenuBar.Random, Localization.MenuBar.RandomHint, StartRandomGame),
                new MenuItem (Localization.MenuBar.Choose, Localization.MenuBar.ChooseHint, ShowChooseDialog),
                new MenuItem (Localization.MenuBar.Quit, string.Empty, () => { top.Running = false; })
            }),
            new MenuBarItem (Localization.MenuBar.Other, new MenuItem [] {
                new MenuItem (Localization.MenuBar.Help, string.Empty, ShowHelpDialog),
                new MenuItem (Localization.MenuBar.Status, string.Empty, ShowStatusDialog),
            })
            });
            top.Add(menu);

            top.KeyDown += Top_KeyPress;

            win.Add(
                login
            );

            Application.Run();
        }

        private static void Top_KeyPress(View.KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.Key == Configuration.Keys.Menu.RandomGame)
            {
                StartRandomGame();
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Menu.ChooseGame)
            {
                ShowChooseDialog();
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Menu.Help)
            {
                ShowHelpDialog();
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Menu.Status)
            {
                ShowStatusDialog();
            } //TODO: extend to playing system
        }

        private static void StartRandomGame()
        {
            counter++;
            theLogin.Text = $"Counter: {counter}"; //TODO: replace by proper code
        }

        private static void ShowChooseDialog()
        {
            var dialog = new Dialog(Localization.ChooseDialog.Title, 40, 20);

            var entry = new TextField()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1,
            };
            dialog.Add(entry); //TODO: evaluate

            var ok = new Button(Localization.OK, true);
            ok.Clicked += () => { Application.RequestStop(); };
            dialog.AddButton(ok);

            Application.Run(dialog);
        }

        private static void ShowHelpDialog()
        {
            throw new NotImplementedException(); //TODO
        }

        private static void ShowStatusDialog()
        {
            throw new NotImplementedException(); //TODO
        }
    }
}
