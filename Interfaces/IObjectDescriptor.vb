'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IObjectDescriptor.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:01:15 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------


Public Interface IObjectDescriptor

  ReadOnly Property Id As String
  ReadOnly Property Name As String
  ReadOnly Property Description As String

End Interface
