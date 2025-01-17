'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  WorkSummaryConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/2/2014 10:25:48 AM
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

  Public Class WorkSummaryConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.Name.Contains("WorkSummary") Then
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

    Public Overrides Function ReadJson(reader As JsonReader, objectType As Type, existingValue As Object, serializer As JsonSerializer) As Object
      Try

        Dim lobjWorkSummary As WorkSummary = Nothing

        Dim llngNotProcessedCount As Long
        Dim llngSuccessCount As Long
        Dim llngFailedCount As Long
        Dim lintProcessingCount As Integer
        Dim llngTotalItemsCount As Long
        Dim ldblAvgProcessingTime As Double
        Dim lstrName As String = String.Empty
        Dim lstrOperation As String = String.Empty

        Dim ldatStartTime As DateTime
        Dim ldatFinishTime As DateTime
        Dim ldatLastUpdateTime As DateTime
        Dim ldblPeakProcessingRate As Double
        Dim ldblProcessingRate As Double

        Dim lstrCurrentPropertyName As String = String.Empty

        While reader.Read

          Select Case reader.TokenType
            Case JsonToken.PropertyName
              lstrCurrentPropertyName = reader.Value

              'If lstrCurrentPropertyName = "ComputerInfo" Then
              '  lobjCurrentObject = lobjComputerInfo
              'End If

            Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
              Select Case lstrCurrentPropertyName

                Case "notProcessedCount"
                  llngNotProcessedCount = reader.Value

                Case "successCount"
                  llngSuccessCount = reader.Value

                Case "failedCount"
                  llngFailedCount = reader.Value

                Case "processingCount"
                  lintProcessingCount = reader.Value

                Case "totalItemsCount"
                  llngTotalItemsCount = reader.Value

                Case "avgProcessingTime"
                  ldblAvgProcessingTime = reader.Value

                Case "name"
                  lstrName = reader.Value

                Case "operation"
                  lstrOperation = reader.Value

                Case "starttime"
                  DateTime.TryParse(reader.Value, ldatStartTime)

                Case "finishtime"
                  DateTime.TryParse(reader.Value, ldatFinishTime)

                Case "lastupdatetime"
                  DateTime.TryParse(reader.Value, ldatLastUpdateTime)

                Case "processingrate"
                  ldblProcessingRate = reader.Value

                Case "peakprocessingrate"
                  ldblPeakProcessingRate = reader.Value

              End Select

            Case JsonToken.EndObject
              Exit While

          End Select

        End While

        lobjWorkSummary = New WorkSummary(lstrName, lstrOperation, llngNotProcessedCount, _
                                          llngSuccessCount, llngFailedCount, lintProcessingCount, _
                                          llngTotalItemsCount, ldblAvgProcessingTime, ldatStartTime, _
                                          ldatFinishTime, ldatLastUpdateTime, ldblProcessingRate, ldblPeakProcessingRate)

        Return lobjWorkSummary

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Function

    Public Overrides Sub WriteJson(writer As JsonWriter, value As Object, serializer As JsonSerializer)
      Try
        'If value Is Nothing Then
        '  Throw New ArgumentNullException("value")
        'End If

        'If Helper.IsRunningInstalled Then
        '  serializer.Formatting = Formatting.None
        'Else
        '  serializer.Formatting = Formatting.Indented
        'End If

        'Dim lobjProjectDescription As IWorkSummary = DirectCast(value, IWorkSummary)

        'writer.WriteStartObject()

        'writer.WritePropertyName("id")
        'writer.WriteValue(lobjProjectDescription.Id)

        'writer.WritePropertyName("name")
        'writer.WriteValue(lobjProjectDescription.Name)

        'writer.WritePropertyName("createDate")
        'writer.WriteValue(lobjProjectDescription.CreateDate)

        'writer.WritePropertyName("itemsProcessed")
        'writer.WriteValue(lobjProjectDescription.ItemsProcessed)

        'writer.WritePropertyName("workSummary")
        ''writer.WriteValue(lobjProjectDescription.WorkSummary.t)
        'writer.WriteRaw(JsonConvert.SerializeObject(lobjProjectDescription.WorkSummary))

        'writer.WritePropertyName("location")
        '' TODO: Write location

        'writer.WritePropertyName("area")
        'writer.WriteValue(lobjProjectDescription.Area.Name)

        'writer.WriteEndObject()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace