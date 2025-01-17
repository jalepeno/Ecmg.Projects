'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"


Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.SerializationUtilities
Imports Documents.Transformations
Imports Documents.Utilities
Imports Microsoft.Data.SqlClient
Imports Operations
Imports Operations.OperationEnumerations
Imports Projects.Configuration

#End Region

Public Class SQLContainer
  Inherits Container

#Region "Class Constants"

  Private Const TABLE_BATCH_ITEM_NAME As String = "tblBatchItems"
  Private Const TABLE_BATCH_NAME As String = "tblBatch"
  Private Const TABLE_JOB_NAME As String = "tblJob"
  Private Const TABLE_JOB_RELATIONSHIPS_NAME As String = "tblJobRelationships"
  Private Const TABLE_RELATED_JOBS_NAME As String = "tblRelatedJobs"
  Private Const TABLE_FILES_NAME As String = "tblFiles"
  Private Const TABLE_PROJECTJOBREL_NAME As String = "tblProjectJobRel"
  Private Const TABLE_JOBBATCHREL_NAME As String = "tblJobBatchRel"
  Private Const TABLE_PROJECT_NAME As String = "tblProject"
  Private Const TABLE_BATCHLOCK_NAME As String = "tblBatchLock"
  Private Const TABLE_AUDIT_NAME As String = "tblAudit"

  Private Const TABLE_CATALOG_NAME As String = "tblCatalog"
  Private Const TABLE_PROJECTS_NAME As String = "tblProjects"
  Private Const TABLE_NODES_NAME As String = "tblNodes"

  Private Const VIEW_DB_FILE_INFO As String = "vwDbFileInfo"
  Private Const VIEW_ORPHAN_BATCHES_NAME As String = "vwOrphanBatches"
  Private Const VIEW_ORPHAN_BATCH_ITEMS_NAME As String = "vwOrphanBatchItems"

  Private Const DATABASE_SCHEMA_VERSION As String = "1.0"
  Private Const CATALOG_DATABASE_SCRIPT_FILENAME As String = "CreateCatalogDatabase.sql"
  Private Const CATALOG_CREATE_TABLES_SCRIPT_FILENAME As String = "CreateCatalogTables.sql"
  Private Const CATALOG_CREATE_STORED_PROCEDURES_SCRIPT_FILENAME As String = "CreateCatalogProcedures.sql"
  Private Const PROJECT_DATABASE_SCRIPT_FILENAME As String = "ContentManagerCreateDatabase.sql"

  Private _
    Const PROJECT_DATABASE_WITH_FILESTREAM_SCRIPT_FILENAME As String = "ContentManagerCreateDatabaseWithFileStream.sql"

  Private Const STORED_PROCEDURE_SCRIPT_FILENAME As String = "ContentManagerCreateStoredProcedures.sql"
  Private Const TABLE_SCRIPT_FILENAME As String = "ContentManagerCreateTables.sql"
  Private Const DELTA_TABLE_SCRIPT_FILENAME As String = "ContentManagerCreateDeltaTable.sql"
  Private Const DELTA_TABLE_WITH_OPERATION_SCRIPT_FILENAME As String = "ContentManagerCreateDeltaTableWithOperation.sql"
  Private Const PERMISSIONS_SCRIPT_FILENAME As String = "GetDbPermissions.sql"

  'Private Const COMMAND_TIMEOUT As Integer = 240
  ''Private Const COMMAND_TIMEOUT As Integer = 30
  Private Const COMMAND_TIMEOUT As Integer = 2700

  Private Const INVALID_OBJECT_NAME As String = "Invalid object name"

  'Private Const MASTERDB_CONNECTION_STRING As String = "Provider=SQLOLEDB;Data Source=<SERVERNAME>;Initial Catalog=master;Trusted_Connection=<TC>;"
  'Private Const DB_CONNECTION_STRING As String = "Provider=SQLOLEDB;Data Source=<SERVERNAME>;Initial Catalog=<DBNAME>;Trusted_Connection=<TC>;"

#End Region

#Region "Class Enumerations"

  Private Enum RepositoryKeyEnum
    Id = 0
    Name = 1
    ConnectionString = 2
  End Enum

  Private Enum ItemScope
    Project = 0
    Job = 1
    Batch = 2
  End Enum

  Private Enum SqlServerVersion
    Unknown = 0
    SQLServer2000 = 8
    SQLServer2005 = 9
    SQLServer2008 = 10
    SQLServer2012 = 11
    SQLServer2014 = 12
    SQLServer2016 = 13
    SQLServervNext = 14
  End Enum

  Private Enum FileStreamAccessLevel
    NotSet = -1
    Disabled = 0
    TSqlAccess = 1
    TSqlAndWin32Access = 2
    TSqlAndWin32AccessWithRemoteStreaming = 3
  End Enum

  Private Enum DatabaseType
    Project = 0
    Catalog = -1
  End Enum

#End Region

#Region "Class Variables"

  Private ReadOnly mobjConnection As SqlConnection = Nothing
  Private mstrMasterDBConnectionString As String = String.Empty 'MASTERDB_CONNECTION_STRING
  Private mstrServerConnectionString As String = String.Empty
  'Private mstrCatalogConnectionString As String = String.Empty

  Private mstrSQLDataDir As String = "c:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA"
  Private mobjDataTable As DataTable
  Private mstrScriptPath As String = String.Empty
  Private menuSqlVersion As SqlServerVersion = SqlServerVersion.Unknown
  Private menuFileStreamAccessLevel As FileStreamAccessLevel = FileStreamAccessLevel.NotSet

  Private mobjChangeListener As DatabaseChangeListener

#End Region

#Region "Constructors"

  Public Sub New()
    Try
      'InitializeLogSession()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' It is the containers responsibility to build the connection string(location)
  ''' </summary>
  ''' <param name="lpItemsLocation"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal lpItemsLocation As ItemsLocation)

    Try
      'InitializeLogSession()
      Me.ItemsLocation = lpItemsLocation

      'Build Location (Connection String)

      Dim lobjBuilder As New Microsoft.Data.SqlClient.SqlConnectionStringBuilder
      lobjBuilder("Data Source") = Me.ItemsLocation.ServerName
      lobjBuilder("Integrated Security") = Me.ItemsLocation.TrustedConnection
      lobjBuilder("Initial Catalog") = Me.ItemsLocation.DatabaseName

      If (ItemsLocation.TrustedConnection.Equals("no", StringComparison.CurrentCultureIgnoreCase) Or ItemsLocation.TrustedConnection.Equals("false", StringComparison.CurrentCultureIgnoreCase)) _
        Then
        lobjBuilder("User Id") = Me.ItemsLocation.UserName
        lobjBuilder("Password") = Me.ItemsLocation.Password
      Else
        lobjBuilder("TrustServerCertificate") = True
      End If

      Me.ItemsLocation.Location = lobjBuilder.ConnectionString

      'menuSqlVersion = GetSqlServerVersion()
      'menuFileStreamAccessLevel = GetFileStreamLevel()

      'If Not String.IsNullOrEmpty(lobjBuilder.InitialCatalog) Then
      '  CreateFileTable()
      'End If

      CreateDataTable()

      'InitializeJobMonitor()

      'Me.ItemsLocation.Location = DB_CONNECTION_STRING
      'Me.ItemsLocation.Location = Me.ItemsLocation.Location.Replace("<SERVERNAME>", Me.ItemsLocation.ServerName)
      'Me.ItemsLocation.Location = Me.ItemsLocation.Location.Replace("<DBNAME>", Me.ItemsLocation.DatabaseName)
      'If (Me.ItemsLocation.TrustedConnection.ToLower = "true") Then
      '  Me.ItemsLocation.TrustedConnection = "yes"
      'End If
      'Me.ItemsLocation.Location = Me.ItemsLocation.Location.Replace("<TC>", Me.ItemsLocation.TrustedConnection)
      'If (Me.ItemsLocation.TrustedConnection.ToLower = "no" Or Me.ItemsLocation.TrustedConnection.ToLower = "false") Then
      '  Me.ItemsLocation.Location = Me.ItemsLocation.Location & "User Id=" & Me.ItemsLocation.UserName & ";Password=" & Me.ItemsLocation.Password
      'End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Properties"

  Private ReadOnly Property MasterDatabaseConnection() As String
    Get

      Dim lobjBuilder As New Microsoft.Data.SqlClient.SqlConnectionStringBuilder
      lobjBuilder("Data Source") = Me.ItemsLocation.ServerName
      lobjBuilder("Integrated Security") = Me.ItemsLocation.TrustedConnection
      lobjBuilder("Initial Catalog") = "master"

      If (ItemsLocation.TrustedConnection.Equals("no", StringComparison.CurrentCultureIgnoreCase) Or ItemsLocation.TrustedConnection.Equals("false", StringComparison.CurrentCultureIgnoreCase)) _
        Then
        lobjBuilder("User Id") = ItemsLocation.UserName
        lobjBuilder("Password") = ItemsLocation.Password
      End If

      mstrMasterDBConnectionString = lobjBuilder.ConnectionString

      'mstrMasterDBConnectionString = MASTERDB_CONNECTION_STRING
      'mstrMasterDBConnectionString = mstrMasterDBConnectionString.Replace("<SERVERNAME>", Me.ItemsLocation.ServerName)
      'mstrMasterDBConnectionString = mstrMasterDBConnectionString.Replace("<TC>", Me.ItemsLocation.TrustedConnection)
      'If (Me.ItemsLocation.TrustedConnection.ToLower = "no") Then
      '  mstrMasterDBConnectionString = mstrMasterDBConnectionString & "Uid=" & Me.ItemsLocation.UserName & ";Pwd=" & Me.ItemsLocation.Password
      'End If

      Return mstrMasterDBConnectionString
    End Get
  End Property

  Private ReadOnly Property ServerConnectionString
    Get
      Try
        If String.IsNullOrEmpty(mstrServerConnectionString) Then
          If Me.ItemsLocation Is Nothing Then
            Throw New InvalidOperationException("ItemsLocation is not initialized.")
          End If

          If String.IsNullOrEmpty(ItemsLocation.Location) Then
            Throw New InvalidConnectionStringException("No connection string in the items location.")
          End If

          Dim lobjConnectionBuilder As New SqlConnectionStringBuilder(Me.ItemsLocation.Location)

          If Not String.IsNullOrEmpty(lobjConnectionBuilder.InitialCatalog) Then
            lobjConnectionBuilder.InitialCatalog = String.Empty
          End If

          mstrServerConnectionString = lobjConnectionBuilder.ToString

        End If

        Return mstrServerConnectionString

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Private ReadOnly Property ScriptPath As String
    Get

      If String.IsNullOrEmpty(mstrScriptPath) Then
        mstrScriptPath = FileHelper.Instance.TempPath
      End If

      Return mstrScriptPath
    End Get
  End Property

  Private ReadOnly Property SqlVersion As SqlServerVersion
    Get
      Try
        If menuSqlVersion = SqlServerVersion.Unknown Then
          menuSqlVersion = GetSqlServerVersion()
        End If

        Return menuSqlVersion

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Private ReadOnly Property FileStreamLevel As FileStreamAccessLevel
    Get
      If menuFileStreamAccessLevel = FileStreamAccessLevel.NotSet Then
        menuFileStreamAccessLevel = GetFileStreamLevel()
      End If

      Return menuFileStreamAccessLevel
    End Get
  End Property

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' If the table tblJob does not already have a column called 'Process' this method will add that column
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub AddNewColumnsToTables()

    'Dim lobjSQLBuilder As New StringBuilder

    Try
      'With lobjSQLBuilder
      '  .AppendFormat("IF NOT Exists(select * from sys.columns where Name = N'Process' and Object_ID = Object_ID(N'{0}')) ", TABLE_JOB_NAME)
      '  .AppendFormat("ALTER TABLE {0} ADD Process ntext NULL", TABLE_JOB_NAME)
      'End With

      'ExecuteNonQuery(lobjSQLBuilder.ToString)

      '	ExecuteNonQuery(CreateInsertColumnSQL("TransformationSourcePath", "nvarchar(512)", TABLE_JOB_NAME))

      '	ExecuteNonQuery(CreateInsertColumnSQL("Process", "ntext NULL", TABLE_JOB_NAME))

      ' <Added by: Ernie at: 12/31/2013-8:54:05 AM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL("WorkSummary", "nvarchar(MAX) NULL", TABLE_JOB_NAME))
      ' </Added by: Ernie at: 12/31/2013-8:54:05 AM on machine: ERNIE-THINK>

      '	Added by Ernie Bahr Nov. 14th, 2012
      ExecuteNonQuery(CreateInsertColumnSQL("Configuration", "ntext NULL", TABLE_JOB_NAME))
      '	End added by Ernie Bahr Nov. 14th, 2012

      ' Do the same thing for the batch table
      'ExecuteNonQuery(lobjSQLBuilder.Replace(TABLE_JOB_NAME, TABLE_BATCH_NAME).ToString)
      '	ExecuteNonQuery(CreateInsertColumnSQL("Process", "ntext NULL", TABLE_BATCH_NAME))

      ' Do the same thing for the batch items table
      'ExecuteNonQuery(lobjSQLBuilder.Replace("Process", "ProcessResult").Replace(TABLE_BATCH_NAME, TABLE_BATCH_ITEM_NAME).ToString)
      ExecuteNonQuery(CreateInsertColumnSQL("ProcessResult", "ntext NULL", TABLE_BATCH_ITEM_NAME))

      ' <Added by: Ernie at: 12/31/2013-8:54:48 AM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL("WorkSummary", "nvarchar(MAX) NULL", TABLE_PROJECT_NAME))
      ' </Added by: Ernie at: 12/31/2013-8:54:48 AM on machine: ERNIE-THINK>

      ' <Added by: Ernie at: 12/31/2013-8:56:21 AM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL("WorkSummary", "nvarchar(MAX) NULL", TABLE_BATCH_NAME))
      ' </Added by: Ernie at: 12/31/2013-8:56:21 AM on machine: ERNIE-THINK>

      ' <Added by: Ernie at: 4/26/2016-5:53:39 PM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL("ResultSummary", "ntext NULL", TABLE_BATCH_NAME))
      ExecuteNonQuery(CreateInsertColumnSQL("ResultSummary", "ntext NULL", TABLE_JOB_NAME))
      ' </Added by: Ernie at: 4/26/2016-5:53:39 PM on machine: ERNIE-THINK>

      ' <Added by: Ernie at: 9/3/2013-8:31:00 AM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL(PROJECT_ITEMS_PROCESSED_COLUMN, "bigint NULL", TABLE_PROJECT_NAME))
      ExecuteNonQuery(CreateInsertColumnSQL(PROJECT_ITEMS_PROCESSED_COLUMN, "bigint NULL", TABLE_JOB_NAME))
      SeedItemsProcessedForProject()
      ' </Added by: Ernie at: 9/3/2013-8:31:00 AM on machine: ERNIE-THINK>

      ' <Added by: Ernie at: 8/14/2015-10:56:52 AM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL("CatalogConfiguration", "ntext NULL", TABLE_CATALOG_NAME),
                      ProjectCatalog.Instance.ConnectionString)
      ExecuteNonQuery(CreateInsertColumnSQL("ItemsProcessed", "bigint NULL", TABLE_CATALOG_NAME),
                      ProjectCatalog.Instance.ConnectionString)
      ' </Added by: Ernie at: 8/14/2015-10:56:52 AM on machine: ERNIE-THINK>

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function CreateInsertColumnSQL(lpColumnName As String, lpColumnDetails As String, lpTableName As String) _
    As String

    Dim lobjSQLBuilder As New StringBuilder

    Try

      With lobjSQLBuilder
        .AppendFormat("IF NOT Exists(select * from sys.columns where Name = N'{0}' and Object_ID = Object_ID(N'{1}')) ",
                      lpColumnName, lpTableName)
        .AppendFormat("ALTER TABLE {1} ADD {0} {2}", lpColumnName, lpTableName, lpColumnDetails)
      End With

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  '''     Sets the value of the ItemsProcessed column of the project table if it is null.
  ''' </summary>
  Private Sub SeedItemsProcessedForProject()
    Try

      Dim lstrItemsProcessedSql As String = String.Format("SELECT ItemsProcessed FROM {0}", TABLE_PROJECT_NAME)

      Dim lstrSuccessCountSql As String = String.Format("SELECT COUNT(ID) FROM {0} WHERE ProcessedStatus = 'Success'",
                                                        TABLE_BATCH_ITEM_NAME)

      Dim lobjItemsProcessedResult As Object = ExecuteSimpleQuery(lstrItemsProcessedSql)

      If IsDBNull(lobjItemsProcessedResult) OrElse lobjItemsProcessedResult = 0 Then
        Dim lobjJobSuccessCountResult As Object = ExecuteSimpleQuery(lstrSuccessCountSql)
        If IsDBNull(lobjJobSuccessCountResult) Then
          SetItemsProcessedForProject(0)
        Else
          SetItemsProcessedForProject(lobjJobSuccessCountResult)
        End If
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Private Function GetItemsProcessedForProject() As Long
  '  Try

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Private Function GetItemsProcessedForJob(lpJobName As String) As Long
    Try

      Dim lstrJobViewName As String = GenerateJobViewName(lpJobName)

      Dim lstrItemsProcessedSql As String = String.Format("SELECT ItemsProcessed FROM [{0}] WHERE JobName = '{1}'",
                                                          TABLE_JOB_NAME, lpJobName)

      Dim lstrSuccessCountSql As String = String.Format("SELECT COUNT(ID) FROM [{0}] WHERE ProcessedStatus = 'Success'",
                                                        lstrJobViewName)

      Dim lobjItemsProcessedResult As Object = ExecuteSimpleQuery(lstrItemsProcessedSql)

      If IsDBNull(lobjItemsProcessedResult) OrElse lobjItemsProcessedResult = 0 Then
        Dim lobjJobSuccessCountResult As Object
        Try
          lobjJobSuccessCountResult = ExecuteSimpleQuery(lstrSuccessCountSql)
        Catch SqlEx As SqlException
          If SqlEx.Message.StartsWith(INVALID_OBJECT_NAME) Then
            CreateOrUpdateJobViewDB(lpJobName)
            lobjJobSuccessCountResult = ExecuteSimpleQuery(lstrSuccessCountSql)
          Else
            Throw
          End If
        End Try

        If IsDBNull(lobjJobSuccessCountResult) Then
          Return 0
        Else
          Return CLng(lobjJobSuccessCountResult)
        End If
      Else
        Return lobjItemsProcessedResult
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  '''     Sets the value of the ItemsProcessed column of the project table if it is null.
  ''' </summary>
  Private Sub SeedItemsProcessedForJob(lpJobName As String)
    Try

      Dim llngJobItemsProcessed As Long = GetItemsProcessedForJob(lpJobName)
      Dim lstrItemsProcessedSql As String = String.Format("SELECT ItemsProcessed FROM [{0}] WHERE JobName = '{1}'",
                                                          TABLE_JOB_NAME, lpJobName)
      Dim lobjItemsProcessedResult As Object = ExecuteSimpleQuery(lstrItemsProcessedSql)

      If IsDBNull(lobjItemsProcessedResult) OrElse lobjItemsProcessedResult = 0 Then
        SetItemsProcessedForJob(lpJobName, llngJobItemsProcessed)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SetItemsProcessedForProject(lpProcessedCount As Long)

    Try

      Dim lstrSQL As String = String.Format("UPDATE [{0}] SET ItemsProcessed = {1}", TABLE_PROJECT_NAME,
                                            lpProcessedCount)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lobjCommand As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintRowsAffected As Integer = lobjCommand.ExecuteNonQuery()
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SetItemsProcessedForJob(lpJobName As String, lpProcessedCount As Long)

    Try
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)


        Dim lstrSQL As String = String.Format("UPDATE [{0}] SET ItemsProcessed = {1} WHERE JobName = '{2}'",
                                              TABLE_JOB_NAME, lpProcessedCount, lpJobName)


        Dim lobjCommand As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintRowsAffected As Integer = lobjCommand.ExecuteNonQuery()

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Public Sub CreateRepositoryTable()

  '  Dim lobjSQLBuilder As New StringBuilder

  '  Try

  '    ' CREATE TABLE [dbo].[tblRepository](
  '    '	  [Id] [nvarchar](255) NOT NULL,
  '    '	  [Name] [nchar](255) NOT NULL,
  '    '	  [ConnectionString] [nvarchar](3000) NULL,
  '    '	  [Repository] [image] NOT NULL,
  '    '	  [LastUpdateDate] [datetime] NULL
  '    ' ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

  '    ' GO

  '    With lobjSQLBuilder
  '      .AppendLine("CREATE TABLE [dbo].[tblRepository](")
  '      .AppendLine("  [Id] [nvarchar](255) NOT NULL,")
  '      .AppendLine("  [Name] [nchar](255) NOT NULL,")
  '      .AppendLine("  [ConnectionString] [nvarchar](3000) NULL,")
  '      .AppendLine("  [Repository] [image] NOT NULL,")
  '      .AppendLine("  [LastUpdateDate] [datetime] NULL")
  '      .AppendLine(") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]")
  '    End With

  '    'ExecuteNonQuery
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Public Overrides Function GetOrphanBatchCount() As Integer
    Try
      Return GetOrphanBatchCountDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetOrphanBatchItemCount() As Integer
    Try
      Return GetOrphanBatchItemCountDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProjectInfo() As IProjectInfo
    Try
      Return GetProjectInfoDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchInfoCollection(lpJob As IJobInfo) As IBatchInfoCollection
    Try
      Return GetBatchInfoCollectionDB(lpJob)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobInfo(lpJobId As String) As IJobInfo
    Try
      Return GetJobInfoDB(lpJobId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetDetailedJobInfo(lpJobId As String) As IDetailedJobInfo
    Try
      Return GetDetailedJobInfoDB(lpJobId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobInfoCollection() As IJobInfoCollection
    Try
      Return GetJobInfoCollectionDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobRelationships(lpJobId As String) As JobRelationships
    Try
      Return GetJobRelationshipsDB(lpJobId)
    Catch SqlEx As SqlException
      ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod)
      If SqlEx.Message.Contains(INVALID_OBJECT_NAME) Then
        CreateTables(DatabaseType.Project)
        Return GetJobRelationships(lpJobId)
      Else
        '   Re-throw the exception to the caller
        Throw
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProjectSummaryCounts(ByVal lpProject As Project) As WorkSummaries

    Try
      Return GetProjectSummaryCountsDB(lpProject.Id)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Function

  Public Overrides Function GetCachedWorkSummaryCounts(lpWorkParent As Object) As IWorkSummary
    Try
      Return GetCachedWorkSummaryCountsDB(lpWorkParent)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProjectAvgProcessingTime(lpProject As Project) As Single

    Try
      Return GetProjectAvgProcessingTimeDB(lpProject.Id)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProjectDbFileInfo() As DbFilesInfo

    Try
      Return GetProjectDbFileInfoDB()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Function

  Public Overrides Function FileExists(ByVal lpJob As Job, ByVal lpFileName As String) As Boolean

    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpJob)
#Else
      If lpJob Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpJob))
      End If
#End If
      Return FileExistsDB(lpJob.Id, lpFileName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFileList(ByVal lpJob As Job) As IList(Of String)

    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpJob)
#Else
      If lpJob Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpJob))
      End If
#End If

      Return GetFileListDB(lpJob.Id)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Overrides Function RetrieveFile(lpJob As Job, lpFileName As String) As IO.Stream
    Try
      Return RetrieveFileDB(lpJob, lpFileName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Overrides Function RetrieveFile(lpFileId As String, lpFileName As String) As IO.Stream
    Try
      Return RetrieveFileDB(lpFileId, lpFileName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub StoreFile(ByVal lpJob As Job, ByVal lpFilePath As String)
    Try

      If String.IsNullOrEmpty(lpFilePath) Then
        Throw New ArgumentNullException(NameOf(lpFilePath))
      End If

      If File.Exists(lpFilePath) = False Then
        Throw New InvalidPathException(lpFilePath)
      End If

      Dim lstrFileName As String = Path.GetFileName(lpFilePath)

      If lpJob IsNot Nothing Then
        StoreFileDB(lpJob.Id, lstrFileName, Helper.WriteFileToByteArray(lpFilePath))
      Else
        StoreFileDB(String.Empty, lstrFileName, Helper.WriteFileToByteArray(lpFilePath))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub StoreFile(lpJob As Job, lpFileName As String, lpFileData As IO.Stream)
    Try

      If String.IsNullOrEmpty(lpFileName) Then
        Throw New ArgumentNullException(NameOf(lpFileName))
      End If

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpFileData)
#Else
      If lpFileData Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpFileData))
      End If
