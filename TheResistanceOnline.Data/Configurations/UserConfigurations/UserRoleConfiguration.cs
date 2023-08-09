using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasData(new IdentityRole
                        {
                            Name = Roles.User.ToString(),
                            NormalizedName = Roles.User.ToString().Normalize()
                        },
                        new IdentityRole
                        {
                            Name = Roles.Moderator.ToString(),
                            NormalizedName = Roles.Moderator.ToString().Normalize()
                        },
                        new IdentityRole
                        {
                            Name = Roles.Admin.ToString(),
                            NormalizedName = Roles.Admin.ToString().Normalize()
                        });
    }

    #endregion
}
