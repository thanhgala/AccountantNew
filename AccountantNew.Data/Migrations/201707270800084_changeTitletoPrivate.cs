namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTitletoPrivate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Private", c => c.Boolean());
            DropColumn("dbo.News", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "Title", c => c.String(maxLength: 500));
            DropColumn("dbo.News", "Private");
        }
    }
}
