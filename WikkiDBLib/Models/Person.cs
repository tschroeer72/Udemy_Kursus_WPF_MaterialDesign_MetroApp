using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikkiDBLib.Modells
{
    public class Person
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Vorname { get; set; }=string.Empty;

        [Required]
        public bool Infiziert { get; set; }

        [Required]
        public bool TestAbgeschlossen { get; set; }

        public byte[]? Bild { get; set; }

        [Required]
        public int StadtID { get; set; }

        [ForeignKey(nameof(StadtID))]
        public Stadt? Stadt { get; set; }
    }
}
