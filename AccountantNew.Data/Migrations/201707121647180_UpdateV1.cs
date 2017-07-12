namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateV1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Comments", "UserID");
            CreateIndex("dbo.SystemLogs", "ApplicationUserId");
            AddForeignKey("dbo.Comments", "UserID", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.SystemLogs", "ApplicationUserId", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SystemLogs", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Comments", "UserID", "dbo.ApplicationUsers");
            DropIndex("dbo.SystemLogs", new[] { "ApplicationUserId" });
            DropIndex("dbo.Comments", new[] { "UserID" });
        }
    }
}
