IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'DatabaseMigrations' AND XTYPE = 'U')
BEGIN
	CREATE TABLE [dbo].[DatabaseMigrations] (
		[Id]			   BIGINT   NOT NULL,
		[Name]             NVARCHAR(MAX) NULL,
		[MigrationDate]    DATETIME NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);

	INSERT INTO [DatabaseMigrations] ([Id], [Name], [MigrationDate])
		VALUES(1000, 'Create Database', GETUTCDATE())

	CREATE TABLE [dbo].[Users] (
		[Id]           BIGINT           IDENTITY (1000000000, 1) NOT NULL,
		[UserName]     NVARCHAR (32) NOT NULL,
		[ProfilePictureUrl] NVARCHAR(MAX)    NOT NULL,
		[RegistrationDate]  DATETIME NOT NULL,
		[CreditBalance]     BIGINT NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [UQ_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
	);

	CREATE TABLE [dbo].[UserLogins] (
		[Id]            BIGINT             IDENTITY (1000000000, 1) NOT NULL,
		[UserId]        BIGINT             NOT NULL,
		[LoginProvider] NVARCHAR (MAX) NOT NULL,
		[ProviderKey]   NVARCHAR (MAX) NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_UserLogins_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
	);

	CREATE TABLE [dbo].[Games] (
		[Id]             BIGINT             IDENTITY (1000000000, 1) NOT NULL,
		[Title]          NVARCHAR (MAX)  NOT NULL,
		[HeaderImageUrl]      NVARCHAR (MAX) NOT NULL,
		[SmallHeaderImageUrl] NVARCHAR (MAX) NULL,
		[LargeHeaderImageUrl]      NVARCHAR (MAX) NOT NULL,
		[SteamAppId]     BIGINT          NOT NULL,
		[Price]          FLOAT NOT NULL
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);

	CREATE TABLE [dbo].[ExcludedStoreGames] (
		[Id] BIGINT IDENTITY (1000000000, 1) NOT NULL,
		[SteamAppId] BIGINT NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);

	CREATE TABLE [dbo].[Giveaways] (
		[Id]              BIGINT            IDENTITY (1000000000, 1) NOT NULL,
		[GameId]          BIGINT            NOT NULL,
		[UserId]          BIGINT            NOT NULL,
		[Description]     NVARCHAR (MAX) NULL,
		[HeaderImageUrl]  NVARCHAR (MAX) NULL,
		[StartDate]       DATETIME       NOT NULL,
		[EndDate]         DATETIME       NOT NULL,
		[Status]          TINYINT       NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_Giveaways_Games] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Games] ([Id]),
		CONSTRAINT [FK_Giveaways_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
	);

	CREATE TABLE [dbo].[GiveawayEntries] (
		[Id]         BIGINT      IDENTITY (1000000000, 1) NOT NULL,
		[GiveawayId] BIGINT      NOT NULL,
		[UserId]     BIGINT      NOT NULL,
		[EntryDate]  DATETIME NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_GiveawayEntries_Giveaways] FOREIGN KEY ([GiveawayId]) REFERENCES [dbo].[Giveaways] ([Id]),
		CONSTRAINT [FK_GiveawayEntries_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
	);

	CREATE TABLE [dbo].[GiveawayWinners] (
		[Id]             BIGINT             IDENTITY (1000000000, 1) NOT NULL,
		[GiveawayId]     BIGINT             NOT NULL,
		[UserId]         BIGINT             NOT NULL,
		[GiftStatus]     SMALLINT        NOT NULL,
		[LastUpdated]    DATETIME        NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_GiveawayWinners_Giveaways] FOREIGN KEY ([GiveawayId]) REFERENCES [dbo].[Giveaways] ([Id]),
		CONSTRAINT [FK_GiveawayWinners_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
	);
END