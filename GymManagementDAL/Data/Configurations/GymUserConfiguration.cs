using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> builder)
        {
            builder.Property(GU => GU.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(GU => GU.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(GU => GU.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidateEmailCheck" , "Email LIKE '_%@_%._%' ");
                Tb.HasCheckConstraint("GymUserValidatePhoneCheck" , "Phone Like '01%' AND LEN(Phone) = 11 AND Phone NOT LIKE '%[^0-9]%' ");
            });
            builder.HasIndex(b => b.Email).IsUnique();
            builder.HasIndex(b => b.Phone).IsUnique();
            builder.OwnsOne(GU => GU.Address, address =>
            {
                address.Property(a => a.BuildingNumber)
                       .HasColumnName("BuildingNumber");
                address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);
                address.Property(a => a.City)
                       .HasColumnName("City")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);
            });
        }
    }
}
