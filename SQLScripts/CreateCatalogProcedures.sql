/****** Object:  StoredProcedure [dbo].[usp_save_area]    Script Date: 12/14/2013 12:03:50 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_area]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_area]
GO

/****** Object:  StoredProcedure [dbo].[usp_save_area]    Script Date: 12/14/2013 12:03:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_area]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[usp_save_area]
  @id NVARCHAR(255) ,
  @name NVARCHAR(255) ,
  @description NTEXT
AS
  BEGIN
    

    IF NULLIF(@id, '''''''') IS NULL
      RETURN -100

    DECLARE @areaCount INT
    SET @areaCount = ( SELECT COUNT(tblAreas.AreaId)
                       FROM   dbo.tblAreas
                       WHERE  tblAreas.AreaId = @id
                     )

    IF @areaCount = 0
      BEGIN
        INSERT  INTO dbo.tblAreas
                ( AreaId, AreaName, AreaDescription )
        VALUES  ( @id, @name, @description )
      END
    ELSE
      BEGIN
        UPDATE  dbo.tblAreas
        SET     tblAreas.areaname = @name ,
                tblAreas.AreaDescription = @description
        WHERE   tblAreas.AreaId = @id
      END

  END' 
  END
GO

/****** Object:  StoredProcedure [dbo].[usp_save_catalog_configuration]    Script Date: 8/14/2015 1:34:07 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_catalog_configuration]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_catalog_configuration]
GO

/****** Object:  StoredProcedure [dbo].[usp_save_catalog_configuration]    Script Date: 8/14/2015 1:34:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_catalog_configuration]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <August 14, 2015>
-- Description:	<Updates a the catalog configuration>
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_catalog_configuration]
  @catalogId NVARCHAR(255) ,
  @catalogconfiguration NTEXT
AS
  BEGIN

    --  If we were passed a null catalog id throw an error
    IF NULLIF(@catalogId, '''''''') IS NULL
      RETURN -100


            -- See if the project is already listed in the catalog
    DECLARE @CatalogCount TINYINT
    SET @CatalogCount = ( SELECT  COUNT(*)
                          FROM    dbo.tblCatalog
                          WHERE   tblCatalog.CatalogId = @catalogId
                        )

    IF @CatalogCount = 0
      RETURN -200
    ELSE
      BEGIN
        UPDATE  dbo.tblCatalog
        SET     CatalogConfiguration = @catalogconfiguration
        WHERE   CatalogId = @catalogId
      END
  END
' 
  END
GO

/****** Object:  StoredProcedure [dbo].[usp_save_project]    Script Date: 12/14/2013 12:04:08 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_project]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_project]
GO

/****** Object:  StoredProcedure [dbo].[usp_save_project]    Script Date: 12/14/2013 12:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_project]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 12, 2013>
-- Description:	<Adds or updates a project entry>
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_project]
  @projectId NVARCHAR(255) ,
  @projectName NVARCHAR(255) ,
  @areaId NVARCHAR(255) ,
  @location NTEXT ,
  @createDate DATETIME ,
  @ItemsProcessed BIGINT
AS
  BEGIN

    --  If we were passed a null project id throw an error
    IF NULLIF(@projectId, '''''''') IS NULL
      RETURN -100


            -- See if the project is already listed in the catalog
    DECLARE @ProjectCount TINYINT
    SET @ProjectCount = ( SELECT  COUNT(*)
                          FROM    dbo.tblProjects
                          WHERE   tblProjects.ProjectId = @projectId
                        )

    IF @ProjectCount = 0
      BEGIN
            -- We will treat this as a new entry in the catalog
        INSERT  INTO dbo.tblProjects
                ( ProjectId ,
                  ProjectName ,
                  AreaId ,
                  Location ,
                  CreateDate ,
                  ItemsProcessed
                )
        VALUES  ( @projectId ,
                  @projectName ,
                  @areaId ,
                  @location ,
                  @createDate ,
                  @ItemsProcessed
                )
      END
    ELSE
      BEGIN
            -- We will treat this as an existing entry in the catalog
        UPDATE  dbo.tblProjects
        SET     ProjectName = @projectName ,
                AreaId = @areaId ,
                Location = @location ,
                CreateDate = @createDate ,
                ItemsProcessed = @ItemsProcessed
        WHERE   ProjectId = @projectId
      END
  END
' 
  END
GO


/****** Object:  StoredProcedure [dbo].[usp_delete_area]    Script Date: 12/16/2013 3:39:22 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_area]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_delete_area]
GO

/****** Object:  StoredProcedure [dbo].[usp_delete_area]    Script Date: 12/16/2013 3:39:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_area]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <December 16, 2013>
-- Description:	<Deletes an area by its id>
-- =============================================
CREATE PROCEDURE [dbo].[usp_delete_area] @areaId NVARCHAR(255)
AS
  BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Make sure the job exists
    DECLARE @AreaCount INT

    SET @AreaCount = ( SELECT COUNT(*)
                       FROM   dbo.tblAreas
                       WHERE  AreaId = @areaId
                     )
    IF @AreaCount = 0
      RETURN -100

    -- Make sure there are currently no projects associated with this area
    DECLARE @ProjectCount INT

    SET @ProjectCount = ( SELECT  COUNT(*)
                          FROM    dbo.tblProjects
                          WHERE   AreaId = @areaId
                        )
    IF @ProjectCount > 0
      RETURN -200

     -- Delete the area
    DELETE  FROM dbo.tblAreas
    WHERE   AreaId = @areaId
     
  END
' 
  END
GO


/****** Object:  StoredProcedure [dbo].[usp_save_node]    Script Date: 1/9/2014 2:38:15 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_node]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_save_node]
GO

/****** Object:  StoredProcedure [dbo].[usp_save_node]    Script Date: 1/9/2014 2:38:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_save_node]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <January 9, 2014>
-- Description:	<Adds or updates a node entry>
-- =============================================
CREATE PROCEDURE [dbo].[usp_save_node]
  @nodeid NVARCHAR(255) ,
  @nodeName NVARCHAR(255) ,
  @nodeDescription NVARCHAR(255) ,
  @nodeAddress NVARCHAR(255) ,
  @nodeRole NVARCHAR(20) ,
  @nodeStatus NVARCHAR(20) ,
  @version NVARCHAR(20) ,
  @createDate DATETIME ,
  @computerInfo NVARCHAR(MAX)

AS
  BEGIN

    --  If we were passed a null node id throw an error
    IF NULLIF(@nodeid, '''''''') IS NULL
      RETURN -100


    -- See if the node is already listed in the catalog
    DECLARE @NodeCount TINYINT
    SET @NodeCount = ( SELECT  COUNT(*)
                          FROM    dbo.tblNodes
                          WHERE   tblNodes.NodeId = @nodeid
                        )

    IF @NodeCount = 0
      BEGIN
            -- We will treat this as a new entry in the catalog
        INSERT  INTO dbo.tblNodes
                ( NodeId ,
                  NodeName ,
                  NodeDescription ,
                  NodeAddress ,
                  Role ,
                  Status ,
                  Version ,
                  CreateDate ,
                  ComputerInfo
                )
        VALUES  ( @nodeid ,
                  @nodeName ,
                  @nodeDescription ,
                  @nodeAddress ,
                  @nodeRole ,
                  @nodeStatus ,
                  @version ,
                  @createDate ,
                  @computerInfo
                )
      END
    ELSE
      BEGIN
            -- We will treat this as an existing entry in the catalog
        UPDATE  dbo.tblNodes
        SET     nodeName = @nodeName ,
                NodeDescription = @nodeDescription ,
                NodeAddress = @nodeAddress ,
                Role = @nodeRole ,
                Status = @nodeStatus ,
                Version = @version ,
                CreateDate = @createDate ,
                ComputerInfo = @computerInfo
        WHERE   NodeId = @nodeid
      END
  END
' 
  END
GO


/****** Object:  StoredProcedure [dbo].[usp_delete_node]    Script Date: 1/9/2014 2:49:30 PM ******/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_node]')
                    AND type IN ( N'P', N'PC' ) )
  DROP PROCEDURE [dbo].[usp_delete_node]
