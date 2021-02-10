using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Entites;
using static TicketingSystem.Data.Migrations.Configuration;

namespace TicketingSystem.Data
{
    public class TicketingStystemContext : DbContext
    {
        public TicketingStystemContext() : base("name=connnectionString")
        {

           // Database.SetInitializer<TicketingStystemContext>(new CreateDatabaseIfNotExists<TicketingStystemContext>());
            Database.SetInitializer<TicketingStystemContext>(new TicketingStystemInitializer());

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Reply> Replys { get; set; }
        public DbSet<FileStorage> FileStorages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // EntityTypeConfiguration<FileStorage> fileStorages = modelBuilder.Entity<FileStorage>();
            EntityTypeConfiguration<Ticket> ticket = modelBuilder.Entity<Ticket>();
            EntityTypeConfiguration<User> user = modelBuilder.Entity<User>();
            EntityTypeConfiguration<Reply> reply = modelBuilder.Entity<Reply>();

            modelBuilder.Entity<User>().HasIndex(u => u.PhoneNumber).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            user.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);
            user.Property(p => p.Email).IsRequired();
            user.Property(p => p.DateOfBirth).IsOptional();
            user.Property(p => p.Password).IsOptional();
            user.Property(p => p.FirstName).IsRequired();
            user.Property(p => p.LastName).IsRequired();
            ticket.Property(p => p.Description).IsRequired();
            reply.Property(p => p.Content).IsRequired();
            ticket.HasRequired(u => u.User).WithMany(u => u.Tickets).HasForeignKey(u => u.ClientId).WillCascadeOnDelete(false);
            reply.HasRequired(u => u.User).WithMany(u => u.Replies).HasForeignKey(u => u.UserId).WillCascadeOnDelete(false);
 
             modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);

        }
    }
}
