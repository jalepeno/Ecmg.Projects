' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobConfiguration.vb
'  Description :  Object containing all of the configuration information for a job.
'  Created     :  8/23/2012 7:45:31 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.Text
Imports Documents.Core
Imports Documents.Migrations
Imports Documents.SerializationUtilities
Imports Documents.Transformations
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Operations

#End Region

Namespace Configuration

  <DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
  Public Class JobConfiguration
    Implements IDisplayable
    Implements INotifyPropertyChanged
    Implements IJsonSerializable(Of JobConfiguration)
    ' Implements IXmlSerializable
    Implements IComparable
    Implements ICloneable

#Region "Class Constants"

    Private Const DEFAULT_BATCH_SIZE As Integer = 1

#End Region

#Region "Class Variables"

    Private mstrName As String = String.Empty
    Private mstrPreviousName As String = String.Empty
    Private mstrDisplayName As String = String.Empty
    Private mstrDescription As String = String.Empty
    Private WithEvents mobjSource As New JobSource
    Private WithEvents mobjItemsLocation As New ItemsLocation
    Private mintBatchSize As Integer = DEFAULT_BATCH_SIZE
    Private mstrOperationName As String = "Export"
    Private mstrDestinationConnectionString As String = String.Empty
    Private menuStorageType As Content.StorageTypeEnum = Content.StorageTypeEnum.Reference
    Private mblnDeclareAsRecordOnImport As Boolean = False
    Private mobjDeclareRecordConfiguration As New DeclareRecordConfiguration
    Private mobjTransformations As New TransformationCollection
    Private mstrTransformationSourcePath As String = String.Empty
    Private menuFilingMode As FilingMode = FilingMode.BaseFolderPathOnly
    Private mblnLeadingDelimiter As Boolean = True
    Private mstrFolderDelimiter As String = "/"
    Private menuBasePathLocation As ePathLocation = ePathLocation.Front
    Private mobjProcess As Process = Nothing
    'Private mobjNotificationConfiguration As New Notifications.NotificationConfiguration
    Private mstrJobId As String = String.Empty

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
          mstrPreviousName = mstrName
          mstrName = value
          OnPropertyChanged("Name")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public ReadOnly Property PreviousName As String
      Get
        Try
          Return mstrPreviousName
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
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

    Public Property Source() As JobSource
      Get
        Try
          Return mobjSource
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As JobSource)
        Try
          mobjSource = value
          OnPropertyChanged("Source")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    ''' <summary>
    ''' Tells each Batch where it's items should be stored.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ItemsLocation() As ItemsLocation
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

    Public Property BatchSize() As Integer
      Get
        Try
          Return mintBatchSize
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As Integer)
        Try
          mintBatchSize = value
          OnPropertyChanged("BatchSize")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property OperationName() As String
      Get
        Try
          Return mstrOperationName
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As String)
        Try
          mstrOperationName = value
          OnPropertyChanged("OperationName")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property DestinationConnectionString() As String
      Get
        Try
          Return mstrDestinationConnectionString
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As String)
        Try
          mstrDestinationConnectionString = value
          OnPropertyChanged("DestinationConnectionString")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property ContentStorageType() As Content.StorageTypeEnum
      Get
        Try
          Return menuStorageType
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As Content.StorageTypeEnum)
        Try
          menuStorageType = value
          OnPropertyChanged("ContentStorageType")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property DeclareAsRecordOnImport() As Boolean
      Get
        Try
          Return mblnDeclareAsRecordOnImport
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As Boolean)
        Try
          mblnDeclareAsRecordOnImport = value
          OnPropertyChanged("DeclareAsRecordOnImport")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property DeclareRecordConfiguration() As DeclareRecordConfiguration
      Get
        Try
          Return mobjDeclareRecordConfiguration
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As DeclareRecordConfiguration)
        Try
          mobjDeclareRecordConfiguration = value
          OnPropertyChanged("DeclareRecordConfiguration")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property Transformations() As TransformationCollection
      Get
        Try
          Return mobjTransformations
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As TransformationCollection)
        Try
          mobjTransformations = value
          OnPropertyChanged("Transformations")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property TransformationSourcePath As String
      Get
        Try
          Return mstrTransformationSourcePath
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As String)
        Try
          mstrTransformationSourcePath = value
          OnPropertyChanged("TransformationSourcePath")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property DocumentFilingMode() As FilingMode
      Get
        Try
          Return menuFilingMode
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As FilingMode)
        Try
          menuFilingMode = value
          OnPropertyChanged("DocumentFilingMode")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property LeadingDelimiter() As Boolean
      Get
        Try
          Return mblnLeadingDelimiter
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As Boolean)
        Try
          mblnLeadingDelimiter = value
          OnPropertyChanged("LeadingDelimiter")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property BasePathLocation() As ePathLocation
      Get
        Try
          Return menuBasePathLocation
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As ePathLocation)
        Try
          menuBasePathLocation = value
          OnPropertyChanged("BasePathLocation")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property FolderDelimiter() As String
      Get
        Try
          Return mstrFolderDelimiter
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(ByVal value As String)
        Try
          mstrFolderDelimiter = value
          OnPropertyChanged("FolderDelimiter")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    Public Property Process As Process
      Get
        Try
          Return mobjProcess
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As Process)
        Try
          mobjProcess = value
          If mobjProcess IsNot Nothing Then
            If ((String.IsNullOrEmpty(Me.OperationName)) AndAlso (Not String.IsNullOrEmpty(mobjProcess.Name))) Then
              Me.OperationName = mobjProcess.Name
            End If
          End If
          OnPropertyChanged("Process")
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

    'Public Property NotificationConfiguration As Notifications.NotificationConfiguration
    '  Get
    '    Try
    '      Return mobjNotificationConfiguration
    '    Catch Ex As Exception
    '      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
    '      ' Re-throw the exception to the caller
    '      Throw
    '    End Try
    '  End Get
    '  Set(value As Notifications.NotificationConfiguration)
    '    Try
    '      mobjNotificationConfiguration = value
    '      OnPropertyChanged("NotificationConfiguration")
    '    Catch Ex As Exception
    '      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
    '      ' Re-throw the exception to the caller
    '      Throw
    '    End Try
    '  End Set
    'End Property

