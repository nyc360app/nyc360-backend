using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260326120000_AddApprovedSpaceLocationListings")]
    public partial class AddApprovedSpaceLocationListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[ApprovedSpaceLocationListings]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [ApprovedSpaceLocationListings]
                    (
                        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ApprovedSpaceLocationListings] PRIMARY KEY,
                        [SpaceListingId] INT NOT NULL,
                        [Department] TINYINT NOT NULL,
                        [EntityType] TINYINT NOT NULL,
                        [Name] NVARCHAR(150) NOT NULL,
                        [LocationName] NVARCHAR(200) NULL,
                        [Borough] NVARCHAR(100) NULL,
                        [Neighborhood] NVARCHAR(200) NULL,
                        [Street] NVARCHAR(200) NULL,
                        [BuildingNumber] NVARCHAR(50) NULL,
                        [ZipCode] NVARCHAR(10) NULL,
                        [Website] NVARCHAR(300) NULL,
                        [PhoneNumber] NVARCHAR(50) NULL,
                        [PublicEmail] NVARCHAR(255) NULL,
                        [SubmitterUserId] INT NOT NULL,
                        [ApprovedByUserId] INT NOT NULL,
                        [ApprovedAt] DATETIME2 NOT NULL,
                        [ModerationNote] NVARCHAR(1000) NULL,
                        [CreatedAt] DATETIME2 NOT NULL,
                        [UpdatedAt] DATETIME2 NOT NULL,
                        CONSTRAINT [FK_ApprovedSpaceLocationListings_SpaceListings_SpaceListingId]
                            FOREIGN KEY ([SpaceListingId]) REFERENCES [SpaceListings]([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_ApprovedSpaceLocationListings_UserProfiles_SubmitterUserId]
                            FOREIGN KEY ([SubmitterUserId]) REFERENCES [UserProfiles]([UserId]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_ApprovedSpaceLocationListings_UserProfiles_ApprovedByUserId]
                            FOREIGN KEY ([ApprovedByUserId]) REFERENCES [UserProfiles]([UserId]) ON DELETE NO ACTION
                    );

                    CREATE UNIQUE INDEX [IX_ApprovedSpaceLocationListings_SpaceListingId]
                        ON [ApprovedSpaceLocationListings]([SpaceListingId]);

                    CREATE INDEX [IX_ApprovedSpaceLocationListings_ApprovedAt]
                        ON [ApprovedSpaceLocationListings]([ApprovedAt]);
                END
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[ApprovedSpaceLocationListings]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [ApprovedSpaceLocationListings];
                END
                """);
        }
    }
}
