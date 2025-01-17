SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblProject]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblProject]
      (
        [ProjectId] [nvarchar](255) NOT NULL ,
        [ProjectName] [nvarchar](255) NULL ,
        [Description] [nvarchar](255) NULL ,
        [BatchSize] [int] NULL ,
        [ItemsLocation] [ntext] NULL ,
        [CreateDate] [datetime] NULL
                                DEFAULT ( GETDATE() ) ,
        [ItemsProcessed] [bigint] NULL ,
        [WorkSummary] [nvarchar](MAX)
          NULL
          CONSTRAINT [aaaaatblProject_PK]
          PRIMARY KEY NONCLUSTERED ( [ProjectId] ASC )
          WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF ) ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ProjectId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProjectId', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProject', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ProjectName',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProjectName', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProject', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ProjectName') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ProjectName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'Description',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'Description', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProject', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'Description') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'Description'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DecimalPlaces', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchSize',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'BatchSize', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProject', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'BatchSize') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'BatchSize'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'XML - This container object', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ItemsLocation',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ItemsLocation', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProject', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextFormat', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'TextFormat', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'12',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'ItemsLocation') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'ItemsLocation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DefaultValue', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'CreateDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'CreateDate', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProject', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 9:13:30 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/28/2010 10:47:30 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProject', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblProject',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProject', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProject'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblJobBatchRel]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblJobBatchRel]
      (
        [JobId] [nvarchar](255) NOT NULL ,
        [BatchId] [nvarchar](255) NOT NULL ,
        CONSTRAINT [aaaaatblJobBatchRel_PK] PRIMARY KEY NONCLUSTERED
          ( [JobId] ASC, [BatchId] ASC )
          WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF ) ON [PRIMARY]
      )
    ON
      [PRIMARY]
  END
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel',
    @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'JobId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'JobId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblJobBatchRel', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel',
    @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel',
    @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblJobBatchRel', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJobBatchRel', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 9:11:11 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/27/2010 9:13:05 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJobBatchRel',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblJobBatchRel',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblJobBatchRel', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJobBatchRel'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblJob]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblJob]
      (
        [JobId] [nvarchar](255) NOT NULL ,
        [JobName] [nvarchar](255) NULL ,
        [Configuration] [ntext] NULL ,
        [Operation] [nvarchar](255) NULL ,
        [CreateDate] [datetime] NULL
                                DEFAULT ( GETDATE() ) ,
        [ItemsProcessed] [bigint] NULL ,
        [WorkSummary] [nvarchar](MAX) ,
		[ResultSummary] [ntext]
          NULL
          CONSTRAINT [aaaaatblJob_PK]
          PRIMARY KEY NONCLUSTERED ( [JobId] ASC )
          WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF ) ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO

--  Set Extended Properties

--  JobId
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'GUID', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'GUID', @value = N'뚼勀䪧㢗笣迓횠',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Unique Job Id', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'JobId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'JobId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblJob',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO

--  JobName
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the Job', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN',
                                                N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'JobName',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'JobName',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblJob',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN', @level2name = N'JobName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN',
                                                N'JobName') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'JobName'
GO

--  Configuration
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'Configuration',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'XML serialized job configuration parameters.',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'6',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'Configuration', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblJob',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextFormat', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'TextFormat', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'12',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN',
                                                N'Configuration') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Configuration'
GO

--  Operation
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'GUID', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'GUID', @value = N'╻�谠䌜沵᳍핚뚞',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Type of Operation; i.e. Export, Migrate, etc',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'Operation',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'6',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'Operation', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblJob',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO

--  CreateDate
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DefaultValue', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'GUID', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'GUID', @value = N'滣炀䃜溓ᢋⶾ嗵',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date Time this entry was created', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblJob',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'CreateDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'16',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'CreateDate', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblJob',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO

