'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectInfo.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 7:51:28 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IProjectInfo
  Inherits IObjectDescriptor

  ReadOnly Property Location As String

  ReadOnly Property CreateDate As DateTime

  ReadOnly Property Jobs As IJobInfoCollection

  ReadOnly Property ItemsProcessed As Long

  ReadOnly Property WorkSummary As IWorkSummary

  ReadOnly Property DetailedWorkSummary As WorkSummaries

  ReadOnly Property IsInitialized As Boolean

  ReadOnly Property IsCompleted As Boolean

  ReadOnly Property OrphanBatchCount As Integer

  ReadOnly Property OrphanBatchItemCount As Integer

  Function ToJson() As String

End Interface
