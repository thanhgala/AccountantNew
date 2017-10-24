using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(256)]
        [Required]
        public string FullName { set; get; }

        [MaxLength(256)]
        public string Address { set; get; }

        public DateTime BirthDay { set; get; }

        [MaxLength(256)]
        public string Avartar { set; get; }

        [MaxLength(50)]
        public string IdentityCard { set; get; }

        [MaxLength(100)]
        public string Department { set; get; }

        public int BA { set; get; }

        public double PCA { set; get; }

        [MaxLength(100)]
        public string NamePCA { set; get; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            //chỉ định ra quản lý identity thông qua cookie
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        //public virtual IEnumerable<New> New { set; get; }
    }
}
