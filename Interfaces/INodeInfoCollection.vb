'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  INodeInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  1/8/2014 8:09:43 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface INodeInfoCollection
  Inherits ICollection(Of INodeInfo)

  Overloads Function Contains(name As String) As Boolean

  Property Item(ByVal name As String) As INodeInfo

  Sub Sort()

  Function ToJson() As String

End Interface
