' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobRepositories.vb
'  Description :  Used to contain references to the pair of repositories for a job
'  Created     :  10/4/2011 6:34:52 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class JobConnections

#Region "Class Variables"

  Private mobjJob As Job

#End Region

#Region "Properties"

  Friend ReadOnly Property Job As Job
    Get
      Return mobjJob
    End Get
  End Property

  Friend Property SourceConnection As JobConnection

  Friend Property DestinationConnection As JobConnection

#End Region

#Region "Constructors"

  Friend Sub New(lpJob As Job)
    Try
      mobjJob = lpJob
      InitializeConnections()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Methods"

  Private Sub InitializeConnections()
    Try

      If Not String.IsNullOrEmpty(Job.SourceConnectionString) Then
        SourceConnection = New JobConnection(Job, Job.SourceConnectionString, ExportScope.Source)
      End If

      If Not String.IsNullOrEmpty(Job.DestinationConnectionString) Then
        SourceConnection = New JobConnection(Job, Job.DestinationConnectionString, ExportScope.Destination)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
