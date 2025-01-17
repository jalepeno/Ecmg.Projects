'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  Areas.vb
'   Description :  [type_description_here]
'   Created     :  12/12/2013 2:42:33 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities

#End Region

Public Class Areas
  Inherits CCollection(Of Area)
  Implements IAreas

#Region "Class Variables"

  Private mobjEnumerator As IEnumeratorConverter(Of IArea)
  Private mobjCatalog As IProjectCatalog = Nothing

#End Region

#Region "Constructors"

  Public Sub New(lpCatalog As IProjectCatalog)
    Try
      mobjCatalog = lpCatalog
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IAreas Implementation"

  Public ReadOnly Property Catalog As IProjectCatalog Implements IAreas.Catalog
    Get
      Try
        Return mobjCatalog
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Friend Function ToAreaListings() As IAreaListings Implements IAreas.ToAreaListings
    Try
      Return New AreaListings(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub Add(item As IArea) Implements ICollection(Of IArea).Add
    Try
      item.SetCatalog(Me.Catalog)
      Add(CType(item, Area))
      If Not Helper.CallStackContainsMethodName("GetCurrentCatalog") Then
        item.Save()
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of IArea).Clear
    Try
      MyBase.Clear()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(item As IArea) As Boolean Implements ICollection(Of IArea).Contains
    Try
      Return Contains(CType(item, Area))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As IArea, arrayIndex As Integer) Implements ICollection(Of IArea).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of IArea).Count
    Get
      Try
        Return MyBase.Count
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IArea).IsReadOnly
    Get
      Try
        Return MyBase.IsReadOnly
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overloads Function Remove(item As IArea) As Boolean Implements ICollection(Of IArea).Remove
    Try
      MyBase.Remove(CType(item, ProjectDescription))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Public Methods"

  Public Overloads Function GetEnumerator() As IEnumerator(Of IArea) Implements IEnumerable(Of IArea).GetEnumerator
    Try
      Return IPropertyEnumerator.GetEnumerator
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Properties"

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of IArea)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of IArea)(Me.ToArray, GetType(Area), GetType(IArea))
        End If
        Return mobjEnumerator
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

End Class
