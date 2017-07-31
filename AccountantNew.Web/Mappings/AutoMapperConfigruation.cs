using AccountantNew.Model.Models;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Mappings
{
    public class AutoMapperConfigruation
    {
        public static void Configure()
        {
            Mapper.Initialize(cg =>
            {
                cg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();

                cg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();

                cg.CreateMap<ApplicationUser, ApplicationUserViewModel>();

                cg.CreateMap<NewCategory, NewCategoryViewModel>();

                cg.CreateMap<NewCategory, TreeViewCategory<NewCategoryViewModel>>();

                cg.CreateMap<New, NewViewModel>();
            });
        }
    }
}