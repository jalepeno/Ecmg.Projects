'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  WorkSummaryFirebaseConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/26/2014 2:27:42 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Globalization
Imports Documents.Utilities
Imports Newtonsoft.Json

#End Region

Namespace Converters

  Public Class WorkSummaryFirebaseConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IWorkSummary") IsNot Nothing Then
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

    Public Overrides ReadOnly Property CanRead() As Boolean
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
        Throw New NotImplementedException("This converter is write only.")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Overrides Sub WriteJson(writer As JsonWriter, value As Object, serializer As JsonSerializer)
      Try

        If value Is Nothing Then
          Throw New ArgumentNullException("value")
        End If

        If Helper.IsRunningInstalled Then
          serializer.Formatting = Formatting.None
        Else
          serializer.Formatting = Formatting.Indented
        End If

        Dim lobjWorkSummary As IWorkSummary = DirectCast(value, IWorkSummary)

        writer.WriteStartObject()

        writer.WritePropertyName("avgProcessingTime")
        writer.WriteValue(lobjWorkSummary.AvgProcessingTime)

        writer.WritePropertyName("failedCount")
        writer.WriteValue(lobjWorkSummary.FailedCount)

        writer.WritePropertyName("failurePercentage")
        writer.WriteValue(lobjWorkSummary.FailurePercentage)

        writer.WritePropertyName("notProcessedCount")
        writer.WriteValue(lobjWorkSummary.NotProcessedCount)

        writer.WritePropertyName("notProcessedPercentage")
        writer.WriteValue(lobjWorkSummary.NotProcessedPercentage)

        writer.WritePropertyName("processedCount")
        writer.WriteValue(lobjWorkSummary.ProcessedCount)

        writer.WritePropertyName("processedPercentage")
        writer.WriteValue(lobjWorkSummary.ProcessedPercentage)

        writer.WritePropertyName("progressPercentage")
        writer.WriteValue(lobjWorkSummary.ProgressPercentage)

        writer.WritePropertyName("processingCount")
        writer.WriteValue(lobjWorkSummary.ProcessingCount)

        writer.WritePropertyName("successCount")
        writer.WriteValue(lobjWorkSummary.SuccessCount)

        writer.WritePropertyName("successPercentage")
        writer.WriteValue(lobjWorkSummary.SuccessPercentage)

        writer.WritePropertyName("totalItemsCount")
        writer.WriteValue(lobjWorkSummary.TotalItemsCount)

        writer.WritePropertyName("totalProcessingTime")
        writer.WriteValue(lobjWorkSummary.TotalProcessingTime)

        writer.WritePropertyName("startTime")
        writer.WriteValue(lobjWorkSummary.StartTime.ToString("G", New CultureInfo("en-US")))

        writer.WritePropertyName("finishTime")
        writer.WriteValue(lobjWorkSummary.FinishTime.ToString("G", New CultureInfo("en-US")))

        writer.WritePropertyName("lastUpdateTime")
        writer.WriteValue(lobjWorkSummary.LastUpdateTime.ToString("G", New CultureInfo("en-US")))

        writer.WritePropertyName("processingRate")
        writer.WriteValue(lobjWorkSummary.ProcessingRate)

        writer.WritePropertyName("peakProcessingRate")
        writer.WriteValue(lobjWorkSummary.PeakProcessingRate)

        writer.WriteEndObject()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace