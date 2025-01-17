'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  CatalogConnectionNotConfiguredException.vb
'   Description :  [type_description_here]
'   Created     :  12/17/2013 8:33:36 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Configuration
Imports Documents.Exceptions

#End Region

Public Class CatalogConnectionNotConfiguredException
  Inherits CtsException

#Region "Constructors"

  Public Sub New()
    MyBase.New(String.Format("The ProjectCatalogConnectionString setting is not configured in the connection settings file {1}'{0}'.",
                             ConnectionSettings.Instance.SettingsPath, ControlChars.CrLf))

  End Sub

#End Region

End Class
