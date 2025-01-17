'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectJobPair.vb
'   Description :  [type_description_here]
'   Created     :  6/10/2014 8:33:42 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class ProjectJobPair

#Region "Class Variables"

  Private mstrProjectName As String = String.Empty
  Private mstrJobName As String = String.Empty

#End Region

#Region "Public Properties"

  Public ReadOnly Property ProjectName As String
    Get
      Try
        Return mstrProjectName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property JobName As String
    Get
      Try
        Return mstrJobName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpProjectName As String, lpJobName As String)
    Try

      If String.IsNullOrEmpty(lpProjectName) Then
        Throw New ArgumentNullException("lpProjectName")
      End If

      If lpJobName Is Nothing Then
        Throw New ArgumentNullException("lpJobName")
      End If

      mstrProjectName = lpProjectName
      mstrJobName = lpJobName

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Overrides Function ToString() As String
    Try
      Return String.Format("{0}: {1}", Me.ProjectName, Me.JobName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
