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
        public static PersonStadtVM Map_PersonVMToPersonVM(PersonStadtVM iPersonStadtVM)
        {
            var config1 = new MapperConfiguration(cfg => cfg.CreateMap<PersonStadtVM, PersonStadtVM>());
            var mapper1 = new Mapper(config1);
            return mapper1.Map<PersonStadtVM, PersonStadtVM>(iPersonStadtVM);
        }

        public static Person Map_PersonVMToPerson(PersonStadtVM iPersonStadtVM2)
        {
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<PersonStadtVM, Person>().ForMember(dest => dest.Stadt, opt => opt.Ignore()));
            var mapper2 = new Mapper(config2);
            return mapper2.Map<PersonStadtVM, Person>(iPersonStadtVM2);
        }
    }
}
