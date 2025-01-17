'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports Documents.Core
Imports Documents.Utilities
Imports OfficeOpenXml
Imports Projects.Operations

#End Region

Public Class WorkSummaries
  Inherits CCollection(Of WorkSummary)

#Region "Class Variables"

  Private mintTotalItems As Integer
  Private mintTotalSuccess As Integer
  Private mintTotalFailed As Integer
  Private mintTotalNotProcessed As Integer
  Private mintTotalProcessing As Integer
  Private mblnTotalRowAdded As Boolean = False

  Private mdatStartTime As DateTime
  Private mdatFinishTime As DateTime
  Private mdatLastUpdateTime As DateTime
  Private mdblPeakProcessingRate As Double
  Private mdblProcessingRate As Double

#End Region

#Region "Public Properties"

  Public ReadOnly Property Totals As WorkSummary
    Get
      Try
        Return GetTotalsRow()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property AvgProcessingTime As Double
    Get
      Try
        Return GetAverageProcessingTime()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Sub New()

  End Sub

#End Region

#Region "Public Methods"

  Public Overloads Sub Add(ByVal lpBatchSummary As WorkSummary)

    Try

      If (lpBatchSummary IsNot Nothing) Then
        mintTotalItems += lpBatchSummary.TotalItemsCount
        mintTotalFailed += lpBatchSummary.FailedCount
        mintTotalNotProcessed += lpBatchSummary.NotProcessedCount
        mintTotalProcessing += lpBatchSummary.ProcessingCount
        mintTotalSuccess += lpBatchSummary.SuccessCount
        mdblProcessingRate += lpBatchSummary.ProcessingRate
        If lpBatchSummary.PeakProcessingRate > mdblPeakProcessingRate Then
          mdblPeakProcessingRate = lpBatchSummary.PeakProcessingRate
        End If
        MyBase.Add(lpBatchSummary)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub AddTotalsRow()
    Try
      AddTotalsRow(AvgProcessingTime)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub AddTotalsRow(lpAvgProcessingTime As Single)
    Try
      If (mblnTotalRowAdded = False) Then
        MyBase.Add(New WorkSummary("TOTAL", OperationType.NotSet, mintTotalNotProcessed, _
                                   mintTotalSuccess, mintTotalFailed, mintTotalProcessing, _
                                   mintTotalItems, lpAvgProcessingTime, DateTime.MinValue, _
                                   DateTime.MinValue, Now, mdblProcessingRate, mdblPeakProcessingRate))
        mblnTotalRowAdded = True
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function ToDataTable() As DataTable
    Try
      ' Create New DataTable
      Dim lobjDataTable As New DataTable("tblProjectSummary")

      ' Create columns        
      With lobjDataTable.Columns
        .Add("JobName", System.Type.GetType("System.String"))
        .Add("Operation", System.Type.GetType("System.String"))
        .Add("NotProcessed", System.Type.GetType("System.Int32"))
        .Add("Success", System.Type.GetType("System.Int32"))
        .Add("Failed", System.Type.GetType("System.Int32"))
        .Add("Processing", System.Type.GetType("System.Int32"))
        .Add("Total", System.Type.GetType("System.Int32"))
        .Add("PercentCompleted", System.Type.GetType("System.Single"))
        .Add("PercentNotProcessed", System.Type.GetType("System.Single"))
        .Add("PercentFailed", System.Type.GetType("System.Single"))
        .Add("PercentSuccess", System.Type.GetType("System.Single"))
        .Add("AvgProcessingTime", System.Type.GetType("System.Double"))
      End With

      For Each lobjSummary As WorkSummary In Me
        lobjDataTable.Rows.Add(lobjSummary.Name, _
                               lobjSummary.Operation, _
                               lobjSummary.NotProcessedCount, _
                               lobjSummary.SuccessCount, _
                               lobjSummary.FailedCount, _
                               lobjSummary.ProcessingCount, _
                               lobjSummary.TotalItemsCount, _
                               lobjSummary.ProcessedPercentage, _
                               lobjSummary.NotProcessedPercentage, _
                               lobjSummary.FailurePercentage, _
                               lobjSummary.SuccessPercentage, _
                               lobjSummary.AvgProcessingTime)
      Next

      Return lobjDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToSpreadsheet() As Object
    Try
      Dim lobjExcelPackage As ExcelPackage = ExcelHelper.CreateSpreadSheetFromDataTable("Project Summary", Me.ToDataTable)

      ' ExcelWorksheet ws = p.Workbook.Worksheets[1];
      Dim lobjWorksheet As ExcelWorksheet = lobjExcelPackage.Workbook.Worksheets(1)
      Dim lintEndColumn As Integer = lobjWorksheet.Dimension.End.Column
      Dim lintEndRow As Integer = lobjWorksheet.Dimension.End.Row
      lobjWorksheet.Cells(1, 3, lintEndRow, 7).Style.Numberformat.Format = "#,##0"
      lobjWorksheet.Cells(1, 8, lintEndRow, 11).Style.Numberformat.Format = "0.00%"
      lobjWorksheet.Cells(1, 12, lintEndRow, 12).Style.Numberformat.Format = "0.00"
      lobjWorksheet.Cells(lintEndRow, 1, lintEndRow, lintEndColumn).Style.Font.Bold = True
      lobjWorksheet.Cells(lintEndRow, 1, lintEndRow, lintEndColumn).Style.Font.UnderLine = True
      lobjWorksheet.Cells(lintEndRow, 1, lintEndRow, lintEndColumn).Style.Font.UnderLineType = Style.ExcelUnderLineType.DoubleAccounting

      'lobjExcelPackage.SaveAs(New IO.FileInfo(lstrOutputPath))
      Return lobjExcelPackage

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function ToSQLTotal() As String
    Try

      Dim lobjTotalsRow As WorkSummary = GetTotalsRow()

      If lobjTotalsRow IsNot Nothing Then
        Return lobjTotalsRow.ToSQLTotal
      End If

      Return String.Empty

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Methods"

  Private Function GetTotalsRow() As WorkSummary
    Try

      If mblnTotalRowAdded = True Then
        Dim lobjQueryTotalsRow As Object = From item In Items Where item.Name = "TOTAL"
        For Each lobjSummary As WorkSummary In lobjQueryTotalsRow
          Return lobjSummary
        Next

        Return Nothing

      Else
        Return Nothing
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetAverageProcessingTime() As Double
    Try
      Dim lintTotalItemsProcessed As Integer
      Dim ldblTotalProcessingTime As Double

      For Each lobjWorkSummary As WorkSummary In Me
        lintTotalItemsProcessed += (lobjWorkSummary.SuccessCount + lobjWorkSummary.FailedCount)
        ldblTotalProcessingTime += lobjWorkSummary.TotalProcessingTime
      Next

      Return ldblTotalProcessingTime / lintTotalItemsProcessed

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
