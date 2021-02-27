using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLibreCell
{
    internal static class Localization
    {
        internal static string OK => "OK";
        
        internal static string WindowTitle => "CSLibreCell";

        internal static class MenuBar
        {
            internal static string Game => "_Game";

            internal static string Random => "_Random";

            internal static string RandomHint => "Start new random game";

            internal static string Choose => "_Choose";

            internal static string ChooseHint => "Choose a new game to start";

            internal static string Quit => "_Quit";

            internal static string Other => "_?";

            internal static string Help => "_Help";

            internal static string Status => "_Status";
        }

        internal static class ChooseDialog
        {
            internal static string Title => "Choose game by ID";
        }
    }
}
