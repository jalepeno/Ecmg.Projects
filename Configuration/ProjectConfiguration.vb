'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectConfiguration.vb
'   Description :  [type_description_here]
'   Created     :  10/10/2013 7:41:27 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports Documents.Core
Imports Documents.Utilities

#End Region

Public Class ProjectConfiguration
  Implements IDisplayable
  Implements INotifyPropertyChanged

#Region "Class Constants"

  Private Const DEFAULT_BATCH_SIZE As Integer = 1000

#End Region

#Region "Class Variables"

  Private mstrName As String = String.Empty
  Private mstrDisplayName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mobjItemsLocation As ItemsLocation
  Private mintBatchSize As Integer = DEFAULT_BATCH_SIZE

#End Region

#Region "Public Events"

  Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) _
    Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

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

#Region "IDisplayable Implementation"

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

  Public Property DisplayName As String Implements IDisplayable.DisplayName
    Get
      Try
        Return mstrDisplayName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrDisplayName = value
        OnPropertyChanged("DisplayName")
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

  Public Property ItemsLocation As ItemsLocation
    Get
      Try
        Return mobjItemsLocation
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As ItemsLocation)
      Try
        mobjItemsLocation = value
        OnPropertyChanged("ItemsLocation")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#End Region

#Region "Constructors"

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region

End Class
