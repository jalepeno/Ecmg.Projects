'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobIdentifier.vb
'   Description :  [type_description_here]
'   Created     :  5/13/2014 10:11:27 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class JobIdentifier
  Implements IJobIdentifier

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty

#End Region

#Region "Constructors"

  Public Sub New(lpId As String, lpName As String)
    Try
      mstrId = lpId
      mstrName = lpName
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IJobIdentifier Implementation"

  Public ReadOnly Property Id As String Implements IJobIdentifier.Id
    Get
      Try
        Return mstrId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Name As String Implements IJobIdentifier.Name
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try
      If Not String.IsNullOrEmpty(Id) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Id)

      Else
        lobjIdentifierBuilder.Append("Id not set: ")
      End If

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}", Name)

      Else
        lobjIdentifierBuilder.Append("<Name not set>")
      End If

      Return lobjIdentifierBuilder.ToString()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
