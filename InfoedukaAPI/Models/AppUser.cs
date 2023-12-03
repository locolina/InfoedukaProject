namespace InfoedukaAPI.Models
{
    public class AppUser
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public int UserTypeID { get; set; }
        public int IsActive { get; set; }
    }
}