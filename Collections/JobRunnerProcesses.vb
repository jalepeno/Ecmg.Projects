'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobRunnerProcesses.vb
'   Description :  [type_description_here]
'   Created     :  9/29/2016 1:27:25 PM
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


Public Class JobRunnerProcesses
  Inherits CCollection(Of JobRunnerProcessInfo)

#Region "Public Methods"

  Public Function GetItemByProcessId(lpProcessId As Integer) As JobRunnerProcessInfo
    Try
      For Each lobjProcessInfoItem As JobRunnerProcessInfo In Me
        If lobjProcessInfoItem.Process IsNot Nothing AndAlso lobjProcessInfoItem.Process.Id = lpProcessId
          Return lobjProcessInfoItem
        End If
      Next

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub RemoveItem(lpProcessId As Integer)
    Try
      Dim lobjProcessInfoItem As JobRunnerProcessInfo = GetItemByProcessId(lpProcessId)
      If lobjProcessInfoItem IsNot Nothing
        Items.Remove(lobjProcessInfoItem)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
