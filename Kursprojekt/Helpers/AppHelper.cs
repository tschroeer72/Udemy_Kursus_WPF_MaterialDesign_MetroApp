using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikkiDBLib.Modells;
using WikkiDBLib.Models.ViewModels;

namespace Kursprojekt.Helpers
{
    public class AppHelper
    {
        public static List<PersonStadtVM> GetListPersonStadtVM_from_ListPersonStad(IEnumerable<Person> iPersonList)
        {
            var list = new List<PersonStadtVM>();

            if (iPersonList != null)
            {
                foreach (var person in iPersonList) 
                {
                    var personStadtVM = new PersonStadtVM()
                    {
                        ID = person.ID,
                        Name = person.Name,
                        Vorname = person.Vorname,
                        Infiziert = person.Infiziert,
                        TestAbgeschlossen = person.TestAbgeschlossen,
                        Bild = person.Bild,
                        StadtID = person.StadtID,
                        Stadt = person.Stadt.Name
                    };
                    list.Add(personStadtVM);
                }
            }

            return list;
        }
    }
}
