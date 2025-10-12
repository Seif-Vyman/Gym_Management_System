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
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(b => b.CreatedAt)
                .HasColumnName("BookingDay")
                .HasDefaultValueSql("GETDATE()");

            builder.HasKey(MS => new { MS.MemberId, MS.SessionId });
            builder.Ignore(MS => MS.Id);
        }
    }
}