--  Misc
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 9:02:08 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblJob', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/28/2010 1:20:08 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblJob', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblJob',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblJob',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblJob'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblJobRelationships]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblJobRelationships]
      (
        [JobRelationshipId] [nvarchar](255) NOT NULL ,
        [JobRelationshipName] [nvarchar](255) NOT NULL ,
        [JobRelationshipDescription] [nvarchar](255) NULL ,
        CONSTRAINT [PK_tblJobRelationships] PRIMARY KEY CLUSTERED
          ( [JobRelationshipId] ASC )
          WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                 IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                 ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
      )
    ON
      [PRIMARY]
  END
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblRelatedJobs]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblRelatedJobs]
      (
        [JobRelationshipId] [nvarchar](255) NOT NULL ,
        [JobId] [nvarchar](255) NOT NULL ,
        CONSTRAINT [aaaaatblRelatedJobs_PK] PRIMARY KEY NONCLUSTERED
          ( [JobRelationshipId] ASC, [JobId] ASC )
          WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                 IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                 ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
      )
    ON
      [PRIMARY]
  END
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblBatchLock]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblBatchLock]
      (
        [ID] [int] IDENTITY(1, 1)
                   NOT NULL ,
        [BatchId] [nvarchar](255) NULL ,
        [IsLocked] [bit] NOT NULL
                         DEFAULT ( (0) ) ,
        [LockDate] [datetime] NULL ,
        [UnlockDate] [datetime] NULL ,
        [LockedBy] [nvarchar](255) NULL ,
        CONSTRAINT [aaaaatblBatchLock_PK] PRIMARY KEY NONCLUSTERED
          ( [ID] ASC ) WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF )
          ON [PRIMARY]
      )
    ON
      [PRIMARY]
  END
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'17',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ID',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'ID',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchLock', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchLock', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DefaultValue', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'DefaultValue', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'106', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Format', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Format', @value = N'Yes/No',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'IsLocked',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'IsLocked',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchLock', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'IsLocked') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'IsLocked'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'LockDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'LockDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchLock', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'UnlockDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'UnlockDate', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchLock', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'UnlockDate') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'UnlockDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'LockedBy',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'LockedBy',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchLock', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                N'COLUMN', N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock', @level2type = N'COLUMN',
    @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchLock', N'COLUMN',
                                                N'LockedBy') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock',
    @level2type = N'COLUMN', @level2name = N'LockedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 1:04:54 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/27/2010 1:14:15 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchLock',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblBatchLock',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchLock', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchLock'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblBatchItems]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblBatchItems]
      (
        [ID] [int] IDENTITY(1, 1)
                   NOT NULL ,
        [BatchId] [nvarchar](255) NULL ,
        [Title] [nvarchar](255) NULL ,
        [SourceDocId] [nvarchar](255) NULL ,
        [DestDocId] [nvarchar](255) NULL ,
        [ProcessedStatus] [nvarchar](50) NULL ,
        [Operation] [nvarchar](255) NULL ,
        [ProcessResult] [ntext] NULL ,
        [ProcessedMessage] [nvarchar](MAX) NULL ,
        [ProcessStartTime] [datetime] NULL ,
        [ProcessFinishTime] [datetime] NULL ,
        [TotalProcessingTime] [real] NULL ,
        [ProcessedBy] [nvarchar](255) NULL ,
        [CreateDate] [datetime] NULL
                                DEFAULT ( GETDATE() ) ,
        CONSTRAINT [aaaaatblBatchItems_PK] PRIMARY KEY NONCLUSTERED
          ( [ID] ASC ) WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF )
          ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]


    CREATE NONCLUSTERED INDEX [idxPS] ON [dbo].[tblBatchItems] 
    (
    [ProcessedStatus] ASC
    )
    INCLUDE ( [BatchId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

    CREATE NONCLUSTERED INDEX [idx_2]
    ON [dbo].[tblBatchItems] ([BatchId],[ProcessedStatus])
    INCLUDE ([ID])

    CREATE NONCLUSTERED INDEX [idx_BatchId]
    ON [dbo].[tblBatchItems] ([BatchId])
    INCLUDE ([ID])

    CREATE NONCLUSTERED INDEX [_dta_index_tblBatchItems_Statistics] ON [dbo].[tblBatchItems]
    (
    [ProcessedStatus] ASC,
    [BatchId] ASC
    )
    INCLUDE ( 	[TotalProcessingTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

  END
GO

--  ID
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'17',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'GUID', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'GUID', @value = N'⼄㪌๋佩芭緥딮',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'AutoGenerated Database ID', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ID',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'ID',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ID') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ID'
GO

--  BatchID
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'1995',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'GUID', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'GUID', @value = N'듺颒ㅁ䑠ށ怃ꁯᮽ',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID of the Batch this item is related', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'BatchId'
GO

--  Title
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Title of the document', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'Title',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'Title',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Title'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Title') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Title'
GO

--  SourceDocId
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'2880',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Document Id from the source system', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'SourceDocId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'SourceDocId', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'SourceDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'SourceDocId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'SourceDocId'
GO

--  DestDocId
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Document Id from the destination system',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'DestDocId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'DestDocId', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'DestDocId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'DestDocId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'DestDocId'
GO

--  ProcessedStatus
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sucess, Failed, NotProcessed', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ProcessedStatus',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'50',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProcessedStatus', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedStatus'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedStatus') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedStatus'
GO

--  Operation
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Export, Migrate, Bulk Delete, Bulk CheckIn, Bulk Checkout',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'Operation',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'6',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'Operation', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'Operation') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'Operation'
GO

--  ProcessResult
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ProcessResult',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'6',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProcessResult', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextFormat', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'TextFormat', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'12',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessResult'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessResult') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessResult'
GO

--  ProcessedMessage
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Informational message about any errors that occured during processing',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'Name',
    @value = N'ProcessedMessage', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'7',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProcessedMessage', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextFormat', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'TextFormat', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'12',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedMessage'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedMessage') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedMessage'
GO

--  ProcessStartTime
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date/Time when the process started', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'Name',
    @value = N'ProcessStartTime', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProcessStartTime', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessStartTime') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessStartTime'
GO

--  ProcessFinishTime
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date/Time when the process finished', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'Name',
    @value = N'ProcessFinishTime', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'9',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProcessFinishTime', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'ProcessFinishTime') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessFinishTime'
GO

--  TotalProcessingTime
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DecimalPlaces', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The difference between StartTime and EndTime',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'Name',
    @value = N'TotalProcessingTime', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'TotalProcessingTime', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN',
                                                N'TotalProcessingTime') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'6',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'TotalProcessingTime'