#End If

      If lpFileData.CanRead = False Then
        Throw New InvalidOperationException("Unable to store file, the stream can't be read.")
      End If

      If lpJob IsNot Nothing Then
        StoreFileDB(lpJob.Id, lpFileName, Helper.CopyStreamToByteArray(lpFileData))
      Else
        StoreFileDB(String.Empty, lpFileName, Helper.CopyStreamToByteArray(lpFileData))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub UpdateProcess(lpJob As Job)
    Try
      Throw New NotImplementedException("This method has been deprecated.")
      'If lpJob.Process IsNot Nothing Then
      '  lpJob.Operation = lpJob.Process.Name
      'End If
      'UpdateProcessDB(lpJob)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub UpdateTransformations(ByVal lpJob As Job)

    Try
      UpdateTransformationsDB(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Sub

  Public Overrides Sub UpdateTransformations(ByVal lpBatch As Batch)

    Try
      UpdateTransformationsDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Sub

  Public Overrides Sub UpdateJobSource(ByVal lpJob As Job)

    Try
      UpdateJobSourceDB(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Sub

  Public Overrides Function GetUpdateTable(ByVal lpJob As Job,
                                           ByVal lpUpdateItems As DataTable) As DataTable

    Try
      Return GetUpdateTableDB(lpJob, lpUpdateItems)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Function

  Public Overrides Sub RefreshTransform(ByVal lpJob As Job)

    Try

      'Insert/Update Job tables
      SaveJobToDB(lpJob)

      'Insert/Update Batch tables
      SaveBatchesToDB(lpJob.Batches)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function IsAvailableForProcessing(ByVal lpBatchId As String) As Boolean

    Try
      Return IsAvailableForProcessingDB(lpBatchId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Return False
    End Try
  End Function

  Public Overrides Function GetItemsById(ByVal lpBatch As Batch,
                                         ByVal lpIdArrayList As ArrayList,
                                         ByVal lpIncludeProcessResults As Boolean) As BatchItems

    Try
      Return GetItemsByIdDB(lpBatch, lpIdArrayList, lpIncludeProcessResults)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemsById(lpJob As Job, lpIdTable As DataTable) As BatchItems
    Try
      Return GetItemsByIdDB(lpJob, lpIdTable)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchItemById(lpId As String) As IBatchItem
    Try
      Return GetBatchItemByIdDB(lpId)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemById(lpJobName As String, lpId As String, lpScope As OperationScope) As IBatchItem
    Try
      Return GetItemByIdDB(lpJobName, lpId, lpScope)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub ResetItemsToNotProcessed(ByVal lpIdArrayList As ArrayList)

    Try
      ResetItemsToNotProcessedDB(lpIdArrayList)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function ResetFailedItemsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) As Integer
    Try
      Return ResetFailedItemsByProcessedMessageDB(lpJob, lpProcessedMessage)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub ResetFailedItemsToNotProcessed(ByVal lpBatch As Batch)

    Try
      ResetFailedItemsToNotProcessedDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overrides Sub ResetItemsToNotProcessed(lpBatch As Batch, lpCurrentStatus As ProcessedStatus)
    Try
      ResetItemsToNotProcessedDB(lpBatch, lpCurrentStatus)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overrides Sub ResetItemsToNotProcessed(lpJob As Job, lpCurrentStatus As ProcessedStatus)
    Try
      ResetItemsToNotProcessedDB(lpJob, lpCurrentStatus)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub LockBatch(ByVal lpBatchId As String,
                                 ByVal lpLockedBy As String)

    Try
      LockBatchDB(lpBatchId, lpLockedBy)

    Catch ex As Exception

      Dim lobjBatchLockedEx As New BatchLockedException(ex.Message)
      ApplicationLogging.LogException(lobjBatchLockedEx, MethodBase.GetCurrentMethod)
      Throw lobjBatchLockedEx
    End Try
  End Sub

  Public Overrides Sub UnLockBatch(ByVal lpBatchId As String,
                                   ByVal lpUnLockedBy As String)

    Try
      UnLockBatchDB(lpBatchId, lpUnLockedBy)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteAll()

    Try
      CleanEntireDB()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteBatches(ByVal lpBatches As Batches)

    Try
      DeleteBatchesDB(lpBatches)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteOrphanBatches(lpProject As Project)
    Try
      DeleteOrphanBatchesDBAsync(lpProject)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteJob(ByVal lpJob As Job)

    Try
      DeleteJobDB(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function GetJobById(lpJobId As String) As Job
    Try
      Return GetJobByIdDB(lpJobId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobConfigurationByName(lpJobName As String) As String
    Try
      Return GetJobConfigurationByNameDB(lpJobName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobIdentifiers() As IJobIdentifiers
    Try
      Return GetJobIdentifiersDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub DeleteProject(ByVal lpProject As Project)

    Try
      DeleteProjectDB(lpProject)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function AddItem(ByVal lpBatchItem As BatchItem) As Boolean

    Try

      'Old way, adds one record at a time
      'Return AddItemToDB(lpBatchItem)

      'Adds items to a dataset.  Requires caller to call CommitBatchItems() to commit to database
      Return AddBatchItem(lpBatchItem)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overrides Sub ClearWorkSummary(lpWorkParent As Object)
    Try
      ClearWorkSummaryDB(lpWorkParent)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub CommitBatchItems()

    Try

      If (mobjDataTable.Rows.Count = 0) Then
        Exit Sub
      End If

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Dim lobjAdapter As New SqlDataAdapter With {
          .InsertCommand =
          New SqlCommand(
            "INSERT INTO " & TABLE_BATCH_ITEM_NAME &
            " (BatchId,Title,SourceDocId,ProcessedStatus,Operation) VALUES (@BatchId,@Title,@SourceDocId,@ProcessedStatus,@Operation);",
            lobjConnection)
        }
        lobjAdapter.InsertCommand.CommandTimeout = COMMAND_TIMEOUT
        lobjAdapter.InsertCommand.Parameters.Add("@BatchId", SqlDbType.NVarChar, 255, "BatchId")
        lobjAdapter.InsertCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 255, "Title")
        lobjAdapter.InsertCommand.Parameters.Add("@SourceDocId", SqlDbType.NVarChar, 255, "SourceDocId")
        lobjAdapter.InsertCommand.Parameters.Add("@ProcessedStatus", SqlDbType.NVarChar, 50, "ProcessedStatus")
        lobjAdapter.InsertCommand.Parameters.Add("@Operation", SqlDbType.NVarChar, 255, "Operation")
        lobjAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None

        ' Set the batch size.
        'lobjAdapter.UpdateBatchSize = 500 '0 = Let sql decide the max batchsize it will take
        lobjAdapter.UpdateBatchSize = 0 '0 = Let sql decide the max batchsize it will take

        ' Execute the Insert.
        lobjAdapter.Update(mobjDataTable)

        lobjConnection.Close()
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally
      CreateDataTable() 'Reset the datatable in case the caller wants to add some more
    End Try
  End Sub

  Public Overrides Sub CreateListBatches(lpArgs As IDBLookupSourceEventArgs)
    Try
      CreateListBatchesDB(lpArgs)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveProject(ByVal lpProject As Project)

    Try
      MyBase.SaveProject(lpProject)
      SaveProjectToDB(lpProject)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub InitializeCachedRepositories(lpProject As Project)
    Try
      InitializeRepositoriesAsync(lpProject)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function OpenProject(ByRef lpErrorMessage As String) As Project

    Try

      Return OpenProjectDB(lpErrorMessage)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub SaveBatch(lpBatch As Batch)
    Try
      SaveBatchToDB(lpBatch)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveBatches(lpJob As Job)
    Try
      SaveBatchesToDB(lpJob.Batches)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveJob(ByRef lpJob As Job)

    Try
      ' SaveJobToDB(lpJob)
      If Not String.IsNullOrEmpty(lpJob.Id) Then
        SaveJobToDB(lpJob.Project.Id, lpJob.Configuration, lpJob.Id)
      Else
        Dim lstrJobId As String = String.Empty
        SaveJobToDB(lpJob.Project.Id, lpJob.Configuration, lstrJobId)
        'lpJob.Id = lstrJobId
        lpJob = GetJobById(lstrJobId)
      End If

      SaveBatchesToDB(lpJob.Batches)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Overrides Sub SaveJob(ByVal lpProjectId As String,
                                         lpJobConfiguration As Configuration.JobConfiguration)
    Try
      SaveJobToDB(lpProjectId, lpJobConfiguration, String.Empty)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveJobRelationship(lpJobRelationship As JobRelationship)
    Try
      SaveJobRelationshipToDB(lpJobRelationship)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function GetItems(ByVal lpBatch As Batch,
                                     ByVal lpStart As Integer,
                                     ByVal lpItemsToGet As Integer,
                                     ByVal lpSortColumn As String,
                                     ByVal lpAscending As Boolean,
                                     ByVal lpProcessedStatusFilter As String) As BatchItems

    Try
      Return GetItemsFromDB(lpBatch, lpStart, lpItemsToGet, lpSortColumn, lpAscending, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllItems(ByVal lpBatch As Batch,
                                        ByVal lpProcessedStatus As String) As BatchItems

    Try
      Return GetAllItemsFromDB(lpBatch, lpProcessedStatus)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemsToDataTable(ByVal lpBatch As Batch,
                                                ByVal lpStart As Integer,
                                                ByVal lpItemsToGet As Integer,
                                                ByVal lpSortColumn As String,
                                                ByVal lpAscending As Boolean,
                                                ByVal lpProcessedStatusFilter As String) As System.Data.DataTable

    Try
      Return _
        GetItemsFromDBToDataTable(lpBatch, lpStart, lpItemsToGet, lpSortColumn, lpAscending, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllItemsToDataTable(ByVal lpObject As Object,
                                                   ByVal lpProcessedStatus As String) As DataTable

    Try
      Return GetAllItemsFromDBToDataTable(lpObject, lpProcessedStatus)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllProcessedByNodeNames(lpJob As Job) As IList(Of String)
    Try
      Return GetAllProcessedByNodeNamesFromDB(lpJob)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFilteredItemsToDataTable(lpItemFilter As ItemFilter) As DataTable

    Try
      Return GetFilteredItemsFromDBToDataTable(lpItemFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllUnprocessedItems(ByVal lpBatch As Batch) As BatchItems

    Try
      Return GetAllUnprocessedItemsFromDB(lpBatch)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchLockCount(lpJobName As String) As Integer
    Try
      Return GetBatchLockCountDB(lpJobName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllBatchLocks() As IBatchLocks
    Try
      Return GetBatchLocksDB(String.Empty)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatches(lpJob As Job) As Batches
    Try
      Return GetBatchesDB(lpJob)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchIds(lpJobId As String) As IList(Of String)
    Try
      Return GetBatchIdsDB(lpJobId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchLocks(lpJobName As String) As IBatchLocks
    Try
      Return GetBatchLocksDB(lpJobName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function


  Public Overrides Function GetItemCount(ByVal lpBatch As Batch) As Integer
    Try
      Return GetItemCountDB(lpBatch)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemCount(ByVal lpJob As Job) As Long
    Try
      Return GetItemCountDB(lpJob)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemCount(ByVal lpProject As Project) As Long
    Try
      Return GetItemCountDB(lpProject)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  ''' Called when a batch item has started to be migrated,exported,etc
  ''' </summary>
  ''' <param name="lpProcessedItemEventArgs"></param>
  ''' <remarks></remarks>
  Public Overrides Sub BeginProcessItem(ByVal lpProcessedItemEventArgs As BatchItemProcessEventArgs)

    Try
      'ApplicationLogging.WriteLogEntry(String.Format("BEGIN: BatchId='{0}'  BatchContainerId='{1}'  CurrentThread='{2}'", lpProcessedItemEventArgs.BatchId, Me.Id, Threading.Thread.CurrentThread.GetHashCode.ToString), TraceEventType.Information, 0)
      'BeginItemProcessDB(lpProcessedItemEventArgs)
      UpdateBatchItemDB(lpProcessedItemEventArgs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      'Throw
    End Try
  End Sub

  ''' <summary>
  ''' Called when a batch item has been migrated,exported,etc
  ''' </summary>
  ''' <param name="lpProcessedItemEventArgs"></param>
  ''' <remarks></remarks>
  Public Overrides Sub EndProcessItem(ByVal lpProcessedItemEventArgs As BatchItemProcessEventArgs)

    Try
      'ApplicationLogging.WriteLogEntry(String.Format("END: BatchId='{0}'  BatchContainerId='{1}'  CurrentThread='{2}'", lpProcessedItemEventArgs.BatchId, Me.Id, Threading.Thread.CurrentThread.GetHashCode.ToString), TraceEventType.Information, 0)
      'EndItemProcessDB(lpProcessedItemEventArgs)
      UpdateBatchItemDB(lpProcessedItemEventArgs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      'Throw
    End Try
  End Sub

  Public Overrides Function GetWorkSummaryCounts(ByVal lpBatch As Batch) As WorkSummary

    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpBatch)
#Else
      If lpBatch Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpBatch))
      End If
#End If

      Return GetBatchSummaryCountsDB(lpBatch.Id)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetWorkSummaryCounts(ByVal lpJob As Job) As WorkSummary

    Try
      Return GetWorkSummaryCountsDB(lpJob)

    Catch invalidOpEx As InvalidOperationException
      Return Nothing
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetWorkSummaryCounts(ByVal lpProject As Project) As WorkSummary

    Try
      Return GetWorkSummaryCountsForProjectDB(lpProject.Id)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFailureItemIdsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) _
    As IList(Of String)

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFailureSummaries(ByVal lpProject As Project) As FailureSummaries

    Try
      Return GetFailureSummariesDB(lpProject)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFailureSummaries(ByVal lpJob As Job) As FailureSummaries

    Try
      Return GetFailureSummariesDB(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFailureSummaries(ByVal lpBatch As Batch) As FailureSummaries

    Try
      Return GetFailureSummariesDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProcessedItemsCount(ByVal lpProject As Project) As Long
    Try
      Return GetItemsProcessedDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProcessedItemsCount(ByVal lpJob As Job) As Long
    Try
      Return GetItemsProcessedForJob(lpJob.Name)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overrides Function GetProcessResults(lpBatch As Batch) As IProcessResults
    Try
      Return GetProcessResultsDB(lpBatch)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overrides Function GetProcessResults(lpJob As Job) As IProcessResults
    Try

      Return GetProcessResultsDB(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overrides Sub SetProcessedItemsCount(lpProject As Project, lpProcessedCount As Long)
    Try
      SetItemsProcessedForProject(lpProcessedCount)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overrides Sub SetProcessedItemsCount(lpJob As Job, lpProcessedCount As Long)
    Try

      Dim lintOriginalJobProcessedCount As Long = GetProcessedItemsCount(lpJob)
      SetItemsProcessedForJob(lpJob.Name, lpProcessedCount)

      ' Update the total for the project to reflect the new processed count.
      If lpProcessedCount > lintOriginalJobProcessedCount Then
        ' If the new number is larger than the original then add the new number
        SetProcessedItemsCount(lpJob.Project, GetProcessedItemsCount(lpJob.Project) + lpProcessedCount)
      ElseIf lintOriginalJobProcessedCount < lpProcessedCount Then
        ' If the new number is less than the original then subtract the difference
        SetProcessedItemsCount(lpJob.Project,
                               GetProcessedItemsCount(lpJob.Project) -
                               (lintOriginalJobProcessedCount - lpProcessedCount))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Methods"

#Region "Get Methods"

  ''' <summary>
  ''' Assumes only 1 project exists in the DB
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function OpenProjectDB(ByRef lpErrorMessage As String) As Project

    Dim lobjProject As Project = Nothing
    Dim lobjDataReader As SqlDataReader = Nothing

    Try

      ' <Added by: Ernie at: 11/26/2011-7:14:50 AM on machine: ERNIE-M4400>
      AddNewColumnsToTables()
      ' </Added by: Ernie at: 11/26/2011-7:14:50 AM on machine: ERNIE-M4400>

      CreateStoredProcedures(DatabaseType.Project)

      '' Get the db file info first
      'Try
      '  Dim lobjDbFilesInfo As DbFilesInfo = GetProjectDbFileInfo()
      '  'LogSession.LogString(level.Message, "Project database file information", lobjDbFilesInfo.ToXmlString())
      'Catch ex As Exception
      '  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      '  'LogSession.LogError("Unable to get project database file information.")
      'End Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)


        Dim lstrSQL As String = String.Format("SELECT TOP 1 * from {0}", TABLE_PROJECT_NAME)
        Dim lobjCommand As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        lobjDataReader = lobjCommand.ExecuteReader

        If (lobjDataReader.HasRows = True) Then

          While lobjDataReader.Read

            '' lobjProject = New Project(lobjDataReader("ProjectName").ToString, lobjDataReader("Description").ToString(), DeserializeString(lobjDataReader("ItemsLocation").ToString(), ItemsLocation.GetType, New ItemsLocation()), Convert.ToInt32(lobjDataReader("BatchSize").ToString), lobjDataReader("ProjectId").ToString())
            'lobjProject = New Project(lobjDataReader("ProjectName").ToString, _
            '                          lobjDataReader("Description").ToString(), _
            '                          Me.ItemsLocation, Convert.ToInt32(lobjDataReader("BatchSize").ToString), _
            '                          lobjDataReader("ProjectId").ToString(), _
            '                          lobjDataReader("CreateDate"), _
            '                          lobjDataReader("ItemsProcessed"))
            'Dim lobjProjectDescription As IProjectDescription = ProjectCatalog.Instance.GetProject(lobjProject.Id)
            'lobjProject.Area = lobjProjectDescription.Area

            lobjProject = OpenProjectFromDataReader(lobjDataReader)

          End While

        Else
          Throw _
            New Exception(String.Format("Unable to find a project in '{0}' table at location '{1}'", TABLE_PROJECT_NAME,
                                        Me.ItemsLocation.Location))
        End If

        lobjDataReader?.Close()

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

      With lobjProject
        ' Get the count of orphan batches and batch items
        If .OrphanBatchCount > 0 Then
          ' Only request the entire orphan batch item count if any orphan batches are found.
          .OrphanBatchItemCount = GetOrphanBatchItemCount()
        Else
          .OrphanBatchItemCount = 0
        End If

        ' Get the cached repositories
        '.Repositories = GetRepositories(lobjProject)
        'InitializeRepositoriesAsync(lobjProject)

        'Get all the associated Jobs and batches
        .Jobs = GetJobsDB(lobjProject, lpErrorMessage)
      End With

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      lobjDataReader?.Close()

    End Try

    Return lobjProject
  End Function

  Private Async Sub DeleteOrphanBatchesDBAsync(lpProject As Project)
    Try
      Dim lobjTask As Task = Task.Factory.StartNew(
        Sub()
          DeleteOrphanBatchesDB(lpProject)
        End Sub)

      Await lobjTask

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub DeleteOrphanBatchesDB(lpProject As Project)
    Try
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_delete_orphan_batches", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          lobjCommand.ExecuteNonQuery()
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Async Sub InitializeRepositoriesAsync(lpProject As Project)
    ' TODO: Change this to a function that returns the repositories collection since we can't pass the project by reference.
    Try
      Dim lobjTask As Task = Task.Factory.StartNew(
        Sub()
          lpProject.Repositories.AddRange(GetRepositories(lpProject))
        End Sub)

      Await lobjTask

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function OpenProjectFromDataReader(lpDataReader As IDataReader) As Project
    Try

      Dim lstrProjectName As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_NAME_COLUMN, String.Empty)
      Dim lstrDescription As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_DESCRIPTION_COLUMN,
                                                                    String.Empty)
      Dim lstrProjectId As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_ID_COLUMN, String.Empty)
      Dim ldatCreateDate As DateTime = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_CREATE_DATE_COLUMN, Now)
      Dim llngItemsProcessed As Long = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_ITEMS_PROCESSED_COLUMN, 0)
      Dim lstrWorkSummary As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_WORK_SUMMARY_COLUMN,
                                                                    String.Empty)
      Dim lintBatchSize As Integer = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_BATCH_SIZE_COLUMN, 1000)
      Dim lobjWorkSummary As IWorkSummary = New WorkSummary(lstrWorkSummary, lstrProjectName)
      Dim lobjProject As New Project(lstrProjectName, lstrDescription, ItemsLocation,
                            lintBatchSize, lstrProjectId, ldatCreateDate, llngItemsProcessed,
                            lobjWorkSummary)

      Dim lobjProjectDescription As IProjectDescription

      If ProjectCatalog.Instance.ProjectExists(lobjProject.Id) = False Then

        ApplicationLogging.WriteLogEntry(
          String.Format("Project does not exist in the catalog, adding project '{0}' based on location string '{1}'.",
                        lstrProjectName, Me.ItemsLocation.Location), MethodBase.GetCurrentMethod,
          TraceEventType.Information, 56393)

        ProjectCatalog.AddProjectFromConnectionString(Me.ItemsLocation.Location)

        If ProjectCatalog.Instance.ProjectExists(lobjProject.Id) = False Then

          ApplicationLogging.WriteLogEntry(
            String.Format("Failed to add project '{0}' to the catalog based on location string '{1}'.",
                          lstrProjectName, Me.ItemsLocation.Location), MethodBase.GetCurrentMethod,
            TraceEventType.Warning, 56394)

          Dim lstrAlternateConnectionString As String = Me.ItemsLocation.ToNativeConnectionString()

          ApplicationLogging.WriteLogEntry(
            String.Format(
              "Project does not exist in the catalog, adding project '{0}' based on derived native connection string '{1}'.",
              lstrProjectName, lstrAlternateConnectionString), MethodBase.GetCurrentMethod,
            TraceEventType.Information, 56395)

          ProjectCatalog.AddProjectFromConnectionString(lstrAlternateConnectionString)

          If ProjectCatalog.Instance.ProjectExists(lobjProject.Id) = False Then

            ApplicationLogging.WriteLogEntry(
              String.Format("Failed again to add project '{0}' to the catalog based on location string '{1}'.",
                            lstrProjectName, lstrAlternateConnectionString), MethodBase.GetCurrentMethod,
              TraceEventType.Warning, 56396)

          End If

        End If

      End If

      If ProjectCatalog.Instance.ProjectExists(lobjProject.Id) Then

        Try
          lobjProjectDescription = ProjectCatalog.Instance.GetProject(lobjProject.Id)

        Catch NoProjectEx As ProjectDoesNotExistException
          ProjectCatalog.AddProjectFromConnectionString(Me.ItemsLocation.Location)
          lobjProjectDescription = ProjectCatalog.Instance.GetProject(lobjProject.Id)

        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try

        If lobjProjectDescription IsNot Nothing Then
          lobjProject.Area = lobjProjectDescription.Area
        End If

      End If

      Return lobjProject

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function OpenProjectDescriptionFromDataReader(lpDataReader As IDataReader, lpArea As IArea) _
    As IProjectDescription
    Try

      Dim lstrProjectName As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_NAME_COLUMN, String.Empty)
      Dim lstrProjectId As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_ID_COLUMN, String.Empty)
      Dim lstrProjectLocation As String = Helper.GetValueFromDataRecord(lpDataReader, CATALOG_PROJECT_LOCATION_COLUMN,
                                                                        String.Empty)
      Dim ldatCreateDate As DateTime = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_CREATE_DATE_COLUMN, Now)
      Dim llngItemsProcessed As Long = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_ITEMS_PROCESSED_COLUMN, 0)
      Dim lstrWorkSummary As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_WORK_SUMMARY_COLUMN,
                                                                    String.Empty)
      Dim lobjWorkSummary As IWorkSummary = New WorkSummary(lstrWorkSummary, lstrProjectName)

      Dim lobjProjectDescription As IProjectDescription

      lobjProjectDescription = New ProjectDescription(lstrProjectId,
                                                      lstrProjectName,
                                                      lstrProjectLocation,
                                                      ldatCreateDate,
                                                      lpArea,
                                                      llngItemsProcessed,
                                                      lobjWorkSummary)

      Return lobjProjectDescription

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  ''' Given a project, retreive all the jobs
  ''' </summary>
  ''' <param name="lpProject"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function GetJobsDB(ByVal lpProject As Project, ByRef lpErrorMessage As String) As Jobs

    Dim lobjJobs As New Jobs(lpProject)
    Dim lobjDataReader As SqlDataReader = Nothing
    Dim lintJobErrorCount As Integer = 0
    Dim lstrJobErrorMessage As String = Nothing
    Dim lstrErrorMessageBuilder As New StringBuilder("Failed to load process for job ")

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        ''''JOBS''''
        'Dim lstrSQL As String = String.Format("SELECT JobId from {0} WHERE ProjectId = '{1}'", TABLE_PROJECTJOBREL_NAME, lpProject.Id)
        Dim lstrSQL As String = String.Empty
        Dim lobjStringBuilder As New StringBuilder
        lobjStringBuilder.AppendFormat("SELECT PJR.JobId FROM {0} PJR JOIN {1} J ON PJR.JobId = J.JobId ",
                                       TABLE_PROJECTJOBREL_NAME, TABLE_JOB_NAME)
        lobjStringBuilder.AppendFormat("WHERE PJR.ProjectId = '{0}' ", lpProject.Id)
        lobjStringBuilder.Append("ORDER BY J.JobName")

        lstrSQL = lobjStringBuilder.ToString

        Dim lobjCommand As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        lobjDataReader = lobjCommand.ExecuteReader

        If (lobjDataReader.HasRows = True) Then

          While lobjDataReader.Read

            Using lobjJobConnection As New SqlConnection(Me.ItemsLocation.Location)

              lstrSQL = String.Format("SELECT * from {0} WHERE JobId = '{1}'", TABLE_JOB_NAME,
                                      lobjDataReader("JobId").ToString)
              lobjCommand = New SqlCommand(lstrSQL, lobjJobConnection)
              Helper.HandleConnection(lobjJobConnection)

              Dim lobjJobDataReader As SqlDataReader = lobjCommand.ExecuteReader

              If (lobjJobDataReader.HasRows = True) Then

                While lobjJobDataReader.Read

                  lstrJobErrorMessage = Nothing
                  Dim lobjJob As Job = Nothing

                  Dim lobjBatchLocks As BatchLocks
                  Try
                    lobjJob = GetJobFromDataReader(lobjJobDataReader, lpProject, lstrJobErrorMessage)
                    SeedItemsProcessedForJob(lobjJob.Name)
                    lobjBatchLocks = GetBatchLocksDB(lobjJob.Name)
                  Catch ex As Exception
                    ' We will log and skip this job
                    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                    Continue While
                  End Try

                  If Not String.IsNullOrEmpty(lstrJobErrorMessage) Then
                    lintJobErrorCount += 1
                    lstrErrorMessageBuilder.AppendFormat("'{0}': {1}, ", lobjJob.Name, lstrJobErrorMessage)
                  End If

                  Dim lobjBatches As Batches = GetBatchesDB(lobjJob)

                  For Each lobjBatch As Batch In lobjBatches
                    ' If for some reason the process was corrupted or lost for the batch, we will reset it from the job
                    If ((lobjBatch.Process Is Nothing) AndAlso (lobjJob.Process IsNot Nothing)) Then
                      lobjBatch.Process = lobjJob.Process.Clone
                    End If
                    If lobjBatchLocks.Contains(lobjBatch.Id) Then
                      lobjBatch.Locked = True
                    End If
                    lobjJob.Batches.Add(lobjBatch)
                  Next

                  lobjJobs.Add(lobjJob)

                End While

                lobjJobDataReader?.Close()

                If (lobjJobConnection.State = ConnectionState.Open) Then
                  lobjJobConnection.Close()
                End If

              End If

            End Using

          End While

        End If

        lobjDataReader?.Close()

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      lobjDataReader?.Close()

      If lintJobErrorCount = 1 Then
        lpErrorMessage = lstrErrorMessageBuilder.Remove(lstrErrorMessageBuilder.Length - 2, 2).ToString
      ElseIf lintJobErrorCount > 1 Then
        lpErrorMessage =
          lstrErrorMessageBuilder.Remove(lstrErrorMessageBuilder.Length - 2, 2).Replace("job ", "jobs ").ToString
      End If
    End Try

    Return lobjJobs
  End Function

  Private Function GetJobRelationshipsDB(lpJobId As String) As JobRelationships
    Dim lobjJobRelationships As New JobRelationships
    Try
      Dim lstrSQL As String = String.Empty
      Dim lstrRelationshipId As String = String.Empty
      Dim lobjJobRelationship As JobRelationship = Nothing

      lstrSQL = String.Format("SELECT JobRelationshipId from {0} WHERE JobId = '{1}'",
                              TABLE_RELATED_JOBS_NAME, lpJobId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                lstrRelationshipId = Helper.GetValueFromDataRecord(lobjDataReader, JOB_RELATIONSHIP_ID_COLUMN,
                                                                   String.Empty)
                lobjJobRelationship = GetJobRelationshipDB(lstrRelationshipId)
                If lobjJobRelationship IsNot Nothing AndAlso Not lobjJobRelationships.Contains(lobjJobRelationship) Then
                  lobjJobRelationships.Add(lobjJobRelationship)
                End If
              End While
            End If
          End Using
        End Using
      End Using

      Return lobjJobRelationships

    Catch SqlEx As SqlException
      ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod)
      If SqlEx.Message.Contains(INVALID_OBJECT_NAME) Then
        CreateTables(DatabaseType.Project)
        Return GetJobRelationshipsDB(lpJobId)
      Else
        '   Re-throw the exception to the caller
        Throw
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobRelationshipDB(lpRelationshipId As String) As JobRelationship
    Dim lobjJobRelationship As JobRelationship = Nothing
    Try
      Dim lstrSQL As String = String.Empty

      lstrSQL = String.Format("SELECT * from {0} WHERE JobRelationshipId = '{1}'",
                              TABLE_JOB_RELATIONSHIPS_NAME, lpRelationshipId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.Read = True) Then
              lobjJobRelationship = GetJobRelationshipFromDataReader(lobjDataReader)
            End If
          End Using
        End Using
      End Using

      lobjJobRelationship.RelatedJobIds.AddRange(GetRelatedJobsIdsDB(lpRelationshipId))

      Return lobjJobRelationship

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetRelatedJobsIdsDB(lpRelationshipId As String) As List(Of String)
    Dim lobjRelatedJobIds As New List(Of String)
    Try
      Dim lstrSQL As String = String.Empty
      Dim lstrJobId As String = String.Empty

      lstrSQL = String.Format("SELECT JobId from {0} WHERE JobRelationshipId = '{1}'",
                              TABLE_RELATED_JOBS_NAME, lpRelationshipId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                lstrJobId = Helper.GetValueFromDataRecord(lobjDataReader, JOB_ID_COLUMN, String.Empty)
                If Not String.IsNullOrEmpty(lstrJobId) Then
                  lobjRelatedJobIds.Add(lstrJobId)
                End If
              End While
            End If
          End Using
        End Using
      End Using

      Return lobjRelatedJobIds

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchIdsDB(ByVal lpJobId As String) As List(Of String)

    Dim lobjDataReader As SqlDataReader = Nothing
    Dim lobjBatchIds As New List(Of String)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrSQL As String =
              String.Format(
                "SELECT j.BatchId, b.BatchName FROM {0} j JOIN {1} b on j.batchid = b.batchid WHERE JobId = '{2}' ORDER BY b.BatchName",
                TABLE_JOBBATCHREL_NAME, TABLE_BATCH_NAME, lpJobId)

        Dim lobjCommand As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        lobjDataReader = lobjCommand.ExecuteReader

        If (lobjDataReader.HasRows = True) Then
          While lobjDataReader.Read
            lobjBatchIds.Add(lobjDataReader("BatchId").ToString())
          End While
        End If

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

      Return lobjBatchIds

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally

      lobjDataReader?.Close()

    End Try
  End Function

  Private Function GetBatchesDB(ByVal lpJob As Job) As Batches

    Dim lobjBatches As New Batches(lpJob)
    Dim lobjConnection As SqlConnection = Nothing
    Dim lobjDataReader As SqlDataReader = Nothing

    Try

      Dim lobjBatchIds As List(Of String) = GetBatchIdsDB(lpJob.Id)

      If lobjBatchIds.Count > 0 Then

        'While lobjDataReader.Read

        Using lobjBatchConnection As New SqlConnection(Me.ItemsLocation.Location)

          For Each lstrBatchId As String In lobjBatchIds

            Dim lstrSQL As String = String.Format("SELECT  * from {0}  WHERE BatchId = '{1}'", TABLE_BATCH_NAME,
                                                  lstrBatchId)
            Dim lobjCommand As New SqlCommand(lstrSQL, lobjBatchConnection)
            Helper.HandleConnection(lobjBatchConnection)

            Dim lobjBatchDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjBatchDataReader.HasRows = True) Then

              While lobjBatchDataReader.Read

                'Dim lobjBatch As New Batch(lobjBatchDataReader("BatchId").ToString, lobjBatchDataReader("BatchName").ToString, lobjBatchDataReader("Description").ToString(), StringToEnum(lobjBatchDataReader("Operation").ToString(), GetType(OperationType)), lobjBatchDataReader("AssignedTo").ToString(), lobjBatchDataReader("ExportPath").ToString(), DeserializeString(lobjBatchDataReader("ItemsLocation").ToString(), ItemsLocation.GetType, New ItemsLocation()), lobjBatchDataReader("DestinationConnectionString").ToString(), lobjBatchDataReader("SourceConnectionString").ToString(), StringToEnum(lobjBatchDataReader("ContentStorageType").ToString(), GetType(Core.Content.StorageTypeEnum)), lobjBatchDataReader("DeclareAsRecordOnImport").ToString(), DeserializeString(lobjBatchDataReader("DeclareRecordConfiguration").ToString(), GetType(DeclareRecordConfiguration), New DeclareRecordConfiguration()), DeserializeString(lobjBatchDataReader("Transformations").ToString(), GetType(Transformations.TransformationCollection), New Transformations.TransformationCollection), StringToEnum(lobjBatchDataReader("DocumentFilingMode").ToString(), GetType(Core.FilingMode)), lobjBatchDataReader("LeadingDelimiter").ToString(), StringToEnum(lobjBatchDataReader("BasePathLocation").ToString(), GetType(Migrations.ePathLocation)), lobjBatchDataReader("FolderDelimiter").ToString())
                Dim lobjBatch As Batch = GetBatchFromDataReader(lpJob, lobjBatchDataReader)

                lobjBatches.Add(lobjBatch)

              End While

              lobjBatchDataReader?.Close()

              If (lobjBatchConnection.State = ConnectionState.Open) Then
                lobjBatchConnection.Close()
              End If

            End If

          Next
          'End While

        End Using

      End If

      lobjDataReader?.Close()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      lobjDataReader?.Close()

      If lobjConnection IsNot Nothing Then
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End If

    End Try

    Return lobjBatches
  End Function

  Private Function GetItemsProcessedDB() As Long
    Try
      Dim lstrItemsProcessedSql As String = String.Format("SELECT ItemsProcessed FROM {0}", TABLE_PROJECT_NAME)

      Dim lobjItemsProcessedResult As Object = ExecuteSimpleQuery(lstrItemsProcessedSql)

      If IsDBNull(lobjItemsProcessedResult) Then
        Return 0
      Else
        Return CLng(lobjItemsProcessedResult)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobByIdDB(lpJobId As String) As Job
    Try
      Dim lobjJob As Job = If(GetJobByIdDB(lpJobId, "JobId"), GetJobByIdDB(lpJobId, "JobName"))

      Return lobjJob

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobByIdDB(lpJobId As String, lpIdColumn As String) As Job
    Dim lobjJob As Job = Nothing
    Try
      Dim lstrSQL As String = String.Empty
      Dim lstrErrorMessage As String = String.Empty

      lstrSQL = String.Format("SELECT * from {0} WHERE {1} = '{2}'", TABLE_JOB_NAME, lpIdColumn, lpJobId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.Read = True) Then
              lobjJob = GetJobFromDataReader(lobjDataReader, Nothing, lstrErrorMessage)
            End If
          End Using
        End Using
      End Using

      Return lobjJob

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobConfigurationByNameDB(lpJobId As String) As String

    Dim lstrJobConfiguration As String

    Try
      lstrJobConfiguration = GetJobConfigurationByNameDB(lpJobId, "JobId")

      If String.IsNullOrEmpty(lstrJobConfiguration) Then
        ' Try to get the job using the name
        lstrJobConfiguration = GetJobConfigurationByNameDB(lpJobId, "JobName")
      End If

      Return lstrJobConfiguration

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobConfigurationByNameDB(lpJobId As String, lpIdColumn As String) As String

    Dim lstrJobConfiguration As String = String.Empty

    Try
      Dim lstrSQL As String = String.Empty

      lstrSQL = String.Format("SELECT Configuration from {0} WHERE {1} = '{2}'", TABLE_JOB_NAME, lpIdColumn, lpJobId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.Read = True) Then
              lstrJobConfiguration = Helper.GetValueFromDataRecord(lobjDataReader, "Configuration", String.Empty)
            End If
          End Using
        End Using
      End Using

      Return lstrJobConfiguration

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobIdentifiersDB() As IJobIdentifiers
    Dim lstrJobId As String
    Dim lstrJobName As String
    Dim lobjJobIdentifier As JobIdentifier
    Dim lobjJobIdentifiers As New JobIdentifiers

    Try
      Dim lstrSQL As String = String.Empty
      Dim lstrErrorMessage As String = String.Empty

      lstrSQL = String.Format("SELECT JobId, JobName FROM {0} ORDER BY JobName", TABLE_JOB_NAME)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            While lobjDataReader.Read
              lstrJobId = Helper.GetValueFromDataRecord(lobjDataReader, "JobId", String.Empty)
              lstrJobName = Helper.GetValueFromDataRecord(lobjDataReader, "JobName", String.Empty)
              lobjJobIdentifier = New JobIdentifier(lstrJobId, lstrJobName)
              lobjJobIdentifiers.Add(lobjJobIdentifier)
            End While
          End Using
        End Using
      End Using

      Return lobjJobIdentifiers

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub SaveJobConfiguration(lpJobConfiguration As Configuration.JobConfiguration)
    Try
      Dim lstrJobConfiguration As String = lpJobConfiguration.ToSQLXmlString()
      Dim lstrSqlBuilder As New StringBuilder
      Dim lintRowsAffected As Integer

      lstrSqlBuilder.AppendFormat("UPDATE {0} SET Configuration =  '{1}' WHERE JobName = '{2}'",
                                  TABLE_JOB_NAME, lstrJobConfiguration, lpJobConfiguration.Name)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSqlBuilder.ToString(), lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          lintRowsAffected = lobjCommand.ExecuteNonQuery()
        End Using
      End Using

      If lintRowsAffected = 0 Then
        Throw New JobDoesNotExistException(lpJobConfiguration.Name)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetJobInfoByBatchId(lpObjectIdentifier As ObjectIdentifier) As String
    Try
      Dim lobjSqlBuilder As New StringBuilder

      Select Case lpObjectIdentifier.IdentifierType
        Case ObjectIdentifier.IdTypeEnum.ID
          lobjSqlBuilder.Append("SELECT J.JobId ")
        Case ObjectIdentifier.IdTypeEnum.Name
          lobjSqlBuilder.Append("SELECT J.JobName ")
      End Select

      lobjSqlBuilder.Append("FROM tblBatch AS B INNER JOIN ")
      lobjSqlBuilder.Append("tblJobBatchRel AS JBR ON B.BatchId = JBR.BatchId INNER JOIN ")
      lobjSqlBuilder.Append("tblJob AS J ON JBR.JobId = J.JobId ")
      lobjSqlBuilder.AppendFormat("WHERE (B.BatchId = N'{0}')", lpObjectIdentifier.IdentifierValue)

      Return ExecuteSimpleQuery(lobjSqlBuilder.ToString)

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetJobByBatchId(ByVal lpBatchId As String) As Job
    Try
      Dim lstrJobId As String = GetJobInfoByBatchId(New ObjectIdentifier(lpBatchId, ObjectIdentifier.IdTypeEnum.ID))

      If Not String.IsNullOrEmpty(lstrJobId) Then
        Return GetJobByIdDB(lstrJobId)
      Else
        Throw _
          New ItemDoesNotExistException(lpBatchId,
                                                   String.Format("Unable to find job using batch id '{0}'.", lpBatchId))
      End If
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchByIdDB(ByVal lpBatchId As String) As Batch
    Try
      Dim lobjBatch As Batch = Nothing
      Dim lstrSQL As String = String.Format("SELECT  * from {0}  WHERE BatchId = '{1}'", TABLE_BATCH_NAME, lpBatchId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              lobjDataReader.Read()
              lobjBatch = GetBatchFromDataReader(GetJobByBatchId(lpBatchId), lobjDataReader)
            Else
              Throw New ItemDoesNotExistException(lpBatchId)
            End If
          End Using
        End Using
      End Using

      Return lobjBatch

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetItemCountDB(ByVal lpBatch As Batch) As Integer
    Try
      Dim lstrSQL As String = CreateSelectCountSQL(lpBatch)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Return lobjCommand.ExecuteScalar
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetItemCountDB(ByVal lpJob As Job) As Long
    Try
      Dim lstrSQL As String = CreateSelectCountSQL(lpJob)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Return lobjCommand.ExecuteScalar
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetItemCountDB(ByVal lpProject As Project) As Long
    Try
      Dim lstrSQL As String = CreateSelectCountSQL(lpProject)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Return lobjCommand.ExecuteScalar
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetRepositoryByConnectionStringDB(ByVal lpConnectionString As String) As Repository
    Try
      Return GetRepositoryDB(RepositoryKeyEnum.ConnectionString, lpConnectionString)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetRepositoryByNameDB(ByVal lpRepositoryName As String) As Repository
    Try
      Return GetRepositoryDB(RepositoryKeyEnum.Name, lpRepositoryName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetRepositoryDB(ByVal lpRepositoryKey As RepositoryKeyEnum, ByVal lpKey As String) As Repository
    Try
      Dim lobjRepositoryByteArray As Byte()
      'Dim lobjDataAdapter As SqlDataAdapter = Nothing
      'Dim lobjDataSet As New DataSet
      Dim lstrKey As String = Nothing

      Dim lstrSQLRequestBuilder As New StringBuilder
      Dim lobjRepository As Repository = Nothing
      Dim lstrRepositoryKeyColumn As String

      Try

        ''lobjRepositoryByteArray = Helper.CopyStreamToByteArray(lpRepository.ToArchiveStream, 512)

        Select Case lpRepositoryKey
          Case RepositoryKeyEnum.Id
            lstrRepositoryKeyColumn = "RepositoryId"

          Case RepositoryKeyEnum.Name
            lstrRepositoryKeyColumn = "RepositoryName"

          Case Else
            lstrRepositoryKeyColumn = lpRepositoryKey.ToString()

        End Select

        Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

          lstrSQLRequestBuilder.Append("SELECT Repository FROM tblRepository ")

          lstrSQLRequestBuilder.AppendFormat("WHERE {0} = '{1}'", lstrRepositoryKeyColumn, lpKey)

          ''Dim lstrSQL As String = String.Format("SELECT RepositoryId from tblRepository where Name = '{0}'", lpRepository.Name)
          ''Dim lstrUpdateSQL As String = String.Empty

          ''Dim lcmdSelect As New SqlCommand(lstrSQL, lobjConnection)
          Helper.HandleConnection(lobjConnection)

          Using lobjCommand As New SqlCommand(lstrSQLRequestBuilder.ToString, lobjConnection)
            lobjCommand.CommandTimeout = COMMAND_TIMEOUT

            lobjCommand.CommandType = CommandType.Text
            Using lobjReader As SqlDataReader = lobjCommand.ExecuteReader(CommandBehavior.CloseConnection)
              If lobjReader.HasRows Then
                lobjReader.Read()
                lobjRepositoryByteArray = lobjReader("Repository")
                Using lobjRepositoryStream As New MemoryStream(lobjRepositoryByteArray)
                  lobjRepository = New Repository(lobjRepositoryStream)
                End Using
              End If
            End Using
          End Using
          If (lobjConnection.State = ConnectionState.Open) Then
            lobjConnection.Close()
          End If
        End Using

        Return lobjRepository

      Catch ex As Exception
        ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw

      Finally

        lobjRepositoryByteArray = Nothing
        ' lobjDataSet = Nothing
        ' lobjDataAdapter = Nothing


      End Try
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function GetRepositoriesDB(ByVal lpJob As Job) As Repositories
    Try
      Return Nothing
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetRepositoriesDB(ByVal lpProject As Project) As Repositories
    Try
      Dim lobjRepositories As New Repositories
      Dim lobjRepository As Repository = Nothing

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Dim lstrSQL As String = "SELECT * FROM tblRepository"
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                lobjRepository = Nothing
                lobjRepository = GetRepositoryByNameDB(lobjDataReader("RepositoryName"))
                If lobjRepository IsNot Nothing Then
                  If Not lobjRepositories.Contains(lobjRepository.Name) Then
                    lobjRepositories.Add(lobjRepository)
                  End If
                End If
              End While
            End If
          End Using
        End Using
      End Using

      Return lobjRepositories

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function


#End Region

#Region "Save Methods"

  Private Sub InitializeCatalogDB(lpCatalogConnectionString As String, lpCreateIfNotExists As Boolean)
    Try
      InitializeCatalogDB(lpCatalogConnectionString, "MyCatalog", String.Empty, lpCreateIfNotExists)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub InitializeCatalogDB(lpCatalogConnectionString As String, lpCatalogName As String,
                                  lpCatalogDescription As String, lpCreateIfNotExists As Boolean)
    Try

      ' Make sure we were given a connection string
#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpCatalogConnectionString)
#Else
          If lpCatalogConnectionString Is Nothing Then
            Throw New ArgumentNullException(NameOf(lpCatalogConnectionString))
          End If
#End If

      Dim lstrCatalogDatabaseName As String = String.Empty
      Dim lobjParser As New SqlConnectionStringBuilder(lpCatalogConnectionString)
      lstrCatalogDatabaseName = lobjParser.InitialCatalog
      lobjParser.InitialCatalog = String.Empty

      If String.IsNullOrEmpty(lstrCatalogDatabaseName) Then
        Throw New CatalogConnectionNotConfiguredException()
      End If

      'Check if database exists, if not, create it
      If (Not DoesDBExist(lstrCatalogDatabaseName, lobjParser.ConnectionString)) Then
        If lpCreateIfNotExists Then
          CreateDatabaseAndTables(DatabaseType.Catalog)
          ProjectCatalog.CurrentConnectionString = lpCatalogConnectionString
          UtilSaveCatalog(Guid.NewGuid.ToString(), lpCatalogName, lpCatalogDescription, lpCatalogConnectionString)
        Else
          Throw _
            New InvalidConnectionStringException(
              String.Format("No database exists with the connection string '{0}'.", lpCatalogConnectionString))
        End If
      ElseIf DbHelper.GetBaseTableCount(lpCatalogConnectionString) = 0 Then
        InitializeBlankDatabase(DatabaseType.Catalog)
      Else
        ' In case this is an older catalog without this column, add it.
        AddNewColumnsToCatalogTables(lpCatalogConnectionString)
      End If

      ' Check if the catalog table exists
      If DoesTableExist(TABLE_CATALOG_NAME, DatabaseType.Catalog) = False Then
        ApplicationLogging.WriteLogEntry(
          "tblCatalog not found in the catalog database.  Re-initializing catalog tables and stored procedures.",
          Reflection.MethodBase.GetCurrentMethod, TraceEventType.Information, 65301)
        CreateTables(DatabaseType.Catalog)
      End If


      ' Check if the nodes table exists
      If DoesTableExist(TABLE_NODES_NAME, DatabaseType.Catalog) = False Then
        ApplicationLogging.WriteLogEntry(
          "tblNodes not found in the catalog database.  Re-initializing catalog tables and stored procedures.",
          Reflection.MethodBase.GetCurrentMethod, TraceEventType.Information, 65302)
        CreateTables(DatabaseType.Catalog)
      End If

      If Not IsCatalogTableInitialized(lpCatalogConnectionString) Then
        UtilSaveCatalog(Guid.NewGuid.ToString(), "MyCatalog", String.Empty, lpCatalogConnectionString)
      End If

      ' Refresh the stored procedures
      CreateStoredProcedures(DatabaseType.Catalog)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Function GetMasterConnectionString(lpDatabaseConnectionString As String) As String
    Try
      Dim lstrCatalogDatabaseName As String = String.Empty
      Dim lobjParser As New SqlConnectionStringBuilder(lpDatabaseConnectionString)
      lstrCatalogDatabaseName = lobjParser.InitialCatalog
      lobjParser.InitialCatalog = String.Empty
      Return lobjParser.ConnectionString
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub SaveProjectToDB(ByVal lpProject As Project)

    Try

      'Check if database exists, if not, create it
      If (Not DoesDBExist(Me.ItemsLocation.DatabaseName, String.Empty)) Then
        CreateDatabaseAndTables(DatabaseType.Project)

      ElseIf DbHelper.GetBaseTableCount(ItemsLocation.Location) = 0 Then
        InitializeBlankDatabase(DatabaseType.Project)
      End If

      'Save Project Related Info
      UtilSaveProject(lpProject)

      'Save Job and Batch Related Info
      SaveJobsAndBatchesToDB(lpProject.Jobs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod, lpProject)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Sub UtilSaveCatalog(lpId As String, lpName As String, lpDescription As String,
                              lpCatalogConnectionString As String)

    Try

      Using lobjConnection As New SqlConnection(lpCatalogConnectionString)

        Dim lstrSQL As String = String.Format("SELECT CatalogId from tblCatalog where CatalogId = '{0}'", lpId)
        Dim lstrUpdateSQL As String = String.Empty

        Dim cmdSelect As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if project already exists in db, if it does, do an update, else do an insert
        Dim lstrCatalogId As String = cmdSelect.ExecuteScalar()

        If (lstrCatalogId = Nothing OrElse lstrCatalogId = String.Empty OrElse IsDBNull(lstrCatalogId)) Then
          lstrUpdateSQL =
            String.Format("INSERT Into tblCatalog(CatalogId, CatalogName, CatalogDescription) VALUES('{0}','{1}','{2}')",
                          lpId, lpName.Replace("'", "''"), lpDescription.Replace("'", "''"))

        Else
          lstrUpdateSQL =
            String.Format(
              "UPDATE tblCatalog set CatalogName = '{1}', CatalogDescription = '{2}' WHERE CatalogId = '{0}'",
              lpId, lpName.Replace("'", "''"), lpDescription.Replace("'", "''"))
        End If

        Dim cmdUpdate As New SqlCommand(lstrUpdateSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintNewRecordAff As Integer = cmdUpdate.ExecuteNonQuery()

        If (lintNewRecordAff = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to insert/update item tblCatalog. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrUpdateSQL))
        End If
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub UtilSaveProject(ByVal lpProject As Project)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)


        Dim lstrSQL As String = String.Format("SELECT ProjectId from tblProject where ProjectId = '{0}'", lpProject.Id)
        Dim lstrUpdateSQL As String = String.Empty

        Dim cmdSelect As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if project already exists in db, if it does, do an update, else do an insert
        Dim lstrProjectId As String = cmdSelect.ExecuteScalar()
        Dim lblnIsNewProject As Boolean =
              (lstrProjectId = Nothing OrElse lstrProjectId = String.Empty OrElse IsDBNull(lstrProjectId))

        If lblnIsNewProject Then
          lstrUpdateSQL =
            String.Format(
              "INSERT Into tblProject(ProjectId,ProjectName,Description,BatchSize,ItemsLocation) VALUES('{0}','{1}','{2}',{3},'{4}')",
              lpProject.Id, lpProject.Name.Replace("'", "''"), lpProject.Description.Replace("'", "''"),
              lpProject.BatchSize, SerializeString(lpProject.ItemsLocation, New ItemsLocation()).Replace("'", "''"))
        Else
          lstrUpdateSQL =
            String.Format(
              "UPDATE tblProject set ProjectName = '{1}', Description = '{2}', BatchSize = {3}, ItemsLocation = '{4}' WHERE ProjectId = '{0}'",
              lpProject.Id, lpProject.Name.Replace("'", "''"), lpProject.Description.Replace("'", "''"),
              lpProject.BatchSize, SerializeString(lpProject.ItemsLocation, New ItemsLocation()).Replace("'", "''"))
        End If

        Dim cmdUpdate As New SqlCommand(lstrUpdateSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintNewRecordAff As Integer = cmdUpdate.ExecuteNonQuery()

        If (lintNewRecordAff = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to insert/update item tblProject. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrUpdateSQL))
          'Else
          '  ' Add the new project to Firebase
          '  Dim lstrPutPath As String = String.Format("{0}/catalogs/{1}", FIREBASE_APP_URL, ProjectCatalog.Instance.Id)
          '  Dim lobjProjectInfo As IProjectInfo = GetProjectInfoDB()
          '  DirectCast(lobjProjectInfo, IFirebasePusher).UpdateFirebase()
        End If
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveJobsAndBatchesToDB(ByVal lpJobs As Jobs)

    Try

      For Each lobjJob As Job In lpJobs

        'Insert/Update Job tables
        SaveJobToDB(lobjJob)

        'Insert/Update Batch tables
        SaveBatchesToDB(lobjJob.Batches)

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub SaveBatchToDB(ByVal lpBatch As Batch)
    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand("usp_save_batch", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjBatchIdParameter As New SqlParameter("@batchid", SqlDbType.NVarChar, 255) With {
            .Value = lpBatch.Id
          }
          lobjCommand.Parameters.Add(lobjBatchIdParameter)

          Dim lobjBatchNameParameter As New SqlParameter("@batchname", SqlDbType.NVarChar, 255) With {
            .Value = lpBatch.Name
          }
          lobjCommand.Parameters.Add(lobjBatchNameParameter)

          Dim lobjAssignedToParameter As New SqlParameter("@assignedto", SqlDbType.NVarChar, 255) With {
            .Value = lpBatch.AssignedTo
          }
          lobjCommand.Parameters.Add(lobjAssignedToParameter)

          Dim lobjJobIdParameter As New SqlParameter("@jobid", SqlDbType.NVarChar, 255) With {
            .Value = lpBatch.Job.Id
          }
          lobjCommand.Parameters.Add(lobjJobIdParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New Exception(String.Format("Failed to save batch '{0}'.", lpBatch.Id))
          End If
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveBatchesToDB(ByVal lpBatches As Batches)
    Try

      For Each lobjBatch As Batch In lpBatches
        SaveBatchToDB(lobjBatch)

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub SaveJobToDB(ByVal lpJob As Job)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrJobSQL As String = String.Format("SELECT JobId from tblJob where JobId = '{0}'", lpJob.Id)
        Dim lstrJobUpdateSQL As String = String.Empty

        Dim cmdJobSelect As New SqlCommand(lstrJobSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if job already exists in db, if it does, do an update, else do an insert
        Dim lstrJobId As String = cmdJobSelect.ExecuteScalar()

        'Need to figure out why this is happening
        If (lpJob.DestinationConnectionString Is Nothing) Then
          lpJob.DestinationConnectionString = String.Empty
        End If

        If (String.IsNullOrEmpty(lstrJobId) OrElse IsDBNull(lstrJobId)) Then
          lstrJobUpdateSQL = CreateInsertJobSQL(lpJob)
          ExecuteJobSql(lpJob.Id, lpJob.Project.Id, lstrJobUpdateSQL, lobjConnection, "insert")
        Else
          lstrJobUpdateSQL = CreateUpdateJobSQL(lpJob)
          ExecuteJobSql(lpJob.Id, lpJob.Project.Id, lstrJobUpdateSQL, lobjConnection, "update")
        End If

        ' Create or replace the view for the job
        CreateOrUpdateJobViewDB(lpJob.Name)

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub SaveJobToDB(ByVal lpProjectId As String, ByRef lpJobConfiguration As Configuration.JobConfiguration,
                          Optional ByRef lpJobId As String = Nothing)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_save_job", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjProjectIdParameter As New SqlParameter("@projectid", SqlDbType.NVarChar, 255) With {
            .Value = lpProjectId
          }
          lobjCommand.Parameters.Add(lobjProjectIdParameter)

          Dim lobjJobIdParameter As New SqlParameter("@jobid", SqlDbType.NVarChar, 255) With {
            .Direction = ParameterDirection.InputOutput,
            .Value = lpJobId
          }
          lobjCommand.Parameters.Add(lobjJobIdParameter)

          Dim lobjJobNameParameter As New SqlParameter("@jobname", SqlDbType.NVarChar, 255) With {
            .Value = lpJobConfiguration.Name
          }
          lobjCommand.Parameters.Add(lobjJobNameParameter)

          Dim lobjOperationNameParameter As New SqlParameter("@operationname", SqlDbType.NVarChar, 255) With {
            .Value = lpJobConfiguration.OperationName
          }
          lobjCommand.Parameters.Add(lobjOperationNameParameter)

          Dim lobjPreviousJobNameParameter As New SqlParameter("@previousjobname", SqlDbType.NVarChar, 255) With {
            .Value = lpJobConfiguration.PreviousName
          }
          lobjCommand.Parameters.Add(lobjPreviousJobNameParameter)

          Dim lobjJobConfigurationParameter As New SqlParameter("@jobconfiguration", SqlDbType.NText) With {
            .Value = lpJobConfiguration.ToSQLXmlString
          }
          lobjCommand.Parameters.Add(lobjJobConfigurationParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          'If lintReturnValue = -100 Then
          '  Throw New Exception(String.Format("Failed to save batch '{0}'.", lpBatch.Id))
          'End If

          If lobjJobIdParameter.Value <> lpJobId Then
            lpJobId = lobjJobIdParameter.Value
          End If

        End Using
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveJobRelationshipToDB(ByVal lpJobRelationship As JobRelationship)
    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand("usp_save_job_relationship", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjIdParameter As New SqlParameter("@relationshipid", SqlDbType.NVarChar, 255) With {
            .Value = lpJobRelationship.Id
          }
          lobjCommand.Parameters.Add(lobjIdParameter)

          Dim lobjNameParameter As New SqlParameter("@relationshipname", SqlDbType.NVarChar, 255) With {
            .Value = lpJobRelationship.Name
          }
          lobjCommand.Parameters.Add(lobjNameParameter)

          Dim lobjDescriptionParameter As New SqlParameter("@relationshipdescription", SqlDbType.NVarChar, 255) With {
            .Value = lpJobRelationship.Description
          }
          lobjCommand.Parameters.Add(lobjDescriptionParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New Exception(String.Format("Failed to save job relationship '{0}'.", lpJobRelationship.Id))
          End If
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

      SaveRelatedJobsToDB(lpJobRelationship)

    Catch SqlEx As SqlException
      ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod)
      If SqlEx.Message.Contains(INVALID_OBJECT_NAME) Then
        CreateTables(DatabaseType.Project)
        SaveJobRelationshipToDB(lpJobRelationship)
      Else
        '   Re-throw the exception to the caller
        Throw
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveRelatedJobsToDB(ByVal lpJobRelationship As JobRelationship)
    Try

      Dim lobjDataTable As New DataTable()
      Dim lobjDataRow As DataRow

      Try
        lobjDataTable.Columns.Add(JOB_RELATIONSHIP_ID_COLUMN, String.Empty.GetType())
        lobjDataTable.Columns.Add(JOB_ID_COLUMN, String.Empty.GetType())

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      End Try

      For Each lstrRelatedJobId As String In lpJobRelationship.RelatedJobIds
        lobjDataRow = lobjDataTable.NewRow
        lobjDataRow(JOB_RELATIONSHIP_ID_COLUMN) = lpJobRelationship.Id
        lobjDataRow(JOB_ID_COLUMN) = lstrRelatedJobId
        lobjDataTable.Rows.Add(lobjDataRow)
      Next

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_save_related_jobs", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          lobjCommand.Parameters.AddWithValue("@relationshipid", lpJobRelationship.Id)
          lobjCommand.Parameters.AddWithValue("@relatedjobs", lobjDataTable)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New Exception(String.Format("Failed to save related job '{0}'.", lpJobRelationship.Id))
          End If

        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Sub ExecuteJobSql(lpJobId As String, lpProjectId As String, lpSql As String, lpConnection As SqlConnection,
                            lpOperation As String)
    Try
      Dim lstrExistingJobId As String = Nothing
      Dim cmdJobUpdate As New SqlCommand(lpSql, lpConnection) With {
        .CommandTimeout = COMMAND_TIMEOUT
      }

      Helper.HandleConnection(lpConnection)

      Dim lintJobNewRecordAff As Integer = cmdJobUpdate.ExecuteNonQuery()

      If (lintJobNewRecordAff = 0) Then
        Throw New Exception(String.Format("{0}: Failed to {1} item tblJob. Sql: {2}",
                                          Reflection.MethodBase.GetCurrentMethod, lpOperation, lpSql))
      End If

      ''''PROJECT JOB RELATIONSHIP''''
      Dim lstrPJSQL As String =
            String.Format("SELECT JobId from tblProjectJobRel where JobId = '{0}' and ProjectId = '{1}'",
                          lpJobId, lpProjectId)
      Dim lstrPJUpdateSQL As String = String.Empty

      Dim cmdPJSelect As New SqlCommand(lstrPJSQL, lpConnection) With {
        .CommandTimeout = COMMAND_TIMEOUT
      }

      Helper.HandleConnection(lpConnection)

      'Check if relationship already exists in db, if it does, do nothing, else do an insert
      lstrExistingJobId = cmdPJSelect.ExecuteScalar()

      If (lstrExistingJobId = Nothing OrElse lstrExistingJobId = String.Empty OrElse IsDBNull(lstrExistingJobId)) Then
        lstrPJUpdateSQL = String.Format("INSERT Into tblProjectJobRel(JobId,ProjectId) VALUES('{0}','{1}')",
                                        lpJobId, lpProjectId)

        Dim cmdPJUpdate As New SqlCommand(lstrPJUpdateSQL, lpConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lpConnection)

        Dim lintPJNewRecordAff As Integer = cmdPJUpdate.ExecuteNonQuery()

        If (lintPJNewRecordAff = 0) Then
          Throw New Exception(String.Format("{0}: Failed to insert item tblProjectJobRel. Sql: {1}",
                                            Reflection.MethodBase.GetCurrentMethod, lstrPJUpdateSQL))
        End If

      End If
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Function CreateInsertBatchSQL(ByVal lpBatch As Batch) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder

      lobjSQLBuilder.AppendFormat("INSERT Into tblBatch (BatchId, BatchName) VALUES('{0}','{1}')",
                                  lpBatch.Id, lpBatch.Name.Replace("'", "''"))

      Return lobjSQLBuilder.ToString

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''Private Function CreateInsertBatchSQL(ByVal lpBatch As Batch) As String
  ''  Try

  ''    ' "INSERT Into tblBatch(BatchId,
  ''    ' BatchName,
  ''    ' Description,
  ''    ' ItemsLocation,
  ''    ' AssignedTo,
  ''    ' ExportPath,
  ''    ' Transformations,
  ''    ' Operation,
  ''    ' DestinationConnectionString,
  ''    ' SourceConnectionString,
  ''    ' ContentStorageType,
  ''    ' DeclareAsRecordOnImport,
  ''    ' DeclareRecordConfiguration,
  ''    ' DocumentFilingMode,
  ''    ' LeadingDelimiter,
  ''    ' BasePathLocation,
  ''    ' FolderDelimiter) 
  ''    ' VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')", 
  ''    ' lobjBatch.Id, 
  ''    ' lobjBatch.Name.Replace("'", "''"), 
  ''    ' lobjBatch.Description.Replace("'", "''"), 
  ''    ' SerializeString(lobjBatch.ItemsLocation, New ItemsLocation()).Replace("'", "''"), 
  ''    ' lobjBatch.AssignedTo, 
  ''    ' lobjBatch.ExportPath, 
  ''    ' SerializeString(lobjBatch.Transformations, New Transformations.TransformationCollection).Replace("'", "''"), 
  ''    ' lobjBatch.Operation, 
  ''    ' lobjBatch.DestinationConnectionString.Replace("'", "''"), 
  ''    ' lobjBatch.SourceConnectionString.Replace("'", "''"), 
  ''    ' lobjBatch.ContentStorageType, 
  ''    ' lobjBatch.DeclareAsRecordOnImport, 
  ''    ' SerializeString(lobjBatch.DeclareRecordConfiguration, New DeclareRecordConfiguration()).Replace("'", "''"), 
  ''    ' lobjBatch.DocumentFilingMode, 
  ''    ' lobjBatch.LeadingDelimiter, 
  ''    ' lobjBatch.BasePathLocation, 
  ''    ' lobjBatch.FolderDelimiter)

  ''    Dim lobjSQLBuilder As New StringBuilder

  ''    lobjSQLBuilder.Append("INSERT Into tblBatch(")
  ''    lobjSQLBuilder.Append("BatchId,")
  ''    lobjSQLBuilder.Append("BatchName,")
  ''    lobjSQLBuilder.Append("Description,")
  ''    lobjSQLBuilder.Append("ItemsLocation,")
  ''    lobjSQLBuilder.Append("AssignedTo,")
  ''    lobjSQLBuilder.Append("ExportPath,")
  ''    lobjSQLBuilder.Append("Transformations,")
  ''    lobjSQLBuilder.Append("Operation,")
  ''    lobjSQLBuilder.Append("Process,")
  ''    lobjSQLBuilder.Append("DestinationConnectionString,")
  ''    lobjSQLBuilder.Append("SourceConnectionString,")
  ''    lobjSQLBuilder.Append("ContentStorageType,")
  ''    lobjSQLBuilder.Append("DeclareAsRecordOnImport,")
  ''    lobjSQLBuilder.Append("DeclareRecordConfiguration,")
  ''    lobjSQLBuilder.Append("DocumentFilingMode,")
  ''    lobjSQLBuilder.Append("LeadingDelimiter,")
  ''    lobjSQLBuilder.Append("BasePathLocation,")
  ''    lobjSQLBuilder.Append("FolderDelimiter) VALUES")

  ''    lobjSQLBuilder.AppendFormat("('{0}',", lpBatch.Id)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.Name.Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.Description.Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpBatch.ItemsLocation, _
  ''                                                         New ItemsLocation()).Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.AssignedTo)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.ExportPath)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpBatch.Transformations, _
  ''                                                          New Transformations.TransformationCollection).Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.Operation)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpBatch.Process, _
  ''                                                               New Process).Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.DestinationConnectionString.Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.SourceConnectionString.Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.ContentStorageType)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.DeclareAsRecordOnImport)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpBatch.DeclareRecordConfiguration, _
  ''                                                         New DeclareRecordConfiguration).Replace("'", "''"))
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.DocumentFilingMode)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.LeadingDelimiter)
  ''    lobjSQLBuilder.AppendFormat("'{0}',", lpBatch.BasePathLocation)
  ''    lobjSQLBuilder.AppendFormat("'{0}')", lpBatch.FolderDelimiter)

  ''    Return lobjSQLBuilder.ToString

  ''  Catch ex As Exception
  ''    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  ''    ' Re-throw the exception to the caller
  ''    Throw
  ''  End Try
  ''End Function

  Private Shared Function CreateUpdateBatchSQL(ByVal lpBatch As Batch) As String
    Try

      ' "UPDATE tblBatch set BatchName = '{1}', 
      ' Description = '{2}', 
      ' ItemsLocation = '{3}', 
      ' AssignedTo = '{4}', 
      ' ExportPath = '{5}', 
      ' Transformations = '{6}',
      ' Operation = '{7}', 
      ' DestinationConnectionString = '{8}',
      ' SourceConnectionString = '{9}', 
      ' ContentStorageType = '{10}', 
      ' DeclareAsRecordOnImport = '{11}',
      ' DeclareRecordConfiguration = '{12}',  
      ' DocumentFilingMode = '{13}', 
      ' LeadingDelimiter = '{14}', 
      ' BasePathLocation = '{15}', 
      ' FolderDelimiter = '{16}' 
      ' WHERE BatchId = '{0}'", 
      ' lobjBatch.Id, 
      ' lobjBatch.Name.Replace("'", "''"), 
      ' lobjBatch.Description.Replace("'", "''"), 
      ' SerializeString(lobjBatch.ItemsLocation, New ItemsLocation()).Replace("'", "''"), 
      ' lobjBatch.AssignedTo, 
      ' lobjBatch.ExportPath, 
      ' SerializeString(lobjBatch.Transformations, New Transformations.TransformationCollection).Replace("'", "''"), 
      ' lobjBatch.Operation, 
      ' lobjBatch.DestinationConnectionString.Replace("'", "''"), 
      ' lobjBatch.SourceConnectionString.Replace("'", "''"), 
      ' lobjBatch.ContentStorageType, 
      ' lobjBatch.DeclareAsRecordOnImport, 
      ' SerializeString(lobjBatch.DeclareRecordConfiguration, New DeclareRecordConfiguration()).Replace("'", "''"), 
      ' lobjBatch.DocumentFilingMode, 
      ' lobjBatch.LeadingDelimiter, 
      ' lobjBatch.BasePathLocation, 
      ' lobjBatch.FolderDelimiter()

      Dim lobjSQLBuilder As New StringBuilder

      lobjSQLBuilder.AppendFormat("UPDATE tblBatch set BatchName = '{0}', ", lpBatch.Name.Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("AssignedTo = '{0}' ", lpBatch.AssignedTo)
      'lobjSQLBuilder.AppendFormat("Description = '{0}', ", lpBatch.Description.Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("ItemsLocation = '{0}', ", SerializeString(lpBatch.ItemsLocation, _
      '                                                                       New ItemsLocation).Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("AssignedTo = '{0}', ", lpBatch.AssignedTo)
      'lobjSQLBuilder.AppendFormat("ExportPath = '{0}', ", lpBatch.ExportPath)
      'lobjSQLBuilder.AppendFormat("Transformations = '{0}', ", SerializeString(lpBatch.Transformations, _
      '                                                                         New TransformationCollection).Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("Operation = '{0}', ", lpBatch.Operation)
      'lobjSQLBuilder.AppendFormat("Process = '{0}', ", SerializeString(lpBatch.Process, New Process).Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("DestinationConnectionString = '{0}', ", lpBatch.DestinationConnectionString.Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("SourceConnectionString = '{0}', ", lpBatch.SourceConnectionString.Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("ContentStorageType = '{0}', ", lpBatch.ContentStorageType)
      'lobjSQLBuilder.AppendFormat("DeclareAsRecordOnImport = '{0}', ", lpBatch.DeclareAsRecordOnImport)
      'lobjSQLBuilder.AppendFormat("DeclareRecordConfiguration = '{0}', ", SerializeString(lpBatch.DeclareRecordConfiguration, New DeclareRecordConfiguration).Replace("'", "''"))
      'lobjSQLBuilder.AppendFormat("DocumentFilingMode = '{0}', ", lpBatch.DocumentFilingMode)
      'lobjSQLBuilder.AppendFormat("LeadingDelimiter = '{0}', ", lpBatch.LeadingDelimiter)
      'lobjSQLBuilder.AppendFormat("BasePathLocation = '{0}', ", lpBatch.BasePathLocation)
      'lobjSQLBuilder.AppendFormat("FolderDelimiter = '{0}' ", lpBatch.FolderDelimiter)
      lobjSQLBuilder.AppendFormat("WHERE BatchId = '{0}'", lpBatch.Id)

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function CreateInsertJobSQL(ByVal lpJob As Job) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder

      lobjSQLBuilder.Append("INSERT Into tblJob(")
      lobjSQLBuilder.Append("JobId,")
      lobjSQLBuilder.Append("JobName,")
      lobjSQLBuilder.Append("Description,")
      lobjSQLBuilder.Append("JobSource,")
      lobjSQLBuilder.Append("BatchSize,")
      lobjSQLBuilder.Append("ItemsLocation,")
      lobjSQLBuilder.Append("Operation,")
      lobjSQLBuilder.Append("Process,")
      lobjSQLBuilder.Append("DestinationConnectionString,")
      lobjSQLBuilder.Append("ContentStorageType,")
      lobjSQLBuilder.Append("DeclareAsRecordOnImport,")
      lobjSQLBuilder.Append("DeclareRecordConfiguration,")
      lobjSQLBuilder.Append("Transformations,")
      lobjSQLBuilder.Append("DocumentFilingMode,")
      lobjSQLBuilder.Append("LeadingDelimiter,")
      lobjSQLBuilder.Append("BasePathLocation,")
      lobjSQLBuilder.Append("FolderDelimiter,")
      lobjSQLBuilder.Append("TransformationSourcePath) VALUES")

      lobjSQLBuilder.AppendFormat("('{0}',", lpJob.Id)
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.Name.Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.Description.Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpJob.Source,
                                                            New JobSource()).Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.BatchSize)
      lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpJob.ItemsLocation,
                                                            Me.ItemsLocation).Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.Operation)
      lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpJob.Process,
                                                            New Process).Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.DestinationConnectionString.Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.ContentStorageType)
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.DeclareAsRecordOnImport)
      lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpJob.DeclareRecordConfiguration,
                                                            New DeclareRecordConfiguration).Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", SerializeString(lpJob.Transformations,
                                                            New TransformationCollection).Replace("'",
                                                                                                                  "''"))
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.DocumentFilingMode)
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.LeadingDelimiter)
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.BasePathLocation)
      lobjSQLBuilder.AppendFormat("'{0}',", lpJob.FolderDelimiter)
      lobjSQLBuilder.AppendFormat("'{0}')", lpJob.TransformationSourcePath)

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub CreateOrUpdateJobViewDB(lpJobName As String)
    Try
      DropJobViewDB(lpJobName)
      ExecuteNonQuery(CreateJobViewSQL(lpJobName))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Do not re-throw this exception until we better understand that nature of the problems we will face.
    End Try
  End Sub

  Private Sub DropJobViewDB(lpJobName As String)
    Try
      ExecuteNonQuery(CreateDropJobViewSQL(lpJobName))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Do not re-throw this exception until we better understand that nature of the problems we will face.
    End Try
  End Sub

  Private Shared Function CreateJobViewSQL(lpJobName As String) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder
      Dim lstrViewName As String = GenerateJobViewName(lpJobName)

      '' Drop the view if it already exists
      'lobjSQLBuilder.AppendFormat("{0}{1}GO{1}", CreateDropJobViewSQL(lpJobName), ControlChars.CrLf)

      lobjSQLBuilder.AppendFormat("CREATE VIEW [{0}] AS {1}{1}", lstrViewName, ControlChars.CrLf)

      ' Create the view header
      ' lobjSQLBuilder.AppendFormat(" --  Job View for {0}{1}", lstrCleanJobName, ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat(" --  Job '{0}' View{1}", lpJobName, ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat(" --  Created by ECMG Content Transformation Services{0}", ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat(" --  Copyright 2009-{0}, Conteage Corp{1}", Now.Year.ToString(), ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat(" --  Last updated {0}{1}{1}",
                                  Helper.ToDetailedDateString(Now, CultureInfo.CurrentCulture), ControlChars.CrLf)

      ' Create the SQL statement
      lobjSQLBuilder.AppendFormat("SELECT BI.* {0}", ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat("FROM tblBatchItems AS BI INNER JOIN {0}", ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat("     tblJobBatchRel AS JBr ON BI.BatchId = JBr.BatchId INNER JOIN {0}",
                                  ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat("     tblJob AS J ON JBr.JobId = J.JobId {0}", ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat("WHERE (J.JobName = N'{0}')", lpJobName)

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function CreateDropJobViewSQL(lpJobName As String) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder
      Dim lstrViewName As String = GenerateJobViewName(lpJobName)

      lobjSQLBuilder.AppendFormat("IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS {0}", ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat("         WHERE TABLE_NAME = '{0}') {1}", lstrViewName, ControlChars.CrLf)
      lobjSQLBuilder.AppendFormat("DROP VIEW [{0}]", lstrViewName)

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function CreateInsertJobSQL(lpJobId As String, lpJobConfiguration As JobConfiguration) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder

      With lobjSQLBuilder
        .Append("INSERT Into tblJob(")
        .Append("JobId,")
        .Append("JobName,")
        .Append("Operation,")
        .Append("Configuration) VALUES")

        .AppendFormat("('{0}',", lpJobId)
        .AppendFormat("'{0}',", lpJobConfiguration.Name.Replace("'", "''"))
        .AppendFormat("'{0}',", lpJobConfiguration.OperationName.Replace("'", "''"))
        '.AppendFormat("'{0}')", SerializeString(lpJobConfiguration, _
        '                                       New Configuration.JobConfiguration).Replace("'", "''"))
        ' <Modified by: Ernie at 1/8/2013-3:08:01 PM on machine: ERNIE-THINK>
        ' .AppendFormat("'{0}')", lpJobConfiguration.ToXmlString)
        .AppendFormat("'{0}')", lpJobConfiguration.ToSQLXmlString)
        ' </Modified by: Ernie at 1/8/2013-3:08:01 PM on machine: ERNIE-THINK>
      End With

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function CreateUpdateJobSQL(ByVal lpJob As Job) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder

      lobjSQLBuilder.AppendFormat("UPDATE tblJob set JobName = '{0}', ", lpJob.Name.Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("Operation = '{0}', ", lpJob.Operation)
      ' <Modified by: Ernie at 1/8/2013-3:15:57 PM on machine: ERNIE-THINK>
      ' lobjSQLBuilder.AppendFormat("Configuration = '{0}' ", lpJob.Configuration.ToXmlString)
      lobjSQLBuilder.AppendFormat("Configuration = '{0}' ", lpJob.Configuration.ToSQLXmlString)
      ' </Modified by: Ernie at 1/8/2013-3:15:57 PM on machine: ERNIE-THINK>
      lobjSQLBuilder.AppendFormat("WHERE JobId = '{0}'", lpJob.Id)

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Private Function CreateUpdateJobSQL(ByVal lpJob As Job) As String
  '  Try
  '    Dim lobjSQLBuilder As New StringBuilder

  '    lobjSQLBuilder.AppendFormat("UPDATE tblJob set JobName = '{0}', ", lpJob.Name.Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("Description = '{0}', ", lpJob.Description.Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("JobSource = '{0}', ", SerializeString(lpJob.Source, New JobSource).Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("BatchSize = '{0}', ", lpJob.BatchSize)
  '    lobjSQLBuilder.AppendFormat("ItemsLocation = '{0}', ", SerializeString(lpJob.ItemsLocation, New ItemsLocation).Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("Operation = '{0}', ", lpJob.Operation)
  '    lobjSQLBuilder.AppendFormat("Process = '{0}', ", SerializeString(lpJob.Process, New Process).Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("DestinationConnectionString = '{0}', ", lpJob.DestinationConnectionString.Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("ContentStorageType = '{0}', ", lpJob.ContentStorageType)
  '    lobjSQLBuilder.AppendFormat("DeclareAsRecordOnImport = '{0}', ", lpJob.DeclareAsRecordOnImport)
  '    lobjSQLBuilder.AppendFormat("DeclareRecordConfiguration = '{0}', ", SerializeString(lpJob.DeclareRecordConfiguration, New DeclareRecordConfiguration).Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("Transformations = '{0}', ", SerializeString(lpJob.Transformations, New Transformations.TransformationCollection).Replace("'", "''"))
  '    lobjSQLBuilder.AppendFormat("DocumentFilingMode = '{0}', ", lpJob.DocumentFilingMode)
  '    lobjSQLBuilder.AppendFormat("LeadingDelimiter = '{0}', ", lpJob.LeadingDelimiter)
  '    lobjSQLBuilder.AppendFormat("BasePathLocation = '{0}', ", lpJob.BasePathLocation)
  '    lobjSQLBuilder.AppendFormat("FolderDelimiter = '{0}', ", lpJob.FolderDelimiter)
  '    lobjSQLBuilder.AppendFormat("TransformationSourcePath = '{0}' ", lpJob.TransformationSourcePath)
  '    lobjSQLBuilder.AppendFormat("WHERE JobId = '{0}'", lpJob.Id)

  '    Return lobjSQLBuilder.ToString

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Private Shared Function CreateUpdateJobSQL(lpJobId As String, lpJobConfiguration As Configuration.JobConfiguration) As String
    Try
      Dim lobjSQLBuilder As New StringBuilder

      lobjSQLBuilder.AppendFormat("UPDATE tblJob set JobName = '{0}', ", lpJobConfiguration.Name.Replace("'", "''"))
      lobjSQLBuilder.AppendFormat("Operation = '{0}', ", lpJobConfiguration.OperationName)
      ' <Modified by: Ernie at 1/8/2013-3:15:57 PM on machine: ERNIE-THINK>
      ' lobjSQLBuilder.AppendFormat("Configuration = '{0}' ", lpJob.Configuration.ToXmlString)
      lobjSQLBuilder.AppendFormat("Configuration = '{0}' ", lpJobConfiguration.ToSQLXmlString)
      ' </Modified by: Ernie at 1/8/2013-3:15:57 PM on machine: ERNIE-THINK>
      lobjSQLBuilder.AppendFormat("WHERE JobId = '{0}'", lpJobId)

      Return lobjSQLBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub SaveRepositoryToDB(ByVal lpRepository As Repository, ByVal lpJobId As String,
                                 ByVal lpJobScope As ExportScope, ByVal lpReplaceExisting As Boolean)

    Dim lobjRepositoryByteArray As Byte()
    'Dim lobjDataAdapter As SqlDataAdapter = Nothing
    'Dim lobjDataSet As New DataSet
    Dim lintReturnValue As Integer

    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpRepository)
#Else
      If lpRepository Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpRepository))
      End If
#End If

      'lobjRepositoryByteArray = Helper.CopyStreamToByteArray(lpRepository.ToArchiveStream, 512)
      lobjRepositoryByteArray = lpRepository.ToByteArray

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_save_repository", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjRepositoryIdParameter As New SqlParameter("@repositoryid", SqlDbType.NVarChar, 255) With {
            .Value = String.Empty
          }
          lobjCommand.Parameters.Add(lobjRepositoryIdParameter)

          Dim lobjRepositoryNameParameter As New SqlParameter("@repositoryname", SqlDbType.NVarChar, 255) With {
            .Value = lpRepository.Name
          }
          lobjCommand.Parameters.Add(lobjRepositoryNameParameter)

          Dim lobjJobIdParameter As New SqlParameter("@jobid", SqlDbType.NVarChar, 255) With {
            .Value = lpJobId
          }
          lobjCommand.Parameters.Add(lobjJobIdParameter)

          Dim lobjJobScopeParameter As New SqlParameter("@jobscope", SqlDbType.NVarChar, 12) With {
            .Value = lpJobScope.ToString()
          }
          lobjCommand.Parameters.Add(lobjJobScopeParameter)

          Dim lobjConnectionStringParameter As New SqlParameter("@connectionstring", SqlDbType.NVarChar, 3000) With {
            .Value = lpRepository.ConnectionString
          }
          lobjCommand.Parameters.Add(lobjConnectionStringParameter)

          Dim lobjRepositoryParameter As New SqlParameter("@repository", SqlDbType.VarBinary, lobjRepositoryByteArray.Length) With {
            .Value = lobjRepositoryByteArray
              }
          lobjCommand.Parameters.Add(lobjRepositoryParameter)

          Dim lobjReplaceParameter As New SqlParameter("@replaceexisting", SqlDbType.Bit)
          If lpReplaceExisting = True Then
            lobjReplaceParameter.Value = 1
          Else
            lobjReplaceParameter.Value = 0
          End If
          lobjCommand.Parameters.Add(lobjReplaceParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value

        End Using
        If ((lobjConnection IsNot Nothing) AndAlso (lobjConnection.State = ConnectionState.Open)) Then
          lobjConnection.Close()
        End If

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

    End Try
  End Sub

  ''' <summary>
  ''' Updates the Job Source
  ''' </summary>
  ''' <param name="lpJob"></param>
  ''' <remarks>Typically called when a search has been updated</remarks>
  Private Sub UpdateJobSourceDB(ByVal lpJob As Job)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrJobSQL As String = String.Format("SELECT JobId from tblJob where JobId = '{0}'", lpJob.Id)
        Dim lstrJobUpdateSQL As String = String.Empty

        Dim cmdJobSelect As New SqlCommand(lstrJobSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if job already exists in db, if it does, do an update, else throw exception
        Dim lstrJobId As String = cmdJobSelect.ExecuteScalar()

        If (lstrJobId = Nothing OrElse lstrJobId = String.Empty OrElse IsDBNull(lstrJobId)) Then
          Throw New Exception(String.Format("Job ({0}) does not exist, cannot update job source.", lpJob.Name))

        Else
          lstrJobUpdateSQL = String.Format("UPDATE tblJob set JobSource = '{0}' WHERE JobId = '{1}'",
                                           SerializeString(lpJob.Source, New JobSource).Replace("'", "''"), lpJob.Id)
        End If

        Dim cmdJobUpdate As New SqlCommand(lstrJobUpdateSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintJobNewRecordAff As Integer = cmdJobUpdate.ExecuteNonQuery()

        If (lintJobNewRecordAff = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to update item tblJob. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrJobUpdateSQL))
        End If
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub UpdateJobConfigurationDB(ByVal lpJob As Job)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrJobSQL As String = String.Format("SELECT JobId from tblJob where JobId = '{0}'", lpJob.Id)
        Dim lstrUpdateJobSQL As String = String.Empty
        Dim lstrUpdateBatchSQL As String = String.Empty
        Dim lstrConfigurationString As String = String.Empty
        Dim lstrProcessString As String = String.Empty
        Dim lintNewRecordsAffected As Integer

        Dim cmdJobSelect As New SqlCommand(lstrJobSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if job already exists in db, if it does, do an update, else throw exception
        Dim lstrJobId As String = cmdJobSelect.ExecuteScalar()

        ' Update the job
        If (lstrJobId = Nothing OrElse lstrJobId = String.Empty OrElse IsDBNull(lstrJobId)) Then
          Throw New Exception(String.Format("Job ({0}) does not exist, cannot update.", lpJob.Name))

        Else
          lstrConfigurationString = lpJob.Configuration.ToXmlString()
          lstrProcessString = SerializeString(lpJob.Process, New Process) '.Replace("'", "''")
          lstrUpdateJobSQL =
            String.Format("UPDATE tblJob set Configuration = '{0}', Operation = '{2}' WHERE JobId = '{1}'",
                          lstrProcessString, lpJob.Id, lpJob.Process.Name)
        End If

        Dim cmdJobUpdate As New SqlCommand(lstrUpdateJobSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        lintNewRecordsAffected = cmdJobUpdate.ExecuteNonQuery()

        If (lintNewRecordsAffected = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to update item tblJob. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrUpdateJobSQL))
        End If

        ' Update the batches
        For Each lobjBatch As Batch In lpJob.Batches
          lstrUpdateBatchSQL =
            String.Format("UPDATE tblBatch SET Process = '{0}', Operation = '{2}' WHERE BatchId = '{1}'",
                          lstrProcessString, lobjBatch.Id, lpJob.Process.Name)

          Dim cmdBatchUpdate As New SqlCommand(lstrUpdateBatchSQL, lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
          }

          Helper.HandleConnection(lobjConnection)

          lintNewRecordsAffected = cmdBatchUpdate.ExecuteNonQuery()

          If (lintNewRecordsAffected = 0) Then
            Throw _
              New Exception(String.Format("{0}: Failed to update item tblBatch. Sql: {1}",
                                          Reflection.MethodBase.GetCurrentMethod, lstrUpdateBatchSQL))
          End If
        Next

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Updates the Job transformation
  ''' </summary>
  ''' <param name="lpJob"></param>
  ''' <remarks></remarks>
  Private Sub UpdateTransformationsDB(ByVal lpJob As Job)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrJobSQL As String = String.Format("SELECT JobId from tblJob where JobId = '{0}'", lpJob.Id)
        Dim lstrJobUpdateSQL As String = String.Empty

        Dim cmdJobSelect As New SqlCommand(lstrJobSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if job already exists in db, if it does, do an update, else throw exception
        Dim lstrJobId As String = cmdJobSelect.ExecuteScalar()

        If (lstrJobId = Nothing OrElse lstrJobId = String.Empty OrElse IsDBNull(lstrJobId)) Then
          Throw New Exception(String.Format("Job ({0}) does not exist, cannot update job source.", lpJob.Name))

        Else
          lstrJobUpdateSQL = String.Format("UPDATE tblJob set Transformations = '{0}' WHERE JobId = '{1}'",
                                           SerializeString(lpJob.Transformations,
                                                           New TransformationCollection).Replace("'",
                                                                                                                 "''"),
                                           lpJob.Id)
        End If

        Dim cmdJobUpdate As New SqlCommand(lstrJobUpdateSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintJobNewRecordAff As Integer = cmdJobUpdate.ExecuteNonQuery()

        If (lintJobNewRecordAff = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to update item tblJob. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrJobUpdateSQL))
        End If

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub UpdateTransformationsDB(ByVal lpBatch As Batch)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrBatchSQL As String = String.Format("SELECT BatchId from tblBatch where BatchId = '{0}'", lpBatch.Id)
        Dim lstrBatchUpdateSQL As String = String.Empty

        Dim cmdJobSelect As New SqlCommand(lstrBatchSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        'Check if batch already exists in db, if it does, do an update, else throw exception
        Dim lstrBatchId As String = cmdJobSelect.ExecuteScalar()

        If (lstrBatchId = Nothing OrElse lstrBatchId = String.Empty OrElse IsDBNull(lstrBatchId)) Then
          Throw New Exception(String.Format("Batch ({0}) does not exist, cannot update batch source.", lpBatch.Name))

        Else
          lstrBatchUpdateSQL = String.Format("UPDATE tblBatch set Transformations = '{0}' WHERE BatchId = '{1}'",
                                             SerializeString(lpBatch.Transformations,
                                                             New TransformationCollection).Replace("'",
                                                                                                                   "''"),
                                             lpBatch.Id)
        End If

        Dim cmdBatchUpdate As New SqlCommand(lstrBatchUpdateSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintBatchNewRecordAff As Integer = cmdBatchUpdate.ExecuteNonQuery()

        If (lintBatchNewRecordAff = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to update item tblBatch. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrBatchUpdateSQL))
        End If

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Sub UpdateRepositoriesDB(ByVal lpJob As Job)
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub UpdateRepositoriesDB(ByVal lpProject As Project)
    Try
      'If lpProject.ProjectConnections IsNot Nothing Then
      '  For Each lobjRepository As Repository In lpProject.ProjectConnections.RepositoryDictionary.Values
      '    SaveRepositoryToDB(lobjRepository)
      '  Next
      'End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Delete Methods"

  ''' <summary>
  ''' Removes items from tblBatch, tblJobBatchRel, tblBatchLock and tblBatchItems
  ''' </summary>
  ''' <param name="lpBatches"></param>
  ''' <remarks></remarks>
  Private Sub DeleteBatchesDB(ByVal lpBatches As Batches)

    Try

      For Each lobjBatch As Batch In lpBatches
        Try
          DeleteBatchDB(lobjBatch.Id)
        Catch MissingItemEx As ItemDoesNotExistException
          Continue For
        Catch ex As Exception
          ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          Continue For
        End Try
      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
    End Try
  End Sub

  Private Sub CreateListBatchesDB(lpArgs As IDBLookupSourceEventArgs)

    Dim lstrDocumentTitle As String
    Dim lstrId As String

    Try

      Dim lobjItems As New Dictionary(Of String, String)
      Dim lintRowCounter As Integer
      Dim lintTotalRowCount As Integer
      Dim lstrErrorMessage As String = String.Empty
      Dim lstrAddResult As String
      Dim lobjBatchItemEventArgs As BatchItemCreatedEventArgs


      'Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
      Using lobjConnection As New OleDbConnection(lpArgs.SourceConnectionString)
        'Using lobjConnection As New SqlConnection(lpArgs.NativeSourceConnectionString)
        lobjConnection.Open()

        ' We need to get the total number of expected records
        Dim lstrRowCountSQL As String = Helper.ConvertSelectSQLToSelectCount(lpArgs.SourceSQLStatement)

        If Not String.IsNullOrEmpty(lstrErrorMessage) Then
          Throw _
            New Exception(String.Format("Unable to build total record count SQL for DB list: '{0}'", lstrErrorMessage))
        ElseIf String.IsNullOrEmpty(lstrRowCountSQL) Then
          Throw New Exception("Unable to build total record count SQL for DB list")
        End If

        Using lobjCommand As New OleDbCommand(lstrRowCountSQL, lobjConnection)
          'Using lobjCommand As New SqlCommand(lstrRowCountSQL, lobjConnection)
          lintTotalRowCount = lobjCommand.ExecuteScalar()
        End Using

        If lintTotalRowCount = 0 Then
          ApplicationLogging.WriteLogEntry(String.Format("No items found using query '{0}'.", lstrRowCountSQL),
                                           MethodBase.GetCurrentMethod(), TraceEventType.Warning, 52404)
        End If

        lobjBatchItemEventArgs = New BatchItemCreatedEventArgs(lpArgs.TargetJob.Name, 0, lintTotalRowCount)

        Using lobjCommand As New OleDbCommand(lpArgs.SourceSQLStatement, lobjConnection)
          'Using lobjCommand As New SqlCommand(lpArgs.SourceSQLStatement, lobjConnection)
          Using lobjDataReader As OleDbDataReader = lobjCommand.ExecuteReader
            'Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If lobjDataReader.HasRows Then
              Do Until lobjDataReader.Read() = False
                If ((lintRowCounter > 0) AndAlso (lintRowCounter Mod 10000 = 0)) Then
                  ' Add the items
                  ' Debug.WriteLine(String.Format("Writing up to {0}", lintRowCounter))
                  'lobjDataReader.
                  lobjBatchItemEventArgs.CurrentCount = lintRowCounter
                  lobjBatchItemEventArgs.Message = String.Format("Writing up to {0}", lintRowCounter)
                  lpArgs.TargetJob.OnBatchCreationUpdate(Me, lobjBatchItemEventArgs)

                  lstrAddResult = lpArgs.TargetJob.AddItems(lobjItems)
                  Debug.WriteLine(lstrAddResult)
                  lobjItems.Clear()
                  GC.Collect()
                End If
                If Not IsDBNull(lobjDataReader(0)) AndAlso Not String.IsNullOrEmpty(lobjDataReader(0).ToString()) Then
                  lstrDocumentTitle = lobjDataReader(0).ToString
                  ' lobjId = lobjDataReader(1)
                  lstrId = lstrDocumentTitle
                  ' Debug.WriteLine(String.Format("Reading row {0}: {1}", lintRowCounter.ToString("#,###"), lstrDocumentTitle))
                  lobjBatchItemEventArgs.CurrentCount = lintRowCounter
                  lobjBatchItemEventArgs.Message = String.Format("Reading row {0}: {1}",
                                                                 lintRowCounter.ToString("#,###"), lstrDocumentTitle)
                  lpArgs.TargetJob.OnBatchCreationUpdate(Me, lobjBatchItemEventArgs)
                  If (lobjItems.ContainsKey(lstrId)) Then
                    ApplicationLogging.WriteLogEntry(String.Format("Duplicate key found: {0}, skipping row.", lstrId),
                                                     Reflection.MethodBase.GetCurrentMethod, TraceEventType.Warning,
                                                     88769)
                  Else
                    lobjItems.Add(lstrId, lstrDocumentTitle)
                  End If

                  lintRowCounter += 1
                Else
                  Debug.Print("Skipping Row")
                End If
              Loop
              ' Debug.WriteLine(String.Format("Writing last {0}", lobjItems.Count))
              lobjBatchItemEventArgs.CurrentCount = lintRowCounter
              lobjBatchItemEventArgs.Message = String.Format("Writing last {0}", lobjItems.Count)
              lpArgs.TargetJob.OnBatchCreationUpdate(Me, lobjBatchItemEventArgs)

              If lobjItems.Count > 0 Then
                lstrAddResult = lpArgs.TargetJob.AddItems(lobjItems)
              End If
            Else
              If lobjItems.Count > 0 Then
                lstrAddResult = lpArgs.TargetJob.AddItems(lobjItems)
              End If
            End If
          End Using
        End Using
      End Using

      'RaiseEvent BatchItemsCreated(Me.Name, mintTotalBatchCount, lintRowCounter)
      lpArgs.TargetJob.OnBatchItemsCreated(Me,
                                           New BatchItemCreatedEventArgs(lpArgs.TargetJob.Name,
                                                                         lpArgs.TargetJob.TotalBatchCount,
                                                                         lintRowCounter))

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub DeleteBatchDB(lpBatchId As String)
    Try
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_delete_batch", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Dim lobjBatchIdParameter As New SqlParameter("@batchid", SqlDbType.NVarChar, 255) With {
            .Value = lpBatchId
          }
          lobjCommand.Parameters.Add(lobjBatchIdParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New ItemDoesNotExistException(lpBatchId)
          End If
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Removes a Job including all of it's batches
  ''' </summary>
  ''' <param name="lpJob"></param>
  ''' <remarks></remarks>
  Private Sub DeleteJobDB(ByVal lpJob As Job)

    Try

      If String.IsNullOrEmpty(lpJob.Id) Then
        Throw New ArgumentOutOfRangeException(NameOf(lpJob), "Job Id is null.")
      End If

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_delete_job", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Dim lobjBatchIdParameter As New SqlParameter("@jobid", SqlDbType.NVarChar, 255) With {
            .Value = lpJob.Id
          }
          lobjCommand.Parameters.Add(lobjBatchIdParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New ItemDoesNotExistException(lpJob.Id)
          End If
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
    End Try
  End Sub

  Private Sub DeleteProjectDB(ByVal lpProject As Project)

    Try

      For Each lobjJob As Job In lpProject.Jobs

        'Delete all batches related to this job
        DeleteBatchesDB(lobjJob.Batches)

        'Delete the Job related entries
        DeleteJobDB(lobjJob)

      Next

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrSQL As String = String.Empty
        Dim cmdDelete As SqlCommand
        Dim lintNewRecordAff As Integer

        lstrSQL = String.Format("Delete FROM {0} WHERE ProjectId = '{1}'", TABLE_PROJECTJOBREL_NAME, lpProject.Id)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0} WHERE ProjectId = '{1}'", TABLE_PROJECT_NAME, lpProject.Id)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try
  End Sub

  'Private Sub DeleteRepositoryDB(ByVal lpRepository As Repository)
  '  Try

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  'Private Sub DeleteRepositoryDB(ByVal lpRepositoryName As String)
  '  Try

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub


#End Region

  Private Sub InitializeJobMonitor()
    Try

      mobjChangeListener = New DatabaseChangeListener(Me.ItemsLocation.Location)
      AddHandler mobjChangeListener.OnChange, AddressOf OnNewMessage
      mobjChangeListener.Start("SELECT JobId, JobName, WorkSummary FROM tblJob")
      'mobjChangeListener.Start("UPDATE tblJob")
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub OnNewMessage()
    Try
      Beep()
      mobjChangeListener.Start("SELECT JobId, JobName, WorkSummary FROM tblJob")
      'mobjChangeListener.Start("UPDATE tblJob")
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' If the batch is NOT locked and there is at least one item 'NotProcessed' return true
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function IsAvailableForProcessingDB(ByVal lpBatchId As String) As Boolean

    Try

      'Check if already locked, if so then throw exception
      Dim lstrSQL As String = String.Format("SELECT ID from {0} (nolock) WHERE BatchID = '{1}' AND IsLocked = '1'",
                                            TABLE_BATCHLOCK_NAME, lpBatchId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Dim lstrId As String = lobjCommand.ExecuteScalar()

          If (lstrId <> String.Empty) Then
            Return False
          End If

          lstrSQL = String.Format("SELECT TOP 1 ID from {0} (nolock) where ProcessedStatus = '{1}' and BatchId = '{2}'",
                                  TABLE_BATCH_ITEM_NAME, ProcessedStatus.NotProcessed.ToString, lpBatchId)
          Using lobjIdCommand As New SqlCommand(lstrSQL, lobjConnection)
            lobjIdCommand.CommandTimeout = COMMAND_TIMEOUT

            Helper.HandleConnection(lobjConnection)
            Using lobjDataReader As SqlDataReader = lobjIdCommand.ExecuteReader

              Return lobjDataReader.HasRows
            End Using
          End Using

        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Return False

    Finally

    End Try
  End Function

  Private Sub AddItemToDB(ByVal lpBatchItem As BatchItem)

    Try

      Dim lstrSql As String =
            String.Format(
              "INSERT INTO {5}(BatchId,Title,SourceDocId,ProcessedStatus,Operation)" &
              "Values('{0}','{1}','{2}','{3}','{4}')", lpBatchItem.BatchId, lpBatchItem.Title.Replace("'", "''"),
              lpBatchItem.SourceDocId, ProcessedStatus.NotProcessed.ToString, lpBatchItem.Operation.ToString,
              TABLE_BATCH_ITEM_NAME)


      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Dim cmdAdd As New SqlCommand(lstrSql, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintNewRecordAff As Integer = cmdAdd.ExecuteNonQuery()

        If (lintNewRecordAff = 0) Then
          Throw _
            New Exception(String.Format("{0}: Failed to add item to the batch tblBatchItems. Sql: {1}",
                                        Reflection.MethodBase.GetCurrentMethod, lstrSql))
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Function GetItemsByIdDB(ByVal lpBatch As Batch,
                                  ByVal lpIdArrayList As ArrayList,
                                  ByVal lpIncludeProcessResults As Boolean) As BatchItems

    Dim lobjBatches As New BatchItems()

    Try

      If (lpIdArrayList.Count = 0) Then
        Return Nothing
      End If

      Dim lstrInList As String = String.Empty

      For i As Integer = 0 To lpIdArrayList.Count - 1
        lstrInList &= lpIdArrayList(i).ToString & ","
      Next

      If (lstrInList.EndsWith(","c)) Then
        lstrInList = lstrInList.TrimEnd(",")
      End If

      'Dim lstrSql As String = String.Format("SELECT ID,BatchID,Title ,SourceDocID,DestDocId,Operation FROM {0} (nolock) WHERE Id in ({1}) order by Id", TABLE_BATCH_ITEM_NAME, lstrInList)
      Dim lstrSQL As String = CreateBaseSelectItemsSQL(lpIncludeProcessResults)
      lstrSQL = String.Format("{0} WHERE Id in ({1}) order by Id", lstrSQL, lstrInList)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              Dim lobjBatchItem As BatchItem = Nothing
              'Dim lenuOperationType As OperationType

              While lobjDataReader.Read

                'If we have an issue with one particular item, log it and move on to the next one
                Try

                  lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, lpIncludeProcessResults)
                  lobjBatches.Add(lobjBatchItem)

                Catch ex As Exception
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                End Try

              End While

            End If

          End Using

        End Using

      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

    End Try

    Return lobjBatches
  End Function

  Private Shared Function GetItemsByIdDB(lpJob As Job, lpIdTable As DataTable) As BatchItems

    Dim lobjBatches As New BatchItems()
    Dim lstrSQLBuilder As New StringBuilder

    Try

      If (lpIdTable.Rows.Count = 0) Then
        Return Nothing
      End If

      '	SELECT tblBatchItems.*
      '	FROM tblBatchItems INNER JOIN
      '	tblJobBatchRel ON tblBatchItems.BatchId = tblJobBatchRel.BatchId INNER JOIN
      '	tblJob ON tblJobBatchRel.JobId = tblJob.JobId
      '	WHERE (tblJob.JobName = N'Migrate all SAP Docs from Prod to Dev')

      '	Use CreateBaseSelectItemsSQL for the start of the statement

      '	Use a table valued property query to filter the results from the batch item table 
      '	similiar to the tecniques shown in http://www.sommarskog.se/arrays-in-sql-2008.html

      lstrSQLBuilder.Append(CreateBaseSelectItemsSQL(False))
      lstrSQLBuilder.Append(" INNER JOIN tblJobBatchRel ON")
      lstrSQLBuilder.Append(" tblBatchItems.BatchId = tblJobBatchRel.BatchId")
      lstrSQLBuilder.Append(" INNER JOIN tblJob ON tblJobBatchRel.JobId = tblJob.JobId")
      lstrSQLBuilder.AppendFormat("	WHERE (tblJob.JobName = N'{0}'", lpJob.Name)

      Throw New NotImplementedException

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchItemByIdDB(lpId As Integer) As IBatchItem
    Try
      Dim lobjBatchItem As BatchItem = Nothing
      Dim lobjSqlBuilder As New StringBuilder

      With lobjSqlBuilder
        .Append("SELECT BI.* ")
        .Append("FROM tblBatchItems AS BI ")
        .AppendFormat("WHERE BI.ID = {0}", lpId)
      End With

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lobjSqlBuilder.ToString, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then
              lobjDataReader.Read()
              Dim lstrBatchId As String = lobjDataReader("BatchId")
              If String.IsNullOrEmpty(lstrBatchId) Then
                Throw _
                  New PropertyDoesNotExistException("Could not get BatchId from query result.", "BatchId")
              End If
              Dim lobjBatch As Batch = GetBatchByIdDB(lstrBatchId)
              lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lobjBatch, True)
              Return lobjBatchItem
            Else
              Dim lobjExMessageBuilder As New StringBuilder
              lobjExMessageBuilder.AppendFormat("No batch item could be found with id '{0}'", lpId)

              Throw New ItemDoesNotExistException(lpId, lobjExMessageBuilder.ToString)
            End If
          End Using
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetItemByIdDB(lpJobName As String, lpId As String, lpScope As OperationScope) As IBatchItem
    Try
      If lpScope = ExportScope.Both Then
        Throw New ArgumentOutOfRangeException(NameOf(lpScope), "Scope must be source or destination.")
      End If

      Dim lobjBatchItem As BatchItem = Nothing
      Dim lobjSqlBuilder As New StringBuilder

      With lobjSqlBuilder
        .Append("SELECT BI.* ")
        .Append("FROM tblJob AS J INNER JOIN ")
        .Append("tblJobBatchRel AS JBR ON J.JobId = JBR.JobId INNER JOIN ")
        .Append("tblBatchItems AS BI ON JBR.BatchId = BI.BatchId ")
        .AppendFormat("WHERE (J.JobName = N'{0}' ", lpJobName)
        If lpScope = ExportScope.Source Then
          .AppendFormat("AND BI.SourceDocId = '{0}')", lpId)
        ElseIf lpScope = ExportScope.Destination Then
          .AppendFormat("AND BI.DestDocId = '{0}')", lpId)
        End If
      End With

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lobjSqlBuilder.ToString, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then
              lobjDataReader.Read()
              Dim lstrBatchId As String = lobjDataReader("BatchId")
              If String.IsNullOrEmpty(lstrBatchId) Then
                Throw _
                  New PropertyDoesNotExistException("Could not get BatchId from query result.", "BatchId")
              End If
              Dim lobjBatch As Batch = GetBatchByIdDB(lstrBatchId)
              lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lobjBatch, True)
              Return lobjBatchItem
            Else
              Dim lstrExIdentifier As String = String.Format("Job ({0}): DocId ({1})", lpJobName, lpId)
              Dim lobjExMessageBuilder As New StringBuilder
              lobjExMessageBuilder.AppendFormat("No batch item could be found with job name '{0}'", lpJobName)
              Select Case lpScope
                Case ExportScope.Source
                  lobjExMessageBuilder.AppendFormat(" and SourceDocId '{0}'.", lpId)
                Case ExportScope.Destination
                  lobjExMessageBuilder.AppendFormat(" and DestDocId '{0}'.", lpId)
              End Select
              Throw New ItemDoesNotExistException(lstrExIdentifier, lobjExMessageBuilder.ToString)
            End If
          End Using
        End Using
      End Using

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetAllUnprocessedItemsFromDB(ByVal lpBatch As Batch) As BatchItems

    Dim lobjBatches As New BatchItems()

    Try

      'Dim lstrSql As String = String.Format("SELECT ID,BatchID,Title ,SourceDocID,DestDocId,Operation FROM {0} (nolock) WHERE ProcessedStatus = '{1}' AND BatchId = '{2}' order by Title", TABLE_BATCH_ITEM_NAME, ProcessedStatus.NotProcessed, lpBatch.Id)
      Dim lstrSQL As String = CreateBaseSelectItemsSQL(False)
      lstrSQL = String.Format("{0} WHERE ProcessedStatus = '{1}' AND BatchId = '{2}' order by Title", lstrSQL,
                              ProcessedStatus.NotProcessed, lpBatch.Id)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              Dim lobjBatchItem As BatchItem = Nothing
              'Dim lenuOperationType As OperationType

              While lobjDataReader.Read

                'If we have an issue with one particular item, log it and move on to the next one
                Try


                  lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, False)
                  lobjBatches.Add(lobjBatchItem)

                Catch ex As Exception
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                End Try

              End While

            End If
          End Using
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

    End Try

    Return lobjBatches
  End Function

  Private Function GetItemsFromDB(ByVal lpBatch As Batch,
                                  ByVal lpStart As Integer,
                                  ByVal lpItemsToGet As Integer,
                                  ByVal lpSortColumn As String,
                                  ByVal lpAscending As Boolean,
                                  ByVal lpProcessedStatusFilter As String) As BatchItems

    Dim lobjBatches As New BatchItems()

    Try

      Dim lstrSql As String = String.Empty

      If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
        'lstrSql = String.Format("SET ROWCOUNT {2};SELECT * FROM {0} (nolock) WHERE BatchId = '{1}' and ID >= {3} order by ID", TABLE_BATCH_ITEM_NAME, lpBatch.Id, lpItemsToGet, lpStart)
        lstrSql = String.Format("SET ROWCOUNT {2};{0} WHERE BatchId = '{1}' and ID >= {3} order by ID",
                                CreateBaseSelectItemsSQL(False), lpBatch.Id, lpItemsToGet, lpStart)

      Else
        lstrSql =
          String.Format(
            "SET ROWCOUNT {2};{0} WHERE BatchId = '{1}' and ID >= {3} and ProcessedStatus = '{4}' order by ID",
            CreateBaseSelectItemsSQL(False), lpBatch.Id, lpItemsToGet, lpStart, lpProcessedStatusFilter)
      End If

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSql, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              Dim lobjBatchItem As BatchItem = Nothing
              'Dim lenuOperationType As OperationType

              While lobjDataReader.Read

                'If we have an issue with one particular item, log it and move on to the next one
                Try

                  'lenuOperationType = StringToEnum(lobjDataReader("Operation").ToString, GetType(OperationType))

                  'lobjBatchItem = BatchItem.CreateBatchItem(lenuOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)

                  'lobjBatchItem.BatchId = lobjDataReader("BatchID").ToString
                  'lobjBatchItem.Id = lobjDataReader("ID").ToString
                  'lobjBatchItem.Operation = lenuOperationType

                  lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, True)
                  lobjBatches.Add(lobjBatchItem)

                Catch ex As Exception
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                End Try

              End While

            End If

            lobjDataReader.Close()
          End Using
        End Using
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

    End Try

    Return lobjBatches
  End Function

  Private Function GetAllItemsFromDB(ByVal lpBatch As Batch,
                                     ByVal lpProcessedStatusFilter As String) As BatchItems

    Dim lobjBatches As New BatchItems()

    Try

      Dim lstrSql As String = String.Empty

      If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
        lstrSql = String.Format("{0} WHERE BatchId = '{1}' ", CreateBaseSelectItemsSQL(True), lpBatch.Id)

      Else
        lstrSql = String.Format("{0} WHERE BatchId = '{1}' AND ProcessedStatus = '{2}' ", CreateBaseSelectItemsSQL(True),
                                lpBatch.Id, lpProcessedStatusFilter)
      End If

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSql, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              Dim lobjBatchItem As BatchItem = Nothing

              While lobjDataReader.Read

                'If we have an issue with one particular item, log it and move on to the next one
                Try

                  lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, True)
                  lobjBatches.Add(lobjBatchItem)

                Catch ex As Exception
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                End Try

              End While

            End If

            lobjDataReader.Close()

          End Using

        End Using

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try

    Return lobjBatches
  End Function

  Private Function GetItemsFromDBToDataTable(ByVal lpBatch As Batch,
                                             ByVal lpStart As Integer,
                                             ByVal lpItemsToGet As Integer,
                                             ByVal lpSortColumn As String,
                                             ByVal lpAscending As Boolean,
                                             ByVal lpProcessedStatusFilter As String) As DataTable

    Dim lobjDataTable As New DataTable()

    Try

      Dim lstrSql As String = String.Empty

      If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
        lstrSql =
          String.Format("SET ROWCOUNT {2};SELECT * FROM {0} (nolock) WHERE BatchId = '{1}' and ID >= {3} order by ID",
                        TABLE_BATCH_ITEM_NAME, lpBatch.Id, lpItemsToGet, lpStart)

      Else
        lstrSql =
          String.Format(
            "SET ROWCOUNT {2};SELECT * FROM {0} (nolock) WHERE BatchId = '{1}' and ID >= {3} and ProcessedStatus = '{4}' order by ID",
            TABLE_BATCH_ITEM_NAME, lpBatch.Id, lpItemsToGet, lpStart, lpProcessedStatusFilter)
      End If

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjDA As New SqlDataAdapter()
          lobjDA.SelectCommand = New SqlCommand(lstrSql, lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
          }

          Helper.HandleConnection(lobjConnection)
          lobjDA.Fill(lobjDataTable)

        End Using

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

    End Try

    Return lobjDataTable
  End Function

  Private Shared Function CreateBaseSelectCountSQL(ByVal lpItemScope As ItemScope) As String
    Try
      Dim lobjSqlBuilder As New StringBuilder
      Dim lstrSourceTable As String = String.Empty

      lobjSqlBuilder.AppendFormat("SELECT COUNT(ID) FROM {0} (nolock)", TABLE_BATCH_ITEM_NAME)

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function CreateBaseSelectItemsSQL(ByVal lpIncludeProcessResult As Boolean) As String

    Dim lobjSqlBuilder As New StringBuilder

    Try

      If lpIncludeProcessResult Then
        lobjSqlBuilder.Append(
          "SELECT ID, BatchId, Title, SourceDocId, DestDocId, ProcessedStatus, ProcessResult, Operation, ")
      Else
        lobjSqlBuilder.Append("SELECT ID, BatchId, Title, SourceDocId, DestDocId, ProcessedStatus, Operation, ")
      End If
      lobjSqlBuilder.Append(
        "ProcessedMessage, ProcessStartTime, ProcessFinishTime, TotalProcessingTime, ProcessedBy, CreateDate")
      lobjSqlBuilder.AppendFormat(" FROM {0} (nolock)", TABLE_BATCH_ITEM_NAME)

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function CreateSelectCountSQL(ByVal lpObject As Object) As String
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpObject)
#Else
      If lpObject Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpObject))
      End If
