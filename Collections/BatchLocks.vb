'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  BatchLocks.vb
'   Description :  [type_description_here]
'   Created     :  5/16/2013 4:29:20 PM
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

Public Class BatchLocks
  Inherits CCollection(Of BatchLock)
  Implements IBatchLocks

#Region "Class Variables"

  Private mobjProject As Project
  Private mobjEnumerator As IEnumeratorConverter(Of IBatchLock)

#End Region

#Region "IBatchLocks Implementation"

  Public Overloads Sub Add(item As IBatchLock) Implements ICollection(Of IBatchLock).Add
    Try
      Add(CType(item, BatchLock))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of IBatchLock).Clear
    Try
      If Project IsNot Nothing Then
        Project.UnlockBatches()
        MyBase.Clear()
      Else
        Throw New ProjectNotSetException
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(item As IBatchLock) As Boolean Implements ICollection(Of IBatchLock).Contains
    Try
      Return Contains(CType(item, BatchLock))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Function Contains(batchId As String) As Boolean
    Try
      Dim list As Object = From lobjBatchLock In Items Where _
        (String.Compare(lobjBatchLock.BatchId, batchId, True) = 0) Select lobjBatchLock

      For Each lobjProperty As BatchLock In list
        Return True
      Next

      Return False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As IBatchLock, arrayIndex As Integer) Implements ICollection(Of IBatchLock).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of IBatchLock).Count
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

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IBatchLock).IsReadOnly
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

  Public Overloads Function Remove(item As IBatchLock) As Boolean Implements ICollection(Of IBatchLock).Remove
    Try
      If Project IsNot Nothing Then
        Project.UnlockBatch(item.BatchId)
        Return Remove(CType(item, BatchLock))
      Else
        Throw New ProjectNotSetException
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Function GetEnumerator() As IEnumerator(Of IBatchLock) Implements IEnumerable(Of IBatchLock).GetEnumerator
    Try
      Return IPropertyEnumerator.GetEnumerator
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

  Public Sub New(lpProject As Project)
    Try
      mobjProject = lpProject
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Properties"

  Public Property Project As Project
    Get
      Try
        Return mobjProject
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Friend Set(value As Project)
      Try
        mobjProject = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#Region "Private Properties"

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of IBatchLock)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of IBatchLock)(Me.ToArray, GetType(BatchLock), GetType(IBatchLock))
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
