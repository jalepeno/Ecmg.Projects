'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IAreas.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 2:30:03 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"


#End Region

Public Interface IAreas
  Inherits ICollection(Of IArea)

  ReadOnly Property Catalog As IProjectCatalog

  Function ToAreaListings() As IAreaListings

End Interface
