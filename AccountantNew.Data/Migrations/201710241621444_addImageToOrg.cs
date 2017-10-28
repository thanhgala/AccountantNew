namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageToOrg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizationals", "Image", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizationals", "Image");
        }
    }
}
