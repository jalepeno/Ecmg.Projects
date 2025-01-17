'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ICatalogInfo.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:43:48 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface ICatalogInfo

  ReadOnly Property Areas As IAreaInfoCollection

  Function ToJson() As String

End Interface
