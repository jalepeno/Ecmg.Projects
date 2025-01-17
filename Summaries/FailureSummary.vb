' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  FailureSummary.vb
'  Description :  [type_description_here]
'  Created     :  01/23/2011 1:35:51 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
' Imports System.Windows.Media

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class FailureSummary

#Region "Class Variables"

  Private mstrJobName As String = String.Empty
  Private mstrMessage As String = String.Empty
  Private mintMessageCount As Integer

#End Region

#Region "Public Properties"

  Public ReadOnly Property JobName As String
    Get
      Return mstrJobName
    End Get
  End Property

  Public ReadOnly Property Message As String
    Get
      Return mstrMessage
    End Get
  End Property

  Public ReadOnly Property Count As Integer
    Get
      Return mintMessageCount
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(ByVal lpJob As String, ByVal lpMessage As String, ByVal lpMessageCount As Integer)
    Try
      mstrJobName = lpJob
      mstrMessage = lpMessage
      mintMessageCount = lpMessageCount
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Protected Methods"

  Protected Function DebuggerIdentifier() As String
    Try
      Return String.Format("{0}-{1}: {2}", JobName, Count, Message)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
