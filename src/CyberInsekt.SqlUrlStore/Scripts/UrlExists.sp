SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UrlExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].UrlExists
GO

-- =============================================
-- Author:		Ali Kheyrollahi
-- Create date: 2013-01-09
-- Description:	checks whether URL exists in Q or Store
-- =============================================
CREATE PROCEDURE UrlExists
	@UrlHash	BINARY(20)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	
	SELECT 1 WHERE
		EXISTS(SELECT 1 FROM UrlStore
			WHERE UrlHash = @UrlHash)
			OR
		EXISTS(SELECT 1 FROM UrlQueue
			WHERE UrlHash = @UrlHash)
			
	

END
GO
