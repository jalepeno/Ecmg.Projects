'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports Documents.Exceptions
Imports Documents.Providers
Imports Documents.Utilities
Imports ExcelDataReader


#End Region


'<Xml.Serialization.XmlInclude(GetType(Search.StoredSearch))>
<Serializable()>
<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class JobSource
  Implements INotifyPropertyChanged

#Region "Class Variables"

  Private menumSourceType As enumSourceType
  Private menuListType As enumListType
  Private mstrFolderPath As String = String.Empty
  Private mstrSourceConnectionString As String = String.Empty
  'Private mobjSearch As Search.Search
  Private mobjSourceIdList As New List(Of String)
  Private mobjSourceIdFileNames As New List(Of String)
  Private mstrListFilePath As String = String.Empty
  Private mstrSourceDBListConnectionString As String = String.Empty
  Private mstrSourceSQLStatement As String = String.Empty
  Private mstrQueryTarget As String
  Private mstrSourceJob As String = String.Empty
  Private mstrProcessedStatusFilter As String = String.Empty
  Private menuSourceIdType As enumSourceIdType

#End Region

#Region "Public Properties"

  Public Property Type() As enumSourceType
    Get
      Return menumSourceType
    End Get
    Set(ByVal value As enumSourceType)

      Try

        If (menumSourceType <> value) Then
          menumSourceType = value
          OnPropertyChanged("Type")
        End If

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public Property SourceConnectionString() As String
    Get
      Return mstrSourceConnectionString
    End Get
    Set(ByVal value As String)

      Try
        mstrSourceConnectionString = value
        OnPropertyChanged("SourceConnectionString")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public Property SourceDBListConnectionString As String
    Get
      Try
        Return mstrSourceDBListConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrSourceDBListConnectionString = value
        OnPropertyChanged("SourceDBListConnectionString")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property SourceJob As String
    Get
      Try
        Return mstrSourceJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrSourceJob = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ProcessedStatusFilter As String
    Get
      Try
        Return mstrProcessedStatusFilter
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrProcessedStatusFilter = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property SourceIdType As enumSourceIdType
    Get
      Try
        Return menuSourceIdType
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As enumSourceIdType)
      Try
        menuSourceIdType = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property SourceSQLStatement As String
    Get
      Try
        Return mstrSourceSQLStatement
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrSourceSQLStatement = value
        OnPropertyChanged("SourceSQLStatement")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property QueryTarget As String
    Get
      Try
        Return mstrQueryTarget
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrQueryTarget = value
        OnPropertyChanged("QueryTarget")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property FolderPath() As String
    Get
      Return mstrFolderPath
    End Get
    Set(ByVal value As String)

      Try
        mstrFolderPath = value
        OnPropertyChanged("FolderPath")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  'Public Property Search() As Search.Search
  '  Get
  '    Return mobjSearch
  '  End Get
  '  Set(ByVal value As Search.Search)

  '    Try
  '      mobjSearch = value

  '      If (mobjSearch IsNot Nothing) Then

  '        'Assign the source connection string of the job from the first connection string in the search
  '        If (mobjSearch.ConnectionStrings.Count > 0) Then
  '          Me.SourceConnectionString = mobjSearch.ConnectionStrings(0)
  '        End If

  '      End If

  '      OnPropertyChanged("Search")

  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try

  '  End Set
  'End Property


  '<Obsolete("The property is deprecated, use SourceIdFileNames instead")>
  'Public Property ListFilePath As String
  '  Get
  '    Return mstrListFilePath
  '  End Get
  '  Set(value As String)

  '    Try
  '      mstrListFilePath = value

  '      Dim lstrListFileName As String = Path.GetFileName(mstrListFilePath)
  '      'If Not SourceIdFileNames.Contains(lstrListFileName) Then
  '      '  SourceIdFileNames.Add(lstrListFileName)
  '      'End If
  '      ' For now we will only support a single file in the collection until we are sure we can avoid duplicates
  '      ' We are deprecating this property in favor of SourceIdFileNames
  '      SourceIdFileNames.Clear()
  '      SourceIdFileNames.Add(lstrListFileName)
  '      OnPropertyChanged("ListFilePath")

  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try

  '  End Set
  'End Property

  Public ReadOnly Property PrimaryListFilePath As String
    Get
      Try
        Return SourceIdFileNames.FirstOrDefault
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property ListType As enumListType
    Get
      Return menuListType
    End Get
    Set(value As enumListType)

      Try

        Try
          menuListType = value
          OnPropertyChanged("ListType")

        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public Property SourceIDs As List(Of String)
    Get
      Return mobjSourceIdList
    End Get
    Set(value As List(Of String))

      Try
        mobjSourceIdList = value
        OnPropertyChanged("SourceIDs")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public Property SourceIdFileNames As List(Of String)
    Get
      Try
        Return mobjSourceIdFileNames
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As List(Of String))
      Try
        mobjSourceIdFileNames = value
        OnPropertyChanged("SourceIdFileNames")

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#Region "Enumerations"

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

