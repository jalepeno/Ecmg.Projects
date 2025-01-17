'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  StartProcessEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  9/29/2016 1:25:31 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region


Public Class StartProcessEventArgs

#Region "Class Variables"

  Private mobjProcessInfo As ProcessStartInfo
  'Private mintJobBatchCount As Integer
  'Private mintProcessIds As New List(Of Integer)
  'Private mobjJobSet As JobSet

#End Region

#Region "Class Properties"

  Friend ReadOnly Property ProcessInfo As ProcessStartInfo
    Get
      Try
        Return mobjProcessInfo
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  'Friend ReadOnly Property JobBatchCount As Integer
  '  Get
  '    Try
  '      Return mintJobBatchCount
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  'Friend Readonly Property ProcessIds As List(Of Integer)
  '  Get
  '    Try
  '      Return mintProcessIds
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

  'Friend ReadOnly Property JobSet As JobSet
  '  Get
  '    Try
  '      Return mobjJobSet
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

#End Region

#Region "Constructors"

  Friend Sub New(lpProcessInfo As ProcessStartInfo)
    Try
      mobjProcessInfo = lpProcessInfo
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
