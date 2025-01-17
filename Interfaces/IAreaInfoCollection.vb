'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IAreaInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:44:20 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IAreaInfoCollection
  Inherits ICollection(Of IAreaInfo)
  Sub Sort()
End Interface
