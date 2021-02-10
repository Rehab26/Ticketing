namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserStatusCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserStatus");
        }
    }
}
