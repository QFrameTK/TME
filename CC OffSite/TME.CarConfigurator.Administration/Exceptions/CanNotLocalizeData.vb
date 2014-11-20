﻿Namespace Exceptions
    <Serializable()> Public Class CanNotLocalizeData
        Inherits SystemException

        Public Sub New()
            MyBase.New("You can not localize the data in the current state.")
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub
        Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class
End NameSpace