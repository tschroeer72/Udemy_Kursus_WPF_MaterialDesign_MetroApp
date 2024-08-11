using Kursprojekt.Helpers;
using Kursprojekt.Validators;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WikkiDBLib.DBAccess;
using WikkiDBLib.Modells;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Kursprojekt.UserControls
{
    /// <summary>
    /// Interaktionslogik für _ucVerwaltung.xaml
    /// </summary>
    public partial class ucVerwaltung : UserControl
    {
        private string? _SelectedFilepath = null;

        public ucVerwaltung()
        {
            InitializeComponent();
        }

        private async void GetAllAndShowCitiesData()
        {
            using (new WaitProgressRing(pgRing))
            {
                var cities = await Task.Run(() => DBUnit.Stadt.GetAll());

                lstCities.ItemsSource = cities;
                cboAddStadt.ItemsSource = cities;
                cboEditStadt.ItemsSource = cities;
            }
        }

        private async void GetAllAndShowPersonsData()
        {
            using (new WaitProgressRing(pgRing))
            {
                var persons = await Task.Run(() => DBUnit.Person.GetAll(includeModels: nameof(Stadt)));

                dgPerson.ItemsSource = persons;
            }
        }

        private void Init()
        {
            rbNichtinfiziert.IsChecked = true;
            rbNichtabgeschlossen.IsChecked = true;

            rbEditNichtinfiziert.IsChecked = true;
            rbEditNichtabgeschlossen.IsChecked = true;

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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetAllAndShowCitiesData();
            GetAllAndShowPersonsData();
        }

        private void ClearAllValidationInfos()
        {
            //Add TabPage
            txtAddNameValidationInfo.Text = string.Empty;
            txtAddVornameValidationInfo.Text = string.Empty;
            cboAddStadtValidationInfo.Text = string.Empty;

            //Edit TabPage
            txtEditNameValidationInfo.Text = string.Empty;
            txtEditVornameValidationInfo.Text = string.Empty;
            cboEditStadtValidationInfo.Text = string.Empty;

            _SelectedFilepath = string.Empty;
        }

        private Person GetPersonToAddData()
        {
            var person = new Person();
            person.Name = txtAddName.Text.Trim();
            person.Vorname = txtAddVorname.Text.Trim();
            person.StadtID = (cboAddStadt.SelectedIndex < 0) ? -1 : (int)cboAddStadt.SelectedValue;
            person.Bild = (_SelectedFilepath is not null) ? File.ReadAllBytes(_SelectedFilepath) : null;
            person.Infiziert = rbInfiziert.IsChecked == true ? true : false;
            person.TestAbgeschlossen = rbAbgeschlossen.IsChecked == true ? true : false;

            return person;
        }

        private void ShowValidationInfos(ValidationResult IValidationResult)
        {
            foreach (var err in IValidationResult.Errors)
            {
                if(err.PropertyName == nameof(Person.Name))
                {
                    txtAddNameValidationInfo.Text = err.ErrorMessage;
                }

                if (err.PropertyName == nameof(Person.Vorname))
                {
                    txtAddVornameValidationInfo.Text = err.ErrorMessage;
                }

                if (err.PropertyName == nameof(Person.StadtID))
                {
                    cboAddStadtValidationInfo.Text = err.ErrorMessage;
                }

                if (err.PropertyName == nameof(Person.Bild))
                {
                    //
                }
            }
        }

        private void ClearAllControls()
        {
            //Add
            txtAddName.Clear();
            txtAddVorname.Clear();
            cboAddStadt.SelectedIndex = -1;
            rbInfiziert.IsChecked = true;
            rbNichtabgeschlossen.IsChecked = true;

            //Edit
            txtEditName.Clear();
            txtEditVorname.Clear();
            cboEditStadt.SelectedIndex = -1;
            rbEditInfiziert.IsChecked = true;
            rbEditNichtabgeschlossen.IsChecked = true;
        }

        private async void BtnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            //Validationinfo leeren
            ClearAllValidationInfos();

            //Daten aus Controls holen
            var personToAdd = GetPersonToAddData();

            //Daten validieren
            var personValidator = new AddNewPersonValidator();
            ValidationResult valResult = personValidator.Validate(personToAdd);
            if (valResult.IsValid)
            {
                using (new WaitProgressRing(pgRing))
                {
                    //Daten speichern
                    var isOK = await Task.Run(() => DBUnit.Person.Add(personToAdd));
                    if (isOK)
                    {
                        //Ausgabe
                        GetAllAndShowPersonsData();
                        ClearAllControls();
                    }                  
                }
            }
            else
            {
                ShowValidationInfos(valResult);
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }
}
