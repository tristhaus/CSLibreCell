﻿namespace CSLibreCell.Internal
{
    internal static class Localization
    {
        internal static string OK => Resources.ResourceManager.GetString("OK");

        internal static string WindowTitle => Resources.ResourceManager.GetString("WindowTitle");

        internal static string GameWonMessage => Resources.ResourceManager.GetString("GameWonMessage");

        internal static string GameIsUnwinnable => Resources.ResourceManager.GetString("GameIsUnwinnable");

        internal static class MenuBar
        {
            internal static string Game => Resources.ResourceManager.GetString("MenuBar_Game");

            internal static string Random => Resources.ResourceManager.GetString("MenuBar_Random");

            internal static string RandomHint => Resources.ResourceManager.GetString("MenuBar_RandomHint");

            internal static string Choose => Resources.ResourceManager.GetString("MenuBar_Choose");

            internal static string ChooseHint => Resources.ResourceManager.GetString("MenuBar_ChooseHint");

            internal static string Undo => Resources.ResourceManager.GetString("MenuBar_Undo");

            internal static string Quit => Resources.ResourceManager.GetString("MenuBar_Quit");

            internal static string Other => Resources.ResourceManager.GetString("MenuBar_Other");

            internal static string Help => Resources.ResourceManager.GetString("MenuBar_Help");

            internal static string Status => Resources.ResourceManager.GetString("MenuBar_Status");
        }

        internal static class ChooseDialog
        {
            internal static string Title => Resources.ResourceManager.GetString("ChooseDialog_Title");
        }

        internal static class HelpDialog
        {
            internal static string Title => Resources.ResourceManager.GetString("HelpDialog_Title");

            internal static string ContentTemplate => Resources.ResourceManager.GetString("HelpDialog_ContentTemplate");
        }

        internal static class StatusDialog
        {
            internal static string Title => Resources.ResourceManager.GetString("StatusDialog_Title");

            internal static string ContentTemplate => Resources.ResourceManager.GetString("StatusDialog_ContentTemplate");

            internal static string NotStarted => Resources.ResourceManager.GetString("StatusDialog_NotStarted");

            internal static string FirstStage => Resources.ResourceManager.GetString("StatusDialog_FirstStage");

            internal static string SecondStage => Resources.ResourceManager.GetString("StatusDialog_SecondStage");

            internal static string Finished => Resources.ResourceManager.GetString("StatusDialog_Finished");

            internal static string ProgressTemplate => Resources.ResourceManager.GetString("StatusDialog_ProgressTemplate");
        }
    }
}
