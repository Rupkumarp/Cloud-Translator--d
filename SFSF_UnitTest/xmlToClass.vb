
'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="urn:oasis:names:tc:xliff:document:1.2"),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:oasis:names:tc:xliff:document:1.2", IsNullable:=False, ElementName:="xliff")>
Partial Public Class xlf

    Private fileField() As xliffFile

    Private versionField As Decimal

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("file")>
    Public Property file() As xliffFile()
        Get
            Return Me.fileField
        End Get
        Set
            Me.fileField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property version() As Decimal
        Get
            Return Me.versionField
        End Get
        Set
            Me.versionField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="urn:oasis:names:tc:xliff:document:1.2")>
Partial Public Class xliffFile

    Private bodyField() As xliffFileTransunit

    Private datatypeField As String

    Private originalField As String

    Private sourcelanguageField As String

    Private targetlanguageField As String

    Private dateField As Date

    Private categoryField As String

    '''<remarks/>
    <System.Xml.Serialization.XmlArrayItemAttribute("trans-unit", IsNullable:=False)>
    Public Property body() As xliffFileTransunit()
        Get
            Return Me.bodyField
        End Get
        Set
            Me.bodyField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property datatype() As String
        Get
            Return Me.datatypeField
        End Get
        Set
            Me.datatypeField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property original() As String
        Get
            Return Me.originalField
        End Get
        Set
            Me.originalField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute("source-language")>
    Public Property sourcelanguage() As String
        Get
            Return Me.sourcelanguageField
        End Get
        Set
            Me.sourcelanguageField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute("target-language")>
    Public Property targetlanguage() As String
        Get
            Return Me.targetlanguageField
        End Get
        Set
            Me.targetlanguageField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property [date]() As Date
        Get
            Return Me.dateField
        End Get
        Set
            Me.dateField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property category() As String
        Get
            Return Me.categoryField
        End Get
        Set
            Me.categoryField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="urn:oasis:names:tc:xliff:document:1.2")>
Partial Public Class xliffFileTransunit

    Private sourceField As String

    Private sizeunitField As String

    Private approvedField As String

    Private maxwidthField As Byte

    Private idField As String

    Private resnameField As String

    Private targetFiled As String

    Public Property target() As String
        Get
            Return Me.targetFiled
        End Get
        Set(value As String)
            Me.targetFiled = value
        End Set
    End Property

    '''<remarks/>
    Public Property source() As String
        Get
            Return Me.sourceField
        End Get
        Set
            Me.sourceField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute("size-unit")>
    Public Property sizeunit() As String
        Get
            Return Me.sizeunitField
        End Get
        Set
            Me.sizeunitField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property approved() As String
        Get
            Return Me.approvedField
        End Get
        Set
            Me.approvedField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property maxwidth() As Byte
        Get
            Return Me.maxwidthField
        End Get
        Set
            Me.maxwidthField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property id() As String
        Get
            Return Me.idField
        End Get
        Set
            Me.idField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property resname() As String
        Get
            Return Me.resnameField
        End Get
        Set
            Me.resnameField = Value
        End Set
    End Property
End Class


