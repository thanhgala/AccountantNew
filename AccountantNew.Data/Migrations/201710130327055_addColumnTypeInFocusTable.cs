namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnTypeInFocusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FocusNotifications", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FocusNotifications", "Type");
        }
    }
}
