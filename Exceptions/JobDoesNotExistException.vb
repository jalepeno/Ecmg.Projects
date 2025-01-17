'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobDoesNotExistException.vb
'   Description :  [type_description_here]
'   Created     :  1/13/2014 9:36:25 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------


#Region "Imports"

Imports Documents.Exceptions

#End Region

''' <summary>
''' Exception thrown when a job can't be located with the specified identifier.
''' </summary>
''' <remarks></remarks>
Public Class JobDoesNotExistException
  Inherits ItemDoesNotExistException

#Region "Constructors"

  ''' <summary>
  ''' Creates a new JobDoesNotExistException 
  ''' with the specified identifier.
  ''' </summary>
  ''' <param name="identifier">The identifier for the item.</param>
  ''' <remarks></remarks>
  Public Sub New(ByVal identifier As String)
    MyBase.New(identifier, FormatMessage(identifier))
  End Sub

  ''' <summary>
  ''' Creates a new JobDoesNotExistException 
  ''' with the specified identifier.
  ''' </summary>
  ''' <param name="identifier">The identifier for the item.</param>
  ''' <param name="innerException">The inner Exception to attach.</param>
  ''' <remarks></remarks>
  Public Sub New(ByVal identifier As String, ByVal innerException As Exception)
    MyBase.New(identifier, FormatMessage(identifier), innerException)
  End Sub

  ''' <summary>
  ''' Creates a new JobDoesNotExistException 
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

End Class
