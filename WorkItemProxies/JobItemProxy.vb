' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobItemProxy.vb
'  Description :  [type_description_here]
'  Created     :  8/14/2012 8:07:20 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports Operations

#End Region

Public Class JobItemProxy
  Inherits WorkItemProxy
  Implements IJobItemProxy

#Region "Class Variables"

  Private mobjJob As Job = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property Job As Job
    Get
      Try
        Return mobjJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property


#End Region

#Region "Constructors"

  Public Sub New(lpJob As Job)
    MyBase.New(lpJob.Id)
    Try
      mobjJob = lpJob
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IJobItemProxy Implementation"

  Public ReadOnly Property JobName As String Implements IJobItemProxy.JobName
    Get
      Try
        If Me.Job IsNot Nothing Then
          Return Me.Job.Name
        Else
          Return String.Empty
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ProjectName As String Implements IJobItemProxy.ProjectName
    Get
      Try
        If Me.Job IsNot Nothing AndAlso Me.Job.Project IsNot Nothing Then
          Return Me.Job.Project.Name
        Else
          Return String.Empty
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property RunBeforeJobBeginCount As Integer Implements IJobItemProxy.RunBeforeJobBeginCount
    Get
      Try
        If Me.Job IsNot Nothing Then
          Return Me.Job.RunBeforeJobBeginCount
        Else
          Throw New InvalidOperationException("Job reference not set.")
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        If Me.Job IsNot Nothing Then
          Me.Job.RunBeforeJobBeginCount = value
        Else
          Throw New InvalidOperationException("Job reference not set.")
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

End Class
