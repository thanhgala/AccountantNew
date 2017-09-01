namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "TimeStarted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "TimeStarted");
        }
    }
}
