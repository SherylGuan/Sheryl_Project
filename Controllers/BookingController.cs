using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheryl_Project.Data;
using Sheryl_Project.Models;
using System.Diagnostics.Metrics;

namespace Sheryl_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            var booking = _context.Bookings.FirstOrDefault(m => m.BookingID == id);
            if (booking == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            return Ok(booking);
        }

        [HttpGet("{bookingstatus}")]
        public IActionResult GetByStatus(string? bookingstatus = "All")
        {
            switch (bookingstatus.ToLower())
            {
                case "all":
                    return Ok(_context.Bookings);
                case "Pending":
                    return Ok(_context.Bookings.Where(e => e.BookingStatus.ToLower() == "Pending"));
                case "Completed":
                    return Ok(_context.Bookings.Where(e => e.BookingStatus.ToLower() == "Completed"));
                case "Rejected":
                    return Ok(_context.Bookings.Where(e => e.BookingStatus.ToLower() == "Rejected"));
                default:
                    return Problem(detail: "Booking with status " + bookingstatus + " is not found.", statusCode: 404);
            }
        }

        [HttpPost]
        public IActionResult Post(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction("GetAll", new { id = booking.BookingID }, booking);
        }

        [HttpPut]
        public IActionResult Put(int? id, Booking booking)
        {
            var entity = _context.Bookings.FirstOrDefault(m => m.BookingID == id);
            if (entity == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            entity.FacilityDescription = booking.FacilityDescription;
            entity.BookingDateFrom = booking.BookingDateFrom;
            entity.BookingDateTo = booking.BookingDateTo;
            entity.BookedBy = booking.BookedBy;
            entity.BookingStatus = booking.BookingStatus;

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete]
        public IActionResult Delete(int? id, Booking booking)
        {
            var entity = _context.Bookings.FirstOrDefault(m => m.BookingID == id);
            if (entity == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            _context.Bookings.Remove(entity);
            _context.SaveChanges();

            return Ok(entity);
        }


    }
}
