namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FluentApiRelations : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ticket", "ClientId");
            RenameColumn(table: "dbo.Ticket", name: "ManagerId", newName: "ClientId");
            RenameIndex(table: "dbo.Ticket", name: "IX_ManagerId", newName: "IX_ClientId");
            AlterColumn("dbo.Ticket", "EmployeeId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "EmployeeId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Ticket", name: "IX_ClientId", newName: "IX_ManagerId");
            RenameColumn(table: "dbo.Ticket", name: "ClientId", newName: "ManagerId");
            AddColumn("dbo.Ticket", "ClientId", c => c.Int(nullable: false));
        }
    }
}
