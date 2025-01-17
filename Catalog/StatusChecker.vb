'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  StatusChecker.vb
'   Description :  [type_description_here]
'   Created     :  1/10/2014 2:13:45 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Threading
Imports Documents.Utilities

#End Region

Public Class NodeStatusChecker

#Region "Class Constants"

  'Private Const JOB_MANAGER_SERVICE_NAME As String = "Ecmg.Cts.JobManagerService"

#End Region

#Region "Class Variables"

  Private ReadOnly mobjNode As NodeInfo = Nothing
  Private mintInvokeCount As Integer
  Private ReadOnly mintMaxCount As Integer = 1

#End Region

#Region "Events"

  Public Event NodeStatusRefreshed As NodeEventHandler

#End Region

#Region "Public Properties"

  Public ReadOnly Property MaxCount As Integer
    Get
      Try
        Return mintMaxCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpNode As NodeInfo, lpMaxCount As Integer)
    Try
      mintMaxCount = lpMaxCount
      mobjNode = lpNode
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Sub UpdateStatus(lpStateInfo As Object)
    Try
      If mobjNode Is Nothing Then
        Exit Sub
      End If
      Dim autoEvent As AutoResetEvent = _
    DirectCast(lpStateInfo, AutoResetEvent)
      mintInvokeCount += 1
      'Console.WriteLine("{0} Checking status {1,2}.", _
      '    DateTime.Now.ToString("h:mm:ss.fff"), _
      '    mintInvokeCount.ToString())

      'UpdateStatusAsync(mobjNode)
      RaiseEvent NodeStatusRefreshed(Me, New NodeEventArgs(mobjNode))
      If mintInvokeCount = MaxCount Then
        ' Reset the counter and signal to stop the timer.
        mintInvokeCount = 0
        autoEvent.Set()
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Methods"

  'Private Async Sub UpdateStatusAsync(lpNode As NodeInfo)
  '  Try
  '    Dim lobjTask As Task = _
  '    Task.Factory.StartNew(Sub()
  '                            lpNode.Update()
  '                            'RaiseEvent NodeStatusRefreshed(Me, New NodeEventArgs(lpNode))
  '                          End Sub)
  '    Await lobjTask
  '    'RaiseEvent NodeStatusRefreshed(Me, New NodeEventArgs(lpNode))

  '  Catch SqlEx As SqlException
  '    ' Sometimes this happens but not usually, it seems to happen primarily when debugging
  '    ' Just log it and move on.
  '    ApplicationLogging.LogException(SqlEx, Reflection.MethodBase.GetCurrentMethod)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

#End Region

End Class
