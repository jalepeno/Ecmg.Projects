'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectInfoFirebaseConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/26/2014 9:50:10 AM
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

  Friend Class ProjectInfoFirebaseConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IProjectInfo") IsNot Nothing Then
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

        Dim lobjProject As IProjectInfo = DirectCast(value, IProjectInfo)

        writer.WriteStartObject()

        writer.WritePropertyName("id")
        writer.WriteValue(lobjProject.Id)

        writer.WritePropertyName("name")
        writer.WriteValue(lobjProject.Name)

        writer.WritePropertyName("createDate")
        writer.WriteValue(lobjProject.CreateDate)

        writer.WritePropertyName("isCompleted")
        writer.WriteValue(lobjProject.IsCompleted)

        'writer.WritePropertyName("isInitialized")
        'writer.WriteValue(lobjProject.IsInitialized)

        writer.WritePropertyName("itemsProcessed")
        writer.WriteValue(lobjProject.ItemsProcessed)

        writer.WritePropertyName("workSummary")
        writer.WriteRawValue(JsonConvert.SerializeObject(lobjProject.WorkSummary, serializer.Formatting, New WorkSummaryFirebaseConverter()))

        'writer.WritePropertyName("itemsProcessed")
        'writer.WriteValue(lobjProject.ItemsProcessed)

        'writer.WritePropertyName("jobs")
        'For Each lobjJobInfo As IJobInfo In lobjProject.Jobs
        '  writer.WriteStartObject()
        '  writer.WritePropertyName(lobjJobInfo.Name)
        '  writer.WriteRawValue(lobjJobInfo.ToJson())
        '  ' writer.WriteEndObject()
        'Next

        'If lobjProject.Location IsNot Nothing Then
        '  writer.WritePropertyName("location")
        '  Dim lstrLocationJson As String = JsonConvert.SerializeObject(lobjProject.Location, serializer.Formatting)
        '  writer.WriteRawValue(lstrLocationJson)
        'End If

        writer.WriteEndObject()


      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace