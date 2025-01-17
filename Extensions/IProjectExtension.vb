' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  IProjectExtension.vb
'  Description :  Used for managing extension dlls for projects.
'  Created     :  11/16/2011 10:07:04 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Extensions

#End Region

Namespace Extensions

  Public Interface IProjectExtension
    Inherits IExtension

#Region "Properties"

    ''' <summary>
    ''' Gets or sets the extension type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Type As ProjectExtensionType

#End Region

  End Interface

End Namespace