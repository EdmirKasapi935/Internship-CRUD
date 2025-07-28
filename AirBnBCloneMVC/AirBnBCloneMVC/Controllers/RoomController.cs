using Microsoft.AspNetCore.Mvc;
using AirBnBCloneMVC.Models;
using AirBnBCloneMVC.Data;
using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace AirBnBCloneMVC.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly BnBDBContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHost;

        public RoomController(BnBDBContext dbContext, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHost)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.webHost = webHost;
        }
        public  IActionResult RegisterForm()
        {
            User user = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));

            return View("Views\\User\\Owner\\RoomForm.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> Result(RoomViewModel model, List<IFormFile> pictures)
        {
            var room = new Room
            {
                Owner_Id = model.Owner_Id,
                Room_Name = model.Room_Name,
                Room_Capacity = model.Room_Capacity,
                Room_Type = model.Room_Type,
                Room_Description = model.Room_Description,
                Room_Location = model.Room_Location,
                Room_PricePerNight = model.Room_PricePerNight,
                Has_AirConditioning = model.Has_AirConditioning,
                Has_Kitchen = model.Has_Kitchen,
                Has_Parking = model.Has_Parking,
                Has_Pool = model.Has_Pool,
                Has_Wifi = model.Has_Wifi
            };

            try
            {
                await dbContext.Rooms.AddAsync(room);  
            }
            catch(SqlException e)
            {
                ViewData["InvalidInfoMessage"] = "Form was filled incorrectly.";
                return RegisterForm();
            }

            try 
            {
                await dbContext.SaveChangesAsync(); //we add the room to the database first
            }
            catch (DbUpdateException e)
            {
                ViewData["CannotSaveMessage"] = "Could not save room in the database.";
                return RegisterForm();
            }



            if (pictures != null)
            {
                string uploadFolder = Path.Combine(webHost.WebRootPath, "PictureUploads");

                

                if(!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                //var stream = new FileStream(uploadFolder, FileMode.Create);

                foreach(var picture in pictures)
                {
                    string savePath = Path.Combine(uploadFolder, Path.GetFileName(picture.FileName)); //the second argument is the filename of the picture
                    
                    using(var stream = new FileStream(savePath,FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    }


                    var roomPicture = new Picture
                    {
                        Path = Path.GetFileName(picture.FileName),
                        Room_Id = room.Id.ToString()
                    };

                    await dbContext.Pictures.AddAsync(roomPicture);
                }

                await dbContext.SaveChangesAsync();
            }


            User owner = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            string check = owner.Id.ToString();
            var rooms = dbContext.Rooms.Where(n => n.Owner_Id == check).ToList();


            return View("Views\\User\\Owner\\OwnerCheck.cshtml", Tuple.Create(owner, rooms) );
        }

     
        public IActionResult EditForm(Guid id)
        {
            var room = dbContext.Rooms.Find(id);
            var rented = IsRented(room);
            var roomPictures = dbContext.Pictures.Where(n => n.Room_Id == room.Id.ToString()).ToList();
            //Guid check = new Guid(room.Owner_Id);
            //var user = await dbContext.Users.FindAsync(check);

            return View("Views\\User\\Owner\\RoomInspect.cshtml", Tuple.Create(room, roomPictures, rented));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room roomModel, List<IFormFile> pictures)
        {
            var room = await dbContext.Rooms.FindAsync(roomModel.Id);
           
                room.Room_Name = roomModel.Room_Name;
                room.Room_Type = roomModel.Room_Type;
                room.Room_Capacity = roomModel.Room_Capacity;
                room.Room_Location = roomModel.Room_Location;
                room.Room_PricePerNight = roomModel.Room_PricePerNight;
                room.Room_Description = roomModel.Room_Description;
                room.Has_Wifi = roomModel.Has_Wifi;
                room.Has_Pool = roomModel.Has_Pool;
                room.Has_Kitchen = roomModel.Has_Kitchen;
                room.Has_Parking = roomModel.Has_Parking;
                room.Has_AirConditioning = roomModel.Has_AirConditioning;

            try
            {
                await dbContext.SaveChangesAsync(); //we add the room to the database first
            }
            catch (DbUpdateException e)
            {
                ViewData["CannotEditMessage"] = "Error editing room's info room in the database.";
                return EditForm(roomModel.Id);
            }


            if (pictures != null)
                {
                    string uploadFolder = Path.Combine(webHost.WebRootPath, "PictureUploads");



                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    //var stream = new FileStream(uploadFolder, FileMode.Create);

                    foreach (var picture in pictures)
                    {
                        string savePath = Path.Combine(uploadFolder, Path.GetFileName(picture.FileName)); //the second argument is the filename of the picture

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await picture.CopyToAsync(stream);
                        }


                        var roomPicture = new Picture
                        {
                            Path = Path.GetFileName(picture.FileName),
                            Room_Id = room.Id.ToString()
                        };

                        await dbContext.Pictures.AddAsync(roomPicture);
                    }

                    await dbContext.SaveChangesAsync();
                }



            //User owner = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            return EditForm(roomModel.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var room = await dbContext.Rooms.FindAsync(Id);



            if (room is not null)
            {
                if(IsRented(room))
                {
                    ViewData["RoomRentedError"] = "The room cannot be deleted because it is currently rented";
                    return EditForm(room.Id);
                }
                else
                {
                    //await dbContext.Reservations.Where(n => n.Room_ID == room.Id.ToString()).ExecuteDeleteAsync();
                    var pictures = await dbContext.Pictures.Where(n => n.Room_Id == room.Id.ToString()).ToListAsync();

                    foreach (var picture in pictures)
                    {
                        await DeleteSingleImageVoid(picture.Id);
                    }

                    //await dbContext.Pictures.Where(n => n.Room_Id == room.Id.ToString()).ExecuteDeleteAsync();

                    dbContext.Rooms.Remove(room);
                    await dbContext.SaveChangesAsync();
                }

            };

            return RedirectToAction("OwnerMenu","User");
        }

        public async Task<IActionResult> DeleteSingleImage(Guid Id)
        {
            var toDelete = await dbContext.Pictures.FindAsync(Id);
            var room = await dbContext.Rooms.FindAsync( new Guid( toDelete.Room_Id ) );
            string uploadFolder = Path.Combine(webHost.WebRootPath, "PictureUploads");
            string fullImgPath = Path.Combine(uploadFolder, toDelete.Path);

            if(System.IO.File.Exists(fullImgPath))
            {
                System.IO.File.Delete(fullImgPath);
                dbContext.Pictures.Remove(toDelete);
            }

            await dbContext.SaveChangesAsync();

            //var roomPictures = await dbContext.Pictures.Where(n => n.Room_Id == room.Id.ToString()).ToListAsync();


            return EditForm(room.Id);
        }

        public async Task DeleteSingleImageVoid(Guid Id)
        {
            var toDelete = await dbContext.Pictures.FindAsync(Id);
            var room = await dbContext.Rooms.FindAsync(new Guid(toDelete.Room_Id));
            string uploadFolder = Path.Combine(webHost.WebRootPath, "PictureUploads");
            string fullImgPath = Path.Combine(uploadFolder, toDelete.Path);

            if (System.IO.File.Exists(fullImgPath))
            {
                System.IO.File.Delete(fullImgPath);
                dbContext.Pictures.Remove(toDelete);
                
            }

            await dbContext.SaveChangesAsync();

        }

        public bool IsRented(Room room)
        {

            var reservations = dbContext.Reservations.Where(n => n.Room_ID == room.Id.ToString()).ToList();

            var query = from reservation in reservations
                        where reservation.StartDate <= DateOnly.FromDateTime(DateTime.Now) && reservation.EndDate >= DateOnly.FromDateTime(DateTime.Now)
                        select reservation;

            var intersects = query.ToList();

            if (intersects.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IActionResult> RoomView(Guid roomID)
        {
            var room = await dbContext.Rooms.FindAsync(roomID);
            var roomOwner = await dbContext.Users.FindAsync( new Guid(room.Owner_Id) );
            var roomPictures = await dbContext.Pictures.Where(n => n.Room_Id == room.Id.ToString()).ToListAsync();
            //var user = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
            //var reservations = await dbContext.Reservations.Where(n => n.Room_ID == room.Id.ToString()).ToListAsync();

            return View("Views\\User\\Customer\\RoomResult.cshtml", Tuple.Create(room, roomOwner, roomPictures));
        }
    }
}
