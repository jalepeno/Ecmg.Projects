'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  SplitJobConfigurations.xaml.vb
'   Description :  [type_description_here]
'   Created     :  8/18/2015 9:52:43 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports Documents.Utilities

#End Region

Namespace Configuration

  Public Class SplitJobConfigurations
    Inherits JobConfigurations

#Region "Public Methods"

    Public Sub RenameSplitItems()
      Try
        If Items.Count < 2 Then
          Throw New InvalidOperationException("Not valid when less than two items are present in the collection.")
        End If
        Dim lstrOriginalName As String = Items.First.Name
        Dim lintDigits As Integer = Math.Floor(Math.Log10(Count) + 1)
        Dim lstrPaddingCode As String = String.Format("D{0}", lintDigits)

        For lintJobCounter As Integer = 0 To Count - 1
          Item(lintJobCounter).Name = String.Format("{0} - (Part {1} of {2})", lstrOriginalName, (lintJobCounter + 1).ToString(lstrPaddingCode), Count)
        Next
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

#Region "Private Methods"

    Private Sub JobConfigurations_ItemPropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Me.ItemPropertyChanged
      Try
        If (((TypeOf (sender) Is JobConfiguration) AndAlso
          (Not Helper.CallStackContainsMethodName("AssignObjectProperty")) AndAlso
          (Not Helper.CallStackContainsMethodName("RenameSplitItems")))) Then
          For Each lobjJobConfiguration As JobConfiguration In Me
            If Not lobjJobConfiguration.Equals(sender) Then
              Helper.AssignObjectProperty(e.PropertyName, sender, lobjJobConfiguration)
            End If
          Next
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Sub

#End Region

  End Class

End Namespace