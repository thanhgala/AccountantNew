namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTypeDepartment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationUsers", "Department", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationUsers", "Department", c => c.String(maxLength: 256));
        }
    }
}
