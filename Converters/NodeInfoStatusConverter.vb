'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  NodeInfoStatusConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/25/2014 8:52:51 AM
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

  Public Class NodeInfoStatusConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.GetInterface("INodeInfo") IsNot Nothing Then
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

        Dim lobjNodeInfo As NodeInfo = Nothing
        'Dim lobjComputerInfo As ComputerInfo = Nothing

        Dim lstrAddress As String = String.Empty
        Dim lstrCreateDate As String = String.Empty
        Dim ldatCreateDate As DateTime = DateTime.MinValue
        Dim lstrDescription As String = String.Empty
        Dim lstrId As String = String.Empty
        Dim lstrName As String = String.Empty
        Dim lstrRole As String = String.Empty
        Dim lenuRole As NodeRole
        Dim lstrStatus As String = String.Empty
        Dim lenuStatus As NodeStatus
        Dim lstrVersion As String = String.Empty

        Dim lstrCurrentPropertyName As String = String.Empty

        While reader.Read

          Select Case reader.TokenType
            Case JsonToken.PropertyName
              lstrCurrentPropertyName = reader.Value.ToString.ToLower()

            Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
              Select Case lstrCurrentPropertyName
                Case "address"
                  lstrAddress = reader.Value

                Case "createdate"
                  lstrCreateDate = reader.Value
                  If DateTime.TryParse(lstrCreateDate, New CultureInfo("en-US"), DateTimeStyles.None, ldatCreateDate) = False Then
                    ApplicationLogging.WriteLogEntry(String.Format("Failed to parse node create date from value '{0}'.", _
                      lstrCreateDate), Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Error, 76529, lstrCreateDate)
                  End If

                Case "description"
                  lstrDescription = reader.Value

                Case "id"
                  lstrId = reader.Value

                Case "name"
                  lstrName = reader.Value

                Case "role"
                  lstrRole = reader.Value
                  lenuRole = Helper.StringToEnum(lstrRole, GetType(NodeRole))

                Case "status"
                  lstrStatus = reader.Value
                  lenuStatus = Helper.StringToEnum(lstrStatus, GetType(NodeStatus))

                Case "version"
                  lstrVersion = reader.Value

              End Select

            Case JsonToken.StartObject
              'If lstrCurrentPropertyName = "computerinfo" Then
              '  Dim lobjComputerInfoConverter As New ComputerInfoConverter
              '  lobjComputerInfo = lobjComputerInfoConverter.ReadJson(reader, GetType(ComputerInfo), Nothing, serializer)
              'End If
            Case JsonToken.EndObject

          End Select
        End While

        'lobjNodeInfo = New NodeInfo(lstrId, lstrName, lstrDescription, lstrAddress, lenuRole, lenuStatus, lstrVersion, ldatCreateDate, lobjComputerInfo)
        lobjNodeInfo = New NodeInfo(lstrId, lstrName, lstrDescription, lstrAddress, lenuRole, lenuStatus, lstrVersion, ldatCreateDate)

        Return lobjNodeInfo

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Overrides Sub WriteJson(writer As JsonWriter, value As Object, serializer As JsonSerializer)
      Try

#If NET8_0_OR_GREATER Then
        ArgumentNullException.ThrowIfNull(value)
#Else
        If value Is Nothing Then
          Throw New ArgumentNullException(NameOf(value))
        End If
#End If

        If Helper.IsRunningInstalled Then
          serializer.Formatting = Formatting.None
        Else
          serializer.Formatting = Formatting.Indented
        End If

        Dim lobjItem As INodeInfo = DirectCast(value, INodeInfo)

        writer.WriteStartObject()

        'writer.WritePropertyName("computerInfo")
        'writer.WriteRawValue(JsonConvert.SerializeObject(lobjItem.ComputerInfo, serializer.Formatting, _
        '                                                 New ComputerInfoStatusConverter()))

        writer.WritePropertyName("status")
        writer.WriteValue(lobjItem.Status.ToString())

        writer.WriteEndObject()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

  End Class

End Namespace