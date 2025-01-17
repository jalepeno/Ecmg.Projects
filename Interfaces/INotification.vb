' ********************************************************************************
' '  Document    :  INotification.vb
' '  Description :  [type_description_here]
' '  Created     :  11/10/2012-12:47:28
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

#Region "Imports"

Imports Documents.Utilities.Messaging

#End Region

Namespace Notifications

  Public Interface INotification
    Inherits IMessage

    Property Name As String
    Property Basis As EventBasis
    Property Threshold As Integer
    Property IncludeProjectSummaryIfAvailable As Boolean
    Property Job As Job

    Function ToXmlElementString() As String

  End Interface

End Namespace