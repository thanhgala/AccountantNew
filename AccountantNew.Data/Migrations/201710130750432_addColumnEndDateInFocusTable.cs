namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnEndDateInFocusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FocusNotifications", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FocusNotifications", "EndDate");
        }
    }
}
