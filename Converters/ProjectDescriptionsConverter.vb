'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectDescriptionsConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/9/2014 8:41:35 AM
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

  Public Class ProjectDescriptionsConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.Name.Contains("ProjectDescriptions") Then
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
        Dim lobjItems As New ProjectDescriptions
        Dim lobjItem As ProjectDescription = Nothing
        Dim lobjItemConverter As New ProjectDescriptionConverter

        While reader.Read

          Select Case reader.TokenType

            Case JsonToken.StartObject
              lobjItem = lobjItemConverter.ReadJson(reader, GetType(ProjectDescription), Nothing, serializer)
              lobjItems.Add(lobjItem)

            Case JsonToken.EndObject

          End Select
        End While

        Return lobjItems

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

        Dim lobjProjectDescriptions As IProjectDescriptions = DirectCast(value, IProjectDescriptions)

        writer.Formatting = serializer.Formatting

        'writer.WriteStartObject()
        'writer.WritePropertyName("projectDescriptions")
        writer.WriteStartArray()

        'For lintProjectCounter As Integer = 0 To lobjProjectDescriptions.Count - 1
        '  writer.WriteRawValue(lobjProjectDescriptions(lintProjectCounter).ToJson())
        '  If lintProjectCounter < lobjProjectDescriptions.Count - 1 Then
        '    writer.
        '  End If
        'Next

        For Each lobjProjectDescription As IProjectDescription In lobjProjectDescriptions
          writer.WriteRawValue(lobjProjectDescription.ToJson())
        Next

        writer.WriteEndArray()
        'writer.WriteEndObject()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace