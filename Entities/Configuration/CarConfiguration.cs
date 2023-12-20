using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class PlaneConfiguration: IEntityTypeConfiguration<Plane>
    {
        public void Configure(EntityTypeBuilder<Plane> builder)
        {
            builder.HasData(
                new Plane
                {
                    Id = new Guid("b9e4d52a-129a-4277-a559-37600c6da2c6"),
                    Brend = "Toyota",
                    Model = "Avensis",
                    PilotId= new Guid("305a8736-8187-4854-8686-f6869493b302")
                },
                new Plane
                {
                    Id = new Guid("0e6191bc-2cbb-47ab-b4f9-246a3a7ecb7d"),
                    Brend = "BMW",
                    Model = "5-series",
                    PilotId = new Guid("27feac3d-b9d9-429f-8ca4-a520513fa714")
                }
                );

        }
    }
}
