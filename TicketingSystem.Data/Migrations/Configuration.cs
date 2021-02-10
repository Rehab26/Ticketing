namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TicketingSystem.Common.Enums;
    using TicketingSystem.Data.Entites;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketingSystem.Data.TicketingStystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        public class TicketingStystemInitializer : CreateDatabaseIfNotExists<TicketingStystemContext>
        {
            protected override void Seed(TicketingSystem.Data.TicketingStystemContext context)
            {
                var manager = new User()
                {
                    FirstName = "Ghadir",
                    LastName = "Aljafen",
                    Email = "Ghadiraljafen@gmail.com",
                    PhoneNumber = "0565413424",
                    Type = UserType.Manager,
                    LoginType = LoginApproach.Password,
                    Password = "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=",
                    CreatedDate = DateTime.Now,
                    DateOfBirth = new DateTime(1998, 1, 29),
                };

                context.Users.AddOrUpdate(manager);
                context.SaveChanges();
                base.Seed(context);
            }


        }
    }
}
