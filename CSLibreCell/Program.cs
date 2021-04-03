/*
 * This file is part of CSLibreCell.
 * 
 * CSLibreCell is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * CSLibreCell is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with CSLibreCell.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using Core;
using CSLibreCell.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using Terminal.Gui;

namespace CSLibreCell
{
    class Program
    {
        private static readonly Random Random = new Random();
        private static readonly Handler Handler;
        private static Location? Source = null;
        private static Location? Highlight = null;

        private static readonly Configuration Config;
        private static readonly List<string> ConfigLog;

        private static readonly List<Label> CellLabels = new List<Label>(4);
        private static readonly List<Label> FoundationLabels = new List<Label>(4);
        private static readonly List<List<Label>> ColumnLabels = new List<List<Label>>(8);
        private static List<Label> StaticLabels;
        private static Label MessageLabel;
        private static Window Win;

        private static readonly Terminal.Gui.Attribute BlackAttribute = new Terminal.Gui.Attribute(Color.Black, Color.White);
        private static readonly Terminal.Gui.Attribute RedAttribute = new Terminal.Gui.Attribute(Color.Red, Color.White);
        private static readonly Terminal.Gui.Attribute BlackHighlightAttribute = new Terminal.Gui.Attribute(Color.Black, Color.Gray);
        private static readonly Terminal.Gui.Attribute RedHighlightAttribute = new Terminal.Gui.Attribute(Color.Red, Color.Gray);
        private static readonly ColorScheme BlackColorScheme = new ColorScheme
        {
            Normal = BlackAttribute,
            Focus = BlackAttribute,
        };
        private static readonly ColorScheme RedColorScheme = new ColorScheme
        {
            Normal = RedAttribute,
            Focus = RedAttribute,
        };
        private static readonly ColorScheme BlackHighlightColorScheme = new ColorScheme
        {
            Normal = BlackHighlightAttribute,
            Focus = BlackHighlightAttribute,
        };
        private static readonly ColorScheme RedHighlightColorScheme = new ColorScheme
        {
            Normal = RedHighlightAttribute,
            Focus = RedHighlightAttribute,
        };

        static Program()
        {
            var configLoader = new ConfigurationLoader();
            (Config, ConfigLog) = configLoader.Read();

            if (Config.UiCulture != null)
            {
                CultureInfo.CurrentCulture = Config.UiCulture;
                CultureInfo.CurrentUICulture = Config.UiCulture;
            }

            Handler = new Handler(Config.JourneyConfig, Config.CardConfig);
        }

        static void Main()
        {
            Application.Init();
            var top = Application.Top;
            top.ColorScheme = BlackColorScheme;

            List<Label> allGameLabels = new List<Label>(100);

            for (int cellIndex = 0; cellIndex < 4; cellIndex++)
            {
                var cellLabel = new Label(" .. ") { X = cellIndex * 4, Y = 1, Width = 4, Height = 1 };
                var copy = cellIndex;
                cellLabel.Clicked += () =>
                {
                    if ((Source != null && Handler.Game != null && Handler.Game.Cells[copy] == null) || Source == null && Handler.Game?.Cells[copy] != null)
                    {
                        HandleLocation(Location.Cell0 + copy, highlight: true);
                        RefreshGame();
                    }
                };

                CellLabels.Add(cellLabel);
            }

            allGameLabels.AddRange(CellLabels);

            for (int foundationIndex = 0; foundationIndex < 4; foundationIndex++)
            {
                var foundationLabel = new Label(" .. ") { X = 18 + foundationIndex * 4, Y = 1, Width = 4, Height = 1 };
                foundationLabel.Clicked += () =>
                {
                    if (Source != null)
                    {
                        HandleLocation(Location.Foundation, highlight: true);
                        RefreshGame();
                    }
                };

                FoundationLabels.Add(foundationLabel);
            }

            allGameLabels.AddRange(FoundationLabels);

            var dividerLabel = new Label("||") { X = 16, Y = 1, Width = 2, Height = 1 };
            dividerLabel.Clicked += () =>
            {
                HandleCancel();
            };

            var lineLabel = new Label("--------------------------------") { X = 1, Y = 2, Width = 32, Height = 1 };
            lineLabel.Clicked += () =>
            {
                HandleCancel();
            };

            StaticLabels = new List<Label>
            {
                dividerLabel,
                lineLabel,
            };

            allGameLabels.AddRange(StaticLabels);

            MessageLabel = new Label(string.Empty) { X = 1, Y = 22, Width = 32, Height = 1 };

            allGameLabels.Add(MessageLabel);

            for (int columnIndex = 0; columnIndex < 8; columnIndex++)
            {
                var list = new List<Label>(19);
                for (int i = 0; i < 19; i++)
                {
                    var label = new Label("    ") { X = 1 + columnIndex * 4, Y = 3 + i, Width = 4, Height = 1 };

                    list.Add(label);
                    allGameLabels.Add(label);
                }

                foreach (var label in list)
                {
                    var copy = columnIndex;
                    label.Clicked += () =>
                    {
                        if ((Source == null && Handler.Game != null && Handler.Game.Columns[copy].Count > 0) || Source != null)
                        {
                            HandleLocation(Location.Column0 + copy, highlight: true);
                            RefreshGame();
                        }
                    };
                }

                ColumnLabels.Add(list);
            }

            // Creates the top-level window to show
            Win = new Window(Localization.WindowTitle)
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            Win.ColorScheme = BlackColorScheme;

            top.Add(Win);


            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem (Localization.MenuBar.Game, new MenuItem [] {
                new MenuItem (Localization.MenuBar.Random, Localization.MenuBar.RandomHint, StartRandomGame),
                new MenuItem (Localization.MenuBar.Choose, Localization.MenuBar.ChooseHint, ShowChooseDialog),
                new MenuItem(Localization.MenuBar.Undo, string.Empty, HandleUndo)
                {
                    CanExecute = () => Handler.CanUndo
                },
                new MenuItem (Localization.MenuBar.Quit, string.Empty, () => { top.Running = false; })
            }),
            new MenuBarItem (Localization.MenuBar.Other, new MenuItem [] {
                new MenuItem (Localization.MenuBar.Help, string.Empty, ShowHelpDialog),
                new MenuItem (Localization.MenuBar.Status, string.Empty, ShowStatusDialog),
            })
            });
            top.Add(menu);

            top.KeyDown += Top_KeyPress;

            Win.Add(allGameLabels.ToArray());

            Application.Run();
        }

        private static void Top_KeyPress(View.KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.Key == Config.KeysConfig.Menu.RandomGame)
            {
                StartRandomGame();
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Menu.ChooseGame)
            {
                ShowChooseDialog();
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Menu.Help)
            {
                ShowHelpDialog();
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Menu.Status)
            {
                ShowStatusDialog();
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Cancel)
            {
                HandleCancel();
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Undo)
            {
                HandleUndo();
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Cell0)
            {
                HandleLocation(Location.Cell0);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Cell1)
            {
                HandleLocation(Location.Cell1);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Cell2)
            {
                HandleLocation(Location.Cell2);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Cell3)
            {
                HandleLocation(Location.Cell3);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column0)
            {
                HandleLocation(Location.Column0);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column1)
            {
                HandleLocation(Location.Column1);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column2)
            {
                HandleLocation(Location.Column2);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column3)
            {
                HandleLocation(Location.Column3);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column4)
            {
                HandleLocation(Location.Column4);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column5)
            {
                HandleLocation(Location.Column5);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column6)
            {
                HandleLocation(Location.Column6);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Column7)
            {
                HandleLocation(Location.Column7);
            }
            else if (obj.KeyEvent.Key == Config.KeysConfig.Game.Foundation0 || obj.KeyEvent.Key == Config.KeysConfig.Game.Foundation1 || obj.KeyEvent.Key == Config.KeysConfig.Game.Foundation2 || obj.KeyEvent.Key == Config.KeysConfig.Game.Foundation3)
            {
                HandleLocation(Location.Foundation);
            }
        }

        private static void HandleLocation(Location location, bool highlight = false)
        {
            if (Handler.Game?.IsWon == false)
            {
                if (Source == null)
                {
                    if (highlight)
                    {
                        Highlight = location;
                    }

                    Source = location;
                }
                else
                {
                    var _ = Handler.Move((Location)Source, location);
                    ResetMoveDisplay();
                }
            }
        }

        private static void HandleCancel()
        {
            ResetMoveDisplay();
        }

        private static void HandleUndo()
        {
            var _ = Handler.Undo();
            ResetMoveDisplay();
        }

        private static void StartRandomGame()
        {
            if (Handler.Stage == Stage.NotStarted || Handler.Stage == Stage.Finished)
            {
                bool isWinnable;
                do
                {
                    var id = Random.Next(1, 65537);
                    isWinnable = StartGame(Convert.ToUInt32(id));
                } while (!isWinnable);
            }
            else
            {
                var refresh = Handler.JourneyGame();

                if (refresh)
                {
                    AddGameIdToWindow();

                    RefreshGame();
                }
            }
        }

        private static void ShowChooseDialog()
        {
            var dialog = new Dialog(Localization.ChooseDialog.Title, 40, 20);

            var entry = new TextField
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
                    var _ = StartGame(id);
                }
            };
            dialog.AddButton(ok);

            Application.Run(dialog);
        }

        /// <summary>
        /// Starts the game with the given ID and indicates whether the game is known to be winnable.
        /// </summary>
        /// <param name="id">The ID of the game to start.</param>
        /// <returns><c>true</c> if the game is winnable.</returns>
        private static bool StartGame(uint id)
        {
            var refresh = Handler.NewGame(id);

            var gameIsWinnable = !Handler.Game.IsImpossibleToWin;

            if (refresh)
            {
                AddGameIdToWindow();

                RefreshGame();
            }

            return gameIsWinnable;
        }

        private static void ShowHelpDialog()
        {
            string FormatKey(Key key)
            {
                switch (key)
                {
                    case Key.Space:
                        return "<SPACE>";
                    case Key.F1:
                        return "<F1>";
                    case Key.F2:
                        return "<F2>";
                    case Key.F3:
                        return "<F3>";
                    case Key.F4:
                        return "<F4>";
                    case Key.F5:
                        return "<F5>";
                    case Key.F6:
                        return "<F6>";
                    case Key.F7:
                        return "<F7>";
                    case Key.F8:
                        return "<F8>";
                    case Key.F9:
                        return "<F9>";
                    case Key.F10:
                        return "<F10>";
                    case Key.F11:
                        return "<F11>";
                    case Key.F12:
                        return "<F12>";

                    default:
                        return $"{(char)key}";
                }
            }

            var content = string.Format(Localization.HelpDialog.ContentTemplate,
                FormatKey(Config.KeysConfig.Game.Cell0),
                FormatKey(Config.KeysConfig.Game.Cell1),
                FormatKey(Config.KeysConfig.Game.Cell2),
                FormatKey(Config.KeysConfig.Game.Cell3),
                FormatKey(Config.KeysConfig.Game.Foundation0),
                FormatKey(Config.KeysConfig.Game.Foundation1),
                FormatKey(Config.KeysConfig.Game.Foundation2),
                FormatKey(Config.KeysConfig.Game.Foundation3),
                FormatKey(Config.KeysConfig.Game.Column0),
                FormatKey(Config.KeysConfig.Game.Column1),
                FormatKey(Config.KeysConfig.Game.Column2),
                FormatKey(Config.KeysConfig.Game.Column3),
                FormatKey(Config.KeysConfig.Game.Column4),
                FormatKey(Config.KeysConfig.Game.Column5),
                FormatKey(Config.KeysConfig.Game.Column6),
                FormatKey(Config.KeysConfig.Game.Column7),
                FormatKey(Config.KeysConfig.Game.Cancel),
                FormatKey(Config.KeysConfig.Game.Undo),
                FormatKey(Config.KeysConfig.Menu.Help),
                FormatKey(Config.KeysConfig.Menu.Status),
                FormatKey(Config.KeysConfig.Menu.RandomGame),
                FormatKey(Config.KeysConfig.Menu.ChooseGame));

            var dialog = new Dialog(Localization.HelpDialog.Title, 0, 0);

            var label = new Label
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Text = content,
            };
            dialog.Add(label);

            var ok = new Button(Localization.OK, is_default: true);
            ok.Clicked += () =>
            {
                Application.RequestStop();
            };
            dialog.AddButton(ok);

            var debugLog = new Button(Localization.HelpDialog.DebugLog, is_default: false);
            debugLog.Clicked += () =>
            {
                var debugDialog = new Dialog(Localization.HelpDialog.DebugLog, 0, 0);

                var textView = new TextView()
                {
                    Text = string.Join("\n", ConfigLog),
                    ReadOnly = true,
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill(),
                    Height = Dim.Fill(),
                };
                
                debugDialog.Add(textView);

                var closeDebugLog = new Button(Localization.OK, is_default: true);
                closeDebugLog.Clicked += () =>
                {
                    debugDialog.Running = false;
                };
                
                debugDialog.AddButton(closeDebugLog);

                Application.Run(debugDialog);
            };

            dialog.AddButton(debugLog);

            Application.Run(dialog);
        }

        private static void ShowStatusDialog()
        {
            string content;

            switch (Handler.Stage)
            {
                case Stage.NotStarted:
                    {
                        content = string.Format(Localization.StatusDialog.ContentTemplate, Localization.StatusDialog.NotStarted, string.Empty);
                        break;
                    }

                case Stage.First32000:
                    {
                        var progress = Handler.OpenGames != null
                            ? string.Format(Localization.StatusDialog.ProgressTemplate, 32000 - Handler.OpenGames)
                            : throw new Exception($"inconsistent state {Handler.Stage}");
                        content = string.Format(Localization.StatusDialog.ContentTemplate, Localization.StatusDialog.FirstStage, progress);
                        break;
                    }

                case Stage.Second32000:
                    {
                        var progress = Handler.OpenGames != null
                            ? string.Format(Localization.StatusDialog.ProgressTemplate, 32000 - Handler.OpenGames)
                            : throw new Exception($"inconsistent state {Handler.Stage}");
                        content = string.Format(Localization.StatusDialog.ContentTemplate, Localization.StatusDialog.SecondStage, progress);
                        break;
                    }

                case Stage.Finished:
                    {
                        content = string.Format(Localization.StatusDialog.ContentTemplate, Localization.StatusDialog.Finished, string.Empty);
                        break;
                    }

                default:
                    throw new Exception($"enum member '{Handler.Stage}' missing in {nameof(ShowStatusDialog)}");
            }

            var dialog = new Dialog(Localization.StatusDialog.Title, 0, 0);

            var label = new Label
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Text = content,
            };
            dialog.Add(label);

            var ok = new Button(Localization.OK, true);
            ok.Clicked += () =>
            {
                Application.RequestStop();
            };
            dialog.AddButton(ok);

            Application.Run(dialog);
        }

        private static void AddGameIdToWindow()
        {
            var idRep = $" #{Handler.Game.Id}";
            Win.Title = $"{ Localization.WindowTitle} {idRep.PadLeft(20, '─')}";
        }

        private static void ResetMoveDisplay()
        {
            Source = null;
            Highlight = null;
            RefreshGame();
        }

        private static void RefreshGame()
        {
            void ApplyCardToLabel(Label label, Card card, string empty, bool highlight)
            {
                label.Text = " " + (card?.UnicodeRepresentation ?? empty) + " ";
                if (card?.IsBlack == false)
                {
                    label.ColorScheme = highlight ? RedHighlightColorScheme : RedColorScheme;
                }
                else
                {
                    label.ColorScheme = highlight ? BlackHighlightColorScheme : BlackColorScheme;
                }
            }

            var game = Handler.Game;
            if (game == null)
            {
                return;
            }

            MessageLabel.Text = game.IsWon ? Localization.GameWonMessage : game.IsImpossibleToWin ? Localization.GameIsUnwinnable : string.Empty;

            for (int cellIndex = 0; cellIndex < 4; cellIndex++)
            {
                bool highlight = Highlight != null && Highlight == Location.Cell0 + cellIndex;
                ApplyCardToLabel(CellLabels[cellIndex], game.Cells[cellIndex], empty: "..", highlight);
            }

            for (int foundationIndex = 0; foundationIndex < 4; foundationIndex++)
            {
                ApplyCardToLabel(FoundationLabels[foundationIndex], game.Foundations[foundationIndex], empty: "..", highlight: false);
            }

            for (int columnIndex = 0; columnIndex < 8; columnIndex++)
            {
                var column = game.Columns[columnIndex];
                var length = column.Count;

                var labels = ColumnLabels[columnIndex];

                bool highlight = Highlight != null && Highlight == Location.Column0 + columnIndex;
                for (int lineIndex = 0; lineIndex < length; lineIndex++)
                {
                    ApplyCardToLabel(labels[lineIndex], column[lineIndex], empty: string.Empty, highlight);
                }

                for (int lineIndex = length; lineIndex < 19; lineIndex++)
                {
                    ApplyCardToLabel(labels[lineIndex], card: null, empty: string.Empty, highlight: false);
                }
            }
        }
    }
}
