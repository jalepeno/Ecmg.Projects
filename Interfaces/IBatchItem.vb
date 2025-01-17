Imports Operations

' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  IBatchItem.vb
'  Description :  Interface to be implemented by all batch item classes.
'  Created     :  11/15/2011 8:45:47 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

Public Interface IBatchItem
  Inherits IWorkItem

  Property BatchId() As String

  ReadOnly Property Operation() As String

  Sub BeginProcessItem()
  Sub EndProcessItem(ByVal lpProcessedStatus As ProcessedStatus,
                            ByVal lpProcessedMessage As String,
                            ByVal lpDestDocId As String)
  Sub EndProcessItem(ByVal lpProcessedStatus As ProcessedStatus,
                            ByVal lpProcessedMessage As String,
                            ByVal lpDestDocId As String,
                            ByVal lpEndTime As DateTime)

End Interface
