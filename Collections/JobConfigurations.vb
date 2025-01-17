'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobConfigurations.xaml.vb
'   Description :  [type_description_here]
'   Created     :  8/18/2015 9:41:29 AM
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

Namespace Configuration

  Public Class JobConfigurations
    Inherits CCollection(Of JobConfiguration)

    Public Function AreAllNamesTheSame() As Boolean
      Try
        Dim lstrCurrentName As String = Nothing
        If Count < 2 Then
          Return False
        Else
          lstrCurrentName = Items.First.Name
        End If

        For lintItemCounter As Integer = 1 To Count - 1

          If Not Item(lintItemCounter).Name.Equals(lstrCurrentName) Then
            Return False
          End If
        Next

        Return True

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

  End Class

End Namespace