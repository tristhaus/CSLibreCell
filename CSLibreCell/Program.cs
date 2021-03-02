using Core;
using CSLibreCell.Internal;
using System;
using Terminal.Gui;

namespace CSLibreCell
{
    class Program
    {
        private static readonly Handler Handler = new Handler();
        private static Label GameLabel;

        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            GameLabel = new Label(string.Empty) { X = 2, Y = 1 };
            GameLabel.Width = Dim.Fill();
            GameLabel.Height = Dim.Fill();

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
                GameLabel
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
            GameLabel.Text = $"random game"; //TODO: replace by proper code
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
            dialog.Add(entry);

            var ok = new Button(Localization.OK, true);
            ok.Clicked += () =>
            {
                Application.RequestStop();
                if (uint.TryParse(entry.Text.ToString(), out var id))
                {
                    var refresh = Handler.ExecuteCommand(Handler.Command.NewGame(id));
                    if (refresh)
                    {
                        RefreshGame();
                    }
                }
            };
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

        private static void RefreshGame()
        {
            GameLabel.Text = Handler.UnicodeGameRepresentation;
        }
    }
}
