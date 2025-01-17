' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobSet.vb
'  Description :  [type_description_here]
'  Created     :  8/23/2012 12:57:51 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Text
Imports Documents.Utilities

#End Region

Namespace Configuration

  <DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
  Public Class JobSet
    Implements INotifyPropertyChanged

#Region "Class Variables"

    Private lobjProjectLocation As New ItemsLocation
    Private WithEvents mobjJobs As New ObservableCollection(Of String)

#End Region

#Region "Public Events"

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Friend Overridable Sub OnPropertyChanged(ByVal sProp As String)
      Try
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(sProp))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

#Region "Public Properties"

    Public Property ProjectLocation As ItemsLocation
      Get
        Try
          Return (lobjProjectLocation)
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As ItemsLocation)
        Try
          lobjProjectLocation = value
          OnPropertyChanged("ProjectLocation")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property Jobs As ObservableCollection(Of String)
      Get
        Try
          Return mobjJobs
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As ObservableCollection(Of String))
        Try
          mobjJobs = value
          OnPropertyChanged("Jobs")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

#End Region

#Region "Protected Methods"

    Protected Friend Overridable Function DebuggerIdentifier() As String
      Dim lobjIdentifierBuilder As New StringBuilder
      Try

        With lobjIdentifierBuilder
          .Append(ProjectLocation.DatabaseName)
          Select Case Jobs.Count
            Case 0
              .Append(": No Jobs")
            Case 1
              .Append(": 1 Job")
            Case Is > 1
              .AppendFormat(": {0} Jobs", Jobs.Count)
          End Select
        End With

        Return lobjIdentifierBuilder.ToString

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        Return lobjIdentifierBuilder.ToString
      End Try
    End Function

#End Region

#Region "Private Methods"

    Private Sub mobjJobs_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) Handles mobjJobs.CollectionChanged
      Try
        If Not Helper.CallStackContainsMethodName("Deserialize") Then
          OnPropertyChanged("Jobs")
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

  End Class

End Namespace