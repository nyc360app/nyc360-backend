using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260319190000_AddNewsDepartmentFoundation")]
    public partial class AddNewsDepartmentFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModeratedByUserId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNote",
                table: "Posts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ModerationStatus",
                table: "Posts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category_ModerationStatus_SourceType",
                table: "Posts",
                columns: new[] { "Category", "ModerationStatus", "SourceType" });

            migrationBuilder.Sql(
                """
                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.0 Publisher Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.0 Publisher Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3000)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3000, N'D08.0 Publisher Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.0 Publisher Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.0.1 Assistant Publisher Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.0.1 Assistant Publisher Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3001)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3001, N'D08.0.1 Assistant Publisher Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.0.1 Assistant Publisher Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.1 Journalist Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.1 Journalist Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3002)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3002, N'D08.1 Journalist Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.1 Journalist Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.2 Author Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.2 Author Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3003)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3003, N'D08.2 Author Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.2 Author Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.3 Documentor Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.3 Documentor Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3004)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3004, N'D08.3 Documentor Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.3 Documentor Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.4 Contributor Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.4 Contributor Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3005)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3005, N'D08.4 Contributor Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.4 Contributor Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.5 Trainee Journalist Badge';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.5 Trainee Journalist Badge')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3006)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3006, N'D08.5 Trainee Journalist Badge', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.5 Trainee Journalist Badge', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'D08.6 List News Organization in THE360.SPACE';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'D08.6 List News Organization in THE360.SPACE')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3007)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3007, N'D08.6 List News Organization in THE360.SPACE', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'D08.6 List News Organization in THE360.SPACE', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'Verified Publisher';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'Verified Publisher')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3008)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3008, N'Verified Publisher', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'Verified Publisher', 2, 7, NULL);
                END;

                UPDATE [Tags] SET [Type] = 2, [Division] = 7, [ParentTagId] = NULL WHERE [Name] = N'Probationary Publisher';
                IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Name] = N'Probationary Publisher')
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [Tags] WHERE [Id] = 3009)
                    BEGIN
                        SET IDENTITY_INSERT [Tags] ON;
                        INSERT INTO [Tags] ([Id], [Name], [Type], [Division], [ParentTagId]) VALUES (3009, N'Probationary Publisher', 2, 7, NULL);
                        SET IDENTITY_INSERT [Tags] OFF;
                    END
                    ELSE
                        INSERT INTO [Tags] ([Name], [Type], [Division], [ParentTagId]) VALUES (N'Probationary Publisher', 2, 7, NULL);
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_Category_ModerationStatus_SourceType",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ModeratedByUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ModerationNote",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Posts");

            migrationBuilder.Sql(
                """
                DELETE FROM [Tags]
                WHERE [Name] IN (
                    N'D08.0 Publisher Badge',
                    N'D08.0.1 Assistant Publisher Badge',
                    N'D08.1 Journalist Badge',
                    N'D08.2 Author Badge',
                    N'D08.3 Documentor Badge',
                    N'D08.4 Contributor Badge',
                    N'D08.5 Trainee Journalist Badge',
                    N'D08.6 List News Organization in THE360.SPACE',
                    N'Verified Publisher',
                    N'Probationary Publisher'
                );
                """);
        }
    }
}
