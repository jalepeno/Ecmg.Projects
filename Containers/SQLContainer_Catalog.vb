'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  SQLContainer_Catalog.vb
'   Description :  [type_description_here]
'   Created     :  1/15/2014 7:20:58 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports System.Reflection
Imports Documents.Exceptions
Imports Documents.Utilities
Imports Microsoft.Data.SqlClient

#End Region

Partial Public Class SQLContainer
  Implements ICatalogContainer

#Region "ICatalogContainer Implementation"

  'Public Property CatalogConnectionString As String
  '  Get
  '    Try
  '      Return mstrCatalogConnectionString
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As String)
  '    Try
  '      mstrCatalogConnectionString = value
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  Public Sub SaveArea(lpArea As IArea) Implements ICatalogContainer.SaveArea
    Dim lobjConnection As SqlConnection = Nothing
    Try
      lobjConnection = New SqlConnection(ProjectCatalog.Instance.ConnectionString)
      Using lobjCommand As New SqlCommand("usp_save_area", lobjConnection)
        lobjCommand.CommandType = CommandType.StoredProcedure
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT

        Dim lobjIdParameter As New SqlParameter("@id", SqlDbType.NVarChar, 255) With {
          .Value = lpArea.Id
        }
        lobjCommand.Parameters.Add(lobjIdParameter)

        Dim lobjNameParameter As New SqlParameter("@name", SqlDbType.NVarChar, 255) With {
          .Value = lpArea.Name
        }
        lobjCommand.Parameters.Add(lobjNameParameter)

        Dim lobjDescriptionParameter As New SqlParameter("@description", SqlDbType.NText) With {
          .Value = lpArea.Description
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
          Throw New ItemDoesNotExistException(lpArea.Id)
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If
    End Try
  End Sub

  Public Sub SaveCatalogConfiguration(lpCatalogId As String, lpConfiguration As ICatalogConfiguration) Implements ICatalogContainer.SaveCatalogConfiguration
    Dim lobjConnection As SqlConnection = Nothing
    Try
      lobjConnection = New SqlConnection(ProjectCatalog.Instance.ConnectionString)
      Using lobjCommand As New SqlCommand("usp_save_catalog_configuration", lobjConnection)
        lobjCommand.CommandType = CommandType.StoredProcedure
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT

        Dim lobjIdParameter As New SqlParameter("@catalogId", SqlDbType.NVarChar, 255) With {
          .Value = lpCatalogId
        }
        lobjCommand.Parameters.Add(lobjIdParameter)

        Dim lobjJobConfigurationParameter As New SqlParameter("@catalogconfiguration", SqlDbType.NText) With {
          .Value = lpConfiguration.ToSQLXmlString
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
        If lintReturnValue = -100 Then
          Throw New ItemDoesNotExistException(lpCatalogId)
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    Finally
      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If
    End Try
  End Sub

  Public Sub SaveProjectToCatalog(lpProject As IProjectDescription) Implements ICatalogContainer.SaveProject
    Dim lobjConnection As SqlConnection = Nothing
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpProject)
#Else
      If lpProject Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpProject))
      End If
