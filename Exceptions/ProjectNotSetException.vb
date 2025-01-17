'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectNotSetException.vb
'   Description :  [type_description_here]
'   Created     :  5/17/2013 10:55:55 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Exceptions

#End Region

Public Class ProjectNotSetException
  Inherits CtsException

#Region "Constructors"

  Public Sub New()
    MyBase.New("The project has not been set.")
  End Sub

  Public Sub New(message As String)
    MyBase.New(message)
  End Sub

  Public Sub New(message As String, innerException As Exception)
    MyBase.New(message, innerException)
  End Sub

#End Region

End Class
