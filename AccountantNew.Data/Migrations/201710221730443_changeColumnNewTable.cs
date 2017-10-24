namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeColumnNewTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "ApplicationUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.News", "AuthorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "AuthorID", c => c.String(maxLength: 128));
            DropColumn("dbo.News", "ApplicationUserId");
        }
    }
}
