﻿' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  JobConfigurationJsonConverter.vb
'  Description :  [type_description_here]
'  Created     :  01/23/2024 9:52:13 PM
'  <copyright company="Conteage">
'      Copyright (c) Conteage Corp. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Projects.Configuration

#End Region

Public Class JobConfigurationJsonConverter
  Inherits JsonConverter

  Public Overrides Sub WriteJson(writer As JsonWriter, value As Object, serializer As JsonSerializer)
    Try
      Dim lobjConfiguration As JobConfiguration = DirectCast(value, JobConfiguration)

      With writer

        If Helper.IsRunningInstalled Then
          .Formatting = Formatting.None
        Else
          .Formatting = Formatting.Indented
        End If

        .WriteStartObject()

        ' Write the Operation Type
        .WritePropertyName("jobconfiguration")
        .WriteStartObject()

        ' Write the 'Name' property
        .WritePropertyName("name")
        .WriteValue(lobjConfiguration.Name)

        ' Write the 'DisplayName' property
        .WritePropertyName("displayname")
        .WriteValue(lobjConfiguration.DisplayName)

        ' Write the 'Description' property
        .WritePropertyName("description")
        .WriteValue(lobjConfiguration.Description)

        '' Write the 'LogResult' property
        '.WritePropertyName("logresult")
        '.WriteValue(lobjConfiguration.LogResult)

        '' Write the 'Locale' property
        '.WritePropertyName("locale")
        '.WriteValue(lobjConfiguration.Locale.ToString())

        '' Write the 'Parameters' collection
        '.WritePropertyName("parameters")
        '.WriteStartArray()
        'For Each lobjParameter As Parameter In lobjConfiguration.Parameters
        '  .WriteRawValue(lobjParameter.ToJson())
        'Next
        '.WriteEndArray()

        '' Write the 'Operations' collection
        '.WritePropertyName("operations")
        '.WriteStartArray()
        'For Each lobjOperation As IOperation In lobjConfiguration.Operations
        '  .WriteRawValue(lobjOperation.ToJson())
        'Next
        '.WriteEndArray()

        .WriteEndObject()
        .WriteEndObject()

      End With


    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function ReadJson(reader As JsonReader, objectType As Type, existingValue As Object, serializer As JsonSerializer) As Object
    Try

      Dim lstrCurrentPropertyName As String = String.Empty
      Dim lstrName As String = String.Empty
      Dim lstrDescription As String = String.Empty
      'Dim lblnLogResult As Boolean = False
      'Dim lstrLocale As String = String.Empty
      'Dim lobjOperations As New Operations
      'Dim lobjParameters As New Parameters
      'Dim lobjProcess As IProcess

      While reader.Read
        Select Case reader.TokenType
          Case JsonToken.PropertyName
            lstrCurrentPropertyName = reader.Value

          Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
            Select Case lstrCurrentPropertyName
              Case "name"
                lstrName = reader.Value
              Case "description"
                lstrDescription = reader.Value
                'Case "logresult"
                '  lblnLogResult = reader.Value
                'Case "locale"
                '  lstrLocale = reader.Value
                'Case "parameters"
                '  lstrCurrentPropertyName = reader.Value
                'Case "operations"
                '  lstrCurrentPropertyName = reader.Value
            End Select

          Case JsonToken.StartObject
            'Select Case lstrCurrentPropertyName
            '  Case "parameters"
            '    lobjParameters.Add(Parameter.CreateFromJsonReader(reader))
            '  Case "operations"
            '    lobjOperations.Add(Operation.CreateFromJsonReader(reader))
            'End Select

        End Select
      End While

      'lobjProcess = New Process(lstrName, lstrDescription, CultureInfo.CreateSpecificCulture(lstrLocale))
      'With lobjProcess
      '  .LogResult = lblnLogResult
      '  .Parameters.AddRange(lobjParameters)
      '  .Operations.AddRange(lobjOperations)
      'End With

      'Return lobjProcess

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function CanConvert(objectType As Type) As Boolean
    Return objectType = GetType(Process)
  End Function

End Class
