
/****** Object:  Table [dbo].[tblAreas]    Script Date: 12/14/2013 12:01:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblCatalog]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblCatalog]
      (
        [CatalogId] [nvarchar](255) NOT NULL ,
        [CatalogName] [nvarchar](255) NOT NULL ,
        [CatalogDescription] [ntext] NULL ,
	    [CatalogConfiguration] [ntext] NULL ,
	    [ItemsProcessed] [bigint] NULL
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblAreas]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblAreas]
      (
        [AreaId] [nvarchar](255) NOT NULL ,
        [AreaName] [nvarchar](255) NOT NULL ,
        [AreaDescription] [ntext] NULL ,
        CONSTRAINT [tblAreas_PK] PRIMARY KEY NONCLUSTERED ( [AreaId] ASC )
          WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                 IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                 ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO


/****** Object:  Table [dbo].[tblProjects]    Script Date: 12/14/2013 12:02:33 PM ******/
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblProjects]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblProjects]
      (
        [ProjectId] [nvarchar](255) NOT NULL ,
        [ProjectName] [nvarchar](255) NOT NULL ,
        [AreaId] [nvarchar](255) NOT NULL ,
        [Location] [ntext] NOT NULL ,
        [CreateDate] [datetime] NOT NULL ,
        [ItemsProcessed] [bigint] NULL ,
        [WorkSummary] [nvarchar](MAX) ,
        CONSTRAINT [tblProjects_PK] PRIMARY KEY NONCLUSTERED
          ( [ProjectId] ASC )
          WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                 IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                 ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.foreign_keys
                WHERE   object_id = OBJECT_ID(N'[dbo].[FK_tblProjects_tblAreas]')
                        AND parent_object_id = OBJECT_ID(N'[dbo].[tblProjects]') )
  ALTER TABLE [dbo].[tblProjects]  WITH CHECK ADD  CONSTRAINT [FK_tblProjects_tblAreas] FOREIGN KEY([AreaId])
  REFERENCES [dbo].[tblAreas] ([AreaId])
GO

IF EXISTS ( SELECT  *
            FROM    sys.foreign_keys
            WHERE   object_id = OBJECT_ID(N'[dbo].[FK_tblProjects_tblAreas]')
                    AND parent_object_id = OBJECT_ID(N'[dbo].[tblProjects]') )
  ALTER TABLE [dbo].[tblProjects] CHECK CONSTRAINT [FK_tblProjects_tblAreas]
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjects', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table stores entries for all the projects.  It can be used to list and discover projects for job manager and other clients.',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjects'
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblNodes]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblNodes]
      (
        [NodeId] [nvarchar](255) NOT NULL ,
        [NodeName] [nvarchar](255) NOT NULL ,
        [NodeDescription] [nvarchar](255) NULL ,
        [NodeAddress] [nvarchar](255) NOT NULL ,
        [Role] [nvarchar](20) NOT NULL ,
        [Status] [nvarchar](20) NOT NULL ,
        [Version] [nvarchar](20) NOT NULL ,
        [CreateDate] [datetime] NOT NULL ,
        [ComputerInfo] [nvarchar](MAX) NOT NULL ,
	    [ItemsProcessed] [bigint] NULL ,
        CONSTRAINT [PK_tblNodes] PRIMARY KEY CLUSTERED ( [NodeId] ASC )
          WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                 IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                 ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'NodeId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique id of the node.', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblNodes',
    @level2type = N'COLUMN', @level2name = N'NodeId'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'NodeName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node, normally this is the computer name but it could be set to an alias name.',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblNodes', @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'NodeDescription') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'An optional description of the node.', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblNodes',
    @level2type = N'COLUMN', @level2name = N'NodeDescription'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'NodeAddress') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The address which will be used to connect to this node.',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblNodes', @level2type = N'COLUMN',
    @level2name = N'NodeAddress'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'Status') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The current status of this node.', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblNodes',
    @level2type = N'COLUMN', @level2name = N'Status'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'Version') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The current version of this node.', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblNodes',
    @level2type = N'COLUMN', @level2name = N'Version'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                N'COLUMN', N'ComputerInfo') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The last known memory utilization expressed in megabytes.',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblNodes', @level2type = N'COLUMN',
    @level2name = N'ComputerInfo'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblNodes',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Stores information about participating job computer nodes.',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblNodes'
GO


