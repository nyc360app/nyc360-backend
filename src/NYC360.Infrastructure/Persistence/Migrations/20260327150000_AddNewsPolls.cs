using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260327150000_AddNewsPolls")]
    public partial class AddNewsPolls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[NewsPolls]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [NewsPolls]
                    (
                        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_NewsPolls] PRIMARY KEY,
                        [CreatorUserId] INT NOT NULL,
                        [Status] TINYINT NOT NULL,
                        [Title] NVARCHAR(200) NOT NULL,
                        [Question] NVARCHAR(500) NOT NULL,
                        [Description] NVARCHAR(2000) NULL,
                        [Slug] NVARCHAR(220) NOT NULL,
                        [CoverImageUrl] NVARCHAR(500) NULL,
                        [ClosesAt] DATETIME2 NULL,
                        [AllowMultipleAnswers] BIT NOT NULL,
                        [ShowResultsBeforeVoting] BIT NOT NULL,
                        [IsFeatured] BIT NOT NULL,
                        [Category] TINYINT NOT NULL CONSTRAINT [DF_NewsPolls_Category] DEFAULT(7),
                        [TagsJson] NVARCHAR(MAX) NULL,
                        [LocationId] INT NULL,
                        [ReviewedByUserId] INT NULL,
                        [ReviewedAt] DATETIME2 NULL,
                        [ModerationNote] NVARCHAR(1000) NULL,
                        [CreatedAt] DATETIME2 NOT NULL,
                        [UpdatedAt] DATETIME2 NOT NULL,
                        CONSTRAINT [FK_NewsPolls_UserProfiles_CreatorUserId]
                            FOREIGN KEY ([CreatorUserId]) REFERENCES [UserProfiles]([UserId]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_NewsPolls_UserProfiles_ReviewedByUserId]
                            FOREIGN KEY ([ReviewedByUserId]) REFERENCES [UserProfiles]([UserId]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_NewsPolls_Locations_LocationId]
                            FOREIGN KEY ([LocationId]) REFERENCES [Locations]([Id]) ON DELETE NO ACTION
                    );

                    CREATE UNIQUE INDEX [IX_NewsPolls_Slug] ON [NewsPolls]([Slug]);
                    CREATE INDEX [IX_NewsPolls_CreatorUserId] ON [NewsPolls]([CreatorUserId]);
                    CREATE INDEX [IX_NewsPolls_Status] ON [NewsPolls]([Status]);
                    CREATE INDEX [IX_NewsPolls_CreatedAt] ON [NewsPolls]([CreatedAt]);
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[NewsPollOptions]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [NewsPollOptions]
                    (
                        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_NewsPollOptions] PRIMARY KEY,
                        [PollId] INT NOT NULL,
                        [Text] NVARCHAR(300) NOT NULL,
                        [SortOrder] INT NOT NULL,
                        CONSTRAINT [FK_NewsPollOptions_NewsPolls_PollId]
                            FOREIGN KEY ([PollId]) REFERENCES [NewsPolls]([Id]) ON DELETE CASCADE
                    );

                    CREATE INDEX [IX_NewsPollOptions_PollId] ON [NewsPollOptions]([PollId]);
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[NewsPollVotes]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [NewsPollVotes]
                    (
                        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_NewsPollVotes] PRIMARY KEY,
                        [PollId] INT NOT NULL,
                        [VoterUserId] INT NOT NULL,
                        [SelectedOptionIds] NVARCHAR(MAX) NOT NULL,
                        [CreatedAt] DATETIME2 NOT NULL,
                        CONSTRAINT [FK_NewsPollVotes_NewsPolls_PollId]
                            FOREIGN KEY ([PollId]) REFERENCES [NewsPolls]([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_NewsPollVotes_UserProfiles_VoterUserId]
                            FOREIGN KEY ([VoterUserId]) REFERENCES [UserProfiles]([UserId]) ON DELETE NO ACTION
                    );

                    CREATE UNIQUE INDEX [IX_NewsPollVotes_PollId_VoterUserId]
                        ON [NewsPollVotes]([PollId], [VoterUserId]);
                    CREATE INDEX [IX_NewsPollVotes_PollId] ON [NewsPollVotes]([PollId]);
                    CREATE INDEX [IX_NewsPollVotes_VoterUserId] ON [NewsPollVotes]([VoterUserId]);
                END
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[NewsPollVotes]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [NewsPollVotes];
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[NewsPollOptions]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [NewsPollOptions];
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[NewsPolls]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [NewsPolls];
                END
                """);
        }
    }
}
