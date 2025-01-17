' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  ProjectExtensionRegistrator.vb
'  Description :  [type_description_here]
'  Created     :  8/9/2012 10:42:47 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Reflection
Imports Documents.Extensions
Imports Documents.Utilities
Imports Operations
Imports Operations.Extensions

#End Region

Public Class ProjectExtensionRegistrator

#Region "Class Variables"

  Private Shared mobjInstance As ProjectExtensionRegistrator
  Private Shared mintReferenceCount As Integer

#End Region

#Region "Constructors"

  Private Sub New()
    mintReferenceCount = 0
  End Sub

#End Region

#Region "Public Properties"

  Public ReadOnly Property ReferenceCount As Integer
    Get
      Return mintReferenceCount
    End Get
  End Property

#End Region

#Region "Singleton Support"

  Public Shared Function Instance() As ProjectExtensionRegistrator
    Try
      If mobjInstance Is Nothing Then
        mobjInstance = New ProjectExtensionRegistrator
      End If
      mintReferenceCount += 1
      Return mobjInstance
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Public Methods"

  Public Sub RegisterCurrentProjectExtensions()
    Try

      ' The complete list of current extensions
      Dim lobjAvailableExtensions As ExtensionEntries = ExtensionCatalog.Instance.Extensions
      ' The currently executing instance of this assembly
      Dim lobjCurrentAssembly As Assembly = Assembly.GetExecutingAssembly
      Dim lstrCurrentAssemblyPath As String = lobjCurrentAssembly.Location

      ' The complete set of currently defined subclasses of OperationExtension in the Ecmg.Cts.Projects assembly
      Dim list As Object = From lobjOperationExtension In lobjCurrentAssembly.GetTypes Where _
          (lobjOperationExtension.IsSubclassOf(GetType(OperationExtension)) _
           AndAlso lobjOperationExtension.IsAbstract = False) Select lobjOperationExtension

      Dim lobjExtensionEntry As IExtensionInformation = Nothing
      Dim lobjExtensionInstance As IOperationInformation = Nothing

      Dim lstrExtensionName As String = String.Empty

      ' Loop through each currently defined operation extension in this assembly and 
      ' check if it is named in the extension catalog.
      For Each lobjExtension As Type In list
        lstrExtensionName = lobjExtension.Name
        'If lobjAvailableExtensions.ContainsKey(lstrExtensionName) = False Then
        lobjExtensionEntry = lobjAvailableExtensions.Item(lstrExtensionName.Replace("Operation", String.Empty))

        If lobjExtensionEntry Is Nothing Then
          ' The operation extension is not listed, we will add it.
          lobjExtensionInstance = lobjCurrentAssembly.CreateInstance(lobjExtension.FullName)
          ExtensionCatalog.Instance.Add(lobjExtensionInstance.Name, _
                                        lobjExtensionInstance.DisplayName, _
                                        lobjExtensionInstance.Description, _
                                        lobjExtensionInstance.CompanyName, _
                                        lobjExtensionInstance.ProductName, _
                                        lobjCurrentAssembly.Location)
        Else
          If String.Compare(lobjExtensionEntry.Path, lstrCurrentAssemblyPath, True) <> 0 Then
            ' The entry in the catalog is referencing a different path, we want to be able to load the extension from the current assembly.
            ExtensionCatalog.Instance.Remove(lobjExtensionEntry.Name)
            lobjExtensionInstance = lobjCurrentAssembly.CreateInstance(lobjExtension.FullName)
            ExtensionCatalog.Instance.Add(lobjExtensionInstance.Name, _
                                          lobjExtensionInstance.DisplayName, _
                                          lobjExtensionInstance.Description, _
                                          lobjExtensionInstance.CompanyName, _
                                          lobjExtensionInstance.ProductName, _
                                          lobjCurrentAssembly.Location)
          End If
          ' The operation extension is currently listed, we can skip it.
          Continue For
        End If
      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
