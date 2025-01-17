'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobIdentifiers.vb
'   Description :  [type_description_here]
'   Created     :  5/13/2014 10:09:47 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities

#End Region

Public Class JobIdentifiers
  Inherits CCollection(Of JobIdentifier)
  Implements IJobIdentifiers

#Region "Class Variables"

  Private mobjEnumerator As IEnumeratorConverter(Of IJobIdentifier)

#End Region

#Region "IJobIdentifiers Implementation"

  Public Shadows Sub Add(item As JobIdentifier)
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

  Public Shadows Sub Add(item As IJobIdentifier) Implements ICollection(Of IJobIdentifier).Add
    Try
      Dim lobjIdentifier As JobIdentifier = Nothing

      If TypeOf item Is JobIdentifier Then
        lobjIdentifier = item
      Else
        Throw New ArgumentOutOfRangeException("item", String.Format("Unexpected type ({0})", item.GetType.Name))
      End If

      If Not Contains(lobjIdentifier) Then
        MyBase.Add(lobjIdentifier)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of IJobIdentifier).Clear
    Try
      MyBase.Clear()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(item As IJobIdentifier) As Boolean Implements ICollection(Of IJobIdentifier).Contains
    Try
      Dim list As Object = From lobjIdentifier In Items Where _
        (String.Compare(lobjIdentifier.Name, item.Name, True) = 0) Or _
        (String.Compare(lobjIdentifier.Id, item.Id, True) = 0) Select lobjIdentifier

      For Each lobjProperty As IJobIdentifier In list
        Return True
      Next

      Return False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As IJobIdentifier, arrayIndex As Integer) Implements ICollection(Of IJobIdentifier).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of IJobIdentifier).Count
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

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IJobIdentifier).IsReadOnly
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

  Public Overloads Function Remove(item As IJobIdentifier) As Boolean Implements ICollection(Of IJobIdentifier).Remove
    Try
      MyBase.Remove(CType(item, ProjectDescription))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Function GetEnumerator() As IEnumerator(Of IJobIdentifier) Implements IEnumerable(Of IJobIdentifier).GetEnumerator
    Try
      Return IPropertyEnumerator.GetEnumerator
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Properties"

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of IJobIdentifier)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of IJobIdentifier)(Me.ToArray, GetType(JobIdentifier), GetType(IJobIdentifier))
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
