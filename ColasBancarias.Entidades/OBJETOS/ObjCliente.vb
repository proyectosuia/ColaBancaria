﻿Namespace OBJETOS

    Public Class ObjCliente

        Property id_cliente As Integer
        Property id_tipo_cliente As Integer
        Property cedula_cliente As String
        Property nombre_cliente As String
        Property edad_cliente As Integer
        Property sexo_cliente As String

        Sub New()
            id_cliente = Integer.MinValue
            id_tipo_cliente = Integer.MinValue
            cedula_cliente = String.Empty
            nombre_cliente = String.Empty
            edad_cliente = Integer.MinValue
            sexo_cliente = String.Empty
        End Sub
    End Class
End Namespace
