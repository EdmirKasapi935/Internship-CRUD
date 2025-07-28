namespace AirBnBCloneMVC.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }
        public bool IsOwner { get; set; }
    }
}
