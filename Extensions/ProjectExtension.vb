' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  ProjectExtension.vb
'  Description :  [type_description_here]
'  Created     :  11/16/2011 10:10:17 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports System.Reflection
Imports Documents.Extensions

#End Region

Namespace Extensions

  Public MustInherit Class ProjectExtension
    Inherits Extension
    Implements IProjectExtension

#Region "Class Variables"

    Private menuType As ProjectExtensionType = ProjectExtensionType.Operation

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets or sets the extension type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property Type As ExtensionEnumerations.ProjectExtensionType Implements IProjectExtension.Type
      Get
        Return menuType
      End Get
    End Property

#End Region

#Region "Constructors"

    ''' <summary>
    ''' The default constructor is not public.  All construction of 
    ''' extensions should go throw the CreateExtension method.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub New()
      MyBase.New()
    End Sub

#End Region

#Region "Public Shared Methods"

    'Public Overloads Shared Function CreateExtension(ByVal lpName As String, _
    '                               ByVal lpDescription As String, _
    '                               ByVal lpType As ProjectExtensionType, _
    '                               ByVal lpAssembly As Assembly) As IExtension
    '  Try
    '    Return CreateExtension(String.Empty, lpName, lpDescription, lpType, _
    '                           AssemblyHelper.WriteAssemblyToByteArray(lpAssembly))
    '  Catch ex As Exception
    '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '    ' Re-throw the exception to the caller
    '    Throw
    '  End Try
    'End Function

    ''' <summary>
    ''' Factory method for creating an extension object.
    ''' </summary>
    ''' <param name="lpName">The name of the extension.</param>
    ''' <param name="lpDescription">The description of the extension.</param>
    ''' <param name="lpType">The extension type.</param>
    ''' <param name="lpAssembly">The assembly as a byte array.</param>
    ''' <returns>A new IExtension object reference.</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function CreateExtension(ByVal lpName As String, _
                                       ByVal lpDescription As String, _
                                       ByVal lpType As ProjectExtensionType, _
                                       ByVal lpAssembly() As Byte) As IExtension
      Try
        Return CreateExtension(String.Empty, lpName, lpDescription, lpType, lpAssembly)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

    ''' <summary>
    ''' Factory method for creating an extension object.
    ''' </summary>
    ''' <param name="lpId">The id of the extension.</param>
    ''' <param name="lpName">The name of the extension.</param>
    ''' <param name="lpDescription">The description of the extension.</param>
    ''' <param name="lpType">The extension type.</param>
    ''' <param name="lpAssembly">The assembly as a byte array.</param>
    ''' <returns>A new IExtension object reference.</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function CreateExtension(ByVal lpId As String, _
                                           ByVal lpName As String, _
                                           ByVal lpDescription As String, _
                                           ByVal lpType As ProjectExtensionType, _
                                           ByVal lpAssembly() As Byte) As IExtension
      Try
        'Dim lobjExtension As New ProjectExtension

        ' TODO: Implement with Activator.CreateInstance

        'With lobjExtension
        '  .Id = lpId
        '  .mstrName = lpName
        '  If Not String.IsNullOrEmpty(lpDescription) Then
        '    .Description = lpDescription
        '  End If
        '  .menuType = lpType
        '  .ByteArray = lpAssembly
        '  .AddedByMachine = Environment.MachineName
        '  .AddedByUser = Helper.GetCurrentUser
        '  .CreateDate = Now
        'End With

        'Return lobjExtension

        ' Until this is implemented, throw an exception
        Throw New NotImplementedException

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Function

#End Region

#Region " IDisposable Support "

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
        MyBase.Dispose()
        menuType = -1
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

  End Class

End Namespace