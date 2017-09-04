namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnDesIntoFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Describtion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Describtion");
        }
    }
}
