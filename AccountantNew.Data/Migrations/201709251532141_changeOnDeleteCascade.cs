namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeOnDeleteCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories");
            AddForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories", "ID");
            AddForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories", "ID");
            AddForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories");
            DropForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories");
            AddForeignKey("dbo.News", "NewCategoryID", "dbo.NewCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Files", "NewCategoryID", "dbo.NewCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories", "ID", cascadeDelete: true);
        }
    }
}
