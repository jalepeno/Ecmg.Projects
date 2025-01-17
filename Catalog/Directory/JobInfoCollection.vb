'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobInfoCollection.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 2:40:17 PM
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
Imports Projects.Converters

#End Region

<JsonConverter(GetType(JobInfoCollectionConverter))> _
Public Class JobInfoCollection
  Inherits CCollection(Of JobInfo)
  Implements IJobInfoCollection

#Region "Class Variables"

  Private mobjEnumerator As IEnumeratorConverter(Of IJobInfo)

#End Region

#Region "IJobInfoCollection Implementation"

  Public Shadows Function Item(id As String) As IJobInfo Implements IJobInfoCollection.Item
    Try

      Dim list As Object = From lobjJobInfo In Items Where _
        (String.Compare(lobjJobInfo.Id, id, True) = 0) Or _
        (String.Compare(lobjJobInfo.Name, id, True) = 0) Select lobjJobInfo

      For Each lobjJobInfo As IJobInfo In list
        Return lobjJobInfo
      Next

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub Add(item As IJobInfo) Implements ICollection(Of IJobInfo).Add
    Try
      Add(CType(item, JobInfo))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Sub Clear() Implements ICollection(Of IJobInfo).Clear
    Try
      MyBase.Clear()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads Function Contains(item As IJobInfo) As Boolean Implements ICollection(Of IJobInfo).Contains
    Try
      Return Contains(CType(item, JobInfo))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overloads Sub CopyTo(array() As IJobInfo, arrayIndex As Integer) Implements ICollection(Of IJobInfo).CopyTo
    Try
      MyBase.CopyTo(array, arrayIndex)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overloads ReadOnly Property Count As Integer Implements ICollection(Of IJobInfo).Count
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

  Public Overloads ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IJobInfo).IsReadOnly
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

  Public Overloads Function Remove(item As IJobInfo) As Boolean Implements ICollection(Of IJobInfo).Remove
    Try
      MyBase.Remove(CType(item, JobInfo))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shadows Sub Sort() Implements IJobInfoCollection.Sort
    Try
      MyBase.Sort()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#Region "Public Methods"

  Public Overloads Function GetEnumerator() As IEnumerator(Of IJobInfo) Implements IEnumerable(Of IJobInfo).GetEnumerator
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

  Private ReadOnly Property IPropertyEnumerator As IEnumeratorConverter(Of IJobInfo)
    Get
      Try
        If mobjEnumerator Is Nothing OrElse mobjEnumerator.Count <> Me.Count Then
          mobjEnumerator = New IEnumeratorConverter(Of IJobInfo)(Me.ToArray, GetType(JobInfo), GetType(IJobInfo))
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

#Region "Constructors"

  Public Sub New()

  End Sub

  'Public Sub New(lpJobs As Jobs)

  'End Sub

#End Region

  Public Shared Function FromJson(lpJsonString As String) As IJobInfoCollection
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(JobInfoCollection), New JobInfoCollectionConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToJson() As String
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

End Class
