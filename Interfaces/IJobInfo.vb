'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IJobInfo.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 7:58:06 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Operations

#End Region

Public Interface IJobInfo
  Inherits IJobIdentifier

  ReadOnly Property CreateDate As DateTime
  ReadOnly Property DisplayName As String
  ReadOnly Property Description As String
  ReadOnly Property ProjectName As String
  ReadOnly Property BatchSize As Integer
  ReadOnly Property Operation As String
  ReadOnly Property Process As IProcess
  ReadOnly Property IsInitialized As Boolean
  ReadOnly Property IsCancelled As Boolean
  ReadOnly Property IsCompleted As Boolean
  ReadOnly Property IsRunning As Boolean
  ReadOnly Property BatchThreadsRunning As Integer
  ReadOnly Property CancellationReason As String
  ReadOnly Property ItemsProcessed As Long
  ReadOnly Property WorkSummary As IWorkSummary
  Function ToJson() As String

End Interface
