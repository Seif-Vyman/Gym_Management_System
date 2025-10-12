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
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                .HasKey(M => M.Id);

            builder.HasOne<Member>()
                .WithOne(M => M.HealthRecord)
                .HasForeignKey<HealthRecord>(H => H.Id);
            builder.Ignore(b => b.CreatedAt);
        }
    }
}
