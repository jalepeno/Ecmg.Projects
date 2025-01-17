' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  WorkItemProxy.vb
'  Description :  [type_description_here]
'  Created     :  8/1/2012 4:48:02 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities
Imports Operations

#End Region

Public Class WorkItemProxy
  Implements IWorkItem

#Region "Class Variables"

  Private mdatCreateDate As Date = Now
  Private mdateStartTime As Date = DateTime.MinValue
  Private mdateFinishTime As Date = DateTime.MinValue
  Private mstrId As String = String.Empty
  Private mobjTag As Object = Nothing

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpId As String)
    Try
      mstrId = lpId
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IWorkItem Implementation"

  Public ReadOnly Property CreateDate As Date Implements IWorkItem.CreateDate
    Get
      Try
        Return mdatCreateDate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property DestinationDocId As String Implements IWorkItem.DestinationDocId
    Get
      Try
        Return String.Empty
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("DestinationDocId set operation '{0}' not honored for WorkItemProxy.", value))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Document As Document Implements IWorkItem.Document
    Get
      Return Nothing
    End Get
    Set(value As Document)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("Document set operation '{0}' not honored for WorkItemProxy.", value.ID))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property [Object] As CustomObject Implements IWorkItem.Object
    Get
      Return Nothing
    End Get
    Set(value As CustomObject)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("Object set operation '{0}' not honored for WorkItemProxy.", value.Id))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Folder As Folder Implements IWorkItem.Folder
    Get
      Return Nothing
    End Get
    Set(value As Folder)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("Folder set operation '{0}' not honored for WorkItemProxy.", value.Id))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Function Execute(lpProcess As IOperable) As Boolean Implements IWorkItem.Execute
    Try
      ApplicationLogging.WriteLogEntry(String.Format("Execute operation '{0}' not honored for WorkItemProxy.", lpProcess.Name))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Property FinishTime As Date Implements IWorkItem.FinishTime
    Get
      Try
        Return mdateFinishTime
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Date)
      Try
        mdateFinishTime = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Id As String Implements IWorkItem.Id
    Get
      Return mstrId
    End Get
    Set(value As String)
      Try
        mstrId = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Parent As IItemParent Implements IWorkItem.Parent
    Get
      Return Nothing
    End Get
    Set(value As IItemParent)
      Try
        ApplicationLogging.WriteLogEntry("Parent set operation not honored for WorkItemProxy.")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Process As IOperable Implements IWorkItem.Process
    Get
      Try
        Return Nothing
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As IOperable)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("Process set operation '{0}' not honored for WorkItemProxy.", value.Name))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ProcessedBy As String Implements IWorkItem.ProcessedBy
    Get
      Return String.Empty
    End Get
    Set(value As String)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("ProcessedBy set operation '{0}' not honored for WorkItemProxy.", value))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ProcessedMessage As String Implements IWorkItem.ProcessedMessage
    Get
      Return String.Empty
    End Get
    Set(value As String)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("ProcessedMessage set operation '{0}' not honored for WorkItemProxy.", value))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ProcessedStatus As OperationEnumerations.ProcessedStatus Implements IWorkItem.ProcessedStatus
    Get
      Return ProcessedStatus.NotProcessed
    End Get
    Set(value As OperationEnumerations.ProcessedStatus)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("ProcessedStatus set operation '{0}' not honored for WorkItemProxy.", value.ToString))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ProcessResults As IProcessResult Implements IWorkItem.ProcessResult
    Get
      Return Nothing
    End Get
    Set(value As IProcessResult)
      Try
        ApplicationLogging.WriteLogEntry("ProcessResults set operation not honored for WorkItemProxy.")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property SourceDocId As String Implements IWorkItem.SourceDocId
    Get
      Return String.Empty
    End Get
    Set(value As String)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("SourceDocId set operation '{0}' not honored for WorkItemProxy.", value))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property StartTime As Date Implements IWorkItem.StartTime
    Get
      Return mdateStartTime
    End Get
    Set(value As Date)
      mdateStartTime = value
    End Set
  End Property

  Public Property Tag As Object Implements IWorkItem.Tag
    Get
      Return mobjTag
    End Get
    Set(value As Object)
      mobjTag = value
    End Set
  End Property

  Public Property Title As String Implements IWorkItem.Title
    Get
      Return String.Empty
    End Get
    Set(value As String)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("Title set operation '{0}' not honored for WorkItemProxy.", value))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property TotalProcessingTime As System.TimeSpan Implements IWorkItem.TotalProcessingTime
    Get
      Return mdateStartTime - mdateFinishTime
    End Get
    Set(value As System.TimeSpan)
      Try
        ApplicationLogging.WriteLogEntry(String.Format("TotalProcessingTime set operation '{0}' not honored for WorkItemProxy.", value.ToString))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Overridable Function ToJsonString(ByVal lpIncludeProcessResult As Boolean) As String Implements IWorkItem.ToJsonString
    Try
      Return WorkItem.ToJsonString(Me, lpIncludeProcessResult)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
