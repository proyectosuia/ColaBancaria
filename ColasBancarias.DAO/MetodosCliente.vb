﻿Imports Oracle.DataAccess.Client
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports ColasBancarias.Entidades


Namespace MetodosCliente
    Public Class MetodosCliente
        Dim objeto As Integer
        Public Function consultarCliente(objeto) As DataTable
            Dim tablaClientes As New DataTable()
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                conn.cmd = New OracleCommand("DATOS_CLIENTE_ID", conn.connection)
                conn.cmd.CommandType = CommandType.StoredProcedure

                conn.cmd.Parameters.Add("P_ID_CEDULA", OracleDbType.Int32, objeto, ParameterDirection.Input)
                conn.cmd.Parameters.Add("P_DATOS", OracleDbType.RefCursor, ParameterDirection.Output)

                Dim dataReader As OracleDataReader = conn.cmd.ExecuteReader()

                tablaClientes.Load(dataReader)
                conn.cmd.Dispose()
                conn.connection.Close()
                Return tablaClientes
            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al consultar Clientes", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                Return tablaClientes
            End Try
        End Function

        Public Function listaCliente() As DataTable
            Dim tablaListaClientes As New DataTable()
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                conn.cmd = New OracleCommand("LISTA_CLIENTES", conn.connection)
                conn.cmd.CommandType = CommandType.StoredProcedure

                conn.cmd.Parameters.Add("P_LISTA", OracleDbType.RefCursor, ParameterDirection.Output)

                Dim dataReader As OracleDataReader = conn.cmd.ExecuteReader()

                tablaListaClientes.Load(dataReader)
                conn.cmd.Dispose()
                conn.connection.Close()
                Return tablaListaClientes
            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al consultar Clientes", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                Return tablaListaClientes
            End Try
        End Function

        Public Sub insertarCliente(ByVal ObjCliente As OBJETOS.ObjCliente)
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                conn.cmd = New OracleCommand("INSERT_TBCLIENTE", conn.connection)
                conn.cmd.CommandType = CommandType.StoredProcedure

                conn.cmd.Parameters.Add("PTIPO_CLIENTE", OracleDbType.Int32, ObjCliente.id_tipo_cliente, ParameterDirection.Input)
                conn.cmd.Parameters.Add("PCEDULA", OracleDbType.Varchar2, ObjCliente.cedula_cliente, ParameterDirection.Input)
                conn.cmd.Parameters.Add("PNOMBRE", OracleDbType.Varchar2, ObjCliente.nombre_cliente, ParameterDirection.Input)
                conn.cmd.Parameters.Add("PEDAD_CLIENTE", OracleDbType.Int32, ObjCliente.edad_cliente, ParameterDirection.Input)
                conn.cmd.Parameters.Add("PSEXO", OracleDbType.Varchar2, ObjCliente.sexo_cliente, ParameterDirection.Input)
                conn.cmd.ExecuteReader()

                conn.cmd.Dispose()
                conn.connection.Close()
            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al insertar Cliente", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        End Sub

        Public Function SiguienteEnFila() As Collection
            Dim valores As New Collection
            Dim Id_Cliente As String
            Dim Nombre_Cliente As String
            Dim Prioridad As String
            Dim Sexo_Cliente As String
            Id_Cliente = String.Empty
            Nombre_Cliente = String.Empty
            Prioridad = String.Empty
            Sexo_Cliente = String.Empty
            Dim Param As OracleParameter
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                conn.cmd = New OracleCommand("SIGUIENTE_EN_FILA", conn.connection)
                conn.cmd.CommandType = CommandType.StoredProcedure
                
                conn.cmd.Parameters.Add("PId_Cliente", OracleDbType.Int32, ParameterDirection.Output)
                conn.cmd.Parameters.Add("PPrioridad", OracleDbType.Int32, ParameterDirection.Output)
                Param = conn.cmd.Parameters.Add("PNombre_Cliente", OracleDbType.Varchar2, ParameterDirection.Output)
                Param.Size = 200
                Param = conn.cmd.Parameters.Add("PSexo_Cliente", OracleDbType.Varchar2, ParameterDirection.Output)
                Param.Size = 200
                conn.cmd.ExecuteNonQuery()

                Nombre_Cliente = conn.cmd.Parameters("PNombre_Cliente").Value.ToString
                Id_Cliente = conn.cmd.Parameters("PId_Cliente").Value.ToString
                Prioridad = conn.cmd.Parameters("PPrioridad").Value.ToString
                Sexo_Cliente = conn.cmd.Parameters("PSexo_Cliente").Value.ToString
                conn.cmd.Dispose()
                conn.connection.Close()

            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al consultar el cliente que sigue en la fila", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
            valores.Add(Id_Cliente, "Id_Cliente")
            valores.Add(Nombre_Cliente, "Nombre_Cliente")
            valores.Add(Prioridad, "Prioridad")
            valores.Add(Sexo_Cliente, "Sexo_Cliente")
            Return valores

        End Function

        Public Function ClientesEnFila() As String
            Dim Cantidad_Clientes As String
            Cantidad_Clientes = String.Empty
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                conn.cmd = New OracleCommand("CLIENTES_EN_FILA", conn.connection)
                conn.cmd.CommandType = CommandType.StoredProcedure
                conn.cmd.Parameters.Add("PCantidad_Clientes", OracleDbType.Int32, ParameterDirection.Output)
                conn.cmd.ExecuteNonQuery()

                Cantidad_Clientes = conn.cmd.Parameters("PCantidad_Clientes").Value.ToString

                conn.cmd.Dispose()
                conn.connection.Close()

            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al consultar la cantidad Clientes", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
            Return Cantidad_Clientes

        End Function

        Public Function ValidarCliente(ByVal cedula As String) As String
            Dim resultado As String
            Dim consulta As String
            resultado = ""
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                consulta = "select VALIDAR_CLIENTE('" & cedula & "') from Dual"
                conn.cmd = New OracleCommand(consulta, conn.connection)
                conn.cmd.CommandType = CommandType.Text
                Dim objResultado As Object = conn.cmd.ExecuteScalar()
                resultado = objResultado.ToString()

                conn.cmd.Dispose()
                conn.connection.Close()

            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al consultar la cantidad Clientes", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
            Return resultado
        End Function

        Public Sub IngresarEnFila(ByVal cedulaCliente As String)
            Try
                Dim conn As New ConeccionOracle.ConeccionOracle()
                If conn.connection.State = ConnectionState.Closed Then
                    conn.connection.Open()
                End If
                conn.cmd = New OracleCommand("POSICIONCLIENTE", conn.connection)
                conn.cmd.CommandType = CommandType.StoredProcedure

                conn.cmd.Parameters.Add("PCEDULA", OracleDbType.Varchar2, cedulaCliente, ParameterDirection.Input)
                conn.cmd.ExecuteReader()

                conn.cmd.Dispose()
                conn.connection.Close()
            Catch ex As Exception
                MessageBox.Show("Error: " + ex.Message, "Error al insertar el cliente en la fila", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        End Sub
    End Class
End Namespace
