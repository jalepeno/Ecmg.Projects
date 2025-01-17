' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobRepository.vb
'  Description :  Used to manage a reference to a repository for a job
'  Created     :  10/4/2011 6:31:05 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Providers
Imports Documents.Utilities

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class JobConnection

#Region "Class Variables"

  Private mobjContentSource As ContentSource

#End Region

#Region "Properties"

  Friend Property Scope As ExportScope
  Friend Property ConnectionString As String
  Friend Property Job As Job

#End Region

#Region "Constructors"

  Friend Sub New(lpJob As Job, lpConnectionString As String, lpScope As ExportScope)
    Try
      Job = lpJob
      ConnectionString = lpConnectionString
      Scope = lpScope
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Not String.IsNullOrEmpty(Job.Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Job.Name)
      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0}", Scope.ToString)

      lobjIdentifierBuilder.AppendFormat(" ({0})", ConnectionString)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function


End Class
