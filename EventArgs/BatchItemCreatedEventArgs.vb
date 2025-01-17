'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  BatchItemCreatedEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  1/7/2013 1:58:09 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class BatchItemCreatedEventArgs
  ' (ByVal lpJobName As String, ByVal lpCurrentCount As Integer, ByVal lpTotalCount As Integer)

#Region "Class Variables"

  Private mstrJobName As String = String.Empty
  Private mintCurrentCount As Integer
  Private mintTotalCount As Integer
  Private mstrMessage As String = String.Empty

#End Region

#Region "Public Properties"

  Public Property JobName As String
    Get
      Try
        Return mstrJobName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrJobName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property CurrentCount As Integer
    Get
      Try
        Return mintCurrentCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintCurrentCount = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property TotalCount As Integer
    Get
      Try
        Return mintTotalCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintTotalCount = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Message As String
    Get
      Try
        Return mstrMessage
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrMessage = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#Region "Constructors"

  Public Sub New(ByVal lpJobName As String, ByVal lpCurrentCount As Integer, ByVal lpTotalCount As Integer)
    Try
      JobName = lpJobName
      CurrentCount = lpCurrentCount
      TotalCount = lpTotalCount
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(ByVal lpJobName As String, ByVal lpCurrentCount As Integer,
                 ByVal lpTotalCount As Integer, ByVal lpMessage As String)
    Try
      JobName = lpJobName
      CurrentCount = lpCurrentCount
      TotalCount = lpTotalCount
      Message = lpMessage
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region

End Class
