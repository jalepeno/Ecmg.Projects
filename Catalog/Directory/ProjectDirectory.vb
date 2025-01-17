'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectDirectory.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 1:54:56 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities
Imports Newtonsoft.Json

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class ProjectDirectory
  Implements IProjectDirectory

#Region "Class Variables"

  'Private mobjAreas As IAreaListings = New AreaListings
  Private mobjProjects As IProjectListings = New ProjectListings

#End Region

#Region "IProjectDirectory Implementation"

  Public ReadOnly Property ProjectCount As Integer Implements IProjectDirectory.ProjectCount
    Get
      Try
        Return Projects.Count
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Projects As IProjectListings Implements IProjectDirectory.Projects
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

  Public Function GetProject(lpProjectId As String) As IProjectListing Implements IProjectDirectory.GetProject
    Try
      Return Projects.FirstOrDefault(Function(Project) Project.Id = lpProjectId Or Project.Name = lpProjectId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  <JsonIgnore()> _
  Public ReadOnly Property IsDisposed As Boolean Implements IProjectDirectory.IsDisposed
    Get
      Return disposedValue
    End Get
  End Property

  Public Function ToJson() As String Implements IProjectDirectory.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None)
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function FromJson(lpJsonString As String) As IProjectDirectory
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(ProjectDirectory))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpCatalog As IProjectCatalog)
    Try
      For Each lobjArea As IAreaListing In lpCatalog.Areas.ToAreaListings
        For Each lobjProject As IProjectListing In lobjArea.Projects
          Projects.Add(New ProjectListing(lobjProject))
        Next
      Next
      Projects.Sort()
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

      If Projects.Count = 0 Then
        lobjIdentifierBuilder.Append("No Projects")
      ElseIf Projects.Count = 1 Then
        lobjIdentifierBuilder.Append("1 Project")
      Else
        lobjIdentifierBuilder.AppendFormat("{0} Projects", ProjectCount)
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "IDisposable Support"
  Private disposedValue As Boolean ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        ' DISPOSETODO: dispose managed state (managed objects).
      End If

      ' DISPOSETODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
      ' DISPOSETODO: set large fields to null.
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
