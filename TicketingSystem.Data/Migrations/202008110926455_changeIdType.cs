namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIdType : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.FileStorage");
            AlterColumn("dbo.FileStorage", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.FileStorage", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.FileStorage");
            AlterColumn("dbo.FileStorage", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.FileStorage", "Id");
        }
    }
}
