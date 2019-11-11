namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamToIndividualResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndividualResults", "Team_Id", c => c.Int());
            CreateIndex("dbo.IndividualResults", "Team_Id");
            AddForeignKey("dbo.IndividualResults", "Team_Id", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualResults", "Team_Id", "dbo.Teams");
            DropIndex("dbo.IndividualResults", new[] { "Team_Id" });
            DropColumn("dbo.IndividualResults", "Team_Id");
        }
    }
}
