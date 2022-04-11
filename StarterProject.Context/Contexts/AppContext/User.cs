using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterProject.Context.Base;
using System.ComponentModel.DataAnnotations;

namespace StarterProject.Context.Contexts.AppContext
{
    public class User : IEntity, IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(360)]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string Password { get; set; }

        public byte[] Salt { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        public bool Active { get; set; }

        public override string ToString()
        {
            return Id > 0 ? Id.ToString() : "New user";
        }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
            using (var context = new AppDbContext())
            {
                if (context.User.Any(c => c.Id != Id && c.Email == Email))
                {
                    yield return new ValidationResult("There is already a User registered with this E-mail", new string[] { nameof(Email) });
                }
            }
        }
	}

    internal class UserConfiguration : DbEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasIndex(c => c.Email).IsUnique();
            entity.Property(c => c.Salt).IsRequired();
        }
    }
}