#End Region

#Region "Friend Properties"

    Friend Property JobId As String
      Get
        Try
          Return mstrJobId
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As String)
        Try
          mstrJobId = value
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(lpJob As Job)
      Try

        With Me
          .BasePathLocation = lpJob.BasePathLocation
          .BatchSize = lpJob.BatchSize
          .ContentStorageType = lpJob.ContentStorageType
          .DeclareAsRecordOnImport = lpJob.DeclareAsRecordOnImport
          .DeclareRecordConfiguration = lpJob.DeclareRecordConfiguration
          .Description = lpJob.Description
          .DestinationConnectionString = lpJob.DestinationConnectionString
          .DisplayName = lpJob.Name
          .DocumentFilingMode = lpJob.DocumentFilingMode
          .FolderDelimiter = lpJob.FolderDelimiter
          .ItemsLocation = lpJob.ItemsLocation
          .LeadingDelimiter = lpJob.LeadingDelimiter
          .Name = lpJob.Name
          .OperationName = lpJob.Operation
          .Process = lpJob.Process
          .Source = lpJob.Source
          .Transformations = lpJob.Transformations
          .TransformationSourcePath = lpJob.TransformationSourcePath
        End With

      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

#Region "Public Methods"

#Region "IJsonSerializable(Of JobConfiguration) Implementation"

    Public Overloads Function ToJson() As String Implements IJsonSerializable(Of JobConfiguration).ToJson
      Try
        Return WriteJsonString()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Overloads Function FromJson(lpJson As String) As JobConfiguration Implements IJsonSerializable(Of JobConfiguration).FromJson
      Try
        Return JsonConvert.DeserializeObject(lpJson, GetType(JobConfiguration), DefaultJsonSerializerSettings.Settings)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Friend Function WriteJsonString() As String
      Try
        'Return JsonConvert.SerializeObject(Me, Newtonsoft.Json.Formatting.None, New JobConfigurationJsonConverter())
        Return JsonConvert.SerializeObject(Me, Newtonsoft.Json.Formatting.None, DefaultJsonSerializerSettings.Settings)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

