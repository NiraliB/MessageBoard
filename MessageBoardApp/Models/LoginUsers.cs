using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardApp.Models
{
    public class LoginUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? LoginDateTime { get; set; }

        public ICollection<PostedMessage> PostedMessages { get; set; }
        public ICollection<UsersActivity> UsersActivities { get; set; }


    }
}
