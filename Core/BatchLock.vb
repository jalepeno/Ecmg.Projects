'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  BatchLock.vb
'   Description :  [type_description_here]
'   Created     :  5/16/2013 4:18:42 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class BatchLock
  Implements IBatchLock

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrBatchId As String = String.Empty
  Private mstrJobId As String = String.Empty
  Private mstrJobName As String = String.Empty
  Private mstrLockedBy As String = String.Empty
  Private mblnIsLocked As Boolean = True
  Private mdatLockDate As DateTime = DateTime.MinValue
  Private mdatUnlockDate As DateTime = DateTime.MinValue

#End Region

#Region "Public Properties"

  Public ReadOnly Property Id As String Implements IBatchLock.Id
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

  Public ReadOnly Property BatchId As String Implements IBatchLock.BatchId
    Get
      Try
        Return mstrBatchId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsLocked As Boolean Implements IBatchLock.IsLocked
    Get
      Try
        Return mblnIsLocked
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property JobId As String Implements IBatchLock.JobId
    Get
      Try
        Return mstrJobId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property JobName As String Implements IBatchLock.JobName
    Get
      Try
        Return mstrJobName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property LockDate As Date Implements IBatchLock.LockDate
    Get
      Try
        Return mdatLockDate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property LockedBy As String Implements IBatchLock.LockedBy
    Get
      Try
        Return mstrLockedBy
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property UnlockDate As Date Implements IBatchLock.UnlockDate
    Get
      Try
        Return mdatUnlockDate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Friend Sub New(lpId As String, lpBatchId As String, lpJobId As String,
                 lpJobName As String, lpLockedBy As String, lpIsLocked As Boolean,
                 lpLockDate As DateTime, lpUnlockDate As DateTime)
    Try
      mstrId = lpId
      mstrBatchId = lpBatchId
      mstrJobId = lpJobId
      mstrJobName = lpJobName
      mstrLockedBy = lpLockedBy
      mblnIsLocked = lpIsLocked
      mdatLockDate = lpLockDate
      mdatUnlockDate = lpUnlockDate
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub New(lpId As String, lpBatchId As String, lpJob As Job,
               lpLockedBy As String, lpIsLocked As Boolean,
               lpLockDate As DateTime, lpUnlockDate As DateTime)
    Try
      mstrId = lpId
      mstrBatchId = lpBatchId
      mstrJobId = lpJob.Id
      mstrJobName = lpJob.Name
      mstrLockedBy = lpLockedBy
      mblnIsLocked = lpIsLocked
      mdatLockDate = lpLockDate
      mdatUnlockDate = lpUnlockDate
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region


End Class
