' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  FailureSummaries.vb
'  Description :  [type_description_here]
'  Created     :  01/23/2011 1:44:32 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports Documents.Core
Imports Documents.Utilities

#End Region

Public Class FailureSummaries
  Inherits CCollection(Of FailureSummary)

#Region "Class Variables"

  Private mblnTotalRowAdded As Boolean = False
  Private mintTotalFailures As Integer

#End Region

#Region "Public Properties"

  Public ReadOnly Property TotalFailures As Integer
    Get
      Return mintTotalFailures
    End Get
  End Property

#End Region

#Region "Public Methods"

  Public Shadows Sub Add(ByVal lpJobName As String, ByVal lpMessage As String, ByVal lpMessageCount As Integer)
    Try

      If Not String.IsNullOrEmpty(lpMessage) Then
        MyBase.Add(New FailureSummary(lpJobName, lpMessage, lpMessageCount))
        mintTotalFailures += lpMessageCount
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Shadows Sub Add(ByVal lpFailureSummary As FailureSummary)
    Try
      MyBase.Add(lpFailureSummary)
      mintTotalFailures += lpFailureSummary.Count
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub AddTotalsRow(ByVal lpJobName As String)

    If (mblnTotalRowAdded = False) Then
      MyBase.Add(New FailureSummary(lpJobName, "TOTAL", TotalFailures))
      mblnTotalRowAdded = True
    End If

  End Sub

  Public Function ToDataTable() As DataTable
    Try
      ' Create New DataTable
      Dim lobjDataTable As New DataTable("tblFailureSummaries")

      ' Create columns        
      With lobjDataTable.Columns
        .Add("Job", System.Type.GetType("System.String"))
        .Add("FailureCount", System.Type.GetType("System.Int32"))
        .Add("Message", System.Type.GetType("System.String"))
      End With

      For Each lobjFailureSummary As FailureSummary In Me
        lobjDataTable.Rows.Add(lobjFailureSummary.JobName, lobjFailureSummary.Count, lobjFailureSummary.Message)
      Next

      Return lobjDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Public Function ToSpreadsheetStream() As Stream
  '  Try
  '    Return ExcelHelper.CreateSpreadSheetFromDataTable("Failure Summary", Me.ToDataTable)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Public Function ToSpreadsheet() As Object
    Try
      Return ExcelHelper.CreateSpreadSheetFromDataTable("Failure Summary", Me.ToDataTable)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
