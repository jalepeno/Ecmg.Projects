'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  CreateJobOperation.vb
'   Description :  [type_description_here]
'   Created     :  2/19/2013 4:28:47 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Utilities
Imports Operations

#End Region

Public Class CreateJobOperation
  Inherits ProjectOperationExtension

#Region "Class Constants"

  Private Shadows ReadOnly OPERATION_NAME As String = "CreateJob"
  Private Shadows ReadOnly OPERATION_DESCRIPTION As String = "Creates a job in the current project using the specified parameters."

  Friend Const PARAM_JOB_NAME As String = "JobName"
  Friend Const PARAM_APPEND_DATE_TIME_TO_BASE_JOB_NAME As String = "AppendDateTimeToBaseJobName"
  ' Friend Const PARAM_APPEND_PROCESS_PARAM_TO_BASE_JOB_NAME As String = "AppendProcessParameterToBaseJobName"
  ' Friend Const PARAM_DISPLAY_NAME As String = "JobDisplayName"
  Friend Const PARAM_DESCRIPTION As String = "JobDescription"
  Friend Const PARAM_SOURCE_CONTENT_SOURCE_NAME As String = "SourceContentSourceName"
  Friend Const PARAM_SOURCE_LIST_PATH As String = "SourceListPath"
  Friend Const PARAM_DESTINATION_CONTENT_SOURCE_NAME As String = "DestinationContentSourceName"
  Friend Const PARAM_PROCESS As String = "Process"
  ' Friend Const PARAM_CONFIGURATION As String = "JobConfiguration"

#End Region

#Region "Class Variables"

  Private ReadOnly mstrBaseJobName As String = String.Empty
  Private mstrJobName As String = String.Empty
  Private mblnAppendDateTimeToBase As Boolean = True
  'Private mstrAppendProcessParameterToBaseJobName As String = String.Empty
  'Private mstrDisplayName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mstrSourceContentSourceName As String = String.Empty
  Private mstrSourceListPath As String = String.Empty
  Private mstrDestinationContentSourceName As String = String.Empty
  Private mobjProcess As New Process

#End Region

#Region "Public Overriden Methods"

  Public Overrides ReadOnly Property CanRollback As Boolean
    Get
      Try
        Return True
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

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

  Protected Overrides Function OnExecute() As OperationEnumerations.Result
    Try
      Return CreateJob()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Overrides Function Rollback() As OperationEnumerations.Result
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
          PARAM_JOB_NAME, String.Empty, "The name for the new job."))
      End If

      'If lobjParameters.Contains(PARAM_APPEND_PROCESS_PARAM_TO_BASE_JOB_NAME) = False Then
      '  lobjParameters.Add(New Core.Parameter(Core.PropertyType.ecmString, PARAM_APPEND_PROCESS_PARAM_TO_BASE_JOB_NAME, String.Empty))
      'End If

      If lobjParameters.Contains(PARAM_APPEND_DATE_TIME_TO_BASE_JOB_NAME) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmBoolean, PARAM_APPEND_DATE_TIME_TO_BASE_JOB_NAME, True,
          "Specifies whether or not to append the date and time to the base job name.  This is useful when creating the job multiple times."))
      End If

      'If lobjParameters.Contains(PARAM_DISPLAY_NAME) = False Then
      '  lobjParameters.Add(New Core.Parameter(Core.PropertyType.ecmString, PARAM_DISPLAY_NAME, String.Empty))
      'End If

      If lobjParameters.Contains(PARAM_DESCRIPTION) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_DESCRIPTION,
          String.Empty, "The description for the new job"))
      End If

      If lobjParameters.Contains(PARAM_SOURCE_CONTENT_SOURCE_NAME) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_SOURCE_CONTENT_SOURCE_NAME,
          String.Empty, "The name of the source ContentSource."))
      End If

      If lobjParameters.Contains(PARAM_SOURCE_LIST_PATH) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_SOURCE_LIST_PATH, String.Empty,
          "If using a list file, this parameter specifies the path of the list file."))
      End If

      If lobjParameters.Contains(PARAM_DESTINATION_CONTENT_SOURCE_NAME) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_DESTINATION_CONTENT_SOURCE_NAME,
          String.Empty, "The name of the destination ContentSource."))
      End If

      If lobjParameters.Contains(PARAM_PROCESS) = False Then
        lobjParameters.Add(New ObjectParameter(PropertyType.ecmObject, PARAM_PROCESS,
          New Process(), "The process to run for this job."))
      End If

      'If lobjParameters.Contains(PARAM_PROCESS) = False Then
      '  lobjParameters.Add(ParameterFactory.Create(Core.PropertyType.ecmObject, PARAM_PROCESS, _
      '   New Process(), "The process to run for this job."))
      'End If

      Return lobjParameters

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Private Function CreateSampleConfiguration() As JobConfiguration
  '  Try
  '    Dim lobjConfiguration As New JobConfiguration
  '    With lobjConfiguration
  '      .Source.SourceConnectionString = String.Empty
  '      .Source.Type = enumSourceType.Empty
  '      .DestinationConnectionString = String.Empty
  '    End With

  '    Return lobjConfiguration

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

