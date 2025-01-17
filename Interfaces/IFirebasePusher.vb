'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IFirebasePusher.vb
'   Description :  [type_description_here]
'   Created     :  6/26/2014 12:25:25 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Friend Interface IFirebasePusher

  Sub UpdateFirebase(lpRootPath As String)

  Sub UpdateFirebase(lpRootPath As String, lpValue As String)

  Function ToFireBaseJson() As String


End Interface
