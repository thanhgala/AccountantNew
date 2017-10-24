namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFKFromNewToAppUser : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.News", "ApplicationUserId");
            AddForeignKey("dbo.News", "ApplicationUserId", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "ApplicationUserId", "dbo.ApplicationUsers");
            DropIndex("dbo.News", new[] { "ApplicationUserId" });
        }
    }
}
