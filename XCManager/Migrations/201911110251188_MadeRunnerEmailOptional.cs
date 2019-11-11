namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeRunnerEmailOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Runners", "Email", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Runners", "Email", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
