' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  ICatalogConfiguration.vb
'  Description :  Runs Jobs using ExecutionConfiguration object.
'  Created     :  8/14/2015 1:42:20 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

Imports System.ComponentModel

Public Interface ICatalogConfiguration
  Inherits INotifyPropertyChanged

  Property MaxProjectSize As Integer

  Property MaxJobSize As Integer

  Function ToSQLXmlString() As String

End Interface
