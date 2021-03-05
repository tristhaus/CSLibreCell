using Terminal.Gui;

namespace CSLibreCell.Internal
{
    internal static class Configuration
    {
        internal static class Keys
        {
            internal static class Menu
            {
                internal static Key RandomGame => Key.F5;

                internal static Key ChooseGame => Key.F8;

                internal static Key Help => Key.F1;

                internal static Key Status => Key.F12;

            }

            internal static class Game
            {
                internal static Key Cancel => Key.Space;

                internal static Key Undo => (Key)'R';

                internal static Key Cell0 => (Key)'q';

                internal static Key Cell1 => (Key)'w';

                internal static Key Cell2 => (Key)'e';

                internal static Key Cell3 => (Key)'r';

                internal static Key Column0 => (Key)'a';

                internal static Key Column1 => (Key)'s';

                internal static Key Column2 => (Key)'d';

                internal static Key Column3 => (Key)'f';

                internal static Key Column4 => (Key)'j';

                internal static Key Column5 => (Key)'k';

                internal static Key Column6 => (Key)'l';

                internal static Key Column7 => (Key)'ö';

                internal static Key Foundation0 => (Key)'u';

                internal static Key Foundation1 => (Key)'i';

                internal static Key Foundation2 => (Key)'o';

                internal static Key Foundation3 => (Key)'p';
            }
        }
    }
}
