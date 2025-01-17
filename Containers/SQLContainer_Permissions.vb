'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  SQLContainer_Permissions.vb
'   Description :  [type_description_here]
'   Created     :  10/08/2015 10:06:28 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports System.Reflection
Imports Documents.Utilities
Imports Microsoft.Data.SqlClient

#End Region

Partial Public Class SQLContainer

#Region "Private Methods"

  Private Function GetPermissionsDB() As DataTable
    Try
      Dim lobjDataTable As New DataTable()
      Dim lobjDataReader As SqlDataReader = Nothing

      lobjDataTable.Columns.Add("UserType", String.Empty.GetType())
      lobjDataTable.Columns.Add("DatabaseUserName", String.Empty.GetType())
      lobjDataTable.Columns.Add("LoginName", String.Empty.GetType())
      lobjDataTable.Columns.Add("Role", String.Empty.GetType())
      lobjDataTable.Columns.Add("PermissionType", String.Empty.GetType())
      lobjDataTable.Columns.Add("PermissionState", String.Empty.GetType())
      lobjDataTable.Columns.Add("ObjectType", String.Empty.GetType())
      lobjDataTable.Columns.Add("Schema", String.Empty.GetType())
      lobjDataTable.Columns.Add("ObjectName", String.Empty.GetType())
      lobjDataTable.Columns.Add("ColumnName", String.Empty.GetType())

      Dim lstrSql As String = Helper.GetResourceFileTextFromAssembly(PERMISSIONS_SCRIPT_FILENAME, Assembly.GetExecutingAssembly)

      Using lobjConnection As New SqlConnection(Me.ItemsLocation.Location)
        Using lobjCommand As New SqlCommand(lstrSql, lobjConnection)
          lobjCommand.CommandTimeout = COMMAND_TIMEOUT
          Helper.HandleConnection(lobjConnection)

          lobjDataReader = lobjCommand.ExecuteReader

          If (lobjDataReader.HasRows = True) Then
            lobjDataTable.Load(lobjDataReader)
          End If

        End Using
      End Using

      Return lobjDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
