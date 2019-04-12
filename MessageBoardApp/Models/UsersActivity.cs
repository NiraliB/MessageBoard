using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardApp.Models
{
    public class UsersActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }

        [Required]
        public int LoginUserId { get; set; }
        public LoginUsers Login { get; set; }

        public int PostedUserId { get; set; }
        public int PostedMessageId { get; set; }
        public bool IsLike { get; set; }
        public string PostedComments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public string CommentedUserName { get; set; }

        

    }
}
