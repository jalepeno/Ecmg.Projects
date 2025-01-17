'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IAreaListing.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 8:21:35 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core

#End Region

Public Interface IAreaListing
  Inherits IDescription

  ReadOnly Property Id As String

  ReadOnly Property Projects As IProjectListings

End Interface
