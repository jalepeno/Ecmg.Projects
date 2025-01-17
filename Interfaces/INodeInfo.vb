'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  INodeInfo.vb
'   Description :  [type_description_here]
'   Created     :  1/8/2014 7:35:05 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

#End Region

Public Interface INodeInfo

  ReadOnly Property Id As String

  Property Name As String

  Property Description As String

  Property Address As String

  Property RoleCode As NodeRole

  Property StatusCode As NodeStatus

  ReadOnly Property Role As String

  ReadOnly Property Status As String

  ReadOnly Property Version As String

  ReadOnly Property CreateDate As DateTime

  'ReadOnly Property PercentCpuUsed As Single

  'ReadOnly Property PercentMemoryUsed As Single

  'ReadOnly Property ComputerInfo As IComputerInfo

  Sub Save()

  Function ToJson() As String

  Function ToStatusJson() As String

End Interface
