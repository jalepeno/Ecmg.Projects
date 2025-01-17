'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectDoesNotExistException.vb
'   Description :  [type_description_here]
'   Created     :  1/13/2014 9:32:55 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Exceptions
Imports Documents.Utilities

#End Region

''' <summary>
''' Exception thrown when a project can't be located with the specified identifier.
''' </summary>
''' <remarks></remarks>
Public Class ProjectDoesNotExistException
  Inherits ItemDoesNotExistException

#Region "Constructors"

  ''' <summary>
  ''' Creates a new ProjectDoesNotExistException 
  ''' with the specified identifier.
  ''' </summary>
  ''' <param name="identifier">The identifier for the item.</param>
  ''' <remarks></remarks>
  Public Sub New(ByVal identifier As String)
    MyBase.New(identifier, FormatMessage(identifier))
  End Sub

  ''' <summary>
  ''' Creates a new ProjectDoesNotExistException 
  ''' with the specified identifier.
  ''' </summary>
  ''' <param name="identifier">The identifier for the item.</param>
  ''' <param name="innerException">The inner Exception to attach.</param>
  ''' <remarks></remarks>
  Public Sub New(ByVal identifier As String, ByVal innerException As Exception)
    MyBase.New(identifier, FormatMessage(identifier), innerException)
  End Sub

  ''' <summary>
  ''' Creates a new ProjectDoesNotExistException 
  ''' with the specified identifier.
  ''' </summary>
  ''' <param name="identifier">The identifier for the item.</param>
  ''' <param name="message">The exception message.</param>
  ''' <remarks></remarks>
  Public Sub New(ByVal identifier As String, ByVal message As String)
    MyBase.New(identifier, message)
  End Sub

  ''' <summary>
  ''' Creates a new ProjectDoesNotExistException 
  ''' with the specified identifier.
  ''' </summary>
  ''' <param name="identifier">The identifier for the item.</param>
  ''' <param name="message">The exception message.</param>
  ''' <param name="innerException">The inner Exception to attach.</param>
  ''' <remarks></remarks>
  Public Sub New(ByVal identifier As String, ByVal message As String, ByVal innerException As Exception)
    MyBase.New(identifier, message, innerException)
  End Sub

#End Region

#Region "Protected Methods"

  Protected Overloads Shared Function FormatMessage(ByVal identifier As String) As String
    Try
      Return String.Format("Project '{0}' could not be found.", identifier)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
