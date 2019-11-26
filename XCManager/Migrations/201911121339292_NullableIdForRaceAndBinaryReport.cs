namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableIdForRaceAndBinaryReport : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RaceReportBinaries", "RaceId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RaceReportBinaries", "RaceId", c => c.Int(nullable: false));
        }
    }
}
