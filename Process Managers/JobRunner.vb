'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobRunner.vb
'   Description :  [type_description_here]
'   Created     :  9/29/2016 9:38:52 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"


Imports System.IO
Imports System.Threading
Imports Documents.Configuration
Imports Documents.Utilities
Imports Projects.Configuration

#End Region

Public Class JobRunner

#Region "Class Variables"

  Private Shared mobjInstance As JobRunner
  Private Shared mintReferenceCount As Integer
  'Private mobjLogSession As Session
  Private mobjJobRunnerProcesses As New JobRunnerProcesses

#End Region

#Region "Class Properties"

  'Friend ReadOnly Property LogSession As Gurock.SmartInspect.Session
  '  Get
  '    Try
  '      Return mobjLogSession
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  Public ReadOnly Property Processes As JobRunnerProcesses
    Get
      Try
        Return mobjJobRunnerProcesses
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property


#End Region

#Region "Constructors"

  Private Sub New()
    Try
      'InitializeLogSession()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Singleton Support"

  Public Shared ReadOnly Property Instance As JobRunner
    Get
      Try
        If Not Helper.CallStackContainsMethodName("AssignObjectProperties") Then
          Return GetInstance(False)
        Else
          Return Nothing
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Private Shared Function GetInstance(ByVal lpForceRefresh As Boolean) As JobRunner
    Try
      If lpForceRefresh = True Then
        mobjInstance = New JobRunner()
        'ElseIf mobjInstance Is Nothing OrElse mobjInstance.IsDisposed = True Then
      ElseIf mobjInstance Is Nothing Then
        mobjInstance = New JobRunner()
      End If
      mintReferenceCount += 1
      Return mobjInstance
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Public Methods"

  Public Sub ExecuteJob(lpJob As Job)

    Dim lobjProcessStartInfo As New ProcessStartInfo

    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
      Dim lobjJobSet As JobSet = CreateJobSet(lpJob)
      Dim lstrEcfPath As String = CreateJobEcf(lobjJobSet)
      Dim lintMaxJobRunnerInstancesPerJob As Integer = ConnectionSettings.Instance.MaxJobRunnerInstancesPerJob

      lobjProcessStartInfo = New ProcessStartInfo("JobRunner.exe", lstrEcfPath)

      lobjProcessStartInfo.WorkingDirectory = Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)
      Dim lobjStartJobRunnerArgs As New StartJobRunnerEventArgs(lobjProcessStartInfo, lpJob.BatchCount, lobjJobSet)

      'LogSession.LogMessage(
      '"Starting {0} instances of JobRunner from '{1}' to run job '{2}' using configuration file '{3}'.",
      'lintMaxJobRunnerInstancesPerJob, lobjProcessStartInfo.WorkingDirectory, lpJob.Name, lstrEcfPath)

      If ((lpJob.BatchCount > 1) AndAlso (lintMaxJobRunnerInstancesPerJob > 1)) Then

        Dim lobjStartJobRunnerThread As New Thread(AddressOf StartJobRunnerInstances)
        lobjStartJobRunnerThread.Start(lobjStartJobRunnerArgs)
        Thread.Sleep(10000)
      Else
        'LogSession.LogMessage(
        '"Starting JobRunner from '{0}' to run job '{1}' using configuration file '{2}'.",
        'lobjProcessStartInfo.WorkingDirectory, lpJob.Name, lstrEcfPath)
        'Diagnostics.Process.Start(lobjProcessStartInfo)
        StartJobRunnerProcess(lobjStartJobRunnerArgs)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      'LogSession.LogMessage(String.Format("FileName = '{0}'", lobjProcessStartInfo.FileName))
      'LogSession.LogMessage(String.Format("WorkingDirectory = '{0}'",
      'lobjProcessStartInfo.WorkingDirectory))
      'LogSession.LogMessage(String.Format("Arguments = '{0}'", lobjProcessStartInfo.Arguments))

      If ex.Message.ToLower.Contains("the system cannot find the file specified") Then
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Error, 62393)
        MsgBox("Job Runner is not available", MsgBoxStyle.Exclamation, "Job Runner is not available")
      Else
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Error, 62394)
        ' Re-throw the exception to the caller
        Throw
      End If
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try
  End Sub

#End Region

