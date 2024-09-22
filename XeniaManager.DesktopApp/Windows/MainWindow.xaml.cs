﻿using System;
using System.Windows;

// Imported
using XeniaManager.DesktopApp.Utilities.Animations;

namespace XeniaManager.DesktopApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // UI Interactions
        /// <summary>
        /// What happens when Exit button is pressed
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            WindowAnimations.ClosingAnimation(this, () => Environment.Exit(0));
        }
    }
}