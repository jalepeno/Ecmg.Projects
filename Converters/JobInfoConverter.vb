'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobInfoConverter.vb
'   Description :  [type_description_here]
'   Created     :  6/2/2014 11:00:22 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Operations

#End Region

Namespace Converters

  Public Class JobInfoConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
      Try
        If objectType.Name.Contains("JobInfo") Then
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

        Dim lobjJobInfo As JobInfo = Nothing

        Dim lintBatchSize As Integer
        Dim lstrCancellationReason As String = String.Empty
        Dim lstrDescription As String = String.Empty
        Dim lstrDisplayName As String = String.Empty
        Dim lstrProjectName As String = String.Empty
        Dim lstrId As String = String.Empty
        Dim lblnIsCancelled As Boolean
        Dim lblnIsCompleted As Boolean
        Dim lblnIsInitialized As Boolean
        Dim lblnIsRunning As Boolean
        Dim lintBatchThreadsRunning As Integer
        Dim llngItemsProcessed As Long
        Dim lstrName As String = String.Empty
        Dim lstrOperation As String = String.Empty
        Dim lstrCreateDate As String = String.Empty
        Dim ldatCreateDate As DateTime
        Dim lobjProcess As IProcess = Nothing
        Dim lobjWorkSummary As WorkSummary = Nothing

        Dim lstrCurrentPropertyName As String = String.Empty

        While reader.Read

          Select Case reader.TokenType
            Case JsonToken.PropertyName
              lstrCurrentPropertyName = reader.Value

            Case JsonToken.String, JsonToken.Boolean, JsonToken.Date, JsonToken.Integer, JsonToken.Float
              Select Case lstrCurrentPropertyName.ToLower()

                Case "batchsize"
                  lintBatchSize = reader.Value

                Case "cancellationreason"
                  lstrCancellationReason = reader.Value

                Case "createdate"
                  lstrCreateDate = reader.Value
                  ldatCreateDate = DateTime.Parse(lstrCreateDate)

                Case "description"
                  lstrDescription = reader.Value

                Case "displayname"
                  lstrDisplayName = reader.Value

                Case "id"
                  lstrId = reader.Value

                Case "iscancelled"
                  lblnIsCancelled = reader.Value

                Case "iscompleted"
                  lblnIsCompleted = reader.Value

                Case "isinitialized"
                  lblnIsInitialized = reader.Value

                Case "isrunning"
                  lblnIsRunning = reader.Value

                Case "batchthreadsrunning"
                  lintBatchThreadsRunning = reader.Value

                Case "itemsprocessed"
                  llngItemsProcessed = reader.Value

                Case "name"
                  lstrName = reader.Value

                Case "projectname"
                  lstrProjectName = reader.Value

                Case "operation"
                  lstrOperation = reader.Value

              End Select

            Case JsonToken.StartObject
              'If lstrCurrentPropertyName = "Process" Then
              '  Dim lobjProcessConverter As New ProcessConverter
              '  lobjProcess = lobjProcessConverter.ReadJson(reader, GetType(Process), existingValue, serializer)
              'End If
              If lstrCurrentPropertyName = "workSummary" Then
                Dim lobjWorkSummaryConverter As New WorkSummaryConverter
                lobjWorkSummary = lobjWorkSummaryConverter.ReadJson(reader, GetType(WorkSummary), existingValue, serializer)
              End If
            Case JsonToken.EndObject
              Exit While
          End Select

        End While

        lobjJobInfo = New JobInfo(lstrId, lstrName, lstrDisplayName, lstrProjectName, lstrDescription, _
                                  lintBatchSize, lblnIsCancelled, lblnIsCompleted, lblnIsInitialized, _
                                  lblnIsRunning, lintBatchThreadsRunning, lstrCancellationReason, _
                                  llngItemsProcessed, lstrOperation, ldatCreateDate, lobjProcess, lobjWorkSummary)

        Return lobjJobInfo

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