#End If

      If lpProject.Area Is Nothing Then
        Throw New InvalidOperationException("Area reference not set.")
      End If

      lobjConnection = New SqlConnection(ProjectCatalog.Instance.ConnectionString)
      Using lobjCommand As New SqlCommand("usp_save_project", lobjConnection)
        lobjCommand.CommandType = CommandType.StoredProcedure
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT

        Dim lobjIdParameter As New SqlParameter("@projectId", SqlDbType.NVarChar, 255) With {
          .Value = lpProject.Id
        }
        lobjCommand.Parameters.Add(lobjIdParameter)

        Dim lobjNameParameter As New SqlParameter("@projectName", SqlDbType.NVarChar, 255) With {
          .Value = lpProject.Name
        }
        lobjCommand.Parameters.Add(lobjNameParameter)

        Dim lobjAreaIdParameter As New SqlParameter("@areaId", SqlDbType.NVarChar, 255) With {
          .Value = lpProject.Area.Id
        }
        lobjCommand.Parameters.Add(lobjAreaIdParameter)

        Dim lobjLocationParameter As New SqlParameter("@location", SqlDbType.NText) With {
          .Value = lpProject.Location.ToString
        }
        lobjCommand.Parameters.Add(lobjLocationParameter)

        Dim lobjCreateDateParameter As New SqlParameter("@createDate", SqlDbType.DateTime) With {
          .Value = lpProject.CreateDate
        }
        lobjCommand.Parameters.Add(lobjCreateDateParameter)

        Dim lobjItemsProcessedParameter As New SqlParameter("@itemsProcessed", SqlDbType.BigInt) With {
          .Value = lpProject.ItemsProcessed
        }
        lobjCommand.Parameters.Add(lobjItemsProcessedParameter)

        Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
          .Direction = ParameterDirection.ReturnValue
        }
        lobjCommand.Parameters.Add(lobjReturnParameter)

        Helper.HandleConnection(lobjConnection)
        Dim lintReturnValue As Integer
        lobjCommand.ExecuteNonQuery()
        lintReturnValue = lobjReturnParameter.Value
        If lintReturnValue = -100 Then
          Throw New ItemDoesNotExistException(lpProject.Id)
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If
    End Try
  End Sub

  Public Sub DeleteArea(lpAreaId As String) Implements ICatalogContainer.DeleteArea
    Dim lobjConnection As SqlConnection = Nothing
    Try
      lobjConnection = New SqlConnection(ProjectCatalog.Instance.ConnectionString)
      Using lobjCommand As New SqlCommand("usp_delete_area", lobjConnection)
        lobjCommand.CommandType = CommandType.StoredProcedure
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT

        Dim lobjIdParameter As New SqlParameter("@areaId", SqlDbType.NVarChar, 255) With {
          .Value = lpAreaId
        }
        lobjCommand.Parameters.Add(lobjIdParameter)

        Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
          .Direction = ParameterDirection.ReturnValue
        }
        lobjCommand.Parameters.Add(lobjReturnParameter)

        Helper.HandleConnection(lobjConnection)
        Dim lintReturnValue As Integer
        lobjCommand.ExecuteNonQuery()
        lintReturnValue = lobjReturnParameter.Value
        If lintReturnValue = -100 Then
          Throw New ItemDoesNotExistException(lpAreaId)
        ElseIf lintReturnValue = -200 Then
          Throw New InvalidOperationException("Unable to delete area as it contains one or more projects.")
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If
    End Try
  End Sub

  Public Function GetAreas() As IAreas Implements ICatalogContainer.GetAreas
    Try
      Dim lobjCatalog As ProjectCatalog = GetCurrentCatalog()
      Return lobjCatalog.Areas

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetNode(lpNodeId As String) As INodeInfo Implements ICatalogContainer.GetNode
    Try
      Return GetNodeDB(lpNodeId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetNodes() As INodeInfoCollection Implements ICatalogContainer.GetNodes
    Try
      Return GetNodesDB()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub DeleteNode(lpNodeId As String) Implements ICatalogContainer.DeleteNode
    Dim lobjConnection As SqlConnection = Nothing
    Try
      lobjConnection = New SqlConnection(ProjectCatalog.Instance.ConnectionString)
      Using lobjCommand As New SqlCommand("usp_delete_node", lobjConnection)
        lobjCommand.CommandType = CommandType.StoredProcedure
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT

        Dim lobjIdParameter As New SqlParameter("@nodeId", SqlDbType.NVarChar, 255) With {
          .Value = lpNodeId
        }
        lobjCommand.Parameters.Add(lobjIdParameter)

        Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
          .Direction = ParameterDirection.ReturnValue
        }
        lobjCommand.Parameters.Add(lobjReturnParameter)

        Helper.HandleConnection(lobjConnection)
        Dim lintReturnValue As Integer
        lobjCommand.ExecuteNonQuery()
        lintReturnValue = lobjReturnParameter.Value
        If lintReturnValue = -100 Then
          Throw New ItemDoesNotExistException(lpNodeId)
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If
    End Try
  End Sub

  Public Sub SaveNode(lpNode As INodeInfo) Implements ICatalogContainer.SaveNode
    Dim lobjConnection As SqlConnection = Nothing
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpNode)
#Else
      If lpNode Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpNode))
      End If
