namespace AirBnBCloneMVC.Models
{
    public class ReservationViewModel
    {
        public string Room_ID { get; set; }
        public string Customer_ID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double Total_price  { get; set; }
    }
}
