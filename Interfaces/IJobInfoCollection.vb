'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IJobInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 2:07:22 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

#End Region

Public Interface IJobInfoCollection
  Inherits ICollection(Of IJobInfo)

  Function Item(id As String) As IJobInfo

  Sub Sort()

End Interface