#End If

      Dim lstrSql As String = String.Empty

      If TypeOf lpObject Is Batch Then
        Dim lobjBatch As Batch = lpObject
        lstrSql = String.Format("{0} WHERE BatchId = '{1}'", CreateBaseSelectCountSQL(ItemScope.Batch), lobjBatch.Id)


      ElseIf TypeOf lpObject Is Job Then
        Dim lobjJob As Job = lpObject
        lstrSql = String.Format("{0} WHERE BatchId in (select batchid from tblJobBatchRel where JobId = '{1}')",
                                CreateBaseSelectCountSQL(ItemScope.Job), lobjJob.Id)

      ElseIf TypeOf lpObject Is Project Then
        lstrSql = "SELECT COUNT(*) AS TotalItemCount FROM tblBatchItems"

      Else
        Throw New Exception(String.Format("Can't get count, object type '{0}' not supported.", lpObject.GetType.Name))

      End If

      Return lstrSql

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function CreateSelectItemsSQL(ByVal lpObject As Object,
                                        ByVal lpProcessedStatusFilter As String) As String
    Try

      Dim lstrSql As String = String.Empty

      If TypeOf lpObject Is Batch Then
        Dim lobjBatch As Batch = lpObject

        If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
          lstrSql = String.Format("{0} WHERE BatchId = '{1}'", CreateBaseSelectItemsSQL(False), lobjBatch.Id)

        Else
          lstrSql = String.Format("{0} WHERE BatchId = '{1}' and ProcessedStatus = '{2}' ",
                                  CreateBaseSelectItemsSQL(False), lobjBatch.Id, lpProcessedStatusFilter)
        End If

      ElseIf TypeOf lpObject Is Job Then

        Dim lobjJob As Job = lpObject

        If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
          lstrSql =
            String.Format("{0} WHERE BatchId in (select batchid from tblJobBatchRel where JobId = '{1}') order by Title",
                          CreateBaseSelectItemsSQL(False), lobjJob.Id)

        Else
          lstrSql =
            String.Format(
              "{0} WHERE BatchId in (select batchid from tblJobBatchRel where JobId = '{1}') and ProcessedStatus = '{2}' order by Title",
              CreateBaseSelectItemsSQL(False), lobjJob.Id, lpProcessedStatusFilter)
        End If

      Else
        Throw New Exception("Can't get batch items, type not supported.")

      End If

      Return lstrSql

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function CreateSelectItemsSQL(ByVal lpFilter As ItemFilter) As String
    Try

      Dim lobjSqlBuilder As New StringBuilder


      Dim lobjJob As Job = lpFilter.Job

      lobjSqlBuilder.AppendFormat("{0} WHERE BatchId in (select batchid from tblJobBatchRel where JobId = '{1}')",
                          CreateBaseSelectItemsSQL(False), lobjJob.Id)

      ' Add the title (if supplied)
      If Not String.IsNullOrEmpty(lpFilter.Title) Then
        lobjSqlBuilder.AppendFormat(" AND Title LIKE '%{0}%'", lpFilter.Title)
      End If

      ' Add the source doc id (if supplied)
      If Not String.IsNullOrEmpty(lpFilter.SourceDocId) Then
        lobjSqlBuilder.AppendFormat(" AND SourceDocId LIKE '%{0}%'", lpFilter.SourceDocId)
      End If

      ' Add the destination doc id (if supplied)
      If Not String.IsNullOrEmpty(lpFilter.DestinationDocId) Then
        lobjSqlBuilder.AppendFormat(" AND DestDocId LIKE '%{0}%'", lpFilter.DestinationDocId)
      End If

      ' Add the ProcessedStatus (if supplied)
      If Not String.IsNullOrEmpty(lpFilter.ProcessedStatus) Then
        lobjSqlBuilder.AppendFormat(" AND ProcessedStatus = '{0}'", lpFilter.ProcessedStatus)
      End If

      ' Add the ProcessedMessage (if supplied)
      If Not String.IsNullOrEmpty(lpFilter.ProcessedMessage) Then
        lobjSqlBuilder.AppendFormat(" AND ProcessedMessage LIKE '%{0}%'", lpFilter.ProcessedMessage)
      End If

      ' Add the ProcessedBy (if supplied)
      If Not String.IsNullOrEmpty(lpFilter.ProcessedBy) Then
        lobjSqlBuilder.AppendFormat(" AND ProcessedBy = '{0}'", lpFilter.ProcessedBy)
      End If

      lobjSqlBuilder.Append(" ORDER BY Title")

      If lpFilter.MaxItems > 0 Then
        Return lobjSqlBuilder.ToString().Replace("SELECT ", String.Format("SELECT TOP {0} ", lpFilter.MaxItems))
      Else
        Return lobjSqlBuilder.ToString()
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetAllItemsFromDBToDataTable(ByVal lpObject As Object,
                                                ByVal lpProcessedStatusFilter As String) As DataTable

    Dim lobjDataTable As New DataTable()

    Try

      Dim lstrSql As String = CreateSelectItemsSQL(lpObject, lpProcessedStatusFilter)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjDA As New SqlDataAdapter()
          lobjDA.SelectCommand = New SqlCommand(lstrSql, lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
          }

          Helper.HandleConnection(lobjConnection)
          lobjDA.Fill(lobjDataTable)

        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try

    Return lobjDataTable
  End Function

  Private Function GetAllProcessedByNodeNamesFromDB(lpJob As Job) As IList(Of String)
    Try

      Dim lobjDataReader As SqlDataReader = Nothing

      Dim lstrSQL As String = String.Format("SELECT DISTINCT ProcessedBy FROM {0} WHERE BatchId in (SELECT BatchId from tblJobBatchRel where JobId = '{1}') AND ProcessedBy IS NOT NULL", TABLE_BATCH_ITEM_NAME, lpJob.Id)
      Dim lobjNodeNames As New List(Of String)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)

          lobjDataReader = lobjCommand.ExecuteReader

          If (lobjDataReader.HasRows = True) Then
            While lobjDataReader.Read
              lobjNodeNames.Add(lobjDataReader("ProcessedBy").ToString())
            End While
          End If

          If (lobjConnection.State = ConnectionState.Open) Then
            lobjConnection.Close()
          End If

        End Using
      End Using

      lobjNodeNames.Sort()

      Return lobjNodeNames

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetFilteredItemsFromDBToDataTable(ByVal lpFilter As ItemFilter) As DataTable

    Dim lobjDataTable As New DataTable()

    Try

      Dim lstrSql As String = CreateSelectItemsSQL(lpFilter)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjDA As New SqlDataAdapter()
          lobjDA.SelectCommand = New SqlCommand(lstrSql, lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
          }

          Helper.HandleConnection(lobjConnection)
          lobjDA.Fill(lobjDataTable)

        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try

    Return lobjDataTable
  End Function

  Private Function ExecuteNonQuery(ByVal lpSQL As String) As Integer
    Try
      Return ExecuteNonQuery(lpSQL, Me.ItemsLocation.Location)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function ExecuteNonQuery(ByVal lpSQL As String, lpConnectionString As String) As Integer

    Dim lintNewRecordAff As Integer = 0

    Try
      Using lobjConnection As New SqlConnection(lpConnectionString)

        Using cmdAdd As New SqlCommand(lpSQL, lobjConnection)
          cmdAdd.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          lintNewRecordAff = cmdAdd.ExecuteNonQuery()
        End Using
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try

    Return lintNewRecordAff
  End Function

  ''' <summary>
  ''' Executes a query with the specified SQL statement and returns the first column of the first row.
  ''' </summary>
  ''' <param name="lpSQL">The SQL statement to execute.</param>
  ''' <returns>Returns the first column of the first row.  If there are no results then returns an empty string.</returns>
  ''' <remarks></remarks>
  Private Function ExecuteSimpleQuery(ByVal lpSQL As String) As Object
    Try
      Return ExecuteSimpleQuery(lpSQL, Me.ItemsLocation.Location)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  ''' Executes a query with the specified SQL statement and returns the first column of the first row.
  ''' </summary>
  ''' <param name="lpSQL">The SQL statement to execute.</param>
  ''' <param name="lpConnectionString">The connection string to use.</param>
  ''' <returns>Returns the first column of the first row.  If there are no results then returns an empty string.</returns>
  ''' <remarks></remarks>
  Private Shared Function ExecuteSimpleQuery(ByVal lpSQL As String, ByVal lpConnectionString As String) As Object

    Dim lobjResult As Object

    Try
      'LogSession.EnterMethod(Level.Debug, String.Format("{0} ({1})", Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod), lpSQL))
      Using lobjConnection As New SqlConnection(lpConnectionString)

        Using lobjCommand As New SqlCommand(lpSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          lobjResult = lobjCommand.ExecuteScalar
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Level.Debug, String.Format("{0} ({1})", Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod), lpSQL))
    End Try

    Return lobjResult
  End Function

  Private Sub BeginItemProcessDB(ByVal e As BatchItemProcessEventArgs)
    Dim lstrSql As String = String.Empty


    Dim lintNewRecordAff As Integer
    Try

      lstrSql = CreateUpdateItemSQL(e)

      lintNewRecordAff = ExecuteNonQuery(lstrSql)

      If (lintNewRecordAff = 0) Then
        Throw _
          New Exception(String.Format("{0}: Failed to update item in tblBatchItems. Sql: {1}",
                                      Reflection.MethodBase.GetCurrentMethod, lstrSql))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

      If (ex.Message.StartsWith("Could not update; currently locked.")) Then

        'Retry
        Try
          ApplicationLogging.WriteLogEntry("BEGIN: Trying to update record after lock error. ItemId=" & e.ItemId,
                                           TraceEventType.Information)
          Threading.Thread.Sleep(1000) 'Hey, wait a sec
          lintNewRecordAff = ExecuteNonQuery(lstrSql)

        Catch exx As Exception
          ApplicationLogging.LogException(exx, MethodBase.GetCurrentMethod)
          Throw
        End Try

      End If

      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub EndItemProcessDB(ByVal e As BatchItemProcessEventArgs)

    Dim lintNewRecordAff As Integer = 0
    Dim lstrSql As String = String.Empty

    Try

      lstrSql = CreateUpdateItemSQL(e)

      Try
        lintNewRecordAff = ExecuteNonQuery(lstrSql)
      Catch SqlEx As SqlException
        ApplicationLogging.WriteLogEntry(String.Format("{0}: {1}", SqlEx.Message, lstrSql),
                                         Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Error, 38761)
      End Try

      If (lintNewRecordAff = 0) Then
        Throw _
          New Exception(String.Format("{0}: Failed to update item in tblBatchItems. Sql: {1}",
                                      Reflection.MethodBase.GetCurrentMethod, lstrSql))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

      If (ex.Message.StartsWith("Could not update; currently locked.")) Then

        'Retry
        Try
          ApplicationLogging.WriteLogEntry("END:Trying to update record after lock error. ItemID=" & e.ItemId,
                                           TraceEventType.Information)
          Threading.Thread.Sleep(1000) 'Hey, wait a sec
          lintNewRecordAff = ExecuteNonQuery(lstrSql)

        Catch exx As Exception
          ApplicationLogging.LogException(exx, MethodBase.GetCurrentMethod)
          Throw
        End Try

      End If

      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub UpdateBatchItemDB(ByVal e As BatchItemProcessEventArgs)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_update_batch_item", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjItemIdParameter As New SqlParameter("@itemid", SqlDbType.NVarChar, 255) With {
            .Value = e.ItemId
          }
          lobjCommand.Parameters.Add(lobjItemIdParameter)

          Dim lobjDestinationDocIdParameter As New SqlParameter("@destinationdocid", SqlDbType.NVarChar, 255) With {
            .Value = e.DestDocId
          }
          lobjCommand.Parameters.Add(lobjDestinationDocIdParameter)

          Dim lobjProcessedStatusParameter As New SqlParameter("@processedstatus", SqlDbType.NVarChar, 255) With {
            .Value = e.ProcessedStatus.ToString()
          }
          lobjCommand.Parameters.Add(lobjProcessedStatusParameter)

          Dim lobjProcessResultParameter As New SqlParameter("@processresult", SqlDbType.NText)
          If e.Process IsNot Nothing AndAlso e.Process.ResultDetail IsNot Nothing Then
            lobjProcessResultParameter.Value = e.Process.ResultDetail.ToXmlString()
          Else
            lobjProcessResultParameter.Value = DBNull.Value
          End If
          lobjCommand.Parameters.Add(lobjProcessResultParameter)

          Dim lobjProcessedMessageParameter As New SqlParameter("@processedmessage", SqlDbType.NVarChar, 255) With {
            .Value = e.ProcessedMessage
          }
          lobjCommand.Parameters.Add(lobjProcessedMessageParameter)

          Dim lobjProcessedStartTimeParameter As New SqlParameter("@processstarttime", SqlDbType.DateTime)
          If e.StartTime <> DateTime.MinValue Then
            lobjProcessedStartTimeParameter.Value = e.StartTime
          Else
            lobjProcessedStartTimeParameter.Value = DBNull.Value
          End If
          lobjCommand.Parameters.Add(lobjProcessedStartTimeParameter)

          Dim lobjProcessedFinishTimeParameter As New SqlParameter("@processfinishtime", SqlDbType.DateTime)
          If e.EndTime <> DateTime.MinValue Then
            lobjProcessedFinishTimeParameter.Value = e.EndTime
          Else
            lobjProcessedFinishTimeParameter.Value = DBNull.Value
          End If
          lobjCommand.Parameters.Add(lobjProcessedFinishTimeParameter)

          Dim lobjTotalProcessingTimeParameter As New SqlParameter("@totalprocessingtime", SqlDbType.Float) With {
            .Value = e.TotalProcessingTime.TotalSeconds
          }
          lobjCommand.Parameters.Add(lobjTotalProcessingTimeParameter)

          Dim lobjProcessedByParameter As New SqlParameter("@processedby", SqlDbType.NVarChar, 255) With {
            .Value = e.ProcessedBy
          }
          lobjCommand.Parameters.Add(lobjProcessedByParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New ItemDoesNotExistException(e.ItemId)
          End If
        End Using
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try
  End Sub

  Private Shared Function CreateUpdateItemSQL(ByVal e As BatchItemProcessEventArgs) As String
    Try

      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.AppendFormat("UPDATE {0} set ProcessedStatus = '{1}', ", TABLE_BATCH_ITEM_NAME, e.ProcessedStatus)

      If e.ProcessedStatus = ProcessedStatus.Processing Then

        lobjSqlBuilder.AppendFormat("ProcessStartTime = '{0}', ", e.StartTime.ToString)
        lobjSqlBuilder.AppendFormat("ProcessedBy = '{0}' ", e.ProcessedBy)

      Else

        lobjSqlBuilder.AppendFormat("DestDocId = '{0}', ", e.DestDocId.Replace("'", "''"))
        lobjSqlBuilder.AppendFormat("ProcessedMessage = '{0}', ", e.ProcessedMessage.Replace("'", "''"))

        If e.Process IsNot Nothing AndAlso e.Process.ResultDetail IsNot Nothing Then
          lobjSqlBuilder.AppendFormat("ProcessResult = '{0}', ", e.Process.ResultDetail.ToXmlString.Replace("'", "''"))
        End If

        lobjSqlBuilder.AppendFormat("ProcessFinishTime = '{0}', ", e.EndTime.ToString("G", New CultureInfo("en-US")))
        lobjSqlBuilder.AppendFormat("TotalProcessingTime = '{0}' ", e.TotalProcessingTime.TotalSeconds)

      End If

      lobjSqlBuilder.AppendFormat("WHERE ID = {0} AND BatchId = '{1}' ", e.ItemId, e.BatchId)

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function CreateResetItemsSQLBase() As String
    Try
      Dim lobjSqlBuilder As New StringBuilder

      With lobjSqlBuilder
        .AppendFormat("Update {0}", TABLE_BATCH_ITEM_NAME)
        .AppendFormat(" SET DestDocID = NULL, ProcessedStatus = '{0}',", ProcessedStatus.NotProcessed.ToString)
        .Append(" ProcessedMessage = NULL, ProcessStartTime = NULL, ProcessFinishTime = NULL,")
        .Append(" TotalProcessingTime = NULL, ProcessedBy = NULL, ProcessResult = NULL ")
      End With

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchSummaryCountsDB(ByVal lpBatchId As String, Optional lpOperation As String = "") _
    As WorkSummary

    Dim lobjBatchSummary As WorkSummary = Nothing

    Dim ldblProcessingRate As Double
    Dim ldblPeakProcessingRate As Double
    Dim ldatStartTime As DateTime
    Dim ldatFinishTime As DateTime
    Dim ldatLastUpdateTime As DateTime

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_get_batch_summary_counts", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjIdParameter As New SqlParameter("@batchId", SqlDbType.NVarChar, 255) With {
            .Value = lpBatchId
          }
          lobjCommand.Parameters.Add(lobjIdParameter)
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                'Should only be 1 row

                Dim ldblAvgProcessingTime As Double = 0

                If (IsDBNull(lobjDataReader("AvgProcessingTime")) = False) Then
                  ldblAvgProcessingTime = Convert.ToDouble(lobjDataReader("AvgProcessingTime"))
                End If

                If (IsDBNull(lobjDataReader("ProcessingRate")) = False) Then
                  ldblProcessingRate = Convert.ToDouble(lobjDataReader("ProcessingRate"))
                End If

                If (IsDBNull(lobjDataReader("PeakProcessingRate")) = False) Then
                  ldblPeakProcessingRate = Convert.ToDouble(lobjDataReader("PeakProcessingRate"))
                End If

                'If (IsDBNull(lobjDataReader("StartTime")) = False) Then
                '  ldatStartTime = Convert.ToDateTime(lobjDataReader("StartTime"))
                'End If

                'If (IsDBNull(lobjDataReader("FinishTime")) = False) Then
                '  ldatFinishTime = Convert.ToDateTime(lobjDataReader("FinishTime"))
                'End If

                If (IsDBNull(lobjDataReader("LastUpdateTime")) = False) Then
                  ldatLastUpdateTime = Convert.ToDateTime(lobjDataReader("LastUpdateTime"))
                End If

                lobjBatchSummary = New WorkSummary(lpBatchId,
                                                   lpOperation,
                                                   Convert.ToInt32(lobjDataReader("NotProcessedCount")),
                                                   Convert.ToInt32(lobjDataReader("SuccessCount")),
                                                   Convert.ToInt32(lobjDataReader("FailedCount")),
                                                   Convert.ToInt32(lobjDataReader("ProcessingCount")),
                                                   Convert.ToInt32(lobjDataReader("TotalItemCount")),
                                                   ldblAvgProcessingTime,
                                                   ldatStartTime,
                                                   ldatFinishTime,
                                                   ldatLastUpdateTime,
                                                   ldblProcessingRate,
                                                   ldblPeakProcessingRate)

              End While

              lobjDataReader.Close()

            End If
          End Using
        End Using
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try

    Return lobjBatchSummary
  End Function

