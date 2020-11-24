
/*CREATE TABLE [__EFMigrationsHistory] ( [MigrationId] nvarchar(150) NOT NULL,
             [ProductVersion] nvarchar(32) NOT NULL,
             CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId]) );


SELECT 1

SELECT OBJECT_ID(N'[__EFMigrationsHistory]');


SELECT [MigrationId],
       [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId];
*/

CREATE TABLE [AspNetRoles] ( 
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id]) 
);



CREATE TABLE [Changes] ( 
    [Id] int NOT NULL IDENTITY,
    [Table] nvarchar(max) NULL,
    [EntryId] int NULL,
    [ChangeType] nvarchar(max) NULL,
    [OfflineTimeStamp] datetime2 NOT NULL,
    [UserId] int NULL,
    [OnlineTimeStamp] datetime2 NOT NULL,
    [DesktopClientId] int NULL,
    CONSTRAINT [PK_Changes] PRIMARY KEY ([Id]) 
);


CREATE TABLE [CostCategories] ( 
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_CostCategories] PRIMARY KEY ([Id]) 
);


CREATE TABLE [Departments] ( [Id] int NOT NULL IDENTITY,
                                               [Name] nvarchar(max) NOT NULL,
                                                                    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]) );


CREATE TABLE [DesktopClients] ( [Id] int NOT NULL IDENTITY,
                                                  [ClientName] nvarchar(max) NULL,
                                                                             [ClientMacAddress] nvarchar(max) NULL,
                                                                                                              [ClientType] nvarchar(max) NULL,
                                                                                                                                         CONSTRAINT [PK_DesktopClients] PRIMARY KEY ([Id]) );

CREATE TABLE [Persons] ( 
    [Id] int NOT NULL IDENTITY,
    [firstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Gender] nvarchar(max) NULL,
    [DateOfBirth] datetime2 NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY ([Id]) 
);


CREATE TABLE [AspNetRoleClaims]
    ( [Id] int NOT NULL IDENTITY,
                        [RoleId] int NOT NULL,
                                     [ClaimType] nvarchar(max) NULL,
                                                               [ClaimValue] nvarchar(max) NULL,
                                                                                          CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]), CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
     FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE );

                                                                                   
CREATE TABLE [Projects]( 
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [DepartmentId] int NOT NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Projects_Departments_DepartmentId]
    FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE 
);



CREATE TABLE [Clients]( 
    [Id] int NOT NULL IDENTITY,
    [BusinessName] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [UId] nvarchar(max) NULL,
    [UId2] nvarchar(max) NULL,
    [AmountReceivable] float NOT NULL,
    [PersonId] int NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Clients_Persons_PersonId]
    FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([Id]) ON DELETE NO ACTION 
);


CREATE TABLE [Users]( 
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [Phone] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [DepartmentId] int NOT NULL,
    [PersonId] int NOT NULL,
    [salt] nvarchar(max) NULL,
    [Status] bit NOT NULL,
    [SearchString] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Users_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Users_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([Id]) ON DELETE CASCADE );



CREATE TABLE [Services]
    ( [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(max) NULL,
                                             [Description] nvarchar(max) NULL,
                                                                         [Amount] float NOT NULL,
                                                                                        [FixedAmount] bit NOT NULL,
                                                                                                          [ProjectId] int NOT NULL,
                                                                                                                          CONSTRAINT [PK_Services] PRIMARY KEY ([Id]), CONSTRAINT [FK_Services_Projects_ProjectId]
     FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE );




CREATE TABLE [AspNetUserClaims]
    ( [Id] int NOT NULL IDENTITY,
                        [UserId] int NOT NULL,
                                     [ClaimType] nvarchar(max) NULL,
                                                               [ClaimValue] nvarchar(max) NULL,
                                                                                          CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]), CONSTRAINT [FK_AspNetUserClaims_Users_UserId]
     FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE);




CREATE TABLE [AspNetUserLogins]
    ( [LoginProvider] nvarchar(450) NOT NULL,
                                    [ProviderKey] nvarchar(450) NOT NULL,
                                                                [ProviderDisplayName] nvarchar(max) NULL,
                                                                                                    [UserId] int NOT NULL,
                                                                                                                 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider],
                                                                                                                                                               [ProviderKey]), CONSTRAINT [FK_AspNetUserLogins_Users_UserId]
     FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE);




CREATE TABLE [AspNetUserRoles] ( [UserId] int NOT NULL,
                                              [RoleId] int NOT NULL,
                                                           [UserId1] int NULL,
                                                                         [RoleId1] int NULL,
                                                                                       CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId],
                                                                                                                                    [RoleId]), CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON
DELETE CASCADE,
CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId1]
FOREIGN KEY ([RoleId1]) REFERENCES [AspNetRoles] ([Id]) ON
DELETE NO ACTION,
          CONSTRAINT [FK_AspNetUserRoles_Users_UserId]
FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON
DELETE CASCADE,
       CONSTRAINT [FK_AspNetUserRoles_Users_UserId1]
FOREIGN KEY ([UserId1]) REFERENCES [Users] ([Id]) ON
DELETE NO ACTION );



CREATE TABLE [AspNetUserTokens]
    ( [UserId] int NOT NULL,
                   [LoginProvider] nvarchar(450) NOT NULL,
                                                 [Name] nvarchar(450) NOT NULL,
                                                                      [Value] nvarchar(max) NULL,
                                                                                            CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId],
                                                                                                                                          [LoginProvider],
                                                                                                                                          [Name]), CONSTRAINT [FK_AspNetUserTokens_Users_UserId]
     FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE);


CREATE TABLE [Expenditures]
    ( [Id] int NOT NULL IDENTITY,
                        [ClientId] int NOT NULL,
                                       [Date] datetime2 NOT NULL,
                                                        [Description] nvarchar(max) NULL,
                                                                                    [Amount] float NOT NULL,
                                                                                                   [CostCategoryId] int NOT NULL,
                                                                                                                        [ProjectId] int NOT NULL,
                                                                                                                                        [IssuerId] int NOT NULL,
                                                                                                                                                       CONSTRAINT [PK_Expenditures] PRIMARY KEY ([Id]), CONSTRAINT [FK_Expenditures_Clients_ClientId]
     FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE,
                                                                    CONSTRAINT [FK_Expenditures_CostCategories_CostCategoryId]
     FOREIGN KEY ([CostCategoryId]) REFERENCES [CostCategories] ([Id]) ON DELETE CASCADE,
                                                                                 CONSTRAINT [FK_Expenditures_Users_IssuerId]
     FOREIGN KEY ([IssuerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
                                                                  CONSTRAINT [FK_Expenditures_Projects_ProjectId]
     FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) );



CREATE TABLE [Incomes]
    ( [Id] int NOT NULL IDENTITY,
                        [Type] nvarchar(max) NULL,
                                             [Date] datetime2 NOT NULL,
                                                              [ServiceId] int NOT NULL,
                                                                              [ClientId] int NOT NULL,
                                                                                             [AmountReceived] float NOT NULL,
                                                                                                                    [Discount] float NOT NULL,
                                                                                                                                     [PaymentType] nvarchar(max) NULL,
                                                                                                                                                                 [AmountReceivable] float NOT NULL,
                                                                                                                                                                                          [DateDue] datetime2 NOT NULL,
                                                                                                                                                                                                              [Unit] int NOT NULL,
                                                                                                                                                                                                                         [IncomeId] int NOT NULL,
                                                                                                                                                                                                                                        [UserId] int NOT NULL,
                                                                                                                                                                                                                                                     CONSTRAINT [PK_Incomes] PRIMARY KEY ([Id]), CONSTRAINT [FK_Incomes_Clients_ClientId]
     FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE,
                                                                    CONSTRAINT [FK_Incomes_Services_ServiceId]
     FOREIGN KEY ([ServiceId]) REFERENCES [Services] ([Id]) ON DELETE CASCADE,
                                                                      CONSTRAINT [FK_Incomes_Users_UserId]
     FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) );


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName])
WHERE [NormalizedName] IS NOT NULL;


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);


CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId1] ON [AspNetUserRoles] ([RoleId1]);

CREATE INDEX [IX_AspNetUserRoles_UserId1] ON [AspNetUserRoles] ([UserId1]);


CREATE INDEX [IX_Clients_PersonId] ON [Clients] ([PersonId]);

CREATE INDEX [IX_Expenditures_ClientId] ON [Expenditures] ([ClientId]);

CREATE INDEX [IX_Expenditures_CostCategoryId] ON [Expenditures] ([CostCategoryId]);

CREATE INDEX [IX_Expenditures_IssuerId] ON [Expenditures] ([IssuerId]);

CREATE INDEX [IX_Expenditures_ProjectId] ON [Expenditures] ([ProjectId]);

CREATE INDEX [IX_Incomes_ClientId] ON [Incomes] ([ClientId]);


CREATE INDEX [IX_Incomes_ServiceId] ON [Incomes] ([ServiceId]);

CREATE INDEX [IX_Incomes_UserId] ON [Incomes] ([UserId]);

CREATE INDEX [IX_Projects_DepartmentId] ON [Projects] ([DepartmentId]);

CREATE INDEX [IX_Services_ProjectId] ON [Services] ([ProjectId]);


CREATE INDEX [IX_Users_DepartmentId] ON [Users] ([DepartmentId]);

CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName])
WHERE [NormalizedUserName] IS NOT NULL;


CREATE INDEX [IX_Users_PersonId] ON [Users] ([PersonId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200407073754_initial',
         N'3.1.3');