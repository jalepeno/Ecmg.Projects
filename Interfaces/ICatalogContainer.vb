'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ICatalogContainer.vb
'   Description :  [type_description_here]
'   Created     :  12/13/2013 6:30:54 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface ICatalogContainer

  'Property CatalogConnectionString As String

  Sub SaveArea(lpArea As IArea)

  Sub SaveCatalogConfiguration(lpCatalogId As String, lpConfiguration As ICatalogConfiguration)

  Sub SaveProject(lpProject As IProjectDescription)

  Function GetAreas() As IAreas

  Sub DeleteArea(lpAreaId As String)

  Sub SaveNode(lpNode As INodeInfo)

  Sub DeleteNode(lpNodeId As String)

  Function GetNodes() As INodeInfoCollection

  Function GetNode(lpNodeId As String) As INodeInfo

  Sub CreateCatalog(lpCatalogConnectionString As String, lpCatalogName As String, lpCatalogDescription As String)

  Function GetCurrentCatalog() As IProjectCatalog

  Function ProjectExists(lpProjectId As String) As Boolean

  Sub AddProjectFromConnectionString(lpProjectConnectionString As String)

End Interface
