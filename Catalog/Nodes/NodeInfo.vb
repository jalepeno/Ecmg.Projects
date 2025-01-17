'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  NodeInfo.vb
'   Description :  [type_description_here]
'   Created     :  1/8/2014 7:52:48 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.Text
Imports Documents.Core
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Projects.Converters

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}"), _
TypeConverter(GetType(ExpandableObjectConverter))> _
Public Class NodeInfo
  Inherits NotifyObject
  Implements INodeInfo

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mstrAddress As String = String.Empty
  Private menuRole As NodeRole
  Private menuStatus As NodeStatus
  Private mstrVersion As String = String.Empty
  Private mdatCreateDate As DateTime
  'Private mobjComputerInfo As IComputerInfo = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property Id As String Implements INodeInfo.Id
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

  'Public Property Id As String
  '  Get
  '    Try
  '      Return mstrId
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As String)
  '    Try
  '      mstrId = value
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  Public Property Name As String Implements INodeInfo.Name
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrName = value.ToLower()
        OnPropertyChanged("Name")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Description As String Implements INodeInfo.Description
    Get
      Try
        Return mstrDescription
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrDescription = value
        OnPropertyChanged("Description")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Address As String Implements INodeInfo.Address
    Get
      Try
        Return mstrAddress
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrAddress = value
        OnPropertyChanged("Address")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property Role As String Implements INodeInfo.Role
    Get
      Try
        Return RoleCode.ToString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Public Property Role As String
  '  Get
  '    Try
  '      Return RoleCode.ToString
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As String)
  '    Try
  '      menuRole = [Enum].Parse(GetType(NodeRole), value)
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  <JsonIgnore()> _
  Public Property RoleCode As NodeRole Implements INodeInfo.RoleCode
    Get
      Try
        Return menuRole
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As NodeRole)
      Try
        menuRole = value
        OnPropertyChanged("Role")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property Status As String Implements INodeInfo.Status
    Get
      Try
        Return StatusCode.ToString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Public Property Status As String
  '  Get
  '    Try
  '      Return StatusCode.ToString
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As String)
  '    Try
  '      menuStatus = [Enum].Parse(GetType(NodeStatus), value)
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  <JsonIgnore()> _
  Public Property StatusCode As NodeStatus Implements INodeInfo.StatusCode
    Get
      Try
        Return menuStatus
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As NodeStatus)
      Try
        If menuStatus <> value Then
          menuStatus = value
          'PushFirebaseStatusUpdate(value)
          OnPropertyChanged("Status")
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property Version As String Implements INodeInfo.Version
    Get
      Try
        Return mstrVersion
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Public Property Version As String
  '  Get
  '    Try
  '      Return mstrVersion
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As String)
  '    Try
  '      mstrVersion = value
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  Public ReadOnly Property CreateDate As Date Implements INodeInfo.CreateDate
    Get
      Try
        Return mdatCreateDate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Public Property CreateDate As Date
  '  Get
  '    Try
  '      Return mdatCreateDate
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As Date)
  '    Try
  '      mdatCreateDate = value
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  '<TypeConverter(GetType(ExpandableObjectConverter))> _
  'Public ReadOnly Property ComputerInfo As IComputerInfo Implements INodeInfo.ComputerInfo
  '  Get
  '    Try
  '      Return mobjComputerInfo
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  'Public Property ComputerInfo As ComputerInfo
  '  Get
  '    Try
  '      Return mobjComputerInfo
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As ComputerInfo)
  '    Try
  '      mobjComputerInfo = value
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

  Public Function ToJson() As String Implements INodeInfo.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None, New NodeInfoConverter())
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented, New NodeInfoConverter())
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToStatusJson() As String Implements INodeInfo.ToStatusJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None, New NodeInfoStatusConverter())
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented, New NodeInfoStatusConverter())
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub Save() Implements INodeInfo.Save
    Try
      If Not Helper.CallStackContainsMethodName("NodeInfo Create", "DeserializeObject") Then
        ProjectCatalog.Instance.Container.SaveNode(Me)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shared Function FromJson(lpJsonString As String) As INodeInfo
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(NodeInfo), New NodeInfoConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Constructors"

  Public Sub New()

    'Try
    '  mstrId = Guid.NewGuid.ToString
    '  mdatCreateDate = Now
    '  RoleCode = NodeRole.Worker
    '  StatusCode = NodeStatus.Available
    '  mobjComputerInfo = Computer.ComputerInfo.create
    '  Name = mobjComputerInfo.OperatingSystem.Name

    'Catch ex As Exception
    '  ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '  ' Re-throw the exception to the caller
    '  Throw
    'End Try

  End Sub

  'Friend Sub New(lpId As String, _
  '               lpName As String, _
  '               lpDescription As String, _
  '               lpAddress As String, _
  '               lpRole As NodeRole, _
  '               lpStatus As NodeStatus, _
  '               lpVersion As String, _
  '               lpCreateDate As DateTime, _
  '               lpComputerInfo As IComputerInfo)
  '  Try
  '    mstrId = lpId
  '    mstrName = lpName
  '    mstrDescription = lpDescription
  '    mstrAddress = lpAddress
  '    menuRole = lpRole
  '    menuStatus = lpStatus
  '    mstrVersion = lpVersion
  '    mdatCreateDate = lpCreateDate
  '    mobjComputerInfo = lpComputerInfo
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Friend Sub New(lpId As String,
                 lpName As String,
                 lpDescription As String,
                 lpAddress As String,
                 lpRole As NodeRole,
                 lpStatus As NodeStatus,
                 lpVersion As String,
                 lpCreateDate As DateTime)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
      mstrAddress = lpAddress
      menuRole = lpRole
      menuStatus = lpStatus
      mstrVersion = lpVersion
      mdatCreateDate = lpCreateDate
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Shared Function Create() As INodeInfo
    Try
      Dim lobjNodeInfo As New NodeInfo

      With lobjNodeInfo
        .mstrId = Guid.NewGuid.ToString
        .mdatCreateDate = Now
        .RoleCode = NodeRole.Worker
        .StatusCode = NodeStatus.Available
        .mstrVersion = FrameworkVersion.CurrentVersion
        .Name = Environment.MachineName
      End With

      Return lobjNodeInfo

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub OnPropertyChanged(sProp As String)
    Try
      MyBase.OnPropertyChanged(sProp)
      Save()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function ToString() As String
    Try
      Return DebuggerIdentifier()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Friend Methods"

  'Friend Sub Update()
  '  Try
  '    Dim lstrCurrentAssemblyName As String = Assembly.GetEntryAssembly.FullName

  '    If Not IsFilematicaInstalled() OrElse Not IsFilematicaServiceRunning() OrElse StatusCode <> NodeStatus.Inactive Then
  '      StatusCode = NodeStatus.Inactive
  '      'ApplicationLogging.WriteLogEntry("Filematica Service Determined to be inactive.", Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Information, 123654)
  '      'DirectCast(ProjectCatalog.Instance(), ProjectCatalog).'LogSession.LogMessage("Filematica Service Determined to be inactive.")
  '    Else
  '      If Assembly.GetEntryAssembly IsNot Nothing Then
  '        If Assembly.GetEntryAssembly.FullName.ToLower.StartsWith("filematicaservice") Then
  '          If StatusCode <> NodeStatus.Working Then
  '            StatusCode = NodeStatus.Available
  '          End If
  '        Else
  '          Try
  '            ' We want to know the status of the service, not any other running apps
  '            Dim lobjServiceClient As ObjectHandle = Activator.CreateInstance("Ecmg.Cts.ServiceClient", "Ecmg.Cts.ServiceClient.FilematicaService")

  '            If lobjServiceClient.Unwrap.ServiceIsAvailable Then
  '              If lobjServiceClient.Unwrap.IsWorking Then
  '                StatusCode = NodeStatus.Working
  '              Else
  '                StatusCode = NodeStatus.Available
  '              End If
  '            Else
  '              StatusCode = NodeStatus.Inactive
  '            End If

  '          Catch ex As Exception
  '            StatusCode = NodeStatus.Inactive
  '          End Try
  '        End If
  '      End If
  '    End If
  '    mstrVersion = FrameworkVersion.CurrentVersion
  '    'ComputerInfo.OperatingSystem.Refresh()
  '    Save()
  '    ' PushFirebaseUpdate(True)
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

      'If ComputerInfo IsNot Nothing Then
      '  lobjIdentifierBuilder.AppendFormat("{0}: {1}-{2} {3} ({4})", Name, Role, Status, Version, ComputerInfo.ToString)
      'Else
      lobjIdentifierBuilder.AppendFormat("{0}: {1}-{2} {3} (ComputerInfo not available)", Name, Role, Status, Version)
      'End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Sub NodeInfo_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
    Try
      Select Case e.PropertyName
        Case "Name"
          If String.IsNullOrEmpty(Address) Then
            Address = Name
          End If
      End Select
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Private Function IsFilematicaInstalled() As Boolean
  '  Try
  '    Dim lblnIsServiceInstalled As Boolean

  '    For Each lobjController As ServiceController In ServiceController.GetServices
  '      If lobjController.ServiceName = FILEMATICA_SERVICE_NAME Then
  '        lblnIsServiceInstalled = True
  '        Exit For
  '      End If
  '    Next

  '    Return lblnIsServiceInstalled

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  'Private Function IsFilematicaServiceRunning() As Boolean
  '  Try

  '    Using lobjServiceController As New ServiceController(FILEMATICA_SERVICE_NAME)
  '      If lobjServiceController Is Nothing Then
  '        Return False
  '      End If

  '      Select Case lobjServiceController.Status
  '        Case ServiceControllerStatus.Running
  '          Return True
  '        Case Else
  '          Return False
  '      End Select
  '    End Using

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  'Friend Sub PushFirebaseUpdate(lpStatusOnly As Boolean)
  '  Try
  '    If Not ConnectionSettings.Instance.DisableNotifications Then
  '      Dim lstrCatalogId As String = ProjectCatalog.Instance.Id
  '      Dim lstrNodeInfoUrl As String = String.Format("catalogs/{0}/nodes/{1}", lstrCatalogId, Me.Id)

  '      Using lobjFirebase As New FirebaseApplication(FIREBASE_APP_URL)
  '        If lobjFirebase.Available Then
  '          If lpStatusOnly Then
  '            ' lobjFirebase.Post(lstrNodeInfoUrl, Me.ToStatusJson())
  '            ' I still can't get status only updates to work, we will have to stick with this for now.
  '            lobjFirebase.Put(lstrNodeInfoUrl, Me.ToJson())
  '          Else
  '            lobjFirebase.Put(lstrNodeInfoUrl, Me.ToJson())
  '          End If
  '        End If
  '      End Using
  '    End If
  '  Catch ex As Exception
  '    ' ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Just log it and move on, we don't want this to cause a problem
  '  End Try
  'End Sub

  'Friend Sub PushFirebaseStatusUpdate(lpStatus As NodeStatus)
  '  Try
  '    If Not ConnectionSettings.Instance.DisableNotifications Then
  '      Dim lstrCatalogId As String = ProjectCatalog.Instance.Id
  '      'Dim lstrNodeInfoUrl As String = String.Format("catalogs/{0}/nodes/{1}", lstrCatalogId, Me.Id)

  '      Using lobjFirebase As New FirebaseApplication(String.Format("{0}catalogs/{1}/nodes/{2}", FIREBASE_APP_URL, lstrCatalogId, Me.Id))
  '        If lobjFirebase.Available Then
  '          Dim lstrSubmission As String = JsonConvert.SerializeObject(lpStatus.ToString)
  '          lobjFirebase.Put("/status", lstrSubmission)
  '        End If
  '      End Using
  '    End If
  '  Catch ex As Exception
  '    ' ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Just log it and move on, we don't want this to cause a problem
  '  End Try
  'End Sub

#End Region

End Class
