' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  ProjectRepositories.vb
'  Description :  Used to manage the set of repositories for a project
'  Created     :  10/4/2011 6:36:54 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Providers
Imports Documents.Utilities

#End Region

Public Class ProjectConnections
  Inherits CCollection(Of JobConnections)

#Region "Class Variables"

  Private mobjConnectionStrings As New List(Of String)
  Private mobjRepositoryDictionary As New Dictionary(Of String, Repository)

#End Region

#Region "Properties"

  Friend Project As Project

  Friend ReadOnly Property RepositoryDictionary As Dictionary(Of String, Repository)
    Get
      Return mobjRepositoryDictionary
    End Get
  End Property

  Friend ReadOnly Property ConnectionStrings As List(Of String)
    Get
      Return mobjConnectionStrings
    End Get
  End Property

#End Region

#Region "Constructors"

  Friend Sub New()

  End Sub

  Friend Sub New(lpProject As Project)
    Try
      Initalize(lpProject)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Friend Methods"

  Friend Sub Initalize(lpProject)
    Try
      Project = lpProject
      InitializeJobConnections()
      InitializeConnectionStrings()
      InitializeRepositoryDictionary()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Methods"

  Private Sub InitializeJobConnections()
    Try
      Clear()
      For Each lobjJob As Job In Me.Project.Jobs
        Add(New JobConnections(lobjJob))
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Initializes a unique set of content source connection strings for the current project
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub InitializeConnectionStrings()
    Try

      ' Clear the list 
      ConnectionStrings.Clear()

      ' Loop through each job connection pair and add any new ones to the collection
      For Each lobjJobConnection As JobConnections In Me
        ' Check and possibly add the source connection string
        If (lobjJobConnection.SourceConnection IsNot Nothing) AndAlso _
        (Not String.IsNullOrEmpty(lobjJobConnection.SourceConnection.ConnectionString)) Then
          If ConnectionStrings.Contains(lobjJobConnection.SourceConnection.ConnectionString) = False Then
            ConnectionStrings.Add(lobjJobConnection.SourceConnection.ConnectionString)
          End If
        End If

        ' Check and possibly add the destination connection string
        If (lobjJobConnection.DestinationConnection IsNot Nothing) AndAlso _
          (Not String.IsNullOrEmpty(lobjJobConnection.DestinationConnection.ConnectionString)) Then
          If ConnectionStrings.Contains(lobjJobConnection.DestinationConnection.ConnectionString) = False Then
            ConnectionStrings.Add(lobjJobConnection.DestinationConnection.ConnectionString)
          End If
        End If

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub InitializeRepositoryDictionary()
    Try

      RepositoryDictionary.Clear()

      Dim lobjCandidateRepository As Repository = Nothing

      For Each lstrConnectionString As String In ConnectionStrings

        ' Create the repository
        lobjCandidateRepository = GetRepository(lstrConnectionString)

        If lobjCandidateRepository IsNot Nothing Then
          ' Add the connection string and the repostory to the dictionary
          RepositoryDictionary.Add(lstrConnectionString, lobjCandidateRepository)
        End If

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Creates and returns a repository object from the specified connection string.
  ''' </summary>
  ''' <param name="lpConnectionString">The candidate connection string.</param>
  ''' <returns>A Repository object for the specified connection.</returns>
  ''' <remarks></remarks>
  Private Function GetRepository(lpConnectionString As String) As Repository
    Try

      Dim lobjCandidateRepository As Repository = Nothing
      Dim lobjCandidateContentSource As ContentSource = Nothing

      ' Try to create a content source from the connection string
      lobjCandidateContentSource = New ContentSource(lpConnectionString)

      If lobjCandidateContentSource IsNot Nothing Then
        ' Make sure the content sourc is initialized
        If lobjCandidateContentSource.Provider.IsInitialized = False Then
          lobjCandidateContentSource.Provider.InitializeProvider(lobjCandidateContentSource)
        End If

        ' Make sure the content source is connected.
        If lobjCandidateContentSource.Provider.IsConnected = False Then
          ' <Modified by: Ernie at 8/1/2014-4:00:47 PM on machine: ERNIE-THINK>
          ' Modified the connect call to use the content source parameter since 
          ' that is the way connect is more commonly called.
          ' This helps to ensure compatibility with older providers.
          ' lobjCandidateContentSource.Provider.Connect()
          lobjCandidateContentSource.Provider.Connect(lobjCandidateContentSource)
          ' </Modified by: Ernie at 8/1/2014-4:00:47 PM on machine: ERNIE-THINK>
        End If
      End If

      ' Try to create a repository from the content source
      lobjCandidateRepository = New Repository(lobjCandidateContentSource)

      Return lobjCandidateRepository

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
