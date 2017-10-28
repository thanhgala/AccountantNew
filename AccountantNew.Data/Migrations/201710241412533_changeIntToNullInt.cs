namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIntToNullInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Organizationals", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Organizationals", "ParentID", c => c.Int(nullable: false));
        }
    }
}
