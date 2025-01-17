'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  BatchInfo.vb
'   Description :  [type_description_here]
'   Created     :  2/5/2014 7:40:14 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities
Imports Newtonsoft.Json

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class BatchInfo
  Implements IBatchInfo
  Implements IComparable

#Region "Class Variables"

  Private mdatCreateDate As DateTime
  Private mstrId As String = String.Empty
  Private mstrName As String = String.Empty
  Private mobjParentJob As IJobInfo = Nothing
  Private mobjWorkSummary As IWorkSummary = Nothing

#End Region

#Region "IBatchInfo Implementation"

  Public ReadOnly Property Id As String Implements IBatchInfo.Id
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

  Public ReadOnly Property Name As String Implements IBatchInfo.Name
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property CreateDate As Date Implements IBatchInfo.CreateDate
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

  <JsonIgnore()> _
  Public ReadOnly Property ParentJob As IJobInfo Implements IBatchInfo.ParentJob
    Get
      Try
        Return mobjParentJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property WorkSummary As IWorkSummary Implements IBatchInfo.WorkSummary
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

#End Region

#Region "Constructors"

  Public Sub New(lpId As String, _
                 lpName As String, _
                 lpCreateDate As DateTime, _
                 lpParentJob As IJobInfo, _
                 lpWorkSummary As IWorkSummary)
    Try
      mstrId = lpId
      mstrName = lpName
      mdatCreateDate = lpCreateDate
      mobjParentJob = lpParentJob
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

      If WorkSummary IsNot Nothing Then
        If String.IsNullOrEmpty(WorkSummary.Name) Then
          If Not String.IsNullOrEmpty(Name) Then
            lobjIdentifierBuilder.AppendFormat("{0}: ", Name)
          Else
            lobjIdentifierBuilder.Append("Name not set: ")
          End If
        End If
        lobjIdentifierBuilder.Append(WorkSummary.ToString)
      Else
        lobjIdentifierBuilder.Append("<not initialized>")
      End If

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
      Return Name.CompareTo(obj.Name)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region
End Class