#End Region

#Region "Public Methods"

#Region "Shared Methods"

  'Public Shared Function CreateSearchSource(ByVal lpSearchFile As String) As JobSource

  '  Try

  '    Dim lobjJobSource As New JobSource
  '    lobjJobSource.Type = enumSourceType.Search
  '    lobjJobSource.Search = New Search.StoredSearch(lpSearchFile)
  '    lobjJobSource.SourceConnectionString = lobjJobSource.Search.ConnectionStrings(0)
  '    Return lobjJobSource

  '  Catch ex As Exception
  '    Throw
  '  End Try

  'End Function

  Public Shared Function CreateFolderSource(ByVal lpFolderPath As String,
                                            ByVal lpConnectionString As String) As JobSource

    Try

      Dim lobjJobSource As New JobSource
      lobjJobSource.Type = enumSourceType.Folder
      lobjJobSource.SourceConnectionString = lpConnectionString
      lobjJobSource.FolderPath = lpFolderPath
      Return lobjJobSource

    Catch ex As Exception
      Throw
    End Try

  End Function

#End Region

  Public Sub ValidateList()

    Try

      If Type <> enumSourceType.List Then
        Exit Sub
      End If

      Select Case ListType

        Case enumListType.TextFile

          For Each lstrListFilePath As String In SourceIdFileNames
            If Not String.IsNullOrEmpty(lstrListFilePath) Then
              ValidateListTextFile(lstrListFilePath)
            End If
          Next

        Case enumListType.ExcelFile

          For Each lstrListFilePath As String In SourceIdFileNames
            If Not String.IsNullOrEmpty(lstrListFilePath) Then
              ValidateListExcelFile(lstrListFilePath)
            End If
          Next

        Case enumListType.DBLookup

      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' Builds the list of SourceIDs based on the list type
  ''' </summary>
  ''' <exception cref="InvalidOperationException">If the Type is not List an InvalidOperationException is thrown.</exception>
  ''' <remarks></remarks>
  Public Sub BuildList()

    Try

      If Type <> enumSourceType.List Then
        Throw New InvalidOperationException(String.Format("Unable to build list, operation is not valid for type '{0}'", Type.ToString))
      End If

      'SourceIDs = ReadList()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub SaveList()
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function ToString() As String

    Try
      Return DebuggerIdentifier()

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

      lobjIdentifierBuilder.AppendFormat("({0})", Type.ToString)

      If Not String.IsNullOrEmpty(SourceConnectionString) Then
        lobjIdentifierBuilder.AppendFormat(";SourceContentSource={0}", ContentSource.GetNameFromConnectionString(SourceConnectionString))

      Else
        lobjIdentifierBuilder.Append(";SourceContentSourceNotSet")
      End If

      Select Case Type

        'Case enumSourceType.Search

        '  If Search IsNot Nothing Then
        '    lobjIdentifierBuilder.AppendFormat(";Search={0}", Search.ToString)
        '  End If

        Case enumSourceType.Folder

          If Not String.IsNullOrEmpty(FolderPath) Then
            lobjIdentifierBuilder.AppendFormat(";FolderPath={0}", FolderPath)
          End If

        Case enumSourceType.List

          If Not String.IsNullOrEmpty(PrimaryListFilePath) Then
            lobjIdentifierBuilder.AppendFormat(";ListFile={0}", Path.GetFileName(PrimaryListFilePath))
          End If

      End Select

      If SourceIDs.Count > 0 Then
        lobjIdentifierBuilder.AppendFormat(";SourceIdCount={0}", SourceIDs.Count)
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Sub ValidateListTextFile(lpFilePath As String)

    Try

      If File.Exists(lpFilePath) = False Then
        Throw New InvalidPathException(String.Format("Unable to validate list text file, the path '{0}' is invalid.", lpFilePath), lpFilePath)
      End If

      Using sr As New StreamReader(lpFilePath)
        sr.Close()
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Private Sub ValidateListExcelFile(lpFilePath As String)

    Try

      If File.Exists(lpFilePath) = False Then
        Throw New InvalidPathException(String.Format("Unable to validate list Excel file, the path '{0}' is invalid.", lpFilePath), lpFilePath)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function ReadList() As List(Of String)

    Try

      Select Case ListType

        Case enumListType.TextFile
          Return ReadTextList()

        Case enumListType.ExcelFile
          Return ReadExcelList()

        Case enumListType.DBLookup
          Return ReadDBList()

        Case Else
          Return New List(Of String)
      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function ReadTextList() As List(Of String)

    Try

      Dim lobjReturnList As New List(Of String)

      ' Make sure the file exists
      If File.Exists(PrimaryListFilePath) = False Then
        Throw New InvalidPathException(String.Format("Unable to validate list text file, the path '{0}' is invalid.", PrimaryListFilePath), PrimaryListFilePath)
      End If

      ' Create an instance of StreamReader to read from a file.
      ' The using statement also closes the StreamReader.
      Using lobjStreamReader As New StreamReader(PrimaryListFilePath)

        Dim lstrLine As String

        ' Read and display lines from the file until the end of
        ' the file is reached.
        Do
          lstrLine = lobjStreamReader.ReadLine()

          If Not String.IsNullOrEmpty(lstrLine) Then
            lobjReturnList.Add(lstrLine)
          End If

        Loop Until lstrLine Is Nothing

      End Using

      Return lobjReturnList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function ReadExcelList() As List(Of String)

    Try

      Dim lobjReturnList As New List(Of String)

      ' Make sure the file exists
      If File.Exists(PrimaryListFilePath) = False Then
        Throw New InvalidPathException(String.Format("Unable to validate list text file, the path '{0}' is invalid.", PrimaryListFilePath), PrimaryListFilePath)
      End If

      Dim lobjExcelReader As IExcelDataReader = Nothing

      Using lobjFileStream As FileStream = File.Open(PrimaryListFilePath, FileMode.Open, FileAccess.Read)

        Select Case Path.GetExtension(PrimaryListFilePath).ToLower

          Case ".xls"
            lobjExcelReader = ExcelReaderFactory.CreateBinaryReader(lobjFileStream)

          Case ".xlsx"
            lobjExcelReader = ExcelReaderFactory.CreateOpenXmlReader(lobjFileStream)
        End Select

        If lobjExcelReader IsNot Nothing Then

          'lobjExcelReader.IsFirstRowAsColumnNames=True
          'Dim lobjDataSet As DataSet = lobjExcelReader.AsDataSet
          While lobjExcelReader.Read
            lobjReturnList.Add(lobjExcelReader.GetString(0))
          End While

          lobjExcelReader.Close()
        End If

      End Using

      Return lobjReturnList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function ReadDBList() As List(Of String)

    Try

      Dim lobjReturnList As New List(Of String)
      Throw New NotImplementedException
      Return lobjReturnList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

#End Region

#Region "NotifyObject Class"

  Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

  Friend Overridable Sub OnPropertyChanged(ByVal sProp As String)
    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(sProp))
  End Sub

#End Region

End Class
