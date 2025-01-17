

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Added this index on June 28, 2014 to speed up the process of getting summary counts
/****** Object:  Index [_dta_index_tblBatchItems_28_421576540__K2]    Script Date: 6/28/2014 2:14:37 PM ******/
IF NOT EXISTS ( SELECT  *
                FROM    sys.indexes
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblBatchItems]')
                        AND name = N'_dta_index_tblBatchItems_28_421576540__K2' )
  CREATE NONCLUSTERED INDEX [_dta_index_tblBatchItems_28_421576540__K2] ON [dbo].[tblBatchItems]
  (
  [BatchId] ASC
  )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  UserDefinedFunction [dbo].[usf_get_start_time]    Script Date: 6/27/2014 4:30:29 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usf_get_start_time]')
                    AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  DROP FUNCTION [dbo].[usf_get_start_time]
GO

/****** Object:  UserDefinedFunction [dbo].[usf_get_start_time]    Script Date: 6/28/2014 12:58:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usf_get_start_time]')
                        AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  BEGIN
    EXECUTE dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <June 27, 2014>
-- Description:	<Gets the start time>
-- =============================================
CREATE FUNCTION [dbo].[usf_get_start_time] (@Id NVARCHAR(255), @SourceTableName NVARCHAR(255))

RETURNS DATETIME

AS
    BEGIN
 
        DECLARE @workSummary NVARCHAR(1000)

        IF @SourceTableName = ''tblBatch''
           BEGIN
              SELECT  @workSummary = WorkSummary
              FROM    dbo.tblBatch
              WHERE   BatchId = @Id
            END

        IF @SourceTableName = ''tblJob''
           BEGIN
              SELECT  @workSummary = WorkSummary
              FROM    dbo.tblJob
              WHERE   JobId = @Id
            END

        IF @SourceTableName = ''tblProject''
           BEGIN
              SELECT  @workSummary = WorkSummary
              FROM    dbo.tblProject
              WHERE   ProjectId = @Id
            END


        DECLARE @startDateString VARCHAR(1000) = dbo.usf_get_value_from_key_value_pairs(@workSummary,
                                                              ''Start'', '';'')
        DECLARE @startDate DATETIME 

        IF @startDateString IS NOT NULL
            SET @startDate = CONVERT(DATETIME, @startDateString, 126)
        ELSE
          SET @startDate = GETDATE()

        RETURN  @startDate

    END    ' 
  END

GO

/****** Object:  UserDefinedFunction [dbo].[usf_format_work_summary]    Script Date: 6/27/2014 4:31:37 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usf_format_work_summary]')
                    AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  DROP FUNCTION [dbo].[usf_format_work_summary]
GO

/****** Object:  UserDefinedFunction [dbo].[usf_create_work_summary]    Script Date: 7/3/2014 8:17:11 AM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usf_create_work_summary]')
                    AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  DROP FUNCTION [dbo].[usf_create_work_summary]
GO

/****** Object:  UserDefinedFunction [dbo].[usf_create_work_summary]    Script Date: 7/3/2014 8:17:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usf_create_work_summary]')
                        AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  BEGIN
    EXECUTE dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <02/06/2014>
-- Description:	<Generates a work summary string>
-- =============================================
CREATE FUNCTION [dbo].[usf_create_work_summary]
  (
    @successcount NVARCHAR(255) ,
    @failedcount NVARCHAR(255) ,
    @notprocessedcount NVARCHAR(255) ,
    @processingcount NVARCHAR(255) ,
    @totalitemcount NVARCHAR(255) ,
    @avgprocessingtime NVARCHAR(255) ,
    @lastSummary NVARCHAR(MAX) ,
    @startTime DATETIME ,
    @finishTime DATETIME
  )
RETURNS NVARCHAR(MAX)
AS
    BEGIN
    -- Declare the return variable here
      DECLARE @WorkSummary NVARCHAR(MAX)

      DECLARE @currentUpdateTime DATETIME = GETDATE()

      SET @WorkSummary = ''SuccessCount='' + ISNULL(@successcount, 0) + 
      '';FailedCount='' + ISNULL(@failedcount, 0) + 
      '';NotProcessedCount='' + ISNULL(@notprocessedcount, 0) + 
      '';ProcessingCount='' + ISNULL(@processingcount, 0) + 
      '';TotalItemCount='' + ISNULL(@totalitemcount, 0) + 
      '';AvgProcessingTime='' + ISNULL(@avgprocessingtime, 0) +
      '';Start='' + CONVERT(VARCHAR, @startTime, 126)

      SET @WorkSummary = @WorkSummary + '';LastUpdate='' + CONVERT(VARCHAR, GETDATE(), 126)
      
      DECLARE @lastSummaryPosition BIGINT = 0
        
      IF @lastSummary IS NOT NULL
        SET @lastSummaryPosition = CHARINDEX(''LastUpdate'', @lastSummary)

        IF @lastSummaryPosition > 0
          BEGIN
            --DECLARE @lastSummaryUpdateString VARCHAR(1000) = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''LastUpdate'', '';'')
            DECLARE @lastChangedString VARCHAR(1000) = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''LastChanged'', '';'')

            IF @lastChangedString IS NULL
              SET @lastChangedString = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''LastUpdate'', '';'')

            DECLARE @lastDelta VARCHAR(1000) = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''LastDelta'', '';'')
            IF @lastDelta IS NULL
              SET @lastDelta = ''0''

            DECLARE @lastChanged DATETIME = CONVERT(DATETIME, @lastChangedString, 126)

            DECLARE @updateIntervalSeconds FLOAT = DATEDIFF(ss, @lastChanged, @currentUpdateTime)
            
            DECLARE @lastSuccessCount INT = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''SuccessCount'', '';'')
            DECLARE @lastTotalItemsCount INT = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''TotalItemCount'', '';'')

            DECLARE @lastNotProcessedCount INT = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''NotProcessedCount'', '';'')

            DECLARE @currentProcessedCount INT = CONVERT(INT, @totalitemcount) - CONVERT(INT, @notprocessedcount)
            --DECLARE @currentProcessedCount INT = CONVERT(INT, @successcount)
            --DECLARE @lastProcessedCount INT = @lastTotalItemsCount - @lastNotProcessedCount
            DECLARE @lastProcessedCount INT = @lastTotalItemsCount - @lastNotProcessedCount

            DECLARE @processedDelta INT = @currentProcessedCount - @lastProcessedCount
            DECLARE @successDelta INT = @successcount - @lastSuccessCount
            DECLARE @documentsPerSecond FLOAT

            IF @notprocessedcount = 0
              SET @documentsPerSecond = 0
            ELSE
              BEGIN
                IF @updateIntervalSeconds = 0
                  SET @documentsPerSecond = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''Rate'', '';'')
                ELSE
                  --SET @documentsPerSecond = @processedDelta / @updateIntervalSeconds
                  IF @successDelta > 0
                    SET @documentsPerSecond = @successDelta / @updateIntervalSeconds
                  ELSE
                    SET @documentsPerSecond = @lastDelta / @updateIntervalSeconds    
              END

            IF  @successDelta > 0
              BEGIN
                SET @WorkSummary = @WorkSummary + '';LastChanged='' + CONVERT(VARCHAR, GETDATE(), 126)
                SET @WorkSummary = @WorkSummary + '';LastDelta='' + CONVERT(VARCHAR(255), @processedDelta)
              END
                ELSE
                  BEGIN
                    SET @WorkSummary = @WorkSummary + '';LastChanged='' + @lastChangedString
                    SET @WorkSummary = @WorkSummary + '';LastDelta='' + @lastDelta
                  END

            SET @WorkSummary = @WorkSummary + '';Rate='' + CONVERT(VARCHAR(255), @documentsPerSecond)

            DECLARE @lastPeakProcessingRate FLOAT = 0
            IF CHARINDEX(''PeakRate'', @lastSummary) > 0
              SET  @lastPeakProcessingRate = dbo.usf_get_value_from_key_value_pairs(@lastsummary,''PeakRate'', '';'')

            DECLARE @peakProcessingRate FLOAT = @lastPeakProcessingRate
            IF @documentsPerSecond > @peakProcessingRate
              BEGIN
                SET @peakProcessingRate = @documentsPerSecond
              END
            SET @WorkSummary = @WorkSummary + '';PeakRate='' + CONVERT(VARCHAR(255), @peakProcessingRate)

            IF @finishTime IS NOT NULL
              SET @WorkSummary = @WorkSummary + '';Finish='' + CONVERT(VARCHAR, @finishTime, 126)
            ELSE
              BEGIN
                DECLARE @SecondsToFinish REAL
                IF @documentsPerSecond <> 0
                  BEGIN
                    SET @SecondsToFinish = (CONVERT(INT, @notprocessedcount) / @documentsPerSecond)
                    DECLARE @ProjectedFinishTime DATETIME = DATEADD(s, @SecondsToFinish, GETDATE())
                    SET @WorkSummary = @WorkSummary + '';Finish='' + CONVERT(VARCHAR, @ProjectedFinishTime, 126)
                  END
                ELSE
                  SET @WorkSummary = @WorkSummary + '';Finish=''
              END

          END
      
      -- Return the result of the function
      RETURN @WorkSummary

    END
' 
  END

GO



IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usf_get_job_view_name]')
                    AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  DROP FUNCTION [dbo].[usf_get_job_view_name]
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usf_get_job_view_name]')
                        AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  BEGIN
    EXECUTE dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <11/27/2013>
-- Description:	<Generates view name for a job>
-- =============================================
CREATE FUNCTION [dbo].[usf_get_job_view_name] ( @jobname NVARCHAR(255) )
RETURNS NVARCHAR(255)
AS
    BEGIN
  -- Declare the return variable here
        DECLARE @JobViewName NVARCHAR(255)

  -- Add the T-SQL statements to compute the return value here
        SET @JobViewName = ''jvw'' + REPLACE(@jobname, ''.'', ''_'')
        SET @JobViewName = REPLACE(@JobViewName, '' '', '''')
        SET @JobViewName = REPLACE(@JobViewName, '''''''', '''''''''''')

  -- Return the result of the function
        RETURN @JobViewName

    END

' 
  END

GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_batch]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_delete_batch]
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_batch]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <November 26, 2013>
-- Description:	<Deletes a batch by it''s batch id>
-- =============================================
CREATE PROCEDURE [dbo].[usp_delete_batch] @batchid NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

-- Make sure the batch exists
        DECLARE @BatchCount INT

        SET @BatchCount = ( SELECT  COUNT(*)
                            FROM    dbo.tblBatch
                            WHERE   BatchId = @batchid
                          )
        IF @BatchCount = 0
            RETURN -100

-- Delete all the batch items for the batch
        DELETE  FROM dbo.tblBatchItems
        WHERE   BatchId = @batchid

-- Delete all the job/batch relationships for the batch
        DELETE  FROM dbo.tblJobBatchRel
        WHERE   BatchId = @batchid

-- Delete the batch
        DELETE  FROM dbo.tblBatch
        WHERE   BatchId = @batchid

-- Delete any remaining batch locks for the batch
        DELETE  FROM dbo.tblBatchLock
        WHERE   BatchId = @batchid

        RETURN 1

    END

' 
  END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_job]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_delete_job]
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_job]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <November 26, 2013>
-- Description:	<Deletes a job by its id>
-- =============================================
CREATE PROCEDURE [dbo].[usp_delete_job] @jobid NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

  -- Make sure the job exists
        DECLARE @JobCount INT

        SET @JobCount = ( SELECT    COUNT(*)
                          FROM      dbo.tblJob
                          WHERE     JobId = @jobid
                        )
        IF @JobCount = 0
            RETURN -100

  -- Delete all the existing batches
        DECLARE @BatchId NVARCHAR(255)
        DECLARE batch_cursor CURSOR FAST_FORWARD
        FOR
            SELECT  BatchId
            FROM    dbo.tblJobBatchRel
            WHERE   JobId = @jobid

        OPEN batch_cursor

        FETCH NEXT FROM batch_cursor INTO @BatchId

        WHILE @@FETCH_STATUS = 0
            BEGIN

-- Delete the batch
                EXEC dbo.usp_delete_batch @BatchId

                FETCH NEXT FROM batch_cursor INTO @BatchId
            END

        CLOSE batch_cursor
        DEALLOCATE batch_cursor

-- Get the job name
        DECLARE @JobName NVARCHAR(255)
        SET @JobName = ( SELECT JobName
                         FROM   tblJob
                         WHERE  JobId = @jobid
                       )


-- Delete the relationship of this job to the project.
        DELETE  FROM dbo.tblProjectJobRel
        WHERE   JobId = @jobid

-- Delete the entry from the job table.
        DELETE  FROM dbo.tblJob
        WHERE   JobId = @jobid

-- If we have a files table we need to delete any job 
-- related entries in there as well.
        IF EXISTS ( SELECT  *
                    FROM    sys.objects
                    WHERE   object_id = OBJECT_ID(N''[dbo].[tblFiles]'')
                            AND type IN ( N''U'' ) )
            BEGIN 
                DELETE  FROM tblFiles
                WHERE   JobId = @jobid
            END 

-- If we have a view for this job we need to delete it as well

        DECLARE @JobViewName NVARCHAR(255)
        SET @JobViewName = dbo.usf_get_job_view_name(@JobName)

        IF EXISTS ( SELECT  TABLE_NAME
                    FROM    INFORMATION_SCHEMA.VIEWS
                    WHERE   TABLE_NAME = @JobViewName )
            EXEC (''DROP VIEW ['' + @JobViewName + '']'')

        RETURN 1

    END
' 
  END


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[[usp_save_batch]]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_delete_batch]
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_batch]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 3, 2013>
-- Description:	<Creates a batch >
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_batch]
    @batchid NVARCHAR(255) ,
    @batchname NVARCHAR(255) ,
    @assignedto NVARCHAR(255) ,
    @jobid NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

  -- Check if batch already exists in db, if it does, do an update, else do an insert
        DECLARE @BatchCount INT
        SET @BatchCount = ( SELECT  COUNT(*)
                            FROM    dbo.tblBatch
                            WHERE   BatchId = @batchid
                          )

        IF @BatchCount = 0
            INSERT  INTO dbo.tblBatch
                    ( BatchId, BatchName )
            VALUES  ( @batchid, @batchname )
        ELSE
            UPDATE  dbo.tblBatch
            SET     BatchName = @batchname ,
                    AssignedTo = @assignedto
            WHERE   BatchId = @batchid

        IF @@ROWCOUNT = 0
            RETURN -100

  -- Check if batch / job relationship already exists in db
        DECLARE @JobBatchRelCount INT
        SET @JobBatchRelCount = ( SELECT    COUNT(*)
                                  FROM      dbo.tblJobBatchRel
                                  WHERE     BatchId = @batchid
                                            AND JobId = @jobid
                                )

        IF @JobBatchRelCount = 0
            INSERT  INTO dbo.tblJobBatchRel
                    ( BatchId, JobId )
            VALUES  ( @batchid, @jobid )
    END
    '
  END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_job]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_job]
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_job]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 3, 2013>
-- Description:	<Creates or saves a job >
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_job]
    @projectid NVARCHAR(255) ,
    @jobid NVARCHAR(255) OUTPUT ,
    @jobname NVARCHAR(255) ,
    @operationname NVARCHAR(255) ,
    @previousjobname NVARCHAR(255) ,
    @jobconfiguration NTEXT
AS
    BEGIN
      -- If we were not passed a job id then try to look it up based on the name
        IF NULLIF(@jobid, '''') IS NULL
            SET @jobid = ( SELECT   JobId
                           FROM     dbo.tblJob
                           WHERE    JobName = @jobname
                         )

        IF NULLIF(@jobid, '''') IS NULL
            BEGIN
            -- This job does not yet exist in the database, we will create it.
                SET @jobid = LOWER(NEWID())
                INSERT  INTO dbo.tblJob
                        ( JobId ,
                          JobName ,
                          Operation ,
                          Configuration
                        )
                VALUES  ( @jobid ,
                          @jobname ,
                          @operationname ,
                          @jobconfiguration
                        )   
                        
                INSERT  INTO dbo.tblProjectJobRel 
                        ( ProjectId ,
                          JobId
                        )
                VALUES  ( @projectid ,
                          @jobid
                        )                              

            END
        ELSE
            BEGIN
                UPDATE  dbo.tblJob
                SET     JobName = @jobname ,
                        Operation = @operationname ,
                        Configuration = @jobconfiguration
                WHERE   JobId = @jobid
            END
        -- EXEC dbo.usp_create_job_view @jobname

        IF ( ( NULLIF(@previousjobname, '''') IS NULL )
             AND ( @jobname <> @previousjobname )
           )
            BEGIN
                DECLARE @PreviousJobViewName NVARCHAR(255)
                SET @PreviousJobViewName = dbo.usf_get_job_view_name(@previousjobname)
                IF EXISTS ( SELECT  TABLE_NAME
                            FROM    INFORMATION_SCHEMA.VIEWS
                            WHERE   TABLE_NAME = @PreviousJobViewName )
                    EXEC (''DROP VIEW ['''' + @PreviousJobViewName + '''']'')
            END

    END' 

  END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_job_relationship]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_job_relationship]
GO

/****** Object:  StoredProcedure [dbo].[usp_save_job_relationship]    Script Date: 8/20/2015 1:53:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_job_relationship]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <August 20, 2015>
-- Description:	<Creates or saves a job relationship>
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_job_relationship]
    @relationshipid NVARCHAR(255) ,
    @relationshipname NVARCHAR(255) ,
    @relationshipdescription NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

  -- Check if relationship already exists in db, if it does, do an update, else do an insert
        DECLARE @RelationshipCount INT
        SET @RelationshipCount = ( SELECT  COUNT(*)
                            FROM    dbo.tblJobRelationships
                            WHERE   JobRelationshipId = @relationshipid
                          )

        IF @RelationshipCount = 0
            INSERT  INTO dbo.tblJobRelationships
                    ( JobRelationshipId, JobRelationshipName, JobRelationshipDescription )
            VALUES  ( @relationshipid, @relationshipname, @relationshipdescription )
        ELSE
            UPDATE  dbo.tblJobRelationships
            SET     JobRelationshipName = @relationshipname ,
                    JobRelationshipDescription = @relationshipdescription
            WHERE   JobRelationshipId = @relationshipid

        IF @@ROWCOUNT = 0
            RETURN -100

    END
    ' 
  END
GO

/****** Object:  UserDefinedTableType [dbo].[RelatedJobsList]    Script Date: 8/20/2015 2:51:14 PM ******/
IF NOT EXISTS ( SELECT  *
                FROM    sys.types st
                        JOIN sys.schemas ss ON st.schema_id = ss.schema_id
                WHERE   st.name = N'RelatedJobsList'
                        AND ss.name = N'dbo' )
  CREATE TYPE [dbo].[RelatedJobsList] AS TABLE(
  [JobRelationshipID] [nvarchar](255) NULL,
  [JobId] [nvarchar](255) NULL
  )
