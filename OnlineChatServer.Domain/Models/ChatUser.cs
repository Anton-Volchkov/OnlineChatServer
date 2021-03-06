namespace OnlineChatServer.Domain.Models
{
    public class ChatUser
    {
        public string UserID { get; set; }
        public string ImagePath { get; set; }
        public string FullName { get; set; }
        public bool HaveUnreadDialog { get; set; }
        public bool CanWriteMessage { get; set; }
        public bool IsBlocked { get; set; }
    }
}