using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineChatServer.Domain
{
    public class UnreadDialog
    {
        public int Id { get; set; }

        [Required] public string SenderID { get; set; }

        [ForeignKey("User")] [Required] public string UserID { get; set; }

        public ApplicationUser User { get; set; }
    }
}