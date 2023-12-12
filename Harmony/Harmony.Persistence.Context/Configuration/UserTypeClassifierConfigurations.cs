using Harmony.Common;
using Harmony.Common.Extensions;
using Harmony.Persistence.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace YunenApi.Persistence.Context.Configurations;

public class UserTypeClassifierConfigurations : IEntityTypeConfiguration<UserTypeClassifier>
{
    public void Configure(EntityTypeBuilder<UserTypeClassifier> builder)
    {
        builder.ToTable("UserTypes");

        builder.HasKey(e => e.Id);
        builder.HasData(
            Enum.GetValues(typeof(EUserTypes))
                .Cast<EUserTypes>()
                .Select(e =>
                {
                    return new UserTypeClassifier
                    {
                        Id = (int)e,
                        Description = e.GetDescription(),
                    };

                }));
    }
}