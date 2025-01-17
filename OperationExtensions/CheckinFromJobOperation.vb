' ********************************************************************************
' '  Document    :  CheckinFromJobOperation.vb
' '  Description :  This operation is to be used to check a document back into a 
' '              :  repository that was checked out to an external repository.
' '              :  
' '              :  It assumes that the source doc id for this operation aligns 
' '              :  with an associated destination doc id in the referenced job.
' '              :  
' '  Created     :  10/10/2012-08:46:39
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

#Region "Imports"

Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Providers
Imports Documents.Utilities
Imports Operations

#End Region

Public Class CheckinFromJobOperation
  Inherits ProjectOperationExtension

#Region "Class Constants"

  Private Shadows OPERATION_NAME As String = "CheckinFromJob"
  Private Shadows OPERATION_DISPLAY_NAME As String = "Checkin From Job"
  Friend Const PARAM_JOB_NAME As String = "JobName"
  Private Const PARAM_CHECKIN_AS_MAJOR As String = "CheckinAsMajor"

#End Region

#Region "Public Properties"

  Public Property CheckinAsMajor As Boolean
    Get
      Try
        Return GetParameterValue(PARAM_CHECKIN_AS_MAJOR, False)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Boolean)
      Try
        SetParameterValue(PARAM_CHECKIN_AS_MAJOR, value)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#Region "Public Overriden Methods"

  Public Overrides ReadOnly Property DisplayName As String
    Get
      Try
        Return OPERATION_DISPLAY_NAME
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

  Public Overrides ReadOnly Property CanRollback As Boolean
    Get
      Return False
    End Get
  End Property

  Protected Overrides Function OnExecute() As OperationEnumerations.Result
    Try
      Return CheckinFromJob()
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
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
          PARAM_JOB_NAME, "Job Name", "The source job to check in from."))
      End If
      If lobjParameters.Contains(PARAM_CHECKIN_AS_MAJOR) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmBoolean, PARAM_CHECKIN_AS_MAJOR, False,
          "Specifies whether or not to check in the document as a major or minor version if applicable."))
      End If
      Return lobjParameters

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Methods"

  Private Function CheckinFromJob() As Result

    Dim lobjBCSProvider As IBasicContentServicesProvider = Nothing

    Try

      RunPreOperationChecks(True)

      Dim lstrCheckedOutDocId As String = GetCheckedOutDocId()
      ' Get the content container to check in
      Dim lobjLatestContent As Content = Me.WorkItem.Document.LatestVersion.PrimaryContent

      ' NOTE: Ernie Bahr at: 10/10/2012-10:21:02 on machine: ERNIEBAHR-THINK
      '       The code below is borrowed from the CheckinOperation class in the operations dll
      '       Although this is a copy, they should be maintained to be as identically implemented as possible.

      lobjBCSProvider = DestinationConnection.Provider.GetInterface(ProviderClass.BasicContentServices)

      If (lobjBCSProvider IsNot Nothing) Then

        Dim lobjContentContainer As IContentContainer = New ContentStreamContainer(lobjLatestContent)

        menuResult = ConvertResult(lobjBCSProvider.CheckinDocument(lstrCheckedOutDocId, lobjContentContainer, CheckinAsMajor))

        If menuResult = OperationEnumerations.Result.Success Then
          ' Add the source doc id to the process parameters collection
          If Me.WorkItem.Process.Parameters.Contains(Process.PARAM_SOURCE_DOC_ID) Then
            Me.WorkItem.Process.Parameters.Item(Process.PARAM_SOURCE_DOC_ID).Value = lstrCheckedOutDocId
          Else
            Me.WorkItem.Process.Parameters.Add(ParameterFactory.Create(PropertyType.ecmString,
                                                                  Process.PARAM_SOURCE_DOC_ID, lstrCheckedOutDocId, "The source document Id to check in."))
          End If
          Me.ProcessedMessage = String.Format("Successfully checked in document over '{0}'.", lstrCheckedOutDocId)
        End If

      Else
        menuResult = OperationEnumerations.Result.Failed
        OnError(New OperableErrorEventArgs(Me, WorkItem, "Unable to get basic content services interface"))
      End If

      Return menuResult

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      Me.ProcessedMessage = String.Format("CheckinFromJob Failed: {0}", Ex.Message)
      menuResult = OperationEnumerations.Result.Failed
      Return menuResult
    End Try
  End Function

  Private Function GetCheckedOutDocId() As String
    Try
      ' Try to get a reference to the source job
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

      ' Try to locate the originating batch item in the referenced job
      Dim lobjSourceBatchItem As IBatchItem = lobjJob.GetItemByDocId(Me.WorkItem.SourceDocId, ExportScope.Destination)
      Return lobjSourceBatchItem.SourceDocId

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
