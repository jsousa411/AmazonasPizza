using Microsoft.EntityFrameworkCore;
using StarterProject.Context.Contexts;
using StarterProject.Context.Contexts.AppContext;
using StarterProject.Crosscutting;

namespace StarterProject.Context
{
	public static class DbInitializer
    {
        public static void Init()
        {
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();

                if (!context.User.Any())
                {
                    var pwd = Tools.EncryptPassword("admin", out byte[] salt);

                    var user = new User
                    {
                        Name = "Administrator",
                        Email = "admin@admin.com",
                        IsEmailConfirmed = true,
                        Active = true,
                        Password = pwd,
                        Salt = salt
                    };

                    context.User.Add(user);
                    context.SaveChanges();
                }
            }
        }
    }
}
