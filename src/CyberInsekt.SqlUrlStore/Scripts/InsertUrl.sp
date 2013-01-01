SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].InsertUrl
GO

-- =============================================
-- Author:		Ali Kheyrollahi
-- Create date: 2013-01-01
-- Description:	Inserts URL if not exists
-- =============================================
CREATE PROCEDURE InsertUrl
	@UrlHash	BINARY(20),
	@Url	NVARCHAR(2048)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	BEGIN TRAN
	IF NOT EXISTS (SELECT 1 FROM dbo.UrlStore 
			WITH (UPDLOCK,SERIALIZABLE) WHERE UrlHash = @UrlHash)
		BEGIN
			INSERT INTO
				UrlStore
			(UrlHash, Url)
			VALUES
				(@UrlHash, @Url)	
		END
	COMMIT TRAN

END
GO