GO

--  ProcessedBy
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'GUID', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'GUID', @value = N'놟黿ꜙ䭈펈饱몹ࠋ',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'The computer name of the system that processed this item',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ProcessedBy',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'11',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProcessedBy', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'ProcessedBy'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'ProcessedBy') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'ProcessedBy'
GO

--  CreateDate
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'2130',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DefaultValue', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date when this record was added', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'CreateDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'12',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'CreateDate', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblBatchItems', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems',
    @level2type = N'COLUMN', @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', N'COLUMN',
                                                N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO

--  Misc
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/7/2010 1:06:04 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/20/2010 2:07:15 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatchItems',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblBatchItems',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'318',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblBatchItems', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatchItems'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblBatch]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblBatch]
      (
        [BatchId] [nvarchar](255) NOT NULL ,
        [BatchName] [nvarchar](255) NULL ,
        [AssignedTo] [nvarchar](255) NULL ,
        [CreateDate] [datetime] NULL
                                DEFAULT ( GETDATE() ) ,
        [WorkSummary] [nvarchar](MAX) NULL ,
		[ResultSummary] [ntext] NULL ,
        CONSTRAINT [aaaaatblBatch_PK] PRIMARY KEY NONCLUSTERED
          ( [BatchId] ASC ) WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF )
          ON [PRIMARY]
      )
    ON
      [PRIMARY] -- TEXTIMAGE_ON [PRIMARY]
  END
GO
--CREATE TABLE [dbo].[tblBatch](
--	[BatchId] [nvarchar](255) NOT NULL,
--	[BatchName] [nvarchar](255) NULL,
--	[AssignedTo] [nvarchar](255) NULL,
--	[ExportPath] [nvarchar](255) NULL,
--	[CreateDate] [datetime] NULL DEFAULT (getdate()),
-- CONSTRAINT [aaaaatblBatch_PK] PRIMARY KEY NONCLUSTERED 
--(
--	[BatchId] ASC
--)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] -- TEXTIMAGE_ON [PRIMARY]
--END
--GO
--  Set Extended Properties

