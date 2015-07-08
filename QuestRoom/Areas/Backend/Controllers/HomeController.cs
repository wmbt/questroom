using System;
using System.Globalization;
using System.Web.Mvc;
using QuestRoom.Areas.Backend.Models;
using QuestRoom.Types;

namespace QuestRoom.Areas.Backend.Controllers
{
    [Authorize]
    public class HomeController : QuestRoomController
    {
        public ActionResult BookingsByEmail(string email)
        {
            var bookings = Provider.GetBookings(email);
            var model = new BookingsByEmailViewModel
            {
                Email = email,
                Bookings = bookings
            };
            return View(model);
        }

        // GET: Backend/Home
        public ActionResult Bookings(string param)
        {
            var currentDate = DateTime.Now;

            if (!string.IsNullOrEmpty(param))
            {
                DateTime pageDate;
                if (DateTime.TryParseExact(param, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out pageDate))
                {
                    currentDate = pageDate;
                }
            }

            var bookings = Provider.GetBookings(currentDate);
            var model = new BookingsViewModel
            {
                CurrentDate = currentDate,
                Bookings = bookings
            };

            return View(model);
        }

        public ActionResult SetBookingStatus(int bookingId, BookingStatus status)
        {
            var userId = int.Parse(User.Identity.Name);
            var booking = Provider.SetBookingStatus(bookingId, status, userId);
            var json = new
            {
                Status = booking.Status.Description(),
                Processed = string.Format("{0} ({1})", booking.Processed.Value.ToString("dd.MM.yyyy HH:mm"), booking.OperatorName)
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Feedback()
        {
            var messages = Provider.GetFeedbackMessages();
            return View(messages);
        }

        public ActionResult SetMessageStatus(int messageId, FeedbackMessageStatus status)
        {
            var userId = int.Parse(User.Identity.Name);
            var message = Provider.SetFeedbackMessageStatus(messageId, status, userId);

            var json = new
            {
                Status = message.Status.Description(),
                Processed = string.Format("{0} ({1})", message.Processed.Value.ToString("dd.MM.yyyy HH:mm"), message.OperatorName)
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}