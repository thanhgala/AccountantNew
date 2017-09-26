namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOndeleteCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationCateGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationCateGroups", "CategoryId", "dbo.NewCategories");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.NewTags", "NewID", "dbo.News");
            DropForeignKey("dbo.NewTags", "TagID", "dbo.Tags");
            AddForeignKey("dbo.ApplicationCateGroups", "GroupId", "dbo.ApplicationGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationCateGroups", "CategoryId", "dbo.NewCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "PostID", "dbo.Posts", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NewTags", "NewID", "dbo.News", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NewTags", "TagID", "dbo.Tags", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.NewTags", "NewID", "dbo.News");
            DropForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationCateGroups", "CategoryId", "dbo.NewCategories");
            DropForeignKey("dbo.ApplicationCateGroups", "GroupId", "dbo.ApplicationGroups");
            AddForeignKey("dbo.NewTags", "TagID", "dbo.Tags", "ID");
            AddForeignKey("dbo.NewTags", "NewID", "dbo.News", "ID");
            AddForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories", "ID");
            AddForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories", "ID");
            AddForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories", "ID");
            AddForeignKey("dbo.Comments", "PostID", "dbo.Posts", "ID");
            AddForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups", "ID");
            AddForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles", "Id");
            AddForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups", "ID");
            AddForeignKey("dbo.ApplicationCateGroups", "CategoryId", "dbo.NewCategories", "ID");
            AddForeignKey("dbo.ApplicationCateGroups", "GroupId", "dbo.ApplicationGroups", "ID");
        }
    }
}
