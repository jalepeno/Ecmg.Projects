'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ThreadBatchItem.vb
'   Description :  [type_description_here]
'   Created     :  7/22/2015 2:05:00 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Class ThreadBatchItem

#Region "Class Variables"

  Private mobjBatchItem As BatchItem
  Private mintResetEventIndex As Integer

#End Region

#Region "Public Properties"

  Public ReadOnly Property BatchItem() As BatchItem
    Get
      Return mobjBatchItem
    End Get
  End Property

  Public ReadOnly Property ResetEventIndex() As Integer
    Get
      Return mintResetEventIndex
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(ByVal lpBatchItem As BatchItem, _
                 ByVal lpResetEventIndex As Integer)
    mobjBatchItem = lpBatchItem
    mintResetEventIndex = lpResetEventIndex
  End Sub

#End Region

End Class