using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;//using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class PeticionAsignar : System.Web.UI.Page
{
    string emptyZP_name = "Instituto Politécnico Nacional";
    string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

    string perfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();
    string idUser = HttpContext.Current.Request.Cookies["id_usuario"].Value.ToString();
    string chP = HttpContext.Current.Request.Cookies["chP"].Value.ToString();

    string zp = HttpContext.Current.Request.Cookies["claveZP"].Value.ToString(); //HttpContext.Current.Request.QueryString["zp"];
    string pe = HttpContext.Current.Request.Cookies["pe"].Value.ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(chP))
        {
            if (!IsPostBack)
            {

                LabelPerfil.Text = perfil;
                LabelZP.Text = zp;
                LabelPE.Text = pe;
                LabelZPDesc.Text = GetUaDesciption(zp);

                validarContenido(perfil);

            }
            else
            {

            }
        }
    }
    public void ShowModal(string idModal)
    {
        string script = "ShowModal('" + idModal + "');";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", script, true);
    }
    private string GetUaDesciption(string zp)
    {
        return Consultas.ConsultaS("SELECT DESCRIPCION_DP FROM CAT_DEPENDENCIAS_POLITECNICAS WHERE CLAVE_ZP = '"+ zp +"'");
    }
    private void validarContenido(string perfil)
    {
        switch (perfil)
        {
            case "7":
                mostrarPanelDirector(true);
                break;
            case "19":
                mostrarPanelEnlace(true);
                break;
            case "43":
                break;
            case "432"://super administrador
                mostrarPanelAdministrador(true);
                break;
        }
    }
    public void mostrarPanelDirector(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;

        LabelZP.Text = string.Empty;
        HiddenFieldCollapseAsignarPeticion_selected.Value  = "1";
        divPanelAsignarPeticion.Visible = data;

        ActualizarEstadisticas();
    }
    public void mostrarPanelEnlace(bool data)
    {

        HiddenFieldCollapseAsignarPeticion_selected.Value  = "1";
        divPanelAsignarPeticion.Visible = data;

        if (!String.IsNullOrEmpty(zp))
        {
            DropDownListAsignarPeticion_ua_SelectCommand();

            DropDownListAsignarPeticion_ua.SelectedValue = zp;
            LabelBreadCrumbZP_name.Text = LabelZPDesc.Text;
            DropDownListAsignarPeticion_ua.Enabled = false;
        }

        ActualizarEstadisticas();
    }
    public void mostrarPanelAdministrador(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseAsignarPeticion_selected.Value  = "1";
        divPanelAsignarPeticion.Visible = data;
        ActualizarEstadisticas();
    }
    public void ActualizarEstadisticas()
    {
        string zp = LabelZP.Text;
        LabelBreadCrumbZP_name.Text = String.IsNullOrEmpty(zp) == true ? emptyZP_name : GetUaDesciption(zp);

        string totalPorAsignar= Consultas.ConsultaS("select count(pet.ID_PETICION) total " +
                                                    "from PETICIONES pet " +
                                                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                                                    "where pet.ID_PETICION not in (select distinct ID_PETICION from ASIGNACION_PETICION) and pli.CLAVE_ZP like '"+ zp +"%'");

        string totalAsignadas = Consultas.ConsultaS("select count(distinct ID_PETICION) total from ASIGNACION_PETICION where ESTATUS = 1  and CLAVE_ZP like '"+ zp +"%'");

        string totalAsignaciones = Consultas.ConsultaS("select COUNT(distinct pet.ID_PETICION) AS 'value', per.DESCRIPCION AS 'name'  " +
                                                        "from ASIGNACION_PETICION pet " +
                                                        "inner join CAT_PERFILES per on per.ID_PERFIL = pet.ID_PERFIL " +
                                                        "where pet.CLAVE_ZP like '"+ zp +"%' " +
                                                        "group by per.DESCRIPCION " +
                                                        "for JSON PATH ");


        HiddenFieldGraficoPiePeticiones_datos.Value = totalAsignaciones;
        LabelPeticionesPorAsignar_total.Text = totalPorAsignar == "0" ? "0" : totalPorAsignar;
        LabelPeticionesAsignadas_total.Text = totalAsignadas == "0" ? "0" : totalAsignadas;
    }


    private void RestaurarDropDownListAsignar(int nivel)
    {
        switch (nivel)
        {
            case 0:
                DropDownListAsignarPeticion_ua.DataBind();
                ClearAndInsertItem(DropDownListAsignarPeticion_pliego);
                ClearAndInsertItem(DropDownListAsignarPeticion_categoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_subcategoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                LabelZP.Text = string.Empty;
                break;
            case 1:
                ClearAndInsertItem(DropDownListAsignarPeticion_pliego);
                ClearAndInsertItem(DropDownListAsignarPeticion_categoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_subcategoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
            case 2:
                ClearAndInsertItem(DropDownListAsignarPeticion_categoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_subcategoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
            case 3:
                ClearAndInsertItem(DropDownListAsignarPeticion_subcategoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
            case 4:
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
        }

        HiddenFieldDivPeticiones_selected.Value = string.Empty;
        HiddenFieldDivUnidad_selected.Value = string.Empty;
        DivContenidoPeticiones_seleccionadas.InnerHtml = string.Empty;

        HiddenFieldMensajeRegistroExitoso_estatus.Value = string.Empty;
        HiddenFieldPeticionEliminar_id.Value = string.Empty;

        LabelRegistrarGarantiaNotificacion_responsable.Text = string.Empty;
        TextBoxRegistrarGarantiaNotificacion_correo.Text = string.Empty;

        DivAsignarPeticion_asignaciones.Visible = false;
        DivAsignarPeticion_unidades.Visible = false;
        DivRegistrarGarantiaNotificacion_contenido.Visible = false;


    }
    private void ClearAndInsertItem(DropDownList dropDownList)
    {
        dropDownList.ClearSelection();
        dropDownList.Items.Clear();
        //if (!dropDownList.Items.Contains(new ListItem("Seleccionar", "")))
        //{
        //    dropDownList.Items.Insert(0, new ListItem("Seleccionar", ""));
        //}
    }


    protected void DropDownListAsignarPeticion_ua_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_ua.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_ua_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelZP.Text = DropDownListAsignarPeticion_ua.SelectedValue.ToString();
        RestaurarDropDownListAsignar(1);
        ActualizarEstadisticas();
    }
    protected void DropDownListAsignarPeticion_ua_SelectCommand()
    {
        SqlDataSourceDropDownAsignarPeticion_ua.SelectCommand = "SELECT CLAVE_ZP, DESCRIPCION_DP FROM  CAT_DEPENDENCIAS_POLITECNICAS WHERE ID_NIVEL_EST = 2 ORDER BY DESCRIPCION_DP ASC";
        DropDownListAsignarPeticion_ua.DataBind();
    }


    protected void DropDownListAsignarPeticion_pliego_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_pliego.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_pliego_SelectedIndexChanged(object sender, EventArgs e)
    {
        RestaurarDropDownListAsignar(2);
    }
    protected void DropDownListAsignarPeticion_categoria_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_categoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        RestaurarDropDownListAsignar(3);
    }
    protected void DropDownListAsignarPeticion_subcategoria_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_subcategoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_subcategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        RestaurarDropDownListAsignar(4);
        DataBindDropDownListAsignarPeticion_peticion();
    }

    protected void DropDownListAsignarPeticion_peticion_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_peticion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_peticion_SelectedIndexChanged(object sender, EventArgs e)
    {
        MostrarAlertPeticion();
        DataBindDropDownListAsignarPeticion_peticion();

        DivAsignarPeticion_asignaciones.Visible = true;
        DivAsignarPeticion_unidades.Visible = true;
        DataBingGridViewUnidadesAdministrativas();

    }
    public void DataBindDropDownListAsignarPeticion_peticion()
    {
        string andListId = "";
        string stringListId = HiddenFieldDivPeticiones_selected.Value;
        string categoriaId = DropDownListAsignarPeticion_categoria.SelectedValue.ToString();
        string subcategoriaId = DropDownListAsignarPeticion_subcategoria.SelectedValue.ToString();
        string pliegoId = DropDownListAsignarPeticion_pliego.SelectedValue.ToString();

        if (!String.IsNullOrEmpty(stringListId))
        {
            andListId = "and pet.ID_PETICION not in("+ stringListId +")";
        }

        string query = "select pet.ID_PETICION, pet.DESC_PETICION " +
                                "from PETICIONES pet " +
                                "where pet.ID_CAT_PETICION = '"+ categoriaId +"' and ID_PLIEGO = '"+ pliegoId +"' and pet.ID_SUBCAT_PETICION = '"+ subcategoriaId +"' "+ andListId +"";

        using (SqlConnection con = new SqlConnection(constr))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(query, con))
            {
                try
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    this.DropDownListAsignarPeticion_peticion.DataSource = ds;
                    this.DropDownListAsignarPeticion_peticion.DataValueField = "ID_PETICION";
                    this.DropDownListAsignarPeticion_peticion.DataTextField = "DESC_PETICION";
                    this.DropDownListAsignarPeticion_peticion.DataBind();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }

    }
    private void MostrarAlertPeticion()
    {

        List<int> intListId = AgregarListaPeticionesId();

        DivContenidoPeticiones_seleccionadas.InnerHtml = string.Empty;

        if (intListId.Count >= 1)
        {
            string contenido = "";

            foreach (int peti in intListId)
            {
                string descripcion = ObtenerIdPeticion_descripcion(peti);

                contenido += "<div class='col-auto' id='DivAlert_"+ peti +"'> " +
                                "<div class='alert alert-warning alert-dismissible fade show' role='alert'> " +
                                    ""+ descripcion +"" +
                                    "<button type='button' class='btn-close' aria-label='Close' onclick='eliminarIdPeticion("+ peti +");'></button> " +
                                "</div> " +
                            "</div>";
            }

            DivContenidoPeticiones_seleccionadas.InnerHtml = contenido;
        }
        else
        {
            string peticionAct = HiddenFieldDivPeticiones_selected.Value;
            
            if (!String.IsNullOrEmpty(peticionAct))
            {
                List<string> peticionIdString = peticionAct.Split(',').ToList();

                string contenido = "";

                foreach (string peti in peticionIdString)
                {

                    string descripcion = ObtenerIdPeticion_descripcion(Convert.ToInt32(peti));

                    contenido += "<div class='col-auto' id='DivAlert_"+ peti +"'> " +
                                    "<div class='alert alert-warning alert-dismissible fade show' role='alert'> " +
                                        ""+ descripcion +"" +
                                        "<button type='button' class='btn-close' aria-label='Close' onclick='eliminarIdPeticion("+ peti +");'></button> " +
                                    "</div> " +
                                "</div>";
                }

                DivContenidoPeticiones_seleccionadas.InnerHtml = contenido;
            }
            else
            {
                DivAsignarPeticion_asignaciones.Visible = false;
                DivAsignarPeticion_unidades.Visible = false;
                HiddenFieldDivUnidad_selected.Value = string.Empty;

                LabelRegistrarGarantiaNotificacion_responsable.Text = string.Empty;
                TextBoxRegistrarGarantiaNotificacion_correo.Text = string.Empty;
                DivRegistrarGarantiaNotificacion_contenido.Visible = false;
            }

        }

    }
    private void EliminarListaPeticionesId(string idPeticion)
    {
        string peticionAct = HiddenFieldDivPeticiones_selected.Value;

        if (!String.IsNullOrEmpty(peticionAct))
        {
            List<string> peticionIdString = peticionAct.Split(',').ToList();
            peticionIdString.Remove(idPeticion);

            string stringListId = string.Join(",", peticionIdString);

            HiddenFieldDivPeticiones_selected.Value = stringListId;

            MostrarAlertPeticion();
            DataBindDropDownListAsignarPeticion_peticion();
        }

    }
    private List<int> AgregarListaPeticionesId()
    {
        string peticionId = DropDownListAsignarPeticion_peticion.SelectedValue.ToString();
        string peticionAct = HiddenFieldDivPeticiones_selected.Value;

        List<int> intListId = new List<int>();

        if (!String.IsNullOrEmpty(peticionId))
        {
            int peticionIdInt = Convert.ToInt32(DropDownListAsignarPeticion_peticion.SelectedValue);

            intListId.Add(peticionIdInt);

            if (!String.IsNullOrEmpty(peticionAct))
            {

                string[] stringId = peticionAct.Split(',');

                foreach (var id in stringId)
                {
                    int intId = Convert.ToInt32(id);
                    intListId.Add(intId);
                }

            }

            string stringListId = string.Join(",", intListId);
            HiddenFieldDivPeticiones_selected.Value = stringListId;
        }
        return intListId;
    }
    private string ObtenerIdPeticion_descripcion(int idPeticion)
    {
        return Consultas.ConsultaS("select CONCAT(SUBSTRING(DESC_PETICION, 0,10),'...') DESCRIPCION from PETICIONES where ID_PETICION = '"+ idPeticion +"'");
    }
    protected void LinkButtonPeticionId_eliminar_Click(object sender, EventArgs e)
    {
        string idPeticion = HiddenFieldPeticionEliminar_id.Value;
        EliminarListaPeticionesId(idPeticion);
    }


    private void DataBingGridViewUnidadesAdministrativas()
    {
        string zp = LabelZP.Text;

        string qryNS = "select ID_PERFIL, " +
                         "case " +
                             "when DESCRIPCION like 'JEFE DEL %' then REPLACE(DESCRIPCION,'JEFE DEL ','') " +
                             "when DESCRIPCION like 'JEFE DE LA %' then REPLACE(DESCRIPCION,'JEFE DE LA ','') " +
                             "else DESCRIPCION " +
                         "end UNIDAD " +
                    "from CAT_PERFILES " +
                    "where CLAVE_ZP = '"+ zp +"' or ID_PERFIL in(11,12,13,14)";

        using (SqlConnection con = new SqlConnection(constr))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    this.GridViewUnidadesAdministrativas.DataSource = dt;
                    GridViewUnidadesAdministrativas.DataBind();
                    GridViewUnidadesAdministrativas.PageIndex = 0;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }

    protected void LinkButtonUnidadesAdministrativas_selecionar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string idPerfil = btn.CommandArgument;

        HiddenFieldDivUnidad_selected.Value = idPerfil;

        GridViewRow rowGV = (GridViewRow)btn.NamingContainer;
        GridView gv = (GridView)rowGV.NamingContainer;

        foreach (GridViewRow row in gv.Rows)
        {
            LinkButton btnGV = ((LinkButton)row.FindControl("LinkButtonUnidadesAdministrativas_selecionar"));
            btnGV.Text = "Seleccionar";
            btnGV.CssClass = "btn btn-sm btn-outline-danger LoadingOverlay";
            row.BackColor = System.Drawing.Color.White;

            string idPerfilRow = btnGV.CommandArgument;
            int asignado = Consultas.ConsultaInt("select COUNT(ID_ASIGNACION) asignado from ASIGNACION_PETICION where CLAVE_ZP = '1751' and ID_PETICION = '332' and ID_PERFIL ='"+ idPerfilRow +"' and ESTATUS = 1");

            if (asignado == 1)
            {
                btnGV.Text = "Asignada";
                btnGV.CssClass = "btn btn-sm btn-outline-warning disabled LoadingOverlay";
                row.BackColor = System.Drawing.Color.White;
            }
        }


        btn.Text = "Seleccionado";
        btn.CssClass = "btn btn-sm btn-outline-success disabled LoadingOverlay";
        rowGV.BackColor = System.Drawing.Color.LightGray;

        string zp = LabelZP.Text;
        int perfilExist = Consultas.ConsultaInt("select COUNT(ID_USER) total from AUTORIDADES_ZP where CLAVE_ZP = '"+ zp +"' and ID_PERFIL = '"+ idPerfil +"' and ESTATUS = 1");

        if (perfilExist == 1)
        {
            int idUsuario = Consultas.ConsultaInt("select ID_USER from AUTORIDADES_ZP where CLAVE_ZP = '"+ zp +"' and ID_PERFIL = '"+ idPerfil +"' and ESTATUS = 1");

            LabelRegistrarGarantiaNotificacion_responsable.Text = Consultas.ConsultaS("select CONCAT(NOMBRE,' ',APELLIDO_PAT, ' ', APELLIDO_MAT) NOMBRE from USERS where ID_USER = '"+ idUsuario +"'");
            TextBoxRegistrarGarantiaNotificacion_correo.Text = Consultas.ConsultaS("select CORREO from AUTORIDADES_ZP where CLAVE_ZP ='"+ zp +"' and ID_PERFIL = '"+ idPerfil +"' and ESTATUS = 1");
        }
        else
        {
            LabelRegistrarGarantiaNotificacion_responsable.Text = "Sin datos registrados";
            TextBoxRegistrarGarantiaNotificacion_correo.Text = string.Empty;
        }

            DivRegistrarGarantiaNotificacion_contenido.Visible = true;

    }
    private void InsertarAsignacion()
    {
        string zp = LabelZP.Text;
        string peticionId = HiddenFieldDivPeticiones_selected.Value;
        string unidad = HiddenFieldDivUnidad_selected.Value;
        string descUnidad = Consultas.ConsultaS("select DESCRIPCION from CAT_PERFILES  where ID_PERFIL = '"+ unidad +"'");
        string correo = TextBoxRegistrarGarantiaNotificacion_correo.Text;

        int idAsignacion = ObtenerIdAsignacion_siguiente();

        if (!String.IsNullOrEmpty(peticionId))
        {

            List<string> stringId = peticionId.Split(',').ToList();

            foreach (var id in stringId)
            {
                int intId = Convert.ToInt32(id);
                string limite = Consultas.ConsultaS("select IIF(FECHA_CUMPLIMIENTO is null, 'sin dato registrado', FORMAT(FECHA_CUMPLIMIENTO ,'dddd dd MMMM, yyyy', 'es-ES'))LIMITE from PETICIONES where ID_PETICION = '"+ intId +"'");

                Consultas.miInsert("insert into ASIGNACION_PETICION (ID_ASIGNACION, CLAVE_ZP, ID_PETICION, ID_PERFIL, DESC_UNIDAD) values('"+ idAsignacion +"','"+ zp +"','"+ intId +"','"+ unidad +"','"+ descUnidad +"')");

                string descPeticion = Consultas.ConsultaS("select DESC_PETICION from PETICIONES where ID_PETICION = '"+ intId +"'");
                enviarCorreoAsignacion(correo, DropDownListAsignarPeticion_pliego.SelectedItem.Text, DropDownListAsignarPeticion_categoria.SelectedItem.Text, DropDownListAsignarPeticion_subcategoria.SelectedItem.Text, descPeticion, limite);
            }

        }

    }
    private int ObtenerIdAsignacion_siguiente()
    {
        return Consultas.ConsultaInt("select COUNT(distinct ID_ASIGNACION) + 1 ID from ASIGNACION_PETICION");
    }
    public bool enviarCorreoAsignacion(string destino, string plie, string cate, string subc, string peti, string lim)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        var mail = new MailMessage();
        var smtp = new SmtpClient();

        string from = "saiee.i.p.n.m.x@gmail.com";
        string fromAlias = "sades@ipn.mx";
        string password = "tjzq ixji anst dccv";

        string html = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n    <title></title>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n    <style type=\"text/css\">\r\n        @media screen {\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local(\\'Lato Regular\\'), local(\\'Lato-Regular\\'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format(\\'woff\\');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local(\\'Lato Bold\\'), local(\\'Lato-Bold\\'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format(\\'woff\\');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: italic;\r\n                font-weight: 400;\r\n                src: local(\\'Lato Italic\\'), local(\\'Lato-Italic\\'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format(\\'woff\\');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: italic;\r\n                font-weight: 700;\r\n                src: local(\\'Lato Bold Italic\\'), local(\\'Lato-BoldItalic\\'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format(\\'woff\\');\r\n            }\r\n        }\r\n\r\n        /* CLIENT-SPECIFIC STYLES */\r\n        body,\r\n        table,\r\n        td,\r\n        a {\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n        }\r\n\r\n        table,\r\n        td {\r\n            mso-table-lspace: 0pt;\r\n            mso-table-rspace: 0pt;\r\n        }\r\n\r\n        img {\r\n            -ms-interpolation-mode: bicubic;\r\n        }\r\n\r\n        /* RESET STYLES */\r\n        img {\r\n            border: 0;\r\n            height: auto;\r\n            line-height: 100%;\r\n            outline: none;\r\n            text-decoration: none;\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse !important;\r\n        }\r\n\r\n        body {\r\n            height: 100% !important;\r\n            margin: 0 !important;\r\n            padding: 0 !important;\r\n            width: 100% !important;\r\n        }\r\n\r\n        /* iOS BLUE LINKS */\r\n        a[x-apple-data-detectors] {\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }\r\n\r\n        /* MOBILE STYLES */\r\n        @media screen and (max-width:600px) {\r\n            h1 {\r\n                font-size: 32px !important;\r\n                line-height: 32px !important;\r\n            }\r\n        }\r\n\r\n        /* ANDROID CENTER FIX */\r\n        div[style*=\"margin: 16px 0;\"] {\r\n            margin: 0 !important;\r\n        }\r\n\r\n    </style>\r\n</head>\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n        <!-- LOGO -->\r\n        <tr>\r\n            <td bgcolor=\"#872456\" align=\"center\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#872456\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\" style=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n                            <h1 style=\"font-size: 40px; font-weight: 400; margin: 2;\">Asignación de petición!</h1> <img src=\"http://148.204.112.186:8081/public/img/sadesCorreo.webp\" width=\"125\" height=\"120\" style=\"display: block; border: 0px;\" />\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 40px 30px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <p style=\"margin: 0;\">Bienvenido al Sistema de Apoyo para la Dirección de Educación Superior <strong>SADES</strong>.</p>\r\n                            <br>\r\n                            <p style=\"margin: 0;\">Una vez realizado el análisis al contenido de la solicitud realizada a la Unidad Académica y considerando el ámbito de atribución de la Unidad Administrativa que se encuentra a su cargo, le ha sido asignada la siguiente solicitud. .</p><br></td></tr> " +
            "<tr> " +
            "<td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 0px 30px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\"> " +
            "<table style=\"width: 85%\" border=\"1\" align=\"center\"> " +
                "<tr > " +
                    "<th >Pliego</th> " +
                    "<th >Categoría</th> " +
                    "<th >Subcategoría</th> " +
                    "<th >Petición</th> " +
                    "<th >Límite para atención</th> " +
                "</tr> " +
                "<tr align=\"center\"> " +
                    "<td >"+ plie +"</td> " +
                    "<td >"+ cate +"</td> " +
                    "<td >"+ subc +"</td> " +
                    "<td >"+ peti +"</td> " +
                    "<td >"+ lim +"</td> " +
                "</tr> " +
            "</table> " +
            "</td> " +
            "</tr> " +
            "<tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\">\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                <tr>\r\n                                    <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 40px 30px 10px 30px;\"> " +
            "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> " +
                "<tr> " +
                    "<td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#872456\"><a href=\"http://148.204.112.186:8081/\" target=\"_blank\" style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #872456; display: inline-block;\"> " +
                            "SADES | Ingresar</a> " +
                        "</td> " +
                "</tr> " +
            "</table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <img src=\"https://ipn.mx/assets/files/imageninstitucional/img/identidad/logotipos/portada-vertical.jpg\" height=\"200\" style=\"display: block; border: 0px;\" />\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#f4f4f4\" align=\"left\" style=\"padding: 0px 30px 30px 30px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 18px;\">\r\n                            <br>\r\n                            <p style=\"margin: 0;\">Secretaría Académica | Instituto Politécnico Nacional <a href=\"https://www.ipn.mx/seacademica/\" target=\"_blank\" style=\"color: #111111; font-weight: 700;\">(SECACADEMICA)</a>.</p>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";

        try
        {
            List<string> destinos = destino.Split(',').ToList();

            foreach (var item in destinos)
            {
                mail.To.Add(item);
            }
            
            mail.From = new MailAddress(fromAlias);

            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Subject = "SADES - Asignación de petición ";
            mail.Body = html;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            smtp.Host = ("smtp.gmail.com");
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(from, password);
            smtp.EnableSsl = true;

            smtp.Send(mail);

            LabelEnvioCorreo_estatus.Text = "El correo se envió correctamente";
            return true;

        }
        catch (Exception e)
        {
            LabelEnvioCorreo_estatus.Text = e.ToString();
            return false;
        }

    }
    protected void LinkButtonAsignarPeticion_guardar_Click(object sender, EventArgs e)
    {
        InsertarAsignacion();
        if (!String.IsNullOrEmpty(zp))
        {
            DropDownListAsignarPeticion_pliego.DataBind();
            RestaurarDropDownListAsignar(2);
        }
        else
        {
            RestaurarDropDownListAsignar(0);
        }
        ActualizarEstadisticas();
        HiddenFieldMensajeRegistroExitoso_estatus.Value = "1";


    }



    protected void LinkButtonResumenAsignaciones_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalResumenAsignaciones";
        string zp = LabelZP.Text;

        LabelModalResumenAsignaciones_titulo.Text = "Resumen de asignaciónes";
        LabelModalResumenAsignaciones_subtitulo.Text = String.IsNullOrEmpty(zp) == true ? emptyZP_name : GetUaDesciption(zp); ;

        ShowModal(IdModal);
        DataBingGridViewResumenAsignaciones();
    }
    private void DataBingGridViewResumenAsignaciones()
    {
        string zp = LabelZP.Text;

        string qryNS = "select pli.FOLIO_PLIEGO, dep.DESCRIPCION_DP, asig.DESC_UNIDAD, pet.DESC_PETICION " +
                        "from ASIGNACION_PETICION asig " +
                        "inner join CAT_DEPENDENCIAS_POLITECNICAS dep on dep.CLAVE_ZP = asig.CLAVE_ZP " +
                        "inner join PETICIONES pet on pet.ID_PETICION = asig.ID_PETICION " +
                        "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                        "where asig.CLAVE_ZP like '"+ zp +"%'";

        using (SqlConnection con = new SqlConnection(constr))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    this.GridViewResumenAsignaciones.DataSource = dt;
                    GridViewResumenAsignaciones.DataBind();
                    GridViewResumenAsignaciones.PageIndex = 0;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }
    protected void LinkButtonModalResumenAsignaciones_Excel_Click(object sender, EventArgs e)
    {

    }




    protected void GridViewUnidadesAdministrativas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int idPerfil = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ID_PERFIL"));
        int asignado = Consultas.ConsultaInt("select COUNT(ID_ASIGNACION) asignado from ASIGNACION_PETICION where CLAVE_ZP = '1751' and ID_PETICION = '332' and ID_PERFIL ='"+ idPerfil +"' and ESTATUS = 1");

        if (asignado == 1)
        {
            LinkButton btnGV = ((LinkButton)e.Row.FindControl("LinkButtonUnidadesAdministrativas_selecionar"));
            btnGV.Text = "Asignada";
            btnGV.CssClass = "btn btn-sm btn-outline-warning disabled LoadingOverlay";
            e.Row.BackColor = System.Drawing.Color.White;
        }

    }
}