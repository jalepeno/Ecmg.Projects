'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  DbFileInfo.vb
'   Description :  [type_description_here]
'   Created     :  12/2/2016 8:17:29 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities

#End Region


Public Class DbFileInfo

#Region "Class Variables"

  Private mstrName As String = String.Empty
  Private mstrLogicalName As String = String.Empty
  Private mobjFileSize As FileSize = Nothing
  Private mblnIsPercentGrowth As Boolean
  Private mstrGrowthInIncrementsOf As String = String.Empty
  Private mobjNextAutoCrowthSize As FileSize = Nothing
  Private mobjMaxSize As FileSize
  Private mstrMaxSize As String = String.Empty
  Private mstrFilePath As String = String.Empty
  Private mstrLogicalDriveName As String = String.Empty
  Private mstrDrive As String = String.Empty
  Private mobjDriveFreeSpace As FileSize = Nothing

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' The name of the SQL Server database.
  ''' </summary>
  ''' <returns></returns>
  Public Property Name As String
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The logical name of the SQL Server database.
  ''' </summary>
  ''' <returns></returns>
  Public Property LogicalName As String
    Get
      Try
        Return mstrLogicalName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrLogicalName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The current size of the SQL Server database file.
  ''' </summary>
  ''' <returns></returns>
  Public Property FileSize As FileSize
    Get
      Try
        Return mobjFileSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As FileSize)
      Try
        mobjFileSize = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' Is the the database file configures to grow automatically as a percentage of the current size.
  ''' </summary>
  ''' <returns></returns>
  Public Property IsPercentGrowth As Boolean
    Get
      Try
        Return mblnIsPercentGrowth
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Boolean)
      Try
        mblnIsPercentGrowth = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' If the database file is configured for automatic growth, the amount it will grow in increments of.
  ''' </summary>
  ''' <returns></returns>
  Public Property GrowthInIncrementsOf As String
    Get
      Try
        Return mstrGrowthInIncrementsOf
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrGrowthInIncrementsOf = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The amount the database file will grow by in the next auto growth.
  ''' </summary>
  ''' <returns></returns>
  Public Property NextAutoGrowthSize As FileSize
    Get
      Try
        Return mobjNextAutoCrowthSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As FileSize)
      Try
        mobjNextAutoCrowthSize = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The maximum size the database file will be allowed to grow to.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks>
  ''' <para>
  ''' If the value is zero then no growth is allowed.
  ''' </para>
  ''' <para>
  ''' If the value is -1 then the database file will be allowed to grow until the disk is full.
  ''' </para>
  ''' </remarks>
  Public Property MaxSize As FileSize
    Get
      Try
        Return mobjMaxSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As FileSize)
      Try
        mobjMaxSize = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The maximum size the database file will be allowed to grow.
  ''' </summary>
  ''' <returns></returns>
  Public Property MaxSizeString As String
    Get
      Try
        Return mstrMaxSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrMaxSize = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The fully qualified path of the database file name as seen by the SQL Server machine.
  ''' </summary>
  ''' <returns></returns>
  Public Property FilePath As String
    Get
      Try
        Return mstrFilePath
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrFilePath = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The logical drive name on the SQL Server machine where the database file is located.
  ''' </summary>
  ''' <returns></returns>
  Public Property LogicalDriveName As String
    Get
      Try
        Return mstrLogicalDriveName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrLogicalDriveName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The drive letter on the SQL Server machine where the database file is located.
  ''' </summary>
  ''' <returns></returns>
  Public Property Drive As String
    Get
      Try
        Return mstrDrive
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrDrive = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  ''' <summary>
  ''' The current amount of free space on the drive where the database file is located.
  ''' </summary>
  ''' <returns></returns>
  Public Property DriveSpaceFree As FileSize
    Get
      Try
        Return mobjDriveFreeSpace
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As FileSize)
      Try
        mobjDriveFreeSpace = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

#End Region

#Region "Constructors"

  Public Sub New

  End Sub

  Friend Sub New(lpName As String, _
                 lpLogicalName As String, _
                 lpFileSize As Double, _
                 lpIsPercentGrowth As String, _
                 lpGrowthInIncrementsOf As String, _
                 lpNextAutoGrowthSize As Double, _
                 lpMaxSize As String, _
                 lpFilePath As String, _
                 lpLogicalDriveName As String, _
                 lpDrive As String, _
                 lpFreeSpace As String)
    Try

      Dim lblnDriveSpaceAvailable As Boolean = True

      Name = lpName
      LogicalName = lpLogicalName
      FileSize = New FileSize(CLng(lpFileSize * 1024 * 1024))
      Select Case lpIsPercentGrowth
        Case "No"
          IsPercentGrowth = False
        Case "Yes"
          IsPercentGrowth = True
        Case Else
          IsPercentGrowth = False
      End Select
      GrowthInIncrementsOf = lpGrowthInIncrementsOf
      NextAutoGrowthSize = New FileSize(CLng(lpNextAutoGrowthSize * 1024 * 1024))

      If IsNumeric(lpFreeSpace) Then
        DriveSpaceFree = New FileSize(CLng(lpFreeSpace) * 1024 * 1024)
      Else
        lblnDriveSpaceAvailable = False
      End If

      Select Case lpMaxSize
        Case "No growth is allowed"
          MaxSize = FileSize
        Case "File will grow until the disk is full"
          If lblnDriveSpaceAvailable
            MaxSize = DriveSpaceFree
          Else
            MaxSize = Nothing
          End If
        Case Else
          If Not IsNumeric(lpMaxSize)
            Throw New ArgumentOutOfRangeException("lpMaxSize", lpMaxSize, "Numeric value expected.")
          End If
          MaxSize = New FileSize(CLng(CDbl(lpMaxSize) * 1024 * 1024))
      End Select
      FilePath = lpFilePath
      LogicalDriveName = lpLogicalDriveName
      Drive = lpDrive
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Overloaded Methods"

  Public Overrides Function ToString() As String
    Try
      Dim lobjStringBuilder As New StringBuilder

      lobjStringBuilder.AppendFormat(LogicalName)

      If FileSize IsNot Nothing
        lobjStringBuilder.AppendFormat(": {0}", FileSize.ToString())
      End If

      If MaxSize IsNot Nothing
        lobjStringBuilder.AppendFormat(" (MaxSize = {0})", MaxSize.ToString())
      End If

      If DriveSpaceFree IsNot Nothing
        lobjStringBuilder.AppendFormat(" - Drive Free Space = {0}", DriveSpaceFree.ToString())
      End If

      Return lobjStringBuilder.ToString()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
