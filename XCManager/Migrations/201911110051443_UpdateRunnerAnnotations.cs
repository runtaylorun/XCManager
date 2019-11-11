namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRunnerAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Runners", "Name", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Runners", "Email", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Runners", "PhoneNumber", c => c.String(maxLength: 14));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Runners", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Runners", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Runners", "Name", c => c.String(nullable: false));
        }
    }
}
