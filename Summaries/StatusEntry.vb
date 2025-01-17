'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  StatusEntry.vb
'   Description :  [type_description_here]
'   Created     :  7/4/2014 10:16:24 AM
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

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class StatusEntry
  Implements IComparable

#Region "Class Variables"

  Private mstrName As String
  Private mstrStatus As String
  Private mlngValue As Long
  Private mobjChildEntries As New StatusEntries

#End Region

#Region "Public Properties"

  Public ReadOnly Property Name As String
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

  Public ReadOnly Property Status As String
    Get
      Try
        Return mstrStatus
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property EntryValue As Long
    Get
      Try
        Return mlngValue
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property EntryValueDouble As Double
    Get
      Try
        Return CDbl(mlngValue)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Children As StatusEntries
    Get
      Try
        Return mobjChildEntries
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Friend Sub New(lpName As String, lpStatus As String, lpValue As Long)
    Try
      mstrName = lpName
      mstrStatus = lpStatus
      mlngValue = lpValue
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IComparable Implementation"

  Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
    Try
      Return EntryValue.CompareTo(obj.EntryValue)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Name)

      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0}", Status)
      lobjIdentifierBuilder.AppendFormat(" - {0}", EntryValue)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region
End Class
