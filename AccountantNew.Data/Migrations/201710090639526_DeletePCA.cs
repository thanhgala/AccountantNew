namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletePCA : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationUsers", "PCA");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "PCA", c => c.String(maxLength: 50));
        }
    }
}
