using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterProject.Context.Base;
using System.ComponentModel.DataAnnotations;

namespace StarterProject.Context.Contexts.AppContext
{
    public class Products : IEntity, IValidatableObject
    {       
        public int Id { get; set; }
            
        [Required]
        [StringLength(360)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Type { get; set; }

        [StringLength(360)]
        public string AdvertisingContext { get; set; }

        [StringLength(360)]
        public string MenuText { get; set; }

        public int price { get; set; }


        [Required]
        public bool Active { get; set; }

        public override string ToString()
        {
            return Id > 0 ? Id.ToString() : "New Field";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var context = new AppDbContext())
            {
                if (context.Product.Any(c => c.Id != Id && c.Name == Name))
                {
                    yield return new ValidationResult("Field not found", new string[] { nameof(Name) });
                }
            }
        }
    }

    internal class ProductNameConfiguration : DbEntityConfiguration<Products>
    {
        public override void Configure(EntityTypeBuilder<Products> entity)
        {
            entity.HasIndex(c => c.Name).IsUnique();
            //entity.Property(c => c.Salt).IsRequired();
        }
    }
     
}
