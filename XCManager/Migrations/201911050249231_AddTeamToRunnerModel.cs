namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamToRunnerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Runners", "Team_Id", c => c.Int());
            CreateIndex("dbo.Runners", "Team_Id");
            AddForeignKey("dbo.Runners", "Team_Id", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Runners", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Runners", new[] { "Team_Id" });
            DropColumn("dbo.Runners", "Team_Id");
        }
    }
}
