'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectDirectory.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 10:09:31 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IProjectDirectory
  Inherits IDisposable

  ReadOnly Property Projects As IProjectListings

  ReadOnly Property ProjectCount() As Integer

  ReadOnly Property IsDisposed As Boolean

  Function GetProject(lpProjectId As String) As IProjectListing

  Function ToJson() As String

End Interface
