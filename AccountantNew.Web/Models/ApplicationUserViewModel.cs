﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { set; get; }

        public string FullName { set; get; }

        public string Address { set; get; }

        public DateTime BirthDay { set; get; }

        public string Avartar { set; get; }

        public string IdentityCard { set; get; }

        public string Email { set; get; }

        public string UserName { set; get; }

        public string PhoneNumber { set; get; }

        public string Department { set; get; }

        public int BA { set; get; }

        public double PCA { set; get; }

        public string NamePCA { set; get; }

        public IEnumerable<ApplicationGroupViewModel> Groups { set; get; }
    }
}