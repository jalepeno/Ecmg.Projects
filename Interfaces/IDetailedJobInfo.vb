﻿'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  IDetailedJobInfo.vb
'   Description :  [type_description_here]
'   Created     :  2/5/2014 9:16:01 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
Public Interface IDetailedJobInfo
  Inherits IJobInfo

  ReadOnly Property Batches As IBatchInfoCollection

End Interface
