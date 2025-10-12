using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {  
                Tb.HasCheckConstraint("SessionValidateStartEndTimeCheck", "EndDate > StartDate");
                Tb.HasCheckConstraint("SessionValidateCapacityCheck", "Capacity Between 1 and 25");
            });

            builder.HasOne(X => X.Category)
                   .WithMany(C => C.Sessions)
                   .HasForeignKey(X => X.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(X => X.Trainer)
                   .WithMany(T => T.Sessions)
                   .HasForeignKey(X => X.TrainerId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
