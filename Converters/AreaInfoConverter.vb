﻿'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  AreaInfoConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 3:05:43 PM
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

  Public Class AreaInfoConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IAreaInfo") IsNot Nothing Then
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

        Dim lobjItemInfo As AreaInfo = Nothing

        Dim lstrDescription As String = String.Empty
        Dim lstrId As String = String.Empty
        Dim lstrName As String = String.Empty
        Dim lobjProjectInfoCollection As New ProjectInfoCollection

        Dim lstrCurrentPropertyName As String = String.Empty

        While reader.Read

          Select Case reader.TokenType
            Case JsonToken.PropertyName
              lstrCurrentPropertyName = reader.Value.ToString.ToLower()

            Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
              Select Case lstrCurrentPropertyName

                Case "description"
                  lstrDescription = reader.Value

                Case "id"
                  lstrId = reader.Value

                Case "name"
                  lstrName = reader.Value

              End Select

            Case JsonToken.StartArray
              If lstrCurrentPropertyName.ToLower = "projects" Then
                lobjProjectInfoCollection = New ProjectInfoCollectionConverter().ReadJson(reader, GetType(JobInfo), existingValue, serializer)
              End If
            Case JsonToken.EndObject
              Exit While

          End Select
        End While

        lobjItemInfo = New AreaInfo(lstrId, lstrName, lstrDescription, lobjProjectInfoCollection)

        Return lobjItemInfo

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