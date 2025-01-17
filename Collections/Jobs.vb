'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities

#End Region

Public Class Jobs
  Inherits CCollection(Of Job)

#Region "Class Variables"

  'This is a reference to each Job's parent Project
  Private mobjProject As Project

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(ByVal lpProject As Project)

    Try
      mobjProject = lpProject

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Public Properties"

  Public ReadOnly Property Project() As Project
    Get
      Return mobjProject
    End Get
  End Property

  Public ReadOnly Property Names As IList(of String)
    Get
      Try
        Return GetNames()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Public Methods"

  Public Overloads Sub Add(ByVal lpJob As Job)

    Try
      lpJob.SetProject(Me.Project)
      lpJob.Index = Me.Count

      MyBase.Add(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function GetBatch(ByVal lpId As String) As Batch

    Try

      Dim lobjBatch As Batch = Nothing

      For Each lobjJob As Job In Me
        lobjBatch = lobjJob.GetBatch(lpId)

        If lobjBatch IsNot Nothing Then
          Return lobjBatch
        End If

      Next

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Function GetNames() As IList(of String)
    Try
      Dim lobjNamesList As New List(Of String)
      For Each lobjJob As Job In Me
        lobjNamesList.Add(lobjJob.Name)
      Next
      lobjNamesList.Sort()

      Return lobjNamesList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
