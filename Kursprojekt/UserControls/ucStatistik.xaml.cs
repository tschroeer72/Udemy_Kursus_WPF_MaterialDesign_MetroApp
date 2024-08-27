using Kursprojekt.Helpers;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
