'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  StatusEntries.vb
'   Description :  [type_description_here]
'   Created     :  7/5/2014 10:55:34 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Collections.ObjectModel
Imports Documents.Utilities

#End Region

Public Class StatusEntries
  Inherits ObservableCollection(Of StatusEntry)

  Default Shadows ReadOnly Property Item(lpStatus As String) As StatusEntry
    Get
      Try
        Dim list = From lobjEntry In Items Where _
          lobjEntry.Status = lpStatus Select lobjEntry

        Return list.FirstOrDefault()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

End Class
