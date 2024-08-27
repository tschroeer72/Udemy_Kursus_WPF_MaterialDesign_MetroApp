using Kursprojekt.AppWindows;
using Kursprojekt.DTOs;
using Kursprojekt.Helpers;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WikkiDBLib.DBAccess;
using WikkiDBLib.Modells;

namespace Kursprojekt.UserControls
{
    /// <summary>
    /// Interaktionslogik für _ucStatistik.xaml
    /// </summary>
    public partial class ucStatistik : UserControl
    {
        private List<Stadt>? m_lstCities = new();

        public ucStatistik()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await InitialLoadAsync();
            ShowData(1, 1, 1);
        }

        private async Task InitialLoadAsync()
        {
            using(new WaitProgressRing(pgRing))
            {
                m_lstCities = await Task.Run(() => DBUnit.Stadt.GetAll()?.ToList());
                lstCities.ItemsSource = m_lstCities;
            }
        }

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            new InfoDialog($"{chartPoint.SeriesView.Title} ({chartPoint.Participation * 100} %)", IWDialogType.Information).ShowDialog();
        }

        private void txtSearchCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtSearchString = txtSearchCity.Text.Trim();
            if (!string.IsNullOrEmpty(txtSearchString))
            {
                lstCities.ItemsSource = m_lstCities?.Where(w => w.Name.Contains(txtSearchString,StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            else
            {
                lstCities.ItemsSource = m_lstCities;
            }                
        }

        private void ShowCartesianChart(int iTotal, int iTotalInfiziert, int iTotalNichtInfiziert)
        {
            var barCol = new SeriesCollection()
            {
                new ColumnSeries()
                {
                    Title = $"Total: {iTotal}",
                    Values = new ChartValues<int>{iTotal },
                    Fill = (Brush)Application.Current.Resources["AppBrushColorBlue"],
                    DataLabels = true,
                    FontSize = 80,
                    MaxColumnWidth = 200,
                    ColumnPadding = 50,
                    Foreground = (Brush)Application.Current.Resources["AppBrushColorBlue"]
                },
                new ColumnSeries()
                {
                    Title = $"Infiziert: {iTotalInfiziert}",
                    Values = new ChartValues<int>{iTotalInfiziert },
                    Fill = Brushes.DarkRed,
                    DataLabels = true,
                    FontSize = 80,
                    MaxColumnWidth = 200,
                    ColumnPadding = 50,
                    Foreground = Brushes.DarkRed,
                },
                new ColumnSeries()
                {
                    Title = $"Nicht infiziert: {iTotalNichtInfiziert}",
                    Values = new ChartValues<int>{iTotalNichtInfiziert },
                    Fill = (Brush)Application.Current.Resources["AppBrushColorCyan"],
                    DataLabels = true,
                    FontSize = 80,
                    MaxColumnWidth = 200,
                    ColumnPadding = 50,
                    Foreground = (Brush)Application.Current.Resources["AppBrushColorCyan"]
                }
            };

            BarChart.Series = barCol;
            CharAxisY.MaxValue = iTotal + 15;
        }

        private void ShowPieChart(int iTotal, int iTotalInfiziert, int iTotalNichtInfiziert)
        {
            var pieCol = new SeriesCollection()
            {
                new PieSeries()
                {
                    Title = $"Infiziert: {iTotalInfiziert}",
                    Values = new ChartValues<int>{iTotalInfiziert },
                    Fill = Brushes.DarkRed,
                    DataLabels = true,
                    FontSize = 80,
                    Foreground = Brushes.White,
                },
                new PieSeries()
                {
                    Title = $"Nicht infiziert: {iTotalNichtInfiziert}",
                    Values = new ChartValues<int>{iTotalNichtInfiziert },
                    Fill = (Brush)Application.Current.Resources["AppBrushColorCyan"],
                    DataLabels = true,
                    FontSize = 80,
                    Foreground = Brushes.White
                }
            };

            PieChart.Series = pieCol;
        }

        private void ShowData(int iTotal, int iTotalInfiziert, int iTotalNichtInfiziert)
        {
            ShowCartesianChart(iTotal, iTotalInfiziert, iTotalNichtInfiziert);
            ShowPieChart(iTotal, iTotalInfiziert, iTotalNichtInfiziert);
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearchCity.Clear();
            await InitialLoadAsync();
        }

        private async void lstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (new WaitProgressRing(pgRingCharts)) 
            {
                var selCity = lstCities.SelectedItem as Stadt;
                var lstPers = await Task.Run(() => DBUnit.Person.GetAll(filter:p => p.StadtID == selCity.ID));
                if (lstPers != null && lstPers.Count() > 0) 
                {
                    int intTotal = lstPers.Count();
                    int intInfiziert = lstPers.Where(w => w.Infiziert == true).Count();
                    int intNichtInfiziert = lstPers.Where(w => w.Infiziert == false).Count();

                    ShowData(intTotal, intInfiziert, intNichtInfiziert);
                }                
            }
        }
    }
}
