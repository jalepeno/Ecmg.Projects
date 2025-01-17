'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectCatalog.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 2:54:18 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.Text
Imports System.Threading
Imports Documents.Configuration
Imports Documents.Exceptions
Imports Documents.Utilities
Imports Microsoft.Data.SqlClient
Imports Newtonsoft.Json

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class ProjectCatalog
  Implements IProjectCatalog

#Region "Class Variables"

  Private Shared mobjInstance As IProjectCatalog
  Private Shared mintReferenceCount As Integer

  Private ReadOnly mstrId As String = String.Empty
  Private ReadOnly mobjAreas As IAreas = New Areas(Me)
  Private mobjDefaultArea As IArea = Nothing

  Private WithEvents MobjConfiguration As ICatalogConfiguration = New CatalogConfiguration
  Private mobjInfo As ICatalogInfo = Nothing

  Property MobjNodes As INodeInfoCollection = New NodeInfoCollection
  Private Shared mstrConnectionString As String = String.Empty
  Private mintNodeStatusRefreshInterval As Integer = 30
  Private Shared mobjContainer As ICatalogContainer = Nothing

  ' For automatically updating the status of this node
  Private mobjAutoEvent As AutoResetEvent
  Private WithEvents MobjStatusChecker As NodeStatusChecker
  Private mobjTimerCallback As TimerCallback
  Private stateTimer As Timer

  'Private mobjNotificationDependencyEntities As New NotificationDependencyEntities
  'Private mobjListenerConnection As SqlConnection
  'Private Shared mobjLogSession As Gurock.SmartInspect.Session

#End Region

