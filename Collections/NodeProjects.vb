'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  NodeProjects.vb
'   Description :  [type_description_here]
'   Created     :  6/4/2014 2:59:17 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Utilities

#End Region

Public Class NodeProjects
  Inherits CCollection(Of NodeProjectPair)

  Public Overloads Sub Add(lpNodeName As String, lpProject As Project)
    Try
      For Each lobjNodeProject As NodeProjectPair In Me
        If lobjNodeProject.NodeName.Equals(lpNodeName) AndAlso
          lobjNodeProject.Project.Name.Equals(lpProject.Name) Then
          Throw New ItemAlreadyExistsException(String.Format("{0}.{1}",
            lpNodeName, lobjNodeProject.Project.Name))
        End If
      Next
      MyBase.Add(New NodeProjectPair(lpNodeName, lpProject))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function GetProjectsForNode(lpNodeName As String) As Projects
    Try

      Dim lobjProjects As New Projects

      For Each lobjNodeProject As NodeProjectPair In Me
        If lobjNodeProject.NodeName.Equals(lpNodeName) Then
          lobjProjects.Add(lobjNodeProject.Project)
        End If
      Next

      Return lobjProjects

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

End Class
