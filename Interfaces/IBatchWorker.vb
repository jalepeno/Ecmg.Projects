' ********************************************************************************
' '  Document    :  IBatchWorker.vb
' '  Description :  [type_description_here]
' '  Created     :  11/20/2012-14:30:50
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

Public Interface IBatchWorker
  Inherits IBackgroundWorker

  Property Batch() As Batch
  Property DoWorkEventArgs() As System.ComponentModel.DoWorkEventArgs

End Interface
