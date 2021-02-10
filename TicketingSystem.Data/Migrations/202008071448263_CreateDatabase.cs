namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileStorage",
                c => new
                    {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                        FileName = c.String(maxLength: 100),
                        Path = c.String(maxLength: 250),
                        Reference = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reply",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticket", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        Description = c.String(),
                        OpenDate = c.DateTime(nullable: false),
                        ClosedDate = c.DateTime(),
                        ClosedBy = c.Int(nullable: false),
                        Client_Id = c.Int(),
                        Employee_Id = c.Int(),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Client_Id)
                .ForeignKey("dbo.User", t => t.Employee_Id)
                .ForeignKey("dbo.User", t => t.Manager_Id)
                .Index(t => t.Client_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Type = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Password = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LoginType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.PhoneNumber, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reply", "TicketId", "dbo.Ticket");
            DropForeignKey("dbo.Ticket", "Manager_Id", "dbo.User");
            DropForeignKey("dbo.Ticket", "Employee_Id", "dbo.User");
            DropForeignKey("dbo.Ticket", "Client_Id", "dbo.User");
            DropIndex("dbo.User", new[] { "PhoneNumber" });
            DropIndex("dbo.User", new[] { "Email" });
            DropIndex("dbo.Ticket", new[] { "Manager_Id" });
            DropIndex("dbo.Ticket", new[] { "Employee_Id" });
            DropIndex("dbo.Ticket", new[] { "Client_Id" });
            DropIndex("dbo.Reply", new[] { "TicketId" });
            DropTable("dbo.User");
            DropTable("dbo.Ticket");
            DropTable("dbo.Reply");
            DropTable("dbo.FileStorage");
        }
    }
}
