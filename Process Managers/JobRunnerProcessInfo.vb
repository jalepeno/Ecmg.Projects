'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobRunnerProcessInfo.vb
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
Imports Projects.Configuration

#End Region


Public Class JobRunnerProcessInfo

#Region "Class Variables"

  Private mobjProcess As Process
  Private mobjJobSet As JobSet

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

  Public ReadOnly Property JobSet As JobSet
    Get
      Try
        Return mobjJobSet
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpProcess As Process, lpJobSet As JobSet)
    Try
      mobjProcess = lpProcess
      mobjJobSet = lpJobSet
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
