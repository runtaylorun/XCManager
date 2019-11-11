namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Team_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Team_Id");
            AddForeignKey("dbo.AspNetUsers", "Team_Id", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Team_Id", "dbo.Teams");
            DropIndex("dbo.AspNetUsers", new[] { "Team_Id" });
            DropColumn("dbo.AspNetUsers", "Team_Id");
        }
    }
}
