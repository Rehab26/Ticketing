namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileStorage", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileStorage", "CreateDate");
        }
    }
}
