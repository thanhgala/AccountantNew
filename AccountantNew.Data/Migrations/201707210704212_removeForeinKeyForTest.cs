namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeForeinKeyForTest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "AuthorID", "dbo.ApplicationUsers");
            DropIndex("dbo.News", new[] { "AuthorID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.News", "AuthorID");
            AddForeignKey("dbo.News", "AuthorID", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
        }
    }
}