#Region "Lock/UnLock"

  ''' <summary>
  ''' Add a lock record
  ''' </summary>
  ''' <param name="lpBatchId"></param>
  ''' <param name="lpLockedBy"></param>
  ''' <remarks></remarks>
  Private Sub LockBatchDB(ByVal lpBatchId As String,
                          ByVal lpLockedBy As String)

    Try

      'Check if already locked, if so then throw exception
      Dim lstrSQL As String = String.Format("SELECT LockedBy from {0} WHERE BatchID = '{1}' AND IsLocked = '1'",
                                            TABLE_BATCHLOCK_NAME, lpBatchId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lobjCommand As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lstrCurrentlyLockedBy As String = lobjCommand.ExecuteScalar()

        If (Not String.IsNullOrEmpty(lstrCurrentlyLockedBy)) Then

          If (String.Equals(lstrCurrentlyLockedBy, lpLockedBy) = False) Then
            Throw _
              New Exception(
                String.Format(
                  "This batch is locked by another system - {0}.  Cannot process a batch that is currently locked.",
                  lstrCurrentlyLockedBy))
          End If

        Else
          'Lock the batch
          lstrSQL = String.Format("Insert into {0}(BatchId,IsLocked,LockDate,LockedBy) VALUES('{1}','1','{2}','{3}')",
                                  TABLE_BATCHLOCK_NAME, lpBatchId,
                                  Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat.SortableDateTimePattern),
                                  lpLockedBy)
          lobjCommand = New SqlCommand(lstrSQL, lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
          }

          Helper.HandleConnection(lobjConnection)

          Dim lintRowsAffected As Integer = lobjCommand.ExecuteNonQuery()

        End If

        lobjCommand.Dispose()

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw

    End Try
  End Sub

  ''' <summary>
  ''' Removes a lock record
  ''' </summary>
  ''' <param name="lpBatchId"></param>
  ''' <param name="lpUnLockedBy"></param>
  ''' <remarks></remarks>
  Private Sub UnLockBatchDB(ByVal lpBatchId As String,
                            ByVal lpUnLockedBy As String)

    Try

      Dim lstrSQL As String = String.Format("Delete FROM {0} WHERE BatchId = '{1}' and IsLocked = '1'",
                                            TABLE_BATCHLOCK_NAME, lpBatchId)

      'UnLock the batch
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          lobjCommand.ExecuteNonQuery()

        End Using

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw

    End Try
  End Sub

  Private Function GetBatchLockCountDB(lpJobName As String) As Integer
    Try
      ' SELECT COUNT(BL.ID) AS BatchLockCount
      ' FROM tblJob AS J INNER JOIN
      ' tblJobBatchRel AS JBR ON J.JobId = JBR.JobId INNER JOIN
      ' tblBatchLock AS BL ON JBR.BatchId = BL.BatchId
      ' WHERE (J.JobName = '')

      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.Append("SELECT COUNT(BL.ID) AS BatchLockCount ")
      lobjSqlBuilder.Append("FROM tblJob AS J INNER JOIN ")
      lobjSqlBuilder.Append("tblJobBatchRel AS JBR ON J.JobId = JBR.JobId INNER JOIN ")
      lobjSqlBuilder.Append("tblBatchLock AS BL ON JBR.BatchId = BL.BatchId ")
      lobjSqlBuilder.AppendFormat("WHERE (J.JobName = '{0}')", lpJobName)

      Return ExecuteSimpleQuery(lobjSqlBuilder.ToString)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchLocksDB(lpJobName As String) As IBatchLocks
    Dim lobjBatchLocks As New BatchLocks

    Try
      Dim lobjSqlBuilder As New StringBuilder

      Dim lstrErrorMessage As String = String.Empty

      lobjSqlBuilder.Append("SELECT BL.ID, BL.BatchId, J.JobId, J.JobName, ")
      lobjSqlBuilder.Append("BL.IsLocked, BL.LockDate, BL.UnlockDate, BL.LockedBy ")
      lobjSqlBuilder.Append("FROM tblBatchLock BL INNER JOIN tblJobBatchRel JBR ")
      lobjSqlBuilder.Append("ON BL.BatchId = JBR.BatchId INNER JOIN tblJob J ")
      lobjSqlBuilder.Append("ON JBR.JobId = J.JobId ")
      If Not String.IsNullOrEmpty(lpJobName) Then
        lobjSqlBuilder.AppendFormat("WHERE j.JobName='{0}' ", lpJobName)
      End If
      lobjSqlBuilder.Append("ORDER BY J.JobName, BL.ID")

      Try
        Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

          Using lobjCommand = New SqlCommand(lobjSqlBuilder.ToString, lobjConnection)
            lobjCommand.CommandTimeout = COMMAND_TIMEOUT

            Helper.HandleConnection(lobjConnection)
            Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

              If (lobjDataReader.HasRows = True) Then
                While lobjDataReader.Read
                  lobjBatchLocks.Add(GetBatchLockFromDataReader(lobjDataReader, lstrErrorMessage))
                End While
              End If
              lobjDataReader.Close()
            End Using
          End Using
        End Using

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw

      End Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return lobjBatchLocks
  End Function

