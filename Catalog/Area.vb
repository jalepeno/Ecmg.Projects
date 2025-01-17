'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  Area.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 2:31:21 PM
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

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class Area
  Inherits NotifyObject
  Implements IArea
  'Implements IAreaListing
  Implements IDescription
  Implements IObjectDescriptor

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mobjProjects As IProjectDescriptions = New ProjectDescriptions
  Friend mobjCatalog As IProjectCatalog = Nothing

#End Region

#Region "IArea Implementation"

#Region "Public Properties"

  Public ReadOnly Property Id As String Implements IArea.Id, IObjectDescriptor.Id
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

  Public Property Name As String Implements IDescription.Name, INamedItem.Name
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

  Public Property Description As String Implements IDescription.Description
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

  Public ReadOnly Property Projects As IProjectDescriptions Implements IArea.Projects
    Get
      Try
        Return mobjProjects
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Catalog As IProjectCatalog Implements IArea.Catalog
    Get
      Try
        Return mobjCatalog
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Operators"

  Public Shared Operator =(ByVal v1 As Area, ByVal v2 As IArea) As Boolean
    Try
      Return v1.Equals(v2)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Operator

  Public Shared Operator <>(ByVal v1 As Area, ByVal v2 As IArea) As Boolean
    Try
      Return Not v1 = v2
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Operator

#End Region

#Region "Public Methods"

  Public Sub Delete() Implements IArea.Delete
    Try
      Catalog.Container.DeleteArea(Me.Id)
      ProjectCatalog.Instance.Refresh()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub Rename(lpNewName As String) Implements IArea.Rename
    Try
      Dim lstrCurrentName As String = Me.Name

      Me.Name = lpNewName

      If Me.Description.Contains(lstrCurrentName) Then
        Me.Description = Me.Description.Replace(lstrCurrentName, lpNewName)
      End If

      Me.Save()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub Save() Implements IArea.Save
    Try
      If Me.Catalog Is Nothing Then
        Throw New InvalidOperationException("The catalog is not initialized.")
      End If
      Me.Catalog.Container.SaveArea(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub AddProject(lpProject As IProjectDescription) Implements IArea.AddProject
    Try
      Beep()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function OpenProject(lpProjectId As String) As Project Implements IArea.OpenProject ' , IAreaListing.OpenProject
    Try

      If TypeOf Catalog.Container Is IProjectContainer Then
        Dim lstrErrorMessage As String = String.Empty
        Dim lobjProject As Project = CType(Catalog.Container, IProjectContainer).OpenProject(lstrErrorMessage)
        If Not String.IsNullOrEmpty(lstrErrorMessage) Then
          Dim lstrLogMessage As String = String.Format("Error opening project id {0}: {1}", lpProjectId, lstrErrorMessage)
          If lobjProject IsNot Nothing Then
            ApplicationLogging.WriteLogEntry(lstrLogMessage, Reflection.MethodBase.GetCurrentMethod, TraceEventType.Warning, 63401)
          Else
            ApplicationLogging.WriteLogEntry(lstrLogMessage, Reflection.MethodBase.GetCurrentMethod, TraceEventType.Error, 63402)
          End If
        End If
        Return lobjProject
      Else
        Throw New InvalidOperationException("The catalog container is not a project container.")
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Sub SetCatalog(lpCatalog As IProjectCatalog) Implements IArea.SetCatalog
    Try
      mobjCatalog = lpCatalog
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Name)

      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0}", Description)

      If Projects.Count = 0 Then
        lobjIdentifierBuilder.Append(" (No Projects)")
      Else
        lobjIdentifierBuilder.AppendFormat(" ({0} Projects)", Projects.Count)
      End If


      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#End Region

#Region "IAreaListing Implementation"

  'Public ReadOnly Property Directory As IProjectDirectory Implements IAreaListing.Directory
  '  Get
  '    Try
  '      Return mobjCatalog
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  'Public ReadOnly Property Id1 As String Implements IAreaListing.Id
  '  Get
  '    Try
  '      Return mstrId
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  'Public ReadOnly Property ProjectListings As IProjectListings Implements IAreaListing.Projects
  '  Get
  '    Try
  '      Return mobjProjects.ToProjectListings
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  Friend Function ToAreaListing() As IAreaListing Implements IArea.ToAreaListing
    Try
      Return New AreaListing(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Constructors"

  Public Sub New()
    Try
      mstrId = Guid.NewGuid.ToString
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpName As String)
    Me.New()
    Try
      mstrName = lpName
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpName As String, lpDescription As String)
    Me.New(lpName)
    Try
      mstrDescription = lpDescription
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub New(lpId As String, lpName As String, lpDescription As String)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

  Public ReadOnly Property ObjectDescription As String Implements IObjectDescriptor.Description
    Get
      Try
        Return Me.Description
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ObjectName As String Implements IObjectDescriptor.Name
    Get
      Try
        Return Me.Name
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

End Class