#Region "Events"

  Public Event NodeStatusRefreshed As NodeEventHandler Implements IProjectCatalog.NodeStatusRefreshed

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

  Friend Sub New(lpId As String, lpConfiguration As ICatalogConfiguration)
    Try
      mstrId = lpId
      MobjConfiguration = lpConfiguration
      'InitializeLogSession()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IProjectCatalog Implementation"

  Public ReadOnly Property Id As String Implements IProjectCatalog.Id
    Get
      Try
        Return mstrId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property ConnectionString As String Implements IProjectCatalog.ConnectionString
    Get
      Try
        If String.IsNullOrEmpty(mstrConnectionString) Then
          mstrConnectionString = CurrentConnectionString
        End If
        Return mstrConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrConnectionString = value
        ' Save the settings in the current settings.csf file
        If ConnectionSettings.Instance.ProjectCatalogConnectionString <> mstrConnectionString Then
          ConnectionSettings.Instance.ProjectCatalogConnectionString = mstrConnectionString
          ConnectionSettings.Instance.Save()
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property NodeStatusRefreshInterval As Integer
    Get
      Try
        Return mintNodeStatusRefreshInterval
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintNodeStatusRefreshInterval = value
        stateTimer?.Change(1000, 1000 * value)
        ConnectionSettings.Instance.NodeStatusRefreshInterval = value
        ConnectionSettings.Instance.Save()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property ProjectCount() As Integer Implements IProjectCatalog.ProjectCount
    Get
      Try
        Return GetProjectCount()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Areas As IAreas Implements IProjectCatalog.Areas
    Get
      Try
        Return mobjAreas
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property DefaultArea As IArea Implements IProjectCatalog.DefaultArea
    Get
      Try
        If mobjDefaultArea Is Nothing Then
          mobjDefaultArea = FirstArea
        End If
        Return mobjDefaultArea
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As IArea)
      Try
        mobjDefaultArea = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property FirstArea As IArea
    Get
      Try
        Return Me.Areas.FirstOrDefault
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Info As ICatalogInfo Implements IProjectCatalog.Info
    Get
      Try
        If mobjInfo Is Nothing Then
          mobjInfo = CatalogInfo.Create()
        End If
        Return mobjInfo
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Nodes As INodeInfoCollection Implements IProjectCatalog.Nodes
    Get
      Try
        Return MobjNodes
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ThisNode As INodeInfo Implements IProjectCatalog.ThisNode
    Get
      Try
        If Nodes.Contains(Environment.MachineName) Then
          Return Nodes.Item(Environment.MachineName)
        Else
          Return Nothing
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Configuration As ICatalogConfiguration Implements IProjectCatalog.Configuration
    Get
      Try
        Return MobjConfiguration
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Friend ReadOnly Property Container As ICatalogContainer Implements IProjectCatalog.Container
    Get
      Try
        If mobjContainer Is Nothing Then
          mobjContainer = New SQLContainer
        End If
        Return mobjContainer
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Shared Property CurrentConnectionString As String
    Get
      Try
        If String.IsNullOrEmpty(mstrConnectionString) Then
          mstrConnectionString = ConnectionSettings.Instance.ProjectCatalogConnectionString
        End If
        Return mstrConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrConnectionString = value
        ConnectionSettings.Instance.ProjectCatalogConnectionString = value
        ConnectionSettings.Instance.Save()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Shared ReadOnly Property IsConnectionConfigured As Boolean
    Get
      Try
        Return Not String.IsNullOrEmpty(CurrentConnectionString)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Shared ReadOnly Property CurrentDatabaseName As String
    Get
      Try
        Return GetDatabaseName()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Private Property AreaListings As Object

  Private Shared Function GetDatabaseName() As String
    Try
      Dim lobjParser As New SqlConnectionStringBuilder(CurrentConnectionString)
      Return lobjParser.InitialCatalog
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Function ToProjectDirectory() As IProjectDirectory Implements IProjectCatalog.ToProjectDirectory
    Try
      Return New ProjectDirectory(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public ReadOnly Property ReferenceCount As Integer Implements IProjectCatalog.ReferenceCount
    Get
      Return mintReferenceCount
    End Get
  End Property

  Public Function ToJson() As String Implements IProjectCatalog.ToJson
    Try

      Dim lobjJsonSettings As New JsonSerializerSettings With {
        .PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        .TypeNameHandling = TypeNameHandling.Auto
      }

      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None, lobjJsonSettings)
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented, lobjJsonSettings)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function FromJson(lpJsonString As String) As IProjectCatalog
    Try
      Dim lobjJsonSettings As New JsonSerializerSettings With {
        .PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        .TypeNameHandling = TypeNameHandling.Auto
      }
      'lobjJsonSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor

      Return JsonConvert.DeserializeObject(lpJsonString, GetType(ProjectCatalog), lobjJsonSettings)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  <JsonIgnore()>
  Public ReadOnly Property IsDisposed() As Boolean Implements IProjectCatalog.IsDisposed
    Get
      Return disposedValue
    End Get
  End Property

  Public Shared Sub AddProjectFromConnectionString(lpProjectConnectionString As String)
    Try
      Instance.Container.AddProjectFromConnectionString(lpProjectConnectionString)
      Instance.Refresh()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod, lpProjectConnectionString)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shared Sub CreateCatalog(lpConnectionString As String, lpCatalogName As String, lpCatalogDescription As String, lpSetAsInstance As Boolean)
    Try
      If mobjContainer Is Nothing Then
        mobjContainer = New SQLContainer
      End If
      mstrConnectionString = lpConnectionString
      mobjContainer.CreateCatalog(lpConnectionString, lpCatalogName, lpCatalogDescription)
      If lpSetAsInstance Then

        mobjInstance = GetCurrentCatalog(True)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function GetArea(lpAreaId As String) As IArea Implements IProjectCatalog.GetArea
    Try
      Dim list As Object = From lobjItem In Areas Where
      lobjItem.Name.Equals(lpAreaId, StringComparison.CurrentCultureIgnoreCase) Or lobjItem.Id = lpAreaId Select lobjItem

      For Each lobjArea As Object In list
        Return lobjArea
      Next

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetNode(lpNodeId As String) As INodeInfo Implements IProjectCatalog.GetNode
    Try

      'Dim list As Object = From lobjItem In Nodes Where _
      '  lobjItem.Name.ToLower = lpNodeId.ToLower Or lobjItem.Id = lpNodeId Select lobjItem

      'For Each lobjNode As INodeInfo In list
      '  Return lobjNode
      'Next

      'Throw New NodeDoesNotExistException(lpNodeId)

      If mobjContainer Is Nothing Then
        mobjContainer = New SQLContainer
      End If

      Return mobjContainer.GetNode(lpNodeId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetNodes() As INodeInfoCollection Implements IProjectCatalog.GetNodes
    Try
      If mobjContainer Is Nothing Then
        mobjContainer = New SQLContainer
      End If

      MobjNodes = mobjContainer.GetNodes

      Return Nodes

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetProjectDescription(lpProjectId As String) As IProjectDescription Implements IProjectCatalog.GetProject
    Try
      For Each lobjArea As IArea In Areas
        Dim list As Object = From lobjItem In lobjArea.Projects Where
        lobjItem.Name.Equals(lpProjectId, StringComparison.CurrentCultureIgnoreCase) Or lobjItem.Id = lpProjectId Select lobjItem

        For Each lobjProjectDescription As IProjectDescription In list
          Return lobjProjectDescription
        Next
      Next

      Throw New ProjectDoesNotExistException(lpProjectId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Function ProjectExists(lpProjectId As String) As Boolean Implements IProjectCatalog.ProjectExists
    Try

      Return Me.Container.ProjectExists(lpProjectId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetProjectInfo(lpProjectId As String) As IProjectInfo Implements IProjectCatalog.GetProjectInfo

    Try
      Dim lobjProjectDescription As IProjectDescription = GetProjectDescription(lpProjectId)
      Using lobjContainer As IProjectContainer = New SQLContainer(lobjProjectDescription.Location)
        Return lobjContainer.GetProjectInfo()
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetJobInfo(lpJob As Job) As IJobInfo Implements IProjectCatalog.GetJobInfo
    Try
#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpJob)
#Else
      If lpJob Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpJob))
      End If
#End If
      If lpJob.Project Is Nothing Then
        Throw New ProjectNotSetException
      End If
      If lpJob.Project.ItemsLocation Is Nothing Then
        Throw New InvalidOperationException("The items location is not initialized for the project.")
      End If
      Using lobjContainer As IProjectContainer = New SQLContainer(lpJob.Project.ItemsLocation)
        Return lobjContainer.GetJobInfo(lpJob.Id)
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetJobInfo(lpProjectId As String, lpJobId As String) As IJobInfo Implements IProjectCatalog.GetJobInfo
    Try
      Dim lobjProjectDescription As IProjectDescription = GetProjectDescription(lpProjectId)
      Using lobjContainer As IProjectContainer = New SQLContainer(lobjProjectDescription.Location)
        Return lobjContainer.GetJobInfo(lpJobId)
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetDetailedJobInfo(lpProjectId As String, lpJobId As String) As IDetailedJobInfo Implements IProjectCatalog.GetDetailedJobInfo
    Try
      Dim lobjProjectDescription As IProjectDescription = GetProjectDescription(lpProjectId)
      Using lobjContainer As IProjectContainer = New SQLContainer(lobjProjectDescription.Location)
        Return lobjContainer.GetDetailedJobInfo(lpJobId)
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Public Function GetJobs(lpProjectId As String) As IJobInfoCollection
  '  Try
  '    Throw New NotImplementedException
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Public Sub MoveProject(lpProjectId As String, lpNewAreaId As String) Implements IProjectCatalog.MoveProject
    Try

      If Not ProjectExists(lpProjectId) Then
        Throw New ProjectDoesNotExistException(lpProjectId)
      End If

      Dim lobjProjectDescription As IProjectDescription = GetProjectDescription(lpProjectId)

      Dim lobjArea As IArea = GetArea(lpNewAreaId)

      If lobjArea Is Nothing Then
        Throw New ItemDoesNotExistException(lpNewAreaId, "The requested area does not exist.")
      End If

      lobjProjectDescription.Area = lobjArea

      Me.Container.SaveProject(lobjProjectDescription)

      ' Refresh the instance to make sure the areas reflect the move
      mobjInstance = GetCurrentCatalog(True)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function OpenProject(lpProjectId As String) As Project Implements IProjectCatalog.OpenProject
    Try
      Dim lobjProjectDescription As IProjectDescription = GetProjectDescription(lpProjectId)
      Dim lstrErrorMessage As String = String.Empty
      Using lobjContainer = New SQLContainer(lobjProjectDescription.Location)
        Dim lobjProject As Project = lobjContainer.OpenProject(lstrErrorMessage)
        If String.IsNullOrEmpty(lstrErrorMessage) Then
          Return lobjProject
        Else
          Throw New InvalidOperationException(lstrErrorMessage)
        End If
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#Region "Private Methods"

  'Private Shared Function GetCurrentCatalog() As IProjectCatalog
  '  Try
  '    Return GetCurrentCatalog(True)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    'Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  'Private Function CreateDbProjectPermissionsSpreadsheet(lpProject As Project) As Object
  '  Try
  '    Return ExcelHelper.CreateSpreadSheetFromDataTable("Provider Interface Matrix", ToInterfaceMatrixDataTable(False))
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Public Sub Open() Implements IProjectCatalog.Open
    Try
      GetCurrentCatalog(True)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub Refresh() Implements IProjectCatalog.Refresh
    Try
      GetCurrentCatalog(True)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function CreateArea(lpName As String, lpDescription As String) Implements IProjectCatalog.CreateArea
    Try
      Dim lobjArea As New Area(lpName, lpDescription)
      lobjArea.SetCatalog(Me)
      Return lobjArea
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Shared Function GetCurrentCatalog(ByVal lpForceRefresh As Boolean) As IProjectCatalog
    Try

      If lpForceRefresh = True OrElse mobjInstance Is Nothing OrElse mobjInstance.IsDisposed = True Then
        mobjContainer = New SQLContainer
        mobjInstance = mobjContainer.GetCurrentCatalog

        ' Make sure this computer is included as a participating node
        If mobjInstance.Nodes.Contains(Environment.MachineName) = False Then
          Dim lobjCurrentNode As INodeInfo = NodeInfo.Create
          mobjInstance.Nodes.Add(lobjCurrentNode)
          lobjCurrentNode.Save()
        End If

        If Reflection.Assembly.GetEntryAssembly IsNot Nothing Then
          CType(mobjInstance, ProjectCatalog).InitializeStatusChecker()
        End If

        'LogSession.LogMessage(String.Format("Project Catalog '{0}' initialized from container.", mobjInstance.Id))

      End If

      Return mobjInstance

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      'Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub MobjConfiguration_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles MobjConfiguration.PropertyChanged
    Try
      mobjContainer.SaveCatalogConfiguration(Me.Id, MobjConfiguration)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub InitializeStatusChecker()
    Try
      ' For automatically updating the status of this node

      ' Create an event to signal the timeout count threshold in the 
      ' timer callback. 
      mobjAutoEvent = New AutoResetEvent(False)
      CType(mobjInstance, ProjectCatalog).MobjStatusChecker = New NodeStatusChecker(mobjInstance.ThisNode, 10)

      ' Create an inferred delegate that invokes methods for the timer. 
      mobjTimerCallback = AddressOf MobjStatusChecker.UpdateStatus

      ' Create a timer that signals the delegate to invoke 
      ' CheckStatus after one second, and every 1/4 second 
      ' thereafter.
      stateTimer = New Timer(mobjTimerCallback, mobjAutoEvent, 1000, 1000 * ConnectionSettings.Instance.NodeStatusRefreshInterval)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetProjectCount() As Integer
    Try
      Dim lintProjectCount As Integer

      For Each lobjArea As IArea In Areas
        lintProjectCount += lobjArea.Projects.Count
      Next

      Return lintProjectCount

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Singleton Support"

  Public Shared ReadOnly Property ConnectionStringInitialized As Boolean
    Get
      Try
        Return ConnectionSettings.Instance.ProjectCatalogConnectionStringInitialized
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '  Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Shared Function Instance() As IProjectCatalog
    Try
      Dim lobjInstance As IProjectCatalog = Instance(False)
      Return lobjInstance
    Catch CatConEx As CatalogConnectionNotConfiguredException
      ApplicationLogging.LogException(CatConEx, Reflection.MethodBase.GetCurrentMethod)
      Helper.WriteConsoleErrorMessage(CatConEx.Message)
      '  Re-throw the exception to the caller
      Throw
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function Instance(ByVal lpForceRefresh As Boolean) As IProjectCatalog
    Try
      If lpForceRefresh = True Then
        mobjInstance = GetCurrentCatalog(lpForceRefresh)
      ElseIf mobjInstance Is Nothing OrElse mobjInstance.IsDisposed = True Then
        If ConnectionSettings.Instance.ProjectCatalogConnectionStringInitialized Then
          mobjInstance = GetCurrentCatalog(lpForceRefresh)
        Else
          Throw New CatalogConnectionNotConfiguredException
        End If
      End If
      mintReferenceCount += 1

      Return mobjInstance
    Catch CatConEx As CatalogConnectionNotConfiguredException
      ApplicationLogging.LogException(CatConEx, Reflection.MethodBase.GetCurrentMethod)
      Helper.WriteConsoleErrorMessage(CatConEx.Message)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#End Region

#Region "Friend Properties"

  'Friend Shared ReadOnly Property LogSession As Gurock.SmartInspect.Session
  '  Get
  '    Try
  '      Return mobjLogSession
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

#End Region

#Region "Public Methods"

  'Public Shared Sub UpdateFirebase()
  '  Try
  '    PushFirebaseUpdateAsync()
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Areas.Count = 0 Then
        lobjIdentifierBuilder.Append("No Areas")
      ElseIf Areas.Count = 1 Then
        lobjIdentifierBuilder.AppendFormat("{0} Area ({1} Projects)", Areas.First.Name, Areas.First.Projects.Count)
      Else
        lobjIdentifierBuilder.AppendFormat("{0} Areas ({1} Projects)", Areas.Count, ProjectCount)
      End If

      If Nodes.Count = 0 Then
        lobjIdentifierBuilder.Append(" - No Nodes")
      ElseIf Nodes.Count = 1 Then
        lobjIdentifierBuilder.AppendFormat(" - 1 Node ({0})", Nodes.First.ToString)
      Else
        lobjIdentifierBuilder.AppendFormat(" - {0} Nodes", Nodes.Count)
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  'Private Sub InitializeLogSession()
  '  Try
  '    mobjLogSession = SiAuto.Si.AddSession("ProjectCatalog")
  '    mobjLogSession.Color = System.Drawing.Color.LightBlue
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Private Sub MobjStatusChecker_NodeStatusRefreshed(sender As Object, ByRef e As NodeEventArgs) Handles MobjStatusChecker.NodeStatusRefreshed
    Try
      RaiseEvent NodeStatusRefreshed(mobjInstance, e)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Private Shared Async Sub PushFirebaseUpdateAsync()
  '  Try
  '    Dim lobjTask As Task =
  '    Task.Factory.StartNew(Sub()
  '                            PushFirebaseUpdate()
  '                          End Sub)
  '    Await lobjTask

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  'Private Shared Sub PushFirebaseUpdate()
  '  Try
  '    If Not ConnectionSettings.Instance.DisableNotifications AndAlso Helper.IsInternetAvailable() Then
  '      Dim lstrCatalogId As String = Instance.Id
  '      Dim lstrPutPath As String = String.Format("{0}catalogs/{1}/areas", FIREBASE_APP_URL, lstrCatalogId)
  '      'Using lobjFirebase As New FirebaseApplication(lstrPutPath)

  '      Dim lobjProjectInfo As IProjectInfo

  '      For Each lobjArea As IArea In Instance.Areas
  '        For Each lobjProjectDescription In lobjArea.Projects
  '          lobjProjectInfo = Instance.GetProjectInfo(lobjProjectDescription.Id)
  '          If lobjProjectInfo IsNot Nothing Then
  '            DirectCast(lobjProjectInfo, IFirebasePusher).UpdateFirebase(String.Format("{0}/{1}/{2}",
  '                                                           lstrPutPath, lobjArea.Name, lobjProjectInfo.Name))
  '          End If
  '        Next
  '      Next
  '      'End Using

  '      For Each lobjNode As NodeInfo In Instance.Nodes
  '        lobjNode.PushFirebaseUpdate(False)
  '      Next
  '    End If

  '  Catch ex As Exception
  '    'ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Just log it and move on, we don't want this to cause a problem
  '  End Try
  'End Sub

#End Region

#Region "IDisposable Support"
  Private disposedValue As Boolean ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        ' DISPOSETODO: dispose managed state (managed objects).
        'LogSession.LogMessage(String.Format("Project Catalog '{0}' disposing.", mobjInstance.Id))
      End If

      ' DISPOSETODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
      ' DISPOSETODO: set large fields to null.
      stateTimer?.Dispose()
    End If
    Me.disposedValue = True
  End Sub

  ' DISPOSETODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
  'Protected Overrides Sub Finalize()
  '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
  '    Dispose(False)
  '    MyBase.Finalize()
  'End Sub

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

#End Region

End Class
