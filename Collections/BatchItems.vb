'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Runtime.Serialization
Imports Documents.Core
Imports Documents.Utilities

#End Region

<DataContract()> Public Class BatchItems
  Inherits CCollection(Of BatchItem)

  Public Overridable Overloads Function Contains(ByVal item As BatchItem) As Boolean
    Try
      Dim lobjBatchItem As BatchItem = Items.FirstOrDefault(Function(b) b.Id = item.Id)
      If lobjBatchItem IsNot Nothing Then
        Return True
      Else
        Return False
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Function

End Class
