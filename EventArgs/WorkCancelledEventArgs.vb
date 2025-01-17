' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  WorkCancelledEventArgs.vb
'  Description :  Used as a parameter to the BatchCancelled and JobCancelled event.
'  Created     :  10/17/2011 9:26:17 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"


#End Region

Public Class WorkCancelledEventArgs
  Inherits WorkEventArgs

#Region "Constructors"

  Public Sub New(lpJob As IJobInfo)
    MyBase.New(lpJob)
  End Sub

  Public Sub New(lpOriginatingBatch As Batch)
    MyBase.New(lpOriginatingBatch)
  End Sub

#End Region

End Class
