'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IBatchLock.vb
'   Description :  [type_description_here]
'   Created     :  5/16/2013 4:18:34 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

Public Interface IBatchLock

  ReadOnly Property Id As String
  ReadOnly Property BatchId As String
  ReadOnly Property JobId As String
  ReadOnly Property JobName As String
  ReadOnly Property IsLocked As Boolean
  ReadOnly Property LockDate As DateTime
  ReadOnly Property UnlockDate As DateTime
  ReadOnly Property LockedBy As String

End Interface
