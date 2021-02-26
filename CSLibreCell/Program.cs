using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace CSLibreCell
{
    class Program
    {
        private static uint counter = 0;
        private static Label theLogin;

        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var login = new Label($"Counter: {counter}") { X = 2, Y = 1 };
            theLogin = login;

            // Creates the top-level window to show
            var win = new Window("MyApp")
            {
                X = 0,
                Y = 0, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);
            top.KeyDown += Top_KeyPress;

            // Add some controls, 
            win.Add(
                // The ones with my favorite layout system, Computed
                login
            );

            Application.Run();
        }

        private static void Top_KeyPress(View.KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.Key == Key.Space)
            {
                counter++;
                theLogin.Text = $"Counter: {counter}";
            }
        }
    }
}
