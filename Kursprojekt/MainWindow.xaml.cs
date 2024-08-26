using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Theming;
using Kursprojekt.Helpers;
using Kursprojekt.UserControls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Kursprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ucInfo? _ucInfo;
        private ucVerwaltung? _ucVerwaltung;
        private ucStatistik? _ucStatistik;

        public MainWindow()
        {
            InitializeComponent();
            GlobVar.GlobMainWindow = this;
            //ThemeManager.Current.ChangeTheme(this, "Light.Teal");
        }

        #region Private Methoden
        private void _Init()
        {
            _ucInfo = new();
            _ucVerwaltung = new();
            _ucStatistik = new();

            UCsPlaceholderGrid.Children.Clear();
            UCsPlaceholderGrid.Children.Add(_ucInfo);
        }

        private void _OpenCloseFlyout(int iFlyoutIndex)
        {
            try
            {
                var flyout = this.Flyouts.Items[iFlyoutIndex] as Flyout;
                if (flyout is null) return;

                flyout.IsOpen = !flyout.IsOpen;

            }
            catch (Exception ex)
            {

            }
        }

        public void OpenButtonFlyout(string sInforText)
        {
            lblFlyoutInfo.Content = sInforText;
            _OpenCloseFlyout(1);
        }

        private void _MoveMenuCursor(int iListViewSelectedIndex)
        {
            var ListViewItemHeight = ListViewvItemHome.Height;
            if (iListViewSelectedIndex < 0) return;

            //ohne Animation
            //BorderCursor.Margin = new Thickness(0, 4 + (ListViewItemHeight * iListViewSelectedIndex), 0, 0);

            //Mit Animation
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();
            thicknessAnimation.Duration = TimeSpan.FromMilliseconds(200);
            thicknessAnimation.To = new Thickness(0, 4 + (ListViewItemHeight * iListViewSelectedIndex), 0, 0);
            BorderCursor.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnimation);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _Init();
        }
        #endregion

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
            _OpenCloseFlyout(0);
        }
                
        private void MainMenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StackPanelSuchen.Visibility = Visibility.Collapsed;
            var selectedIndex = MainMenuListView.SelectedIndex;
            _MoveMenuCursor(selectedIndex);
            _OpenCloseFlyout(0);
            TglBtnMenuOpenClose.IsChecked = false;

            switch (selectedIndex)
            {
                case 0:
                    UCsPlaceholderGrid.Children.Clear();
                    UCsPlaceholderGrid.Children.Add(_ucInfo);
                    StackPanelSuchen.Visibility = Visibility.Visible;
                    break;
                case 1:
                    UCsPlaceholderGrid.Children.Clear();
                    UCsPlaceholderGrid.Children.Add(_ucVerwaltung);
                    break;
                case 2:
                    UCsPlaceholderGrid.Children.Clear();
                    UCsPlaceholderGrid.Children.Add(_ucStatistik);
                    break;
                default:
                    break;
            }
        }

        private void MetroWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Flyout? flyout = this.Flyouts.Items[0] as Flyout;
                if (flyout?.IsOpen == true) 
                {
                    flyout.IsOpen = false;
                    TglBtnMenuOpenClose.IsChecked = false;
                }              
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCloseFlyoutInfo_Click(object sender, RoutedEventArgs e)
        {
            _OpenCloseFlyout(1);
        }

        private void txtSearchPerson_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearchPerson.Clear();
            await _ucInfo!.GetPersonsAsync();
        }
    }
}