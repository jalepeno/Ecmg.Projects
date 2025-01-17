'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobInfoFirebaseConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/26/2014 10:00:42 AM
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

  Public Class JobInfoFirebaseConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IJobInfo") IsNot Nothing Then
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

        Dim lobjJob As IJobInfo = DirectCast(value, IJobInfo)

        writer.WriteStartObject()

        writer.WritePropertyName("id")
        writer.WriteValue(lobjJob.Id)

        writer.WritePropertyName("createDate")
        writer.WriteValue(lobjJob.CreateDate)

        writer.WritePropertyName("isCompleted")
        writer.WriteValue(lobjJob.IsCompleted)

        writer.WritePropertyName("isInitialized")
        writer.WriteValue(lobjJob.IsInitialized)

        writer.WritePropertyName("isRunning")
        writer.WriteValue(lobjJob.IsRunning)

        writer.WritePropertyName("itemsProcessed")
        writer.WriteValue(lobjJob.ItemsProcessed)

        writer.WritePropertyName("operation")
        writer.WriteValue(lobjJob.Operation)

        writer.WritePropertyName("workSummary")
        If Not lobjJob.WorkSummary Is Nothing Then
          writer.WriteRawValue(JsonConvert.SerializeObject(lobjJob.WorkSummary, serializer.Formatting, New WorkSummaryFirebaseConverter()))
        Else
          writer.WriteRawValue(JsonConvert.SerializeObject(New WorkSummary, serializer.Formatting, New WorkSummaryFirebaseConverter()))
        End If


        writer.WriteEndObject()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Sub

  End Class

End Namespace