#End If

      'If lpNode.ComputerInfo Is Nothing Then
      '  Throw New InvalidOperationException("ComputerInfo reference not set.")
      'End If

      lobjConnection = New SqlConnection(ProjectCatalog.Instance.ConnectionString)
      Using lobjCommand As New SqlCommand("usp_save_node", lobjConnection)
        lobjCommand.CommandType = CommandType.StoredProcedure
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT

        Dim lobjIdParameter As New SqlParameter("@nodeid", SqlDbType.NVarChar, 255) With {
          .Value = lpNode.Id
        }
        lobjCommand.Parameters.Add(lobjIdParameter)

        Dim lobjNameParameter As New SqlParameter("@nodeName", SqlDbType.NVarChar, 255) With {
          .Value = lpNode.Name
        }
        lobjCommand.Parameters.Add(lobjNameParameter)

        Dim lobjDescriptionParameter As New SqlParameter("@nodeDescription", SqlDbType.NVarChar, 255) With {
          .Value = lpNode.Description
        }
        lobjCommand.Parameters.Add(lobjDescriptionParameter)

        Dim lobjAddressParameter As New SqlParameter("@nodeAddress", SqlDbType.NVarChar, 255) With {
          .Value = lpNode.Address
        }
        lobjCommand.Parameters.Add(lobjAddressParameter)

        Dim lobjRoleParameter As New SqlParameter("@nodeRole", SqlDbType.NVarChar, 20) With {
          .Value = lpNode.Role
        }
        lobjCommand.Parameters.Add(lobjRoleParameter)

        Dim lobjStatusParameter As New SqlParameter("@nodeStatus", SqlDbType.NVarChar, 20) With {
          .Value = lpNode.Status
        }
        If lpNode.Status = "Inactive" Then
          'ApplicationLogging.WriteLogEntry("Filematica Service saved as inactive.", Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Information, 123655)
          ''LogSession.LogMessage("Filematica Service saved as inactive.")
          'Else
          '  ApplicationLogging.WriteLogEntry("Job Manager Service saved as active.", Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Information, 123656)
        End If
        lobjCommand.Parameters.Add(lobjStatusParameter)

        Dim lobjVersionParameter As New SqlParameter("@version", SqlDbType.NVarChar, 20) With {
          .Value = lpNode.Version
        }
        lobjCommand.Parameters.Add(lobjVersionParameter)

        Dim lobjCreateDateParameter As New SqlParameter("@createDate", SqlDbType.DateTime) With {
          .Value = lpNode.CreateDate
        }
        lobjCommand.Parameters.Add(lobjCreateDateParameter)

        'Dim lobjItemsProcessedParameter As New SqlParameter("@computerInfo", SqlDbType.NVarChar)
        'lobjItemsProcessedParameter.Value = lpNode.ComputerInfo.ToJson
        'lobjCommand.Parameters.Add(lobjItemsProcessedParameter)

        Dim lobjReturnParameter As New SqlParameter("@returnvalue", SqlDbType.Int) With {
          .Direction = ParameterDirection.ReturnValue
        }
        lobjCommand.Parameters.Add(lobjReturnParameter)

        Helper.HandleConnection(lobjConnection)
        Dim lintReturnValue As Integer
        lobjCommand.ExecuteNonQuery()
        lintReturnValue = lobjReturnParameter.Value
        If lintReturnValue = -100 Then
          Throw New ItemDoesNotExistException(lpNode.Id)
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If
    End Try

  End Sub

  Public Sub AddProjectFromConnectionString(lpProjectConnectionString As String) Implements ICatalogContainer.AddProjectFromConnectionString
    Dim lobjConnection As SqlConnection = Nothing
    Dim lobjDataReader As SqlDataReader = Nothing
    Dim lobjProjectDescription As IProjectDescription = Nothing
    Dim lobjArea As IArea = Nothing

    Try
      lobjConnection = New SqlConnection(lpProjectConnectionString)
      Dim lstrSQL As String = "SELECT * FROM tblProject ORDER BY ProjectName"

      If ProjectCatalog.Instance.Areas.Count = 0 Then
        Dim lobjDefaultArea As New Area("Default", "Default Area")
        ProjectCatalog.Instance.Areas.Add(lobjDefaultArea)
        lobjArea = lobjDefaultArea
      Else
        For Each lobjArea In ProjectCatalog.Instance.Areas()
          If lobjArea.Name.Equals("default", StringComparison.CurrentCultureIgnoreCase) Then
            Exit For
          End If
        Next
      End If

      If lobjArea Is Nothing Then
        Throw New InvalidOperationException("Unable to add project, area not initialized.")
      End If

      Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
        lobjCommand.CommandTimeout = COMMAND_TIMEOUT
        Helper.HandleConnection(lobjConnection)
        lobjDataReader = lobjCommand.ExecuteReader()

        If (lobjDataReader.HasRows = True) Then
          While lobjDataReader.Read
            lobjProjectDescription = Nothing
            Try
              lobjProjectDescription = OpenProjectDescriptionFromDataReader(lobjDataReader, lobjArea)
              If lobjProjectDescription IsNot Nothing Then
                lobjProjectDescription.Location = New ItemsLocation(lpProjectConnectionString)
                SaveProjectToCatalog(lobjProjectDescription)
              Else
                ApplicationLogging.WriteLogEntry(String.Format("Failed to open project description using connection string '{0}'.",
                                                               lpProjectConnectionString), TraceEventType.Error, 53812)
              End If
            Catch ex As Exception
              ' We will log and skip this area
              ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
              Continue While
            End Try
          End While
        End If
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod, lpProjectConnectionString)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub CreateCatalog(lpCatalogConnectionString As String, lpCatalogName As String, lpCatalogDescription As String) Implements ICatalogContainer.CreateCatalog
    Try
      InitializeCatalogDB(lpCatalogConnectionString, lpCatalogName, lpCatalogDescription, True)
      'CreateDatabase(DatabaseType.Catalog)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function GetCurrentCatalog() As IProjectCatalog Implements ICatalogContainer.GetCurrentCatalog
    Try
      Dim lstrCatalogConnectionString As String = ProjectCatalog.CurrentConnectionString
      If Not String.IsNullOrEmpty(lstrCatalogConnectionString) Then
        If ((lstrCatalogConnectionString.Contains("Integrated Security")) AndAlso (Not lstrCatalogConnectionString.Contains("TrustServerCertificate"))) Then
          lstrCatalogConnectionString = $"{lstrCatalogConnectionString};TrustServerCertificate=True"
        End If

        ' If we changed the connection string here, update it for downstream use.
        ProjectCatalog.CurrentConnectionString = lstrCatalogConnectionString

        Return GetCatalogDB(lstrCatalogConnectionString)
      Else
        Throw New CatalogConnectionNotConfiguredException
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub AddNewColumnsToCatalogTables(lpConnectionString As String)
    Try

      ExecuteNonQuery(CreateInsertColumnSQL("WorkSummary", "nvarchar(MAX) NULL", TABLE_PROJECTS_NAME), lpConnectionString)

      ' <Added by: Ernie at: 8/14/2015-10:56:52 AM on machine: ERNIE-THINK>
      ExecuteNonQuery(CreateInsertColumnSQL("CatalogConfiguration", "ntext NULL", TABLE_CATALOG_NAME), lpConnectionString)
      ExecuteNonQuery(CreateInsertColumnSQL("ItemsProcessed", "bigint NULL", TABLE_CATALOG_NAME), lpConnectionString)
      ExecuteNonQuery(CreateInsertColumnSQL("ItemsProcessed", "bigint NULL", TABLE_NODES_NAME), lpConnectionString)
      ' </Added by: Ernie at: 8/14/2015-10:56:52 AM on machine: ERNIE-THINK>

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetCatalogDB(lpConnectionString As String) As IProjectCatalog

    Dim lobjCatalog As ProjectCatalog = Nothing
    Dim lobjCatalogConfiguration As CatalogConfiguration = Nothing
    Dim lobjArea As IArea = Nothing
    Dim lobjProjectDescriptions As IProjectDescriptions = Nothing
    Dim lobjNodeCollection As INodeInfoCollection
    Try

      ' Make sure we have as connection string value
