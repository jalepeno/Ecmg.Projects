'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Core
Imports Documents.Utilities

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> Public Class ProvisionStatus
  Implements INamedItem

#Region "Class Variables"

#End Region

#Region "Public Properties"

  Public Property Name As String _
         Implements INamedItem.Name

  Public Property ItemCount As Long

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpName As String, _
                 lpItemCount As Long)

    Try
      Me.Name = lpName
      Me.ItemCount = lpItemCount

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If String.IsNullOrEmpty(Me.Name) Then
        lobjIdentifierBuilder.Append("Name Not Set: ")

      Else
        lobjIdentifierBuilder.AppendFormat("{0}: ", Me.Name)
      End If

      If ItemCount = 0 Then
        lobjIdentifierBuilder.Append("No Items")

      Else
        lobjIdentifierBuilder.AppendFormat("{0} items", Me.ItemCount)
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

End Class
