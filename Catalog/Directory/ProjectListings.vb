'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectListings.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 10:26:24 AM
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
#End Region

Public Class ProjectListings
  Inherits CCollection(Of ProjectListing)
  Implements IProjectListings

#Region "Class Variables"

  Private mobjEnumerator As IEnumeratorConverter(Of IProjectListing)

#End Region

#Region "IProjectListings Implementation"

  Public Shadows Function Item(id As String) As IProjectListing Implements IProjectListings.Item
    Try

      Dim list As Object = From lobjItem In Items Where _
        (String.Compare(lobjItem.Id, id, True) = 0) Or _
        (String.Compare(lobjItem.Name, id, True) = 0) Select lobjItem

      For Each lobjItem As IProjectListing In list
        Return lobjItem
      Next

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shadows Sub Add(item As ProjectListing)
    Try
      If Not Contains(item) Then
        MyBase.Add(item)
      End If
      Sort()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shadows Sub Add(item As IProjectListing) Implements ICollection(Of IProjectListing).Add
    Try
      Dim lobjProjectListing As ProjectListing = Nothing

      If TypeOf item Is ProjectListing Then
        lobjProjectListing = item
      ElseIf TypeOf item Is ProjectDescription Then
        lobjProjectListing = New ProjectListing(item)
      ElseIf TypeOf item Is Project Then
        lobjProjectListing = New ProjectListing(item)
      End If

      If Not Contains(lobjProjectListing) Then
        MyBase.Add(lobjProjectListing)
      End If
      Sort()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of IProjectListing).Clear
    Try
      MyBase.Clear()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(item As IProjectListing) As Boolean Implements ICollection(Of IProjectListing).Contains
    Try
      ' Return Contains(CType(item, ProjectDescription))
      Dim list As Object = From lobjProject In Items Where _
        (String.Compare(lobjProject.Name, item.Name, True) = 0) Or _
        (String.Compare(lobjProject.Id, item.Id, True) = 0) Select lobjProject

      For Each lobjProperty As IProjectListing In list
        Return True
      Next

      Return False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As IProjectListing, arrayIndex As Integer) Implements ICollection(Of IProjectListing).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of IProjectListing).Count
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

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IProjectListing).IsReadOnly
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

  Public Overloads Function Remove(item As IProjectListing) As Boolean Implements ICollection(Of IProjectListing).Remove
    Try
      MyBase.Remove(CType(item, ProjectListing))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shadows Sub Sort() Implements IProjectListings.Sort
    Try
      MyBase.Sort()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shared Function FromJson(lpJsonString As String) As IProjectListings
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(ProjectListings))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToJson() As String Implements IProjectListings.ToJson
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

#Region "Public Methods"

  Public Overloads Function GetEnumerator() As IEnumerator(Of IProjectListing) Implements IEnumerable(Of IProjectListing).GetEnumerator
    Try
      Return IPropertyEnumerator.GetEnumerator
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

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of IProjectListing)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of IProjectListing)(Me.ToArray, GetType(ProjectListing), GetType(IProjectListing))
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

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpProjectDescriptions As IProjectDescriptions)
    Try
      For Each lobjProjectDescription As IProjectDescription In lpProjectDescriptions
        MyBase.Add(New ProjectListing(lobjProjectDescription))
      Next
      Sort()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
