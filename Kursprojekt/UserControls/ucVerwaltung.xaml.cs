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

namespace Kursprojekt.UserControls
{
    /// <summary>
    /// Interaktionslogik für _ucVerwaltung.xaml
    /// </summary>
    public partial class ucVerwaltung : UserControl
    {
        public ucVerwaltung()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            ShowTabPage(BtnTabAdd);
        }

        private void ShowTabPage(Button iSender)
        {
            ShowTabBtnCursor(iSender);

            if (iSender == BtnTabAdd)
            {
                TabItemAdd.IsSelected = true;
            }
            else if(iSender == BtnTabEdit)
            {
                TabItemEdit.IsSelected = true;
            } 
            else if (iSender == BtnTabCity)
            {
                TabItemCity.IsSelected = true;
            }
        }

        private void ShowTabBtnCursor(Button iSender)
        {
            RctglBtnTabAdd.Fill = Brushes.Transparent;
            RctglBtnTabEdit.Fill = Brushes.Transparent;
            RctglBtnTabCity.Fill = Brushes.Transparent;

            if (iSender == BtnTabAdd)
            {
                RctglBtnTabAdd.Fill = (Brush)Application.Current.Resources["AppBrushColorCyan"];
            }
            else if (iSender == BtnTabEdit)
            {
                RctglBtnTabEdit.Fill = (Brush)Application.Current.Resources["AppBrushColorCyan"];
            }
            else if (iSender == BtnTabCity)
            {
                RctglBtnTabCity.Fill = (Brush)Application.Current.Resources["AppBrushColorCyan"];
            }
        }

        private void BtnTabOnClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button MyBtn)
            {
                if (MyBtn is null) return;
                ShowTabPage(MyBtn);
            }
        }
    }
}
