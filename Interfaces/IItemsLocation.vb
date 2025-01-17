'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IItemsLocation.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 10:07:09 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IItemsLocation

  Property Type() As ContainerType
  Property Location() As String
  Property ServerName() As String
  Property DatabaseName() As String
  Property UserName() As String
  Property Password() As String
  Property TrustedConnection() As String
  Property DatabasePath() As String

  Function ToNativeConnectionString() As String
  Function ToOleDBConnectionString() As String
  Function ToString() As String

End Interface
