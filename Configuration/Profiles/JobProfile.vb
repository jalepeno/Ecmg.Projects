' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobProfile.vb
'  Description :  [type_description_here]
'  Created     :  11/15/2012 4:17:13 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities

#End Region

Namespace Configuration

  <DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
  Public Class JobProfile

#Region "Class Variables"

    Private mstrName As String = String.Empty
    Private mstrConnectionString As String = String.Empty
    Private mstrJobName As String = String.Empty

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets or sets the name of the profile.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Name As String
      Get
        Try
          Return mstrName
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As String)
        Try
          mstrName = value
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    ''' <summary>
    ''' Gets or sets the connection string for the associated project dabase.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConnectionString As String
      Get
        Try
          Return mstrConnectionString
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As String)
        Try
          mstrConnectionString = value
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the job to run.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobName As String
      Get
        Try
          Return mstrJobName
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As String)
        Try
          mstrJobName = value
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

#End Region

#Region "Protected Methods"

    Protected Friend Overridable Function DebuggerIdentifier() As String
      Dim lobjIdentifierBuilder As New StringBuilder
      Try
        With lobjIdentifierBuilder
          If String.IsNullOrEmpty(Me.Name) Then
            .Append("Name not set: ")
          Else
            .AppendFormat("{0}: ", Me.Name)
          End If
          If String.IsNullOrEmpty(Me.JobName) Then
            .Append("(JobName not set): ")
          Else
            .AppendFormat("({0}): ", Me.JobName)
          End If
          If String.IsNullOrEmpty(Me.ConnectionString) Then
            .Append("<ConnectionString not set>")
          Else
            .AppendFormat("<{0}>: ", Me.ConnectionString)
          End If
        End With
        Return lobjIdentifierBuilder.ToString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        Return lobjIdentifierBuilder.ToString
      End Try
    End Function

#End Region

  End Class

End Namespace