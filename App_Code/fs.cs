using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

public class fs : System.Web.UI.Page
{
    public static object miBitacora(int tipo_mov, string desc)
    {
        string consulta = "SELECT CASE WHEN MAX(ID_BITACORA) IS NULL THEN 1 ELSE MAX(ID_BITACORA) + 1 END FROM Bitacora";
        string id = Consultas.TipoUsuarioP(consulta);

        string usuario = System.Web.HttpContext.Current.Request.Cookies["id_usuario"].Value;

        string perfil = System.Web.HttpContext.Current.Request.Cookies["Tipo"].Value;

        string fec_mov = String.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

        string ipAdresses = ObtenerIPs();
        string ip = LasIPv4(ipAdresses);

        string consulta2 = "INSERT INTO BITACORA VALUES ('" + id + "', '" + usuario + "','" + perfil + "',CONVERT(datetime,'" + fec_mov + "'),'" + tipo_mov + "', '" + desc + "', '" + ip + "')";        
        Consultas.SentenciaP(consulta2);

        return 1;
    }
    public static object miBitacoraClient(int tipo_mov, string hostCl, string desc, string nomCli)
    {
        string consulta = "SELECT CASE WHEN MAX(ID_BITACORA) IS NULL THEN 1 ELSE MAX(ID_BITACORA) + 1 END FROM Bitacora";
        string id = Consultas.TipoUsuarioP(consulta);

        string usuario = System.Web.HttpContext.Current.Request.Cookies["id_usuario"].Value;
        string perfil = System.Web.HttpContext.Current.Request.Cookies["Tipo"].Value;
        string fec_mov = String.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

        /******************/        
        string nombreCliente = "";
        string ipV4 = "";

        try
        {
            nombreCliente = System.Net.Dns.GetHostEntry(hostCl).HostName;
            ipV4 = hostCl;
        }
        catch (Exception)
        {
            nombreCliente = String.IsNullOrEmpty(nomCli) == true ? "PC Desconocida (no resuelta)" : nomCli;
        }
        /******************/
        desc += "; desde "+nombreCliente;

        string consulta2 = "INSERT INTO BITACORA VALUES ('" + id + "', '" + usuario + "','" + perfil + "',CONVERT(datetime,'" + fec_mov + "'),'" + tipo_mov + "', '" + desc + "', '" + ipV4 + "')";        
        Consultas.SentenciaP(consulta2);

        return 1;
    }
    public static string ObtenerIPs()
    {
        StringBuilder sb = new StringBuilder();
        string ipAddresses;

        try
        {
            var hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
                sb.Append(string.Format("{0}, ", address));

            ipAddresses = sb.ToString().TrimEnd(", ".ToCharArray());
        }
        catch (Exception ex)
        {
            ipAddresses = "ERROR: " + ex.Message;
        }
        return ipAddresses;
    }


    public static string LasIPv4(string ipAdresses)
    {
        StringBuilder sb = new StringBuilder();

        string sRegExIPv4 = @"((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)";
        Regex r = new Regex(sRegExIPv4);
        foreach (Match m in r.Matches(ipAdresses))
        {
            if (m.Success)
            {
                sb.Append(string.Format("{0}, ", m.Value));
            }
        }

        return sb.ToString().TrimEnd(", ".ToCharArray());
    }
}