using MessageBoardApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardApp.Data
{
    public class MessageBoardDbContext : DbContext
    {
        public MessageBoardDbContext(DbContextOptions<MessageBoardDbContext> options) : base(options)
        {
        }

        public DbSet<LoginUsers> LoginUsers { get; set; }
        public DbSet<PostedMessage> PostedMessage { get; set; }
        public DbSet<UsersActivity> UsersActivity { get; set; }
    }
}
