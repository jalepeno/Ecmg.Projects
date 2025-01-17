'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectInfoConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/2/2014 10:05:07 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports Newtonsoft.Json

#End Region

Namespace Converters

  Public Class ProjectInfoConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.Name.Contains("ProjectInfo") Then
          Return True
        Else
          Return False
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Overrides ReadOnly Property CanWrite() As Boolean
      Get
        Try
          Return False
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Re-throw the exception to the caller
          Throw
        End Try
      End Get
    End Property

    Public Overrides Function ReadJson(reader As JsonReader, objectType As Type, existingValue As Object, serializer As JsonSerializer) As Object
      Try

        Dim lobjProjectInfo As ProjectInfo = Nothing
        'Dim lobjComputerInfo As ComputerInfo = Nothing

        'Dim lstrAddress As String = String.Empty
        Dim lstrCreateDate As String = String.Empty
        Dim ldatCreateDate As DateTime
        Dim lstrDescription As String = String.Empty
        Dim lstrId As String = String.Empty
        Dim lstrName As String = String.Empty
        Dim lstrLocation As String = String.Empty
        Dim llngItemsProcessed As Long
        Dim lobjWorkSummary As WorkSummary = Nothing
        Dim lobjJobInfo As IJobInfo = Nothing
        Dim lobjJobInfoCollection As New JobInfoCollection

        Dim lstrCurrentPropertyName As String = String.Empty

        While reader.Read

          Select Case reader.TokenType
            Case JsonToken.PropertyName
              lstrCurrentPropertyName = reader.Value.ToString.ToLower()

              'If lstrCurrentPropertyName = "ComputerInfo" Then
              '  lobjCurrentObject = lobjComputerInfo
              'End If

            Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
              Select Case lstrCurrentPropertyName

                Case "createdate"
                  lstrCreateDate = reader.Value
                  ldatCreateDate = DateTime.Parse(lstrCreateDate)

                Case "description"
                  lstrDescription = reader.Value

                Case "id"
                  lstrId = reader.Value

                Case "name"
                  lstrName = reader.Value

                Case "location"
                  lstrLocation = reader.Value


                Case "itemsprocessed"
                  llngItemsProcessed = reader.Value

              End Select

            Case JsonToken.StartObject
              If lstrCurrentPropertyName = "worksummary" Then
                Dim lobjWorkSummaryConverter As New WorkSummaryConverter
                lobjWorkSummary = lobjWorkSummaryConverter.ReadJson(reader, GetType(WorkSummary), existingValue, serializer)
              End If

            Case JsonToken.StartArray
              If lstrCurrentPropertyName = "jobs" Then
                Dim lobjJobInfoConverter As New JobInfoCollectionConverter
                lobjJobInfoCollection = lobjJobInfoConverter.ReadJson(reader, GetType(JobInfo), existingValue, serializer)
              End If
            Case JsonToken.EndArray, JsonToken.EndObject
              Exit While
          End Select
        End While

        lobjProjectInfo = New ProjectInfo(lstrId, lstrName, lstrDescription, ldatCreateDate, lstrLocation, _
                                          llngItemsProcessed, lobjWorkSummary, lobjJobInfoCollection)

        Return lobjProjectInfo

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Function

    Public Overrides Sub WriteJson(writer As JsonWriter, value As Object, serializer As JsonSerializer)
      Try
        Throw New NotImplementedException("Unnecessary because CanWrite is false.  The type will skip the converter.")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace