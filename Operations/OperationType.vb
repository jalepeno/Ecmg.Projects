' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  Operation.vb
'  Description :  Used to support intellisense and extensibility for operation types.
'  Created     :  11/15/2011 10:29:27 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

Namespace Operations

  Public Class OperationType

#Region "Private Constants"
    Private Const CANCEL_CHECKOUT_CONST As String = "CancelCheckOut"
    Private Const CHECKIN_CONST As String = "CheckIn"
    Private Const CHECKOUT_CONST As String = "CheckOut"
    Private Const DELETE_CONST As String = "Delete"
    Private Const EXPORT_CONST As String = "Export"
    Private Const MIGRATE_CONST As String = "Migrate"

    Private Const NOT_SET_CONST As String = "NotSet"
    Private Const REPLACE_CONST As String = "Replace"
    Private Const UNFILE_CONST As String = "UnFile"

#End Region

#Region "Public Shared Properties"

    Public Shared ReadOnly Property CancelCheckOut As String
      Get
        Return CANCEL_CHECKOUT_CONST
      End Get
    End Property

    Public Shared ReadOnly Property CheckIn As String
      Get
        Return CHECKIN_CONST
      End Get
    End Property

    Public Shared ReadOnly Property CheckOut As String
      Get
        Return CHECKOUT_CONST
      End Get
    End Property

    Public Shared ReadOnly Property Delete As String
      Get
        Return DELETE_CONST
      End Get
    End Property

    Public Shared ReadOnly Property Export As String
      Get
        Return EXPORT_CONST
      End Get
    End Property

    Public Shared ReadOnly Property Migrate As String
      Get
        Return MIGRATE_CONST
      End Get
    End Property

    Public Shared ReadOnly Property NotSet As String
      Get
        Return NOT_SET_CONST
      End Get
    End Property

    Public Shared ReadOnly Property Replace As String
      Get
        Return REPLACE_CONST
      End Get
    End Property

    Public Shared ReadOnly Property UnFile As String
      Get
        Return UNFILE_CONST
      End Get
    End Property

#End Region

  End Class

End Namespace