'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  AreaInfo.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:48:51 PM
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
Public Class AreaInfo
  Implements IAreaInfo
  Implements IComparable

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mobjProjects As IProjectInfoCollection = New ProjectInfoCollection

#End Region

#Region "IAreaInfo Implementation"

  <JsonProperty("id")> _
  Public ReadOnly Property Id As String Implements IObjectDescriptor.Id
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

  <JsonProperty("name")> _
  Public ReadOnly Property Name As String Implements IObjectDescriptor.Name
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("description")> _
  Public ReadOnly Property Description As String Implements IObjectDescriptor.Description
    Get
      Try
        Return mstrDescription
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("projects")> _
  Public ReadOnly Property Projects As IProjectInfoCollection Implements IAreaInfo.Projects
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

  Public Function ToJson() As String Implements IAreaInfo.ToJson
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

#End Region

#Region "IComparable Implementation"

  Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
    Try
      Return Name.CompareTo(obj.Name)
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

  Friend Sub New(lpArea As IArea)
    Try
      mstrId = lpArea.Id
      mstrName = lpArea.Name
      mstrDescription = lpArea.Description

      For Each lobjProject As IProjectDescription In lpArea.Projects
        Me.Projects.Add(lobjProject.GetProjectInfo())
      Next

      Me.Projects.Sort()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub New(lpId As String, _
                 lpName As String, _
                 lpDescription As String, _
                 lpProjects As IProjectInfoCollection)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
      mobjProjects = lpProjects
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

End Class
