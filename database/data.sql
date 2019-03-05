-- *****************************************************************************
-- This script contains INSERT statements for populating tables with seed data
-- *****************************************************************************

BEGIN;

USE [MealPlanner]
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([Id], [Name]) VALUES (1, N'Standard')
GO
INSERT [dbo].[Role] ([Id], [Name]) VALUES (2, N'Administrator')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (1, N'Tom', N'Ploetz', N'tomploetz', N'11111', N'11111', 2)
GO
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (2, N'Jon', N'Bedell', N'jonbedell', N'11112', N'11112', 2)
GO
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (3, N'Ally', N'Hurt', N'allyhurt', N'11113', N'11113', 2)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO


COMMIT;