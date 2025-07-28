using AirBnBCloneMVC.Data;
using AirBnBCloneMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AirBnBCloneMVC.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly BnBDBContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ReservationController(BnBDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> ReservationForm(Guid RoomID)
        {
            var customer = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
            var room = await dbContext.Rooms.FindAsync(RoomID);
            var reservations = await dbContext.Reservations.Where(n => n.Room_ID == RoomID.ToString() && n.EndDate >= DateOnly.FromDateTime(DateTime.Now) ).ToListAsync();

            return View("Views\\User\\Customer\\ReservationForm.cshtml", Tuple.Create(customer, room, reservations));
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationViewModel viewmodel)
        {
            var customer = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
            var room = await dbContext.Rooms.FindAsync(new Guid(viewmodel.Room_ID));
            var reservations = dbContext.Reservations.Where(n => n.Room_ID == viewmodel.Room_ID && n.EndDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();

            if ((viewmodel.EndDate.DayNumber < viewmodel.StartDate.DayNumber) || (viewmodel.StartDate < DateOnly.FromDateTime(DateTime.Now) || viewmodel.EndDate < DateOnly.FromDateTime(DateTime.Now)))
            {
                ViewData["InvalidDatesMessage"] = "Invalid dates entered";
                return View("Views\\User\\Customer\\ReservationForm.cshtml", Tuple.Create(customer, room, reservations));
            }
            else
            {
                int totalDays = CalcDays(viewmodel.StartDate, viewmodel.EndDate);

                var reservation = new Reservation
                {
                    Room_ID = viewmodel.Room_ID,
                    Customer_ID = viewmodel.Customer_ID,
                    StartDate = viewmodel.StartDate,
                    EndDate = viewmodel.EndDate,
                    Total_price = totalDays * room.Room_PricePerNight
                };

                //await Response.WriteAsync("<script> alert('reached') </script>");

                if(CheckAvailability(viewmodel.StartDate, viewmodel.EndDate, viewmodel.Room_ID) == true)
                {
                    await dbContext.Reservations.AddAsync(reservation);

                    await dbContext.SaveChangesAsync();

                    //await Response.WriteAsync("<script> alert('Insertion Successful') </script>");

                    var customerReservations = getReservations(customer.Id);
                    var ownerList = await dbContext.Users.Where(n => n.IsOwner == true).ToListAsync();


                    return View("Views\\User\\Customer\\ReservationsList.cshtml", Tuple.Create(customerReservations, dbContext.Rooms.ToList(), ownerList));
                }
                else
                {
                    ViewData["NotAvailableMessage"] = "Room cannot be reserved for dates entered";  
                    return View("Views\\User\\Customer\\ReservationForm.cshtml", Tuple.Create(customer, room, reservations));
                }
               
            }
        }

        private int CalcDays(DateOnly startDate, DateOnly endDate)
        {
            
                return endDate.DayNumber - startDate.DayNumber + 1;
        }

        private List<Reservation> getReservations(Guid id)
        {
            string check = id.ToString();
            var reservations = dbContext.Reservations.Where(n => n.Customer_ID == check).ToList();

            return reservations;
        }

        public async Task<IActionResult> ReservationsList()
        {
            var customer = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
            var rooms = await dbContext.Rooms.ToListAsync();
            var reservations = getReservations(customer.Id);
            var ownerList = await dbContext.Users.Where(n => n.IsOwner == true).ToListAsync();

            return View("Views\\User\\Customer\\ReservationsList.cshtml", Tuple.Create(reservations, rooms, ownerList));
        }

        public async Task<IActionResult> EditForm(Guid id)
        {
            //var customer = await dbContext.Users.FindAsync(userId);
            var reservation = await dbContext.Reservations.FindAsync(id);
     
            return View("Views\\User\\Customer\\ReservationEdit.cshtml", reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reservation viewmodel)
        {
            var reservation = await dbContext.Reservations.FindAsync(viewmodel.Id);
            var customer = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            var reservations = dbContext.Reservations.Where(n => n.Room_ID == viewmodel.Room_ID).ToList();

            if ((viewmodel.EndDate.DayNumber < viewmodel.StartDate.DayNumber) || (viewmodel.StartDate < DateOnly.FromDateTime(DateTime.Now) || viewmodel.EndDate < DateOnly.FromDateTime(DateTime.Now)))
            {
                ViewData["InvalidDatesMessage"] = "Invalid dates entered";
                return View("Views\\User\\Customer\\ReservationEdit.cshtml", reservation);
            }
            else
            {
                var room = await dbContext.Rooms.FindAsync(new Guid(viewmodel.Room_ID));
                int totalDays = CalcDays(viewmodel.StartDate, viewmodel.EndDate);

                if (CheckAvailability(viewmodel.StartDate, viewmodel.EndDate, viewmodel.Room_ID) == true)
                {
                    reservation.StartDate = viewmodel.StartDate;
                    reservation.EndDate = viewmodel.EndDate;
                    reservation.Total_price = room.Room_PricePerNight * totalDays;

                    await dbContext.SaveChangesAsync();

                    //await Response.WriteAsync("<script> alert('Insertion Successful') </script>");

                    var customerReservations = getReservations(customer.Id);
                    var ownerList = await dbContext.Users.Where(n => n.IsOwner == true).ToListAsync();


                    return View("Views\\User\\Customer\\ReservationsList.cshtml", Tuple.Create(customerReservations, dbContext.Rooms.ToList(), ownerList));
                }
                else
                {
                    
                    ViewData["NotAvailableMessage"] = "Room cannot be reserved for dates entered";
                    return View("Views\\User\\Customer\\ReservationEdit.cshtml", reservation);
                }

            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var reservation = await dbContext.Reservations.FindAsync(id);
            var customer = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            dbContext.Reservations.Remove(reservation);
            await dbContext.SaveChangesAsync();


            var reservations = getReservations(customer.Id);

            return View("Views\\User\\Customer\\ReservationsList.cshtml", Tuple.Create(reservations, dbContext.Rooms.ToList()));
        }

        public async Task<IActionResult> DisplayRoomReservations()
        {
            var owner = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            string check = owner.Id.ToString();
            var rooms = dbContext.Rooms.Where(n => n.Owner_Id == check).ToList();
            var reservations = await dbContext.Reservations.ToListAsync();

            var query = from reservation in reservations
                        from room in rooms
                        where new Guid(reservation.Room_ID) == room.Id && new Guid(room.Owner_Id) == owner.Id
                        select reservation;

            var roomReservations = query.ToList();

            return View("Views\\User\\Owner\\RoomReservations.cshtml", Tuple.Create(roomReservations, rooms));
        }

        private bool CheckAvailability(DateOnly startDateInput, DateOnly endDateInput, string Room_IDinput)
        {
            var reservations = dbContext.Reservations.Where(n => n.Room_ID == Room_IDinput).ToList();

            var query = from reservation in reservations
                        where (startDateInput.DayNumber >= reservation.StartDate.DayNumber && startDateInput.DayNumber <= reservation.EndDate.DayNumber) || (endDateInput.DayNumber >= reservation.StartDate.DayNumber && endDateInput.DayNumber <= reservation.EndDate.DayNumber) || (startDateInput.DayNumber <= reservation.StartDate.DayNumber && endDateInput.DayNumber >= reservation.EndDate.DayNumber)
                        select reservation;

            var intersects = query.ToList();

            if (intersects.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
