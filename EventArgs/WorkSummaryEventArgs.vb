'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  WorkSummaryEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  6/28/2014 3:03:55 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class WorkSummaryEventArgs
  Inherits EventArgs

#Region "Class Variables"

  Private mobjWorkSummary As IWorkSummary = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property WorkSummary As IWorkSummary
    Get
      Try
        Return mobjWorkSummary
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Friend Sub New(workSummary As IWorkSummary)
    MyBase.New()
    Try
      mobjWorkSummary = workSummary
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
