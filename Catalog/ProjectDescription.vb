
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectDescription.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 10:27:06 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Core
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Projects.Converters

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class ProjectDescription
  Inherits NotifyObject
  Implements IProjectDescription

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty
  Private ReadOnly mdatCreateDate As DateTime
  Private mlngItemsProcessed As Long
  Private ReadOnly mobjWorkSummary As IWorkSummary = Nothing
  Private mobjLocation As IItemsLocation = Nothing
  Private mobjArea As IArea = Nothing

#End Region

#Region "Public Properties"

  <JsonProperty("id")>
  Public Property Id As String Implements IProjectDescription.Id
    Get
      Try
        Return mstrId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrId = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <JsonProperty("name")>
  Public Property Name As String Implements IProjectDescription.Name
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
        mstrName = value
        OnPropertyChanged("Name")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <JsonProperty("createDate")>
  Public ReadOnly Property CreateDate As Date Implements IProjectDescription.CreateDate
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

  <JsonProperty("itemsProcessed")>
  Public Property ItemsProcessed As Long Implements IProjectDescription.ItemsProcessed
    Get
      Try
        Return mlngItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Long)
      Try
        mlngItemsProcessed = value
        OnPropertyChanged("ItemsProcessed")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <JsonIgnore()>
  Public Property Area As IArea Implements IProjectDescription.Area
    Get
      Try
        Return mobjArea
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As IArea)
      Try
        mobjArea = value
        If Not Helper.CallStackContainsMethodName("ToJson") Then
          If Not mobjArea.Projects.Contains(Me) Then
            mobjArea.Projects.Add(Me)
          End If
        End If
        OnPropertyChanged("Area")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod, value)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <JsonProperty("area")>
  ReadOnly Property AreaName As String Implements IProjectDescription.AreaName
    Get
      Try
        Return Area.Name
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("location")>
  Public Property Location As IItemsLocation Implements IProjectDescription.Location
    Get
      Try
        Return mobjLocation
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As IItemsLocation)
      Try
        mobjLocation = value
        OnPropertyChanged("Location")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <JsonProperty("workSummary")>
  Public ReadOnly Property WorkSummary As IWorkSummary Implements IProjectDescription.WorkSummary
    Get
      Try
        Return mobjWorkSummary
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Public Property AreaListing As IAreaListing Implements IProjectListing.Area
  '  Get
  '    Try
  '      Return mobjArea
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As IAreaListing)
  '    Try
  '      mobjArea = value
  '      If Not mobjArea.Projects.Contains(Me) Then
  '        mobjArea.Projects.Add(Me)
  '      End If
  '      OnPropertyChanged("Area")
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpProjectDescription As IProjectDescription)
    Try
      Me.Id = lpProjectDescription.Id
      Me.Name = lpProjectDescription.Name
      Me.Location = lpProjectDescription.Location
      mdatCreateDate = lpProjectDescription.CreateDate
      Me.Area = lpProjectDescription.Area
      Me.ItemsProcessed = lpProjectDescription.ItemsProcessed
      mobjWorkSummary = lpProjectDescription.WorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpId As String, _
                 lpName As String, _
                 lpLocation As String, _
                 lpCreateDate As DateTime, _
                 lpArea As IArea, _
                 lpItemsProcessed As Long, _
                 lpWorkSummary As IWorkSummary)
    Me.New(lpId, lpName, New ItemsLocation(lpLocation), lpCreateDate, lpArea, lpItemsProcessed, lpWorkSummary)
  End Sub

  Public Sub New(lpId As String, _
                 lpName As String, _
                 lpLocation As IItemsLocation, _
                 lpCreateDate As DateTime, _
                 lpArea As IArea, _
                 lpItemsProcessed As Long, _
                 lpWorkSummary As IWorkSummary)
    Try
      mstrId = lpId
      mstrName = lpName
      Me.Location = lpLocation
      mdatCreateDate = lpCreateDate
      Me.Area = lpArea
      mlngItemsProcessed = lpItemsProcessed
      mobjWorkSummary = lpWorkSummary
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Function GetProjectInfo() As IProjectInfo Implements IProjectDescription.GetProjectInfo
    Try
      If Me.Location Is Nothing Then
        Throw New InvalidOperationException("Unable to get project info, the location is not initialized.")
      End If
      Using lobjContainer As New SQLContainer(Me.Location)
        Return lobjContainer.GetProjectInfo()
      End Using
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub Save() Implements IProjectDescription.Save
    Try
      If Me.Area Is Nothing Then
        Throw New InvalidOperationException("Area not set.")
      End If

      Me.Area.Catalog.Container.SaveProject(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod, Me)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shared Function FromJson(lpJsonString As String) As IProjectDescription
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(ProjectDescription), New ProjectDescriptionConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToJson() As String Implements IProjectDescription.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None, New ProjectDescriptionConverter())
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented, New ProjectDescriptionConverter())
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      lobjIdentifierBuilder.AppendFormat("{0}: {1} ({2} Items) - {3} ", Area.Name, Name, ItemsProcessed, Id)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

End Class
