﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// Imported
using Microsoft.Win32;
using Serilog;
using XeniaManager.DesktopApp.CustomControls;
using XeniaManager.DesktopApp.Windows;

namespace XeniaManager.DesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for Library.xaml
    /// </summary>
    public partial class Library : Page
    {
        public Library()
        {
            InitializeComponent();
            LoadGames();
        }

        // Functions
        /// <summary>
        /// Loads the games into the Wrappanel
        /// </summary>
        private void LoadGamesIntoUI()
        {
            // Check if there are any games installed
            if (GameManager.Games == null && GameManager.Games.Count <= 0)
            {
                return;
            }

            // Sort the games by name
            IOrderedEnumerable<Game> orderedGames = GameManager.Games.OrderBy(game => game.Title);
            Mouse.OverrideCursor = Cursors.Wait;

            // Go through every game in the list
            foreach (Game game in orderedGames)
            {
                Log.Information($"Adding {game.Title} to the Library");

                // Create a new button for the game
                GameButton button = new GameButton(game, this);

                // Adding game to WrapPanel
                GameLibrary.Children.Add(button);
            }

            Mouse.OverrideCursor = null;
        }

        /// <summary>
        /// Clears the WrapPanel of games and adds the games
        /// </summary>
        public void LoadGames()
        {
            GameLibrary.Children.Clear();
            LoadGamesIntoUI();
        }

        // Adding games into Xenia Manager
        /// <summary>
        /// Goes through every game in the array, calls the function that grabs their TitleID and MediaID and opens a new window where the user selects the game
        /// </summary>
        /// <param name="newGames">Array of game ISOs/xex files</param>
        /// <param name="emulatorVersion">Tells us what Xenia version to use for this game</param>
        private async void AddGames(string[] newGames, EmulatorVersion xeniaVersion)
        {
            // Go through every game in the array
            foreach (string gamePath in newGames)
            {
                Log.Information($"File Name: {Path.GetFileName(gamePath)}");
                (string gameTitle, string gameId, string mediaId) = GameManager.GetGameDetails(gamePath, xeniaVersion); // Get Title, TitleID and MediaID
                Log.Information($"Title: {gameTitle}, Game ID: {gameId}, Media ID: {mediaId}");
                SelectGame selectGame = new SelectGame(gameTitle, gameId, mediaId, gamePath, xeniaVersion);
                selectGame.Show();
                await selectGame.WaitForCloseAsync();
            }
            LoadGames();
        }

        // UI Interactions
        /// <summary>
        /// Opens FileDialog where user selects the game/games they want to add to Xenia Manager
        /// </summary>
        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening file dialog");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a game";
            openFileDialog.Filter = "All Files|*|Supported Files|*.iso;*.xex;*.zar";
            openFileDialog.Multiselect = true;
            bool? result = openFileDialog.ShowDialog();
            if (result == false)
            {
                Log.Information("Cancelling adding of games");
                return;
            }

            // Checking what emulator versions are installed
            List<EmulatorVersion> installedXeniaVersions = new List<EmulatorVersion>();
            if (ConfigurationManager.AppConfig.XeniaStable != null) installedXeniaVersions.Add(EmulatorVersion.Stable);
            if (ConfigurationManager.AppConfig.XeniaCanary != null) installedXeniaVersions.Add(EmulatorVersion.Canary);
            if (ConfigurationManager.AppConfig.XeniaNetplay != null) installedXeniaVersions.Add(EmulatorVersion.Netplay);

            switch (installedXeniaVersions.Count)
            {
                case 0:
                    Log.Information("Xenia has not been installed");
                    break;
                case 1:
                    Log.Information($"Only Xenia {installedXeniaVersions[0]} is installed");
                    // Calls for the function that adds the game into Xenia Manager
                    AddGames(openFileDialog.FileNames, installedXeniaVersions[0]);
                    break;
                default:
                    Log.Information("Detected multiple Xenia installations");
                    Log.Information("Asking user what Xenia version will the game use");
                    XeniaSelection xeniaSelection = new XeniaSelection();
                    xeniaSelection.ShowDialog();
                    Log.Information($"User selected Xenia {xeniaSelection.UserSelection}");
                    AddGames(openFileDialog.FileNames, xeniaSelection.UserSelection);
                    break;
            }
        }
    }
}
