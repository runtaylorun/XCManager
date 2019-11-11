namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulatePhoneNumbers : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Runners SET PhoneNumber = \'574-551-7411\' WHERE Id=1");
            Sql("UPDATE Runners SET PhoneNumber = \'574-267-3325\' WHERE Id=2");
            Sql("UPDATE Runners SET PhoneNumber = \'574-551-8215\' WHERE Id=3");
            Sql("UPDATE Runners SET PhoneNumber = \'574-527-6753\' WHERE Id=6");
            Sql("UPDATE Runners SET PhoneNumber = \'574-551-2994\' WHERE Id=7");
        }
        
        public override void Down()
        {
        }
    }
}
