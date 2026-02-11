-- create-database.sql
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'NewsPortal')
BEGIN
    CREATE DATABASE NewsPortal;
END
GO

USE NewsPortal;
GO

-- efcore InitalCreate migration to sql output
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Items] (
    [Id] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Url] nvarchar(max) NULL,
    [Time] datetime2 NULL,
    [ItemType] int NOT NULL,
    CONSTRAINT [PK_Items] PRIMARY KEY ([Id])
    );

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(20) NOT NULL,
    [LastName] nvarchar(30) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );

CREATE TABLE [UserItems] (
    [UserId] int NOT NULL,
    [ItemId] int NOT NULL,
    [SavedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_UserItems] PRIMARY KEY ([UserId], [ItemId]),
    CONSTRAINT [FK_UserItems_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Items] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserItems_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );

CREATE INDEX [IX_UserItems_ItemId] ON [UserItems] ([ItemId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260211121405_InitialCreate', N'10.0.3');

COMMIT;
GO
