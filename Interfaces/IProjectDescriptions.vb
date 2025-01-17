'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectDescriptions.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 10:36:13 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"


#End Region

Public Interface IProjectDescriptions
  Inherits ICollection(Of IProjectDescription)

  Function ToProjectListings() As IProjectListings

End Interface
