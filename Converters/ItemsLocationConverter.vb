'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ItemsLocationConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/9/2014 1:36:06 PM
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

  Public Class ItemsLocationConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("IItemsLocation") IsNot Nothing Then
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

        Dim lobjItem As IItemsLocation = Nothing

        Dim lstrDatabaseName As String = String.Empty
        Dim lstrDatabasePath As String = String.Empty
        Dim lstrLocation As String = String.Empty
        Dim lstrPassword As String = String.Empty
        Dim lstrUserName As String = String.Empty
        Dim lstrServerName As String = String.Empty
        Dim lstrTrustedConnection As String = "no"
        Dim lstrType As String = String.Empty
        Dim lenuType As ContainerType

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

                Case "databaseName"
                  lstrDatabaseName = reader.Value

                Case "databasePath"
                  lstrDatabasePath = reader.Value

                Case "location"
                  lstrLocation = reader.Value

                Case "password"
                  lstrPassword = reader.Value

                Case "userName"
                  lstrUserName = reader.Value

                Case "serverName"
                  lstrServerName = reader.Value

                Case "trustedConnection"
                  lstrTrustedConnection = reader.Value

                Case "type"
                  lstrType = reader.Value
                  lenuType = [Enum].Parse(GetType(ContainerType), lstrType)
              End Select

            Case JsonToken.EndObject
              Exit While

          End Select

        End While

        lobjItem = New ItemsLocation(lenuType, lstrServerName, lstrDatabaseName, lstrTrustedConnection, lstrUserName, lstrPassword)

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

        Dim lobjItem As IItemsLocation = DirectCast(value, IItemsLocation)

        writer.WriteStartObject()

        writer.WritePropertyName("type")
        writer.WriteValue(lobjItem.Type.ToString())

        If Not String.IsNullOrEmpty(lobjItem.Location) Then
          writer.WritePropertyName("location")
          writer.WriteValue(lobjItem.Location)
        End If

        writer.WritePropertyName("serverName")
        writer.WriteValue(lobjItem.ServerName)

        writer.WritePropertyName("databaseName")
        writer.WriteValue(lobjItem.DatabaseName)

        If Not String.IsNullOrEmpty(lobjItem.UserName) Then
          writer.WritePropertyName("userName")
          writer.WriteValue(lobjItem.UserName)
        End If

        If Not String.IsNullOrEmpty(lobjItem.Password) Then
          writer.WritePropertyName("password")
          writer.WriteValue(lobjItem.Password)
        End If

        If Not String.IsNullOrEmpty(lobjItem.DatabasePath) Then
          writer.WritePropertyName("databasePath")
          writer.WriteValue(lobjItem.DatabasePath)
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