namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFieldAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "BA", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationUsers", "PCA", c => c.String(maxLength: 50));
            AddColumn("dbo.ApplicationUsers", "NamePCA", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApplicationUsers", "FullName", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationUsers", "FullName", c => c.String(maxLength: 256));
            DropColumn("dbo.ApplicationUsers", "NamePCA");
            DropColumn("dbo.ApplicationUsers", "PCA");
            DropColumn("dbo.ApplicationUsers", "BA");
        }
    }
}
