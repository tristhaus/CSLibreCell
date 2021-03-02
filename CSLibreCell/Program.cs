using Core;
using CSLibreCell.Internal;
using System;
using Terminal.Gui;

namespace CSLibreCell
{
    class Program
    {
        private static readonly Random Random = new Random();
        private static readonly Handler Handler = new Handler();
        private static Label GameLabel;
        private static Location? Source = null;

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
            void HandleLocation(Location location)
            {
                if (Source == null)
                {
                    Source = location;
                }
                else
                {
                    var refresh = Handler.ExecuteCommand(Handler.Command.Move((Location)Source, location));
                    Source = null;
                    if (refresh)
                    {
                        RefreshGame();
                    }
                }
            }

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
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Cancel)
            {
                Source = null;
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Cell0)
            {
                HandleLocation(Location.Cell0);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Cell1)
            {
                HandleLocation(Location.Cell1);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Cell2)
            {
                HandleLocation(Location.Cell2);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Cell3)
            {
                HandleLocation(Location.Cell3);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column0)
            {
                HandleLocation(Location.Column0);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column1)
            {
                HandleLocation(Location.Column1);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column2)
            {
                HandleLocation(Location.Column2);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column3)
            {
                HandleLocation(Location.Column3);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column4)
            {
                HandleLocation(Location.Column4);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column5)
            {
                HandleLocation(Location.Column5);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column6)
            {
                HandleLocation(Location.Column6);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Column7)
            {
                HandleLocation(Location.Column7);
            }
            else if (obj.KeyEvent.Key == Configuration.Keys.Game.Foundation0 || obj.KeyEvent.Key == Configuration.Keys.Game.Foundation1 || obj.KeyEvent.Key == Configuration.Keys.Game.Foundation2 || obj.KeyEvent.Key == Configuration.Keys.Game.Foundation3)
            {
                HandleLocation(Location.Foundation);
            }
        }

        private static void StartRandomGame()
        {
            var id = Random.Next(1, 65537);
            StartGame(Convert.ToUInt32(id));
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
                    StartGame(id);
                }
            };
            dialog.AddButton(ok);

            Application.Run(dialog);
        }

        private static void StartGame(uint id)
        {
            var refresh = Handler.ExecuteCommand(Handler.Command.NewGame(id));
            if (refresh)
            {
                RefreshGame();
            }
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
