'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ListDBLookupSourceEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  11/6/2015 2:47:49 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class ListDBLookupSourceEventArgs
  Implements IDBLookupSourceEventArgs

#Region "Class Variables"

  Private mstrSourceSQLStatement As String = String.Empty
  Private mobjTargetJob As Job = Nothing
  Private mstrSourceConnectionString As String = String.Empty

#End Region

#Region "Constructors"

  Public Sub New(lpTargetJob As Job, lpSourceSqlStatement As String, lpSourceConnectionString As String)
    Try

      If lpTargetJob Is Nothing Then
        Throw New ArgumentNullException("lpTargetJob")
      End If

      If String.IsNullOrEmpty("lpSourceSqlStatement") Then
        Throw New ArgumentNullException("lpSourceSqlStatement")
      End If

      If String.IsNullOrEmpty("lpSourceConnectionString") Then
        Throw New ArgumentNullException("lpSourceConnectionString")
      End If

      mobjTargetJob = lpTargetJob
      mstrSourceConnectionString = lpSourceConnectionString
      mstrSourceSQLStatement = lpSourceSqlStatement

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IDBLookupSourceEventArgs Implementation"

  Public ReadOnly Property SourceConnectionString As String Implements IDBLookupSourceEventArgs.SourceConnectionString
    Get
      Try
        Return mstrSourceConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property NativeSourceConnectionString As String Implements IDBLookupSourceEventArgs.NativeSourceConnectionString
    Get
      Try
        Return mstrSourceConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property SourceSQLStatement As String Implements IDBLookupSourceEventArgs.SourceSQLStatement
    Get
      Try
        Return mstrSourceSQLStatement
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property TargetJob As Job Implements IDBLookupSourceEventArgs.TargetJob
    Get
      Try
        Return mobjTargetJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Job)
      Try
        mobjTargetJob = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

End Class
