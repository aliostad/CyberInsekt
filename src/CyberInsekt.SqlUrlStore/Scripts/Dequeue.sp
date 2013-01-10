SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dequeue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].Dequeue
GO

-- =============================================
-- Author:		Ali Kheyrollahi
-- Create date: 2013-01-01
-- Description:	Inserts URL if not exists
-- =============================================
CREATE PROCEDURE Dequeue
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	
	DECLARE @Url	NVARCHAR(2048)
	DECLARE @Id		BIGINT

	BEGIN TRAN
	SELECT 
		@Url = URL,
		@Id	 = [UrlHash] 
	FROM
	(SELECT TOP 1 * FROM dbo.UrlQueue) 
			AS P 
	
	IF @URL IS NOT NULL
		BEGIN
		
		DELETE FROM UrlQueue
			WHERE [UrlHash] = @Id
		
		END
		
	
	
	COMMIT TRAN

	SELECT @Url as Url
		

END
GO
