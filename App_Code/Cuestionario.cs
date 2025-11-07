using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for conexion
/// </summary>
public class cuestionario
{

    internal sealed class ConnectionManager
    {

        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            return connection;

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


    public static void miGridView(string sql, GridView GV)
    {
        //***********************************
        //* FUNCION PARA LLENAR UN GridView *
        //***********************************
        DataSet ds = miDataSet(sql);
        GV.DataSource = ds;
        GV.DataBind();

    }

    public static void miDataList(string sql, DropDownList ddl)
    {
        //***************************************
        //* FUNCION PARA LLENAR UN DropDownList *
        //***************************************

        DataSet ds = miDataSet(sql);
        ddl.Items.Clear();

        ddl.DataSource = ds;
        ddl.DataTextField = ("descripcion");
        ddl.DataBind();



        //Dim sq As Integer = -1

        //For i As Integer = i To ds.Tables(0).Rows.Count
        //sq = ddl.Items.IndexOf(ddl.Items.FindByValue(ds.Tables(0).Rows(i)(1).ToString()))
        //If sq < 0 Then
        //ddl.Items.Add(ds.Tables(0).Rows(i)(1).ToString())
        //ddl.Items(ddl.Items.Count - 1).Value = ds.Tables(0).Rows(i)(1).ToString()
        //End If
        //Next

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
        rbl.DataValueField= ("cve_respuesta");
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

    public static void miLabelP(string sql, Label lb)
    {
        //***************************************
        //* FUNCION PARA LLENAR UN RadioButtonList *
        //***************************************

        DataSet ds = miDataSet(sql);
        lb.Text = "";

        lb.Text = (String)ds.Tables[0].Rows[0]["pregunta"];  
    }



}