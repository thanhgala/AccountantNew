namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnForTableComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "CreateDate");
        }
    }
}
