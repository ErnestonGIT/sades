using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de GenerarHash
/// </summary>
public class GenerarHash
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringSAIEEIR"].ConnectionString;

    public GenerarHash()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static string GenerarHashPDF(byte[] pdfBytes)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(pdfBytes);
            StringBuilder hash = new StringBuilder();
            foreach (byte b in hashBytes)
                hash.Append(b.ToString("x2"));
            return hash.ToString();
        }
    }

    
    //public static void GuardarHashEnBaseDeDatos(string periodo, string pdfHash)
    //{
    //    using (SqlConnection sqlConn = new SqlConnection(connectionString))
    //    {
    //        sqlConn.Open();

    //        string query = "INSERT INTO PDF_Hash (PeriodoEscolar, Hash) VALUES (@PeriodoEscolar, @Hash)";
    //        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConn))
    //        {
    //            sqlCommand.Parameters.AddWithValue("@PeriodoEscolar", periodo);
    //            sqlCommand.Parameters.AddWithValue("@Hash", pdfHash);
    //            sqlCommand.ExecuteNonQuery();
    //        }
    //    }
    //}

}