GO

/****** Object:  StoredProcedure [dbo].[usp_save_related_jobs]    Script Date: 8/20/2015 2:32:40 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_related_jobs]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_related_jobs]
GO

/****** Object:  StoredProcedure [dbo].[usp_save_related_jobs]    Script Date: 8/20/2015 2:32:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_related_jobs]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <August 20, 2015>
-- Description:	<Saves related jobs>
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_related_jobs]
    @relationshipid NVARCHAR(255) ,
    @relatedjobs AS [dbo].[RelatedJobsList] READONLY
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

  -- Check if relationship already exists in db, if it does, do an update, else do an insert
        DECLARE @RelationshipCount INT
        SET @RelationshipCount = ( SELECT  COUNT(*)
                            FROM    dbo.tblJobRelationships
                            WHERE   JobRelationshipId = @relationshipid
                          )

        IF @RelationshipCount = 0
            RETURN -500
        ELSE
            
            -- Clear all the existing items for this relationship
            -- We will then insert all the items given to us
            DELETE FROM dbo.tblRelatedJobs
            WHERE  JobRelationshipId = @relationshipid

            INSERT INTO dbo.tblRelatedJobs(JobRelationshipId, JobId)
            SELECT JobRelationshipId, JobId FROM @RelatedJobs
            WHERE   JobRelationshipId = @relationshipid

        IF @@ROWCOUNT = 0
            RETURN -100

    END
    ' 
  END
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_detailed_project_summary_counts]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].usp_get_detailed_project_summary_counts
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_detailed_project_summary_counts]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 11, 2013>
-- Description:	<Gets the detailed project summary counts>
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_detailed_project_summary_counts]
    @projectId NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

DECLARE @JobName NVARCHAR(255)
DECLARE @Operation NVARCHAR(255)
DECLARE @TotalItemCount NVARCHAR(255)
DECLARE @SuccessCount NVARCHAR(255)
DECLARE @FailedCount NVARCHAR(255)
DECLARE @NotProcessedCount NVARCHAR(255)
DECLARE @ProcessingCount NVARCHAR(255)
DECLARE @TotalProcessingTime NVARCHAR(255)
DECLARE @AvgProcessingTime NVARCHAR(255)

DECLARE @WorkSummary NVARCHAR(255)

-- We are going to loop through each job and reset its work summary.
DECLARE JobSummaryCursor CURSOR
FOR
  SELECT  JobName ,
          Operation ,
          ISNULL(TotalItemCount, 0) AS TotalItemCount ,
          ISNULL(SuccessCount, 0) AS SuccessCount ,
          ISNULL(FailedCount, 0) AS FailedCount ,
          ISNULL(NotProcessedCount, 0) AS NotProcessedCount ,
          ISNULL(ProcessingCount, 0) AS ProcessingCount ,
          ISNULL(AvgProcessingTime, 0) AS AvgProcessingTime
  FROM    tblJob j
          LEFT OUTER JOIN ( SELECT  COUNT(*) AS SuccessCount ,
                                    jbr.JobId
                            FROM    tblBatchItems bi ( NOLOCK )
                                    JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                    JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                            WHERE   pjr.ProjectId = @projectId
                                    AND bi.ProcessedStatus = ''Success''
                            GROUP BY jbr.JobId
                          ) a ON a.JobId = j.JobId
          LEFT OUTER JOIN ( SELECT  COUNT(*) AS FailedCount ,
                                    jbr.JobId
                            FROM    tblBatchItems bi ( NOLOCK )
                                    JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                    JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                            WHERE   pjr.ProjectId = @projectId
                                    AND bi.ProcessedStatus = ''Failed''
                            GROUP BY jbr.JobId
                          ) b ON b.JobId = j.JobId
          LEFT OUTER JOIN ( SELECT  COUNT(*) AS NotProcessedCount ,
                                    jbr.JobId
                            FROM    tblBatchItems bi ( NOLOCK )
                                    JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                    JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                            WHERE   pjr.ProjectId = @projectId
                                    AND bi.ProcessedStatus = ''NotProcessed''
                            GROUP BY jbr.JobId
                          ) c ON c.JobId = j.JobId
          LEFT OUTER JOIN ( SELECT  COUNT(*) AS ProcessingCount ,
                                    jbr.JobId
                            FROM    tblBatchItems bi ( NOLOCK )
                                    JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                    JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                            WHERE   pjr.ProjectId = @projectId
                                    AND bi.ProcessedStatus = ''Processing''
                            GROUP BY jbr.JobId
                          ) d ON d.jobId = j.JobId
          LEFT OUTER JOIN ( SELECT  COUNT(*) AS TotalItemCount ,
                                    jbr.JobId
                            FROM    tblBatchItems bi ( NOLOCK )
                                    JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                    JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                            WHERE   pjr.ProjectId = @projectId
                            GROUP BY jbr.JobId
                          ) e ON e.jobId = j.JobId
          LEFT OUTER JOIN ( SELECT  ISNULL(SUM(TotalProcessingTime) / COUNT(*), 0) AS AvgProcessingTime ,
                                    jbr.JobId
                            FROM    tblBatchItems bi ( NOLOCK )
                                    JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                    JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                            WHERE   pjr.ProjectId = @projectId
                                    AND bi.ProcessedStatus IN ( ''Success'',
                                                              ''Failed'' )
                            GROUP BY jbr.JobId
                          ) tpt ON tpt.jobId = j.JobId
  ORDER BY j.JobName

OPEN JobSummaryCursor

FETCH NEXT FROM JobSummaryCursor

INTO @JobName, @Operation, @TotalItemCount, @SuccessCount, @FailedCount,
  @NotProcessedCount, @ProcessingCount, @AvgProcessingTime

IF NULLIF(@AvgProcessingTime, '''') IS NULL
  SET @AvgProcessingTime = 0

    DECLARE @LastWorkSummary NVARCHAR(1000)
    DECLARE @JobStartTime DATETIME

    DECLARE @jobid VARCHAR(255)
    SELECT @jobid = JobId FROM tblJob WHERE JobName = @JobName
    SELECT @LastWorkSummary = WorkSummary FROM tblJob WHERE JobName = @JobName

    SET @JobStartTime = dbo.usf_get_start_time(@jobId, ''tblJob'')

    SET @WorkSummary = dbo.usf_create_work_summary(@SuccessCount, @FailedCount,
                                               @NotProcessedCount,
                                               @ProcessingCount,
                                               @TotalItemCount,
                                               @AvgProcessingTime, 
                                               @LastWorkSummary,
                                               @JobStartTime,
                                               NULL)


UPDATE  tblJob
SET     WorkSummary = @WorkSummary
WHERE   JobName = @JobName

WHILE @@FETCH_STATUS = 0
  BEGIN

    FETCH NEXT FROM JobSummaryCursor

INTO @JobName, @Operation, @TotalItemCount, @SuccessCount, @FailedCount,
      @NotProcessedCount, @ProcessingCount, @AvgProcessingTime

IF NULLIF(@AvgProcessingTime, '''') IS NULL
      SET @AvgProcessingTime = 0

    SELECT @jobid = JobId FROM tblJob WHERE JobName = @JobName
    SELECT @LastWorkSummary = WorkSummary FROM tblJob WHERE JobName = @JobName

    SET @JobStartTime = dbo.usf_get_start_time(@jobId, ''tblJob'')

    SET @WorkSummary = dbo.usf_create_work_summary(@SuccessCount, @FailedCount,
                                               @NotProcessedCount,
                                               @ProcessingCount,
                                               @TotalItemCount,
                                               @AvgProcessingTime, 
                                               @LastWorkSummary,
                                               @JobStartTime,
                                               NULL)

    UPDATE  tblJob
    SET     WorkSummary = @WorkSummary
    WHERE   JobName = @JobName

  END

CLOSE JobSummaryCursor

DEALLOCATE JobSummaryCursor



        SELECT  JobName ,
                Operation ,
                ISNULL(TotalItemCount, 0) AS TotalItemCount ,
                ISNULL(SuccessCount, 0) AS SuccessCount ,
                ISNULL(FailedCount, 0) AS FailedCount ,
                ISNULL(NotProcessedCount, 0) AS NotProcessedCount ,
                ISNULL(ProcessingCount, 0) AS ProcessingCount ,
                AvgProcessingTime
        FROM    tblJob j
                LEFT OUTER JOIN ( SELECT    COUNT(*) AS SuccessCount ,
                                            jbr.JobId
                                  FROM      tblBatchItems bi ( NOLOCK )
                                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                                  WHERE     pjr.ProjectId = @projectId
                                            AND bi.ProcessedStatus = ''Success''
                                  GROUP BY  jbr.JobId
                                ) a ON a.JobId = j.JobId
                LEFT OUTER JOIN ( SELECT    COUNT(*) AS FailedCount ,
                                            jbr.JobId
                                  FROM      tblBatchItems bi ( NOLOCK )
                                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                                  WHERE     pjr.ProjectId = @projectId
                                            AND bi.ProcessedStatus = ''Failed''
                                  GROUP BY  jbr.JobId
                                ) b ON b.JobId = j.JobId
                LEFT OUTER JOIN ( SELECT    COUNT(*) AS NotProcessedCount ,
                                            jbr.JobId
                                  FROM      tblBatchItems bi ( NOLOCK )
                                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                                  WHERE     pjr.ProjectId = @projectId
                                            AND bi.ProcessedStatus = ''NotProcessed''
                                  GROUP BY  jbr.JobId
                                ) c ON c.JobId = j.JobId
                LEFT OUTER JOIN ( SELECT    COUNT(*) AS ProcessingCount ,
                                            jbr.JobId
                                  FROM      tblBatchItems bi ( NOLOCK )
                                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                                  WHERE     pjr.ProjectId = @projectId
                                            AND bi.ProcessedStatus = ''Processing''
                                  GROUP BY  jbr.JobId
                                ) d ON d.jobId = j.JobId
                LEFT OUTER JOIN ( SELECT    COUNT(*) AS TotalItemCount ,
                                            jbr.JobId
                                  FROM      tblBatchItems bi ( NOLOCK )
                                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                                  WHERE     pjr.ProjectId = @projectId
                                  GROUP BY  jbr.JobId
                                ) e ON e.jobId = j.JobId
                LEFT OUTER JOIN ( SELECT    ISNULL((SUM(TotalProcessingTime) / COUNT(*)), 0) AS AvgProcessingTime ,
                                            jbr.JobId
                                  FROM      tblBatchItems bi ( NOLOCK )
                                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                                  WHERE     pjr.ProjectId = @projectId
                                            AND bi.ProcessedStatus IN (
                                            ''Success'', ''Failed'' )
                                  GROUP BY  jbr.JobId
                                ) tpt ON tpt.jobId = j.JobId
        ORDER BY j.JobName

    END
    ' 
  END
GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_avg_processing_time_project]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].usp_get_avg_processing_time_project
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_avg_processing_time_project]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 11, 2013>
-- Description:	<Gets the average processing time for the project>
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_avg_processing_time_project]
    @projectName NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @currentProjectName NVARCHAR(255)
        SET @currentProjectName = ( SELECT ProjectName
               FROM   dbo.tblProject
               WHERE  ProjectName = @projectName
             )

        IF NULLIF(@currentProjectName, '''') IS NULL
          SET @currentProjectName = ( SELECT ProjectName
                         FROM   dbo.tblProject
                         WHERE  ProjectId = @projectName
                       )

SELECT  ( SUM(BI.TotalProcessingTime) / COUNT(BI.ID) ) AS AvgProcessingTime
FROM    tblJobBatchRel AS JBR
        INNER JOIN tblBatchItems AS BI ON JBR.BatchId = BI.BatchId
        INNER JOIN tblProjectJobRel AS PJR ON JBR.JobId = PJR.JobId
        INNER JOIN tblProject AS P ON PJR.ProjectId = P.ProjectId
WHERE   P.ProjectName = @currentProjectName
        AND BI.ProcessedStatus IN ( ''Success'', ''Failed'' )
    END
    '
  END
GO


/****** Object:  StoredProcedure [dbo].[usp_get_project_summary_counts]    Script Date: 9/22/2014 4:26:53 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_project_summary_counts]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_get_project_summary_counts]
GO

/****** Object:  StoredProcedure [dbo].[usp_get_project_summary_counts]    Script Date: 9/22/2014 4:26:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_project_summary_counts]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 11, 2013>
-- Description:	<Gets the project summary counts>
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_project_summary_counts] @projectId NVARCHAR(255)
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @SuccessCount NVARCHAR(255)
        DECLARE @FailedCount NVARCHAR(255)
        DECLARE @NotProcessedCount NVARCHAR(255)
        DECLARE @ProcessingCount NVARCHAR(255)
        DECLARE @TotalItemCount NVARCHAR(255)
        DECLARE @TotalProcessingTime NVARCHAR(255)
        DECLARE @AvgProcessingTime NVARCHAR(255)

        SELECT  @SuccessCount=SuccessCount ,
                @FailedCount=FailedCount ,
                @NotProcessedCount=NotProcessedCount ,
                @ProcessingCount=ProcessingCount ,
                @TotalItemCount=TotalItemCount ,
                @TotalProcessingTime=TotalProcessingTime ,
                @AvgProcessingTime=AvgProcessingTime

        FROM    ( SELECT    COUNT(bi.ID) AS SuccessCount
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                            AND bi.ProcessedStatus = ''Success''
                ) a ,
                ( SELECT    COUNT(bi.ID) AS FailedCount
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                            AND bi.ProcessedStatus = ''Failed''
                ) b ,
                ( SELECT    COUNT(bi.ID) AS NotProcessedCount
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                            AND bi.ProcessedStatus = ''NotProcessed''
                ) c ,
                ( SELECT    COUNT(bi.ID) AS ProcessingCount
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                            AND bi.ProcessedStatus = ''Processing''
                ) d ,
                ( SELECT    COUNT(bi.ID) AS TotalItemCount
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                ) e ,
                ( SELECT    SUM(TotalProcessingTime) AS TotalProcessingTime
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                            AND bi.ProcessedStatus IN ( ''Success'', ''Failed'' )
                ) f ,
                ( SELECT    ISNULL((SUM(TotalProcessingTime) / COUNT(*)), 0) AS AvgProcessingTime
                  FROM      tblBatchItems bi ( NOLOCK )
                            JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
                            JOIN tblProjectJobRel pjr ON pjr.JobId = jbr.JobId
                  WHERE     pjr.ProjectId = @projectId
                            AND bi.ProcessedStatus IN ( ''Success'', ''Failed'' )
                ) g

        DECLARE @WorkSummary NVARCHAR(255)
        DECLARE @LastWorkSummary NVARCHAR(1000)
        DECLARE @ProjectStartTime DATETIME

        SELECT @LastWorkSummary = WorkSummary FROM tblProject WHERE ProjectId = @projectId
        SET @ProjectStartTime = dbo.usf_get_start_time(@projectId, ''tblProject'')

        SET @WorkSummary = dbo.usf_create_work_summary(@SuccessCount, @FailedCount,
                                                   @NotProcessedCount,
                                                   @ProcessingCount,
                                                   @TotalItemCount,
                                                   @AvgProcessingTime, 
                                                   @LastWorkSummary,
                                                   @ProjectStartTime,
                                                   NULL)
    
        UPDATE tblProject SET WorkSummary = @WorkSummary WHERE ProjectId = @projectId

    DECLARE @ProcessingRateString VARCHAR(10)
    DECLARE @PeakProcessingRateString VARCHAR(10)

    DECLARE @ProcessingRate FLOAT
    DECLARE @PeakProcessingRate FLOAT

    SET @ProcessingRateString = dbo.usf_get_value_from_key_value_pairs(@WorkSummary,''Rate'', '';'')
    SET @PeakProcessingRateString = dbo.usf_get_value_from_key_value_pairs(@WorkSummary,''PeakRate'', '';'')

    SET @ProcessingRate = CONVERT(FLOAT, @ProcessingRateString)
    SET @PeakProcessingRate = CONVERT(FLOAT, @PeakProcessingRateString)

    SELECT  @SuccessCount AS SuccessCount ,
            @FailedCount AS FailedCount ,
            @NotProcessedCount AS NotProcessedCount ,
            @ProcessingCount AS ProcessingCount ,
            @TotalItemCount AS TotalItemCount ,
            @AvgProcessingTime AS AvgProcessingTime ,
            GETDATE() AS LastUpdateTime ,
            @ProcessingRate AS ProcessingRate , 
            @PeakProcessingRate AS PeakProcessingRate

    END
' 
  END
GO

/****** Object:  StoredProcedure [dbo].[usp_get_job_summary_counts]    Script Date: 6/30/2014 4:54:12 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_job_summary_counts]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_get_job_summary_counts]
GO

/****** Object:  StoredProcedure [dbo].[usp_get_job_summary_counts]    Script Date: 6/30/2014 4:54:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_job_summary_counts]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <September 3, 2024>
-- Description:	<Gets the job summary counts>
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_job_summary_counts] @jobId NVARCHAR(255)
AS
  BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @SuccessCount NVARCHAR(255)
    DECLARE @FailedCount NVARCHAR(255)
    DECLARE @NotProcessedCount NVARCHAR(255)
    DECLARE @ProcessingCount NVARCHAR(255)
    DECLARE @TotalItemCount NVARCHAR(255)
    
	DECLARE @SuccessCountValue BIGINT
	DECLARE @FailedCountValue BIGINT

    DECLARE @TotalProcessingTime REAL
    DECLARE @AvgProcessingTime NVARCHAR(255)

    SELECT  @SuccessCount = SuccessCount ,
            @FailedCount = FailedCount ,
            @NotProcessedCount = NotProcessedCount ,
            @ProcessingCount = ProcessingCount ,
            @TotalItemCount = TotalItemCount ,
            @TotalProcessingTime = TotalProcessingTime --,
            --@AvgProcessingTime = AvgProcessingTime
    FROM    ( SELECT  COUNT(*) AS SuccessCount
              FROM    tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
              WHERE   jbr.JobId = @jobId
                      AND bi.ProcessedStatus = ''Success''
            ) a ,
            ( SELECT  COUNT(*) AS FailedCount
              FROM    tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
              WHERE   jbr.JobId = @jobId
                      AND bi.ProcessedStatus = ''Failed''
            ) b ,
            ( SELECT  COUNT(*) AS NotProcessedCount
              FROM    tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
              WHERE   jbr.JobId = @jobId
                      AND bi.ProcessedStatus = ''NotProcessed''
            ) c ,
            ( SELECT  COUNT(*) AS ProcessingCount
              FROM    tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
              WHERE   jbr.JobId = @jobId
                      AND bi.ProcessedStatus = ''Processing''
            ) d ,
            ( SELECT  COUNT(*) AS TotalItemCount
              FROM    tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
              WHERE   jbr.JobId = @jobId
            ) e ,
            ( SELECT  SUM(TotalProcessingTime) AS TotalProcessingTime
              FROM    tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
              WHERE   jbr.JobId = @jobId
                      AND bi.ProcessedStatus IN ( ''Success'', ''Failed'' )
            ) f --,
            --( SELECT  ISNULL(( SUM(TotalProcessingTime) / COUNT(*) ), 0) AS AvgProcessingTime
            --  FROM    tblBatchItems bi ( NOLOCK )
            --          JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId
            --  WHERE   jbr.JobId = @jobId
            --          AND bi.ProcessedStatus IN ( ''Success'', ''Failed'' )
            --) g

	SET @SuccessCountValue = CONVERT(BIGINT, @SuccessCount)
	SET @FailedCountValue = CONVERT(BIGINT, @FailedCount)


    IF (@SuccessCountValue + @FailedCountValue) > 0
      BEGIN
        SET @AvgProcessingTime = ISNULL(( @TotalProcessingTime / (@SuccessCountValue + @FailedCountValue) ), 0)
      END
    ELSE
      BEGIN
        SET @AvgProcessingTime = 0
      END

    DECLARE @WorkSummary NVARCHAR(1000)
    DECLARE @LastWorkSummary NVARCHAR(1000)
    DECLARE @JobStartTime DATETIME
    DECLARE @JobFinishTime DATETIME

    -- This should be a temporary assignment, we need to have a more accurate way to get the finish time.

    IF @NotProcessedCount = 0
      SET @JobFinishTime = GETDATE()

    SELECT  @LastWorkSummary = WorkSummary
    FROM    tblJob
    WHERE   JobId = @jobId
    SET @JobStartTime = dbo.usf_get_start_time(@jobId, ''tblJob'')

    SET @WorkSummary = dbo.usf_create_work_summary(@SuccessCount, @FailedCount,
                                                   @NotProcessedCount,
                                                   @ProcessingCount,
                                                   @TotalItemCount,
                                                   @AvgProcessingTime,
                                                   @LastWorkSummary,
                                                   @JobStartTime, NULL)
    
    UPDATE  tblJob
    SET     WorkSummary = @WorkSummary
    WHERE   JobId = @jobId

    DECLARE @ProcessingRateString VARCHAR(10)
    DECLARE @PeakProcessingRateString VARCHAR(10)

    DECLARE @ProcessingRate FLOAT
    DECLARE @PeakProcessingRate FLOAT

    SET @ProcessingRateString = dbo.usf_get_value_from_key_value_pairs(@WorkSummary,''Rate'', '';'')
    SET @PeakProcessingRateString = dbo.usf_get_value_from_key_value_pairs(@WorkSummary,''PeakRate'', '';'')

    SET @ProcessingRate = CONVERT(FLOAT, @ProcessingRateString)
    SET @PeakProcessingRate = CONVERT(FLOAT, @PeakProcessingRateString)

    SELECT  @SuccessCount AS SuccessCount ,
            @FailedCount AS FailedCount ,
            @NotProcessedCount AS NotProcessedCount ,
            @ProcessingCount AS ProcessingCount ,
            @TotalItemCount AS TotalItemCount ,
            @AvgProcessingTime AS AvgProcessingTime ,
            @JobStartTime AS StartTime ,
            @JobFinishTime AS FinishTime ,
            GETDATE() AS LastUpdateTime ,
            @ProcessingRate AS ProcessingRate , 
            @PeakProcessingRate AS PeakProcessingRate
  END         ' 
  END
GO

/****** Object:  StoredProcedure [dbo].[usp_get_batch_summary_counts]    Script Date: 6/29/2014 10:52:27 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_batch_summary_counts]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_get_batch_summary_counts]
GO

/****** Object:  StoredProcedure [dbo].[usp_get_batch_summary_counts]    Script Date: 6/29/2014 10:52:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_get_batch_summary_counts]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 11, 2013>
-- Description:	<Gets the batch summary counts>
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_batch_summary_counts] @batchId NVARCHAR(255)
AS
    BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
        SET NOCOUNT ON;

DECLARE @SuccessCount NVARCHAR(255)
DECLARE @FailedCount NVARCHAR(255)
DECLARE @NotProcessedCount NVARCHAR(255)
DECLARE @ProcessingCount NVARCHAR(255)
DECLARE @TotalItemCount NVARCHAR(255)
DECLARE @TotalProcessingTime NVARCHAR(255)
DECLARE @AvgProcessingTime NVARCHAR(255)

SELECT  @SuccessCount=SuccessCount ,
        @FailedCount=FailedCount ,
        @NotProcessedCount=NotProcessedCount ,
        @ProcessingCount=ProcessingCount ,
        @TotalItemCount=TotalItemCount ,
        @TotalProcessingTime=TotalProcessingTime ,
        @AvgProcessingTime=AvgProcessingTime

FROM    ( SELECT    COUNT(*) AS SuccessCount
          FROM      dbo.tblBatchItems (NOLOCK)
          WHERE     processedstatus = ''Success''
                    AND batchid = @batchId
        ) a ,
        ( SELECT    COUNT(*) AS FailedCount
          FROM      dbo.tblBatchItems (NOLOCK)
          WHERE     processedstatus = ''Failed''
                    AND batchid = @batchId
        ) b ,
        ( SELECT    COUNT(*) AS NotProcessedCount
          FROM      dbo.tblBatchItems (NOLOCK)
          WHERE     processedstatus = ''NotProcessed''
                    AND batchid = @batchId
        ) c ,
        ( SELECT    COUNT(*) AS ProcessingCount
          FROM      dbo.tblBatchItems (NOLOCK)
          WHERE     processedstatus = ''Processing''
                    AND batchid = @batchId
        ) d ,
        ( SELECT    COUNT(*) AS TotalItemCount
          FROM      dbo.tblBatchItems (NOLOCK)
          WHERE     batchid = @batchId
        ) e ,
        ( SELECT    SUM(TotalProcessingTime) AS TotalProcessingTime
          FROM      dbo.tblBatchItems (NOLOCK)
          WHERE     batchid = @batchId
                    AND processedstatus IN ( ''Success'', ''Failed'' )
        ) f ,
        ( SELECT    ISNULL(SUM(TotalProcessingTime) / COUNT(*), 0) AS AvgProcessingTime
          FROM      tblBatchItems (NOLOCK)
          WHERE     processedstatus = ''Success''
                    AND batchid = @batchId
        ) g

    DECLARE @WorkSummary NVARCHAR(255)
    DECLARE @LastWorkSummary NVARCHAR(1000)
    DECLARE @BatchStartTime DATETIME

    SELECT @LastWorkSummary = WorkSummary FROM tblBatch WHERE BatchId = @batchId
    SET @BatchStartTime = dbo.usf_get_start_time(@batchId, ''tblBatch'')

    SET @WorkSummary = dbo.usf_create_work_summary(@SuccessCount, @FailedCount,
                                               @NotProcessedCount,
                                               @ProcessingCount,
                                               @TotalItemCount,
                                               @AvgProcessingTime, 
                                               @LastWorkSummary,
                                               @BatchStartTime,
                                               NULL)

    UPDATE tblBatch SET WorkSummary = @WorkSummary WHERE BatchId = @batchId

      DECLARE @ProcessingRateString VARCHAR(10)
    DECLARE @PeakProcessingRateString VARCHAR(10)

    DECLARE @ProcessingRate FLOAT
    DECLARE @PeakProcessingRate FLOAT

    SET @ProcessingRateString = dbo.usf_get_value_from_key_value_pairs(@WorkSummary,''Rate'', '';'')
    SET @PeakProcessingRateString = dbo.usf_get_value_from_key_value_pairs(@WorkSummary,''PeakRate'', '';'')

    SET @ProcessingRate = CONVERT(FLOAT, @ProcessingRateString)
    SET @PeakProcessingRate = CONVERT(FLOAT, @PeakProcessingRateString)

    SELECT  @SuccessCount AS SuccessCount ,
            @FailedCount AS FailedCount ,
            @NotProcessedCount AS NotProcessedCount ,
            @ProcessingCount AS ProcessingCount ,
            @TotalItemCount AS TotalItemCount ,
            @AvgProcessingTime AS AvgProcessingTime ,
            GETDATE() AS LastUpdateTime ,
            @ProcessingRate AS ProcessingRate , 
            @PeakProcessingRate AS PeakProcessingRate

    END
    ' 
  END
GO

    -- We are going to create a stored procedure for working with repositories
    -- First make sure we don't have the old table schema
DECLARE @badColumnCount INT

SELECT  @badColumnCount = COUNT(COLUMN_NAME)
FROM    INFORMATION_SCHEMA.COLUMNS
WHERE   TABLE_NAME = 'tblRepository'
        AND COLUMN_NAME = 'RepositoryId'
    
    -- If we don't find this column name we need to drop both the 
    -- tblRepository and tblJobRepositoryRel tables and recreate them.
IF @badColumnCount = 0
  BEGIN
    IF EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblRepository]')
                        AND type IN ( N'U' ) )
      DROP TABLE [dbo].[tblRepository]


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

      
    IF EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[tblJobRepositoryRel]')
                        AND type IN ( N'U' ) )
      DROP TABLE [dbo].[tblJobRepositoryRel]


    CREATE TABLE [dbo].[tblJobRepositoryRel]
      (
        [JobId] [nvarchar](255) NULL ,
        [RepositoryId] [nvarchar](255) NULL ,
        [JobScope] [nvarchar](12) NULL
      )
    ON
      [PRIMARY]


  END

  GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_repository]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].usp_save_repository
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_repository]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <May 1, 2014>
-- Description:	<Creates or saves a repository>
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_repository]
  @repositoryid NVARCHAR(255) ,
  @repositoryname NVARCHAR(255) ,
  @jobid NVARCHAR(255) ,
  @jobscope NVARCHAR(12) ,
  @connectionstring NVARCHAR(3000) ,
  @repository VARBINARY(MAX) , 
  @replaceexisting BIT
AS
  BEGIN

    -- If we were not passed a repository id then try to look it up based on the job id
    IF NULLIF(@repositoryid, '''') IS NULL
      SET @repositoryid = ( SELECT  RepositoryId
                            FROM    dbo.tblRepository
                            WHERE   ConnectionString = @connectionstring
                          )

    IF NULLIF(@repositoryid, '''') IS NULL
    SET @repositoryid = LOWER(NEWID())

    -- See if the repository id exists
    DECLARE @repositoryIdCount INT

    SELECT  @repositoryIdCount = COUNT(RepositoryId)
    FROM    tblRepository
    WHERE   RepositoryId = @repositoryid
    
    -- If we don''t find the repository id we add it, otherwise we update it 

    IF @repositoryIdCount = 0
      BEGIN
            -- This repository does not yet exist in the database, we will create it.
        SET @repositoryid = LOWER(NEWID())
        INSERT  INTO dbo.tblRepository
                ( RepositoryId ,
                  RepositoryName ,
                  ConnectionString ,
                  Repository ,
                  LastUpdateDate
                )
        VALUES  ( @repositoryid ,
                  @repositoryname ,
                  @connectionstring ,
                  @repository ,
                  GETDATE()
                )   
                        
        INSERT  INTO dbo.tblJobRepositoryRel
                ( JobId, RepositoryId, JobScope )
        VALUES  ( @jobid, @repositoryid, @jobscope )                              

      END
    ELSE
      IF @replaceexisting <> 0
        BEGIN
          UPDATE  dbo.tblRepository
          SET     RepositoryName = @repositoryname ,
                  ConnectionString = @connectionstring ,
                  Repository = @repository ,
                  LastUpdateDate = GETDATE()
          WHERE   RepositoryId = @repositoryid
        END

      SELECT  @repositoryIdCount = COUNT(RepositoryId)
      FROM    tblJobRepositoryRel
      WHERE   RepositoryId = @repositoryid AND JobId = @jobid

      IF @repositoryIdCount = 0
              INSERT  INTO dbo.tblJobRepositoryRel
                ( JobId, RepositoryId, JobScope )
        VALUES  ( @jobid, @repositoryid, @jobscope )  


  END
'
  END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_update_batch_item]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_update_batch_item]
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_update_batch_item]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <June 18, 2014>
-- Description:	<Updates a batch item>
-- =============================================
CREATE PROCEDURE [dbo].[usp_update_batch_item]
  @itemid NVARCHAR(255) ,
  @destinationdocid NVARCHAR(255) ,
  @processedstatus NVARCHAR(255) ,
  @processresult NTEXT ,
  @processedmessage NVARCHAR(255) ,
  @processstarttime DATETIME ,
  @processfinishtime DATETIME ,
  @totalprocessingtime FLOAT ,
  @processedby NVARCHAR(255)
AS
  BEGIN

  -- Make sure the item exists
    DECLARE @ItemCount INT

    SET @ItemCount = ( SELECT COUNT(*)
                       FROM   dbo.tblBatchItems
                       WHERE  ID = @itemid
                     )
    IF @ItemCount = 0
      RETURN -100

    ELSE
      BEGIN
        IF @processedstatus = ''Processing''
          BEGIN
            UPDATE  dbo.tblBatchItems
            SET     ProcessedStatus = @processedstatus ,
                    ProcessStartTime = @processstarttime ,
                    ProcessedBy = @processedby
            WHERE   ID = @itemid
          END
        ELSE
          BEGIN         
            IF @processresult IS NULL
              BEGIN
                UPDATE  dbo.tblBatchItems
                SET     ProcessedStatus = @processedstatus ,
                        DestDocId = @destinationdocid ,
                        ProcessedMessage = @processedmessage ,
                        ProcessStartTime = @processstarttime ,
                        ProcessFinishTime = @processfinishtime ,
                        TotalProcessingTime = @totalprocessingtime ,
                        ProcessedBy = @processedby
                WHERE   ID = @itemid
              END
            ELSE
              BEGIN
                UPDATE  dbo.tblBatchItems
                SET     ProcessedStatus = @processedstatus ,
                        DestDocId = @destinationdocid ,
                        ProcessedMessage = @processedmessage ,
                        ProcessResult = @processresult ,
                        ProcessStartTime = @processstarttime ,
                        ProcessFinishTime = @processfinishtime ,
                        TotalProcessingTime = @totalprocessingtime, 
                        ProcessedBy = @processedby
                WHERE   ID = @itemid
              END
          END
      END

  END
' 
  END

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usf_parse_string]')
                    AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  DROP FUNCTION [dbo].[usf_parse_string]
GO

/****** Object:  UserDefinedFunction [dbo].[usf_parse_string]    Script Date: 6/28/2014 12:55:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usf_parse_string]')
                        AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  BEGIN
    EXECUTE dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <06/27/2014>
-- Description:	<Parses a delimited string>
-- =============================================
CREATE FUNCTION [dbo].[usf_parse_string]
  (
    @incomingString VARCHAR(1000) ,
    @delimiter CHAR(1) ,
    @index INT
  )
RETURNS VARCHAR(1000)
AS
    BEGIN
      DECLARE @currentIndex INT = 0 ,
        @position INT = 1 ,
        @prevPosition INT = 0 ,
        @result VARCHAR(1000)

      WHILE @position > 0
        BEGIN

          SET @position = CHARINDEX(@delimiter, @incomingString, @prevPosition);

          IF ( @position > 0 )
            BEGIN-- get the chars between the prev position to next delimiter pos
              SET @result = SUBSTRING(@incomingString, @prevPosition,
                                      @position - @prevPosition)
            END
          ELSE
            BEGIN--get last delim message
              SET @result = SUBSTRING(@incomingString, @prevPosition,
                                      LEN(@incomingString))
            END

          IF ( @index = @currentIndex )
            BEGIN
              RETURN @result
            END

          SET @prevPosition = @position + 1
          SET @currentIndex = @currentIndex + 1;

        END
      RETURN NULL
    END' 
  END

GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usf_get_value_from_key_value_pairs]')
                    AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  DROP FUNCTION [dbo].[usf_get_value_from_key_value_pairs]
GO

/****** Object:  UserDefinedFunction [dbo].[usf_get_value_from_key_value_pairs]    Script Date: 6/28/2014 12:56:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usf_get_value_from_key_value_pairs]')
                        AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) )
  BEGIN
    EXECUTE dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <06/27/2014>
-- Description:	<Gets a value from a string of key/value pairs>
-- =============================================
CREATE FUNCTION [dbo].[usf_get_value_from_key_value_pairs]
  (
    @incomingString VARCHAR(1000) ,
    @key VARCHAR(100) ,
    @delimiter CHAR(1)
  )

RETURNS VARCHAR(1000)
AS
    BEGIN
      DECLARE @currentIndex INT = 0 ,
        @position INT = 1 ,
        @prevPosition INT = 0 ,
        @iteration INT = 0 ,
        @keyValuePair VARCHAR(1000) ,
        @result VARCHAR(1000)

      WHILE @result IS NULL
        BEGIN

        SET @keyValuePair = dbo.usf_parse_string(@incomingString, @delimiter, @iteration)
        
        IF @keyValuePair IS NOT NULL
          BEGIN
            IF CHARINDEX(@key, @keyValuePair) > 0
              BEGIN
                SET @result = dbo.usf_parse_string(@keyValuePair,''='', 1)
                RETURN @result
              END
            ELSE
              SET @iteration = @iteration + 1
            END
          ELSE
            RETURN NULL
          END

      RETURN NULL
    END' 
  END

GO

/****** Object:  StoredProcedure [dbo].[usp_reset_batch_items]    Script Date: 8/12/2015 8:13:01 AM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_reset_batch_items]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_reset_batch_items]
GO

/****** Object:  StoredProcedure [dbo].[usp_reset_batch_items]    Script Date: 8/12/2015 8:13:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_reset_batch_items]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 11, 2013>
-- Description:	<Resets all job items for the specified job in the specified status to not processed>
-- =============================================
CREATE PROCEDURE [dbo].[usp_reset_batch_items] @batchId NVARCHAR(255) , @currentprocessingstatus NVARCHAR(25)
AS
  BEGIN

    UPDATE    bi 
              SET     bi.ProcessedStatus = ''NotProcessed''
              FROM tblBatchItems bi ( NOLOCK )
              WHERE   bi.BatchId = @batchId
              AND bi.ProcessedStatus = @currentprocessingstatus

  END    ' 
  END
GO

/****** Object:  StoredProcedure [dbo].[usp_reset_job_items]    Script Date: 6/29/2014 8:14:05 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_reset_job_items]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_reset_job_items]
GO

/****** Object:  StoredProcedure [dbo].[usp_reset_job_items]    Script Date: 6/29/2014 8:14:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_reset_job_items]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 11, 2013>
-- Description:	<Resets all job items for the specified job in the specified status to not processed>
-- =============================================
CREATE PROCEDURE [dbo].[usp_reset_job_items] @jobId NVARCHAR(255) , @currentprocessingstatus NVARCHAR(25)
AS
  BEGIN

    UPDATE    bi 
              SET     bi.ProcessedStatus = ''NotProcessed''
              FROM tblBatchItems bi ( NOLOCK )
                      JOIN tblJobBatchRel jbr ON jbr.BatchId = bi.BatchId             
              WHERE   jbr.JobId = @jobId
                      AND bi.ProcessedStatus = @currentprocessingstatus

  END    ' 
  END
GO


IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vwDbFileInfo', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwDbFileInfo'

GO

IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vwDbFileInfo', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwDbFileInfo'

GO

/****** Object:  View [dbo].[vwDbFileInfo]    Script Date: 12/1/2016 3:43:52 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDbFileInfo]'))
DROP VIEW [dbo].[vwDbFileInfo]
GO

/****** Object:  View [dbo].[vwDbFileInfo]    Script Date: 12/1/2016 3:43:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DECLARE @SQL_VERSION AS SQL_VARIANT
SET @SQL_VERSION = CONVERT(VARCHAR, SERVERPROPERTY('productversion'))

IF @SQL_VERSION > '10.50.1600.1'

BEGIN


IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDbFileInfo]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vwDbFileInfo]
AS
SELECT        DB_NAME(mf.database_id) DBName, mf.name LogicalDBName, CONVERT(DECIMAL(20, 2), (CONVERT(DECIMAL, mf.size) / 128)) [FileSizeInMB], 
                         CASE mf.is_percent_growth WHEN 1 THEN ''Yes'' ELSE ''No'' END AS [IsPercentGrowth], CASE mf.is_percent_growth WHEN 1 THEN CONVERT(VARCHAR, 
                         mf.growth) + ''%'' WHEN 0 THEN CONVERT(VARCHAR, mf.growth / 128) + '' MB'' END AS [GrowthInIncrementOf], 
                         CASE mf.is_percent_growth WHEN 1 THEN CONVERT(DECIMAL(20, 2), (((CONVERT(DECIMAL, mf.size) * mf.growth) / 100) * 8) / 1024) 
                         WHEN 0 THEN CONVERT(DECIMAL(20, 2), (CONVERT(DECIMAL, mf.growth) / 128)) END AS [NextAutoGrowthSizeInMB], 
                         CASE mf.max_size WHEN 0 THEN ''No growth is allowed'' WHEN - 1 THEN ''File will grow until the disk is full'' ELSE CONVERT(VARCHAR, 
                         CONVERT(DECIMAL(20, 2), (CONVERT(DECIMAL, mf.max_size) / 128))) END AS [MaxSize], mf.physical_name As FileName, fs.LogicalDriveName, fs.Drive, 
                         fs.FreeSpaceInMB
FROM            sys.master_files mf INNER JOIN
                         sys.database_files df ON mf.name = df.name INNER JOIN
                             (SELECT DISTINCT 
                                                         mf.name, dovs.logical_volume_name AS LogicalDriveName, dovs.volume_mount_point AS Drive, CONVERT(INT, 
                                                         dovs.available_bytes / 1048576.0) AS FreeSpaceInMB
                               FROM            sys.master_files mf INNER JOIN
                                                         sys.database_files df ON mf.physical_name = df.physical_name CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs) 
                         fs ON mf.name = fs.name;
' 

END

ELSE

BEGIN

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDbFileInfo]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vwDbFileInfo]
AS
SELECT        DB_NAME(mf.database_id) DBName, mf.name LogicalDBName, CONVERT(DECIMAL(20, 2), (CONVERT(DECIMAL, mf.size) / 128)) [FileSizeInMB], 
                         CASE mf.is_percent_growth WHEN 1 THEN ''Yes'' ELSE ''No'' END AS [IsPercentGrowth], CASE mf.is_percent_growth WHEN 1 THEN CONVERT(VARCHAR, 
                         mf.growth) + ''%'' WHEN 0 THEN CONVERT(VARCHAR, mf.growth / 128) + '' MB'' END AS [GrowthInIncrementOf], 
                         CASE mf.is_percent_growth WHEN 1 THEN CONVERT(DECIMAL(20, 2), (((CONVERT(DECIMAL, mf.size) * mf.growth) / 100) * 8) / 1024) 
                         WHEN 0 THEN CONVERT(DECIMAL(20, 2), (CONVERT(DECIMAL, mf.growth) / 128)) END AS [NextAutoGrowthSizeInMB], 
                         CASE mf.max_size WHEN 0 THEN ''No growth is allowed'' WHEN - 1 THEN ''File will grow until the disk is full'' ELSE CONVERT(VARCHAR, 
                         CONVERT(DECIMAL(20, 2), (CONVERT(DECIMAL, mf.max_size) / 128))) END AS [MaxSize], mf.physical_name As FileName, fs.LogicalDriveName, fs.Drive, 
                         fs.FreeSpaceInMB
FROM            sys.master_files mf INNER JOIN
                         sys.database_files df ON mf.name = df.name INNER JOIN
                             (SELECT DISTINCT 
                                                         mf.name, 
                                                         ''UNAVAILABLE WITH SQL SERVER VERSION '' + (CONVERT(VARCHAR, (SELECT SERVERPROPERTY(''productversion'')))) LogicalDriveName, 
                                                         ''UNAVAILABLE WITH SQL SERVER VERSION '' + (CONVERT(VARCHAR, (SELECT SERVERPROPERTY(''productversion'')))) Drive, 
                                                         ''UNAVAILABLE WITH SQL SERVER VERSION '' + (CONVERT(VARCHAR, (SELECT SERVERPROPERTY(''productversion'')))) AS FreeSpaceInMB
                               FROM            sys.master_files mf INNER JOIN
                                                         sys.database_files df ON mf.physical_name = df.physical_name) 
                         fs ON mf.name = fs.name;
' 

END

IF EXISTS ( SELECT  *
            FROM    ::
                    FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPaneCount', N'SCHEMA',
                                            N'dbo', N'VIEW',
                                            N'vwOrphanBatches', NULL, NULL) )
  EXEC sys.sp_dropextendedproperty @name = N'MS_DiagramPaneCount',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatches'

GO

IF EXISTS ( SELECT  *
            FROM    ::
                    FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPane1', N'SCHEMA',
                                            N'dbo', N'VIEW',
                                            N'vwOrphanBatches', NULL, NULL) )
  EXEC sys.sp_dropextendedproperty @name = N'MS_DiagramPane1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatches'

GO

/****** Object:  View [dbo].[vwOrphanBatches]    Script Date: 1/7/2015 4:12:53 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.views
            WHERE   object_id = OBJECT_ID(N'[dbo].[vwOrphanBatches]') )
  DROP VIEW [dbo].[vwOrphanBatches]
GO

/****** Object:  View [dbo].[vwOrphanBatches]    Script Date: 1/7/2015 4:12:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.views
                WHERE   object_id = OBJECT_ID(N'[dbo].[vwOrphanBatches]') )
  EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vwOrphanBatches]
AS
SELECT DISTINCT BI.BatchId
FROM            dbo.tblJobBatchRel AS JBR INNER JOIN
                         dbo.tblJob AS J ON JBR.JobId = J.JobId RIGHT OUTER JOIN
                         dbo.tblBatch AS B ON JBR.BatchId = B.BatchId RIGHT OUTER JOIN
                         dbo.tblBatchItems AS BI ON B.BatchId = BI.BatchId
WHERE        (J.JobName IS NULL)
' 
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPane1', N'SCHEMA',
                                                N'dbo', N'VIEW',
                                                N'vwOrphanBatches', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DiagramPane1',
    @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "JBR"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 103
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "J"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 137
               Right = 417
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "B"
            Begin Extent = 
               Top = 6
               Left = 455
               Bottom = 137
               Right = 625
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BI"
            Begin Extent = 
               Top = 6
               Left = 663
               Bottom = 137
               Right = 863
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatches'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPaneCount',
                                                N'SCHEMA', N'dbo', N'VIEW',
                                                N'vwOrphanBatches', NULL, NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1,
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatches'
GO

IF EXISTS ( SELECT  *
            FROM    ::
                    FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPaneCount', N'SCHEMA',
                                            N'dbo', N'VIEW',
                                            N'vwOrphanBatchItems', NULL, NULL) )
  EXEC sys.sp_dropextendedproperty @name = N'MS_DiagramPaneCount',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatchItems'

GO

IF EXISTS ( SELECT  *
            FROM    ::
                    FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPane1', N'SCHEMA',
                                            N'dbo', N'VIEW',
                                            N'vwOrphanBatchItems', NULL, NULL) )
  EXEC sys.sp_dropextendedproperty @name = N'MS_DiagramPane1',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatchItems'

GO

/****** Object:  View [dbo].[vwOrphanBatchItems]    Script Date: 1/7/2015 4:13:51 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.views
            WHERE   object_id = OBJECT_ID(N'[dbo].[vwOrphanBatchItems]') )
  DROP VIEW [dbo].[vwOrphanBatchItems]
GO

/****** Object:  View [dbo].[vwOrphanBatchItems]    Script Date: 1/7/2015 4:13:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.views
                WHERE   object_id = OBJECT_ID(N'[dbo].[vwOrphanBatchItems]') )
  EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vwOrphanBatchItems]
AS
SELECT        BI.*
FROM            dbo.tblBatchItems AS BI INNER JOIN
                         dbo.vwOrphanBatches AS OB ON BI.BatchId = OB.BatchId
' 
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPane1', N'SCHEMA',
                                                N'dbo', N'VIEW',
                                                N'vwOrphanBatchItems', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DiagramPane1',
    @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "BI"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 322
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OB"
            Begin Extent = 
               Top = 6
               Left = 276
               Bottom = 86
               Right = 446
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatchItems'
GO

IF NOT EXISTS ( SELECT  *
                FROM    ::
                        FN_LISTEXTENDEDPROPERTY(N'MS_DiagramPaneCount',
                                                N'SCHEMA', N'dbo', N'VIEW',
                                                N'vwOrphanBatchItems', NULL,
                                                NULL) )
  EXEC sys.sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1,
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW',
    @level1name = N'vwOrphanBatchItems'
GO


/****** Object:  StoredProcedure [dbo].[usp_delete_orphan_batches]    Script Date: 1/7/2015 4:15:49 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_orphan_batches]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_delete_orphan_batches]
GO

/****** Object:  StoredProcedure [dbo].[usp_delete_orphan_batches]    Script Date: 1/7/2015 4:15:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_orphan_batches]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <January 07, 2015>
-- Description:	<Deletes all batches and associated 
--   batch items no longer associated with a job.>
-- =============================================
CREATE PROCEDURE [dbo].[usp_delete_orphan_batches]
AS
  BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
    SET NOCOUNT ON;

-- Find all batches that are not associated with a job
    DECLARE @BatchId NVARCHAR(255)
    DECLARE batch_cursor CURSOR FAST_FORWARD
    FOR
      SELECT * FROM dbo.vwOrphanBatches

    OPEN batch_cursor

    FETCH NEXT FROM batch_cursor INTO @BatchId

    WHILE @@FETCH_STATUS = 0
      BEGIN

-- Delete the batch
        EXEC dbo.usp_delete_batch @BatchId

        FETCH NEXT FROM batch_cursor INTO @BatchId
      END

    CLOSE batch_cursor
    DEALLOCATE batch_cursor

  END
' 
  END
GO
