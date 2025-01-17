'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IDBLookupSourceEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  11/6/2015 2:43:19 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IDBLookupSourceEventArgs

  Property TargetJob As Job
  ReadOnly Property SourceConnectionString As String
  ReadOnly Property NativeSourceConnectionString As String
  ReadOnly Property SourceSQLStatement As String

End Interface
