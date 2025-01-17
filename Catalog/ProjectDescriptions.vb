'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectDescriptions.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 10:32:29 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Projects.Converters

#End Region

Public Class ProjectDescriptions
  Inherits CCollection(Of ProjectDescription)
  Implements IProjectDescriptions

#Region "Class Variables"

  Private mobjEnumerator As IEnumeratorConverter(Of IProjectDescription)

#End Region

#Region "IProjectDescriptions Implementation"

  Public Shadows Sub Add(item As ProjectDescription)
    Try
      If Not Contains(item) Then
        MyBase.Add(item)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shadows Sub Add(item As IProjectDescription) Implements ICollection(Of IProjectDescription).Add
    Try
      Dim lobjProjectDescription As ProjectDescription = Nothing

      If TypeOf item Is ProjectDescription Then
        lobjProjectDescription = item
      ElseIf TypeOf item Is Project Then
        lobjProjectDescription = New ProjectDescription(item)
      End If

      If Not Contains(lobjProjectDescription) Then
        MyBase.Add(lobjProjectDescription)
      End If

      If Not String.IsNullOrEmpty(item.Name) Then
        If Not Helper.CallStackContainsMethodName("GetCurrentCatalog", "FromJson", "NewProjectPanel_Loaded") Then
          lobjProjectDescription.Save()
        End If
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of IProjectDescription).Clear
    Try
      MyBase.Clear()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(item As IProjectDescription) As Boolean Implements ICollection(Of IProjectDescription).Contains
    Try
      ' Return Contains(CType(item, ProjectDescription))
      Dim list As Object = From lobjProject In Items Where _
        (String.Compare(lobjProject.Name, item.Name, True) = 0) Or _
        (String.Compare(lobjProject.Id, item.Id, True) = 0) Select lobjProject

      For Each lobjProperty As IProjectDescription In list
        Return True
      Next

      Return False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As IProjectDescription, arrayIndex As Integer) Implements ICollection(Of IProjectDescription).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of IProjectDescription).Count
    Get
      Try
        Return MyBase.Count
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IProjectDescription).IsReadOnly
    Get
      Try
        Return MyBase.IsReadOnly
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overloads Function Remove(item As IProjectDescription) As Boolean Implements ICollection(Of IProjectDescription).Remove
    Try
      MyBase.Remove(CType(item, ProjectDescription))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Public Methods"

  Public Overloads Function GetEnumerator() As IEnumerator(Of IProjectDescription) Implements IEnumerable(Of IProjectDescription).GetEnumerator
    Try
      Return IPropertyEnumerator.GetEnumerator
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function FromJson(lpJsonString As String) As IProjectDescriptions
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(ProjectDescriptions), New ProjectDescriptionsConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToJson() As String
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None, New ProjectDescriptionsConverter())
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented, New ProjectDescriptionsConverter())
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Friend Methods"

  Friend Function ToProjectListings() As IProjectListings Implements IProjectDescriptions.ToProjectListings
    Try
      Return New ProjectListings(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Methods"

  Private Sub ProjectDescriptions_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles Me.CollectionChanged
    Try
      If e.Action = Specialized.NotifyCollectionChangedAction.Add Then
        If ((e.NewStartingIndex > 0) AndAlso (Me(e.NewStartingIndex).Equals(Me(e.NewStartingIndex - 1)))) Then
          RemoveAt(e.NewStartingIndex)
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Properties"

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of IProjectDescription)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of IProjectDescription)(Me.ToArray, GetType(ProjectDescription), GetType(IProjectDescription))
        End If
        Return mobjEnumerator
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

End Class
