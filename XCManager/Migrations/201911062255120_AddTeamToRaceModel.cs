namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamToRaceModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Races", "Team_Id", c => c.Int());
            CreateIndex("dbo.Races", "Team_Id");
            AddForeignKey("dbo.Races", "Team_Id", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Races", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Races", new[] { "Team_Id" });
            DropColumn("dbo.Races", "Team_Id");
        }
    }
}
