'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectDescription.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 10:04:57 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IProjectDescription
  Property Id As String
  Property Name As String
  ReadOnly Property CreateDate As DateTime
  Property ItemsProcessed As Long
  Property Area As IArea
  ReadOnly Property AreaName As String
  ReadOnly Property WorkSummary As IWorkSummary
  Property Location As IItemsLocation
  Sub Save()
  Function GetProjectInfo() As IProjectInfo
  Function ToJson() As String

End Interface
