' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  NotRunningInProjectException.vb
'  Description :  [type_description_here]
'  Created     :  8/8/2012 3:27:27 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Reflection
Imports System.Text
Imports Documents.Exceptions
Imports Documents.Utilities

#End Region

''' <summary>
''' To be thrown when a request is made that 
''' is only valid when running in a project.
''' </summary>
''' <remarks></remarks>
Public Class NotRunningInProjectException
  Inherits CtsException

#Region "Constructors"

  Public Sub New()
    MyBase.New(GetDefaultMessage(False))
  End Sub

  Public Sub New(lpIncludeEntryAssembly As Boolean)
    MyBase.New(GetDefaultMessage(lpIncludeEntryAssembly))
  End Sub

  Public Sub New(message As String)

  End Sub

#End Region

#Region "Private Shared Methods"

  Private Shared Function GetDefaultMessage(lpIncludeEntryAssembly As Boolean) As String
    Try

      Dim lobjMessageBuilder As New StringBuilder
      If lpIncludeEntryAssembly = False Then
        lobjMessageBuilder.Append("Unable to process request, not running as part of a project.")
      Else
        lobjMessageBuilder.AppendFormat("Unable to process request, not running as part of a project.  Running under '{0}'.", Assembly.GetEntryAssembly.FullName)
      End If

      Return lobjMessageBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
