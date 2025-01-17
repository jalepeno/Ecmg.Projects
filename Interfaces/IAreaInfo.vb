'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IAreaInfo.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:38:08 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IAreaInfo
  Inherits IObjectDescriptor

  ReadOnly Property Projects As IProjectInfoCollection

  Function ToJson() As String

End Interface
