' ********************************************************************************
' '  Document    :  BatchItemProcessedEventArgs.vb
' '  Description :  [type_description_here]
' '  Created     :  10/2/2012-11:44:36
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

#Region "Imports"

Imports Documents.Utilities
Imports Operations

#End Region

Public Class BatchItemProcessedEventArgs
  Inherits ItemProcessedEventArgs


#Region "Class Variables"

  Private ReadOnly mobjBatchWorker As BaseBatchWorker
  Private ReadOnly mobjWorkSummary As WorkSummary



#End Region
#Region "Public Properties"
  Public ReadOnly Property BatchWorker As BaseBatchWorker
    Get
      Try
        Return mobjBatchWorker
      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property
  Public ReadOnly Property WorkSummary As WorkSummary
    Get
      Try
        Return mobjWorkSummary
      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property



#End Region
#Region "Constructors"
  Public Sub New(lpWorkItem As IWorkItem, lpBatchWorker As BaseBatchWorker, lpWorkSummary As WorkSummary)
    MyBase.New(lpWorkItem)
    Try
      mobjBatchWorker = lpBatchWorker
      mobjWorkSummary = lpWorkSummary
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub


#End Region

End Class
