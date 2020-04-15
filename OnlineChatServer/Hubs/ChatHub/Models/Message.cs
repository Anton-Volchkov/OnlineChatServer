namespace OnlineChatServer.Hubs.ChatHub.Models
{
    public class Message
    {
        public string RecipientID { get; set; }
        public string SenderID { get; set; }
        public string FullNameSender { get; set; }
        public string DispatchTime { get; set; }
        public string TextMessage { get; set; }
        public string RecipientConnectionID { get; set; }
    }
}