/*
   Tuesday, August 21, 20183:43:23 PM
   User: 
   Server: TE-0CV07514507B\SQLEXPRESS
   Database: MealPlanner
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recipe_Ingredient
	DROP CONSTRAINT FK_Recipe_Ingredient_Recipe
GO
ALTER TABLE dbo.Recipe SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recipe_Ingredient ADD CONSTRAINT
	FK_Recipe_Ingredient_Recipe FOREIGN KEY
	(
	Recipe_Id
	) REFERENCES dbo.Recipe
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Recipe_Ingredient SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Meal_Recipe
	DROP CONSTRAINT FK_Meal_Recipe_Meal
GO
ALTER TABLE dbo.Meal SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Meal_Recipe ADD CONSTRAINT
	FK_Meal_Recipe_Meal FOREIGN KEY
	(
	Meal_Id
	) REFERENCES dbo.Meal
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Meal_Recipe SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
