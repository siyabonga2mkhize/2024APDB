using _2024Exam.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace _2024Exam.Controllers
{
    public class BookingsController : Controller
    {
        private TripBookingContext db = new TripBookingContext();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Trip);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            ViewBag.TripId = new SelectList(db.Trips, "TripId", "Destination");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,BookingDate,TriEndDate,TripPurpose,FianalPrice,CustomerTypeDiscount,LoyaltyDiscount,DurationDiscount,PurposeDiscount,BookingReference,CustomerId,TripId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                //1
                booking.CustomerTypeDiscount = booking.calcCustomerTypeDiscount();
                booking.LoyaltyDiscount = booking.calcLoyaltyDiscount();
                booking.DurationDiscount = booking.calcTripDurationDiscount();
                //booking.PurposeDiscount = booking.calcPurposeDiscount();

                //2 booking Reference 
                booking.BookingReference = booking.generateBookingReference();

                //3 Final Price
                booking.FinalPrice = booking.calcFinalBookingPrice();

                //Update loyalty points
                booking.AddLoyaltyPoints();

                //5 Add to database and Save
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", booking.CustomerId);
            ViewBag.TripId = new SelectList(db.Trips, "TripId", "Destination", booking.TripId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", booking.CustomerId);
            ViewBag.TripId = new SelectList(db.Trips, "TripId", "Destination", booking.TripId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,BookingDate,TriEndDate,TripPurpose,FianalPrice,CustomerTypeDiscount,LoyaltyDiscount,DurationDiscount,PurposeDiscount,BookingReference,CustomerId,TripId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", booking.CustomerId);
            ViewBag.TripId = new SelectList(db.Trips, "TripId", "Destination", booking.TripId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
