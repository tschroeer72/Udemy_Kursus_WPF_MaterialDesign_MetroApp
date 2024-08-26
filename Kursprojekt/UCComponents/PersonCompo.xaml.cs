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
using WikkiDBLib.Models.ViewModels;

namespace Kursprojekt.UCComponents
{
    /// <summary>
    /// Interaktionslogik für PersonCompo.xaml
    /// </summary>
    public partial class PersonCompo : UserControl
    {
        public PersonCompo(PersonStadtVM personStadtVM)
        {
            InitializeComponent();
            DataContext = personStadtVM;
            ShowStatus(personStadtVM);
        }

        private void ShowStatus(PersonStadtVM personStadtVM)
        {
            lblStatus.Foreground = Brushes.DarkRed;
            if (!personStadtVM.TestAbgeschlossen)
            {
                lblStatus.Content = "Keine Angabe...";
            }
            else
            {
                if (personStadtVM.Infiziert)
                {
                    lblStatus.Content = "Infiziert";
                }
                else
                {
                    lblStatus.Foreground = Brushes.ForestGreen;
                    lblStatus.Content = "Nicht infiziert";
                }
            }
        }
    }
}
