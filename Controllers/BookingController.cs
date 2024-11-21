using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheryl_Project.Data;
using Sheryl_Project.Models;
using System.Diagnostics.Metrics;

namespace Sheryl_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Booking);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            var booking = _context.Booking.FirstOrDefault(m => m.BookingID == id);
            if (booking == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            return Ok(booking);
        }

        [HttpPost]
        public IActionResult Post(Booking booking)
        {
            _context.Booking.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction("GetAll", new { id = booking.BookingID }, booking);
        }

        [HttpPut]
        public IActionResult Put(int? id, Booking booking)
        {
            var entity = _context.Booking.FirstOrDefault(m => m.BookingID == id);
            if (entity == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            entity.BookingID = booking.BookingID;
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
            var entity = _context.Booking.FirstOrDefault(m => m.BookingID == id);
            if (entity == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            _context.Booking.Remove(entity);
            _context.SaveChanges();

            return Ok(entity);
        }

    }
}
