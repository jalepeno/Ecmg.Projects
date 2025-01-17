' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  WorkErrorEventArgs.vb
'  Description :  [type_description_here]
'  Created     :  8/1/2012 3:25:40 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class WorkErrorEventArgs
  Inherits WorkEventArgs

#Region "Class Variables"

  Private mstrErrorMessage As String = String.Empty
  Private mobjException As Exception

#End Region

#Region "Public Properties"

  Public ReadOnly Property ErrorMessage As String
    Get
      Try
        Return mstrErrorMessage
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Exception As Exception
    Get
      Return mobjException
    End Get
  End Property



#End Region
#Region "Constructors"

  Public Sub New(lpJob As Job, lpErrorMessage As String)
    MyBase.new(lpJob)
    Try
      mstrErrorMessage = lpErrorMessage
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpOriginatingBatch As Batch, lpErrorMessage As String)
    MyBase.New(lpOriginatingBatch)
    Try
      mstrErrorMessage = lpErrorMessage
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJob As Job, lpException As Exception)
    MyBase.new(lpJob)
    Try
      mobjException = lpException
      mstrErrorMessage = lpException.Message
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpOriginatingBatch As Batch, lpException As Exception)
    MyBase.New(lpOriginatingBatch)
    Try
      mobjException = lpException
      mstrErrorMessage = lpException.Message
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
