using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineChatServer.Domain.Entities
{
    public class BlackList
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("BlockUser")]
        public string BlockUserId { get; set; }
        public ApplicationUser BlockUser { get; set; }
    }
}
