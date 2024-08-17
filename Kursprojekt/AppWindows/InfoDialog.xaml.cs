using Kursprojekt.DTOs;
using Kursprojekt.Helpers;
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
using System.Windows.Shapes;

namespace Kursprojekt.AppWindows
{
    /// <summary>
    /// Interaktionslogik für InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : Window
    {
        private string _Message { get; set; } = string.Empty;
        private IWDialogType _DialogType { get; set; } = new();
        public string DialogInputText { get; set; } = string.Empty;

        public InfoDialog(string iMessage, IWDialogType eDialogType)
        {
            InitializeComponent();

            Owner = GlobVar.GlobMainWindow;

            _Message = iMessage;
            _DialogType = eDialogType;

            grdJaNein.Visibility = Visibility.Collapsed;
            grdOKCancel.Visibility = Visibility.Collapsed;
            txtInputText.Visibility = Visibility.Collapsed;
            BorderInputText.Visibility = Visibility.Collapsed;

            txtMessage.Text = _Message;
            txtInputText.Clear();

            switch (_DialogType)
            {
                case IWDialogType.Information:
                    lblTitle.Content = "Information";
                    grdOKCancel.Visibility = Visibility.Visible;
                    btnAbbrechen.Visibility = Visibility.Collapsed;
                    break;
                case IWDialogType.Bestätigen:
                    lblTitle.Content = "Bestätigen";
                    grdJaNein.Visibility = Visibility.Visible;
                    break;
                case IWDialogType.Warnung:
                    lblTitle.Content = "Warnung";
                    txtMessage.Foreground = Brushes.DarkRed;
                    grdOKCancel.Visibility = Visibility.Visible;
                    btnAbbrechen.Visibility = Visibility.Collapsed;
                    break;
                case IWDialogType.Texteingabe:
                    lblTitle.Content = "Texteingabe";
                    BorderInputText.Visibility = Visibility.Visible;
                    grdOKCancel.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnNein_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnJa_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void grdTitle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void txtInputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            DialogInputText = txtInputText.Text;
        }
    }
}
