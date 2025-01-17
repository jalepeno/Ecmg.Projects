'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IBatchInfo.vb
'   Description :  [type_description_here]
'   Created     :  2/5/2014 7:36:48 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

#End Region

Public Interface IBatchInfo

  ReadOnly Property Id As String

  ReadOnly Property ParentJob As IJobInfo

  ReadOnly Property Name As String

  ReadOnly Property CreateDate As DateTime

  ReadOnly Property WorkSummary As IWorkSummary

End Interface
