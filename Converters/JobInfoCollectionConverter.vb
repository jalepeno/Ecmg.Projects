'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobInfoCollectionConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/9/2014 4:41:59 PM
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

  Public Class JobInfoCollectionConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IJobInfoCollection") IsNot Nothing Then
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
        Dim lobjItems As New JobInfoCollection
        Dim lobjItem As JobInfo = Nothing
        Dim lobjItemConverter As New JobInfoConverter

        While reader.Read

          Select Case reader.TokenType

            Case JsonToken.StartObject
              lobjItem = lobjItemConverter.ReadJson(reader, GetType(JobInfo), Nothing, serializer)
              lobjItems.Add(lobjItem)

            Case JsonToken.EndArray
              Exit While
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

        Dim lobjItems As IJobInfoCollection = DirectCast(value, IJobInfoCollection)

        writer.Formatting = serializer.Formatting

        writer.WriteStartArray()

        For Each lobjItem As IJobInfo In lobjItems
          writer.WriteRawValue(lobjItem.ToJson())
        Next

        writer.WriteEndArray()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace