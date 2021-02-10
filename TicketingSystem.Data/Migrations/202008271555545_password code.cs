namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passwordcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "ResetPasswordCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "ResetPasswordCode");
        }
    }
}
