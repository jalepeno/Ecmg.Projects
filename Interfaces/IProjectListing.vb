'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectListing.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 8:20:26 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IProjectListing

  Property Id As String
  Property Area As String
  Property Name As String
  ReadOnly Property CreateDate As DateTime
  Property ItemsProcessed As Long
  ReadOnly Property WorkSummary As IWorkSummary

End Interface