#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpConnectionString)
#Else
          If lpConnectionString Is Nothing Then
            Throw New ArgumentNullException(NameOf(lpConnectionString))
          End If
#End If
      InitializeCatalogDB(lpConnectionString, False)

      Using lobjConnection As New SqlConnection(lpConnectionString)

        Dim lstrCatalogId As String = ExecuteSimpleQuery("SELECT CatalogId FROM tblCatalog", lpConnectionString)
        Dim lstrCatalogConfigXml As Object = ExecuteSimpleQuery(String.Format("SELECT CatalogConfiguration FROM tblCatalog WHERE CatalogId = '{0}'", lstrCatalogId), lpConnectionString)

        If IsDBNull(lstrCatalogConfigXml) Then
          lobjCatalogConfiguration = New CatalogConfiguration
        Else
          If Not String.IsNullOrEmpty(lstrCatalogConfigXml) Then
            lobjCatalogConfiguration = CatalogConfiguration.FromXmlString(lstrCatalogConfigXml)
          Else
            lobjCatalogConfiguration = New CatalogConfiguration
          End If
        End If

        lobjCatalog = New ProjectCatalog(lstrCatalogId, lobjCatalogConfiguration) With {
          .NodeStatusRefreshInterval = 30
        }

        ' Get the areas and projects
        Dim lstrSQL As String = "SELECT * FROM tblAreas ORDER BY AreaName"

        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                lobjArea = Nothing
                lobjProjectDescriptions = Nothing

                Try
                  lobjArea = New Area(lobjDataReader("AreaId"), lobjDataReader("AreaName"), lobjDataReader("AreaDescription"))
                  GetProjectsForAreaDB(lobjArea)
                  lobjCatalog.Areas.Add(lobjArea)
                Catch ex As Exception
                  ' We will log and skip this area
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                  Continue While
                End Try
              End While
            End If
            If Not lobjDataReader.IsClosed Then
              lobjDataReader.Close()
            End If
          End Using
        End Using
      End Using

      ' Make sure we have the newer columns
      If DoesColumnExist("tblNodes", "Version", ProjectCatalog.CurrentConnectionString) = False Then
        ExecuteNonQuery(CreateInsertColumnSQL("Version", "nvarchar(20) NULL", TABLE_NODES_NAME), ProjectCatalog.CurrentConnectionString)
        CreateStoredProcedures(DatabaseType.Catalog)
      End If

      ' Get the nodes
      lobjNodeCollection = GetNodesDB()

      For Each lobjNode As INodeInfo In lobjNodeCollection
        lobjCatalog.Nodes.Add(lobjNode)
      Next

      Return lobjCatalog

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetNodesDB() As INodeInfoCollection
    Dim lobjNodeCollection As INodeInfoCollection = New NodeInfoCollection
    Try
      Dim lobjNode As INodeInfo = Nothing

      Dim lstrSQL As String = "SELECT * FROM tblNodes ORDER BY NodeName"
      Using lobjConnection As New SqlConnection(ProjectCatalog.CurrentConnectionString)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                lobjNode = Nothing

                Try
                  lobjNode = GetNodeFromDataReader(lobjDataReader)
                  lobjNodeCollection.Add(lobjNode)
                Catch ex As Exception
                  ' We will log and skip this area
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                  Continue While
                End Try
              End While

            End If
            If Not lobjDataReader.IsClosed Then
              lobjDataReader.Close()
            End If
          End Using
        End Using

      End Using

      Return lobjNodeCollection

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetNodeDB(lpNodeId As String) As INodeInfo
    Try
      Dim lobjNode As INodeInfo = Nothing

      Dim lstrSQL As String = String.Format("SELECT * FROM tblNodes WHERE NodeId = '{0}'", lpNodeId)

      Using lobjConnection As New SqlConnection(ProjectCatalog.CurrentConnectionString)
        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                lobjNode = Nothing
                lobjNode = GetNodeFromDataReader(lobjDataReader)
                Exit While
              End While

            Else
              ' The node was not found, close the reader and throw an exception
              If Not lobjDataReader.IsClosed Then
                lobjDataReader.Close()
              End If
              Throw New NodeDoesNotExistException(lpNodeId)
            End If
            If Not lobjDataReader.IsClosed Then
              lobjDataReader.Close()
            End If
          End Using
        End Using

      End Using

      Return lobjNode

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetNodeFromDataReader(lpDataReader As IDataReader) As INodeInfo
    Try
      Dim lobjNode As INodeInfo = Nothing
      Dim lstrRole As String = String.Empty
      Dim lstrStatus As String = String.Empty
      Dim lstrVersion As String = String.Empty
      Dim lstrComputerInfo As String = String.Empty
      Dim lenuNodeRole As NodeRole
      Dim lenuNodeStatus As NodeStatus
      'Dim lobjComputerInfo As IComputerInfo = Nothing

      ' Get the role
      If Not IsDBNull(lpDataReader("Role")) Then
        lstrRole = lpDataReader("Role")
        If Not [Enum].TryParse(Of NodeRole)(lstrRole, lenuNodeRole) Then
          Throw New InvalidCastException(String.Format("Invalid role value: {0}", lstrRole))
        End If
      Else
        lenuNodeRole = NodeRole.Worker
      End If

      ' Get the status
      If Not IsDBNull(lpDataReader("Status")) Then
        lstrStatus = lpDataReader("Status")
        If Not [Enum].TryParse(Of NodeStatus)(lstrStatus, lenuNodeStatus) Then
          Throw New InvalidCastException(String.Format("Invalid status value: {0}", lstrStatus))
        End If
      Else
        lenuNodeStatus = NodeStatus.Inactive
      End If

      ' Get the version
      If Not IsDBNull(lpDataReader("Version")) Then
        lstrVersion = lpDataReader("Version")
      End If

      '' Get the computerinfo
      'If Not IsDBNull(lpDataReader("ComputerInfo")) Then
      '  lstrComputerInfo = lpDataReader("ComputerInfo")
      '  lobjComputerInfo = ComputerInfo.FromJson(lstrComputerInfo)
      'Else
      '  lobjComputerInfo = ComputerInfo.Create
      'End If

      'lobjNode = New NodeInfo(lpDataReader("NodeId"),
      '                          lpDataReader("NodeName"),
      '                          lpDataReader("NodeDescription"),
      '                          lpDataReader("NodeAddress"),
      '                          lenuNodeRole,
      '                          lenuNodeStatus,
      '                          lstrVersion,
      '                          lpDataReader("CreateDate"),
      '                          lobjComputerInfo)

      lobjNode = New NodeInfo(lpDataReader("NodeId"),
                                lpDataReader("NodeName"),
                                lpDataReader("NodeDescription"),
                                lpDataReader("NodeAddress"),
                                lenuNodeRole,
                                lenuNodeStatus,
                                lstrVersion,
                                lpDataReader("CreateDate"))

      Return lobjNode

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function IsCatalogTableInitialized(lpCatalogConnectionString As String) As Boolean
    Try
      Dim lintCatalogRowCount As Integer = ExecuteSimpleQuery("SELECT COUNT(*) FROM tblCatalog", lpCatalogConnectionString)
      If lintCatalogRowCount = 0 Then
        Return False
      Else
        Return True
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function ProjectExists(lpProjectId As String) As Boolean Implements ICatalogContainer.ProjectExists
    Try
      Return ProjectExistsDB(lpProjectId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProjectsForAreaDB(lpArea As IArea) As IProjectDescriptions
    Dim lobjProjectDescriptions As New ProjectDescriptions
    Dim lobjProjectDescription As IProjectDescription = Nothing

    Try

      Using lobjConnection As New SqlConnection(ProjectCatalog.CurrentConnectionString)

        Dim lstrSQL As String = String.Format("SELECT * FROM tblProjects WHERE AreaId = '{0}' ORDER BY ProjectName", lpArea.Id)

        Using lobjCommand As New SqlCommand(lstrSQL, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)
          Using lobjDataReader As SqlDataReader = lobjCommand.ExecuteReader

            If (lobjDataReader.HasRows = True) Then

              While lobjDataReader.Read
                lobjProjectDescription = Nothing
                Try
                  lobjProjectDescription = OpenProjectDescriptionFromDataReader(lobjDataReader, lpArea)
                  'lobjProjectDescription = New ProjectDescription(lobjDataReader("ProjectId"), _
                  '                                                lobjDataReader("ProjectName"), _
                  '                                                lobjDataReader("Location"), _
                  '                                                lobjDataReader("CreateDate"), _
                  '                                                lpArea, _
                  '                                                lobjDataReader("ItemsProcessed"), _
                  '                                                lobjDataReader("WorkSummary"))

                  ' Make sure this is not a phantom project
                  If Not String.IsNullOrEmpty(lobjProjectDescription.Name) Then
                    lobjProjectDescriptions.Add(lobjProjectDescription)
                  End If

                Catch ex As Exception
                  ' We will log and skip this project
                  ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
                  Continue While
                End Try
              End While
            End If
            lobjDataReader.Close()
          End Using
        End Using
      End Using

      Return lobjProjectDescriptions

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
