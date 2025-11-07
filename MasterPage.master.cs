using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;

public partial class MasterPage : System.Web.UI.MasterPage
{
    string today = DateTime.UtcNow.ToString("MM-dd-yyyy");
    string IdUser = HttpContext.Current.Request.Cookies["id_usuario"].Value.ToString();
    string perfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();
    string chP = HttpContext.Current.Request.Cookies["chP"].Value.ToString();
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            if (!IsPostBack)
            {
                LabelCambioPassword_valor.Text = chP;

                ShowPeriodoEscolar();

                LabelPerfil.Text = Consultas.ConsultaS("select descripcion from cat_perfiles where id_perfil = '" + Request.Cookies["Tipo"].Value + "'");
                LabelNomC1.Text = Request.Cookies["Nombre"].Value;
                LabelNomC2.Text = Request.Cookies["Nombre"].Value;
                LabelUAHeader.Text = Consultas.ConsultaS("select DESCRIPCION_DP from CAT_DEPENDENCIAS_POLITECNICAS where CLAVE_ZP = '" + Request.Cookies["claveZp"].Value + "'");
                ImageUAHeader.ImageUrl = Consultas.ConsultaS("select logo from CAT_DEPENDENCIAS_POLITECNICAS where CLAVE_ZP = '" + Request.Cookies["claveZp"].Value + "'");

                String rutaFoto = "~/public/img/Foto_perfil/" + Request.Cookies["id_usuario"].Value + ".jpg";
                String rutaSinFoto = "~/public/img/sin_foto.jpg";

                if (File.Exists(Server.MapPath(rutaFoto)))
                {
                    Random rnd = new Random();
                    int numa = rnd.Next(1, 999);
                    ImagePerfil.ImageUrl = rutaFoto + "?v=" + numa.ToString();
                    Avatar.ImageUrl = rutaFoto + "?v=" + numa.ToString();
                }
                else
                {
                    ImagePerfil.ImageUrl = rutaSinFoto;
                    Avatar.ImageUrl = rutaSinFoto;
                }

                string consulta3 = "select count(p.ID_Perfil) as num_perfil from USERS as u, USERS_PERFIL as up, cat_PERFILES as p where u.ID_USER=up.ID_USER and up.ID_Perfil=p.ID_PERFIL AND USERNAME = '" + Request.Cookies["Usuario"].Value + "' and up.estatus=1";
                int num_perfil = Int32.Parse(Consultas.TipoUsuario(consulta3));

                if (num_perfil == 1)
                {
                    CambioPerfil.Visible = false;
                    //tipoPerfilDropdown.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                }
                else if (num_perfil > 1)
                {
                    for (int i = 0; i < GridViewCambioPerfil.Rows.Count; i++)
                    {
                        if (Request.Cookies["Tipo"].Value == GridViewCambioPerfil.Rows[i].Cells[0].Text)
                        {
                            ((Button)GridViewCambioPerfil.Rows[i].Cells[2].FindControl("ButtonCambioP")).Enabled = false;
                            ((Button)GridViewCambioPerfil.Rows[i].Cells[2].FindControl("ButtonCambioP")).Text = "Activo";
                        }
                        else
                        {
                            ((Button)GridViewCambioPerfil.Rows[i].Cells[2].FindControl("ButtonCambioP")).Enabled = true;
                        }

                    }
                    CambioPerfil.Visible = true;
                }

            }

            if (IsPostBack)
            {
                loadTools();
            }
            
            string getPerfil = Request.Cookies["Tipo"].Value;

            if (String.IsNullOrEmpty(chP))
            {
                MostrarPanelUsuario("cambioP");
            }
            else
            {
                switch (getPerfil)
                {
                    case "432"://super administrador
                        MostrarPanelUsuario("administrador");
                        break;
                    case "2":
                        MostrarPanelUsuario("secAcademica");
                        break;
                    case "7":
                    case "8":
                        MostrarPanelUsuario("directorE");
                        menuCaptPeticion.Visible = true;
                        break;
                    case "42":
                        MostrarPanelUsuario("coordinacion");
                        break;
                    case "43":
                        MostrarPanelUsuario("analista");
                        break;
                    case "11":
                    case "78":
                        MostrarPanelUsuario("directorUA");
                        break;
                    case "12":
                    case "79":
                        MostrarPanelUsuario("subAcademica");
                        break;
                    case "19":
                    case "96":
                        MostrarPanelUsuario("enlace");
                        break;
                    case "33":
                    case "92":
                        MostrarPanelUsuario("cHumano");
                        break;
                    case "16":
                        MostrarPanelUsuario("docente");
                        break;
                    case "30":
                    case "88":
                        MostrarPanelUsuario("gestion");
                        break;


                    //requerimiento 09 12 2024, estos perfiles no tendrán acceso al sistema

                    //case "ok":
                    //    MostrarPanelUsuario("jefatura");
                    //    break;
                    //case "41":
                    //case "95":
                    //    MostrarPanelUsuario("academia");
                    //    break;

                    default:
                        //Response.Redirect("Dashboard.aspx");
                        break;
                }
            }
        }
    }
    private void MostrarPanelUsuario(string perfil)
    {
        switch (perfil)
        {
            case "administrador":
                menuBitacora.Visible = true;
                menuPassword.Visible = true;
                break;
            case "cambioP":;
                menuPassword.Visible = true;
                break;
        }
    }
    protected void loadTools()
    {
        string script = "infoToolStart();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", script, true);
    }
    public string FloatIconText
    {
        get { return LabelFloatIcon_pe.Text; }
        set { LabelFloatIcon_pe.Text = value; }
    }
    protected void ShowPeriodoEscolar()
    {
        string zp = Request.Cookies["claveZp"].Value;
        
        switch (perfil)
        {
            case "7":
            case "19":
            case "43":
                zp = "";
                break;
        }

        if (String.IsNullOrEmpty(zp))
        {

            LabelFloatIcon_pe.Text = Consultas.ConsultaS("select TOP 1 PE_ACTUAL from PARAMETROS where ESTATUS_PE_ACTUAL =1 order by FECHA_INI_PE_ACTUAL,PE_ACTUAL desc");

        }
        else
        {
            LabelFloatIcon_pe.Text = Consultas.ConsultaS("select PE_ACTUAL from PARAMETROS where CLAVE_ZP = '" + zp + "'");
        }
        
    }
    protected void cerrarCS(object sender, EventArgs e)
    {
        ExpireCookies();

        FormsAuthentication.SignOut();
        Response.Redirect("Default.aspx");
    }
    private void ExpireCookies()
    {
        if (HttpContext.Current != null)
        {
            int cookieCount = HttpContext.Current.Request.Cookies.Count;
            for (var i = 0; i < cookieCount; i++)
            {
                var cookie = HttpContext.Current.Request.Cookies[i];
                if (cookie != null)
                {
                    var expiredCookie = new HttpCookie(cookie.Name)
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        Domain = cookie.Domain
                    };
                    HttpContext.Current.Response.Cookies.Add(expiredCookie);
                }
            }
            HttpContext.Current.Request.Cookies.Clear();
        }
    }
    protected void LinkButtonPerfil_Click(object sender, System.EventArgs e)
    {

        string javaScriptHDocD2 = "ShowModalModalPerfil();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }
    protected void Guardar_Foto_Click(object sender, EventArgs e)
    {
        string directorio = "~/public/img/Foto_perfil/";

        if (avatarUpload.HasFile)
        {
            try
            {
                avatarUpload.SaveAs(Server.MapPath(directorio + "\\" + Request.Cookies["id_usuario"].Value + ".jpg"));
                ImagePerfil.DataBind();
                Response.Redirect(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                //string javaScriptShow1 = "ShowModalF();";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScriptShow1, true);
            }
            catch
            {
                Label1.Text = "Error al guardar imagen";
            }
        }
    }
    protected void ButtonCambiarPassword_Click(object sender, EventArgs e)
    {
        LabelE0.Text = "";
        LabelE1.Text = "";
        LabelE2.Text = "";
        LabelerrorCC.Text = "";

        if (TextBoxClaveActual.Text == "" || TextBoxClaveCambio1.Text == "" || TextBoxClaveCambio.Text == "")
        {
            if (TextBoxClaveActual.Text == "") { LabelE0.Text = "Campo Requerido"; } 
            if (TextBoxClaveCambio.Text == "") { LabelE2.Text = "Campo Requerido"; }
            if (TextBoxClaveCambio1.Text == "") { LabelE1.Text = "Campo Requerido"; }
        }

        else
        {

            string claveActual = Consultas.ConsultaS("select PASSWORD from USERS where ID_USER = '" + IdUser + "' ");

            if (claveActual != TextBoxClaveActual.Text)
            {
                LabelE0.Text = "La contraseña actual es incorrecta";
            }
            else
            {

                if (TextBoxClaveCambio1.Text == TextBoxClaveCambio.Text)
                {
                    //Consultas.miUpdate(" Update Users set Password = '" + TextBoxClaveCambio.Text + "', Cambio_password = 1 where ID_USER = '" + Request.Cookies["id_usuario"].Value + "'  ");

                    //TextBoxClaveCambio.Text = "";
                    //TextBoxClaveCambio1.Text = "";

                    LabelerrorCC.Text = "";

                    string javaScriptHide = "ModalConfirm();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHide, true);


                }
                else
                {
                    LabelerrorCC.Text = "La contraseña de verificación no coincide, por favor intenta de nuevo.";

                }
            }
        }
    }
    protected void GridViewCambioPerfil_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
    }
    protected void ButtonCambioP_Click(object sender, System.EventArgs e)
    {
        Button S_B = (Button)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewCambioPerfil.SelectedIndex = i;

        string tip = GridViewCambioPerfil.Rows[i].Cells[0].Text;

        Response.Cookies.Set(new HttpCookie("Tipo", tip));
        Response.Redirect("Dashboard.aspx");
    }
    private void UpdateData(string tb, string dt, string wh)
    {
        Consultas.miUpdate("UPDATE "+tb+" SET "+ dt +" WHERE "+ wh +"");
    }
    protected void redirectPage_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string redirectPage = btn.CommandArgument;
        string perfil = Request.Cookies["Tipo"].Value;
        switch (perfil)
        {
            case "2"://S Academica
            case "6"://S Administracion
            case "7"://Direccion NS
            case "8"://Direccion NMS
            case "11"://Direccion UA
            case "43"://Analista
                Response.Cookies.Set(new HttpCookie("claveZP", ""));
                break;
        }

        if (redirectPage == "1") { redirectPage = "MapasCurriculares.aspx"; Response.Cookies.Set(new HttpCookie("mnuNE", "1")); }
        if (redirectPage == "2") { redirectPage = "MapasCurriculares.aspx"; Response.Cookies.Set(new HttpCookie("mnuNE", "2")); }

        Response.Redirect(redirectPage);
    }
    private void ModalHide(string IdModal)
    {
        string script = "ModalHideMaster('" + IdModal + "');";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", script, true);
    }
    private void ModalShow(string IdModal)
    {
        string script = "ModalShowMaster('" + IdModal + "');";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", script, true);
    }
    protected void LinkButtonActualizar_pass_Click(object sender, EventArgs e)
    {
        Consultas.miUpdate(" Update Users set Password = '" + TextBoxClaveCambio.Text + "', Cambio_password = 1 where ID_USER = '" + Request.Cookies["id_usuario"].Value + "'  ");

        ExpireCookies();

        FormsAuthentication.SignOut();
        Response.Redirect("Default.aspx");
    }

}