#End Region

  Private Function GetProcessResultsDB(lpBatch As Batch) As IProcessResults
    Try

      Dim lobjProcessResults As New ProcessResults
      Dim lobjProcessResult As ProcessResult = Nothing


      Dim lobjSQLBuilder As New StringBuilder
      lobjSQLBuilder.AppendFormat("SELECT ProcessResult FROM {0} WHERE ProcessResult IS NOT NULL", TABLE_BATCH_ITEM_NAME)
      lobjSQLBuilder.AppendFormat(" AND BatchId = '{0}'", lpBatch.Id)

      'ExecuteSimpleQuery(lobjSQLBuilder.ToString())

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lobjSQLBuilder.ToString(), lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                lobjProcessResult = Nothing
                Try
                  lobjProcessResult = Serializer.Deserialize.XmlString(lobjDataReader("ProcessResult"),
                                                                       GetType(ProcessResult))
                  lobjProcessResults.Add(lobjProcessResult)
                Catch ex As Exception
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
                  Continue While
                End Try

              End While
            End If
          End Using

        End Using
      End Using

      Return lobjProcessResults

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProcessResultsDB(lpJob As Job) As IProcessResults
    Try

      Dim lobjProcessResults As New ProcessResults
      Dim lobjProcessResult As ProcessResult = Nothing


      Dim lobjSQLBuilder As New StringBuilder
      lobjSQLBuilder.AppendFormat("SELECT ProcessResult FROM [{0}] WHERE ProcessResult IS NOT NULL", Me.GetJobViewName(lpJob.Name))
      'lobjSQLBuilder.AppendFormat(" AND ID = '{0}'", lpJob.Id)

      'ExecuteSimpleQuery(lobjSQLBuilder.ToString())

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lobjSQLBuilder.ToString(), lobjConnection)
          lobjCommand.CommandTimeout = 60

          Helper.HandleConnection(lobjConnection)

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                lobjProcessResult = Nothing
                Try
                  lobjProcessResult = Serializer.Deserialize.XmlString(lobjDataReader("ProcessResult"),
                                                                       GetType(ProcessResult))
                  lobjProcessResults.Add(lobjProcessResult)
                Catch ex As Exception
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
                  Continue While
                End Try

              End While
            End If
          End Using

        End Using
      End Using

      Return lobjProcessResults

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProjectAvgProcessingTimeDB(ByVal lpProjectId As String) As Single

    Try

      Dim lobjQueryResult As Object = Nothing

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_get_avg_processing_time_project", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjProjectIdParameter As New SqlParameter("@projectName", SqlDbType.NVarChar, 255) With {
            .Value = lpProjectId
          }
          lobjCommand.Parameters.Add(lobjProjectIdParameter)

          Helper.HandleConnection(lobjConnection)
          lobjQueryResult = lobjCommand.ExecuteScalar
        End Using

        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If

      End Using

      If Not IsDBNull(lobjQueryResult) Then
        Return lobjQueryResult
      Else
        Return 0
      End If


    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProjectDbFileInfoDB() As DbFilesInfo
    Try
      Dim lobjDbFilesInfo As New DbFilesInfo
      Dim lobjDbFileInfo As DbFileInfo

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Dim lstrSQL As String = String.Format("SELECT * FROM {0}", VIEW_DB_FILE_INFO)
        Using lobjProjectCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjProjectCommand.CommandType = CommandType.Text
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjProjectCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                lobjDbFileInfo = New DbFileInfo(lobjDataReader("DBName"),
                                                lobjDataReader("LogicalDBName"),
                                                lobjDataReader("FileSizeInMB"),
                                                lobjDataReader("IsPercentGrowth"),
                                                lobjDataReader("GrowthInIncrementOf"),
                                                lobjDataReader("NextAutoGrowthSizeInMB"),
                                                lobjDataReader("MaxSize"),
                                                lobjDataReader("FileName"),
                                                lobjDataReader("LogicalDriveName"),
                                                lobjDataReader("Drive"),
                                                lobjDataReader("FreeSpaceInMB"))
                lobjDbFilesInfo.Add(lobjDbFileInfo)
              End While
            End If
          End Using
        End Using
      End Using

      Return lobjDbFilesInfo

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetCurrentProjectId() As String
    Try
      Dim lstrId As String = String.Empty
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Dim lstrSQL As String = String.Format("SELECT [ProjectId] FROM {0}", TABLE_PROJECT_NAME)
        Using lobjProjectCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjProjectCommand.CommandType = CommandType.Text
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjProjectCommand.ExecuteReader
            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                If (IsDBNull(lobjDataReader("ProjectId")) = False) Then
                  lstrId = lobjDataReader("ProjectId")
                End If
              End While
              lobjDataReader.Close()
            End If
          End Using
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

      Return lstrId

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function ProjectExistsDB(lpProjectId As String) As Boolean
    Try
      Dim lintProjectCount As Integer =
            ExecuteSimpleQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE ProjectId = '{1}'",
                                             TABLE_PROJECTS_NAME, lpProjectId), ProjectCatalog.CurrentConnectionString)
      If lintProjectCount > 0 Then
        Return True
      Else
        Return False
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetOrphanBatchCountDB() As Integer
    Try
      'LogSession.EnterMethod(Level.Debug, Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
      Return ExecuteSimpleQuery(String.Format("SELECT COUNT(*) FROM {0}", VIEW_ORPHAN_BATCHES_NAME))
    Catch SqlEx As SqlException
      ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod())
      If SqlEx.Message.StartsWith(INVALID_OBJECT_NAME) Then
        Dim lstrProjectName As String = Me.GetProjectNameDB()
        'LogSession.LogWarning("Project '{0}' missing view 'vwOrphanBatches'", lstrProjectName)
        'LogSession.LogMessage("Re-creating stored procedures for project '{0}'.", lstrProjectName)
        CreateStoredProcedures(DatabaseType.Project)
      End If
      Return 0
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Level.Debug, Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try
  End Function

  Private Function GetOrphanBatchItemCountDB() As Integer
    Try
      Return ExecuteSimpleQuery(String.Format("SELECT COUNT(*) FROM {0}", VIEW_ORPHAN_BATCH_ITEMS_NAME))

    Catch SqlEx As SqlException
      ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod())
      If SqlEx.Message.ToLower.Contains("timeout expired") Then
        Return 0
      Else
        ' Re-throw the exception to the caller
        Throw
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function


  Private Function GetProjectInfoDB() As IProjectInfo

    Dim lobjProjectInfo As ProjectInfo = Nothing

    Try

      Dim lstrProjectId As String = String.Empty
      Dim lstrJobId As String = String.Empty
      Dim lstrName As String = String.Empty
      Dim lstrDescription As String = String.Empty
      Dim ldatCreateDate As DateTime = DateTime.MinValue
      Dim lstrOperation As String = String.Empty
      Dim llngItemsProcessed As Long
      Dim lobjWorkSummary As IWorkSummary = Nothing

      Dim lstrSQL As String =
            String.Format(
              "SELECT [ProjectId], [ProjectName], [Description], [CreateDate], [ItemsProcessed], [WorkSummary] FROM {0}",
              TABLE_PROJECT_NAME)
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjProjectCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjProjectCommand.CommandType = CommandType.Text
          lobjProjectCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader = lobjProjectCommand.ExecuteReader


            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read

                If (IsDBNull(lobjDataReader("ProjectId")) = False) Then
                  lstrProjectId = lobjDataReader("ProjectId")
                End If
                If (IsDBNull(lobjDataReader("ProjectName")) = False) Then
                  lstrName = lobjDataReader("ProjectName")
                End If
                If (IsDBNull(lobjDataReader("Description")) = False) Then
                  lstrDescription = lobjDataReader("Description")
                End If
                If (IsDBNull(lobjDataReader("CreateDate")) = False) Then
                  ldatCreateDate = lobjDataReader("CreateDate")
                End If
                If (IsDBNull(lobjDataReader(PROJECT_ITEMS_PROCESSED_COLUMN)) = False) Then
                  llngItemsProcessed = lobjDataReader(PROJECT_ITEMS_PROCESSED_COLUMN)
                End If
                If (IsDBNull(lobjDataReader("WorkSummary")) = False) Then
                  lobjWorkSummary = New WorkSummary(lobjDataReader("WorkSummary"), lstrName)
                End If
              End While
              lobjDataReader.Close()

              Dim lintOrphanBatchCount As Integer = GetOrphanBatchCount()
              Dim lintOrphanBatchItemCount As Integer = GetOrphanBatchItemCount()

              lobjProjectInfo = New ProjectInfo(lstrProjectId, lstrName, lstrDescription,
                                                ldatCreateDate, Me.ItemsLocation.Location,
                                                llngItemsProcessed, lintOrphanBatchCount,
                                                lintOrphanBatchItemCount, lobjWorkSummary)

              Dim lobjJobs As IJobInfoCollection = GetJobInfoCollectionDB()

              For Each lobjJob As IJobInfo In lobjJobs
                lobjProjectInfo.Jobs.Add(lobjJob)
              Next

              lobjProjectInfo.InitializeChildStatuses()

            End If
          End Using
        End Using
      End Using
    Catch SqlEx As SqlException
      If SqlEx.Message = "Invalid column name 'WorkSummary'." Then
        AddNewColumnsToTables()
        CreateStoredProcedures(DatabaseType.Project)
        Return GetProjectInfoDB()
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try

    Return lobjProjectInfo
  End Function

  Private Function GetJobInfoCollectionDB() As IJobInfoCollection

    Dim lobjJobInfoCollection As IJobInfoCollection = New JobInfoCollection

    Try
      Dim lstrJobId As String = String.Empty
      Dim lstrSQL As String = String.Format("SELECT [JobId] FROM {0}", TABLE_JOB_NAME)
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjJobCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjJobCommand.CommandType = CommandType.Text
          lobjJobCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjJobCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                If (IsDBNull(lobjDataReader("JobId")) = False) Then
                  lstrJobId = lobjDataReader("JobId")
                  lobjJobInfoCollection.Add(GetJobInfoDB(lstrJobId))
                End If

              End While
            End If

          End Using
        End Using
      End Using

      lobjJobInfoCollection.Sort()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try

    Return lobjJobInfoCollection
  End Function

  Private Function GetProjectNameDB(lpJobName As String) As String
    Dim lstrProjectName As String = String.Empty

    Try

      Dim lobjSqlBuilder As New StringBuilder
      lobjSqlBuilder.Append("SELECT P.ProjectName ")
      lobjSqlBuilder.AppendFormat("FROM {0} AS P INNER JOIN ", TABLE_PROJECT_NAME)
      lobjSqlBuilder.AppendFormat("{0} AS PJR ON P.ProjectId = PJR.ProjectId INNER JOIN ", TABLE_PROJECTJOBREL_NAME)
      lobjSqlBuilder.AppendFormat("{0} AS J ON PJR.JobId = J.JobId ", TABLE_JOB_NAME)
      lobjSqlBuilder.AppendFormat("WHERE (J.JobName = N'{0}')", lpJobName)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lobjSqlBuilder.ToString(), lobjConnection)
          lobjCommand.CommandType = CommandType.Text
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then
              lobjDataReader.Read()
              If (IsDBNull(lobjDataReader("ProjectName")) = False) Then
                lstrProjectName = lobjDataReader("ProjectName")
              End If
            End If
            lobjDataReader.Close()
          End Using
        End Using
      End Using

      Return lstrProjectName

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Function

  Private Function GetJobInfoDB(lpJobId As String,
                                Optional lpProjectId As String = ControlChars.NullChar,
                                Optional lpExistingConnection As SqlConnection = Nothing) As IJobInfo
    'Dim lobjConnection As SqlConnection = Nothing
    Dim lobjDataReader As SqlDataReader = Nothing
    Dim lobjJobInfo As IJobInfo = Nothing

    Try

      Dim lstrName As String = String.Empty
      Dim lstrOperation As String = String.Empty
      Dim ldatCreateDate As DateTime = DateTime.MinValue
      Dim llngItemsProcessed As Long
      Dim lobjWorkSummary As IWorkSummary = Nothing
      Dim lstrProjectName As String = GetProjectNameDB()
      Dim lobjConnection As SqlConnection = Nothing

      If lpExistingConnection IsNot Nothing Then
        lobjConnection = lpExistingConnection
      Else
        lobjConnection = New SqlConnection(Me.ItemsLocation.Location)
      End If

      Dim lstrSQL As String =
            String.Format(
              "SELECT [JobName], [Operation], [CreateDate], [ItemsProcessed], [WorkSummary] FROM {0} WHERE JobId = '{1}'",
              TABLE_JOB_NAME, lpJobId)

      Using lobjConnection

        Using lobjJobCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjJobCommand.CommandType = CommandType.Text
          lobjJobCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          lobjDataReader = lobjJobCommand.ExecuteReader

          If (lobjDataReader.HasRows = True) Then
            While lobjDataReader.Read

              If (IsDBNull(lobjDataReader("JobName")) = False) Then
                lstrName = lobjDataReader("JobName")
              End If
              If (IsDBNull(lobjDataReader("Operation")) = False) Then
                lstrOperation = lobjDataReader("Operation")
              End If
              If (IsDBNull(lobjDataReader("CreateDate")) = False) Then
                ldatCreateDate = lobjDataReader("CreateDate")
              End If
              If (IsDBNull(lobjDataReader(PROJECT_ITEMS_PROCESSED_COLUMN)) = False) Then
                llngItemsProcessed = lobjDataReader(PROJECT_ITEMS_PROCESSED_COLUMN)
              End If
              If (IsDBNull(lobjDataReader("WorkSummary")) = False) Then
                lobjWorkSummary = New WorkSummary(lobjDataReader("WorkSummary"), lstrOperation, lstrName)
              Else
                ' If there is no value here then the worksummary value has not been set for one of more projects
                ' We will force it to recalculate these values and then come back.
                ' Since the value is not set it is possible that the stored procedures in place have not been 
                ' updated to the version that updates the cache either, so we will force a refresh to 
                ' the latest version of the stored procedures as well.
                If String.IsNullOrEmpty(lpProjectId) Then
                  lpProjectId = GetProjectIdDB()
                End If
                CreateStoredProcedures(DatabaseType.Project)
                GetProjectSummaryCountsDB(lpProjectId)
                'lobjDataReader.Close()
                'lobjJobCommand.Dispose()
                'Return GetJobInfoDB(lpJobId, , lobjConnection)
              End If

              lobjJobInfo = New JobInfo(lpJobId, lstrName, lstrProjectName, llngItemsProcessed, lstrOperation,
                                        ldatCreateDate, lobjWorkSummary)

            End While
          End If
        End Using
        If (lobjConnection.State = ConnectionState.Open) Then
          lobjConnection.Close()
        End If
      End Using

    Catch SqlEx As SqlException
      If SqlEx.Message = "Invalid column name 'WorkSummary'." Then
        AddNewColumnsToTables()
        CreateStoredProcedures(DatabaseType.Project)
        Return GetJobInfoDB(lpJobId)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally

    End Try

    Return lobjJobInfo
  End Function

  Private Function GetDetailedJobInfoDB(lpJobId As String, Optional lpProjectId As String = ControlChars.NullChar) _
    As IDetailedJobInfo
    Try

      Dim lobjJobInfo As IJobInfo = GetJobInfoDB(lpJobId, lpProjectId)
      Dim lobjBatchInfoCollection As IBatchInfoCollection = GetBatchInfoCollectionDB(lobjJobInfo)

      Return New DetailedJobInfo(lobjJobInfo, lobjBatchInfoCollection)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchInfoCollectionDB(lpJob As IJobInfo) As IBatchInfoCollection

    Dim lobjBatchInfoCollection As IBatchInfoCollection = New BatchInfoCollection

    Try
      Dim lstrId As String = String.Empty
      Dim lstrSQL As String = String.Format("SELECT [BatchId] FROM {0}", TABLE_BATCH_NAME)
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjJobCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjJobCommand.CommandType = CommandType.Text
          lobjJobCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjJobCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read
                If (IsDBNull(lobjDataReader("BatchId")) = False) Then
                  lstrId = lobjDataReader("BatchId")
                  lobjBatchInfoCollection.Add(GetBatchInfoDB(lstrId))
                End If
              End While
            End If

          End Using
        End Using
      End Using

      lobjBatchInfoCollection.Sort()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try

    Return lobjBatchInfoCollection
  End Function

  Private Function GetBatchInfoDB(lpBatchId As String, Optional lpProjectId As String = "") As IBatchInfo
    Dim lobjBatchInfo As IBatchInfo = Nothing

    Try

      Dim lstrName As String = String.Empty
      Dim ldatCreateDate As DateTime = DateTime.MinValue
      Dim lobjWorkSummary As IWorkSummary = Nothing

      Dim lstrSQL As String =
            String.Format("SELECT [BatchName], [CreateDate], [WorkSummary] FROM {0} WHERE BatchId = '{1}'",
                          TABLE_BATCH_NAME, lpBatchId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjJobCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjJobCommand.CommandType = CommandType.Text
          lobjJobCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjJobCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then
              While lobjDataReader.Read

                If (IsDBNull(lobjDataReader("BatchName")) = False) Then
                  lstrName = lobjDataReader("BatchName")
                End If
                If (IsDBNull(lobjDataReader("CreateDate")) = False) Then
                  ldatCreateDate = lobjDataReader("CreateDate")
                End If
                If (IsDBNull(lobjDataReader("WorkSummary")) = False) Then
                  lobjWorkSummary = New WorkSummary(lobjDataReader("WorkSummary"), lstrName)
                Else
                  ' If there is no value here then the worksummary value has not been set for one of more projects
                  ' We will force it to recalculate these values and then come back.
                  ' Since the value is not set it is possible that the stored procedures in place have not been 
                  ' updated to the version that updates the cache either, so we will force a refresh to 
                  ' the latest version of the stored procedures as well.
                  If String.IsNullOrEmpty(lpProjectId) Then
                    lpProjectId = GetProjectIdDB()
                  End If
                  ' CreateStoredProcedures(DatabaseType.Project)
                  GetBatchSummaryCountsDB(lpBatchId)
                  Return GetBatchInfoDB(lpBatchId)
                End If

                lobjBatchInfo = New BatchInfo(lpBatchId, lstrName, ldatCreateDate, Nothing, lobjWorkSummary)

              End While
            End If

          End Using
        End Using
      End Using

    Catch SqlEx As SqlException
      If SqlEx.Message = "Invalid column name 'WorkSummary'." Then
        AddNewColumnsToTables()
        CreateStoredProcedures(DatabaseType.Project)
        Return GetBatchInfoDB(lpBatchId)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try

    Return lobjBatchInfo
  End Function

  Private Function GetProjectIdDB() As String
    Try
      Return ExecuteSimpleQuery(String.Format("SELECT ProjectId FROM {0}", TABLE_PROJECT_NAME))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProjectNameDB() As String
    Try
      Return ExecuteSimpleQuery(String.Format("SELECT ProjectName FROM {0}", TABLE_PROJECT_NAME))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub ClearWorkSummaryDB(ByVal lpWorkParent As Object)

    Dim lstrObjectName As String
    Dim lobjSqlBuilder As New StringBuilder
    Dim lintRecordsAffected As Integer

    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpWorkParent)
