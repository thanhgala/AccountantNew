namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Path", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Path");
        }
    }
}