#End Region

#Region "Private Methods"

  Private Function CreateJob() As OperationEnumerations.Result
    Try

      If GetParameters() <> OperationEnumerations.Result.Success Then
        menuResult = OperationEnumerations.Result.Failed
        Return menuResult
      End If

      ' Initialize the job name
      'mstrJobName = ResolveInlineParameter(mstrBaseJobName)

      'mstrDestinationContentSourceName = ResolveInlineParameter(mstrDestinationContentSourceName)

      ' If we were asked to append the date/time we will do it now.
      If mblnAppendDateTimeToBase = True Then
        mstrJobName = String.Format("{0}{1}", mstrJobName, Now.ToUniversalTime)
      End If

      Dim lobjProject As Project = Me.GetCurrentJob.Project

      ' TODO: Work on adding the Ids to the target job.
      Dim lobjListSource As JobSource = Nothing
      Dim lobjIdList As List(Of String) = Nothing

      ' mstrSourceListPath = ResolveInlineParameter(mstrSourceListPath)

      If Not String.IsNullOrEmpty(mstrSourceListPath) Then
        If IO.File.Exists(mstrSourceListPath) = False Then
          Throw New InvalidPathException(String.Format("Unable to resolve source list path '{0}'.",
                                                                  mstrSourceListPath), mstrSourceListPath)
        End If
        lobjListSource = New JobSource
        With lobjListSource
          .Type = enumSourceType.List
          ' .ListFilePath = mstrSourceListPath
          .SourceIdFileNames.Add(mstrSourceListPath)
          .ListType = enumListType.TextFile
        End With
        ' TODO: Add the items to the job.
      End If

      Dim lobjJob As New Job(lobjProject, mstrJobName, mstrJobName, _
                       mstrDescription, mstrSourceContentSourceName, _
                       mstrDestinationContentSourceName, mobjProcess)

      If lobjListSource IsNot Nothing Then
        lobjListSource.SourceConnectionString = lobjJob.Source.SourceConnectionString
        lobjJob.Source = lobjListSource
        lobjIdList = lobjListSource.ReadList
      End If

      If lobjIdList IsNot Nothing AndAlso lobjIdList.Count > 0 Then
        lobjJob.AddItems(lobjIdList)
      End If

      menuResult = OperationEnumerations.Result.Success

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      menuResult = OperationEnumerations.Result.Failed
      OnError(New OperableErrorEventArgs(Me, WorkItem, ex))
    End Try

    Return menuResult

  End Function

  Private Function GetParameters() As OperationEnumerations.Result
    Try
      ' mstrBaseJobName = GetParameterValue(PARAM_JOB_NAME, String.Empty)
      mstrJobName = GetParameterValue(PARAM_JOB_NAME, String.Empty)
      ' mstrAppendProcessParameterToBaseJobName = GetParameterValue(PARAM_APPEND_PROCESS_PARAM_TO_BASE_JOB_NAME, String.Empty)
      mblnAppendDateTimeToBase = GetParameterValue(PARAM_APPEND_DATE_TIME_TO_BASE_JOB_NAME, True)
      ' mstrDisplayName = GetParameterValue(PARAM_DISPLAY_NAME, String.Empty)
      mstrDescription = GetParameterValue(PARAM_DESCRIPTION, String.Empty)
      mstrSourceContentSourceName = GetParameterValue(PARAM_SOURCE_CONTENT_SOURCE_NAME, String.Empty)
      mstrSourceListPath = GetParameterValue(PARAM_SOURCE_LIST_PATH, String.Empty)
      mstrDestinationContentSourceName = GetParameterValue(PARAM_DESTINATION_CONTENT_SOURCE_NAME, String.Empty)
      mobjProcess = GetParameterValue(PARAM_PROCESS, New Process)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      menuResult = OperationEnumerations.Result.Failed
      OnError(New OperableErrorEventArgs(Me, WorkItem, ex))
    End Try

    Return OperationEnumerations.Result.Success

  End Function

#End Region

End Class
