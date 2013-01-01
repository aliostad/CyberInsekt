SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].GetUrl
GO

-- =============================================
-- Author:		Ali Kheyrollahi
-- Create date: 2013-01-01
-- Description:	Checks whether a URL exists by its hash
-- =============================================
CREATE PROCEDURE GetUrl
	@UrlHash	BINARY(20)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	
	SELECT 
		Url
	FROM
		UrlStore
	WHERE
		@UrlHash = UrlHash
	

END
GO
