namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeViewCountInPost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "ViewCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "ViewCount", c => c.Int());
        }
    }
}
