namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRelationshipWithFluentApi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ticket", "Employee_Id", "dbo.User");
            DropForeignKey("dbo.Ticket", "Manager_Id", "dbo.User");
            DropIndex("dbo.Ticket", new[] { "Client_Id" });
            DropIndex("dbo.Ticket", new[] { "Employee_Id" });
            DropIndex("dbo.Ticket", new[] { "Manager_Id" });
            RenameColumn(table: "dbo.Ticket", name: "Client_Id", newName: "ManagerId");
            AddColumn("dbo.Ticket", "ClientId", c => c.Int(nullable: false));
            AddColumn("dbo.Ticket", "EmployeeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ticket", "ManagerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ticket", "ManagerId");
            DropColumn("dbo.Ticket", "Employee_Id");
            DropColumn("dbo.Ticket", "Manager_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ticket", "Manager_Id", c => c.Int());
            AddColumn("dbo.Ticket", "Employee_Id", c => c.Int());
            DropIndex("dbo.Ticket", new[] { "ManagerId" });
            AlterColumn("dbo.Ticket", "ManagerId", c => c.Int());
            DropColumn("dbo.Ticket", "EmployeeId");
            DropColumn("dbo.Ticket", "ClientId");
            RenameColumn(table: "dbo.Ticket", name: "ManagerId", newName: "Client_Id");
            CreateIndex("dbo.Ticket", "Manager_Id");
            CreateIndex("dbo.Ticket", "Employee_Id");
            CreateIndex("dbo.Ticket", "Client_Id");
            AddForeignKey("dbo.Ticket", "Manager_Id", "dbo.User", "Id");
            AddForeignKey("dbo.Ticket", "Employee_Id", "dbo.User", "Id");
        }
    }
}
