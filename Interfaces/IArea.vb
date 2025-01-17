'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IArea.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 2:28:38 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core

#End Region

Public Interface IArea
  Inherits IDescription

  ReadOnly Property Id As String

  ReadOnly Property Projects As IProjectDescriptions

  ReadOnly Property Catalog As IProjectCatalog

  Sub SetCatalog(lpCatalog As IProjectCatalog)

  Sub AddProject(lpProject As IProjectDescription)

  Function OpenProject(lpProjectId As String) As Project

  Sub Rename(lpNewName As String)

  Sub Save()

  Sub Delete()

  Function ToAreaListing() As IAreaListing

End Interface
