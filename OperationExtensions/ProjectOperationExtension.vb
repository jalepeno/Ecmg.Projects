' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  ProjectOperationExtension.vb
'  Description :  [type_description_here]
'  Created     :  8/13/2012 4:20:00 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Utilities
Imports Operations
Imports Operations.Extensions

#End Region

Public MustInherit Class ProjectOperationExtension
  Inherits OperationExtension

#Region "Class Constants"

  Protected Const ASSEMBLY_JOB_MANAGER As String = "JobManager.exe"
  Protected Const JOB_MANAGER_WINDOW As String = "JobManagerWindow"

#End Region

#Region "Public Overriden Methods"

  Public Overrides ReadOnly Property CompanyName As String
    Get
      Try
        Return ConstantValues.CompanyName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overrides ReadOnly Property ProductName As String
    Get
      Try
        Return ConstantValues.ProductName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Other Public Methods"

  Public Shared Function GetCurrentJob(lpOperable As IOperable)
    Try
      If TypeOf lpOperable.Parent Is Batch Then
        Dim lobjParentBatch As Batch = lpOperable.Parent
        If lobjParentBatch.Job IsNot Nothing Then
          Return lobjParentBatch.Job
        Else
          Throw New InvalidOperationException("The current job could not be found, the job reference is not set for the current batch.")
        End If
      ElseIf lpOperable.WorkItem IsNot Nothing AndAlso TypeOf lpOperable.WorkItem Is WorkItemProxy Then
        If TypeOf lpOperable.WorkItem Is JobItemProxy Then
          Return CType(lpOperable.WorkItem, JobItemProxy).Job
        ElseIf TypeOf lpOperable.WorkItem Is BatchItemProxy Then
          Return CType(lpOperable.WorkItem, BatchItemProxy).Batch.Job
        Else
          Throw New NotRunningInProjectException(True)
        End If
      Else
        Throw New NotRunningInProjectException(True)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function GetJobByName(lpOperable As IOperable, lpJobName As String) As Job
    Try
      If TypeOf lpOperable.Parent Is Batch Then
        Dim lobjJobs As Jobs = Nothing
        Dim lobjParentBatch As Batch = lpOperable.Parent
        If lobjParentBatch.Job IsNot Nothing Then
          If lobjParentBatch.Job.Project IsNot Nothing Then
            lobjJobs = lobjParentBatch.Job.Project.Jobs
            Return lobjJobs(lpJobName)
          Else
            Throw New ItemDoesNotExistException(lpJobName, String.Format("Unable to get Project reference from job '{0}'.", lobjParentBatch.Job.Name))
          End If
        Else
          Throw New ItemDoesNotExistException(lpJobName, String.Format("Unable to get Job reference from batch '{0}'.", lobjParentBatch.Id))
        End If
      ElseIf lpOperable.WorkItem IsNot Nothing AndAlso TypeOf lpOperable.WorkItem Is WorkItemProxy Then
        If TypeOf lpOperable.WorkItem Is JobItemProxy Then
          Return CType(lpOperable.WorkItem, JobItemProxy).Job.Project.Jobs(lpJobName)
        ElseIf TypeOf lpOperable.WorkItem Is BatchItemProxy Then
          Return CType(lpOperable.WorkItem, BatchItemProxy).Batch.Job.Project.Jobs(lpJobName)
        Else
          Throw New NotRunningInProjectException(True)
        End If
      Else
        Throw New NotRunningInProjectException(True)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Function GetCurrentJob() As Job
    Try
      Return GetCurrentJob(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Function GetJobByName(lpJobName As String) As Job
    Try
      Return GetJobByName(Me, lpJobName)
      'If TypeOf Me.Parent Is Batch Then
      '  Dim lobjJobs As Jobs = Nothing
      '  Dim lobjParentBatch As Batch = Me.Parent
      '  If lobjParentBatch.Job IsNot Nothing Then
      '    If lobjParentBatch.Job.Project IsNot Nothing Then
      '      lobjJobs = lobjParentBatch.Job.Project.Jobs
      '      Return lobjJobs(lpJobName)
      '    Else
      '      Throw New Exceptions.ItemDoesNotExistException(lpJobName, String.Format("Unable to get Project reference from job '{0}'.", lobjParentBatch.Job.Name))
      '    End If
      '  Else
      '    Throw New Exceptions.ItemDoesNotExistException(lpJobName, String.Format("Unable to get Job reference from batch '{0}'.", lobjParentBatch.Id))
      '  End If
      'ElseIf Me.WorkItem IsNot Nothing AndAlso TypeOf Me.WorkItem Is WorkItemProxy Then
      '  If TypeOf Me.WorkItem Is JobItemProxy Then
      '    Return CType(Me.WorkItem, JobItemProxy).Job.Project.Jobs(lpJobName)
      '  ElseIf TypeOf Me.WorkItem Is BatchItemProxy Then
      '    Return CType(Me.WorkItem, BatchItemProxy).Batch.Job.Project.Jobs(lpJobName)
      '  Else
      '    Throw New NotRunningInProjectException(True)
      '  End If
      'Else
      '  Throw New NotRunningInProjectException(True)
      'End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