#Else
      If lpWorkParent Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpWorkParent))
      End If
#End If

      Dim lstrOperation As String
      If TypeOf lpWorkParent Is IJobInfo Then
        lobjSqlBuilder.AppendFormat("UPDATE {0} SET WorkSummary = NULL WHERE JobId = '{1}'",
                                    TABLE_JOB_NAME, DirectCast(lpWorkParent, IJobInfo).Id)
        lstrObjectName = DirectCast(lpWorkParent, IJobInfo).Name
        lstrOperation = DirectCast(lpWorkParent, IJobInfo).Operation

      ElseIf TypeOf lpWorkParent Is IProjectInfo Then
        lobjSqlBuilder.AppendFormat("UPDATE {0} SET WorkSummary = NULL WHERE ProjectId = '{1}'",
                                    TABLE_PROJECT_NAME, DirectCast(lpWorkParent, IProjectInfo).Id)
        lstrObjectName = DirectCast(lpWorkParent, IProjectInfo).Name
        lstrOperation = DirectCast(lpWorkParent, IProjectInfo).Description

      ElseIf TypeOf lpWorkParent Is IBatchInfo Then
        lobjSqlBuilder.AppendFormat("UPDATE {0} SET WorkSummary = NULL BatchId = '{1}'",
                                    TABLE_BATCH_NAME, DirectCast(lpWorkParent, IBatchInfo).Id)
        lstrObjectName = DirectCast(lpWorkParent, IBatchInfo).Name
        If DirectCast(lpWorkParent, IBatchInfo).ParentJob IsNot Nothing Then
          lstrOperation = DirectCast(lpWorkParent, IBatchInfo).ParentJob.Operation
        End If

      Else
        Throw New ArgumentOutOfRangeException(NameOf(lpWorkParent))
      End If

      lintRecordsAffected = ExecuteNonQuery(lobjSqlBuilder.ToString())

      If lintRecordsAffected = 0 Then
        ApplicationLogging.WriteLogEntry(String.Format("Failed to clear work summary for {0}: {1}",
                                                       lpWorkParent.GetType.Name, lstrObjectName))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetCachedWorkSummaryCountsDB(ByVal lpWorkParent As Object) As IWorkSummary

    Dim lstrObjectName As String
    Dim lstrOperation As String = String.Empty
    Dim lstrWorkSummaryString As String
    Dim lobjWorkSummary As WorkSummary = Nothing
    Dim lobjSqlBuilder As New StringBuilder

    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpWorkParent)
#Else
      If lpWorkParent Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpWorkParent))
      End If
