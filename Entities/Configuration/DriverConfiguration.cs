﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class PilotConfiguration: IEntityTypeConfiguration<Pilot>
    {
        public void Configure(EntityTypeBuilder<Pilot> builder)
        {
            builder.HasData(
                new Pilot
                {
                    Id = new Guid("305a8736-8187-4854-8686-f6869493b302"),
                    Name= "Aleksandr Kanaikin",
                    Address= "Voroshilova 5"
                },
                new Pilot 
                {
                    Id = new Guid("27feac3d-b9d9-429f-8ca4-a520513fa714"),
                    Name = "Ruslan Palytin",
                    Address = "Volgogradskaya 74"
                }
            );
        }
    }
}
