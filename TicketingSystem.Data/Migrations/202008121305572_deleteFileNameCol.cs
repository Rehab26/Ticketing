namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteFileNameCol : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FileStorage", "FileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileStorage", "FileName", c => c.String(maxLength: 100));
        }
    }
}
