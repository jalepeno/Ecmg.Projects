'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports System.Runtime.Serialization
Imports System.Text
Imports Documents.Core
Imports Documents.Utilities
Imports Microsoft.Data.SqlClient
Imports Newtonsoft.Json
Imports Projects.Converters


#End Region

<DataContract(), JsonConverter(GetType(ItemsLocationConverter))> Public Class ItemsLocation
  Inherits NotifyObject
  Implements IItemsLocation

#Region "Class Variables"

  Private menumType As ContainerType
  Private mstrLocation As String = String.Empty
  Private mstrServerName As String = String.Empty
  Private mstrDBName As String = String.Empty
  Private mstrUserName As String = String.Empty
  Private mstrPassword As String = String.Empty
  Private mstrTrustedConnection As String = "yes"
  Private ReadOnly mstrIntegratedSecurity As String = "yes"
  Private ReadOnly mstrTrustServerCertificate As String = "yes"
  Private mstrDatabasePath As String = String.Empty

#End Region

#Region "Public Properties"

  Public Property Type() As ContainerType Implements IItemsLocation.Type
    Get
      Return menumType
    End Get
    Set(ByVal value As ContainerType)
      menumType = value
    End Set
  End Property

  Public Property Location() As String Implements IItemsLocation.Location
    Get
      Return mstrLocation
    End Get
    Set(ByVal value As String)
      mstrLocation = value
      OnPropertyChanged("Location")
    End Set
  End Property

  Public Property ServerName() As String Implements IItemsLocation.ServerName
    Get
      Return mstrServerName
    End Get
    Set(ByVal value As String)
      mstrServerName = value
    End Set
  End Property

  Public Property DatabaseName() As String Implements IItemsLocation.DatabaseName
    Get
      Return mstrDBName
    End Get
    Set(ByVal value As String)
      mstrDBName = value
    End Set
  End Property

  Public Property UserName() As String Implements IItemsLocation.UserName
    Get
      Return mstrUserName
    End Get
    Set(ByVal value As String)
      mstrUserName = value
    End Set
  End Property

  Public Property Password() As String Implements IItemsLocation.Password
    Get
      Return mstrPassword
    End Get
    Set(ByVal value As String)
      mstrPassword = value
    End Set
  End Property

  Public Property TrustedConnection() As String Implements IItemsLocation.TrustedConnection
    Get
      Return mstrTrustedConnection
    End Get
    Set(ByVal value As String)
      mstrTrustedConnection = value
    End Set
  End Property

  Public Property DatabasePath() As String Implements IItemsLocation.DatabasePath
    Get
      Return mstrDatabasePath
    End Get
    Set(ByVal value As String)
      mstrDatabasePath = value
    End Set
  End Property

#End Region

