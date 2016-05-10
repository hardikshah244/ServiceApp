USE [ServiceAppDB]
GO

INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name])
     VALUES
           (NEWID()
           ,'Admin')
GO

INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name])
     VALUES
           (NEWID()
           ,'Customer');

INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name])
     VALUES
           (NEWID()
           ,'Engineer')
GO



