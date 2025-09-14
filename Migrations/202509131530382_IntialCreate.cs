namespace _2024Exam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "FinalPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Bookings", "FianalPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "FianalPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Bookings", "FinalPrice");
        }
    }
}
