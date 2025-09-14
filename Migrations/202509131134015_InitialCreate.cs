namespace _2024Exam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        BookingDate = c.DateTime(nullable: false),
                        TriEndDate = c.DateTime(nullable: false),
                        TripPurpose = c.String(),
                        FianalPrice = c.Double(nullable: false),
                        CustomerTypeDiscount = c.Double(nullable: false),
                        LoyaltyDiscount = c.Double(nullable: false),
                        DurationDiscount = c.Double(nullable: false),
                        PurposeDiscount = c.Double(nullable: false),
                        BookingReference = c.String(),
                        CustomerId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        CustomerType = c.String(),
                        LoyaltyPoints = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        TripId = c.Int(nullable: false, identity: true),
                        Destination = c.String(),
                        BaseFare = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TripId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Bookings", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Bookings", new[] { "TripId" });
            DropIndex("dbo.Bookings", new[] { "CustomerId" });
            DropTable("dbo.Trips");
            DropTable("dbo.Customers");
            DropTable("dbo.Bookings");
        }
    }
}
