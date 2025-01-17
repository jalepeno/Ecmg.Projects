'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  DatabaseChangeListener.vb
'   Description :  [type_description_here]
'   Created     :  7/6/2014 9:30:44 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports Documents.Utilities
Imports Microsoft.Data.SqlClient

#End Region

Public Class DatabaseChangeListener

#Region "Class Variables"

  Private ReadOnly mstrConnectionString As String
  Private ReadOnly mobjConnection As SqlConnection

#End Region

#Region "Class Events"

  Public Delegate Sub NewMessage()
  Public Event OnChange As NewMessage

#End Region

#Region "Constructors"

  Public Sub New(lpConnectionString As String)
    Try
      Me.mstrConnectionString = lpConnectionString
      SqlDependency.[Stop](lpConnectionString)
      SqlDependency.Start(lpConnectionString)
      mobjConnection = New SqlConnection(lpConnectionString)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Function Start(lpChangeQuery As String) As DataTable
    Try
      Dim lobjCommand = New SqlCommand(lpChangeQuery, mobjConnection) With {
        .Notification = Nothing
      }

      Dim lobjDependency = New SqlDependency(lobjCommand)
      AddHandler lobjDependency.OnChange, AddressOf NotifyOnChange
      If mobjConnection.State = ConnectionState.Closed Then
        mobjConnection.Open()
      End If
      Dim lobjDataTable = New DataTable()
      lobjDataTable.Load(lobjCommand.ExecuteReader(CommandBehavior.CloseConnection))

      Return lobjDataTable
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Methods"

  Private Sub NotifyOnChange(sender As Object, e As SqlNotificationEventArgs)
    Try
      Dim lobjDependency = TryCast(sender, SqlDependency)
      If lobjDependency IsNot Nothing Then
        RemoveHandler lobjDependency.OnChange, AddressOf NotifyOnChange
      End If
      RaiseEvent OnChange()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Protected Overrides Sub Finalize()
    Try
      SqlDependency.[Stop](mstrConnectionString)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    Finally
      MyBase.Finalize()
    End Try
  End Sub

#End Region

End Class