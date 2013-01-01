SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Enqueue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].Enqueue
GO

-- =============================================
-- Author:		Ali Kheyrollahi
-- Create date: 2013-01-01
-- Description:	Enqueues a URL into queue
-- =============================================
CREATE PROCEDURE Enqueue
	@Url	NVARCHAR(2048)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	
	INSERT INTO 
		UrlQueue
	(Url)
	VALUES
		(@Url)
	

END
GO
