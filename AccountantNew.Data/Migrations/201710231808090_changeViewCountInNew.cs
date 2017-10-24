namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeViewCountInNew : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "ViewCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "ViewCount", c => c.Int());
        }
    }
}
