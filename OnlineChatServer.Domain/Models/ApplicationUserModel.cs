namespace OnlineChatServer.Domain.Models
{
    public class ApplicationUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
    }
}