--  BatchId
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'Batch Id', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'BatchId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblBatch',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', N'COLUMN',
                                                N'BatchId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO

--  BatchName
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', N'COLUMN',
                                                N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchName',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'BatchName', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblBatch',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', N'COLUMN',
                                                N'BatchName') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'BatchName'
GO

--  AssignedTo
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', N'COLUMN',
                                                N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'AssignedTo',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'AssignedTo', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblBatch',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', N'COLUMN',
                                                N'AssignedTo') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'AssignedTo'
GO

----  ExportPath
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'AggregateType' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'AllowZeroLength' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'AppendOnly' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'Attributes' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'CollatingOrder' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'ColumnHidden' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'ColumnOrder' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'ColumnWidth' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'DataUpdatable' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DisplayControl' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_IMEMode' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_IMESentMode' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'Name' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'ExportPath' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'OrdinalPosition' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'Required' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'Size' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'SourceField' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'ExportPath' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'SourceTable' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'tblBatch' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'TextAlign' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'Type' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO
--IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'UnicodeCompression' , N'SCHEMA',N'dbo', N'TABLE',N'tblBatch', N'COLUMN',N'ExportPath'))
--EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblBatch', @level2type=N'COLUMN',@level2name=N'ExportPath'
--GO

--  CreateDate
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DefaultValue', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'CreateDate',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'17',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'CreateDate', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblBatch',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch',
                                                N'COLUMN', N'CreateDate') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch', @level2type = N'COLUMN',
    @level2name = N'CreateDate'
GO

