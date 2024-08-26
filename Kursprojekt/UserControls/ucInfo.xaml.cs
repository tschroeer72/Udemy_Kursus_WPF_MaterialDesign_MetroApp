using Kursprojekt.Helpers;
using Kursprojekt.UCComponents;
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
using WikkiDBLib.Models.ViewModels;

namespace Kursprojekt.UserControls
{
    /// <summary>
    /// Interaktionslogik für _ucInfo.xaml
    /// </summary>
    public partial class ucInfo : UserControl
    {
        public ucInfo()
        {
            InitializeComponent();
        }

        public async Task GetPersonsAsync()
        {
            using (new WaitProgressRing(pgRing))
            {
                var PersonListe = await Task.Run(() => DBUnit.Person.GetAll(includeModels: nameof(Stadt)));
                if (PersonListe != null)
                {
                    lstPerson.ItemsSource = AppHelper.GetListPersonStadtVM_from_ListPersonStad(PersonListe);
                }
                ShowCompoOnPlaceholder(new StartCompo());
            }
        }

        public async Task PersonSuchenAsync()
        {

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await GetPersonsAsync();
        }

        private void rpBtnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if (lstPerson.Items.Count <= 0) return;

            if(lstPerson.SelectedIndex< lstPerson.Items.Count - 1)
            {
                lstPerson.SelectedIndex++;
            }
            else
            {
                lstPerson.SelectedIndex = 0;
            }
            lstPerson.ScrollIntoView(lstPerson.SelectedItem);
        }

        private void rpBtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (lstPerson.Items.Count <= 0) return;

            if (lstPerson.SelectedIndex > 0)
            {
                lstPerson.SelectedIndex--;
            }
            else
            {
                lstPerson.SelectedIndex = lstPerson.Items.Count - 1;
            }
            lstPerson.ScrollIntoView(lstPerson.SelectedItem);
        }

        private void ShowCompoOnPlaceholder(UserControl userControl)
        {
            grdPlaceholder.Children.Clear();
            grdPlaceholder.Children.Add(userControl);
        }

        private void lstPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstPerson.SelectedItem != null)
            {
                if(lstPerson.SelectedItem is PersonStadtVM oPersonStadtVM)
                {
                    ShowCompoOnPlaceholder(new PersonCompo(oPersonStadtVM));
                }
            }
        }
    }
}
