using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikkiDBLib.Modells;
using WikkiDBLib.Models.ViewModels;

namespace Kursprojekt.Helpers
{
    public class MapperHelper
    {
        public static PersonStadtVM Map_PersonVMToPersonVM(PersonStadtVM iPerosnStadtVM)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PersonStadtVM, PersonStadtVM>());
            var mapper = new Mapper(config);
            return mapper.Map<PersonStadtVM, PersonStadtVM>(iPerosnStadtVM);
        }

        public static Person Map_PersonVMToPerson(PersonStadtVM iPersonStadtVM)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PersonStadtVM, Person>());
            var mapper = new Mapper(config);
            return mapper.Map<PersonStadtVM, Person>(iPersonStadtVM);
        }
    }
}
