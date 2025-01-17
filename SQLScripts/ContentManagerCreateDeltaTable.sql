/****** Object:  Table [dbo].[tmpDelta]    Script Date: 08/17/2011 11:29:09 ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[tmpDelta]')
                    AND type IN ( N'U' ) )
    DROP TABLE [dbo].[tmpDelta]
GO

/****** Object:  Table [dbo].[tmpDelta]    Script Date: 08/17/2011 11:29:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tmpDelta]
    (
      [DocId] [nvarchar](255) NULL ,
      [Title] [nvarchar](255) NULL
    )
ON  [PRIMARY]

GO