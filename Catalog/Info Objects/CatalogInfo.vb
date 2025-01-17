'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  CatalogInfo.vb
'   Description :  [type_description_here]
'   Created     :  6/11/2014 1:52:55 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Projects.Converters

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class CatalogInfo
  Implements ICatalogInfo

#Region "Class Variables"

  Private ReadOnly mobjAreas As IAreaInfoCollection = New AreaInfoCollection

#End Region

#Region "ICatalogInfo Implementation"

  <JsonProperty("areas")>
  Public ReadOnly Property Areas As IAreaInfoCollection Implements ICatalogInfo.Areas
    Get
      Try
        Return mobjAreas
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Private Sub New()

  End Sub

  Friend Sub New(lpAreas As IAreaInfoCollection)
    Try
      mobjAreas = lpAreas
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Shared Function FromJson(lpJsonString As String) As ICatalogInfo
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(CatalogInfo), New CatalogInfoConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ToJson() As String Implements ICatalogInfo.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None)
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function Create() As ICatalogInfo
    Try
      Dim lobjCatalogInfo As New CatalogInfo

      For Each lobjArea As IArea In ProjectCatalog.Instance.Areas
        lobjCatalogInfo.Areas.Add(New AreaInfo(lobjArea))
      Next

      Return lobjCatalogInfo

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      Select Case Areas.Count
        Case 0
          lobjIdentifierBuilder.Append(" no areas")

        Case 1
          lobjIdentifierBuilder.Append(" 1 area ({0})", Areas.First.Name)

        Case Is > 1
          lobjIdentifierBuilder.AppendFormat("{0} areas ({1}...)", Areas.Count, Areas.First.Name)

      End Select

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

End Class
