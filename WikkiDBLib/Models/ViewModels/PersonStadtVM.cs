using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace WikkiDBLib.Models.ViewModels
{
    public class PersonStadtVM
    {
        //DB Properties
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Vorname { get; set; } = string.Empty;
        public bool Infiziert { get; set; }
        public bool TestAbgeschlossen { get; set; }
        public byte[]? Bild { get; set; }
        public int StadtID { get; set; }
        public string Stadt { get; set; }

        //Helper Properties
        public bool NichtInfiziert
        {
            get
            {
                return !Infiziert;
            }
            set
            {
                Infiziert = !value;
            }
        }

        public bool TestNichtAbgeschlossen {
            get
            {
                return !TestAbgeschlossen;
            }
            set
            {
                TestAbgeschlossen = !value;
            }
        }

        public string VornameName => $"{Vorname} {Name}";

        public BitmapImage BitmapImage 
        {
            get 
            { 
                return GetBitmapImage(Bild);
            }
            set
            {
                if (value != null && value.UriSource != null)
                {
                    Bild = File.ReadAllBytes(value.UriSource.OriginalString);
                }
            }
        } 

        //Helper Functions
        private BitmapImage GetBitmapImage(byte[]? iBitByte)
        {
            var img = new BitmapImage();
            if (iBitByte != null)  // && iBitByte.Length > 0
            {
                using (var stream = new MemoryStream(iBitByte))
                {
                    stream.Position = 0;
                    img.BeginInit();
                    img.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.StreamSource = stream;
                    img.EndInit();
                }
            }

            return img;
        }
    }
}
