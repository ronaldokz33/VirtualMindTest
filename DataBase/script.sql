IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'VirtualMind')
	CREATE DATABASE VirtualMind
GO

USE VirtualMind
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Purchase]') AND type in (N'U'))
	DROP TABLE [dbo].[Purchase]
GO

CREATE TABLE [dbo].[Purchase] (
	id INT PRIMARY KEY IDENTITY(1,1),
	userId VARCHAR(50) NOT NULL,
	amount DECIMAL(10,2) NOT NULL DEFAULT 0,
	currency CHAR(3) NOT NULL,
	createdAt DATETIME NOT NULL DEFAULT GETDATE()
)
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[I_sp_Purchase]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[I_sp_Purchase]
GO


CREATE PROCEDURE [dbo].[I_sp_Purchase] (
	@userId	VARCHAR(50),
	@amount DECIMAL(10,2),
	@currency CHAR(3)
)
AS
BEGIN
	DECLARE @t TABLE (id INT)

	INSERT INTO Purchase (
		userId,
		amount,
		currency
	)
	OUTPUT inserted.id INTO @t
	VALUES (
		@userId,
		@amount,
		@currency
	)

	SELECT id FROM @t
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[S_sp_Purchase_ListForLookup]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[S_sp_Purchase_ListForLookup]
GO


CREATE PROCEDURE [dbo].[S_sp_Purchase_ListForLookup] (
	@userId	VARCHAR(50),
	@currency CHAR(3)
)
AS
BEGIN
	
	SELECT
		id,
		userId,
		amount,
		currency,
		createdAt
	FROM
		Purchase
	WHERE
		(@userId IS NULL OR userId = @userId) AND
		(@currency IS NULL OR currency = @currency)
END
GO