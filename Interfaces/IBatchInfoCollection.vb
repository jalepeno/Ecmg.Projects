'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IBatchInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  2/5/2014 7:53:16 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IBatchInfoCollection
  Inherits ICollection(Of IBatchInfo)

  Sub Sort()

End Interface