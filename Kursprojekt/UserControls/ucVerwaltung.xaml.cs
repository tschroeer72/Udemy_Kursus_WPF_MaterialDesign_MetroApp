using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace Kursprojekt.UserControls
{
    /// <summary>
    /// Interaktionslogik für _ucVerwaltung.xaml
    /// </summary>
    public partial class ucVerwaltung : UserControl
    {
        private string _SelectedFilepath = string.Empty;

        public ucVerwaltung()
        {
            InitializeComponent();

            Init();
        }

        private async void Init()
        {
            rbNichtinfiziert.IsChecked = true;
            rbNichtabgeschlossen.IsChecked = true;

            rbEditNichtinfiziert.IsChecked = true;
            rbEditNichtabgeschlossen.IsChecked = true;

            ShowTabPage(BtnTabAdd);

            var cities = await Task.Run(() => DBUnit.Stadt.GetAll());

            lstCities.ItemsSource = cities;
            cboAddStadt.ItemsSource = cities;
            cboEditStadt.ItemsSource = cities;            
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

        private void canImageDragDrop_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if ((files != null) && (files.Length > 0))
                {
                    if (files.Length > 1)
                    {
                        MessageBox.Show("Nur ein Bild erlaubt!");
                        return;
                    }
                }

                var myFotoFile = files[0];
                var fileInfo = new FileInfo(myFotoFile);
                if (fileInfo.Exists)
                {
                    bool isFileTypeOK = (fileInfo.Extension == ".png") || fileInfo.Extension == ".jpg";
                    if (isFileTypeOK)
                    {
                        _SelectedFilepath = fileInfo.FullName;
                        var img = new BitmapImage(new Uri(fileInfo.FullName));
                        imgAdd.Source = img;
                    }
                }
            }
            catch (Exception ex) 
            { 
            
            }
            
        }

        private void AddImageToControl(Image iImage)
        {
            try
            {
                var fileDialog = new OpenFileDialog()
                {
                    Filter = "Imagefiles | *.png; *jpg",
                    Multiselect = false
                };
                
                var erg = fileDialog.ShowDialog();
                if(erg == true && erg.HasValue)
                {
                    _SelectedFilepath = fileDialog.FileName;
                    var img = new BitmapImage(new Uri(_SelectedFilepath));
                    iImage.Source = img;
                }
            }
            catch (Exception ex) 
            { 
            
            }
        }

        private void BtnAddImage_Click(object sender, RoutedEventArgs e)
        {
            //AddImageToControl((Button)sender);
            var myBtn = sender as Button;
            if (myBtn != null)
            {
                if (myBtn == BtnAddImage)
                {
                    AddImageToControl(imgAdd);
                }
                else
                {
                    AddImageToControl(imgEdit);
                }                        
            }
        }
    }
}
