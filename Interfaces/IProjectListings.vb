'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectListings.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 8:22:56 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"


#End Region
Public Interface IProjectListings
  Inherits ICollection(Of IProjectListing)

  Function Item(id As String) As IProjectListing

  Sub Sort()

  Function ToJson() As String

End Interface
