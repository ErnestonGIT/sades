using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                                                    "from PETICIONES_POR_UA pet " +
                                                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                                                    "where pet.ID_PETICION not in (select distinct ID_PETICION from GARANTIA_PETICION) and pli.CLAVE_ZP like '"+ zp +"%'");

        string totalAsignadas = Consultas.ConsultaS("select count(distinct ID_PETICION) total from GARANTIA_PETICION where ESTATUS = 1  and CLAVE_ZP like '"+ zp +"%'");

        string totalAsignaciones = Consultas.ConsultaS("select COUNT(distinct gar.ID_PETICION) AS 'value',cat_pet.DESCRIPCION_CAT_PETICION AS 'name' " +
                                                    "from GARANTIA_PETICION gar " +
                                                    "inner join CAT_CATEGORIA_PETICION cat_pet on cat_pet.ID_CAT_PETICION = gar.ID_CAT_PETICION " +
                                                    "inner join PLIEGO pli on pli.ID_PLIEGO = gar.ID_PLIEGO " +
                                                    "where pli.CLAVE_ZP like '"+ zp +"%' " +
                                                    "group by cat_pet.DESCRIPCION_CAT_PETICION " +
                                                    "for JSON PATH ");


        HiddenFieldGraficoPiePeticiones_datos.Value = totalAsignaciones;
        LabelPeticionesPorAsignar_total.Text = totalPorAsignar == "0" ? "0" : totalPorAsignar;
        LabelPeticionesAsignadas_total.Text = totalAsignadas == "0" ? "0" : totalAsignadas;
    }

    private void RestaurarDropDownListGarantias(int nivel)
    {
        switch (nivel)
        {
            case 0:
                DropDownListAsignarPeticion_ua.DataBind();
                ClearAndInsertItem(DropDownListAsignarPeticion_pliego);
                ClearAndInsertItem(DropDownListAsignarPeticion_categoria);
                ClearAndInsertItem(DropDownListAsignarPeticion_peticion);
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
        DivContenidoPeticiones_seleccionadas.InnerHtml = string.Empty;

        HiddenFieldMensajeRegistroExitoso_estatus.Value = string.Empty;
        HiddenFieldPeticionEliminar_id.Value = string.Empty;
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
        ActualizarEstadisticas(); ;
    }
    protected void DropDownListAsignarPeticion_pliego_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_pliego.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_pliego_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void DropDownListAsignarPeticion_categoria_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_categoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ButtonRegistrarAsignarPeticion_guardar_Click(object sender, EventArgs e)
    {

    }

    protected void DropDownListAsignarPeticion_unidad_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_unidad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListAsignarPeticion_unidad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListAsignarPeticion_peticion_DataBound(object sender, EventArgs e)
    {
        DropDownListAsignarPeticion_peticion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListAsignarPeticion_peticion_SelectedIndexChanged(object sender, EventArgs e)
    {
        MostrarAlertPeticion();
        DataBindDropDownListAsignarPeticion_peticion();

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
                                "from PETICIONES_POR_UA pet " +
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
    private int ObtenerIdPeticion_siguiente()
    {
        return Consultas.ConsultaInt("select COUNT(distinct ID_GARANTIA) + 1 ID from GARANTIA_PETICION");
    }
    private string ObtenerIdPeticion_descripcion(int idPeticion)
    {
        return Consultas.ConsultaS("select CONCAT(SUBSTRING(DESC_PETICION, 0,10),'...') DESCRIPCION from PETICIONES_POR_UA where ID_PETICION = '"+ idPeticion +"'");
    }

    protected void LinkButtonPeticionId_eliminar_Click(object sender, EventArgs e)
    {
        string idPeticion = HiddenFieldPeticionEliminar_id.Value;
        EliminarListaPeticionesId(idPeticion);
    }
}