--  Misc
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 10:59:21 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblBatch', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/28/2010 1:21:08 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblBatch', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblBatch',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblBatch',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblBatch'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblAudit]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblAudit]
      (
        [AuditId] [int] IDENTITY(1, 1)
                        NOT NULL ,
        [BatchItemId] [int] NULL ,
        [AuditAction] [nvarchar](255) NULL ,
        [Comments] [ntext] NULL ,
        [AuditDateTime] [datetime] NULL
                                   DEFAULT ( GETDATE() ) ,
        CONSTRAINT [aaaaatblAudit_PK] PRIMARY KEY NONCLUSTERED
          ( [AuditId] ASC ) WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF )
          ON [PRIMARY]
      )
    ON
      [PRIMARY] TEXTIMAGE_ON [PRIMARY]
  END
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'17',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'AuditId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'AuditId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblAudit',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DecimalPlaces', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblAudit', N'COLUMN',
                                                N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'BatchItemId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'BatchItemId', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblAudit',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'BatchItemId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'BatchItemId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Description', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'i.e. Changed Failed to ''Not Processed''',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblAudit', N'COLUMN',
                                                N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'AuditAction',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'AuditAction', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblAudit',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblAudit', N'COLUMN',
                                                N'AuditAction') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditAction'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'Comments',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'Comments',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblAudit',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextFormat', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'TextFormat', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'12',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblAudit', N'COLUMN',
                                                N'Comments') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'Comments'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DefaultValue', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'AuditDateTime',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ShowDatePicker', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'ShowDatePicker', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'AuditDateTime', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable', @value = N'tblAudit',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit',
                                                N'COLUMN', N'AuditDateTime') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'8',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit', @level2type = N'COLUMN',
    @level2name = N'AuditDateTime'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 1:17:26 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblAudit', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/29/2010 9:29:38 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblAudit', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblAudit',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE', N'tblAudit',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblAudit'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblVersion]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblVersion]
      (
        [SchemaVersion] [nvarchar](255) NULL ,
        [ApplicationVersion] [nvarchar](255) NULL
      )
    ON
      [PRIMARY]
  END
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'1860',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN', N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'SchemaVersion',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN', N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN', N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'SchemaVersion', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblVersion', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN', N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'SchemaVersion') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'SchemaVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'2205',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Name',
    @value = N'ApplicationVersion', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ApplicationVersion', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblVersion', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion',
                                                N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion', @level2type = N'COLUMN',
    @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblVersion', N'COLUMN',
                                                N'ApplicationVersion') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion',
    @level2type = N'COLUMN', @level2name = N'ApplicationVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 1:03:47 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/27/2010 3:44:55 PM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblVersion', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'tblVersion',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblVersion', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblVersion'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblProjectJobRel]')
                        AND type IN ( N'U' ) )
  BEGIN
    CREATE TABLE [dbo].[tblProjectJobRel]
      (
        [ProjectId] [nvarchar](255) NOT NULL ,
        [JobId] [nvarchar](255) NOT NULL ,
        CONSTRAINT [aaaaatblProjectJobRel_PK] PRIMARY KEY NONCLUSTERED
          ( [ProjectId] ASC, [JobId] ASC )
          WITH ( PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF ) ON [PRIMARY]
      )
    ON
      [PRIMARY]
  END
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProjectJobRel',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'ProjectId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField',
    @value = N'ProjectId', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProjectJobRel',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProjectJobRel', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'ProjectId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProjectJobRel',
    @level2type = N'COLUMN', @level2name = N'ProjectId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AggregateType', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AggregateType', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AllowZeroLength', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'AppendOnly', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'AppendOnly', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'CollatingOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnHidden', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnHidden', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnOrder', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnOrder', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'ColumnWidth', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DataUpdatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'DataUpdatable', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DisplayControl',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_DisplayControl',
    @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProjectJobRel',
    @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMEMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_IMESentMode', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Name', @value = N'JobId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrdinalPosition', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Required', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Required', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Size', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Size', @value = N'255',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceField', @value = N'JobId',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'SourceTable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'SourceTable',
    @value = N'tblProjectJobRel', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TextAlign', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'TextAlign', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Type', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                N'COLUMN', N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'Type', @value = N'10',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel', @level2type = N'COLUMN',
    @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'UnicodeCompression',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProjectJobRel', N'COLUMN',
                                                N'JobId') )
  EXEC sys.sp_addextendedproperty @name = N'UnicodeCompression',
    @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProjectJobRel',
    @level2type = N'COLUMN', @level2name = N'JobId'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Attributes', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Attributes', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DateCreated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DateCreated',
    @value = N'1/27/2010 10:57:54 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'DisplayViewsOnSharePointSite',
                                                N'SCHEMA', N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite',
    @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'FilterOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'HideNewField', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'HideNewField', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'LastUpdated', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'LastUpdated',
    @value = N'1/27/2010 10:59:17 AM', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DefaultView', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_OrderByOn', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_Orientation', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_Orientation', @value = N'0',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Name', N'SCHEMA', N'dbo',
                                                N'TABLE', N'tblProjectJobRel',
                                                NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Name',
    @value = N'tblProjectJobRel', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'OrderByOnLoad', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'RecordCount', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'RecordCount', @value = N'1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'TotalsRow', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'TotalsRow', @value = N'False',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'
GO
IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'Updatable', N'SCHEMA',
                                                N'dbo', N'TABLE',
                                                N'tblProjectJobRel', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'Updatable', @value = N'True',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'tblProjectJobRel'


/****** Object:  Table [dbo].[tblRepository]    Script Date: 10/10/2011 09:12:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[tblRepository]')
                    AND type IN ( N'U' ) )
  DROP TABLE [dbo].[tblRepository]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblRepository]
  (
    [RepositoryId] [nvarchar](255) NOT NULL ,
    [RepositoryName] [nvarchar](255) NOT NULL ,
    [ConnectionString] [nvarchar](3000) NULL ,
    [Repository] [varbinary](MAX) NOT NULL ,
    [LastUpdateDate] [datetime] NULL
  )
ON
  [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[tblJobRepositoryRel]')
                    AND type IN ( N'U' ) )
  DROP TABLE [dbo].[tblJobRepositoryRel]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblJobRepositoryRel]
  (
    [JobId] [nvarchar](255) NULL ,
    [RepositoryId] [nvarchar](255) NULL ,
    [JobScope] [nvarchar](12) NULL
  )
ON
  [PRIMARY]

GO

