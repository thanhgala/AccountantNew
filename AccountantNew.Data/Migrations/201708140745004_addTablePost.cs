namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTablePost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "NewsID", "dbo.News");
            DropIndex("dbo.Comments", new[] { "NewsID" });
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        NewCategoryID = c.Int(nullable: false),
                        ViewCount = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NewCategories", t => t.NewCategoryID, cascadeDelete: true)
                .Index(t => t.NewCategoryID);
            
            AddColumn("dbo.Comments", "PostID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "PostID");
            AddForeignKey("dbo.Comments", "PostID", "dbo.Posts", "ID", cascadeDelete: true);
            DropColumn("dbo.Comments", "NewsID");
            DropColumn("dbo.News", "CommentCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "CommentCount", c => c.Int());
            AddColumn("dbo.Comments", "NewsID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Posts", "NewCategoryID", "dbo.NewCategories");
            DropIndex("dbo.Posts", new[] { "NewCategoryID" });
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropColumn("dbo.Comments", "PostID");
            DropTable("dbo.Posts");
            CreateIndex("dbo.Comments", "NewsID");
            AddForeignKey("dbo.Comments", "NewsID", "dbo.News", "ID", cascadeDelete: true);
        }
    }
}
