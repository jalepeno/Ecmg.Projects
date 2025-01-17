' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  RunJobOperation.vb
'  Description :  [type_description_here]
'  Created     :  8/8/2012 3:15:13 PM
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

#End Region

Public Class RunJobOperation
  Inherits ProjectOperationExtension

#Region "Class Constants"

  Private Shadows ReadOnly OPERATION_NAME As String = "RunJob"
  Private Shadows ReadOnly OPERATION_DESCRIPTION As String = "Runs the job specified in the 'JobName' parameter."
  Friend Const PARAM_JOB_NAME As String = "JobName"

#End Region

#Region "Constructors"

  Public Sub New()
    MyBase.New()
  End Sub

#End Region

#Region "Public Overriden Methods"

  Public Overrides ReadOnly Property Name As String
    Get
      Try
        Return OPERATION_NAME
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overrides ReadOnly Property DisplayName As String
    Get
      Try
        Return OPERATION_NAME
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overrides ReadOnly Property Description As String
    Get
      Try
        Return OPERATION_DESCRIPTION
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overrides ReadOnly Property CanRollback As Boolean
    Get
      Return False
    End Get
  End Property

  Protected Overrides Function OnExecute() As OperationEnumerations.Result
    Try
      Dim lstrJobName As String = MyBase.GetParameterValue(PARAM_JOB_NAME, String.Empty)
      If String.IsNullOrEmpty(lstrJobName) Then
        Throw New ParameterValueNotSetException(PARAM_JOB_NAME)
      End If
      Dim lobjJob As Job = GetJobByName(lstrJobName)
      If lobjJob Is Nothing Then
        menuResult = OperationEnumerations.Result.Failed
        Me.ProcessedMessage = String.Format("Could not find job named '{0}' in the current project.", lstrJobName)
        Return menuResult
      End If
      If ((Me.Host IsNot Nothing) AndAlso (String.Compare(Me.Host.GetType.Name, JOB_MANAGER_WINDOW) = 0)) Then
        ' The process is being hosted in a job manager window, 
        ' let it process the job so that the UI can show the progress.
        Me.Host.StartAllBatches(lobjJob)
      Else
        ' Define and invoke Job.Execute
        lobjJob.Execute()
        'lobjJob.Project.RunJob(lobjJob.Name, 2)
      End If

      menuResult = OperationEnumerations.Result.Success

      Return menuResult

      'If Assembly.GetEntryAssembly.ManifestModule.Name = ASSEMBLY_JOB_MANAGER Then
      '  ' Invoke RunAllBatches in JobManager
      '  'lobjj
      '  'Dim lobjWindowModule As Reflection.Module = Assembly.GetEntryAssembly.GetModule("Ecmg.Cts.UI.Wpf.JobManager.JobManagerWindow")
      '  Dim lobjAppModule As Reflection.Module = Assembly.GetEntryAssembly.GetModule(ASSEMBLY_JOB_MANAGER)
      '  'Dim lobjWindowType As Type = Assembly.GetEntryAssembly.ManifestModule.GetType("Ecmg.Cts.UI.Wpf.JobManager.JobManagerWindow")
      '  Dim lobjWindowType As Type = Assembly.GetEntryAssembly.GetType("Ecmg.Cts.UI.Wpf.JobManager.JobManagerWindow")

      '  ' This is not yet working.  We need to find a way to get a handle to the current JobManagerWindow instance and call StartAllBatches.
      '  Dim lobjWindow As Object = lobjWindowType.InvokeMember(Nothing, BindingFlags.Instance, Nothing, Nothing, Nothing)

      '  lobjWindow.InvokeMember("StartAllBatches", BindingFlags.DeclaredOnly + BindingFlags.Public + BindingFlags.Instance + BindingFlags.InvokeMethod, Nothing, lobjWindow, New Object() {lobjJob})
      '  'Dim lobjStartAllBatchesMethod As MethodInfo = lobjAppModule.GetMethod("StartAllBatches")
      '  'Dim lobjParameters As New List(Of Object)
      '  'lobjParameters.Add(lobjJob)
      '  'lobjStartAllBatchesMethod.Invoke(lobjAppModule, lobjParameters.ToArray)
      'Else
      '  ' Define and invoke Job.Execute
      '  lobjJob.Execute()
      'End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function Rollback() As OperationEnumerations.Result
    Try
      Return OnRollback()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Overrides Function GetDefaultParameters() As IParameters
    Try
      Dim lobjParameters As IParameters = New Parameters

      If lobjParameters.Contains(PARAM_JOB_NAME) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString,
          PARAM_JOB_NAME, String.Empty, "The job to run."))
      End If

      Return lobjParameters

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
