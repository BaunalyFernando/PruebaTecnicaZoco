//IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
//BEGIN
//    CREATE TABLE [__EFMigrationsHistory] (
//        [MigrationId] nvarchar(150) NOT NULL,
//        [ProductVersion] nvarchar(32) NOT NULL,
//        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
//    );
//END;
//GO

//BEGIN TRANSACTION;
//GO

//CREATE TABLE [Users] (
//    [Id] int NOT NULL IDENTITY,
//    [Nombre] nvarchar(max) NOT NULL,
//    [Apellido] nvarchar(max) NOT NULL,
//    [Email] nvarchar(max) NOT NULL,
//    [Password] nvarchar(max) NOT NULL,
//    [Role] nvarchar(max) NOT NULL,
//    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
//);
//GO

//CREATE TABLE [Addresses] (
//    [Id] int NOT NULL IDENTITY,
//    [Calle] nvarchar(max) NOT NULL,
//    [Numero] nvarchar(max) NOT NULL,
//    [Ciudad] nvarchar(max) NOT NULL,
//    [UserId] int NOT NULL,
//    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
//    CONSTRAINT [FK_Addresses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
//);
//GO

//CREATE TABLE [SessionLogs] (
//    [Id] int NOT NULL IDENTITY,
//    [UserId] int NOT NULL,
//    [FechaInicio] datetime2 NOT NULL,
//    [FechaFin] datetime2 NULL,
//    CONSTRAINT [PK_SessionLogs] PRIMARY KEY ([Id]),
//    CONSTRAINT [FK_SessionLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
//);
//GO

//CREATE TABLE [Studies] (
//    [Id] int NOT NULL IDENTITY,
//    [Nombre] nvarchar(max) NOT NULL,
//    [Descripcion] nvarchar(max) NOT NULL,
//    [UserId] int NOT NULL,
//    CONSTRAINT [PK_Studies] PRIMARY KEY ([Id]),
//    CONSTRAINT [FK_Studies_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
//);
//GO

//CREATE INDEX [IX_Addresses_UserId] ON [Addresses] ([UserId]);
//GO

//CREATE INDEX [IX_SessionLogs_UserId] ON [SessionLogs] ([UserId]);
//GO

//CREATE INDEX [IX_Studies_UserId] ON [Studies] ([UserId]);
//GO

//INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
//VALUES (N'20250411182759_Primera Migracion', N'8.0.15');
//GO

//COMMIT;
//GO

