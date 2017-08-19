namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Content");
        }
    }
}
