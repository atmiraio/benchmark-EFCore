IF db_id('EFCore5') IS NULL 
    CREATE DATABASE EFCore5
ELSE
    DROP DATABASE EFCore5
    CREATE DATABASE EFCore5

GO

BEGIN TRANSACTION;
GO

CREATE TABLE EFCore5.dbo.[Entities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Property1] int NOT NULL,
    [Property2] float NOT NULL,
    [Property3] decimal(18,2) NOT NULL,
    [Date] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Entities] PRIMARY KEY ([Id])
);
GO

CREATE TABLE EFCore5.dbo.[Fathers] (
    [FatherId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Fathers] PRIMARY KEY ([FatherId])
);
GO

CREATE TABLE EFCore5.dbo.[Grades] (
    [GradeId] int NOT NULL IDENTITY,
    [GradeName] nvarchar(max) NULL,
    [Section] nvarchar(max) NULL,
    CONSTRAINT [PK_Grades] PRIMARY KEY ([GradeId])
);
GO

CREATE TABLE EFCore5.dbo.[Products] (
    [ProductId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId])
);
GO

CREATE TABLE EFCore5.dbo.[SimpleEntities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_SimpleEntities] PRIMARY KEY ([Id])
);
GO

CREATE TABLE EFCore5.dbo.[Suppliers] (
    [SupplierId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [Country] nvarchar(max) NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([SupplierId])
);
GO

CREATE TABLE EFCore5.dbo.[Children] (
    [ChildId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [ChildOfFatherId] int NOT NULL,
    CONSTRAINT [PK_Children] PRIMARY KEY ([ChildId]),
    CONSTRAINT [FK_Children_Fathers_ChildOfFatherId] FOREIGN KEY ([ChildOfFatherId]) REFERENCES [Fathers] ([FatherId]) ON DELETE CASCADE
);
GO

CREATE TABLE EFCore5.dbo.[Students] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [CurrentGradeId] int NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Students_Grades_CurrentGradeId] FOREIGN KEY ([CurrentGradeId]) REFERENCES [Grades] ([GradeId]) ON DELETE CASCADE
);
GO

CREATE TABLE EFCore5.dbo.[SupplierProducts] (
    [SupplierId] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_SupplierProducts] PRIMARY KEY ([SupplierId], [ProductId]),
    CONSTRAINT [FK_SupplierProducts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SupplierProducts_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Children_ChildOfFatherId] ON EFCore5.dbo.[Children] ([ChildOfFatherId]);
GO

CREATE INDEX [IX_Students_CurrentGradeId] ON EFCore5.dbo.[Students] ([CurrentGradeId]);
GO

CREATE INDEX [IX_SupplierProducts_ProductId] ON EFCore5.dbo.[SupplierProducts] ([ProductId]);
GO

COMMIT;
GO