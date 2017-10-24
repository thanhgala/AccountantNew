namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPCA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "PCA", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "PCA");
        }
    }
}
