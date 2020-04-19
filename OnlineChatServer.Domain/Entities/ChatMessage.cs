using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineChatServer.Domain
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string RecipientID { get; set; }

        public string FullNameSender { get; set; }
        public string DispatchTime { get; set; }
        public string TextMessage { get; set; }

        [ForeignKey("Sender")] [Required] public string SenderID { get; set; }

        public ApplicationUser Sender { get; set; }
    }
}