#End If

      If TypeOf lpWorkParent Is IJobInfo Then
        lobjSqlBuilder.AppendFormat("SELECT WorkSummary FROM {0} WHERE JobId = '{1}'",
                                    TABLE_JOB_NAME, DirectCast(lpWorkParent, IJobInfo).Id)
        lstrObjectName = DirectCast(lpWorkParent, IJobInfo).Name
        lstrOperation = DirectCast(lpWorkParent, IJobInfo).Operation

      ElseIf TypeOf lpWorkParent Is IProjectInfo Then
        lobjSqlBuilder.AppendFormat("SELECT WorkSummary FROM {0} WHERE ProjectId = '{1}'",
                                    TABLE_PROJECT_NAME, DirectCast(lpWorkParent, IProjectInfo).Id)
        lstrObjectName = DirectCast(lpWorkParent, IProjectInfo).Name
        lstrOperation = DirectCast(lpWorkParent, IProjectInfo).Description

      ElseIf TypeOf lpWorkParent Is IBatchInfo Then
        lobjSqlBuilder.AppendFormat("SELECT WorkSummary FROM {0} WHERE BatchId = '{1}'",
                                    TABLE_BATCH_NAME, DirectCast(lpWorkParent, IBatchInfo).Id)
        lstrObjectName = DirectCast(lpWorkParent, IBatchInfo).Name
        If DirectCast(lpWorkParent, IBatchInfo).ParentJob IsNot Nothing Then
          lstrOperation = DirectCast(lpWorkParent, IBatchInfo).ParentJob.Operation
        End If

      Else
        Throw New ArgumentOutOfRangeException(NameOf(lpWorkParent))
      End If

      Dim lobjResult As Object = ExecuteSimpleQuery(lobjSqlBuilder.ToString())

      If Not IsDBNull(lobjResult) Then
        lstrWorkSummaryString = lobjResult
        If Not String.IsNullOrEmpty(lstrWorkSummaryString) Then
          lobjWorkSummary = New WorkSummary(lstrWorkSummaryString, lstrOperation, lstrObjectName)
        End If
      End If

      Return lobjWorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProjectSummaryCountsDB(ByVal lpProjectId As String) As WorkSummaries

    Dim lstrJobName As String
    Dim lstrOperation As String
    Dim lstrWorkSummaryString As String
    Dim lobjWorkSummary As WorkSummary
    Dim lobjWorkSummaries As New WorkSummaries

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Helper.HandleConnection(lobjConnection)

        Using lobjCommand As New SqlCommand("SELECT JobName, Operation, WorkSummary FROM tblJob", lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          lobjCommand.CommandType = CommandType.Text
          Using lobjReader As SqlDataReader = lobjCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If lobjReader.HasRows Then
              While lobjReader.Read()
                lstrJobName = lobjReader("JobName")
                lstrOperation = lobjReader("Operation")
                If Not IsDBNull(lobjReader("WorkSummary")) Then
                  lstrWorkSummaryString = lobjReader("WorkSummary")
                  lobjWorkSummary = New WorkSummary(lstrWorkSummaryString, lstrOperation, lstrJobName)
                  lobjWorkSummaries.Add(lobjWorkSummary)
                End If
              End While
            End If
          End Using
        End Using
      End Using

      lobjWorkSummaries.Sort()
      Return lobjWorkSummaries

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Function

  Private Sub UpdateCachedProjectWorkSummaryDB(lpProjectId As String, lpNewValue As String)
    Try

      Dim lstrSql As String = String.Format("UPDATE {0} SET WorkSummary = '{1}' WHERE ProjectId = '{2}'",
                                            TABLE_PROJECT_NAME, lpNewValue, lpProjectId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSql, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Helper.HandleConnection(lobjConnection)

          Dim lintNewRecordAff As Integer = lobjCommand.ExecuteNonQuery()

          If (lintNewRecordAff = 0) Then
            Throw _
              New Exception(String.Format("{0}: Failed to update work summary for project. Sql: {1}",
                                          Reflection.MethodBase.GetCurrentMethod, lstrSql))
          End If

          ' Update the catalog as well
          ' This is supposed to keep the work summary information in the catalog in sync with that of the project database.
          Dim lstrCatalogConnectionString As String = ProjectCatalog.CurrentConnectionString
          If Not String.IsNullOrEmpty(lstrCatalogConnectionString) Then
            Dim llngCurrentItemsProcessed As Long = GetItemsProcessedDB()

            lstrSql = String.Format("UPDATE {0} SET WorkSummary = '{1}', ItemsProcessed = {2} WHERE ProjectId = '{3}'",
                                    TABLE_PROJECTS_NAME, lpNewValue, llngCurrentItemsProcessed, lpProjectId)

            Using lobjCatalogConnection As New SqlConnection(lstrCatalogConnectionString)
              Using lobjCatalogAdd As New SqlCommand(lstrSql, lobjCatalogConnection)
                lobjCatalogAdd.CommandTimeout = COMMAND_TIMEOUT

                Helper.HandleConnection(lobjCatalogConnection)

                Dim lintRecordAff As Integer = lobjCatalogAdd.ExecuteNonQuery()

                If (lintRecordAff = 0) Then
                  Throw _
                    New Exception(String.Format("{0}: Failed to update project work summary for catalog. Sql: {1}",
                                                Reflection.MethodBase.GetCurrentMethod, lstrSql))
                End If
              End Using
            End Using

          End If

        End Using

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function FileExistsDB(ByVal lpJobId As String, ByVal lpFileName As String) As Boolean
    Try
      Dim lobjStringBuilder As New StringBuilder

      lobjStringBuilder.AppendFormat("SELECT COUNT(FileId) FROM [{0}] WHERE [JobId] = '{1}' AND [Name] = '{2}'",
                                     TABLE_FILES_NAME, lpJobId, lpFileName)

      Dim lintFileCount As Integer = ExecuteSimpleQuery(lobjStringBuilder.ToString)

      If lintFileCount > 0 Then
        Return True
      Else
        Return False
      End If

    Catch SqlEx As SqlException
      If SqlEx.Message.StartsWith(INVALID_OBJECT_NAME) Then
        CreateFileTable()
        Return False
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetFileListDB(ByVal lpJobId As String) As IList(Of String)
    Try
      Dim lobjReturnList As New List(Of String)
      Dim lobjStringBuilder As New StringBuilder

      lobjStringBuilder.AppendFormat("SELECT [Name] FROM [{0}] WHERE JobId = '{1}'", TABLE_FILES_NAME, lpJobId)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        lobjConnection.Open()
        Using lobjCommand As SqlCommand = lobjConnection.CreateCommand

          lobjCommand.CommandText = lobjStringBuilder.ToString
          lobjCommand.CommandType = CommandType.Text

          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader
            While lobjDataReader.Read()
              If (IsDBNull(lobjDataReader("Name")) = False) Then
                lobjReturnList.Add(lobjDataReader("Name"))
              End If
            End While
          End Using

        End Using
      End Using

      Return lobjReturnList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function RetrieveFileDB(ByVal lpFileId As String, ByVal lpFileName As String) As Stream
    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        lobjConnection.Open()
        Using lobjCommand As SqlCommand = lobjConnection.CreateCommand
          Dim lobjParameter As SqlParameter = Nothing
          'Dim lobjDataReader As SqlDataReader = Nothing
          Dim lobjData As Byte() = Nothing

          lobjCommand.CommandText = String.Format("SELECT [Data] FROM [{0}] WHERE [FileId] = @Id", TABLE_FILES_NAME)
          lobjCommand.CommandType = CommandType.Text
          lobjCommand.CommandTimeout = "180"
          lobjParameter = New SqlParameter("@Id", SqlDbType.UniqueIdentifier) With {
            .Value = New Guid(lpFileId)
          }
          lobjCommand.Parameters.Add(lobjParameter)

          'lobjDataReader = lobjCommand.ExecuteReader

          'If lobjDataReader.Read() Then
          '  lobjData = lobjDataReader("Data")
          '  Return Helper.CopyByteArrayToStream(lobjData)
          'Else
          '  Throw New Exceptions.DocumentDoesNotExistException(lpFileId)
          'End If

          Dim lobjResult As IAsyncResult = lobjCommand.BeginExecuteReader()

          Dim lintCounter As Integer

          While Not lobjResult.IsCompleted
            lintCounter += 1
            'LogSession.LogMessage(String.Format("Waiting ({0}) to retrieve file '{1}'.", lintCounter, lpFileName))
            ' Wait for 1/10 second, so the counter 
            ' does not consume all available resources  
            ' on the main thread.
            Threading.Thread.Sleep(100)
          End While

          ' Once the IAsyncResult object signals that it is done 
          ' waiting for results, we can retrieve the results. 

          Using lobjDataReader As SqlDataReader = lobjCommand.EndExecuteReader(lobjResult)
            If lobjDataReader.Read() Then
              lobjData = lobjDataReader("Data")
              Return Helper.CopyByteArrayToStream(lobjData)
            Else
              Throw New DocumentDoesNotExistException(lpFileId)
            End If
          End Using
        End Using
      End Using

      'Catch SqlEx As SqlException
      '  ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod)
      '  If SqlEx.Message.ToLower().Contains("timeout expired") Then
      '    ' Re-throw the exception to the caller
      '    Throw
      '  End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try
  End Function

  Private Function RetrieveFileDB(ByVal lpJob As Job, ByVal lpFileName As String) As Stream
    Try

      Dim lobjFileId As Guid =
            ExecuteSimpleQuery(String.Format("SELECT [FileId] FROM {0} WHERE [JobId] = '{1}' AND [Name] = '{2}'",
                                             TABLE_FILES_NAME, lpJob.Id, lpFileName))


      Return RetrieveFileDB(lobjFileId.ToString, lpFileName)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub StoreFileDB(ByVal lpJobId As String, ByVal lpFileName As String, ByVal lpFileData As Byte())
    Try

      Dim lobjFileId As Object = ExecuteSimpleQuery(String.Format("SELECT FileId FROM [{0}] WHERE JobId = '{1}' AND Name = '{2}'", TABLE_FILES_NAME, lpJobId, lpFileName))

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        lobjConnection.Open()
        Using lobjCommand As SqlCommand = lobjConnection.CreateCommand
          Dim lobjParameter As SqlParameter = Nothing
          Dim lintRowsInserted As Integer = 0

          If ((lobjFileId Is Nothing) OrElse (IsDBNull(lobjFileId))) Then
            ' The file is not yet in the database
            lobjCommand.CommandText =
              String.Format("INSERT INTO [{0}] ([JobId], [Name], [Data]) VALUES (@JobId, @Name, @Data)", TABLE_FILES_NAME)
          Else
            ' The file is already there, we need to replace it
            lobjCommand.CommandText =
              String.Format("UPDATE [{0}] SET [Data] = @Data WHERE JobId = @JobId AND Name = @Name", TABLE_FILES_NAME)
          End If

          lobjCommand.CommandType = CommandType.Text

          ' Add the job id
          lobjParameter = New SqlParameter("@JobId", SqlDbType.NVarChar, 255) With {
            .Value = lpJobId
          }
          lobjCommand.Parameters.Add(lobjParameter)

          ' Add the file name 
          lobjParameter = New SqlParameter("@Name", SqlDbType.NVarChar, 100) With {
            .Value = lpFileName
          }
          lobjCommand.Parameters.Add(lobjParameter)

          ' Add the file data 
          lobjParameter = New SqlParameter("@Data", SqlDbType.VarBinary) With {
            .Value = lpFileData
          }
          lobjCommand.Parameters.Add(lobjParameter)

          lobjCommand.Transaction = lobjConnection.BeginTransaction
          lobjCommand.ExecuteNonQuery()
          lobjCommand.Transaction.Commit()

          lobjConnection.Close()

        End Using
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetSqlServerVersion() As SqlServerVersion
    Try

      Using lobjConnection As New SqlConnection(ServerConnectionString)
        lobjConnection.Open()
        Dim lstrServerVersion As String = lobjConnection.ServerVersion
        lobjConnection.Close()

        Dim lstrServersionDetails As String() = lstrServerVersion.Split(".")
        Dim lintVersionNumber As Integer = Integer.Parse(lstrServersionDetails(0))
        Return lintVersionNumber
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetFileStreamLevel() As FileStreamAccessLevel
    Try
      If SqlVersion < SqlServerVersion.SQLServer2008 Then
        Return FileStreamAccessLevel.Disabled
      Else
        Return ExecuteSimpleQuery("SELECT SERVERPROPERTY ('FilestreamEffectiveLevel')", ServerConnectionString)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return FileStreamAccessLevel.Disabled
    End Try
  End Function

  Private Function GetPrimaryFileStreamDataFile() As String
    Try
      Dim lobjSQLBuilder As New StringBuilder

      Dim lobjFileName As Object =
            ExecuteSimpleQuery("SELECT name FROM sys.database_files WHERE type_desc = 'FILESTREAM'")

      If lobjFileName Is Nothing Then
        Return String.Empty
      ElseIf TypeOf lobjFileName Is DBNull Then
        Return String.Empty
      Else
        Return lobjFileName.ToString
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetWorkSummaryCountsForProjectDB(ByVal lpProjectId As String) As WorkSummary

    Dim lobjProjectWorkSummary As WorkSummary = Nothing

    Dim ldblProcessingRate As Double
    Dim ldblPeakProcessingRate As Double
    Dim ldatStartTime As DateTime = DateTime.MinValue
    Dim ldatFinishTime As DateTime = DateTime.MinValue
    Dim ldatLastUpdateTime As DateTime

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_get_project_summary_counts", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjIdParameter As New SqlParameter("@projectId", SqlDbType.NVarChar, 255) With {
            .Value = lpProjectId
          }
          lobjCommand.Parameters.Add(lobjIdParameter)
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                'Should only be 1 row

                Dim ldblAvgProcessingTime As Double = 0

                If (IsDBNull(lobjDataReader("AvgProcessingTime")) = False) Then
                  ldblAvgProcessingTime = Convert.ToDouble(lobjDataReader("AvgProcessingTime"))
                End If


                If (IsDBNull(lobjDataReader("ProcessingRate")) = False) Then
                  ldblProcessingRate = Convert.ToDouble(lobjDataReader("ProcessingRate"))
                End If

                If (IsDBNull(lobjDataReader("PeakProcessingRate")) = False) Then
                  ldblPeakProcessingRate = Convert.ToDouble(lobjDataReader("PeakProcessingRate"))
                End If

                'If (IsDBNull(lobjDataReader("StartTime")) = False) Then
                '  ldatStartTime = Convert.ToDateTime(lobjDataReader("StartTime"))
                'End If

                'If (IsDBNull(lobjDataReader("FinishTime")) = False) Then
                '  ldatFinishTime = Convert.ToDateTime(lobjDataReader("FinishTime"))
                'End If

                If (IsDBNull(lobjDataReader("LastUpdateTime")) = False) Then
                  ldatLastUpdateTime = Convert.ToDateTime(lobjDataReader("LastUpdateTime"))
                End If

                lobjProjectWorkSummary = New WorkSummary("Project Totals",
                                                         String.Empty,
                                                         Convert.ToInt32(lobjDataReader("NotProcessedCount")),
                                                         Convert.ToInt32(lobjDataReader("SuccessCount")),
                                                         Convert.ToInt32(lobjDataReader("FailedCount")),
                                                         Convert.ToInt32(lobjDataReader("ProcessingCount")),
                                                         Convert.ToInt32(lobjDataReader("TotalItemCount")),
                                                         ldblAvgProcessingTime,
                                                         ldatStartTime,
                                                         ldatFinishTime,
                                                         ldatLastUpdateTime,
                                                         ldblProcessingRate,
                                                         ldblPeakProcessingRate)

              End While

              lobjDataReader.Close()

            End If

            If lobjProjectWorkSummary IsNot Nothing Then
              UpdateCachedProjectWorkSummaryDB(lpProjectId, lobjProjectWorkSummary.ToSQLTotal)
            End If
          End Using
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try

    Return lobjProjectWorkSummary
  End Function

  Private Function GetWorkSummaryCountsDB(ByVal lpJob As Job) As WorkSummary

    Dim lobjBatchSummary As WorkSummary = Nothing

    Try

      'LogSession.EnterMethod(Level.Debug, Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpJob)
#Else
          If lpJob Is Nothing Then
            Throw New ArgumentNullException(NameOf(lpJob))
          End If
#End If

      If String.IsNullOrEmpty(lpJob.Id) Then
        Throw New InvalidOperationException("The job id is not available.")
      End If

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_get_job_summary_counts", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          Dim lobjIdParameter As New SqlParameter("@jobId", SqlDbType.NVarChar, 255) With {
            .Value = lpJob.Id
          }
          lobjCommand.Parameters.Add(lobjIdParameter)
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                'Should only be 1 row

                Dim ldblAvgProcessingTime As Double = 0
                Dim ldblProcessingRate As Double = 0
                Dim ldblPeakProcessingRate As Double = 0
                Dim ldatStartTime As DateTime
                Dim ldatFinishTime As DateTime
                Dim ldatLastUpdateTime As DateTime

                If (IsDBNull(lobjDataReader("AvgProcessingTime")) = False) Then
                  ldblAvgProcessingTime = Convert.ToDouble(lobjDataReader("AvgProcessingTime"))
                End If

                If (IsDBNull(lobjDataReader("ProcessingRate")) = False) Then
                  ldblProcessingRate = Convert.ToDouble(lobjDataReader("ProcessingRate"))
                End If

                If (IsDBNull(lobjDataReader("PeakProcessingRate")) = False) Then
                  ldblPeakProcessingRate = Convert.ToDouble(lobjDataReader("PeakProcessingRate"))
                End If

                If (IsDBNull(lobjDataReader("StartTime")) = False) Then
                  ldatStartTime = Convert.ToDateTime(lobjDataReader("StartTime"))
                End If

                If (IsDBNull(lobjDataReader("FinishTime")) = False) Then
                  ldatFinishTime = Convert.ToDateTime(lobjDataReader("FinishTime"))
                End If

                If (IsDBNull(lobjDataReader("LastUpdateTime")) = False) Then
                  ldatLastUpdateTime = Convert.ToDateTime(lobjDataReader("LastUpdateTime"))
                End If

                lobjBatchSummary = New WorkSummary(lpJob, Convert.ToInt64(lobjDataReader("NotProcessedCount")),
                                                   Convert.ToInt64(lobjDataReader("SuccessCount")),
                                                   Convert.ToInt64(lobjDataReader("FailedCount")),
                                                   Convert.ToInt32(lobjDataReader("ProcessingCount")),
                                                   Convert.ToInt64(lobjDataReader("TotalItemCount")),
                                                   ldblAvgProcessingTime, ldatStartTime, ldatFinishTime,
                                                   ldatLastUpdateTime, ldblProcessingRate, ldblPeakProcessingRate)

              End While

              lobjDataReader.Close()

            End If

          End Using
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
    Finally
      'LogSession.LeaveMethod(Level.Debug, Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

    Return lobjBatchSummary
  End Function

  Private Function ResetFailedItemsByProcessedMessageDB(lpJob As Job, lpProcessedMessage As String) As Integer
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpJob)
#Else
          If lpJob Is Nothing Then
            Throw New ArgumentNullException(NameOf(lpJob))
          End If
#End If

      If String.IsNullOrEmpty(lpProcessedMessage) Then
        Throw New ArgumentNullException(NameOf(lpProcessedMessage))
      End If

      Dim lstrJobViewName As String = GenerateJobViewName(lpJob.Name)
      Dim lstrSql As String =
            String.Format(
              "UPDATE JV SET ProcessedStatus = 'NotProcessed' FROM [{0}] JV WHERE ProcessedStatus = 'Failed' AND ProcessedMessage = '{1}'",
              lstrJobViewName, lpProcessedMessage.Replace("'", "''"))
      'LogSession.LogMessage("About to reset items using SQL ({0}).", lstrSql)
      Dim lintRecordsAffected As Integer

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Helper.HandleConnection(lobjConnection)

        Using lobjCommand As New SqlCommand(lstrSql, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT

          lobjCommand.CommandType = CommandType.Text
          lintRecordsAffected = lobjCommand.ExecuteNonQuery()
        End Using
      End Using

      If lintRecordsAffected > 0 Then
        'LogSession.LogMessage("Reset {0} items as not processed for job '{1}' where the processed message was '{2}'.",
        'lintRecordsAffected, lpJob.Name, lpProcessedMessage)
      Else
        'LogSession.LogWarning("Reset 0 items as not processed for job '{0}' where the processed message was '{1}'.",
        'lpJob.Name, lpProcessedMessage)
      End If

      Return lintRecordsAffected

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function GetFailureItemIdsByProcessedMessageDB(lpJob As Job, lpProcessedMessage As String) As List(Of String)
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpJob)
#Else
          If lpJob Is Nothing Then
            Throw New ArgumentNullException(NameOf(lpJob))
          End If
