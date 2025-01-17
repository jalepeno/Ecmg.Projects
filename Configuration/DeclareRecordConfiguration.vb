'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Providers
Imports Documents.Utilities
Imports Operations.PlugIns

#End Region

Public Class DeclareRecordConfiguration

#Region "Class Variables"

  'Connection string to the FPOS or Record that is related to the document being migrated
  Private mstrExportRecordConnectionString As String = String.Empty

  'Generated from above connection string
  Private mobjExportRecordContentSource As ContentSource = Nothing

  'Path to the declare record plug in DLL
  Private mstrDeclareRecordPlugInConfigFile As String = String.Empty

  'Object that contains the declare record plugin
  Private WithEvents mobjDeclareRecordPlugIn As CDocumentPlugIn = Nothing

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

#End Region

#Region "Public Properties"

  Public Property ExportRecordConnectionString() As String
    Get
      Return mstrExportRecordConnectionString
    End Get
    Set(ByVal value As String)
      mstrExportRecordConnectionString = value
    End Set
  End Property

  Public Property DeclareRecordPlugInConfigFile() As String
    Get
      Return mstrDeclareRecordPlugInConfigFile
    End Get
    Set(ByVal value As String)
      mstrDeclareRecordPlugInConfigFile = value
    End Set
  End Property

  'Public ReadOnly Property DeclareRecordPlugIn() As CDocumentPlugIn
  '  Get

  '    If (mobjDeclareRecordPlugIn Is Nothing) Then

  '      Try
  '        mobjDeclareRecordPlugIn = CPlugIn.Create(PlugInConfiguration.Deserialize(mstrDeclareRecordPlugInConfigFile))

  '      Catch ex As Exception
  '        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      End Try

  '    End If

  '    Return mobjDeclareRecordPlugIn
  '  End Get
  'End Property

  Public ReadOnly Property ExportRecordContentSource() As ContentSource
    Get

      If (mobjExportRecordContentSource Is Nothing) Then

        Try
          mobjExportRecordContentSource = New ContentSource(mstrExportRecordConnectionString)

        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        End Try

      End If

      Return mobjExportRecordContentSource
    End Get
  End Property

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region

End Class
