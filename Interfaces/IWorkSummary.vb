'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IWorkSummary.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 8:10:06 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IWorkSummary

  ReadOnly Property Name As String

  ReadOnly Property Operation As String

#Region "Counts"

  ReadOnly Property NotProcessedCount() As Long
  ReadOnly Property ProcessedCount As Long
  ReadOnly Property SuccessCount() As Long
  ReadOnly Property FailedCount() As Long
  ReadOnly Property ProcessingCount() As Integer
  ReadOnly Property TotalItemsCount() As Long

#End Region

#Region "Percentages"

  ReadOnly Property FailurePercentage As Single
  ReadOnly Property SuccessPercentage As Single
  ReadOnly Property ProcessedPercentage As Single
  ReadOnly Property ProgressPercentage As Single
  ReadOnly Property NotProcessedPercentage As Single

#End Region

  ReadOnly Property StartTime As DateTime

  ReadOnly Property FinishTime As DateTime

  ReadOnly Property LastUpdateTime As DateTime

  ReadOnly Property AvgProcessingTime() As Double

  ReadOnly Property ProcessingRate() As Double

  ReadOnly Property PeakProcessingRate() As Double

  ReadOnly Property TotalProcessingTime As Double

  ReadOnly Property IsCompleted As Boolean

  ReadOnly Property IsInitialized As Boolean

  ReadOnly Property StatusEntries As IList(Of StatusEntry)

End Interface
