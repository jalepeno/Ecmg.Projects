'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectListing.vb
'   Description :  [type_description_here]
'   Created     :  12/30/2013 10:30:52 AM
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
Public Class ProjectListing
  Inherits NotifyObject
  Implements IProjectListing
  Implements IComparable

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrArea As String = String.Empty
  Private mstrName As String = String.Empty
  Private mdatCreateDate As DateTime
  Private mlngItemsProcessed As Long
  Private mobjWorkSummary As IWorkSummary = Nothing
  Private mobjArea As IAreaListing = Nothing

#End Region

#Region "Public Properties"

  Public Property Id As String Implements IProjectListing.Id
    Get
      Try
        Return mstrId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrId = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Area As String Implements IProjectListing.Area
    Get
      Try
        Return mstrArea
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrArea = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Name As String Implements IProjectListing.Name
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

  Public ReadOnly Property CreateDate As Date Implements IProjectListing.CreateDate
    Get
      Try
        Return mdatCreateDate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property ItemsProcessed As Long Implements IProjectListing.ItemsProcessed
    Get
      Try
        Return mlngItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Long)
      Try
        mlngItemsProcessed = value
        OnPropertyChanged("ItemsProcessed")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property WorkSummary As IWorkSummary Implements IProjectListing.WorkSummary
    Get
      Try
        Return mobjWorkSummary
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Public Property WorkSummary As WorkSummary
  '  Get
  '    Try
  '      Return mobjWorkSummary
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As WorkSummary)
  '    Try
  '      mobjWorkSummary = value
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpProjectListing As IProjectListing)
    Try
      Me.Id = lpProjectListing.Id
      Me.Name = lpProjectListing.Name
      Me.Area = lpProjectListing.Area
      mdatCreateDate = lpProjectListing.CreateDate
      Me.ItemsProcessed = lpProjectListing.ItemsProcessed
      mobjWorkSummary = lpProjectListing.WorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpProjectDescription As IProjectDescription)
    Try
      Me.Id = lpProjectDescription.Id
      Me.Name = lpProjectDescription.Name
      If lpProjectDescription.Area IsNot Nothing Then
        Me.Area = lpProjectDescription.Area.Name
      End If
      mdatCreateDate = lpProjectDescription.CreateDate
      Me.ItemsProcessed = lpProjectDescription.ItemsProcessed
      mobjWorkSummary = lpProjectDescription.WorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpId As String, _
               lpName As String, _
               lpArea As String, _
               lpCreateDate As DateTime, _
               lpItemsProcessed As Long, _
               lpWorkSummary As IWorkSummary)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrArea = lpArea
      mdatCreateDate = lpCreateDate
      mlngItemsProcessed = lpItemsProcessed
      mobjWorkSummary = lpWorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      lobjIdentifierBuilder.AppendFormat("{0}: {1} ({2} Items) - {3} ", Area, Name, ItemsProcessed, Id)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "IComparable Implementation"

  Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
    Try
      'Return Name.CompareTo(obj.Name)
      If Area.CompareTo(obj.Area) = 0 Then
        Return Name.CompareTo(obj.Name)
      Else
        Return Area.CompareTo(obj.Area)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
