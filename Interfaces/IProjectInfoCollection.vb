'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:39:39 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IProjectInfoCollection
  Inherits ICollection(Of IProjectInfo)
  Sub Sort()
End Interface
