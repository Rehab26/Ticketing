namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCityCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "City", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "City");
        }
    }
}
