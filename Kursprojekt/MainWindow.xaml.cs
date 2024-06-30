﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Kursprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //ThemeManager.Current.ChangeTheme(this, "Light.Teal");
        }

        public async void BtnMinMaxRestoreOnClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button senderBtn)
            {
                if (senderBtn is null) return;

                if(senderBtn.Name == BtnWindowsMinimize.Name)
                {
                    this.WindowState = WindowState.Minimized;
                    return;
                }

                if (senderBtn.Name == BtnWindowsClose.Name)
                {
                    var dialogSettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ja",
                        NegativeButtonText = "Nein",
                        AnimateShow = true,
                        AnimateHide = true,
                        ColorScheme = MetroDialogColorScheme.Theme
                    };

                    var erg = await this.ShowMessageAsync("ACHTUNG", "Wollen Sie wirklich WIKKI beenden?", MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
                    if(erg == MessageDialogResult.Affirmative)
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
            else
            {
                if(sender is ToggleButton senderTgl)
                {
                    if (senderTgl.IsChecked == true)
                    {
                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                    }
                }
            }
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void TglBtnMenuOpenClose_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseFlyout(0);
        }

        private void OpenCloseFlyout(int iFlyoutIndex)
        {
            try
            {
                var flyout = this.Flyouts.Items[iFlyoutIndex] as Flyout;
                if (flyout is null) return;
                
                flyout.IsOpen = !flyout.IsOpen;

            }
            catch(Exception ex)
            {

            }
        }
    }
}