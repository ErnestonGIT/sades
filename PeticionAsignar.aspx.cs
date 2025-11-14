using DocumentFormat.OpenXml.Office2010.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;
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
        string nivel = HiddenFieldPerfil_nivel.Value;

        HiddenFieldCollapseAsignarPeticion_selected.Value  = "1";
        divPanelAsignarPeticion.Visible = data;

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
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                LabelZP.Text = string.Empty;
                break;
            case 1:
                ClearAndInsertItem(DropDownListAsignarPeticion_pliego);
                ClearAndInsertItem(DropDownListAsignarPeticion_categoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
            case 2:
                ClearAndInsertItem(DropDownListAsignarPeticion_categoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
            case 3:
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
                break;
        }

        HiddenFieldDivPeticiones_selected.Value = string.Empty;
        HiddenFieldDivUnidad_selected.Value = string.Empty;
        DivContenidoPeticiones_seleccionadas.InnerHtml = string.Empty;

        HiddenFieldMensajeRegistroExitoso_estatus.Value = string.Empty;
        HiddenFieldPeticionEliminar_id.Value = string.Empty;

        DivAsignarPeticion_asignaciones.Visible = false;
        DivAsignarPeticion_unidades.Visible = false;
    }
    private void ClearAndInsertItem(DropDownList dropDownList)
    {
        dropDownList.ClearSelection();
        dropDownList.Items.Clear();
        if (!dropDownList.Items.Contains(new ListItem("Seleccionar", "")))
        {
            dropDownList.Items.Insert(0, new ListItem("Seleccionar", ""));
        }
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
        string pliegoId = DropDownListAsignarPeticion_pliego.SelectedValue.ToString();

        if (!String.IsNullOrEmpty(stringListId))
        {
            andListId = "and pet.ID_PETICION not in("+ stringListId +")";
        }

        string query = "select pet.ID_PETICION, pet.DESC_PETICION " +
                                "from PETICIONES pet " +
                                "where pet.ID_CAT_PETICION = '"+ categoriaId +"' and ID_PLIEGO = '"+ pliegoId +"' "+ andListId +"";

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
        }


        btn.Text = "Seleccionado";
        btn.CssClass = "btn btn-sm btn-outline-success LoadingOverlay";
        rowGV.BackColor = System.Drawing.Color.LightGray;

    }
    private void InsertarAsignacion()
    {
        string zp = LabelZP.Text;
        string peticionId = HiddenFieldDivPeticiones_selected.Value;
        string unidad = HiddenFieldDivUnidad_selected.Value;
        string descUnidad = Consultas.ConsultaS("select DESCRIPCION from CAT_PERFILES  where ID_PERFIL = '"+ unidad +"'");

        int idAsignacion = ObtenerIdAsignacion_siguiente();

        if (!String.IsNullOrEmpty(peticionId))
        {

            List<string> stringId = peticionId.Split(',').ToList();

            foreach (var id in stringId)
            {
                int intId = Convert.ToInt32(id);
                Consultas.miInsert("insert into ASIGNACION_PETICION (ID_ASIGNACION, CLAVE_ZP, ID_PETICION, ID_PERFIL, DESC_UNIDAD) values('"+ idAsignacion +"','"+ zp +"','"+ intId +"','"+ unidad +"','"+ descUnidad +"')");
            }

        }

    }
    private int ObtenerIdAsignacion_siguiente()
    {
        return Consultas.ConsultaInt("select COUNT(distinct ID_ASIGNACION) + 1 ID from ASIGNACION_PETICION");
    }
    protected void LinkButtonAsignarPeticion_guardar_Click(object sender, EventArgs e)
    {
        InsertarAsignacion();

        RestaurarDropDownListAsignar(0);
        ActualizarEstadisticas();
        HiddenFieldMensajeRegistroExitoso_estatus.Value = "1";
    }
    /*
        CLAVE_ZP int NULL,
        ID_PETICION int NULL,
        ID_PERFIL int NULL,
        DESC_UNIDAD nvarchar(500), 
     */
}