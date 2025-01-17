'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  NodeProjectPair.vb
'   Description :  [type_description_here]
'   Created     :  6/4/2014 2:52:22 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class NodeProjectPair

#Region "Class Variables"

  Private mstrNodeName As String = String.Empty
  Private mobjProject As Project = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property NodeName As String
    Get
      Try
        Return mstrNodeName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Project As Project
    Get
      Try
        Return mobjProject
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpNodeName As String, lpProject As Project)
    Try

      If String.IsNullOrEmpty(lpNodeName) Then
        Throw New ArgumentNullException("lpNodeName")
      End If

      If lpProject Is Nothing Then
        Throw New ArgumentNullException("lpProject")
      End If

      mstrNodeName = lpNodeName
      mobjProject = lpProject

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
