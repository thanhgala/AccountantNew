namespace AccountantNew.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnRoleGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationRoleGroups", "CanCreate", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationRoleGroups", "CanRead", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationRoleGroups", "CanUpdate", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationRoleGroups", "CanDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationRoleGroups", "CanDelete");
            DropColumn("dbo.ApplicationRoleGroups", "CanUpdate");
            DropColumn("dbo.ApplicationRoleGroups", "CanRead");
            DropColumn("dbo.ApplicationRoleGroups", "CanCreate");
        }
    }
}
