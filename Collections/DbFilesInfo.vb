'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  DbFilesInfo.vb
'   Description :  [type_description_here]
'   Created     :  12/2/2016 11:11:38 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.SerializationUtilities
Imports Documents.Utilities

#End Region

Public Class DbFilesInfo
  Inherits CCollection(Of DbFileInfo)

  Public Function ToXmlString AS String
    Try
      Return Serializer.Serialize.XmlString(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

End Class
