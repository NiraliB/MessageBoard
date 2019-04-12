using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardApp.Models.ViewModels
{
    public class PostedMesViewModel
    {
        public int MessageId { get; set; }
        public int LoginUserId { get; set; }
        public string LogerFullName { get; set; }
        public int PostedUserId { get; set; }
        public string PostedMessages { get; set; }
        public bool IsLike { get; set; }
        public int LikeCount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UsersActivity userActivity { get; set; }
       public List<UsersActivity> userCommentsAct { get; set; }

    }
}
