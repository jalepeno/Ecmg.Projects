' ********************************************************************************
' '  Document    :  IBackgroundWorker.vb
' '  Description :  [type_description_here]
' '  Created     :  11/20/2012-14:28:18
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

Public Interface IBackgroundWorker

  Sub CancelAsync()
  Sub ReportProgress(percentProgress As Integer)
  Sub ReportProgress(percentProgress As Integer, userState As Object)
  Sub RunWorkerAsync()
  Sub RunWorkerAsync(argument As Object)

  ReadOnly Property CancellationPending As Boolean
  ReadOnly Property IsBusy As Boolean
  Property WorkerReportsProgress As Boolean
  Property WorkerSupportsCancellation As Boolean
  Event DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
  Event ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs)
  Event RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)

End Interface
