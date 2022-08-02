using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ABMProductos
{
    class AccesoDatos
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=Informatica;Integrated Security=True");
        SqlCommand comando = new SqlCommand();
        SqlDataReader lector; 
        
            public SqlDataReader pLector
            {
                get { return lector; }
                set { lector = value; }
            }
                public void leerBD(string consultaSQL)
                {
                    conectar();
                    comando.CommandText = consultaSQL; //cargue la consulta 
                    lector = comando.ExecuteReader(); //ejecute esa consulta en el lector
                }

        public DataTable consultarBD(string consultaSQL)
        {
            DataTable tabla = new DataTable(); //creo mi data table 
            conectar();//abrir la base de datos 
            comando.CommandText = consultaSQL; //configura el comando con la consulta por parametros 
            tabla.Load(comando.ExecuteReader()); //ejecuta el comando 
            desconectar(); //desconectar la base de datos 
            return tabla; //retorna una tabla 
        }
            private void conectar() //no voy a dejar que se conecte deade afuera de la base de datos 
            {
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
            }
        public void desconectar() //metodo desconectar me desconecta 
        {
            conexion.Close(); //me desconecta con la base de datos 
        }
        public void actualizarBD(string consultaSQL) 
        {
            conectar();
            comando.CommandType = CommandType.Text; //recibe una sentencia en texto 
            comando.CommandText = consultaSQL;
            comando.ExecuteNonQuery();
            desconectar();
        }
    }
}
