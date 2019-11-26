namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidationToRaceModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Races", "RaceName", c => c.String(nullable: false, maxLength: 55));
            AlterColumn("dbo.Races", "Distance", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Races", "Location", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Races", "Location", c => c.String(nullable: false));
            AlterColumn("dbo.Races", "Distance", c => c.String(nullable: false));
            AlterColumn("dbo.Races", "RaceName", c => c.String(nullable: false));
        }
    }
}