GO

/****** Object:  StoredProcedure [dbo].[usp_delete_node]    Script Date: 1/9/2014 2:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  *
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'[dbo].[usp_delete_node]')
                        AND type IN ( N'P', N'PC' ) )
  BEGIN
    EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <January 9, 2014>
-- Description:	<Deletes a node by its id>
-- =============================================
CREATE PROCEDURE [dbo].[usp_delete_node] @nodeId NVARCHAR(255)
AS
  BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Make sure the node exists
    DECLARE @NodeCount INT

    SET @NodeCount = ( SELECT COUNT(*)
                       FROM   dbo.tblNodes
                       WHERE  NodeId = @nodeId
                     )
    IF @NodeCount = 0
      RETURN -100

     -- Delete the node
    DELETE  FROM dbo.tblNodes
    WHERE   NodeId = @nodeId
     
  END
' 
  END
GO

/****** Object:  StoredProcedure [dbo].[usp_update_node_counter]    Script Date: 6/22/2016 3:56:08 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_update_node_counter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_update_node_counter]
GO

/****** Object:  StoredProcedure [dbo].[usp_update_node_counter]    Script Date: 6/22/2016 3:56:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_update_node_counter]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <June 22, 2016>
-- Description:	<Updates the node counter>
-- =============================================
CREATE PROCEDURE [dbo].[usp_update_node_counter]
  @nodeAddress NVARCHAR(255) ,
  @additionalItemCount BIGINT

AS
  BEGIN

    --  If we were passed a null node address throw an error
    IF NULLIF(@nodeAddress, '''''''') IS NULL
      RETURN -100


    -- Make sure the node exists
    DECLARE @NodeCount TINYINT
    SET @NodeCount = ( SELECT  COUNT(*)
                          FROM    dbo.tblNodes
                          WHERE   tblNodes.NodeAddress = @nodeAddress
                        )

    IF @NodeCount = 0
      BEGIN
        RETURN -200
      END
    ELSE
      BEGIN
            -- We will add the additional item count to the items processed column
    DECLARE @ItemsProcessed BIGINT
    SET @ItemsProcessed = ( SELECT  ItemsProcessed
                          FROM    dbo.tblNodes
                          WHERE   tblNodes.NodeAddress = @nodeAddress
                        )
		IF @ItemsProcessed IS NULL
			BEGIN
				SET @ItemsProcessed = 0
			END
        UPDATE  dbo.tblNodes
        SET     ItemsProcessed = @ItemsProcessed + @additionalItemCount
        WHERE   NodeAddress = @nodeAddress
      END
  END
' 
END
GO


/****** Object:  Trigger [trg_tblNodes_AfterUpdate]    Script Date: 6/21/2016 2:29:44 PM ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trg_tblNodes_AfterUpdate]'))
DROP TRIGGER [dbo].[trg_tblNodes_AfterUpdate]
GO

/****** Object:  Trigger [dbo].[trg_tblNodes_AfterUpdate]    Script Date: 6/21/2016 2:29:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trg_tblNodes_AfterUpdate]'))
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Ernie Bahr>
-- Create date: <June 21, 2016>
-- Description:	<Sets the ItemsProcessed count for the catalog to the sum of the node counts>
-- =============================================
CREATE TRIGGER [dbo].[trg_tblNodes_AfterUpdate] ON [dbo].[tblNodes]
  FOR UPDATE
AS
  DECLARE @totalItemsProcessed BIGINT;

	
  IF UPDATE(ItemsProcessed)
    BEGIN
      SELECT  @totalItemsProcessed = SUM(ItemsProcessed)
      FROM    tblNodes;
      UPDATE  tblCatalog
      SET     ItemsProcessed = @totalItemsProcessed;
    END
  PRINT ''AFTER UPDATE Trigger fired.''
' 
GO
