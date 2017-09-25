namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createApplicationCateGroupTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationCateGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.CategoryId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId)
                .ForeignKey("dbo.NewCategories", t => t.CategoryId)
                .Index(t => t.GroupId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationCateGroups", "CategoryId", "dbo.NewCategories");
            DropForeignKey("dbo.ApplicationCateGroups", "GroupId", "dbo.ApplicationGroups");
            DropIndex("dbo.ApplicationCateGroups", new[] { "CategoryId" });
            DropIndex("dbo.ApplicationCateGroups", new[] { "GroupId" });
            DropTable("dbo.ApplicationCateGroups");
        }
    }
}
