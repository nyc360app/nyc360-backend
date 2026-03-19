using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260319153000_AddCommunityVerificationTags")]
    public partial class AddCommunityVerificationTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE [Tags]
                SET [Type] = 2,
                    [Division] = 0,
                    [ParentTagId] = NULL
                WHERE [Name] = N'Apply for Community Leader Badges';

                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'Apply for Community Leader Badges')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 2000)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId])
                        VALUES (2000, N'Apply for Community Leader Badges', 2, 0, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId])
                        VALUES (N'Apply for Community Leader Badges', 2, 0, NULL);
                    END
                END;

                UPDATE [Tags]
                SET [Type] = 2,
                    [Division] = 0,
                    [ParentTagId] = NULL
                WHERE [Name] = N'Apply for Create a Community';

                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'Apply for Create a Community')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 2001)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId])
                        VALUES (2001, N'Apply for Create a Community', 2, 0, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId])
                        VALUES (N'Apply for Create a Community', 2, 0, NULL);
                    END
                END;

                UPDATE [Tags]
                SET [Type] = 2,
                    [Division] = 0,
                    [ParentTagId] = NULL
                WHERE [Name] = N'List Community Organization in Space';

                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'List Community Organization in Space')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 2002)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId])
                        VALUES (2002, N'List Community Organization in Space', 2, 0, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId])
                        VALUES (N'List Community Organization in Space', 2, 0, NULL);
                    END
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM [Tags]
                WHERE [Id] IN (2000, 2001, 2002)
                  AND [Name] IN (
                      N'Apply for Community Leader Badges',
                      N'Apply for Create a Community',
                      N'List Community Organization in Space'
                  );
                """);
        }
    }
}
