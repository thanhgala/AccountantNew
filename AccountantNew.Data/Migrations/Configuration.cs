namespace AccountantNew.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AccountantNew.Data.AccountantNewDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AccountantNew.Data.AccountantNewDbContext context)
        {

            //  This method will be called after migrating to the latest version.

            //CreateRole();

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

        }

        private void CreateRole()
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new AccountantNewDbContext()));
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new ApplicationRole { Name = "Category",Description = "Quản lý danh mục" });
                roleManager.Create(new ApplicationRole { Name = "New", Description = "Quản lý bài viết" });
                roleManager.Create(new ApplicationRole { Name = "Group", Description = "Quản lý group" });
                roleManager.Create(new ApplicationRole { Name = "Account", Description = "Quản lý tài khoản" });
            }
        }

        private void CreateProductCategorySample(AccountantNewDbContext context)
        {
            if (context.NewCategories.Count() == 0)
            {
                List<NewCategory> listProductCategory = new List<NewCategory>()
                {
                    new NewCategory() {Name="Tin tức",Alias="tin-tuc",Status = true },
                    new NewCategory() { Name = "Chính sách", Alias = "chinh-sach", Status = true },
                    new NewCategory() { Name = "Pháp luật", Alias = "phap-luat", Status = true },
                    new NewCategory() { Name = "License", Alias = "license", Status = true },
                    new NewCategory() { Name = "Báo cáo", Alias = "bao-cao", Status = true },
                    new NewCategory() { Name = "Hỗ trợ", Alias = "ho-tro", Status = true },
                    new NewCategory() { Name = "Comment", Alias = "comment", Status = true },
                };
                context.NewCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }
        }

        private void CreateChildCategorySample(AccountantNewDbContext context)
        {

            List<NewCategory> listProductCategory = new List<NewCategory>()
            {
                new NewCategory() { Name = "Kế toán", Alias = "ke-toan", Status = true , ParentID = 5},
                new NewCategory() { Name = "Thuế", Alias = "thue", Status = true , ParentID = 5},

                new NewCategory() { Name = "Chuẩn mực kế toán", Alias = "chuan-muc-ke-toan", Status = true , ParentID = 17},
                new NewCategory() { Name = "Quyết định", Alias = "quyet-dinh", Status = true , ParentID = 17},
                new NewCategory() { Name = "Nghị định", Alias = "nghi-dinh", Status = true , ParentID = 17},
                new NewCategory() { Name = "Thông tư", Alias = "thong-tu", Status = true , ParentID = 17},
                new NewCategory() { Name = "Công văn", Alias = "cong-van", Status = true, ParentID = 17 },

                new NewCategory() { Name = "Hóa đơn", Alias = "hoa-don", Status = true, ParentID = 18 },

                new NewCategory() { Name = "Luật", Alias = "luat", Status = true , ParentID = 24},
                new NewCategory() { Name = "Quyết định", Alias = "quyet-dinh", Status = true , ParentID = 24},
                new NewCategory() { Name = "Nghị định", Alias = "nghi-dinh", Status = true , ParentID = 24},
                new NewCategory() { Name = "Thông tư", Alias = "thong-tu", Status = true , ParentID = 24},
                new NewCategory() { Name = "Công văn", Alias = "cong-van", Status = true, ParentID = 24 }
            };
            context.NewCategories.AddRange(listProductCategory);
            context.SaveChanges();
        }
    }
}
