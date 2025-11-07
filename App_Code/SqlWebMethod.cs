using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

/// <summary>
/// Summary description for SqlWebMethod
/// </summary>
public class SqlWebMethod
{
    public SqlWebMethod()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // run simple command
    [WebMethod]
    public void executesql(string cmd)
    {

        using (SqlConnection con = new SqlConnection())
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand execmd = new SqlCommand(cmd, con);
            try
            {
                execmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { execmd.Dispose(); con.Close(); }
        }

    }

    [WebMethod]
    //return ds value
    public DataSet getds(string cmd)
    {
        DataSet ds = new DataSet();
        using (SqlConnection con = new SqlConnection())
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataAdapter adp = new SqlDataAdapter(cmd, con);
            adp.SelectCommand.ExecuteNonQuery();
            adp.Fill(ds);
            con.Close();
            return ds;

        }
    }

    [WebMethod]
    // return bool to validate data value
    public bool valdatedata(string cmd)
    {
        //SqlDataReader dr;
        using (SqlConnection con = new SqlConnection())
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand execmd = new SqlCommand(cmd, con);
            dr = execmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                execmd.Dispose();
                dr.Close();
                return true;
            }
            else
            {
                execmd.Dispose();
                dr.Close();
                return false;
            }
        }
    }

    public SqlDataReader dr;

    [WebMethod]
    public IDataReader getdr(string cmd)
    {

        SqlConnection con = new SqlConnection();
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand execmd = new SqlCommand(cmd, con);
            dr = execmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                return dr;

            }
            else
            {
                return null;
            }
        }

    }

    public string getsinglevallue(string cmd)
    {
        string value;
        SqlConnection con = new SqlConnection();
        {

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand execmd = new SqlCommand(cmd, con);
            dr = execmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                value = dr[0].ToString();


            }
            else
            {
                value = "";
            }
        }
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        dr.Close();
        return value;
    }
}