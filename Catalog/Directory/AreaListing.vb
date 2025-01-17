'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  AreaListing.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 11:15:54 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Core
Imports Documents.Utilities

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class AreaListing
  Inherits NotifyObject
  Implements IAreaListing
  Implements IDescription

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mobjProjects As IProjectListings = New ProjectListings

#End Region

#Region "IAreaListing Implementation"

  Public ReadOnly Property Id As String Implements IAreaListing.Id
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

#Region "IDescription Implementation"

  Public Property Name As String Implements IDescription.Name, INamedItem.Name
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrName = value
        OnPropertyChanged("Name")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property
  Public Property Description As String Implements IDescription.Description
    Get
      Try
        Return mstrDescription
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrDescription = value
        OnPropertyChanged("Description")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

  Public ReadOnly Property Projects As IProjectListings Implements IAreaListing.Projects
    Get
      Try
        Return mobjProjects
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

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Name)

      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0}", Description)

      If Projects.Count = 0 Then
        lobjIdentifierBuilder.Append(" (No Projects)")
      Else
        lobjIdentifierBuilder.AppendFormat(" ({0} Projects)", Projects.Count)
      End If


      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpArea As IArea)
    Try
      mstrId = lpArea.Id
      mstrName = lpArea.Name
      mstrDescription = lpArea.Description
      mobjProjects = lpArea.Projects.ToProjectListings
      ' mobjDirectory = lpArea.Catalog

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
