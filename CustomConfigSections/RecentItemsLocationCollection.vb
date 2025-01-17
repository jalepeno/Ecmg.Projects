'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

Imports System.Configuration

Namespace CustomConfigSections

  Public Class RecentItemsLocationCollection
    Inherits ConfigurationElementCollection

    Public Sub New()
      ' When the collection is created, always add one element 
      ' with the default values. (This is not necessary; it is
      ' here only to illustrate what can be done; you could 
      ' also create additional elements with other hard-coded 
      ' values here.)
      'Dim itemslocation As RecentItemsLocationElement = _
      'CType(CreateNewElement(), RecentItemsLocationElement)
      'Add(itemslocation)
    End Sub 'New

    Public Overrides ReadOnly Property CollectionType() As ConfigurationElementCollectionType
      Get
        Return ConfigurationElementCollectionType.AddRemoveClearMap
      End Get
    End Property

    Protected Overrides Function CreateNewElement() As ConfigurationElement
      Return New RecentItemsLocationElement()
    End Function 'CreateNewElement

    Protected Overrides Function GetElementKey(ByVal element As ConfigurationElement) As [Object]
      Return CType(element, RecentItemsLocationElement).Name
    End Function 'GetElementKey

    Default Public Shadows Property Item(ByVal index As Integer) As RecentItemsLocationElement
      Get
        Return CType(BaseGet(index), RecentItemsLocationElement)
      End Get
      Set(ByVal value As RecentItemsLocationElement)

        If Not (BaseGet(index) Is Nothing) Then
          BaseRemoveAt(index)
        End If

        BaseAdd(index, value)
      End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As RecentItemsLocationElement
      Get
        Return CType(BaseGet(Name), RecentItemsLocationElement)
      End Get
    End Property

    Public Function IndexOf(ByVal importedsearch As RecentItemsLocationElement) As Integer
      Return BaseIndexOf(importedsearch)
    End Function 'IndexOf

    Public Sub Add(ByVal importedsearch As RecentItemsLocationElement)
      BaseAdd(importedsearch)
    End Sub 'Add

    Protected Overrides Sub BaseAdd(ByVal element As ConfigurationElement)
      BaseAdd(element, False)
    End Sub 'BaseAdd

    Public Overloads Sub Remove(ByVal importedsearch As RecentItemsLocationElement)

      If BaseIndexOf(importedsearch) >= 0 Then
        BaseRemove(importedsearch.Name)
      End If

    End Sub 'Remove

    Public Sub RemoveAt(ByVal index As Integer)
      BaseRemoveAt(index)
    End Sub 'RemoveAt

    Public Overloads Sub Remove(ByVal name As String)
      BaseRemove(name)
    End Sub 'Remove

    Public Sub Clear()
      BaseClear()
    End Sub 'Clear

    Public Overrides Function IsReadOnly() As Boolean
      Return False
    End Function

  End Class

End Namespace
