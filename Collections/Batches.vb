'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.SerializationUtilities
Imports Documents.Utilities
Imports Operations

#End Region

Public Class Batches
  Inherits CCollection(Of Batch)

#Region "Class Variables"

  'This is a reference to each Batch item's parent job
  Private mobjJob As Job

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(ByVal lpJob As Job)

    Try
      mobjJob = lpJob

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Public Properties"

  Public ReadOnly Property Job() As Job
    Get
      Return mobjJob
    End Get
  End Property

#End Region

#Region "Public Methods"

  Public Overloads Sub Add(ByVal lpBatch As Batch)

    Try
      lpBatch.Number = Count + 1
      lpBatch.SetJob(Me.Job)
      MyBase.Add(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Overridable Overloads Function Contains(ByVal item As Batch) As Boolean
    Try
      Dim lobjBatch As Batch = Me.Item(item.Id)
      If lobjBatch IsNot Nothing Then
        Return True
      Else
        Return False
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Default Shadows Property Item(ByVal lpId As String) As Batch
    Get

      Try
        Return GetBatchById(lpId)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
    Set(ByVal value As Batch)
      Throw New NotImplementedException
    End Set
  End Property

  Default Shadows Property Item(ByVal index As Integer) As Batch
    Get
      Return MyBase.Item(index)
    End Get
    Set(ByVal value As Batch)
      MyBase.Item(index) = value
    End Set
  End Property

#End Region

#Region "Friend Methods"

  'Friend Sub UpdateProcess(ByVal lpProcess As IProcess)
  '  Try
  '    For Each lobjBatch As Batch In Me
  '      'lobjBatch.Process = lpProcess.Clone
  '      lobjBatch.Process = lpProcess
  '    Next
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Friend Sub UpdateProcess(ByVal lpXML As String)
    Try
      For Each lobjBatch As Batch In Me
        lobjBatch.Process = Serializer.Deserialize.XmlString(lpXML, GetType(Process))
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Methods"

  Private Function GetBatchById(ByVal lpId As String) As Batch

    Try

      Dim lobjBatch As Batch = (From b In Me Where b.Id = lpId Select b).FirstOrDefault

      Return lobjBatch

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

#End Region

End Class
