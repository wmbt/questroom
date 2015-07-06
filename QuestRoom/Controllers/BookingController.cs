using System;
using System.Globalization;
using System.Linq;
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
                    return View("InfoPanel", new InfoPanelViewModel
                    {
                        Title = "Бронирование не выполнено",
                        Message = "Указанные дата и время уже заняты",
                        ShowLinkToSchedule = true
                    });
                case ProcessBookingStatus.NotExist:
                    return View("InfoPanel", new InfoPanelViewModel
                    {
                        Title = "Бронирование невозможно",
                        Message = "Указанные дата и время недопустимы",
                        ShowLinkToSchedule = false
                    });
                default:
                    var model = new ConfirmViewModel
                    {
                        Quest = quest,
                        Costs = costs,
                        Prices = costs.Select(x => new SelectListItem { Text = x.Persons, Value = x.Id.ToString() }),
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
                    return View("InfoPanel", new InfoPanelViewModel
                    {
                        Title = "Бронирование не выполнено",
                        Message = "Указанные дата и время уже заняты",
                        ShowLinkToSchedule = true
                    });
                case ProcessBookingStatus.NotExist:
                    return View("InfoPanel", new InfoPanelViewModel
                    {
                        Title = "Бронирование невозможно",
                        Message = "Указанные дата и время недопустимы",
                        ShowLinkToSchedule = false
                    });
                default:
                    return View("InfoPanel", new InfoPanelViewModel
                    {
                        Title = "Бронирование выполнено",
                        Message = "В течении часа с вами свяжется оператор и подтвердит бронь",
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
            var defaultDaysCount = new TimeSpan(3, 0, 0, 0);
            var jumpOffset = new TimeSpan(7, 0, 0, 0);
          

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
                let periods = Provider.GetPeriods(q.Id)
                select new Booking
                {
                    Quest = q, Schedule = periods
                }).ToArray();

            var model = new BookingViewModel
            {
                Bookings = bookings,
                Costs = costs,
                CurrentMaxDate = currentMaxDate,
                CurrentMinDate = currentMinDate,
                JumpPrevDate = jumpPrevDate > minDate ? jumpPrevDate : minDate,
                JumpNextDate = jumpNextDate < maxDate ? jumpNextDate : maxDate,
                SelectedDate = selectedDate
            };

            return View(model);
        }
    }
}