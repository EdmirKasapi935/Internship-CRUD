using Microsoft.EntityFrameworkCore;
using AirBnBCloneMVC.Models;

namespace AirBnBCloneMVC.Data
{
    public class BnBDBContext : DbContext
    {
        public BnBDBContext(DbContextOptions<BnBDBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Picture> Pictures { get; set; }
    }
}
