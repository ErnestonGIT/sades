using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PeticionEstatus : System.Web.UI.Page
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
        HiddenFieldCollapseEstatusPeticion_selected.Value  = "1";
        divPanelEstatusPeticion.Visible = data;

        ActualizarEstadisticas();
    }
    public void mostrarPanelEnlace(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;

        HiddenFieldCollapseEstatusPeticion_selected.Value  = "1";
        divPanelEstatusPeticion.Visible = data;

        ActualizarEstadisticas();
    }
    public void mostrarPanelAdministrador(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseEstatusPeticion_selected.Value  = "1";
        divPanelEstatusPeticion.Visible = data;
        ActualizarEstadisticas();
    }
    public void ActualizarEstadisticas()
    {
        string zp = LabelZP.Text;
        string cate = DropDownListEstatusPeticion_categoria.SelectedValue.ToString();

        LabelBreadCrumbZP_name.Text = String.IsNullOrEmpty(zp) == true ? emptyZP_name : GetUaDesciption(zp);

        LabelTotalPeticiones_pendientes.Text = Consultas.ConsultaS("select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where ID_EST_PETICION = 1 and pli.CLAVE_ZP like '"+ zp +"%' and ID_CAT_PETICION like '"+ cate +"%'");
        LabelTotalPeticiones_proceso.Text = Consultas.ConsultaS("select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where ID_EST_PETICION = 2 and pli.CLAVE_ZP like '"+ zp +"%' and ID_CAT_PETICION like '"+ cate +"%'");
        LabelTotalPeticiones_atendidas.Text = Consultas.ConsultaS("select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where ID_EST_PETICION = 3 and pli.CLAVE_ZP like '"+ zp +"%' and ID_CAT_PETICION like '"+ cate +"%'");

        DataBindGridViewDetallePeticiones();
    }

    public void DataBindGridViewDetallePeticiones()
    {
        string zp = LabelZP.Text;
        string cate = DropDownListEstatusPeticion_categoria.SelectedValue.ToString();

        string totalRows;

        string qry = "select pli.CLAVE_ZP, pet.ID_PETICION, pet_e.DESCRIPCION_PETICION, pet_d.DESCRIPCION_CAT_PETICION, pet.DESC_PETICION, pet.DESC_RESP_PETICION, " +
                    "FORMAT(pet.FECHA_PETICION,'d-MM-yyyy') FECHA_INICIO, " +
					"FORMAT(pet.FECHA_RESP_PETICION,'d-MM-yyyy') FECHA_FIN " +
                    "from PETICIONES pet " +
                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                    "inner join CAT_CATEGORIA_PETICION pet_d on pet_d.ID_CAT_PETICION = pet.ID_CAT_PETICION " +
                    "inner join ESTATUS_PETICION pet_e on pet_e.ID_EST_PETICION = pet.ID_EST_PETICION " +
                    "where pli.CLAVE_ZP like '"+ zp +"%' and pet.ID_CAT_PETICION like '"+ cate +"%'";

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlDataAdapter da = new SqlDataAdapter(qry, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    totalRows = dt.Rows.Count.ToString();

                    this.GridViewDetallePeticiones.DataSource = dt;

                    GridViewDetallePeticiones.DataBind();
                    GridViewDetallePeticiones.PageIndex = 0;

                    if(!String.IsNullOrEmpty(zp) || !String.IsNullOrEmpty(cate))
                    {
                        LabelEstatusPeticionFiltro_total.Text = totalRows;
                    }
                    else
                    {
                        LabelEstatusPeticionFiltro_total.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            con.Close();
        }

    }
    protected void DropDownListEstatusPeticion_ua_DataBound(object sender, EventArgs e)
    {
        DropDownListEstatusPeticion_ua.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Unidad académica", ""));
    }
    protected void DropDownListEstatusPeticion_ua_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelZP.Text = DropDownListEstatusPeticion_ua.SelectedValue.ToString();
        RestaurarDropDownListEstatus(1);
        DropDownListEstatusPeticion_categoria.DataBind();
        ActualizarEstadisticas();
    }


    protected void DropDownListEstatusPeticion_categoria_DataBound(object sender, EventArgs e)
    {
        DropDownListEstatusPeticion_categoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Categoría", ""));
    }
    protected void DropDownListEstatusPeticion_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        ActualizarEstadisticas();
    }


    private void RestaurarDropDownListEstatus(int nivel)
    {
        switch (nivel)
        {
            case 0:
                DropDownListEstatusPeticion_ua.DataBind();
                ClearAndInsertItem(DropDownListEstatusPeticion_categoria);
                LabelZP.Text = string.Empty;
                break;
            case 1:
                ClearAndInsertItem(DropDownListEstatusPeticion_categoria);
                break;
        }

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
    protected void LinkButtonFiltroEstatusPeticion_limpiar_Click(object sender, EventArgs e)
    {
        RestaurarDropDownListEstatus(0);
        ActualizarEstadisticas();
    }



    [WebMethod]
    public static List<object> chartGanttPeticiones_datos(string zp)
    {
        List<object> chartData = new List<object>();

        chartData.Add(new object[]
            {
                "VIGENTES", "VENCIDOS", "VACANTES"
            });

        string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

        bool exist = Consultas.ConsultaInt("select COUNT(ID_USER) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%'") == 0 ? false : true;

        string query = "select SUM(VIGENTES) VIGENTES, SUM(VENCIDOS) VENCIDOS, SUM(VACANTES) VACANTES " +
                        "FROM( " +
                            "select 0 VIGENTES, 0 VENCIDOS, 0 VACANTES " +
                            "from AUTORIDADES_ZP " +
                        ")DATOS";
        if (exist == true)
        {
            query = "select SUM(VIGENTES) VIGENTES, SUM(VENCIDOS) VENCIDOS, SUM(VACANTES) VACANTES " +
                    "from( " +
                        "select dp.CLAVE_ZP, " +
                            "(select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and FECHA_FIN > (select FORMAT(GETDATE(), 'yyyy-M-dd')) and ESTATUS = 1) VIGENTES ,  " +
                            "(select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and FECHA_FIN< (select FORMAT(GETDATE(),'yyyy-M-dd')) and ESTATUS = 1) VENCIDOS ,  " +
                            "(select((select COUNT(distinct ID_PERFIL) total from AUTORIDADES_ZP) - COUNT(ID_PERFIL)) TOTAL from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and ESTATUS = 1) VACANTES " +
                        "from CAT_DEPENDENCIAS_POLITECNICAS dp " +
                        "where dp.ID_NIVEL_EST = 2 " +
                    ")datos " +
                    "where CLAVE_ZP like '"+ zp +"%'";
        }

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {

                    while (sdr.Read())
                    {

                        chartData.Add(new object[]
                        {
                            sdr["VIGENTES"],sdr["VENCIDOS"],sdr["VACANTES"]
                        });

                    }

                }
                con.Close();
                return chartData;
            }
        }

    }
}