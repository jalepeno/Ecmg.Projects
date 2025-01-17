'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

Imports System.Configuration

Namespace CustomConfigSections

  Public Class RecentItemsLocationSection
    Inherits ConfigurationSection

    ' Declare the ImportedSearches collection property.
    ' Note: the "IsDefaultCollection = false" instructs 
    '.NET Framework to build a nested section of 
    'the kind <recentitemslocation> ...</recentitemslocation>.
    <ConfigurationProperty("RecentItems", IsDefaultCollection:=False), _
     ConfigurationCollection(GetType(RecentItemsLocationCollection), AddItemName:="add", ClearItemsName:="clear", RemoveItemName:="remove")> _
    Public ReadOnly Property RecentItems() As RecentItemsLocationCollection
      Get

        Dim isCollection As RecentItemsLocationCollection = CType(MyBase.Item("RecentItems"), RecentItemsLocationCollection)
        Return isCollection
      End Get
    End Property

  End Class

End Namespace
