using Kursprojekt.AppWindows;
using Kursprojekt.DTOs;
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
using WikkiDBLib.Models.ViewModels;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Kursprojekt.UserControls
{
    /// <summary>
    /// Interaktionslogik für _ucVerwaltung.xaml
    /// </summary>
    public partial class ucVerwaltung : UserControl
    {
        private string? _SelectedFilepath = null;
        private PersonStadtVM? _SelectedPerson = null;
        private List<Stadt>? _AllCities = null;

        public ucVerwaltung()
        {
            InitializeComponent();
        }

        private async void GetAllAndShowCitiesData()
        {
            using (new WaitProgressRing(pgRing))
            {
                _AllCities = await Task.Run(() => DBUnit.Stadt.GetAll()?.ToList());

                lstCities.ItemsSource = _AllCities;
                cboAddStadt.ItemsSource = _AllCities;
                cboEditStadt.ItemsSource = _AllCities;
            }
        }

        private async void GetAllAndShowPersonsData()
        {
            using (new WaitProgressRing(pgRing))
            {
                var persons = await Task.Run(() => DBUnit.Person.GetAll(includeModels: nameof(Stadt)));
                var lstPersonStadt = AppHelper.GetListPersonStadtVM_from_ListPersonStad(persons);
                if (lstPersonStadt != null)
                {
                    dgPerson.ItemsSource = lstPersonStadt;
                }
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
        }

        private Person GetPersonToAddData()
        {
            var person = new Person();
            person.Name = txtAddName.Text.Trim();
            person.Vorname = txtAddVorname.Text.Trim();
            person.StadtID = (cboAddStadt.SelectedIndex < 0) ? -1 : (int)(cboAddStadt.SelectedItem as Stadt)?.ID;
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
            _SelectedFilepath = string.Empty;

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

            _SelectedPerson = null;
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
                        GlobVar.GlobMainWindow?.OpenButtonFlyout($"{personToAdd.Name} wurde hinzugefügt");
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

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = txtSearch.Text.Trim();
            IEnumerable<Person>? lstPerson;
            using (new WaitProgressRing(pgRing)) 
            { 
                if(text != string.Empty)
                {
                    lstPerson = await Task.Run(() => DBUnit.Person.GetAll(filter: p => p.Name.Contains(text) || p.Vorname.Contains(text), 
                                                                            includeModels: "Stadt")                                                                        );
                }
                else
                {
                    lstPerson = await Task.Run(() => DBUnit.Person.GetAll(includeModels: "Stadt"));
                }

                if (lstPerson != null) 
                {
                    var lstPersonStadt = AppHelper.GetListPersonStadtVM_from_ListPersonStad(lstPerson);
                    dgPerson.ItemsSource = lstPersonStadt;
                }
            }
        }

        private void dgPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItemEdit.IsSelected = false;
            TabItemEdit.IsSelected = true;
            ShowTabBtnCursor(BtnEditPerson);
            ClearAllValidationInfos();

            if (dgPerson.SelectedItem != null) 
            {
                var selectedPerson = dgPerson.SelectedItem as PersonStadtVM;

                if (selectedPerson != null)
                {
                    _SelectedPerson = MapperHelper.Map_PersonVMToPersonVM(selectedPerson);

                    if (_SelectedPerson != null)
                    {
                        grdPersonEdit.DataContext = _SelectedPerson;
                        cboEditStadt.SelectedItem = _AllCities?.Where(s => s.ID == _SelectedPerson.StadtID).FirstOrDefault();
                    }
                }
            }

        }

        private void cboEditStadt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStadt = cboEditStadt.SelectedItem as Stadt;
            if (selectedStadt != null)
            {
                if (_SelectedPerson != null)
                {
                    _SelectedPerson.StadtID = selectedStadt.ID;
                    _SelectedPerson.Stadt = selectedStadt.Name;
                }
            }                      
        }

        private void BtnDelPerson_Click(object sender, RoutedEventArgs e)
        {
            if (_SelectedPerson != null)
            {
                var persName = _SelectedPerson.Name;
                var persID = _SelectedPerson.ID;

                if (new InfoDialog($"Wollen Sie {persName} wirklich löschen?", IWDialogType.Bestätigen).ShowDialog() == true)
                {
                    DBUnit.Person.DeleteByID(persID);
                    GetAllAndShowPersonsData();
                    ClearAllValidationInfos();
                    ClearAllControls();

                    GlobVar.GlobMainWindow?.OpenButtonFlyout($"{persName} wurde gelöscht");
                }
            }
            else
            {
                new InfoDialog("Bitte wählen Sie eine Person aus!", IWDialogType.Information).ShowDialog();
            }
        }
    }
}
