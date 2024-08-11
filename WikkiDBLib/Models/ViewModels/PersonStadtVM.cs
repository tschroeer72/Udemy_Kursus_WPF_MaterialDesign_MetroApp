using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikkiDBLib.Models.ViewModels
{
    public class PersonStadtVM
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Vorname { get; set; } = string.Empty;
        public bool Infiziert { get; set; }
        public bool TestAbgeschlossen { get; set; }
        public byte[]? Bild { get; set; }
        public int StadtID { get; set; }
        public string Stadt { get; set; }
    }
}
