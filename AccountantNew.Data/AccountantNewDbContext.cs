using Microsoft.AspNet.Identity.EntityFramework;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AccountantNew.Data
{
    public class AccountantNewDbContext : IdentityDbContext<ApplicationUser>
    {
        public AccountantNewDbContext() : base("AccountantNewConnection")
        {
            //load bảng cha không include thêm bảng con
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Comment> Comments { set; get; }

        public DbSet<Page> Pages { set; get; }

        public DbSet<New> News { set; get; }

        public DbSet<File> Files { set; get; }

        public DbSet<NewCategory> NewCategories { set; get; }

        public DbSet<NewTag> NewTags { set; get; }

        public DbSet<SupportOnline> SupportOnlines { set; get; }

        public DbSet<Tag> Tags { set; get; }

        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }

        public DbSet<ContactDetail> ContactDetails { set; get; }

        public DbSet<Feedback> Feedbacks { set; get; }

        public DbSet<SystemLog> SystemLogs { set; get; }

        public DbSet<Error> Errors { set; get; }

        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }

        public static AccountantNewDbContext Create()
        {
            return new AccountantNewDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }
    }
}
