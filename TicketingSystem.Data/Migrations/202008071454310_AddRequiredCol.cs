namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredCol : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reply", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Ticket", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Ticket", "ClosedBy", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "ClosedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Ticket", "Description", c => c.String());
            AlterColumn("dbo.Reply", "Content", c => c.String());
        }
    }
}
