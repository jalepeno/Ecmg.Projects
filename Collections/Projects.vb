'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  Projects.vb
'   Description :  [type_description_here]
'   Created     :  5/30/2014 1:39:27 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities

#End Region

Public Class Projects
  Inherits CCollection(Of Project)

  Public Shadows Function Item(id As String) As Project
    Try

      Dim list As Object = From lobjItem In Items Where _
        (String.Compare(lobjItem.Id, id, True) = 0) Or _
        (String.Compare(lobjItem.Name, id, True) = 0) Select lobjItem

      For Each lobjItem As Project In list
        Return lobjItem
      Next

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

End Class
