'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  NodeInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  1/8/2014 8:11:22 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Core
Imports Documents.Utilities
Imports Newtonsoft.Json

#End Region

Public Class NodeInfoCollection
  Inherits CCollection(Of NodeInfo)
  Implements INodeInfoCollection

#Region "Class Variables"

  Private mobjEnumerator As IEnumeratorConverter(Of INodeInfo)

#End Region

#Region "INodeInfoCollection Implementation"

  Public Shadows Sub Sort() Implements INodeInfoCollection.Sort
    Try
      MyBase.Sort()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Add(item As INodeInfo) Implements ICollection(Of INodeInfo).Add
    Try

      If Not Contains(item.Name) Then
        MyBase.Add(CType(item, NodeInfo))
        If Not Helper.CallStackContainsMethodName("GetCurrentCatalog", "FromJson") Then
          item.Save()
        End If
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of INodeInfo).Clear
    Try
      MyBase.Clear()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(name As String) As Boolean Implements INodeInfoCollection.Contains
    Try
      Return MyBase.Contains(name)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Function Contains(item As INodeInfo) As Boolean Implements ICollection(Of INodeInfo).Contains
    Try
      Return Contains(CType(item, JobInfo))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As INodeInfo, arrayIndex As Integer) Implements ICollection(Of INodeInfo).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Property Item(name As String) As INodeInfo Implements INodeInfoCollection.Item
    Get
      Try
        Return MyBase.GetItemByName(name)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As INodeInfo)
      Try
        MyBase.Item(name) = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of INodeInfo).Count
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

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of INodeInfo).IsReadOnly
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

  Public Overloads Function Remove(item As INodeInfo) As Boolean Implements ICollection(Of INodeInfo).Remove
    Try
      MyBase.Remove(CType(item, JobInfo))
      Return True
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToJson() As String Implements INodeInfoCollection.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None)
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function FromJson(lpJson As String) As INodeInfoCollection
    Try
      Return JsonConvert.DeserializeObject(lpJson, GetType(NodeInfoCollection))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#Region "Public Methods"

  Public Overloads Function GetEnumerator() As IEnumerator(Of INodeInfo) Implements IEnumerable(Of INodeInfo).GetEnumerator
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

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of INodeInfo)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of INodeInfo)(Me.ToArray, GetType(NodeInfo), GetType(INodeInfo))
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

#End Region

End Class
