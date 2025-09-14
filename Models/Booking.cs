using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace _2024Exam.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime TriEndDate { get; set; }
        public string TripPurpose { get; set; }
        public double FinalPrice { get; set; }
        public double CustomerTypeDiscount { get; set; }
        public double LoyaltyDiscount { get; set; }
        public double DurationDiscount { get; set; }
        public double PurposeDiscount { get; set; }
        public string BookingReference { get; set; }
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public virtual Trip Trip { get; set; }
        public int TripId { get; set; }


        public double getTripBaseFare()
        {
            // Create an instance of the DbContext
            TripBookingContext db = new TripBookingContext();

            // Use a LINQ query to find the BaseFare for the trip
            var tripBaseFare = (from t in db.Trips
                                where t.TripId == this.TripId // 'this.TripId' refers to the TripId of the current Booking object
                                select t.BaseFare).SingleOrDefault();

            // The query returns a double, so you can return it directly.
            return tripBaseFare;
        }
        public double calcCustomerTypeDiscount()
        {
            // Create an instance of the DbContext
            TripBookingContext db = new TripBookingContext();
            // Use a LINQ query to find the CustomerType for the customer
            var customerType = (from c in db.Customers
                                where c.CustomerId == this.CustomerId // 'this.CustomerId' refers to the CustomerId of the current Booking object
                                select c.CustomerType).SingleOrDefault();
            if (customerType == "Regular")
            {
                CustomerTypeDiscount = 0.0;
            }
            else if (customerType == "Premium")
            {
                CustomerTypeDiscount = 0.11; // 10% discount for Premium customers
            }
            else if (customerType == "VIP")
            {
                CustomerTypeDiscount = 0.05; // 5% discount for VIP customers
            }
            return CustomerTypeDiscount;
        }
        public double calcTripDurationDiscount()
        {

            TimeSpan duration = TriEndDate - BookingDate;
            int days = duration.Days;
            if (days >= 10)
            {
                DurationDiscount = 0.05; // 5% discount for trips between 7 and 14 days
            }
            return DurationDiscount;
        }
        public double AddLoyaltyPoints()
        {
            //AddLoyalty Points() is the method that calculates loyalty points earned by the customer for each trip booking. Each trip earns a customer one point.
            TripBookingContext db = new TripBookingContext();
            var customer = db.Customers.Find(this.CustomerId);
            if (customer != null)
            {
                customer.LoyaltyPoints += 1; // Each trip earns a customer one point
                db.SaveChanges(); // Save changes to the database
                return customer.LoyaltyPoints; // Return updated loyalty points
            }
            return customer.LoyaltyPoints;
        }
        public double calcLoyaltyDiscount()
        {
            //calcLoyalty PointsDiscount() is the method that will check if the customer has more than 5 completed trips and apply a 5% discount to the base fare.
            TripBookingContext db = new TripBookingContext();
            var customer = db.Customers.Find(this.CustomerId);
            if (customer != null && customer.LoyaltyPoints > 5)
            {
                LoyaltyDiscount = 0.05; // 5% discount for customers with more than 5 completed trips
            }
            return LoyaltyDiscount;
        }

        // generate BookingReference() is the method that generates a unique booking reference number for each booking. It should combine the CustomerId, TripId, and the current booking timestamp to ensure that the reference is unique and easily traceable.
        public string generateBookingReference()
        {
            // Generate a unique booking reference using CustomerId, TripId, and current timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            BookingReference = $"C{CustomerId}-T{TripId}-{timestamp}";
            return BookingReference;
        }

        //calc FinalBooking Price() is the method that calculates the final price of a booking after all elevant discounts have been applied.
        public double calcFinalBookingPrice()
        {
            double baseFare = getTripBaseFare();
            double totalDiscount = CustomerTypeDiscount + LoyaltyDiscount + DurationDiscount + PurposeDiscount;
            FinalPrice = baseFare * (1 - totalDiscount);
            return FinalPrice;
        }
    }

}