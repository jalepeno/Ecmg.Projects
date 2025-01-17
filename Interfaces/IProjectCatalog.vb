'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IProjectCatalog.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 10:01:36 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IProjectCatalog
  Inherits IDisposable

  ReadOnly Property Id As String

  ReadOnly Property Configuration As ICatalogConfiguration

  Property ConnectionString As String

  ReadOnly Property Areas As IAreas

  ReadOnly Property Info As ICatalogInfo

  Property DefaultArea As IArea

  ReadOnly Property Nodes As INodeInfoCollection

  ReadOnly Property ThisNode As INodeInfo

  ReadOnly Property ProjectCount() As Integer

  ReadOnly Property Container As ICatalogContainer

  ReadOnly Property IsDisposed As Boolean

  ReadOnly Property ReferenceCount As Integer

  Sub Open()

  Sub Refresh()

  Function CreateArea(lpName As String, lpDescription As String)

  Function GetArea(lpAreaId As String) As IArea

  Function GetNode(lpNodeId As String) As INodeInfo

  Function GetNodes() As INodeInfoCollection

  Function ProjectExists(lpProjectId As String) As Boolean

  Function GetProject(lpProjectId As String) As IProjectDescription

  Function GetProjectInfo(lpProjectId As String) As IProjectInfo

  Function GetJobInfo(lpJob As Job) As IJobInfo

  Function GetJobInfo(lpProjectId As String, lpJobId As String) As IJobInfo

  Function GetDetailedJobInfo(lpProjectId As String, lpJobId As String) As IDetailedJobInfo

  Sub MoveProject(lpProjectId As String, lpNewAreaId As String)

  Function OpenProject(lpProjectId As String) As Project

  Event NodeStatusRefreshed As NodeEventHandler

  Function ToJson() As String

  Function ToProjectDirectory() As IProjectDirectory

End Interface
