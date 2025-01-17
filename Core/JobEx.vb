' ********************************************************************************
' '  Document    :  JobEx.vb
' '  Description :  [type_description_here]
' '  Created     :  11/13/2012-15:54:50
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

#Region "Imports"

Imports System.ComponentModel
Imports System.Data
Imports System.Threading
Imports Documents.Core
Imports Documents.Utilities
Imports Operations
Imports Operations.OperationEnumerations

#End Region

Partial Public Class Job

#Region "Class Variables"

  Private mintBatchWorkersRunning As Integer = 0
  Private mobjIProcessResults As ProcessResults

#End Region

#Region "Execute Items"

  Public Function ExecuteItem(lpId As String) As IProcessResults
    Try
      Try
        ' Return ExecuteItems(New String() {lpId})
        Dim lobjIdentifiers As New ObjectIdentifiers From {
          {lpId, ObjectIdentifier.ObjectTypeEnum.Document}
        }
        Return ExecuteItems(lobjIdentifiers)
      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ExecuteItem(lpId As String, ByRef lpTitle As String) As IProcessResults
    Try
      Dim lobjDictionary As New Dictionary(Of String, String) From {
        {lpId, lpTitle}
      }
      Return ExecuteItems(lobjDictionary)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>Executes the specified items.  If the items do not currently exist in the job, they will be added automatically.  Folder items will be expanded and
  ''' all contained items will be added recursively.</summary>
  ''' <param name="lpItems">
  '''  <para>The items to execute.</para>
  ''' </param>
  ''' <returns>An IProcessResults collection with the results of each individual item.</returns>
  Public Function ExecuteItems(lpItems As ObjectIdentifiers) As IProcessResults
    Try
      Return ExecuteItems(lpItems, True, RecursionLevel.ecmAllChildren, True)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>Executes the specified items. Depending on the value of lpAutoAddItems, non existing items will either be added automatically or will be automatically failed.
  ''' Folder items will be expanded and all contained items will be added recursively.</summary>
  ''' <param name="lpItems">
  '''  <para>The items to execute.</para>
  ''' </param>
  ''' <param name="lpAutoAddItems">
  '''  <para>Specifies whether or not to automatically add items that do not yet exist in the job.</para>
  ''' </param>
  Public Function ExecuteItems(lpItems As ObjectIdentifiers,
                               lpAutoAddItems As Boolean) As IProcessResults
    Try
      Return ExecuteItems(lpItems, lpAutoAddItems, RecursionLevel.ecmAllChildren, True)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ExecuteItems(lpItems As IDictionary(Of String, String)) As IProcessResults
    Try
      Return ExecuteItems(DictionaryToDataTable(lpItems), True, RecursionLevel.ecmAllChildren, True)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>Executes the specified items. Depending on the value of lpAutoAddItems, non existing items will either be added automatically or will be automatically failed.
  ''' Folder items will be expanded subject to the specified recursion level.</summary>
  ''' <param name="lpItems">
  '''  <para>The items to execute.</para>
  ''' </param>
  ''' <param name="lpAutoAddItems">
  '''  <para>Specifies whether or not to automatically add items that do not yet exist in the job.</para>
  ''' </param>
  ''' <param name="lpRecursionLevel">Specifies how deep, if at all to recurse any folder items.</param>
  ''' <remarks>
  ''' Will execute all items passed in.  If an item is a folder item it will also expand the list of items with the resolved containees.
  ''' </remarks>
  ''' <returns>An IProcessResults collection with the results of each individual item.</returns>
  ''' <example>
  '''  <code title="Usage Example" description="Shows how to call this method" lang="VB.NET">
  ''' Private Function ExecuteJobItemsTest(lpJob As Job) As IProcessResults
  '''  Try
  ''' 
  '''    Dim lobjObjectIdentifiers As New ObjectIdentifiers
  '''    lobjObjectIdentifiers.Add(New ObjectIdentifier("95", ObjectIdentifier.IdTypeEnum.ID, ObjectIdentifier.ObjectTypeEnum.Document))
  '''    lobjObjectIdentifiers.Add(New ObjectIdentifier("96", ObjectIdentifier.IdTypeEnum.ID, ObjectIdentifier.ObjectTypeEnum.Document))
  '''    lobjObjectIdentifiers.Add(New ObjectIdentifier("105", ObjectIdentifier.IdTypeEnum.ID, ObjectIdentifier.ObjectTypeEnum.Folder))
  ''' 
  '''    Return lpJob.ExecuteItems(lobjObjectIdentifiers, True, RecursionLevel.ecmAllChildren)
  ''' 
  '''  Catch Ex As Exception
  '''    ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
  '''    ' Re-throw the exception to the caller
  '''    Throw
  '''  End Try
  ''' End Function</code>
  ''' </example>
  Public Function ExecuteItems(lpItems As ObjectIdentifiers,
                               lpAutoAddItems As Boolean,
                               lpRecursionLevel As RecursionLevel,
                               lpWaitForResults As Boolean) As IProcessResults
    Try

      '	Throw New NotImplementedException
      mobjIProcessResults = New ProcessResults()

      Dim lobjCandidateItemTable As DataTable = ObjectIdentifiersToDataTable(lpItems, lpRecursionLevel)
      If lpAutoAddItems = True Then
        Dim lstrAddItemsResult As String = String.Empty
        AddItemsFromDataTable(lobjCandidateItemTable, lstrAddItemsResult)
      End If

      Dim lobjSelectedBatches As Batches = GetSelectedBatches(lpItems)

      If (lobjSelectedBatches.Count = 0) Then
        Return New ProcessResults
      End If

      Dim lobjBatchWorker As BaseBatchWorker = Nothing

      mintBatchWorkersRunning = lobjSelectedBatches.Count

      ' mblnIsRunning = True

      For Each lobjBatch As Batch In lobjSelectedBatches
        lobjBatchWorker = CreateBatchWorker(lobjBatch)
        lobjBatchWorker.RunWorkerAsync(lobjBatch.SelectedItemList)
      Next

      If (lpWaitForResults) Then
        'Wait for all threads to finish
        mobjResetEvent.WaitOne()
      End If

      Return mobjIProcessResults

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      ' mblnIsRunning = False
    End Try
  End Function

  Public Function GetSelectedBatches(lpItems As ObjectIdentifiers) As Batches

    Try

      Dim lobjSelectedBatches As New Batches(Me)
      Dim lobjSelectedBatch As Batch = Nothing
      Dim lobjSelectedBatchItem As BatchItem = Nothing

      'Before we set the selected items, make sure the selected items are cleared out
      ClearSelectedBatchItems()

      For Each item As ObjectIdentifier In lpItems

        lobjSelectedBatchItem = Me.GetItemByDocId(item.IdentifierValue, OperationScope.Source)

        If lobjSelectedBatches.Contains(lobjSelectedBatchItem.Batch) = False Then
          lobjSelectedBatches.Add(lobjSelectedBatchItem.Batch)
        End If

        lobjSelectedBatch = lobjSelectedBatches(lobjSelectedBatchItem.Batch.Id)

        If lobjSelectedBatch IsNot Nothing Then

          If (lobjSelectedBatchItem IsNot Nothing) AndAlso
            (lobjSelectedBatch.SelectedItems.Contains(lobjSelectedBatchItem) = False) Then

            lobjSelectedBatch.SelectedItems.Add(lobjSelectedBatchItem)

          End If

          'If lobjSelectedBatches.Contains(lobjSelectedBatch) = False Then
          '	lobjSelectedBatches.Add(lobjSelectedBatch)
          'End If

        End If

      Next

      Return lobjSelectedBatches

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function


#Region "Private Methods"

  ''' <summary>
  ''' Takes a collection of object identifiers and returns 
  ''' a candidate data table for inserting into a job.
  ''' </summary>
  ''' <param name="lpItems"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function ObjectIdentifiersToDataTable(lpItems As ObjectIdentifiers,
                                                lpRecursionLevel As RecursionLevel) As DataTable
    Try

      Dim lobjItemTable As DataTable = CreateNewSourceDataTable()
      Dim lobjNewRow As DataRow

      For Each lobjItem As ObjectIdentifier In lpItems
        Select Case lobjItem.ObjectType
          Case ObjectIdentifier.ObjectTypeEnum.Document
            lobjNewRow = lobjItemTable.NewRow()
            lobjNewRow(0) = lobjItem.IdentifierValue
            lobjNewRow(1) = lobjItem.IdentifierValue
            lobjItemTable.Rows.Add(lobjNewRow)

          Case ObjectIdentifier.ObjectTypeEnum.Folder
            '	Create some magic recursion code to add all the folder items.

        End Select
      Next

      Return lobjItemTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function DataTableToObjectIdentifiers(lpItemsTable As DataTable) As ObjectIdentifiers
    Try
      Dim lobjReturnIdentifiers As New ObjectIdentifiers

      For Each lobjRow As DataRow In lpItemsTable.Rows
        lobjReturnIdentifiers.Add(New ObjectIdentifier(lobjRow(0),
                                      ObjectIdentifier.IdTypeEnum.ID,
                                      ObjectIdentifier.ObjectTypeEnum.Document))
      Next

      Return lobjReturnIdentifiers

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function CreateBatchWorker(ByVal lpBatch As Batch) As BaseBatchWorker

    Try

      Dim lobjBatchWorker As New BaseBatchWorker(lpBatch)
      AddHandler lobjBatchWorker.DoWork, AddressOf Me.DoWork
      AddHandler lobjBatchWorker.RunWorkerCompleted, AddressOf Me.ExecuteItemsWorkerCompleted
      AddHandler lobjBatchWorker.ProgressChanged, AddressOf Me.ProgressChanged
      AddHandler lpBatch.ItemProcessed, AddressOf Me.Batch_ItemProcessed
      Return lobjBatchWorker

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

    Return Nothing

  End Function

  Private Function ExecuteItems(lpItemsTable As DataTable,
                             lpAutoAddItems As Boolean,
                             lpRecursionLevel As RecursionLevel,
                             lpWaitForResults As Boolean) As IProcessResults
    Try
      '	Throw New NotImplementedException
      mobjIProcessResults = New ProcessResults()
      If lpAutoAddItems = True Then
        Dim lstrAddItemsResult As String = String.Empty
        AddItemsFromDataTable(lpItemsTable, lstrAddItemsResult)
      End If

      Dim lobjSelectedBatches As Batches = GetSelectedBatches(DataTableToObjectIdentifiers(lpItemsTable))

      If (lobjSelectedBatches.Count = 0) Then
        Return New ProcessResults
      End If

      Dim lobjBatchWorker As BaseBatchWorker = Nothing

      mintBatchWorkersRunning = lobjSelectedBatches.Count

      ' mblnIsRunning = True

      For Each lobjBatch As Batch In lobjSelectedBatches
        lobjBatchWorker = CreateBatchWorker(lobjBatch)
        lobjBatchWorker.RunWorkerAsync(lobjBatch.SelectedItemList)
      Next

      If (lpWaitForResults) Then
        'Wait for all threads to finish
        mobjResetEvent.WaitOne()
      End If

      Return mobjIProcessResults

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      ' mblnIsRunning = False
    End Try
  End Function

  Private Sub ExecuteItemsWorkerCompleted(ByVal sender As Object,
                                 ByVal e As RunWorkerCompletedEventArgs)

    Try

      Dim worker As BaseBatchWorker = CType(sender, BaseBatchWorker)

      RaiseEvent BatchCompleted(sender, worker.Batch)

      'Does this in a thread safe manner - mintBatchWorkersRunning -= 1
      Interlocked.Decrement(mintBatchWorkersRunning)

      'If all threads are done running
      If mintBatchWorkersRunning = 0 Then
        mobjIProcessResults.Add(worker.ProcessResults)
        mobjResetEvent.Set()
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub

#End Region

#End Region

  Private Sub Job_AfterJobComplete(sender As Object, ByRef e As WorkEventArgs) Handles Me.AfterJobComplete
    Try
      'Dim lobjMessagingService As IMessagingService = Nothing
      'Dim lobjMessage As IMessage = Nothing

      Project.Container.SaveProcessResultSummary(Me)

      'Notifications.Notifier.SendNotification(e.Job, Notifications.EventBasis.JobCompleted)

      ApplicationLogging.WriteLogEntry(String.Format("Job '{0}' completed.", e.Job.Name))

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub
  Private Sub Job_BeforeJobBegin(sender As Object, ByRef e As WorkEventArgs) Handles Me.BeforeJobBegin
    Try
      'Notifications.Notifier.SendNotification(e.Job, Notifications.EventBasis.JobStarted)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub Job_JobCancelled(sender As Object, ByRef e As WorkCancelledEventArgs) Handles Me.JobCancelled
    Try
      Project.Container.SaveProcessResultSummary(Me)
      'Notifications.Notifier.SendNotification(e.Job, Notifications.EventBasis.JobCancelled)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

End Class
