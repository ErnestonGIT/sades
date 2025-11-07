using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;

public static class Consultas
{


    internal sealed class ConnectionManager
    {

        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            return connection;
        }

        public static SqlConnection GetConnection2()
        {
            string connectionString2 = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
            SqlConnection connection2 = new SqlConnection(connectionString2);

            connection2.Open();
            return connection2;

        }

        public static SqlConnection GetConnectionPrueb()
        {
            string connectionString2 = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
            SqlConnection connection2 = new SqlConnection(connectionString2);

            connection2.Open();
            return connection2;

        }
    }

    public static string TipoUsuario(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }

    public static string TipoUsuarioP(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnectionPrueb())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }

    public static void Sentencia(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void SentenciaP(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnectionPrueb())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                cmd.ExecuteNonQuery();
            }
        }
    }

    public static string ConsultaS(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }

    public static string ConsultaSP(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnectionPrueb())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }

    public static int ConsultaInt(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
    }

    public static int ConsultaIntPr(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnectionPrueb())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
    }

    public static double ConsultaD(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToDouble(cmd.ExecuteScalar());

            }
        }
    }

    public static float ConsultaFloat(string consulta)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {

                return Convert.ToSingle(cmd.ExecuteScalar());

            }
        }
    }

    // con parametros
    public static int ConsultaIntP(string consulta, params SqlParameter[] parametros)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(consulta, conn))
            {
                foreach (SqlParameter parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public static void miInsert(string sql)
    {
        using (SqlConnection connection = ConnectionManager.GetConnection())
        {
            SqlCommand comando = new SqlCommand(sql, connection);
            comando.ExecuteNonQuery();
        }
    }

    public static void miInsert2(string sql)
    {
        using (SqlConnection connection = ConnectionManager.GetConnection2())
        {
            SqlCommand comando = new SqlCommand(sql, connection);
            comando.ExecuteNonQuery();
        }
    }

    // con parametros
    public static void miInsertP(string sql, params SqlParameter[] parametros)
    {
        using (SqlConnection conn = ConnectionManager.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                foreach (SqlParameter parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void miUpdate(string sql)
    {
        using (SqlConnection connection = ConnectionManager.GetConnection())
        {
            SqlCommand comando = new SqlCommand(sql, connection);
            comando.ExecuteNonQuery();
        }
    }

    public static void miUpdateP(string sql)
    {
        using (SqlConnection connection = ConnectionManager.GetConnectionPrueb())
        {
            SqlCommand comando = new SqlCommand(sql, connection);
            comando.ExecuteNonQuery();
        }
    }

    public static void miDelete(string sql)
    {
        using (SqlConnection connection = ConnectionManager.GetConnection())
        {
            SqlCommand comando = new SqlCommand(sql, connection);
            comando.ExecuteNonQuery();
        }
    }

    public static DataSet miDataSet(string sql)
    {
        //**********************************
        //* FUNCION PARA LLENAR UN DataSet *
        //**********************************

        using (SqlConnection connection = ConnectionManager.GetConnection())
        {


            SqlDataAdapter da = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
                HttpContext.Current.Response.End();
            }

            connection.Close();
            return ds;

        }

    }

    public static DataTable miDataTable(string sql)
    {
        DataTable dataTable = new DataTable();

        using (SqlConnection connection = ConnectionManager.GetConnection())
        {
           
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
            {
                adapter.Fill(dataTable);
            }
            connection.Close();
        }
        return dataTable;      
    }



    public static void miLabelP(string sql, Label lb)
    {

        DataSet ds = miDataSet(sql);
        lb.Text = "";

        lb.Text = (string)ds.Tables[0].Rows[0]["pregunta"];
    }

    public static void miRadioButtonL(string sql, RadioButtonList rbl)
    {
        //***************************************
        //* FUNCION PARA LLENAR UN RadioButtonList *
        //***************************************

        DataSet ds = miDataSet(sql);
        rbl.Items.Clear();

        rbl.DataSource = ds;
        rbl.DataTextField = ("respuesta");
        rbl.DataValueField = ("cve_respuesta");
        rbl.DataBind();
    }

    public static void miCheckBoxL(string sql, CheckBoxList cbl)
    {
        //***************************************
        //* FUNCION PARA LLENAR UN CheckBoxList *
        //***************************************

        DataSet ds = miDataSet(sql);
        cbl.Items.Clear();

        cbl.DataSource = ds;
        cbl.DataTextField = ("respuesta");
        cbl.DataValueField = ("cve_respuesta");
        cbl.DataBind();
    }


    public static void miDropDownList(string sql, DropDownList ddl)
    {
        //***************************************
        //* FUNCION PARA LLENAR UN CheckBoxList *
        //***************************************

        DataSet ds = miDataSet(sql);
        ddl.Items.Clear();

        ddl.DataSource = ds;
        ddl.DataTextField = ("respuesta");
        ddl.DataValueField = ("cve_respuesta");
        ddl.DataBind();
    }
}