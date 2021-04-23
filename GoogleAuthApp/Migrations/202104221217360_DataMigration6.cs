namespace GoogleAuthApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration6 : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Photo");
            DropColumn("dbo.AspNetUsers", "BirthDay");
        }
    }
}
