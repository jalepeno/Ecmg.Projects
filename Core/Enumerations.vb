'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

Imports System.Runtime.Serialization

'<DataContract()> Public Enum ProcessedStatus
'  NotProcessed = 0
'  Success = 1
'  Failed = 2
'  Processing = 3
'End Enum

'<DataContract()> Public Enum OperationType
'  NotSet = -1
'  Export = 0
'  Migrate = 1
'  Delete = 2
'  CheckIn = 3
'  CheckOut = 4
'  CancelCheckOut = 5
'  Replace = 6
'  UnFile = 7
'  MigratePhysicalRecord = 20 'This is temporary so we will remove it later
'  ReplacePhysicalRecord = 21 'This is temporary so we will remove it later
'End Enum

<DataContract()> Public Enum enumSourceIdType
  SourceDocId = 0
  DestinationDocId = 1
End Enum

<DataContract()> Public Enum enumSourceType
  Search = 0
  Folder = 1
  List = 2
  OtherJob = 3
  Empty = 4
End Enum

<DataContract()> Public Enum enumListType
  TextFile = 0
  ExcelFile = 1
  DBLookup = 2
End Enum

<DataContract()> Public Enum ContainerType
  OLEDB = 0
  CSV = 1
  SQLServer = 2
End Enum

<DataContract()> Public Enum ExportScope
  Source = 0
  Destination = 1
  Both = 2
End Enum

<DataContract()> Public Enum NodeStatus
  Available = 0
  Working = 1
  Inactive = 3
End Enum

<DataContract()> Public Enum NodeRole
  Head = -1
  Worker = 0
End Enum

