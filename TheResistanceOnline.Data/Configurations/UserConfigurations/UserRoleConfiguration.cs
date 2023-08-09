using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserRoleConfiguration: IEntityTypeConfiguration<IdentityRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(new IdentityRole
                        {
                            Name = "User",
                            NormalizedName = "USER"
                        }, new IdentityRole
                           {
                               Name = "Administrator",
                               NormalizedName = "ADMINISTRATOR"
                           });
    }

    #endregion
}