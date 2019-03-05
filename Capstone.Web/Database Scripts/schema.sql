USE [master]
GO

DROP DATABASE [MealPlanner]
GO

CREATE DATABASE [MealPlanner]
GO

USE [MealPlanner]
GO
/****** Object:  Table [dbo].[Ingredient]    Script Date: 8/20/2018 2:48:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Ingredient] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meal]    Script Date: 8/20/2018 2:48:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_Meal] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meal_Plan]    Script Date: 8/20/2018 2:48:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meal_Plan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_Meal_Plan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meal_Recipe]    Script Date: 8/20/2018 2:48:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meal_Recipe](
	[Meal_Id] [int] NOT NULL,
	[Recipe_Id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealPlan_Meal]    Script Date: 8/20/2018 2:48:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealPlan_Meal](
	[MealPlan_Id] [int] NOT NULL,
	[Meal_Id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipe]    Script Date: 8/20/2018 2:48:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipe](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Prep_Time] [int] NOT NULL,
	[Cook_Time] [int] NOT NULL,
	[Category_Id] [int] NULL,
	[Description] [varchar](200) NOT NULL,
	[Instructions] [varchar](max) NULL,
	[Image] [varchar](50) NULL,
	[Shareable] [bit] NULL,
	[User_Id] [int] NULL,
 CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipe_Ingredient]    Script Date: 8/20/2018 2:48:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipe_Ingredient](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Ingredient_Id] [int] NOT NULL,
	[Recipe_Id] [int] NOT NULL,
	[Quantity] [int] NULL,
	[Unit_Type] [varchar](4) NULL,
 CONSTRAINT [PK_Recipe_Ingredient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 8/20/2018 2:48:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 8/20/2018 2:48:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Salt] [varchar](100) NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Recipe] ADD  CONSTRAINT [DF_Recipe_Shareable]  DEFAULT ((0)) FOR [Shareable]
GO
ALTER TABLE [dbo].[Meal_Plan]  WITH CHECK ADD  CONSTRAINT [FK_Meal_Plan_MealPlan_Meal] FOREIGN KEY([User_Id])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Meal_Plan] CHECK CONSTRAINT [FK_Meal_Plan_MealPlan_Meal]
GO
ALTER TABLE [dbo].[Meal_Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Meal_Recipe_Meal] FOREIGN KEY([Meal_Id])
REFERENCES [dbo].[Meal] ([Id])
GO
ALTER TABLE [dbo].[Meal_Recipe] CHECK CONSTRAINT [FK_Meal_Recipe_Meal]
GO
ALTER TABLE [dbo].[Meal_Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Meal_Recipe_Recipe] FOREIGN KEY([Recipe_Id])
REFERENCES [dbo].[Recipe] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Meal_Recipe] CHECK CONSTRAINT [FK_Meal_Recipe_Recipe]
GO
ALTER TABLE [dbo].[MealPlan_Meal]  WITH CHECK ADD  CONSTRAINT [FK_MealPlan_Meal_Meal] FOREIGN KEY([Meal_Id])
REFERENCES [dbo].[Meal] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MealPlan_Meal] CHECK CONSTRAINT [FK_MealPlan_Meal_Meal]
GO
ALTER TABLE [dbo].[MealPlan_Meal]  WITH CHECK ADD  CONSTRAINT [FK_MealPlan_Meal_Meal_Plan] FOREIGN KEY([MealPlan_Id])
REFERENCES [dbo].[Meal_Plan] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MealPlan_Meal] CHECK CONSTRAINT [FK_MealPlan_Meal_Meal_Plan]
GO
ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT [FK_Recipe_User]
GO
ALTER TABLE [dbo].[Recipe_Ingredient]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_Ingredient_Ingredient] FOREIGN KEY([Ingredient_Id])
REFERENCES [dbo].[Ingredient] ([Id])
GO
ALTER TABLE [dbo].[Recipe_Ingredient] CHECK CONSTRAINT [FK_Recipe_Ingredient_Ingredient]
GO
ALTER TABLE [dbo].[Recipe_Ingredient]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_Ingredient_Recipe] FOREIGN KEY([Recipe_Id])
REFERENCES [dbo].[Recipe] ([Id])
GO
ALTER TABLE [dbo].[Recipe_Ingredient] CHECK CONSTRAINT [FK_Recipe_Ingredient_Recipe]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
