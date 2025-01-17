IF EXISTS ( SELECT  *
            FROM    sys.types
            WHERE   is_user_defined = 1
                    AND name = 'NewBatchItemsTableType' )
    IF EXISTS ( SELECT  *
                FROM    dbo.sysobjects
                WHERE   id = OBJECT_ID(N'[usp_AddBatchItems]')
                        AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
        DROP PROCEDURE usp_AddBatchItems
DROP TYPE NewBatchItemsTableType

GO

CREATE TYPE NewBatchItemsTableType AS TABLE
(BatchId NVARCHAR(255), Title NVARCHAR(255), SourceDocId NVARCHAR(255), ProcessedStatus NVARCHAR(50), Operation NVARCHAR(255))

GO

IF EXISTS ( SELECT  *
            FROM    dbo.sysobjects
            WHERE   id = OBJECT_ID(N'[usp_AddBatchItems]')
                    AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
    DROP PROCEDURE usp_AddBatchItems
GO

CREATE PROCEDURE usp_AddBatchItems
    @tvpNewBatchItems dbo.NewBatchItemsTableType READONLY
AS
    INSERT  INTO dbo.tblBatchItems
            ( BatchId ,
              Title ,
              SourceDocId ,
              ProcessedStatus ,
              Operation
            )
            SELECT  BatchId ,
                    Title ,
                    SourceDocId ,
                    ProcessedStatus ,
                    Operation
            FROM    @tvpNewBatchItems AS NBI;