#Region "Friend Methods"

  Private Function CreateJobSet(lpJob As Job) As JobSet
    Try
      Dim lobjJobSet As New JobSet

      lobjJobSet.ProjectLocation = lpJob.Project.ItemsLocation

      lobjJobSet.Jobs.Add(lpJob.Name)

      Return lobjJobSet

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Function CreateJobEcf(lpJobSet As JobSet) As String
    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
      Dim lobjExecutionConfiguration As New ExecutionConfiguration

      lobjExecutionConfiguration.JobSets.Add(lpJobSet)

      Dim lstrFileName As String = String.Format("{0}.ecf", Helper.CleanFile(lpJobSet.Jobs.First, "_")).Replace(" ", String.Empty)
      Dim lstrFilePath As String = String.Format("{0}\{1}", ConnectionSettings.Instance.JobRunnerExecutionFilePath,
                                                 lstrFileName)

      If File.Exists(lstrFilePath) Then
        File.Delete(lstrFilePath)
        'LogSession.LogMessage("Deleted previous version of file '{0}'.", lstrFilePath)
      End If

      lobjExecutionConfiguration.Save(lstrFilePath)

      'LogSession.LogMessage("Successfully created file '{0}'.", lstrFilePath)

      Return lstrFileName

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try
  End Function

#End Region

#Region "Private Methods"

  Private Sub StartJobRunnerInstances(lpEventArgs As StartJobRunnerEventArgs)
    Try
      Dim lintMaxJobRunnerInstancesPerJob As Integer = ConnectionSettings.Instance.MaxJobRunnerInstancesPerJob

      ' Make sure we have enough batches to run for each JobRunner instance.
      If lintMaxJobRunnerInstancesPerJob > lpEventArgs.JobBatchCount Then
        lintMaxJobRunnerInstancesPerJob = lpEventArgs.JobBatchCount
      End If
      For lintJobRunnerInstanceCount As Integer = 0 To lintMaxJobRunnerInstancesPerJob - 1
        StartJobRunner(lpEventArgs)
      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub StartJobRunner(lpEventArgs As StartJobRunnerEventArgs)
    Try

      Dim lintMultipleJobRunnerInvocationDelaySeconds As Integer =
            ConnectionSettings.Instance.MultipleJobRunnerInvocationDelaySeconds
      StartJobRunnerProcess(lpEventArgs)

      Thread.Sleep(lintMultipleJobRunnerInvocationDelaySeconds * 1000)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub StartJobRunnerProcess(lpEventArgs As StartJobRunnerEventArgs)
    Try
      Dim lstrJobRunnerPath As String = Path.Combine(lpEventArgs.ProcessInfo.WorkingDirectory, lpEventArgs.ProcessInfo.FileName)
      'LogSession.LogMessage("Starting JobRunner from '{0}' using configuration file '{1}'.",
      'lstrJobRunnerPath, lpEventArgs.ProcessInfo.Arguments)
      Dim lobjProcess As Diagnostics.Process = Diagnostics.Process.Start(lpEventArgs.ProcessInfo)
      lobjProcess.EnableRaisingEvents = True
      AddProcessHandlers(lobjProcess)
      Processes.Add(New JobRunnerProcessInfo(lobjProcess, lpEventArgs.JobSet))
      'LogSession.LogMessage("JobRunner process ({0}) started '{1}' using configuration file '{2}'.",
      'lobjProcess.Id, lstrJobRunnerPath, lpEventArgs.ProcessInfo.Arguments)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub AddProcessHandlers(lpProcess As Process)
    Try
      AddHandler lpProcess.Disposed, AddressOf Process_Disposed
      AddHandler lpProcess.Exited, AddressOf Process_Exited
      AddHandler lpProcess.ErrorDataReceived, AddressOf Process_ErrorDataReceived
      AddHandler lpProcess.OutputDataReceived, AddressOf Process_OutputDataReceived

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Private Sub InitializeLogSession()
  '  Try
  '    mobjLogSession = SiAuto.Si.AddSession("JobRunner")
  '    mobjLogSession.Color = System.Drawing.Color.LawnGreen
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Private Sub Process_Disposed(sender As Object, e As EventArgs)
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub Process_ErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub Process_Exited(sender As Object, e As EventArgs)
    Try
      Dim lobjProcess As Process = sender
      'LogSession.LogMessage("JobRunner process ({0}) exited.", lobjProcess.Id)
      Processes.RemoveItem(lobjProcess.Id)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub Process_OutputDataReceived(sender As Object, e As DataReceivedEventArgs)
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class