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

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> Public Class ProvisionStatuses
  Inherits CCollection(Of ProvisionStatus)

#Region "Public Properties"

  Public Property Project As Project

  Public ReadOnly Property TotalItemCount As Long
    Get

      Try
        Return GetTotalItemCount()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpProject As Project)

    Try
      Me.Project = lpProject

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Public Methods"

  Public Overloads Sub Add(lpName As String, _
                           lpItemCount As Long)

    Try
      MyBase.Add(New ProvisionStatus(lpName, lpItemCount))

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Protected Methods"

  Protected Friend Shadows Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Me.Project Is Nothing Then
        lobjIdentifierBuilder.Append("Project Not Set: ")

      Else
        lobjIdentifierBuilder.AppendFormat("{0}: ", Me.Project.Name)
      End If

      If Items.Count = 0 Then
        lobjIdentifierBuilder.Append("No jobs")

      Else
        lobjIdentifierBuilder.AppendFormat("{0} items in {1} jobs", Me.TotalItemCount, Items.Count)
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Function GetTotalItemCount() As Long

    Try

      Dim llngTotalItemCount As Long = 0

      For Each lobjStatus As ProvisionStatus In Me
        llngTotalItemCount += lobjStatus.ItemCount
      Next

      Return llngTotalItemCount

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

#End Region

End Class
