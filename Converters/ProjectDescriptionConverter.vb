'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectDescriptionConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/9/2014 9:12:53 AM
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

  Public Class ProjectDescriptionConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IProjectDescription") IsNot Nothing Then
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

    'Public Overrides ReadOnly Property CanRead() As Boolean
    '  Get
    '    Try
    '      Return False
    '    Catch ex As Exception
    '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '      ' Re-throw the exception to the caller
    '      Throw
    '    End Try
    '  End Get
    'End Property

    Public Overrides Function ReadJson(reader As JsonReader, objectType As Type, existingValue As Object, serializer As JsonSerializer) As Object
      Try

        Dim lobjItem As ProjectDescription = Nothing
        Dim lobjWorkSummary As IWorkSummary = Nothing
        Dim lobjLocation As IItemsLocation = Nothing

        Dim lstrCreateDate As String = String.Empty
        Dim ldatCreateDate As DateTime
        Dim lstrArea As String = String.Empty
        Dim lobjArea As IArea = Nothing

        Dim lstrId As String = String.Empty
        Dim lstrName As String = String.Empty
        Dim llngItemsProcessed As Long

        Dim lstrCurrentPropertyName As String = String.Empty

        While reader.Read

          Select Case reader.TokenType
            Case JsonToken.PropertyName
              lstrCurrentPropertyName = reader.Value

            Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
              Select Case lstrCurrentPropertyName
                Case "area"
                  lstrArea = reader.Value
                  lobjArea = ProjectCatalog.Instance.GetArea(lstrArea)

                Case "createDate"
                  lstrCreateDate = reader.Value
                  ldatCreateDate = DateTime.Parse(lstrCreateDate)

                Case "id"
                  lstrId = reader.Value

                Case "name"
                  lstrName = reader.Value

                Case "itemsProcessed"
                  llngItemsProcessed = reader.Value

              End Select

            Case JsonToken.StartObject
              Select Case lstrCurrentPropertyName
                Case "workSummary"
                  Dim lobjWorkSummaryConverter As New WorkSummaryConverter
                  lobjWorkSummary = lobjWorkSummaryConverter.ReadJson(reader, GetType(WorkSummary), Nothing, serializer)

                Case "location"
                  Dim lobjItemsLocationConverter As New ItemsLocationConverter
                  lobjLocation = lobjItemsLocationConverter.ReadJson(reader, GetType(ItemsLocation), Nothing, serializer)

              End Select

            Case JsonToken.EndObject
              Exit While

          End Select
        End While

        lobjItem = New ProjectDescription(lstrId, lstrName, lobjLocation, ldatCreateDate, lobjArea, llngItemsProcessed, lobjWorkSummary)

        Return lobjItem

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

        Dim lobjProjectDescription As IProjectDescription = DirectCast(value, IProjectDescription)

        writer.WriteStartObject()

        writer.WritePropertyName("id")
        writer.WriteValue(lobjProjectDescription.Id)

        writer.WritePropertyName("name")
        writer.WriteValue(lobjProjectDescription.Name)

        writer.WritePropertyName("area")
        writer.WriteValue(lobjProjectDescription.Area.Name)

        writer.WritePropertyName("createDate")
        writer.WriteValue(lobjProjectDescription.CreateDate)

        writer.WritePropertyName("itemsProcessed")
        writer.WriteValue(lobjProjectDescription.ItemsProcessed)

        writer.WritePropertyName("workSummary")
        writer.WriteRawValue(JsonConvert.SerializeObject(lobjProjectDescription.WorkSummary, serializer.Formatting))

        If lobjProjectDescription.Location IsNot Nothing Then
          writer.WritePropertyName("location")
          Dim lstrLocationJson As String = JsonConvert.SerializeObject(lobjProjectDescription.Location, serializer.Formatting)
          writer.WriteRawValue(lstrLocationJson)
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