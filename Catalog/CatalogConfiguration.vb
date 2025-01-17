' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  CatalogConfiguration.vb
'  Description :  Runs Jobs using ExecutionConfiguration object.
'  Created     :  8/14/2015 10:11:20 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports Documents.Core
Imports Documents.SerializationUtilities
Imports Documents.Utilities

#End Region

Public Class CatalogConfiguration
  Implements ICatalogConfiguration
  Implements IDisplayable

#Region "Class Constants"

  Private Const DEFAULT_MAX_PROJECT_SIZE As Integer = 5000000
  Private Const DEFAULT_MAX_JOB_SIZE As Integer = 1000000

#End Region

#Region "Class Variables"

  Private mstrName As String = String.Empty
  Private mstrDisplayName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mintMaxProjectSize As Integer = DEFAULT_MAX_PROJECT_SIZE
  Private mintMaxJobSize As Integer = DEFAULT_MAX_JOB_SIZE

#End Region

#Region "Public Events"

  Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

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

#End Region

  Public Property MaxProjectSize As Integer Implements ICatalogConfiguration.MaxProjectSize
    Get
      Try
        Return mintMaxProjectSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintMaxProjectSize = value
        OnPropertyChanged("MaxProjectSize")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property MaxJobSize As Integer Implements ICatalogConfiguration.MaxJobSize
    Get
      Try
        Return mintMaxJobSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintMaxJobSize = value
        OnPropertyChanged("MaxJobSize")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#Region "Friend Methods"

  Friend Shared Function FromXmlString(lpXml As String) As ICatalogConfiguration
    Try
      Return Serializer.Deserialize.XmlString(lpXml.Replace("''", "'"), GetType(CatalogConfiguration))
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Function ToXmlString() As String Implements ICatalogConfiguration.ToSQLXmlString
    Try
      Dim lstrReturnString As String = Serializer.Serialize.XmlString(Me)
      lstrReturnString = Helper.RemoveEntriesFromString(lstrReturnString, 50,
                                                        "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""",
                                                        "xmlns:xsd=""http://www.w3.org/2001/XMLSchema""")
      Return Helper.FormatXmlString(lstrReturnString)
      'Return Serializer.Serialize.XmlString(Me)
      ' Return lstrReturnString
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Methods"

#End Region

End Class