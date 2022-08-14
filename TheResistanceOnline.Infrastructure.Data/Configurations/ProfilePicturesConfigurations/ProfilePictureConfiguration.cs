using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.ProfilePictures;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.ProfilePicturesConfigurations
{
    public class ProfilePictureConfiguration: IEntityTypeConfiguration<ProfilePicture>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<ProfilePicture> builder)
        {
            
            builder.HasKey(p => p.Id)
                   .HasName("PK_ProfilePictures");


            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasData(new ProfilePicture
                            {
                                Id = 1,
                                Name = "1.png",
                                Description = "Black man cyberpunk"
                            }, new ProfilePicture
                               {
                                   Id = 2,
                                   Name = "2.png",
                                   Description = "White woman cyberpunk"
                               }, new ProfilePicture
                                  {
                                      Id = 3,
                                      Name = "3.png",
                                      Description = "White man spy"
                                  },
                            new ProfilePicture
                            {
                                Id = 4,
                                Name = "4.png",
                                Description = "White woman spy"
                            },
                            new ProfilePicture
                            {
                                Id = 5,
                                Name = "5.png",
                                Description = "White man cyber warrior"
                            });
        }

        #endregion
    }
}