#Region "Enumerations"

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(ByVal lpConnectionString As String)

    Try
      InitializeFromConnectionString(lpConnectionString)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' If the container is a csv file or access database use this constructor  
  ''' </summary>
  ''' <param name="lpType"></param>
  ''' <param name="lpFullPath"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal lpType As ContainerType,
                 ByVal lpFullPath As String)

    Try
      menumType = lpType
      mstrDatabasePath = IO.Path.GetDirectoryName(lpFullPath)
      mstrDBName = IO.Path.GetFileNameWithoutExtension(lpFullPath)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' If the container is a csv file or access database use this constructor  
  ''' </summary>
  ''' <param name="lpType"></param>
  ''' <param name="lpDatabasePath"></param>
  ''' <param name="lpDatabaseName"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal lpType As ContainerType,
                 ByVal lpDatabasePath As String,
                 ByVal lpDatabaseName As String)

    Try
      menumType = lpType
      mstrDatabasePath = lpDatabasePath
      mstrDBName = lpDatabaseName

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' For database type containers use this constructor
  ''' </summary>
  ''' <param name="lpType"></param>
  ''' <param name="lpServerName"></param>
  ''' <param name="lpDatabaseName"></param>
  ''' <param name="lpTrustedConnection"></param>
  ''' <param name="lpUserName"></param>
  ''' <param name="lpPassword"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal lpType As ContainerType,
                 ByVal lpServerName As String,
                 ByVal lpDatabaseName As String,
                 Optional ByVal lpTrustedConnection As String = "yes",
                 Optional ByVal lpUserName As String = "",
                 Optional ByVal lpPassword As String = "")

    Try
      menumType = lpType
      mstrServerName = lpServerName
      mstrDBName = lpDatabaseName
      mstrTrustedConnection = lpTrustedConnection
      mstrUserName = lpUserName
      mstrPassword = lpPassword

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Public Methods"

  Public Overrides Function ToString() As String Implements IItemsLocation.ToString

    Try
      Select Case Type
        Case ContainerType.SQLServer
          Return ToNativeConnectionString()
        Case ContainerType.OLEDB
          Return ToOleDBConnectionString()
        Case Else
          Return Me.GetType.Name
      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function ToNativeConnectionString() As String Implements IItemsLocation.ToNativeConnectionString
    Try

      Dim lobjConnectionStringBuilder As New SqlConnectionStringBuilder
      With lobjConnectionStringBuilder
        .Add("Server", ServerName)
        If Not String.IsNullOrEmpty(DatabaseName) Then
          .Add("Database", DatabaseName)
        End If
        If Not String.IsNullOrEmpty(TrustedConnection) AndAlso (TrustedConnection.Equals("true", StringComparison.CurrentCultureIgnoreCase) OrElse TrustedConnection.Equals("yes", StringComparison.CurrentCultureIgnoreCase)) Then
          .Add("Trusted_Connection", "True")
        Else
          .Add("User Id", UserName)
          .Add("Password", Password)
        End If
      End With

      Return lobjConnectionStringBuilder.ConnectionString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToOleDBConnectionString() As String Implements IItemsLocation.ToOleDBConnectionString
    Try
      ' "Provider=SQLOLEDB.1;Password=beacon;Persist Security Info=True;User ID=jm_test_user;Initial Catalog=jmTestDB;Data Source=ernie-think"

      'Dim lobjOleReader As OleDbDataReader = OleDb.OleDbEnumerator.GetRootEnumerator()
      'Dim lobjOleDbProviderNames As New List(Of String)
      'Dim lstrProviderName As String = String.Empty
      'While lobjOleReader.Read()
      '  'Console.WriteLine(String.Format("{0}", lobjOleReader(0)))
      '  lstrProviderName = lobjOleReader(0)
      '  If Not lobjOleDbProviderNames.Contains(lstrProviderName)
      '    lobjOleDbProviderNames.Add(lstrProviderName)
      '  End If
      'End While
      'lobjOleDbProviderNames.Sort()

      Dim lobjConnectionStringBuilder As New OleDb.OleDbConnectionStringBuilder

      With lobjConnectionStringBuilder

        '.Provider = "SQLNCLI10"
        .Provider = "SQLOLEDB"

        If ServerName.ToLower.Contains("localhost") Then
          ' .DataSource = Environment.MachineName
          .Add("Server", ServerName.ToLower.Replace("localhost", Environment.MachineName))
        Else
          ' .DataSource = ServerName
          .Add("Server", ServerName)
        End If

        ' .Add("Server", ServerName)
        If Not String.IsNullOrEmpty(TrustedConnection) AndAlso (TrustedConnection.Equals("true", StringComparison.CurrentCultureIgnoreCase) OrElse TrustedConnection.Equals("yes", StringComparison.CurrentCultureIgnoreCase)) Then
          .Add("Trusted_Connection", "yes")
        Else
          ' .Add("Persist Security Info", "True")
          .Add("User Id", UserName)
          .Add("Password", Password)
        End If
        If Not String.IsNullOrEmpty(DatabaseName) Then
          .Add("Database", DatabaseName)
        End If
      End With

      Return lobjConnectionStringBuilder.ConnectionString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Private Methods"

  Private Function GenerateConnectionString() As String

    Try

      Dim lobjStringBuilder As New StringBuilder

      With lobjStringBuilder
        .AppendFormat("Type={0};", Type)
        .AppendFormat("Location={0};", Location)
        .AppendFormat("ServerName={0};", ServerName)
        .AppendFormat("DBName={0};", DatabaseName)
        .AppendFormat("UserName={0};", UserName)
        .AppendFormat("Password={0};", Password)
        .AppendFormat("TrustedConnection={0};", TrustedConnection)
        .AppendFormat("DatabasePath={0}", DatabasePath)
      End With

      Return lobjStringBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Sub InitializeFromConnectionString(ByVal lpConnectionString As String)

    Try

      Dim lstrLocationParts As String() = lpConnectionString.Split(";")
      Dim lstrLocationPartComponents As String()

      For lintLocationPartIndex As Integer = 0 To lstrLocationParts.Length - 1
        lstrLocationPartComponents = lstrLocationParts(lintLocationPartIndex).Split("=")

        Select Case lstrLocationPartComponents(0)

          Case "Type"
            Type = [Enum].Parse(Type.GetType, lstrLocationPartComponents(1))

          Case "Location"
            Location = lstrLocationPartComponents(1)

          Case "ServerName", "Data Source", "DataSource"
            ServerName = lstrLocationPartComponents(1)

          Case "DBName", "Initial Catalog"
            DatabaseName = lstrLocationPartComponents(1)

          Case "UserName", "User ID"
            UserName = lstrLocationPartComponents(1)

          Case "Password"
            Password = lstrLocationPartComponents(1)

          Case "TrustedConnection"
            TrustedConnection = lstrLocationPartComponents(1)

          Case "Integrated Security"
            Dim lblnIntegratedSecurity As Boolean
            If (Not String.IsNullOrEmpty(lstrLocationPartComponents(1)) AndAlso Boolean.TryParse(lstrLocationPartComponents(1), lblnIntegratedSecurity)) Then
              If lblnIntegratedSecurity.ToString.Equals("true", StringComparison.CurrentCultureIgnoreCase) Then
                TrustedConnection = "yes"
              ElseIf lblnIntegratedSecurity.ToString.Equals("false", StringComparison.CurrentCultureIgnoreCase) Then
                TrustedConnection = "no"
              End If
            End If

          Case "DatabasePath"
            DatabasePath = lstrLocationPartComponents(1)
        End Select

      Next

      If lpConnectionString.ToLower.Contains("provider") = False Then
        Type = ContainerType.SQLServer
      End If

      If (Not String.IsNullOrEmpty(UserName) AndAlso Not String.IsNullOrEmpty(Password)) Then
        Me.TrustedConnection = "no"
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

End Class
