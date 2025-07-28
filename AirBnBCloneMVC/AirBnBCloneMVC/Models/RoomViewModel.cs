namespace AirBnBCloneMVC.Models
{
    public class RoomViewModel
    {
        public string Owner_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Type { get; set; }
        public int Room_Capacity { get; set; }
        public string Room_Location { get; set; }
        public double Room_PricePerNight { get; set; }
        public string Room_Description { get; set; }
        public bool Has_Wifi { get; set; }
        public bool Has_Pool { get; set; }
        public bool Has_Kitchen { get; set; }
        public bool Has_Parking { get; set; }
        public bool Has_AirConditioning { get; set; }


    }
}
