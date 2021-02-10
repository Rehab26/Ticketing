namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReplyAndUserRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reply", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reply", "UserId");
            AddForeignKey("dbo.Reply", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reply", "UserId", "dbo.User");
            DropIndex("dbo.Reply", new[] { "UserId" });
            DropColumn("dbo.Reply", "UserId");
        }
    }
}
