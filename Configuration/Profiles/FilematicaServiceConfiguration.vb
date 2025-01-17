' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  FilematicaServiceConfiguration.vb
'  Description :  [type_description_here]
'  Created     :  11/15/2012 4:23:34 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.SerializationUtilities
Imports Documents.Utilities

#End Region

Namespace Configuration

  <DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
  Public Class FilematicaServiceConfiguration

#Region "Class Variables"

    Private mobjProfiles As New JobProfiles

#End Region

#Region "Public Properties"

    Public Property Profiles As JobProfiles
      Get
        Try
          Return mobjProfiles
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Get
      Set(value As JobProfiles)
        Try
          mobjProfiles = value
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          '   Re-throw the exception to the caller
          Throw
        End Try
      End Set
    End Property

#End Region

#Region "Public Methods"

    Public Shared Function Open(lpFilePath As String) As FilematicaServiceConfiguration
      Try
        Return Serializer.Deserialize.XmlFile(lpFilePath, GetType(FilematicaServiceConfiguration))
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Function

    Public Sub Save(lpFilePath As String)
      Try
        Serializer.Serialize.XmlFile(Me, lpFilePath)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

#Region "Protected Methods"

    Protected Friend Overridable Function DebuggerIdentifier() As String
      Dim lobjIdentifierBuilder As New StringBuilder
      Try
        With lobjIdentifierBuilder
          If Profiles Is Nothing Then
            .Append("Profiles collection not initialized.")
          Else
            Select Case Profiles.Count
              Case 0
                .Append("No Profiles")
              Case 1
                .Append("1 Profile")
              Case Is > 1
                .AppendFormat("{0} Profiles", Profiles.Count)
            End Select
          End If
        End With
        Return lobjIdentifierBuilder.ToString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        Return lobjIdentifierBuilder.ToString
      End Try
    End Function

#End Region

  End Class

End Namespace