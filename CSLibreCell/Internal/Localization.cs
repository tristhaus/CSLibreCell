namespace CSLibreCell.Internal
{
    internal static class Localization
    {
        internal static string OK => "OK";
        
        internal static string WindowTitle => "CSLibreCell";

        internal static string GameWon => "CONGRATULATIONS!";

        internal static string GameIsUnwinnable => "Game is known to be unwinnable";

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

        internal static class HelpDialog
        {
            internal static string Title => "Help";

            internal static string ContentTemplate => "CSLibreCell - A FreeCell implementation.\r\n\r\nThis help: {18}\r\nStatus: {19} \r\nNew random game: {20}\r\nChoose Game: {21}\r\n\r\nMaking moves: specify source and destination using:\r\n{0} {1} {2} {3} || {4} {5} {6} {7}\r\n {8} {9} {10} {11} {12} {13} {14} {15}\r\n\r\nCancel move: {16}\r\nUnlimited undo: {17}";
        }

        internal static class StatusDialog
        {
            internal static string Title => "Status";

            internal static string ContentTemplate => "{0}\r\n{1}";

            internal static string NotStarted => "Journey not started.";
            
            internal static string FirstStage => "First stage of journey: 1-32000";
            
            internal static string SecondStage => "Second stage of journey: 32001-64000";
            
            internal static string Finished => "Journey 1-64000 completed!";

            internal static string ProgressTemplate => "{0} of 32000 games in stage completed";
        }
    }
}
