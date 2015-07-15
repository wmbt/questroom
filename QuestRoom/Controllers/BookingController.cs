using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web.Mvc;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Controllers
{
    public class BookingController : QuestRoomController
    {
        [HttpGet]
        public ActionResult Confirm(int questId, string date, string time)
        {
            var quest = Provider.GetQuest(questId);
            var costs = Provider.GetCosts();
            var bDate = DateTime.ParseExact(date, "ddMMyy", CultureInfo.InvariantCulture);
            var bTime = TimeSpan.ParseExact(time, "hhmm", CultureInfo.InvariantCulture);
            
            var checkResult = Provider.CheckBooking(questId, bDate + bTime);

            switch (checkResult)
            {
                case ProcessBookingStatus.Booked:
                    return View("ConfirmResult", new ConfirmResultViewModel
                    {
                        Title = Resources.Strings.BookingBookedTitle,
                        Message = Resources.Strings.BookingBookedMessage,
                        ShowLinkToSchedule = true
                    });
                case ProcessBookingStatus.NotExist:
                    return View("ConfirmResult", new ConfirmResultViewModel
                    {
                        Title = Resources.Strings.BookingFailureTitle,
                        Message = Resources.Strings.BookingFailureMessage,
                        ShowLinkToSchedule = false
                    });
                default:
                    var model = new ConfirmViewModel
                    {
                        Quest = quest,
                        Costs = costs,
                        Prices = costs.Select(x => new SelectListItem
                        {
                            Text = x.Persons, 
                            Value = bDate.DayOfWeek == DayOfWeek.Saturday || bDate.DayOfWeek == DayOfWeek.Sunday 
                            ? x.Weekends.ToString() 
                            : (bTime < new TimeSpan(0, 17, 0, 0) 
                                ? x.WorkdaysDay.ToString() 
                                : x.WorkdaysEvening.ToString())
                        }),
                        SelectedDate = bDate,
                        SelectedTime = bTime
                    };
                    return View(model);
            }
        }

        [HttpPost]
        public ActionResult Confirm(ConfirmViewModel model)
        {
            var questId = int.Parse((string)RouteData.Values["questId"]);
            var selectedDate = DateTime.ParseExact((string)RouteData.Values["date"], "ddMMyy", CultureInfo.InvariantCulture);
            var selectedTime = TimeSpan.ParseExact((string)RouteData.Values["time"], "hhmm", CultureInfo.InvariantCulture);
            var result = Provider.SaveBooking(
                questId, 
                selectedDate + selectedTime, 
                model.Price, 
                model.PlayerName, 
                model.Phone, 
                model.Email, 
                model.Note);

            switch (result)
            {
                case ProcessBookingStatus.Booked:
                    return View("ConfirmResult", new ConfirmResultViewModel
                    {
                        Title = Resources.Strings.BookingBookedTitle,
                        Message = Resources.Strings.BookingBookedMessage,
                        ShowLinkToSchedule = true
                    });
                case ProcessBookingStatus.NotExist:
                    return View("ConfirmResult", new ConfirmResultViewModel
                    {
                        Title = Resources.Strings.BookingFailureTitle,
                        Message = Resources.Strings.BookingFailureMessage,
                        ShowLinkToSchedule = false
                    });
                default:
                    var quest = Provider.GetQuest(questId);
                    try
                    {
                        MessageToPlayer(model.Email, model.PlayerName, quest.Name, selectedDate + selectedTime, model.Price);
                        MessageToAdmins();
                    }
                    catch { }
                    return View("ConfirmResult", new ConfirmResultViewModel
                    {
                        Title = Resources.Strings.BookingCompleteTitle,
                        Message = Resources.Strings.BookingCompleteMessage,
                        ShowLinkToSchedule = false
                    });
            }
        }

        // GET: Booking
        public ActionResult Index(string date)
        {
            DateTime selectedDate;
            DateTime currentMinDate;
            DateTime currentMaxDate;
            
            var minDate = DateTime.Now.Date;
            var maxDate = minDate + new TimeSpan(45, 0, 0, 0);
            var defaultDaysCount = new TimeSpan(2, 0, 0, 0);
            var jumpOffset = new TimeSpan(5, 0, 0, 0);
          

            if (string.IsNullOrEmpty(date))
            {
                selectedDate = minDate;
            }
            else
            {
                DateTime parsedDate;
                if (DateTime.TryParseExact(date, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    if (parsedDate > maxDate || parsedDate < minDate)
                    {
                        return RedirectToAction("Index");
                    }
                    selectedDate = parsedDate;
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            
            var beforeDaysCount = selectedDate - minDate;
            var nextDaysCount = maxDate - selectedDate;
            if (beforeDaysCount < defaultDaysCount)
            {
                currentMinDate = minDate;
                currentMaxDate = selectedDate + defaultDaysCount + (defaultDaysCount - beforeDaysCount);
            }
            else if (nextDaysCount < defaultDaysCount)
            {
                currentMaxDate = maxDate;
                currentMinDate = selectedDate - defaultDaysCount - (defaultDaysCount - nextDaysCount);
            }
            else
            {
                currentMaxDate = selectedDate + defaultDaysCount;
                currentMinDate = selectedDate - defaultDaysCount;
            }
            var jumpNextDate = selectedDate + jumpOffset;
            var jumpPrevDate = selectedDate - jumpOffset;
            
            var quests = Provider.GetQuests();
            var costs = Provider.GetCosts();

            var bookings = (from q in quests
                let periods = Provider.GetPeriods(q.Id, selectedDate)
                select new QuestSchedule
                {
                    Quest = q, Schedule = periods
                }).ToArray();

            var model = new SchedulesViewModel
            {
                QuestSchedules = bookings,
                Costs = costs,
                CurrentMaxDate = currentMaxDate,
                CurrentMinDate = currentMinDate,
                JumpPrevDate = jumpPrevDate > minDate ? jumpPrevDate : minDate,
                JumpNextDate = jumpNextDate < maxDate ? jumpNextDate : maxDate,
                SelectedDate = selectedDate
            };

            return View(model);
        }

        private void MessageToPlayer(string email, string playername, string questName, DateTime questTime, int price)
        {
            var fileName = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                ? "MessageToPlayer.en.txt"
                : "MessageToPlayer.txt";

            var bodyTemplate = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/" + fileName));
            var body = string.Format(bodyTemplate, playername, questName, questTime.ToString("d MMMM yyyy"), price);
            var subject = string.Format("Бронирование квеста \"{0}\"", questName);
            SendEmail(body, subject, new [] { email});
        }

        private void MessageToAdmins()
        {
            var body = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/MessageToAdmin.txt"));
            var subject = "Новое бронирование";
            var emails = Provider.GetUsersEmails();
            SendEmail(body, subject, emails);
        }

        private void SendEmail(string message, string subject, string[] sendTo)
        {
            var username = ConfigurationManager.AppSettings["SmtpUsername"];
            var password = ConfigurationManager.AppSettings["SmtpPassword"];
            var host = ConfigurationManager.AppSettings["SmtpHost"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var from = ConfigurationManager.AppSettings["From"];
            var recipients = string.Join(";", sendTo);

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = username,  // replace with valid value
                    Password = password  // replace with valid value
                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = host;
                smtp.Port = smtpPort;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(from, recipients, subject, message);
            }
        }
    }
}