#End If

      If String.IsNullOrEmpty(lpProcessedMessage) Then
        Throw New ArgumentNullException(NameOf(lpProcessedMessage))
      End If

      Dim lstrJobViewName As String = GenerateJobViewName(lpJob.Name)
      Dim lstrSql As String =
            String.Format("SELECT ID FROM [{0}] WHERE ProcessedStatus = 'Failed' AND ProcessedMessage = '{1}'",
                          lstrJobViewName, lpProcessedMessage)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetFailureSummariesDB(ByVal lpParent As Object) As FailureSummaries

    Dim lobjFailureSummaries As New FailureSummaries

    Try

      Dim lstrSqlBuilder As New StringBuilder()

      ' SELECT  J.JobName, 
      ' BI.ProcessedStatus AS Status, 
      ' CONVERT(NVARCHAR(1000), BI.ProcessedMessage) AS Message, 
      ' COUNT(CONVERT(NVARCHAR(1000), BI.ProcessedMessage)) AS MessageCount

      ' FROM tblBatchItems AS BI INNER JOIN
      ' tblJobBatchRel AS JBR ON BI.BatchId = JBR.BatchId INNER JOIN
      ' tblJob AS J ON JBR.JobId = J.JobId
      ' WHERE (J.JobName = N'SourceNet QA Test') AND (BI.ProcessedStatus = 'Failed')
      ' GROUP BY J.JobName, BI.ProcessedStatus, CONVERT(NVARCHAR(1000), BI.ProcessedMessage)
      ' ORDER BY MessageCount DESC

      lstrSqlBuilder.Append("SELECT J.JobName, CONVERT(NVARCHAR(1000), BI.ProcessedMessage) AS Message, ")
      lstrSqlBuilder.Append("COUNT(CONVERT(NVARCHAR(1000), BI.ProcessedMessage)) AS MessageCount ")
      lstrSqlBuilder.Append("FROM tblBatchItems AS BI INNER JOIN ")
      lstrSqlBuilder.Append("tblJobBatchRel AS JBR ON BI.BatchId = JBR.BatchId INNER JOIN ")
      lstrSqlBuilder.Append("tblJob AS J ON JBR.JobId = J.JobId ")
      If TypeOf (lpParent) Is Project Then
        lstrSqlBuilder.Append("WHERE BI.ProcessedStatus = 'Failed' ")
      ElseIf TypeOf (lpParent) Is Job Then
        lstrSqlBuilder.AppendFormat("WHERE (J.JobName = N'{0}') AND (BI.ProcessedStatus = 'Failed') ", lpParent.Name)
      ElseIf TypeOf (lpParent) Is Batch Then
        lstrSqlBuilder.AppendFormat("WHERE (J.JobName = N'{0}') AND (BI.ProcessedStatus = 'Failed') ", lpParent.Job.Name)
      End If

      lstrSqlBuilder.Append("GROUP BY J.JobName, CONVERT(NVARCHAR(1000), BI.ProcessedMessage) ")
      lstrSqlBuilder.Append("ORDER BY J.JobName, MessageCount DESC")

      Dim lstrSql As String = lstrSqlBuilder.ToString

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Using lobjCommand As New SqlCommand(lstrSql, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              Dim lstrJobName As String
              Dim lstrMessage As String
              Dim lintMessageCount As Integer

              While lobjDataReader.Read
                'Should only be 1 row
                If Not IsDBNull(lobjDataReader("JobName")) Then
                  lstrJobName = lobjDataReader("JobName")
                Else
                  lstrJobName = String.Empty
                End If
                If Not IsDBNull(lobjDataReader("Message")) Then
                  lstrMessage = lobjDataReader("Message")
                Else
                  lstrMessage = String.Empty
                End If
                If Not IsDBNull(lobjDataReader("MessageCount")) Then
                  lintMessageCount = lobjDataReader("MessageCount")
                Else
                  lintMessageCount = 0
                End If
                lobjFailureSummaries.Add(New FailureSummary(lstrJobName, lstrMessage, lintMessageCount))

              End While

              lobjDataReader.Close()

            End If
          End Using
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
    Return lobjFailureSummaries
  End Function

  'Private Function DBNullToDouble(ByVal lpDoubleValue As Object) As Double

  '  Dim ldblDouble As Double = 0

  '  Try

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

  '  End Try
  'End Function

  ''' <summary>
  ''' Delete most all items.  Leave tblaudit and tblversion alone
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub CleanEntireDB()

    Try
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrSQL As String = String.Format("Delete FROM {0}", TABLE_BATCH_ITEM_NAME)
        Dim cmdDelete As New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)

        Dim lintNewRecordAff As Integer = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0}", TABLE_BATCH_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0}", TABLE_JOB_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0}", TABLE_PROJECTJOBREL_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0}", TABLE_JOBBATCHREL_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0}", TABLE_PROJECT_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0}", TABLE_BATCHLOCK_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        'Insert an audit record - TODO:make it's own method and add to all delete calls
        lstrSQL = String.Format("INSERT INTO {0}(AuditAction,Comments) VALUES('Database Clean','Database Clean')",
                                TABLE_AUDIT_NAME)
        cmdDelete = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }

        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try
  End Sub

  Private Sub CreateDatabaseAndTables(lpDatabaseType As DatabaseType)

    Try

      CopyScriptsToCTSTemp()
      CreateDatabase(lpDatabaseType)
      CreateTables(lpDatabaseType)
      CreateStoredProcedures(lpDatabaseType)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally
      'RemoveScripts(lpScriptPath)
    End Try
  End Sub

  Private Sub InitializeBlankDatabase(lpDatabaseType As DatabaseType)

    Try

      CopyScriptsToCTSTemp()
      CreateTables(lpDatabaseType)
      CreateStoredProcedures(lpDatabaseType)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally
      'RemoveScripts(lpScriptPath)
    End Try
  End Sub

  Private Shared Sub RemoveScripts(ByVal lpScriptPath As String)

    Try

      If (lpScriptPath.Length > 0) Then

        RemoveScript(String.Format("{0}{1}", lpScriptPath, PROJECT_DATABASE_SCRIPT_FILENAME))
        RemoveScript(String.Format("{0}{1}", lpScriptPath, TABLE_SCRIPT_FILENAME))
        RemoveScript(String.Format("{0}{1}", lpScriptPath, DELTA_TABLE_SCRIPT_FILENAME))

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
    End Try
  End Sub

  Private Shared Sub RemoveScript(ByVal lstrScriptFilePath As String)

    Try

      If Not String.IsNullOrEmpty(lstrScriptFilePath) Then

        If (File.Exists(lstrScriptFilePath)) Then
          File.Delete(lstrScriptFilePath)
        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' If the scripts don't exist, then copy then out
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub CopyScriptsToCTSTemp()

    Try

      'Dim objStream As Stream = Nothing
      'Dim objFileStream As FileStream = Nothing

      'Dim abytResource As Byte()
      'Dim objAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()


      ' Copy over the scripts for creating the database
      CopyScriptToCTSTemp(PROJECT_DATABASE_SCRIPT_FILENAME)
      CopyScriptToCTSTemp(PROJECT_DATABASE_WITH_FILESTREAM_SCRIPT_FILENAME)

      'If (Not File.Exists(lpScriptPath & DATABASE_SCRIPT_FILENAME)) Then
      '  'Dim lstrStrings As String() = objAssembly.GetManifestResourceNames()
      '  objStream = objAssembly.GetManifestResourceStream("Ecmg.Cts.Projects.ContentManagerCreateDatabase.sql")
      '  abytResource = New [Byte](objStream.Length - 1) {}
      '  objStream.Read(abytResource, 0, objStream.Length)
      '  objFileStream = New FileStream(lpScriptPath & DATABASE_SCRIPT_FILENAME, FileMode.Create)
      '  objFileStream.Write(abytResource, 0, objStream.Length)
      '  objFileStream.Close()
      'End If

      ' Copy over the script for creating the standard tables
      CopyScriptToCTSTemp(TABLE_SCRIPT_FILENAME)
      'If (Not File.Exists(lpScriptPath & TABLE_SCRIPT_FILENAME)) Then
      '  objStream = objAssembly.GetManifestResourceStream("Ecmg.Cts.Projects.ContentManagerCreateTables.sql")
      '  abytResource = New [Byte](objStream.Length - 1) {}
      '  objStream.Read(abytResource, 0, objStream.Length)
      '  objFileStream = New FileStream(lpScriptPath & TABLE_SCRIPT_FILENAME, FileMode.Create)
      '  objFileStream.Write(abytResource, 0, objStream.Length)
      '  objFileStream.Close()
      'End If

      ' Added additional script to read db permissions
      CopyScriptToCTSTemp(PERMISSIONS_SCRIPT_FILENAME)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub CopyScriptToCTSTemp(ByVal lpScriptName As String)

    Try

      Dim lobjStream As Stream = Nothing
      Dim lobjFileStream As FileStream = Nothing
      Dim lstrScriptFilePath As String = String.Format("{0}{1}", ScriptPath, lpScriptName)
      Dim lstrAssemblyName As String = String.Format("Ecmg.Cts.Projects.{0}", lpScriptName)
      Dim labytResource As Byte()
      Dim lobjAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()

      If File.Exists(lstrScriptFilePath) Then
        File.Delete(lstrScriptFilePath)
      End If

      lobjStream = lobjAssembly.GetManifestResourceStream(lstrAssemblyName)
      labytResource = New [Byte](lobjStream.Length - 1) {}
      lobjStream.Read(labytResource, 0, lobjStream.Length)
      lobjFileStream = New FileStream(lstrScriptFilePath, FileMode.Create)
      lobjFileStream.Write(labytResource, 0, lobjStream.Length)
      lobjFileStream.Close()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Function CopyScriptToString(ByVal lpScriptName As String) As String

    Try

      'Dim lobjStream As Stream = Nothing
      'Dim lstrAssemblyName As String = String.Format("Ecmg.Cts.Projects.{0}", lpScriptName)
      'Dim lobjAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
      Dim lstrResourceString As String = Nothing

      'lobjStream = lobjAssembly.GetManifestResourceStream(lstrAssemblyName)

      'If lobjStream.CanSeek Then
      '  lobjStream.Position = 0
      'End If

      Dim lobjStream As Stream = Helper.GetResourceFileFromAssembly(lpScriptName,
                                                                    System.Reflection.Assembly.GetExecutingAssembly())

      Using lobjStreamReader As New StreamReader(lobjStream)
        lstrResourceString = lobjStreamReader.ReadToEnd
        lobjStreamReader.Close()
      End Using

      Return lstrResourceString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub CreateDatabase(lpDatabaseType As DatabaseType)

    Try

      'If (Not lpScriptPath.EndsWith("\")) Then
      '  lpScriptPath &= "\"
      'End If

      'This will allow you to see where the data directory exists for the installed SQL - you must have sufficient priveleges to execute this sp
      'DECLARE @retvalue int, @data_dir varchar(500)
      'EXECUTE @retvalue = master.dbo.xp_instance_regread 'HKEY_LOCAL_MACHINE','SOFTWARE\Microsoft\MSSQLServer\Setup','SQLDataRoot', @param = @data_dir OUTPUT
      'Print() 'SQL Server Data Path: '+ @data_dir 

      Dim oleConn As SqlConnection = Nothing
      Dim lstrMasterCatalogConnectionString As String = String.Empty

      Try

        Select Case lpDatabaseType
          Case DatabaseType.Project
            oleConn = New SqlConnection(MasterDatabaseConnection)
          Case DatabaseType.Catalog
            lstrMasterCatalogConnectionString = GetMasterConnectionString(ProjectCatalog.CurrentConnectionString)
            oleConn = New SqlConnection(lstrMasterCatalogConnectionString)
        End Select

        oleConn.Open()

        Dim oleCommand As New SqlCommand With {
          .CommandTimeout = COMMAND_TIMEOUT,
          .CommandText = "dbo.xp_instance_regread",
          .CommandType = CommandType.StoredProcedure,
          .Connection = oleConn
        }

        Dim paramReturn As New SqlParameter("RETURN_VALUE", SqlDbType.VarChar, 1024) With {
          .Direction = ParameterDirection.ReturnValue
        }

        Dim paramJobItemID As New SqlParameter("", "HKEY_LOCAL_MACHINE")
        Dim paramStatus As New SqlParameter("", "SOFTWARE\Microsoft\MSSQLServer\Setup")
        Dim param3 As New SqlParameter("", "SQLDataRoot")
        Dim param4 As New SqlParameter("@param", SqlDbType.VarChar, 1024) With {
          .Direction = ParameterDirection.Output
        }

        oleCommand.Parameters.Add(paramReturn)
        oleCommand.Parameters.Add(paramJobItemID)
        oleCommand.Parameters.Add(paramStatus)
        oleCommand.Parameters.Add(param3)
        oleCommand.Parameters.Add(param4)

        oleCommand.ExecuteNonQuery()

        If (param4.Value.ToString.Length > 0) Then
          mstrSQLDataDir = param4.Value
        End If

        'Console.WriteLine("RETURN_VALUE:" + param4.Value)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

      Finally

        If (oleConn.State = ConnectionState.Open) Then
          oleConn.Close()
        End If

      End Try

      ' ExecuteSQLScript(ScriptPath & DATABASE_SCRIPT_FILENAME, MasterDatabaseConnection)

      Select Case lpDatabaseType
        Case DatabaseType.Project
          If FileStreamLevel > FileStreamAccessLevel.Disabled Then
            ExecuteSQLScript(PROJECT_DATABASE_WITH_FILESTREAM_SCRIPT_FILENAME, MasterDatabaseConnection)
          Else
            ExecuteSQLScript(PROJECT_DATABASE_SCRIPT_FILENAME, MasterDatabaseConnection)
          End If
        Case DatabaseType.Catalog
          ExecuteSQLScript(CATALOG_DATABASE_SCRIPT_FILENAME, lstrMasterCatalogConnectionString,
                           ProjectCatalog.CurrentDatabaseName)
      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Private Sub CreateStoredProcedures(lpDatabaseType As DatabaseType)
  '  Try

  '    Select Case lpDatabaseType
  '      Case DatabaseType.Project
  '        ExecuteSQLScript(STORED_PROCEDURE_SCRIPT_FILENAME, Me.ItemsLocation.Location)
  '      Case DatabaseType.Catalog
  '        ExecuteSQLScript(TABLE_SCRIPT_FILENAME, ProjectCatalog.CurrentConnectionString, ProjectCatalog.CurrentDatabaseName)
  '    End Select

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Private Sub CreateTables(lpDatabaseType As DatabaseType)

    Try

      Select Case lpDatabaseType
        Case DatabaseType.Project
          ExecuteSQLScript(TABLE_SCRIPT_FILENAME, Me.ItemsLocation.Location)
          CreateFileTable()
        Case DatabaseType.Catalog
          ExecuteSQLScript(CATALOG_CREATE_TABLES_SCRIPT_FILENAME, ProjectCatalog.CurrentConnectionString,
                           ProjectCatalog.CurrentDatabaseName)
      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub CreateStoredProcedures(lpDatabaseType As DatabaseType)
    Try
      Select Case lpDatabaseType
        Case DatabaseType.Project
          ExecuteSQLScript(STORED_PROCEDURE_SCRIPT_FILENAME, Me.ItemsLocation.Location)
        Case DatabaseType.Catalog
          ExecuteSQLScript(CATALOG_CREATE_STORED_PROCEDURES_SCRIPT_FILENAME, ProjectCatalog.CurrentConnectionString,
                           ProjectCatalog.CurrentDatabaseName)
      End Select
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function DoesTableExist(lpTableName As String, lpDatabaseType As DatabaseType) As Boolean
    Try
      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.AppendFormat(
        "SELECT CASE WHEN EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) THEN 1 ELSE 0 END",
        lpTableName)

      Dim lintTableExists As Integer
      Select Case lpDatabaseType
        Case DatabaseType.Project
          lintTableExists = ExecuteSimpleQuery(lobjSqlBuilder.ToString)
        Case DatabaseType.Catalog
          lintTableExists = ExecuteSimpleQuery(lobjSqlBuilder.ToString, ProjectCatalog.CurrentConnectionString)
      End Select

      If lintTableExists > 0 Then
        Return True
      Else
        Return False
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function


  Private Function DoesFileTableExist() As Boolean
    Try
      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.AppendFormat(
        "SELECT CASE WHEN EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) THEN 1 ELSE 0 END",
        TABLE_FILES_NAME)

      Dim lintTableExists As Integer = ExecuteSimpleQuery(lobjSqlBuilder.ToString)

      Return Boolean.Parse(lintTableExists)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub CreateFileTable()
    Try
      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.AppendLine(
        "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblFiles]') AND type in (N'U'))")
      lobjSqlBuilder.AppendLine("BEGIN")
      lobjSqlBuilder.AppendLine("CREATE TABLE [tblFiles] (")
      lobjSqlBuilder.AppendLine("[FileId] uniqueidentifier ROWGUIDCOL NOT NULL DEFAULT (NEWID()) PRIMARY KEY, ")
      lobjSqlBuilder.AppendLine("[JobId]  nvarchar(255), ")
      lobjSqlBuilder.AppendLine("[Name]   nvarchar(100)     NOT NULL, ")

      Dim lstrPrimaryFileStreamTable As String = GetPrimaryFileStreamDataFile()

      If _
        ((FileStreamLevel > FileStreamAccessLevel.Disabled) AndAlso
         (Not String.IsNullOrEmpty(lstrPrimaryFileStreamTable))) Then
        lobjSqlBuilder.AppendLine("[Data] varbinary(max)    FILESTREAM NOT NULL)")
      Else
        lobjSqlBuilder.AppendLine("[Data] varbinary(max)    NOT NULL)")
      End If

      lobjSqlBuilder.AppendLine("END")

      ExecuteNonQuery(lobjSqlBuilder.ToString)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub CreateJobRelationshipTables()
    Try
      ExecuteNonQuery(GenerateJobRelationshipsCreateTableStatement())
      ExecuteNonQuery(GenerateRelatedJobsCreateTableStatement())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Function GenerateJobRelationshipsCreateTableStatement() As String
    Try
      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.AppendLine(
        "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblJobRelationships]') AND type in (N'U'))")
      lobjSqlBuilder.AppendLine("BEGIN")
      lobjSqlBuilder.AppendLine("CREATE TABLE [tblJobRelationships] (")
      lobjSqlBuilder.AppendLine("[JobRelationshipId] [nvarchar](255) NOT NULL , ")
      lobjSqlBuilder.AppendLine("[JobRelationshipName] [nvarchar](255) NOT NULL , ")
      lobjSqlBuilder.AppendLine("[JobRelationshipDescription] [nvarchar](255) NOT NULL , ")
      lobjSqlBuilder.AppendLine("CONSTRAINT [PK_tblJobRelationships] PRIMARY KEY CLUSTERED ")

      lobjSqlBuilder.AppendLine(" ( [JobRelationshipId] ASC ) ")
      lobjSqlBuilder.AppendLine(" WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, ")
      lobjSqlBuilder.AppendLine("        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ")
      lobjSqlBuilder.AppendLine("        ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]) ON [PRIMARY] END GO")

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function GenerateRelatedJobsCreateTableStatement() As String
    Try
      Dim lobjSqlBuilder As New StringBuilder

      lobjSqlBuilder.AppendLine(
        "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblRelatedJobs]') AND type in (N'U'))")
      lobjSqlBuilder.AppendLine("BEGIN")
      lobjSqlBuilder.AppendLine("CREATE TABLE [tblRelatedJobs] (")
      lobjSqlBuilder.AppendLine("[JobRelationshipId] [nvarchar](255) NOT NULL , ")
      lobjSqlBuilder.AppendLine("[JobId] [nvarchar](255) NOT NULL , ")
      lobjSqlBuilder.AppendLine("CONSTRAINT [aaaaatblRelatedJobs_PK] PRIMARY KEY NONCLUSTERED ")

      lobjSqlBuilder.AppendLine(" ( [JobRelationshipId] ASC, [JobId] ASC ) ")
      lobjSqlBuilder.AppendLine(" WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, ")
      lobjSqlBuilder.AppendLine("        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ")
      lobjSqlBuilder.AppendLine("        ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]) ON [PRIMARY] END GO")

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub ExecuteSQLScript(ByVal Filename As String,
                               ByVal lpDBConnectionString As String)
    Try
      ExecuteSQLScript(Filename, lpDBConnectionString, String.Empty)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Executes a .sql script
  ''' </summary>
  ''' <param name="Filename"></param>
  ''' <remarks></remarks>
  Private Sub ExecuteSQLScript(ByVal Filename As String,
                               ByVal lpDBConnectionString As String,
                               ByVal lpDatabaseName As String)


    Dim lstrIterationSQL As String = String.Empty

    Try
      Using lobjConnection As New SqlConnection(lpDBConnectionString)
        Using cmd As New SqlCommand
          cmd.CommandType = CommandType.Text
          cmd.Connection = lobjConnection
          Helper.HandleConnection(lobjConnection)

          Dim s As String = Helper.GetResourceFileTextFromAssembly(Filename, Assembly.GetExecutingAssembly)

          s = Replace(s, "GO", "~") 'Replace GO with a "~". Split only works with char

          Dim delimiter() As Char = "~".ToCharArray
          Dim SQL() As String = s.Split(delimiter) 'Now split the different SQL statements into an array

          If String.IsNullOrEmpty(lpDatabaseName) Then
            lpDatabaseName = Me.ItemsLocation.DatabaseName
          End If

          For I As Integer = 0 To UBound(SQL) 'Loop through array, executing each statement separately
            lstrIterationSQL = SQL(I)
            cmd.CommandText = lstrIterationSQL

            cmd.CommandText = cmd.CommandText.Replace("<DBNAME>", lpDatabaseName)
            cmd.CommandText = cmd.CommandText.Replace("<SQLDATADIR>", mstrSQLDataDir)

            If Not String.IsNullOrEmpty(cmd.CommandText) Then
              cmd.ExecuteNonQuery()
            End If

          Next

        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod, lpDBConnectionString, lstrIterationSQL)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

  Private Shared Function DoesColumnExist(lpTableName As String, lpColumnName As String, lpConnectionString As String) _
    As Boolean
    Try

      Dim lstrSQL As String =
            String.Format("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}'",
                          lpTableName, lpColumnName)

      Dim lblnReturn As Boolean = False
      Using lobjConnection As New SqlConnection(lpConnectionString)
        lobjConnection.Open()
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Using lobjReader As SqlDataReader = lobjCommand.ExecuteReader
            lblnReturn = lobjReader.HasRows
          End Using
        End Using
      End Using

      Return lblnReturn

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function DoesDBExist(ByVal lpDBName As String, ByVal lpMasterDatabaseConnection As String) As Boolean

    Try

      ' Make sure we were given a database name
#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpDBName)
#Else
          If lpDBName Is Nothing Then
            Throw New ArgumentNullException(NameOf(lpDBName))
          End If
#End If

      Dim lstrSQL As String = "select * from master.dbo.sysdatabases where name='" & lpDBName & "'"

      Dim bRet As Boolean = False
      If String.IsNullOrEmpty(lpMasterDatabaseConnection) Then
        lpMasterDatabaseConnection = MasterDatabaseConnection
      End If
      Using lobjConnection As New SqlConnection(lpMasterDatabaseConnection)
        Try
          lobjConnection.Open()
        Catch SqlEx As SqlException
          ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod)
          Dim lobjSqlConnectionBuilder As New SqlConnectionStringBuilder(lpMasterDatabaseConnection)
          Throw New ServerUnavailableException(String.Format("Check the database server name: {0}", SqlEx.Message),
                                               lobjSqlConnectionBuilder.DataSource)
        End Try

        Using sqlCmd As New SqlCommand(lstrSQL, lobjConnection)
          sqlCmd.CommandTimeout = COMMAND_TIMEOUT
          Using reader As SqlDataReader = sqlCmd.ExecuteReader
            bRet = reader.HasRows
          End Using
        End Using
      End Using
      Return bRet

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  ''' Not tested, not used right now
  ''' </summary>
  ''' <param name="lpDatabaseName"></param>
  ''' <remarks></remarks>
  Private Sub CreateDatabaseBySQLScript(ByVal lpDatabaseName As String)


    Try
      Using lobjConnection As New SqlConnection(MasterDatabaseConnection)
        Helper.HandleConnection(lobjConnection)
        Using lobjCommand As New SqlCommand

          lobjCommand.CommandType = CommandType.Text
          lobjCommand.Connection = lobjConnection

          lobjCommand.CommandText = "CREATE DATABASE [" & lpDatabaseName & "] ON  PRIMARY " &
                                    "( NAME = N'ContentManager', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\" &
                                    lpDatabaseName &
                                    ".mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )" & "LOG ON " &
                                    "( NAME = N'ContentManager_log', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\" &
                                    lpDatabaseName & ".ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)" &
                                    "COLLATE SQL_Latin1_General_CP1_CI_AS"
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "EXEC dbo.sp_dbcmptlevel @dbname=N'" & lpDatabaseName & "', @new_cmptlevel=90"
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))" & " begin " & " EXEC [" &
                                    lpDatabaseName & "].[dbo].[sp_fulltext_database] @action = 'enable' " & " end "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET ANSI_NULL_DEFAULT OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET ANSI_NULLS OFF  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET ANSI_PADDING OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET ANSI_WARNINGS OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET ARITHABORT OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET AUTO_CLOSE OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET AUTO_CREATE_STATISTICS ON "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET AUTO_SHRINK OFF  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET AUTO_UPDATE_STATISTICS ON  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET CURSOR_CLOSE_ON_COMMIT OFF  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET CURSOR_DEFAULT  GLOBAL  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET CONCAT_NULL_YIELDS_NULL OFF  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET NUMERIC_ROUNDABORT OFF  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET QUOTED_IDENTIFIER OFF  "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET RECURSIVE_TRIGGERS OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET  ENABLE_BROKER "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET DATE_CORRELATION_OPTIMIZATION OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET TRUSTWORTHY OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET ALLOW_SNAPSHOT_ISOLATION OFF "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET PARAMETERIZATION SIMPLE "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET  READ_WRITE "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET RECOVERY SIMPLE "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET  MULTI_USER "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET PAGE_VERIFY CHECKSUM "
          lobjCommand.ExecuteNonQuery()

          lobjCommand.CommandText = "ALTER DATABASE [" & lpDatabaseName & "] SET DB_CHAINING OFF  "
          lobjCommand.ExecuteNonQuery()

        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    End Try
  End Sub

#End Region

#Region "Batch Insert"

  Private Function AddBatchItem(ByVal lpBatchItem As BatchItem) As Boolean

    Try

      Dim lintRowCount As Long = mobjDataTable.Rows.Count
      Dim lobjNewRow As DataRow = mobjDataTable.NewRow()
      lobjNewRow("BatchId") = lpBatchItem.BatchId
      lobjNewRow("Title") = lpBatchItem.Title
      lobjNewRow("SourceDocId") = lpBatchItem.SourceDocId
      lobjNewRow("ProcessedStatus") = ProcessedStatus.NotProcessed.ToString
      lobjNewRow("Operation") = lpBatchItem.Operation.ToString
      mobjDataTable.Rows.Add(lobjNewRow)

      If mobjDataTable.Rows.Count > lintRowCount Then
        Return True
      Else
        Return False
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return True
  End Function

  Private Sub CreateDataTable()

    mobjDataTable = New DataTable()

    Try
      mobjDataTable.Columns.Add("BatchId", String.Empty.GetType())
      mobjDataTable.Columns.Add("Title", String.Empty.GetType())
      mobjDataTable.Columns.Add("SourceDocId", String.Empty.GetType())
      mobjDataTable.Columns.Add("ProcessedStatus", String.Empty.GetType())
      mobjDataTable.Columns.Add("Operation", String.Empty.GetType())

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try
  End Sub

#End Region

#Region "Batch Update To Not Processed"

  Public Sub ResetItemsToNotProcessedDB(ByVal lpIdArrayList As ArrayList)

    Try

      If (lpIdArrayList.Count = 0) Then
        Exit Sub
      End If

      'Dim lobjDataTable As New DataTable
      'lobjDataTable.Columns.Add("Id", GetType(Integer))

      'For i As Integer = 0 To lpIdArrayList.Count - 1
      '  Dim lobjNewRow As DataRow = lobjDataTable.NewRow()
      '  lobjNewRow("Id") = lpIdArrayList(i)
      '  lobjDataTable.Rows.Add(lobjNewRow)
      'Next

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrSQL = String.Empty
        Dim lintRowsAffected As Integer = 0
        Dim lobjCommand As SqlCommand

        For i As Integer = 0 To lpIdArrayList.Count - 1
          ' <Modified by: Ernie at 11/26/2012-3:33:09 PM on machine: ERNIE-THINK>
          '	Updated to use a method for the base SQL creation to reduce redundant SQL generation."
          '	lstrSQL = "Update " & TABLE_BATCH_ITEM_NAME & " SET DestDocID = null, ProcessedStatus = '" & ProcessedStatus.NotProcessed.ToString & "', ProcessedMessage=null, ProcessStartTime = null,ProcessFinishTime = null, TotalProcessingTime=null,ProcessedBy=null,ProcessResult=null WHERE Id = " & lpIdArrayList(i)
          lstrSQL = String.Format("{0} WHERE Id = '{1}'",
                                  CreateResetItemsSQLBase, lpIdArrayList(i))
          ' </Modified by: Ernie at 11/26/2012-3:33:09 PM on machine: ERNIE-THINK>
          lobjCommand = New SqlCommand(lstrSQL, lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
          }
          Helper.HandleConnection(lobjConnection)
          lintRowsAffected = lobjCommand.ExecuteNonQuery()
        Next

        ' Force an update to the work summary to reflect the reset
        Dim lstrProjectId As String = GetCurrentProjectId()
        GetProjectSummaryCountsDB(lstrProjectId)

        'Dim lobjAdapter As New SqlDataAdapter()
        'lobjAdapter.UpdateCommand = New SqlCommand( _
        '  "Update " & TABLE_BATCH_ITEM_NAME & " SET DestDocID = null, ProcessedStatus = '" & ProcessedStatus.NotProcessed.ToString & "', ProcessedMessage=null, ProcessStartTime = null,ProcessFinishTime = null, TotalProcessingTime=null,ProcessedBy=null WHERE Id = @Id", lobjConnection)
        'lobjAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id")
        'lobjAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None

        '' Set the batch size.
        'lobjAdapter.UpdateBatchSize = 500 '0 = Let sql decide the max batchsize it will take

        '' Execute the Updates.
        'lobjAdapter.Update(lobjDataTable)

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Reset all the items in a batch from 'Failed' to 'Not Processed'
  ''' </summary>
  ''' <param name="lpBatch"></param>
  ''' <remarks></remarks>
  Public Sub ResetFailedItemsToNotProcessedDB(ByVal lpBatch As Batch)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lstrSQL = String.Empty
        Dim lintRowsAffected As Integer = 0
        Dim lobjCommand As SqlCommand

        ' <Modified by: Ernie at 11/26/2012-3:31:14 PM on machine: ERNIE-THINK>
        '	Updated to use a method for the base SQL creation to reduce redundant SQL generation."
        '	lstrSQL = "Update " & TABLE_BATCH_ITEM_NAME & " SET DestDocID = null, ProcessedStatus = '" & ProcessedStatus.NotProcessed.ToString & "', ProcessedMessage=null, ProcessStartTime = null,ProcessFinishTime = null, TotalProcessingTime=null,ProcessedBy=null WHERE BatchId = '" & lpBatch.Id & "' AND ProcessedStatus = '" & ProcessedStatus.Failed.ToString & "'"
        lstrSQL = String.Format("{0} WHERE BatchId = '{1}' AND ProcessedStatus = '{2}'",
                                CreateResetItemsSQLBase, lpBatch.Id, ProcessedStatus.Failed.ToString)
        ' </Modified by: Ernie at 11/26/2012-3:31:14 PM on machine: ERNIE-THINK>
        lobjCommand = New SqlCommand(lstrSQL, lobjConnection) With {
          .CommandTimeout = COMMAND_TIMEOUT
        }
        Helper.HandleConnection(lobjConnection)
        lintRowsAffected = lobjCommand.ExecuteNonQuery()

      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub ResetItemsToNotProcessedDB(lpBatch As Batch, lpCurrentStatus As ProcessedStatus)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_reset_batch_items", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Dim lobjIdParameter As New SqlParameter("@batchid", SqlDbType.NVarChar, 255) With {
            .Value = lpBatch.Id
          }
          lobjCommand.Parameters.Add(lobjIdParameter)

          Dim lobjProcessedStatusParameter As New SqlParameter("@currentprocessingstatus", SqlDbType.NVarChar, 25) With {
            .Value = lpCurrentStatus.ToString()
          }
          lobjCommand.Parameters.Add(lobjProcessedStatusParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New ItemDoesNotExistException(lpBatch.Id)
          End If
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try
  End Sub

  Private Sub ResetItemsToNotProcessedDB(lpJob As Job, lpCurrentStatus As ProcessedStatus)

    Try

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand("usp_reset_job_items", lobjConnection)
          lobjCommand.CommandType = CommandType.StoredProcedure
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Dim lobjIdParameter As New SqlParameter("@jobid", SqlDbType.NVarChar, 255) With {
            .Value = lpJob.Id
          }
          lobjCommand.Parameters.Add(lobjIdParameter)

          Dim lobjProcessedStatusParameter As New SqlParameter("@currentprocessingstatus", SqlDbType.NVarChar, 25) With {
            .Value = lpCurrentStatus.ToString()
          }
          lobjCommand.Parameters.Add(lobjProcessedStatusParameter)

          Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
            .Direction = ParameterDirection.ReturnValue
          }
          lobjCommand.Parameters.Add(lobjReturnParameter)

          Helper.HandleConnection(lobjConnection)
          Dim lintReturnValue As Integer
          lobjCommand.ExecuteNonQuery()
          lintReturnValue = lobjReturnParameter.Value
          If lintReturnValue = -100 Then
            Throw New ItemDoesNotExistException(lpJob.Id)
          End If
        End Using
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try
  End Sub

  Private Function GetUpdateTableDB(ByVal lpJob As Job,
                                    ByVal lpUpdateItems As DataTable) As DataTable

    Dim lobjReturnTable As DataTable '= Job.CreateNewSourceDataTable
    'Dim lobjReturnTable As New DataTable

    Try

      If lpUpdateItems.Columns.Count = 3 Then
        lobjReturnTable = Job.CreateNewSourceDataTable(True)
        ExecuteSQLScript(DELTA_TABLE_WITH_OPERATION_SCRIPT_FILENAME, Me.ItemsLocation.Location)
      Else
        lobjReturnTable = Job.CreateNewSourceDataTable()
        ExecuteSQLScript(DELTA_TABLE_SCRIPT_FILENAME, Me.ItemsLocation.Location)
      End If

      'Create the data table
      ''Dim lobjDataTable As New DataTable()
      ''lobjDataTable.Columns.Add("DocId", String.Empty.GetType())

      ' ''Add the rows to the data table
      ''For Each lstrID As String In lpUpdateItems.Keys
      ''  Dim lobjNewRow As DataRow = lobjDataTable.NewRow()
      ''  lobjNewRow("DocId") = lstrID
      ''  lobjDataTable.Rows.Add(lobjNewRow)
      ''Next
      ' CopyScriptToCTSTemp(DELTA_TABLE_SCRIPT_FILENAME)
      ' ExecuteSQLScript(ScriptPath & DELTA_TABLE_SCRIPT_FILENAME, Me.ItemsLocation.Location)
      'ExecuteSQLScript(DELTA_TABLE_SCRIPT_FILENAME, Me.ItemsLocation.Location)

      'Insert the rows into a temporary database table
      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)

        Dim lobjAdapter As New SqlDataAdapter()
        If lpUpdateItems.Columns.Count = 3 Then
          lobjAdapter.InsertCommand =
            New SqlCommand("INSERT INTO tmpDelta(DocId, Title, Operation) VALUES (@DocId, @Title, @Operation);",
                           lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
                           }
          lobjAdapter.InsertCommand.Parameters.Add("@DocId", SqlDbType.NVarChar, 255, "Id")
          lobjAdapter.InsertCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 255, "Title")
          lobjAdapter.InsertCommand.Parameters.Add("@Operation", SqlDbType.NVarChar, 255, "Operation")
        Else
          lobjAdapter.InsertCommand = New SqlCommand("INSERT INTO tmpDelta(DocId, Title) VALUES (@DocId, @Title);",
                                                     lobjConnection) With {
            .CommandTimeout = COMMAND_TIMEOUT
                                                     }
          lobjAdapter.InsertCommand.Parameters.Add("@DocId", SqlDbType.NVarChar, 255, "Id")
          lobjAdapter.InsertCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 255, "Title")
        End If

        lobjAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None

        ' Set the batch size.
        lobjAdapter.UpdateBatchSize = 500 '0 = Let sql decide the max batchsize it will take

        ' Execute the Insert.
        'lobjAdapter.Update(lobjDataTable)
        Dim lintRecordsUpdated As Integer = lobjAdapter.Update(lpUpdateItems)

        'Compare temp table with entire set of batch items for the job

        ' This is the SQL we need to run

        ' Select D.DocId
        ' FROM tmpDelta AS D LEFT JOIN
        '   (SELECT BI.SourceDocId
        '    FROM dbo.tblBatchItems AS BI INNER JOIN
        '      (SELECT BatchId
        '       FROM (dbo.tblJobBatchRel)
        '       WHERE (JobId = '92c75129-e7c0-477e-8c83-4dec6b70eee3')) AS JBR 
        '       ON BI.BatchId = JBR.BatchId) AS BI ON D.DocId = BI.SourceDocId 
        ' WHERE(BI.SourceDocId Is NULL)

        Dim lobjSQLBuilder As New StringBuilder

        If lpUpdateItems.Columns.Count = 3 Then
          lobjSQLBuilder.AppendLine("Select D.DocId AS Id, D.Title AS Title, D.Operation as Operation")
        Else
          lobjSQLBuilder.AppendLine("Select D.DocId AS Id, D.Title AS Title")
        End If

        lobjSQLBuilder.AppendLine("FROM tmpDelta AS D LEFT JOIN")
        lobjSQLBuilder.AppendLine("  (SELECT BI.SourceDocId")
        lobjSQLBuilder.AppendLine("   FROM dbo.tblBatchItems AS BI INNER JOIN")
        lobjSQLBuilder.AppendLine("     (SELECT BatchId")
        lobjSQLBuilder.AppendLine("      FROM dbo.tblJobBatchRel")
        lobjSQLBuilder.AppendLine(String.Format("      WHERE (JobId = '{0}')) AS JBR ", lpJob.Id))
        lobjSQLBuilder.AppendLine("      ON BI.BatchId = JBR.BatchId) AS BI ON D.DocId = BI.SourceDocId ")
        lobjSQLBuilder.AppendLine("WHERE(BI.SourceDocId Is NULL)")

        Dim lobjDA As New SqlDataAdapter With {
          .SelectCommand = New SqlCommand(lobjSQLBuilder.ToString, lobjConnection)
        }
        lobjDA.SelectCommand.CommandTimeout = COMMAND_TIMEOUT
        Helper.HandleConnection(lobjConnection)
        lobjDA.Fill(lobjReturnTable)
      End Using

      'Return the delta as a DataTable
      Return lobjReturnTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return lobjReturnTable
  End Function

#End Region

#Region "Repository Access Methods"

  Public Overloads Overrides Function DeleteRepository(lpRepository As Repository) As Boolean

    Try
      Return False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Overrides Function DeleteRepository(lpRepositoryName As String) As Boolean

    Try
      Return False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositoryByName(lpRepositoryName As String) As Repository

    Try
      Return GetRepositoryByNameDB(lpRepositoryName)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositoryByConnectionString(lpConnectionString As String) As Repository
    Try
      Return GetRepositoryByConnectionStringDB(lpConnectionString)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositories(lpJob As Job) As Repositories

    Try
      Return GetRepositoriesDB(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositories(lpProject As Project) As Repositories

    Try
      Return GetRepositoriesDB(lpProject)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function SaveRepository(lpRepository As Repository,
                                           ByVal lpJob As Job, ByVal lpScope As ExportScope) As Boolean

    Try
      SaveRepositoryToDB(lpRepository, lpJob.Id, lpScope, False)
      Return True
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      '' Re-throw the exception to the caller
      'Throw
      Return False
    End Try
  End Function

  Public Overrides Sub UpdateRepositories(lpJob As Job)
    Try
      UpdateRepositoriesDB(lpJob)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub UpdateRepositories(lpProject As Project)
    Try
      UpdateRepositoriesDB(lpProject)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#Region "ProcessResultSummary Methods"

  Public Overrides Function GetProcessResultSummary(ByVal lpBatch As Batch) As IProcessResultSummary
    Try
      Return GetProcessResultSummaryDB(lpBatch)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProcessResultSummary(ByVal lpJob As Job) As IProcessResultSummary
    Try

      Return GetProcessResultSummaryDB(lpJob, False)

      '' Get the current value
      'Dim lobjCurrentProcessResultSummary As IProcessResultSummary = GetProcessResultSummaryDB(lpJob, False)
      'Dim lobjDifference As TimeSpan = DateTime.Now - lobjCurrentProcessResultSummary.CreateDate

      '' If the current create date is not within the configuired job status refresh interval, create a new one
      'If lobjDifference.TotalSeconds < ConnectionSettings.GetCurrentSettings.JobStatusRefreshInterval AndAlso Helper.CallStackContainsMethodName("SaveProcessResultSummaryDB") Then
      '  Return lobjCurrentProcessResultSummary
      '  'Return GetProcessResultSummaryDB(lpJob, True)
      'Else
      '  Dim lobjCachedJobWorkSummary As IWorkSummary = lpJob.GetCachedWorkSummaryCounts()

      '  Select Case lobjCachedJobWorkSummary.SuccessCount
      '    Case < 100000
      '      Return GetProcessResultSummaryDB(lpJob)
      '    Case Else
      '      ' When the completed counts get over a certain size, it is more efficient to get them one batch at a time
      '      Dim lobjBatchSummaries As New ProcessResultSummaries
      '      For Each lobjBatch As Batch In lpJob.Batches
      '        lobjBatchSummaries.Add(lobjBatch.GetProcessResultsSummary())
      '      Next

      '      lobjCurrentProcessResultSummary = New ProcessResultSummary(lobjBatchSummaries)
      '  End Select
      'End If



    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub SaveProcessResultSummary(ByVal lpBatch As Batch)
    Try
      SaveProcessResultSummaryDB(lpBatch, True)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveProcessResultSummary(ByVal lpJob As Job)
    Try
      SaveProcessResultSummaryDB(lpJob, True)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetProcessResultSummaryDB(ByVal lpBatch As Batch) As IProcessResultSummary
    Try
      Dim lstrSQL As String = String.Format("SELECT ResultSummary FROM {0} WHERE BatchId = '{1}'", TABLE_BATCH_NAME, lpBatch.Id)
      Dim lobjReturnValue As Object = ExecuteSimpleQuery(lstrSQL)
      If Not IsDBNull(lobjReturnValue) OrElse Not String.IsNullOrEmpty(lobjReturnValue.ToString()) Then
        Return ProcessResultSummary.FromXmlString(lobjReturnValue.ToString())
      Else
        Dim lobjResultSummary As New ProcessResultSummary(GetProcessResults(lpBatch))
        SaveProcessResultSummaryDB(lobjResultSummary, lpBatch)
        Return lobjResultSummary
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function


  Private Function GetProcessResultSummaryDB(ByVal lpJob As Job, Optional lpForceRefresh As Boolean = False) As IProcessResultSummary
    Try

      If lpForceRefresh = True Then
        'Dim lobjResultSummary As New ProcessResultSummary(GetProcessResults(lpJob))
        Dim lobjResultSummary As IProcessResultSummary = GetProcessResultSummaryFromBatches(lpJob)

        SaveProcessResultSummaryDB(lobjResultSummary, lpJob)
        Return lobjResultSummary
      Else
        Dim lstrSQL As String = String.Format("SELECT ResultSummary FROM {0} WHERE JobId = '{1}'", TABLE_JOB_NAME, lpJob.Id)
        Dim lobjReturnValue As Object = ExecuteSimpleQuery(lstrSQL)
        If Not IsDBNull(lobjReturnValue) OrElse Not String.IsNullOrEmpty(lobjReturnValue.ToString()) Then
          Return ProcessResultSummary.FromXmlString(lobjReturnValue.ToString())
        Else
          'Dim lobjResultSummary As New ProcessResultSummary(GetProcessResults(lpJob))
          'SaveProcessResultSummaryDB(lobjResultSummary, lpJob)
          'Return lobjResultSummary
          Dim lobjCurrentProcessResultSummary As IProcessResultSummary = GetProcessResultSummaryFromBatches(lpJob)
          'Dim lobjBatchSummaries As New ProcessResultSummaries
          'For Each lobjBatch As Batch In lpJob.Batches
          '  lobjBatchSummaries.Add(lobjBatch.GetProcessResultsSummary())
          'Next

          'lobjCurrentProcessResultSummary = New ProcessResultSummary(lobjBatchSummaries)
          Return lobjCurrentProcessResultSummary
        End If
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function GetProcessResultSummaryFromBatches(ByVal lpJob As Job) As IProcessResultSummary
    Try
      Dim lobjCurrentProcessResultSummary As IProcessResultSummary
      Dim lobjBatchSummaries As New ProcessResultSummaries
      For Each lobjBatch As Batch In lpJob.Batches
        lobjBatchSummaries.Add(lobjBatch.GetProcessResultsSummary())
      Next

      lobjCurrentProcessResultSummary = New ProcessResultSummary(lobjBatchSummaries)
      Return lobjCurrentProcessResultSummary
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub SaveProcessResultSummaryDB(ByVal lpBatch As Batch, ByVal lpForceRefresh As Boolean)
    Try
      If lpForceRefresh Then
        Dim lobjResultSummary As New ProcessResultSummary(GetProcessResults(lpBatch))
        SaveProcessResultSummaryDB(lobjResultSummary, lpBatch)
      Else
        SaveProcessResultSummaryDB(GetProcessResultSummary(lpBatch), TABLE_BATCH_NAME, lpBatch.Id)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveProcessResultSummaryDB(ByVal lpProcessResultSummary As IProcessResultSummary, ByVal lpBatch As Batch)
    Try
      SaveProcessResultSummaryDB(lpProcessResultSummary, TABLE_BATCH_NAME, lpBatch.Id)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveProcessResultSummaryDB(ByVal lpProcessResultSummary As IProcessResultSummary, ByVal lpJob As Job)
    Try
      SaveProcessResultSummaryDB(lpProcessResultSummary, TABLE_JOB_NAME, lpJob.Id)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveProcessResultSummaryDB(ByVal lpJob As Job, ByVal lpForceRefresh As Boolean)
    Try
      'If lpForceRefresh Then
      '  Dim lobjResultSummary As New ProcessResultSummary(GetProcessResults(lpJob))
      '  SaveProcessResultSummaryDB(lobjResultSummary, lpJob)
      'Else
      '  SaveProcessResultSummaryDB(GetProcessResultSummary(lpJob), TABLE_JOB_NAME, lpJob.Id)
      'End If


      Dim lobjResultSummary As ProcessResultSummary = GetProcessResultSummaryDB(lpJob, lpForceRefresh)
      SaveProcessResultSummaryDB(lobjResultSummary, lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SaveProcessResultSummaryDB(ByVal lpResultSummary As IProcessResultSummary, ByVal lpTableName As String, lpId As String)
    Try

      'Dim lstrSQL As String = String.Format("UPDATE {0} SET ResultSummary = '{1}' WHERE BatchId = '{2}'", lpTableName, lpResultSummary.ToXmlString, lpId)
      Dim lobjSQLBuilder As New StringBuilder

      lobjSQLBuilder.AppendFormat("UPDATE {0} SET ResultSummary = '{1}' WHERE ", lpTableName, lpResultSummary.ToXmlString)

      Select Case lpTableName
        Case TABLE_BATCH_NAME
          lobjSQLBuilder.Append("BatchId")

        Case TABLE_JOB_NAME
          lobjSQLBuilder.Append("JobId")

      End Select

      lobjSQLBuilder.AppendFormat(" = '{0}'", lpId)

      ExecuteNonQuery(lobjSQLBuilder.ToString())

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#End Region

#Region "IDisposible"

  Protected Overrides Sub Dispose(ByVal disposing As Boolean)

    Try

      ' Log the db file info first
      Try
        Dim lobjDbFilesInfo As DbFilesInfo = GetProjectDbFileInfo()
        'LogSession.LogString(level.Message, "Project database file information", lobjDbFilesInfo.ToXmlString())
      Catch ex As Exception
        ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
        'LogSession.LogError("Unable to get project database file information.")
      End Try

      MyBase.Dispose(disposing)

      If (mobjConnection IsNot Nothing) Then

        If (mobjConnection.State <> ConnectionState.Closed Or mobjConnection.State <> ConnectionState.Broken) Then
          mobjConnection.Close()
        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region
End Class
