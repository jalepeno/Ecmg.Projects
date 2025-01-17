' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  WorkInvokedEventArgs.vb
'  Description :  [type_description_here]
'  Created     :  8/8/2012 4:04:04 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class WorkInvokedEventArgs
  Inherits WorkEventArgs

#Region "Class Variables"

  Private mstrTargetHandler As String = String.Empty

#End Region

#Region "Public Properties"

  Public ReadOnly Property TargetHandler As String
    Get
      Try
        Return mstrTargetHandler
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpJob As Job, lpTargetHandler As String)
    MyBase.New(lpJob)
    Try
      mstrTargetHandler = lpTargetHandler
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpOriginatingBatch As Batch, lpTargetHandler As String)
    MyBase.New(lpOriginatingBatch)
    Try
      mstrTargetHandler = lpTargetHandler
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJob As Job, lpOriginatingBatch As Batch, lpTargetHandler As String)
    MyBase.New(lpJob, lpOriginatingBatch)
    Try
      mstrTargetHandler = lpTargetHandler
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
