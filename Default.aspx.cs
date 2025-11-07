using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using MessagingToolkit.QRCode.Helper;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Login_Authenticate(object sender, EventArgs e)
    {
        HiddenField_error.Value = "";
        //consulta para saber numero de perfiles que tiene cada persona
        int num_perfil = Consultas.ConsultaInt("select count(p.ID_Perfil) as num_perfil from USERS as u, USERS_PERFIL as up, CAT_PERFILES as p where u.ID_USER = up.ID_USER and up.ID_Perfil = p.ID_PERFIL and up.ESTATUS = '1' AND USERNAME = '" + Login.UserName + "' AND PASSWORD = '" + Login.Password + "' COLLATE Latin1_General_CS_AI");
        string cambioPass = "";

        if (num_perfil != 0) { cambioPass = Consultas.ConsultaS("SELECT CAMBIO_PASSWORD chP FROM USERS WHERE username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI"); }

        if (num_perfil == 1)
        {
            string consulta = "select p.ID_Perfil from USERS as u, USERS_PERFIL as up, CAT_PERFILES as p where u.ID_USER=up.ID_USER and up.ID_Perfil=p.ID_PERFIL and up.ESTATUS = '1'  AND USERNAME = '" + Login.UserName + "' AND PASSWORD = '" + Login.Password + "' COLLATE Latin1_General_CS_AI ";
            string tip = Consultas.TipoUsuario(consulta);

            if (!string.IsNullOrEmpty(tip))
            {
                string nomb = "ST";
                string consulta3 = "SELECT Nombre AS Nom FROM USERS WHERE username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI";
                nomb = Consultas.TipoUsuario(consulta3);

                String id_user;
                string consultaIdU = "SELECT id_user FROM USERS WHERE username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI";
                id_user = Consultas.TipoUsuario(consultaIdU);

                string claveZp = Consultas.TipoUsuario("SELECT uz.CLAVE_ZP FROM USERS u INNER JOIN USER_ZP uz ON u.ID_USER = uz.ID_USER WHERE u.username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND u.password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI");

                //adicion para los analistad, derivado de la asignacion de zonas
                if (tip == "43") { claveZp = ""; }

                string modalidad = Consultas.ConsultaS("select min(distinct cm.ID_MODALIDAD) from PROGRAMAS_ACADEMICOS as pa, CAT_MODALIDADES as cm where pa.ID_MODALIDAD= cm.ID_MODALIDAD and CLAVE_ZP='" + claveZp + "' ");

                string pe = Consultas.ConsultaS("select TOP 1 PE_ACTUAL from PARAMETROS where ESTATUS_PE_ACTUAL =1 order by FECHA_INI_PE_ACTUAL,PE_ACTUAL desc");

                //confirmar si el perfil esta controlado en PERMISOS_ZP
                string pflCtrl = Consultas.ConsultaS("select ID_PERFIL from PERMISOS_ZP where ID_PERFIL = '"+ tip +"' and CLAVE_ZP like '"+ claveZp + "%'");

                if (String.IsNullOrEmpty(pflCtrl))
                {
                    redireccionarDashboard( nomb,  id_user,  tip,  claveZp,  modalidad,  pe, cambioPass);
                }
                else
                {
                    //de existir se confirma si tiene permiso de acceso
                    int pflAccs = Consultas.ConsultaInt("select ESTATUS from PERMISOS_ZP where ID_PERFIL = '"+ tip +"' and CLAVE_ZP like '"+ claveZp +"%' and ESTATUS = '1'");

                    if (pflAccs == 1)
                    {
                        redireccionarDashboard(nomb, id_user, tip, claveZp, modalidad, pe, cambioPass);

                    }
                    else
                    {
                        Login.FailureText = "El perfil al que pertenece, se encuentra inhabilitado. \nConsultelo en su unidad académica";
                        HiddenField_error.Value = "3";
                    }

                }
            }

            else
            {
                //PanelUA.Visible = false;
                //Login.FailureText = "<br/><br/><br/><br/><br/>Su usuario o contraseña es incorrecto.<br/>";
                HiddenField_error.Value = "1";
            }

        }
        else if (num_perfil > 1)
        {
            string nomb = "ST";
            string consulta3 = "SELECT Nombre AS Nom FROM USERS WHERE username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI";
            nomb = Consultas.TipoUsuario(consulta3);

            String id_user;
            string consultaIdU = "SELECT id_user FROM USERS WHERE username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI";
            id_user = Consultas.TipoUsuario(consultaIdU);

            string claveZp = Consultas.TipoUsuario("SELECT c.ZONA_PAGADORA FROM USERS u INNER JOIN CAPITAL_HUMANO_PLAZAS c ON u.NUMERO_EMPLEADO = c.NUMERO_EMPLEADO WHERE u.username = '" + Login.UserName + "' COLLATE Latin1_General_CS_AI AND u.password = '" + Login.Password + "' COLLATE Latin1_General_CS_AI");

            string modalidad = Consultas.ConsultaS("select min(distinct cm.ID_MODALIDAD) from PROGRAMAS_ACADEMICOS as pa, CAT_MODALIDADES as cm where pa.ID_MODALIDAD= cm.ID_MODALIDAD and CLAVE_ZP='" + claveZp + "' ");

            string pe = Consultas.ConsultaS("select PE_ACTUAL from PARAMETROS where CLAVE_ZP = '" + claveZp + "'");

            Response.Cookies.Set(new HttpCookie("Usuario", Login.UserName));

            Response.Cookies.Set(new HttpCookie("Nombre", nomb));
            Response.Cookies.Set(new HttpCookie("id_usuario", id_user));

            Response.Cookies.Set(new HttpCookie("claveZP", claveZp));
            Response.Cookies.Set(new HttpCookie("modalidad", modalidad));
            Response.Cookies.Set(new HttpCookie("pe", pe));
            Response.Cookies.Set(new HttpCookie("chP", cambioPass));

            //string UA1 = "1";
            //Response.Cookies.Set(new HttpCookie("UA", UA1));

            Login.Visible = false;
            Login_Perfil.Visible = true;
            SqlDataSourcePerfiles.SelectCommand = "select p.ID_Perfil, DESCRIPCION from USERS as u, USERS_PERFIL as up, CAT_PERFILES as p where u.ID_USER=up.ID_USER and up.ID_Perfil=p.ID_PERFIL and Username = '" + Login.UserName + "' and  Password = '" + Login.Password + "' and up.estatus=1 ";

            GridViewPerfiles.DataBind();
        }
        else
        {
            Login.FailureText = "Su usuario o contraseña es incorrecto.";
            HiddenField_error.Value = "2";
        }

    }

    protected void redireccionarDashboard(string nomb, string id_user, string tip, string claveZp, string modalidad, string pe, string chP)
    {
        Response.Cookies.Set(new HttpCookie("Usuario", Login.UserName));

        Response.Cookies.Set(new HttpCookie("Nombre", nomb));
        Response.Cookies.Set(new HttpCookie("id_usuario", id_user));
        Response.Cookies.Set(new HttpCookie("Tipo", tip));

        Response.Cookies.Set(new HttpCookie("claveZP", claveZp));
        Response.Cookies.Set(new HttpCookie("modalidad", modalidad));
        Response.Cookies.Set(new HttpCookie("pe", pe));
        Response.Cookies.Set(new HttpCookie("chP", chP));

        //string host = Request.UserHostAddress;
        string host = GetClientIpAddress();

        string esMovil = IsMobileDevice(Request);
        fs.miBitacoraClient(1, host, nomb+" ingresó al sistema", esMovil);

        FormsAuthentication.RedirectFromLoginPage(Login.UserName, true);
        Response.Redirect("Dashboard.aspx");
    }
    private string GetClientIpAddress()
    {
        string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = Request.ServerVariables["REMOTE_ADDR"];
        }
        return ipAddress;
    }
    private string IsMobileDevice(HttpRequest request)
    {
        string userAgent = request.UserAgent.ToLower();

        if (userAgent.Contains("mobil"))
        {
            userAgent = "Móvil";
        }
        if (userAgent.Contains("android"))
        {
            userAgent = "Móvil android";
        }
        if (userAgent.Contains("iphone"))
        {
            userAgent = "Móvil iphone";
        }
        if (userAgent.Contains("windows phone"))
        {
            userAgent = "Móvil windows phone";
        }

        return userAgent;//!= null && (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone"));
    }

    protected void GridViewPerfiles_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        HttpCookie zp = Request.Cookies["claveZP"];
        string tip = GridViewPerfiles.SelectedDataKey["ID_Perfil"].ToString();
                
        if (tip == "7") {
            Response.Cookies.Set(new HttpCookie("claveZP", ""));
        }
        Response.Cookies.Set(new HttpCookie("Tipo", tip));

        FormsAuthentication.RedirectFromLoginPage(Login.UserName, true);
        Response.Redirect("Dashboard.aspx");
    }

    //protected void GridView1_SelectedIndexChangedi(object sender, System.EventArgs e)
    //{
    //    string tip = GridView1.SelectedDataKey["ID_Perfil"].ToString();

    //    string Dep_perfil = Consultas.ConsultaS("select DEPENDENCIA_PERFIL from CAT_Perfiles where ID_PERFIL ='" + tip + "'");



    //    Response.Cookies.Set(new HttpCookie("Tipo", tip));
    //    Response.Cookies.Set(new HttpCookie("Dep_perfil", Dep_perfil));

    //    if (tip == "31")
    //    {
    //        Response.Cookies.Set(new HttpCookie("JPA", "1"));
    //    }
    //    else if (tip == "32")
    //    {
    //        Response.Cookies.Set(new HttpCookie("JPA", "2"));
    //    }
    //    else if (tip == "33")
    //    {
    //        Response.Cookies.Set(new HttpCookie("JPA", "3"));
    //    }
    //    else if (tip == "34")
    //    {
    //        Response.Cookies.Set(new HttpCookie("JPA", "4"));
    //    }
    //    else if (tip == "35")
    //    {
    //        Response.Cookies.Set(new HttpCookie("JPA", "5"));
    //    }
    //    else
    //    {
    //        Response.Cookies.Set(new HttpCookie("JPA", ""));
    //    }

    //    FormsAuthentication.RedirectFromLoginPage(Login.UserName, true);
    //    fs.miBitacora(1, "Inicio sesión");
    //    Response.Redirect("Inicio.aspx");

    //}

    //protected void RadioButtonListNE_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    RadioButtonListUA.Visible = true;
    //    ButtonInicioSesionE.Visible = true;
    //}

    //protected void ButtonInicioSesionE_Click(object sender, System.EventArgs e)
    //{

    //    string UA = RadioButtonListUA.SelectedValue;
    //    Response.Cookies.Set(new HttpCookie("UA", UA));

    //    FormsAuthentication.RedirectFromLoginPage(Login.UserName, true);
    //    fs.miBitacora(1, "Inicio sesión");
    //    Response.Redirect("Inicio.aspx");

    //}

    //protected void ButtonInicioSesionE1_Click(object sender, System.EventArgs e)
    //{

    //    string UA = RadioButtonListUA1.SelectedValue;
    //    Response.Cookies.Set(new HttpCookie("UA", UA));

    //    FormsAuthentication.RedirectFromLoginPage(Login.UserName, true);
    //    fs.miBitacora(1, "Inicio sesión");
    //    Response.Redirect("Inicio.aspx");

    //}
}