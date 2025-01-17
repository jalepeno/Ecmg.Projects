'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Configuration
Imports System.Globalization
Imports Documents.SerializationUtilities

Namespace CustomConfigSections

  Public Class RecentItemsLocationElement
    Inherits ConfigurationElement

    Public Sub New(ByVal name As String, _
                   ByVal itemslocation As ItemsLocation, _
                   ByVal lpIsPinned As String)
      Me.Name = name
      Me.RecentItems = itemslocation
      Me.IsPinned = lpIsPinned
    End Sub 'New

    Public Sub New()
    End Sub

    <ConfigurationProperty("name", DefaultValue:="", IsRequired:=True, IsKey:=True)> _
    Public Property Name() As String
      Get
        Return CStr(Me("name"))
      End Get
      Set(ByVal value As String)
        Me("name") = value
      End Set
    End Property

    <ConfigurationProperty("ispinned", DefaultValue:="", IsRequired:=True, IsKey:=True)> _
    Public Property IsPinned() As String
      Get
        Return CStr(Me("ispinned"))
      End Get
      Set(ByVal value As String)
        Me("ispinned") = value
      End Set
    End Property

    'RegexStringValidator("^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$")
    <ConfigurationProperty("RecentItems", DefaultValue:="", IsRequired:=True), _
     TypeConverter(GetType(CustomItemsLocationConverter))> _
    Public Property RecentItems() As ItemsLocation
      Get
        Return Me("RecentItems")
      End Get
      Set(ByVal value As ItemsLocation)
        Me("RecentItems") = value
      End Set
    End Property

  End Class

  Public NotInheritable Class CustomItemsLocationConverter
    Inherits ConfigurationConverterBase

    Friend Function ValidateType(ByVal value As Object, _
                                 ByVal expected As Type) As Boolean

      Dim result As Boolean

      If Not (value Is Nothing) AndAlso value.ToString() <> expected.ToString() Then
        result = False

      Else
        result = True
      End If

      Return result

    End Function 'ValidateType

    Public Overrides Function CanConvertTo(ByVal ctx As ITypeDescriptorContext, _
                                           ByVal type As Type) As Boolean
      Return (type.ToString() = GetType(String).ToString())

    End Function 'CanConvertTo

    Public Overrides Function CanConvertFrom(ByVal ctx As ITypeDescriptorContext, _
                                             ByVal type As Type) As Boolean
      Return (type.ToString() = GetType(String).ToString())

    End Function 'CanConvertFrom

    Public Overrides Function ConvertTo(ByVal ctx As ITypeDescriptorContext, _
                                        ByVal ci As CultureInfo, _
                                        ByVal value As Object, _
                                        ByVal type As Type) As Object
      ValidateType(value, GetType(ItemsLocation))

      'Dim data As Long = Fix(CType(value, TimeSpan).TotalMinutes)

      'Return data.ToString(CultureInfo.InvariantCulture)
      If (value IsNot Nothing) Then

        If (TypeOf value Is ItemsLocation) Then
          Return Serializer.Serialize.XmlString(value)
        End If

      End If

      Return value

    End Function 'ConvertTo

    Public Overrides Function ConvertFrom(ByVal ctx As ITypeDescriptorContext, _
                                          ByVal ci As CultureInfo, _
                                          ByVal data As Object) As Object

      'Dim min As Long = Long.Parse(CStr(data), CultureInfo.InvariantCulture)

      'Return TimeSpan.FromMinutes(System.Convert.ToDouble(min))
      If (data IsNot Nothing) Then

        If (data <> String.Empty) Then
          Return Serializer.Deserialize.XmlString(data, GetType(ItemsLocation))
        End If

      End If

      Return data

    End Function 'ConvertFrom

  End Class 'CustomItemsLocationConverter 

End Namespace
