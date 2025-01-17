' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  ExecutionConfiguration.vb
'  Description :  [type_description_here]
'  Created     :  8/23/2012 12:55:17 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.Text
Imports Documents.SerializationUtilities
Imports Documents.Utilities

#End Region

Namespace Configuration

  <DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
  Public Class ExecutionConfiguration
    Implements INotifyPropertyChanged

#Region "Class Variables"

    Private WithEvents mobjJobSets As New JobSets

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

    Public Property JobSets As JobSets
      Get
        Try
          Return mobjJobSets
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As JobSets)
        Try
          mobjJobSets = value
          OnPropertyChanged("JobSets")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

#End Region

#Region "Public Methods"

    Public Sub Save(lpFilePath As String)
      Try
        Serializer.Serialize.XmlFile(Me, lpFilePath)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

    Public Shared Function Open(lpFilePath As String) As ExecutionConfiguration
      Try
        Return Serializer.Deserialize.XmlFile(lpFilePath, GetType(ExecutionConfiguration))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Shared Function OpenFromXml(lpXml As String) As ExecutionConfiguration
      Try
        Return Serializer.Deserialize.XmlString(lpXml, GetType(ExecutionConfiguration))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

#End Region

#Region "Protected Methods"

    Protected Friend Overridable Function DebuggerIdentifier() As String
      Dim lobjIdentifierBuilder As New StringBuilder
      Try

        With lobjIdentifierBuilder
          '.Append(ProjectLocation.DatabaseName)
          Select Case JobSets.Count
            Case 0
              .Append("No JobSets defined")
            Case 1
              .AppendFormat("1 JobSet: {0}", JobSets.First.DebuggerIdentifier)
            Case Is > 1
              .AppendFormat("{0} JobSets: 1st - {1}", JobSets.Count, JobSets.First.DebuggerIdentifier)
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

    Private Sub mobjJobSets_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) Handles mobjJobSets.CollectionChanged
      Try
        OnPropertyChanged("JobSets")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

  End Class

End Namespace