#End Region

    Public Function ToXmlString() As String
      Try
        Dim lstrReturnString As String = Serializer.Serialize.XmlString(Me)
        lstrReturnString = Helper.RemoveEntriesFromString(lstrReturnString, 500, _
                                                          "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", _
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

    Public Function ToXmlElementString() As String
      Try
        Return Serializer.Serialize.XmlElementString(Me)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    ''' <summary>
    '''     Used to create an XML string that is safe for SQL INSERT and UPDATE statements.
    ''' </summary>
    ''' <returns>
    '''     An XML string with escaped single quotes.
    ''' </returns>
    Public Function ToSQLXmlString() As String
      Try
        ' We want to replace any single quotes with escaped single quotes for putting the xml into a SQL statement.
        Return Me.ToXmlString.Replace("'", "''")
        'Return Me.ToXmlString.Replace("'", "@apos;")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Shared Function FromXmlFile(lpFilePath As String) As JobConfiguration
      Try
        Return Serializer.Deserialize.XmlFile(lpFilePath, GetType(JobConfiguration))
      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Shared Function FromXmlString(lpXml As String) As JobConfiguration
      Try
        Return Serializer.Deserialize.XmlString(lpXml.Replace("''", "'"), GetType(JobConfiguration))
      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
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
          .Append(Name)
          If Not String.IsNullOrEmpty(OperationName) Then
            .AppendFormat(": {0}", OperationName)
          Else
            .Append(": Operation not defined")
          End If
        End With

        Return lobjIdentifierBuilder.ToString

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        Return lobjIdentifierBuilder.ToString
      End Try
    End Function

#End Region

    '#Region "IXmlSerializable Implementation"

    '  Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
    '    Try
    '      ' As per the Microsoft guidelines this is not implemented
    '      Return Nothing
    '    Catch ex As Exception
    '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '      ' Re-throw the exception to the caller
    '      Throw
    '    End Try
    '  End Function

    '  Public Sub ReadXml(reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml

    '    Dim lobjXmlDocument As New XmlDocument
    '    Dim lobjAttribute As XmlAttribute = Nothing
    '    Dim lobjCandidateNode As XmlNode = Nothing

    '    Try

    '      lobjXmlDocument.Load(reader)

    '      With lobjXmlDocument

    '        Name = .DocumentElement.GetAttribute("Name")
    '        DisplayName = .DocumentElement.GetAttribute("DisplayName")
    '        Description = .DocumentElement.GetAttribute("Description")
    '        BatchSize = Integer.Parse(.DocumentElement.GetAttribute("BatchSize"))
    '        OperationName = .DocumentElement.GetAttribute("Operation")
    '        FolderDelimiter = .DocumentElement.GetAttribute("FolderDelimiter")

    '        lobjCandidateNode = .DocumentElement.SelectSingleNode("JobSource")
    '        If ((lobjCandidateNode IsNot Nothing) AndAlso (Not String.IsNullOrEmpty(lobjCandidateNode.OuterXml))) Then
    '          Source = Serializer.Deserialize.XmlString(lobjCandidateNode.OuterXml, GetType(JobSource))
    '        End If

    '        lobjCandidateNode = .DocumentElement.SelectSingleNode("ItemsLocation")
    '        If ((lobjCandidateNode IsNot Nothing) AndAlso (Not String.IsNullOrEmpty(lobjCandidateNode.OuterXml))) Then
    '          ItemsLocation = Serializer.Deserialize.XmlString(lobjCandidateNode.OuterXml, GetType(ItemsLocation))
    '        End If

    '        DestinationConnectionString = Helper.GetElementValue(.DocumentElement, "DestinationConnectionString")

    '        Dim lstrContentStorageType As String = Helper.GetElementValue(.DocumentElement, "ContentStorageType")
    '        If Not String.IsNullOrEmpty(lstrContentStorageType) Then
    '          ContentStorageType = System.Enum.Parse(GetType(Content.StorageTypeEnum), lstrContentStorageType)
    '        End If

    '        Dim lstrDeclareAsRecordOnImport As String = Helper.GetElementValue(.DocumentElement, "DeclareAsRecordOnImport")
    '        If Not String.IsNullOrEmpty(lstrDeclareAsRecordOnImport) Then
    '          DeclareAsRecordOnImport = Boolean.Parse(lstrDeclareAsRecordOnImport)
    '        Else
    '          DeclareAsRecordOnImport = False
    '        End If

    '        lobjCandidateNode = .DocumentElement.SelectSingleNode("DeclareRecordConfiguration")
    '        If ((lobjCandidateNode IsNot Nothing) AndAlso (Not String.IsNullOrEmpty(lobjCandidateNode.OuterXml))) Then
    '          DeclareRecordConfiguration = Serializer.Deserialize.XmlString(lobjCandidateNode.OuterXml, GetType(DeclareRecordConfiguration))
    '        End If

    '        lobjCandidateNode = .DocumentElement.SelectSingleNode("Transformations")
    '        If ((lobjCandidateNode IsNot Nothing) AndAlso (Not String.IsNullOrEmpty(lobjCandidateNode.OuterXml))) Then
    '          Transformations = Serializer.Deserialize.XmlString(lobjCandidateNode.InnerXml, GetType(TransformationCollection))
    '        End If

    '        TransformationSourcePath = Helper.GetElementValue(.DocumentElement, "TransformationSourcePath")

    '        DocumentFilingMode = System.Enum.Parse(GetType(FilingMode), .GetElementById("DocumentFilingMode").Value)
    '        LeadingDelimiter = Boolean.Parse(.GetElementById("LeadingDelimiter").Value)
    '        BasePathLocation = System.Enum.Parse(GetType(ePathLocation), .GetElementById("BasePathLocation").Value)
    '        Process = Serializer.Deserialize.XmlString(.GetElementById("Process").OuterXml, GetType(Process))
    '      End With

    '    Catch ex As Exception
    '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '      ' Re-throw the exception to the caller
    '      Throw
    '    End Try
    '  End Sub

    '  Public Sub WriteXml(writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml
    '    Try

    '      With writer

    '        '.WriteStartElement("root")
    '        '.WriteAttributeString("xmlns", "x", Nothing, "urn:1")
    '        '.WriteStartElement("item", "urn:1")
    '        '.WriteEndElement()
    '        '.WriteStartElement("item", "urn:1")
    '        '.WriteEndElement()

    '        .WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
    '        .WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
    '        .WriteAttributeString("Name", Name)
    '        .WriteAttributeString("DisplayName", DisplayName)
    '        .WriteAttributeString("Description", Description)
    '        .WriteAttributeString("BatchSize", BatchSize.ToString)
    '        .WriteAttributeString("Operation", OperationName)
    '        .WriteAttributeString("FolderDelimiter", FolderDelimiter)
    '        .WriteRaw(Serializer.Serialize.XmlElementString(Source))
    '        .WriteRaw(Serializer.Serialize.XmlElementString(ItemsLocation))
    '        .WriteElementString("DestinationConnectionString", DestinationConnectionString)
    '        .WriteElementString("ContentStorageType", ContentStorageType.ToString)
    '        .WriteElementString("DeclareAsRecordOnImport", DeclareAsRecordOnImport.ToString)
    '        .WriteRaw(Serializer.Serialize.XmlElementString(DeclareRecordConfiguration))
    '        .WriteStartElement("Transformations")
    '        .WriteRaw(Serializer.Serialize.XmlElementString(Transformations))
    '        .WriteEndElement()
    '        .WriteElementString("TransformationSourcePath", TransformationSourcePath)
    '        .WriteElementString("DocumentFilingMode", DocumentFilingMode.ToString)
    '        .WriteElementString("LeadingDelimiter", LeadingDelimiter.ToString)
    '        .WriteElementString("BasePathLocation", BasePathLocation.ToString)
    '        .WriteRaw(Serializer.Serialize.XmlElementString(Process))

    '        '.WriteEndElement()

    '      End With

    '    Catch ex As Exception
    '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '      ' Re-throw the exception to the caller
    '      Throw
    '    End Try
    '  End Sub

    '#End Region

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

#Region "ICloneable Implementation"

    Public Function Clone() As Object Implements ICloneable.Clone
      Try
        Return JobConfiguration.FromXmlString(Me.ToXmlString)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Private Sub mobjSource_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles mobjSource.PropertyChanged
      Try
        If Not Helper.CallStackContainsMethodName("nvpAnyListFile_FileSelected") Then
          OnPropertyChanged(String.Format("Source.{0}", e.PropertyName))
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

    Private Sub mobjItemsLocation_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles mobjItemsLocation.PropertyChanged
      Try
        OnPropertyChanged("ItemsLocation")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

  End Class

End Namespace