'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProcessInfo.vb
'   Description :  [type_description_here]
'   Created     :  9/29/2016 1:19:39 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region


Public Class ProcessInfo

#Region "Class Variables"

  Private mobjProcess As Process

#End Region

#Region "Public Properties"

  Public ReadOnly Property Process As Process
    Get
      Try
        Return mobjProcess
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpProcess As Process)
    Try
      mobjProcess = lpProcess
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Public Sub New(lpProcess As Process, lpStartEvents As StartJobRunnerEventArgs)
  '  Try
  '    mintProcessId = lpProcess.Id
  '    mstrMachineName = lpProcess.MachineName
  '    mstrProjectId = lpStartEvents.p
  '    mstrJobId = lpJobId
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

#End Region

End Class
