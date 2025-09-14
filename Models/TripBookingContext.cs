using System.Data.Entity;

namespace _2024Exam.Models
{
    public class TripBookingContext : DbContext
    {
        public TripBookingContext() : base("obj") { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Trip> Trips { get; set; }

    }
}