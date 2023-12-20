using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities.Configuration
{
    public class PlaneConfiguration : IEntityTypeConfiguration<Plane>
    {
        public void Configure(EntityTypeBuilder<Plane> builder)
        {
            builder.HasData
            (
            new Plane
            {
                Id = new Guid("c9dcb048-79b5-11ee-b962-0242ac120002"),
                Name = "137",
                Make = "Cessna",
                Country = "uk"
            },
             new Plane
             {
                 Id = new Guid("0f9a002f-8ef4-40f6-866b-2835ca37fd07"),
                 Name = "777",
                 Make = "Boeing",
                 Country = "uk"
             }
            ); ; ;
        }
    }
}
