using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;//System.Web.UI.WebControls;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    string emptyZP_name = "Instituto Politécnico Nacional";

    string perfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();
    string idUser = HttpContext.Current.Request.Cookies["id_usuario"].Value.ToString();
    string chP = HttpContext.Current.Request.Cookies["chP"].Value.ToString();

    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString);

    int hrsCE_NS = 24;//horas carga educativa NS
    int hrsCE_NMS = 26;
    //Definición de perfiles

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //evita el error en la declaracion del form para la exportacion de datos  
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        string zp = HttpContext.Current.Request.Cookies["claveZP"].Value.ToString(); //HttpContext.Current.Request.QueryString["zp"];
        string pe = HttpContext.Current.Request.Cookies["pe"].Value.ToString();

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

    private void validarContenido(string perfil)
    {
        switch (perfil)
        {
            case "7":
                LabelZP.Text = string.Empty;
                mostrarPanelDependencias(true, "1");
                mostrarPanelEventos(true, "0");
                break;
            case "43":
                mostrarPanelInicial(true, "1");
                break;
            case "432"://super administrador
                mostrarPanelEventos(true, "0");
                mostrarPanelInicial(true, "0");
                mostrarPanelDependencias(true, "0");
                break;
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

    private void CaragarTablaWS_zp(string zp)
    {
        Consultas.Sentencia("exec WS_MATEXGRUPO @zp ='"+ zp +"'");
    }
    public void mostrarPanelInicial(bool data, string collapsed)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapsePlan_selected.Value = collapsed;
        divPanelInicial.Visible = data;

    }
    public void mostrarPanelEventos(bool data, string collapsed)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseEventos_selected.Value = collapsed;

        divPanelEventos.Visible = data;
        DivCardEventosRiesgo.Visible = data;
        DivCardEventosAtendidos.Visible = false;
        DivCardDetallePendientes.Visible = false;

        DataBindGridViewUnidadesAcademicasNS();
        datosPanelEvento();

    }
    private void datosPanelEvento()
    {
        LabelCardEventosRiesgo_total.Text =  Consultas.ConsultaS("select COUNT(distinct ID) total from EVENTOS_RIESGO where estatus =1");
        LabelCardEventosAtendidos_total.Text = "100 %";
        LabelCardDetallePendientes_total.Text = "0 %";
    }
    public void mostrarPanelDependencias(bool data, string collapsed)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseDependencias_selected.Value = collapsed;
        divPanelDependencias.Visible = data;
        DivDetalleDependencias_seccion.Visible = data;

        DataBindGridViewUnidadesAcademicasNS_resumen();
        AsignarValoresZP_resumen_seleccionada("");

    }


    public void DataBindDropDownPeriodos()
    {
        string zp = LabelZP.Text;
        string tipo = DropDownListPeriodo.SelectedValue.ToString();
        string whr = "";

        if (!String.IsNullOrEmpty(tipo))
        {
            whr = "where RIGHT(PERIODO_ESCOLAR,1) = '" + tipo + "'";
        }

        string query = "select * from( " +
                            "select distinct PERIODO_ESCOLAR from CARGA_DOCENTE where CLAVE_ZP = '" + zp + "' " +
                            "union " +
                            "select distinct PERIODO_ESCOLAR from HISTORIAL_CARGA_DOCENTE where CLAVE_ZP = '" + zp + "' " +
                            "union " +
                            "select distinct PERIODO_ESCOLAR from TEMP_WS_MATEXGRUPO where CLAVE_ZP = '" + zp + "' " +
                        ")datos " +
                        "" +
                        "" + whr + " " +
                        "group by PERIODO_ESCOLAR " +
                        "order by PERIODO_ESCOLAR desc";

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(query, con))
            {
                try
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    this.DropDownFiltro_periodo.DataSource = ds;
                    this.DropDownFiltro_periodo.DataValueField = "PERIODO_ESCOLAR";
                    this.DropDownFiltro_periodo.DataTextField = "PERIODO_ESCOLAR";
                    this.DropDownFiltro_periodo.DataBind();
                    this.DropDownFiltro_periodo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecciona un periodo", ""));

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }

    }
    protected void DropDownFiltro_periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string zp = LabelZP.Text;
        string pe = LabelPE.Text = DropDownFiltro_periodo.SelectedValue.ToString();

        if (!String.IsNullOrEmpty(pe))
        {
            GruposMapa_limpiar();
            GruposMapaGeneral();
            ShowModal("ModalMapaCalorDetalle");

            tittleModalMapaCalorDetalle.Text = "Mapa de calor de la unidad de la unidad académica";
            LabelModalMapaCalorDetalle_pe.Text = pe;
            LabelModalMapaCalorDetalle_zp.Text = DropDownListUnidadAcademica.SelectedItem.Text;
        }
        else
        {

        }
    }
    protected void GruposMapaGeneral()
    {
        string zp = DropDownListUnidadAcademica.SelectedValue.ToString();
        string pe = DropDownFiltro_periodo.SelectedValue.ToString();

        string tb = Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from TEMP_WS_MATEXGRUPO where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "TEMP_WS_MATEXGRUPO" : Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from CARGA_DOCENTE where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "CARGA_DOCENTE" : "HISTORIAL_CARGA_DOCENTE";

        LabeltotalLun.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and LUNES != ''");
        LabeltotalMar.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and MARTES != ''");
        LabeltotalMie.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and MIERCOLES != ''");
        LabeltotalJue.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and JUEVES != ''");
        LabeltotalVie.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and VIERNES != ''");
        totalFin.Text = "";
    }
    [WebMethod]
    public static List<object> GetChartMapaCalor(string zp, string pe)
    {

        string h1 = "07:00-07:30";
        string h2 = "07:30-08:00";
        string h3 = "08:00-08:30";
        string h4 = "08:30-09:00";
        string h5 = "09:00-09:30";
        string h6 = "09:30-10:00";
        string h7 = "10:00-10:30";
        string h8 = "10:30-11:00";
        string h9 = "11:00-11:30";
        string h10 = "11:30-12:00";
        string h11 = "12:00-12:30";
        string h12 = "12:30-13:00";
        string h13 = "13:00-13:30";
        string h14 = "13:30-14:00";
        string h15 = "14:00-14:30";
        string h16 = "14:30-15:00";
        string h17 = "15:00-15:30";
        string h18 = "15:30-16:00";
        string h19 = "16:00-16:30";
        string h20 = "16:30-17:00";
        string h21 = "17:00-17:30";
        string h22 = "17:30-18:00";
        string h23 = "18:00-18:30";
        string h24 = "18:30-19:00";
        string h25 = "19:00-19:30";
        string h26 = "19:30-20:00";
        string h27 = "20:00-20:30";
        string h28 = "20:30-21:00";
        string h29 = "21:00-21:30";
        string h30 = "21:30-22:00";

        string query = "";

        string tb = "";
        string lu = "select 1 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string ma = "union select 2 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string mi = "union select 3 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string ju = "union select 4 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string vi = "union select 5 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";


        List<object> chartData = new List<object>();

        chartData.Add(new object[]
            {
                "DIA","h1","h2","h3","h4","h5","h6","h7","h8","h9","h10","h11","h12","h13","h14","h15","h16","h17","h18","h19","h20","h21","h22","h23","h24","h25","h26","h27","h28","h29","h30"
            });

        string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
        
        tb = Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from TEMP_WS_MATEXGRUPO where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "TEMP_WS_MATEXGRUPO" : Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from CARGA_DOCENTE where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "CARGA_DOCENTE" : "HISTORIAL_CARGA_DOCENTE";

        lu = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (LUNES is not null and LUNES != '')") == 0 ? lu : "select 1 as DIA, cd.LUNES HORARIO, count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, LUNES FROM " + tb + ") as cd where cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.LUNES is not null and cd.LUNES != '')group by cd.LUNES ";
        ma = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (MARTES is not null and MARTES != '')") == 0 ? ma : "union select 2 as DIA, cd.MARTES HORARIO, count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, MARTES FROM " + tb + ") as cd where cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.MARTES is not null and cd.MARTES != '')group by cd.MARTES ";
        mi = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (MIERCOLES is not null and MIERCOLES != '')") == 0 ? mi : "union select 3 as DIA, cd.MIERCOLES HORARIO, count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, MIERCOLES FROM " + tb + ") as cd where cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.MIERCOLES is not null and cd.MIERCOLES != '')group by cd.MIERCOLES ";
        ju = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (JUEVES is not null and JUEVES != '')") == 0 ? ju : "union select 4 as DIA, cd.JUEVES HORARIO, count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, JUEVES FROM " + tb + ") as cd where cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.JUEVES is not null and cd.JUEVES != '')group by cd.JUEVES ";
        vi = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (VIERNES is not null and VIERNES != '')") == 0 ? vi : "union select 5 as DIA, cd.VIERNES HORARIO, count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, VIERNES FROM " + tb + ") as cd where cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.VIERNES is not null and cd.VIERNES != '')group by cd.VIERNES ";

        query = "select DIA,sum(h1) h1,sum(h2) h2,sum(h3) h3,sum(h4) h4,sum(h5) h5,sum(h6) h6,sum(h7) h7,sum(h8) h8,sum(h9) h9,sum(h10) h10,sum(h11) h11,sum(h12) h12,sum(h13) h13,sum(h14) h14,sum(h15) h15,sum(h16) h16,sum(h17) h17,sum(h18) h18,sum(h19) h19,sum(h20) h20,sum(h21) h21,sum(h22) h22,sum(h23) h23,sum(h24) h24,sum(h25) h25,sum(h26) h26,sum(h27) h27,sum(h28) h28,sum(h29) h29,sum(h30) h30 " +
                "from( ";
        query += lu;
        query += ma;
        query += mi;
        query += ju;
        query += vi;
        query += ") horarios group by DIA ";

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
                            sdr["DIA"],sdr["h1"],sdr["h2"],sdr["h3"],sdr["h4"],sdr["h5"],sdr["h6"],sdr["h7"],sdr["h8"],sdr["h9"],sdr["h10"],sdr["h11"],sdr["h12"],sdr["h13"],sdr["h14"],sdr["h15"],sdr["h16"],sdr["h17"],sdr["h18"],sdr["h19"],sdr["h20"],sdr["h21"],sdr["h22"],sdr["h23"],sdr["h24"],sdr["h25"],sdr["h26"],sdr["h27"],sdr["h28"],sdr["h29"],sdr["h30"]
                        });

                    }

                }
                con.Close();
                return chartData;
            }
        }

    }


    protected void LinkButtonGVocupabilidad_pe_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string pe = btn.CommandArgument;

        LabelPE.Text = pe;
        DropDownFiltro_periodo.SelectedIndex = 0;

        string zp = DropDownListUnidadAcademica.SelectedValue.ToString();
        // string mod = DropDownListModalidad.SelectedValue.ToString();
        string pac = DropDownListProgramaAcad.SelectedValue.ToString();
        string pes = DropDownListPlanEstudio.SelectedValue.ToString();
        string uap = HiddenFieldFiltro_ua.Value = DropDownListUnidadAprend.SelectedValue.ToString();

        string IdModal = "ModalMapaCalorDetalleUA";

        tittleModalMapaCalorDetalleUA.Text = "Mapa de calor de la unidad de aprendizaje";
        LabelModalMapaCalorDetalleUA_pe.Text = pe;
        LabelModalMapaCalorDetalleUA_zp.Text = DropDownListUnidadAcademica.SelectedItem.Text;
        // LabelModalMapaCalorDetalleUA_mod.Text = DropDownListModalidad.SelectedItem.Text;
        LabelModalMapaCalorDetalleUA_pac.Text = DropDownListProgramaAcad.SelectedItem.Text;
        LabelModalMapaCalorDetalleUA_pes.Text = DropDownListPlanEstudio.SelectedItem.Text;
        LabelModalMapaCalorDetalleUA_ua.Text = DropDownListUnidadAprend.SelectedItem.Text;

        GruposMapa_limpiar();
        GruposMapaUnidadAprendizaje();
        ShowModal(IdModal);

    }
    protected void GruposMapaUnidadAprendizaje()
    {
        string zp = DropDownListUnidadAcademica.SelectedValue.ToString();
        string pe = LabelPE.Text;
        string asi = DropDownListUnidadAprend.SelectedValue.ToString();

        string tb = Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from CARGA_DOCENTE where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "CARGA_DOCENTE" : "HISTORIAL_CARGA_DOCENTE";

        LabeltotalLun.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and LUNES != '' and ID_ASIGNATURA = '" + asi + "'");
        LabeltotalMar.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and MARTES != '' and ID_ASIGNATURA = '" + asi + "'");
        LabeltotalMie.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and MIERCOLES != '' and ID_ASIGNATURA = '" + asi + "'");
        LabeltotalJue.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and JUEVES != '' and ID_ASIGNATURA = '" + asi + "'");
        LabeltotalVie.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and VIERNES != '' and ID_ASIGNATURA = '" + asi + "'");
        totalFinUA.Text = "";
    }

    [WebMethod]
    public static List<object> GetChartMapaCalorUA(string zp, string pe, string ua)
    {

        string h1 = "07:00-07:30";
        string h2 = "07:30-08:00";
        string h3 = "08:00-08:30";
        string h4 = "08:30-09:00";
        string h5 = "09:00-09:30";
        string h6 = "09:30-10:00";
        string h7 = "10:00-10:30";
        string h8 = "10:30-11:00";
        string h9 = "11:00-11:30";
        string h10 = "11:30-12:00";
        string h11 = "12:00-12:30";
        string h12 = "12:30-13:00";
        string h13 = "13:00-13:30";
        string h14 = "13:30-14:00";
        string h15 = "14:00-14:30";
        string h16 = "14:30-15:00";
        string h17 = "15:00-15:30";
        string h18 = "15:30-16:00";
        string h19 = "16:00-16:30";
        string h20 = "16:30-17:00";
        string h21 = "17:00-17:30";
        string h22 = "17:30-18:00";
        string h23 = "18:00-18:30";
        string h24 = "18:30-19:00";
        string h25 = "19:00-19:30";
        string h26 = "19:30-20:00";
        string h27 = "20:00-20:30";
        string h28 = "20:30-21:00";
        string h29 = "21:00-21:30";
        string h30 = "21:30-22:00";

        string query = "";

        string tb = "";
        string lu = "select 1 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string ma = "union select 2 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string mi = "union select 3 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string ju = "union select 4 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";
        string vi = "union select 5 as DIA , '07:00-07:30' HORARIO, 0 as h1, 0 as h2,0 as h3, 0 as h4, 0 as h5, 0 as h6, 0 as h7, 0 as h8, 0 as h9, 0 as h10, 0 as h11, 0 as h12,0 as h13, 0 as h14, 0 as h15, 0 as h16, 0 as h17, 0 as h18, 0 as h19, 0 as h20, 0 as h21, 0 as h22,0 as h23, 0 as h24, 0 as h25, 0 as h26, 0 as h27, 0 as h28, 0 as h29, 0 as h30 ";

        List<object> chartData = new List<object>();

        chartData.Add(new object[]
            {
                "DIA","h1","h2","h3","h4","h5","h6","h7","h8","h9","h10","h11","h12","h13","h14","h15","h16","h17","h18","h19","h20","h21","h22","h23","h24","h25","h26","h27","h28","h29","h30"
            });

        string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

        tb = Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from CARGA_DOCENTE where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "CARGA_DOCENTE" : "HISTORIAL_CARGA_DOCENTE";

        lu = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where ID_ASIGNATURA = '" + ua + "' and CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (LUNES is not null and LUNES != '')") == 0 ? lu : "select 1 as DIA, cd.LUNES HORARIO, count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, LUNES FROM " + tb + ") as cd where cd.ID_ASIGNATURA = '" + ua + "' and cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.LUNES is not null and cd.LUNES != '')group by cd.LUNES ";
        ma = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where ID_ASIGNATURA = '" + ua + "' and CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (MARTES is not null and MARTES != '')") == 0 ? ma : "union select 2 as DIA, cd.MARTES HORARIO, count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, MARTES FROM " + tb + ") as cd where cd.ID_ASIGNATURA = '" + ua + "' and cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.MARTES is not null and cd.MARTES != '')group by cd.MARTES ";
        mi = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where ID_ASIGNATURA = '" + ua + "' and CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (MIERCOLES is not null and MIERCOLES != '')") == 0 ? mi : "union select 3 as DIA, cd.MIERCOLES HORARIO, count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, MIERCOLES FROM " + tb + ") as cd where cd.ID_ASIGNATURA = '" + ua + "' and cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.MIERCOLES is not null and cd.MIERCOLES != '')group by cd.MIERCOLES ";
        ju = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where ID_ASIGNATURA = '" + ua + "' and CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (JUEVES is not null and JUEVES != '')") == 0 ? ju : "union select 4 as DIA, cd.JUEVES HORARIO, count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, JUEVES FROM " + tb + ") as cd where cd.ID_ASIGNATURA = '" + ua + "' and cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.JUEVES is not null and cd.JUEVES != '')group by cd.JUEVES ";
        vi = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where ID_ASIGNATURA = '" + ua + "' and CLAVE_ZP = '" + zp + "' and PERIODO_ESCOLAR = '" + pe + "' and (VIERNES is not null and VIERNES != '')") == 0 ? vi : "union select 5 as DIA, cd.VIERNES HORARIO, count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h1 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h1 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h1 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h1 + "', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h2 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h2 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h2 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h2 + "', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h3 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h3 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h3 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h3 + "', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h4 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h4 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h4 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h4 + "', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h5 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h5 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h5 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h5 + "', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h6 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h6 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h6 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h6 + "', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h7 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h7 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h7 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h7 + "', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h8 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h8 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h8 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h8 + "', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h9 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h9 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h9 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h9 + "', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h10 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h10 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h10 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h10 + "', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h11 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h11 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h11 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h11 + "', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h12 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h12 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h12 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h12 + "', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h13 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h13 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h13 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h13 + "', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h14 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h14 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h14 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h14 + "', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h15 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h15 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h15 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h15 + "', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h16 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h16 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h16 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h16 + "', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h17 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h17 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h17 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h17 + "', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h18 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h18 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h18 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h18 + "', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h19 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h19 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h19 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h19 + "', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h20 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h20 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h20 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h20 + "', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h21 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h21 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h21 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h21 + "', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h22 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h22 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h22 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h22 + "', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h23 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h23 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h23 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h23 + "', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h24 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h24 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h24 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h24 + "', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h25 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h25 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h25 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h25 + "', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h26 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h26 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h26 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h26 + "', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h27 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h27 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h27 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h27 + "', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h28 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h28 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h28 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h28 + "', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h29 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h29 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h29 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h29 + "', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('" + h30 + "', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('" + h30 + "', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('" + h30 + "', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('" + h30 + "', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, VIERNES FROM " + tb + ") as cd where cd.ID_ASIGNATURA = '" + ua + "' and cd.CLAVE_ZP = '" + zp + "' and cd.PERIODO_ESCOLAR = '" + pe + "' and (cd.VIERNES is not null and cd.VIERNES != '')group by cd.VIERNES ";

        query = "select DIA,sum(h1) h1,sum(h2) h2,sum(h3) h3,sum(h4) h4,sum(h5) h5,sum(h6) h6,sum(h7) h7,sum(h8) h8,sum(h9) h9,sum(h10) h10,sum(h11) h11,sum(h12) h12,sum(h13) h13,sum(h14) h14,sum(h15) h15,sum(h16) h16,sum(h17) h17,sum(h18) h18,sum(h19) h19,sum(h20) h20,sum(h21) h21,sum(h22) h22,sum(h23) h23,sum(h24) h24,sum(h25) h25,sum(h26) h26,sum(h27) h27,sum(h28) h28,sum(h29) h29,sum(h30) h30 " +
                "from( ";
        query += lu;
        query += ma;
        query += mi;
        query += ju;
        query += vi;
        query += ") horarios group by DIA ";

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
                            sdr["DIA"],sdr["h1"],sdr["h2"],sdr["h3"],sdr["h4"],sdr["h5"],sdr["h6"],sdr["h7"],sdr["h8"],sdr["h9"],sdr["h10"],sdr["h11"],sdr["h12"],sdr["h13"],sdr["h14"],sdr["h15"],sdr["h16"],sdr["h17"],sdr["h18"],sdr["h19"],sdr["h20"],sdr["h21"],sdr["h22"],sdr["h23"],sdr["h24"],sdr["h25"],sdr["h26"],sdr["h27"],sdr["h28"],sdr["h29"],sdr["h30"]
                        });

                    }

                }
                con.Close();
                return chartData;
            }
        }

    }
    protected void GruposMapa_limpiar()
    {
        LabeltotalLun.Text = "";
        LabeltotalMar.Text = "";
        LabeltotalMie.Text = "";
        LabeltotalJue.Text = "";
        LabeltotalVie.Text = "";
        totalFinUA.Text = "";

    }

    /*************************************************************/
    // Función para mostrar una alerta en el cliente
    private void MostrarAlerta(string mensaje)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('" + mensaje + "');", true);
    }

    //Limpiar items de dropdownlist e insertar el item 0
    private void ClearAndInsertItem(DropDownList dropDownList)
    {
        dropDownList.ClearSelection();
        dropDownList.Items.Clear();
        if (!dropDownList.Items.Contains(new ListItem("Seleccionar", "")))
        {
            dropDownList.Items.Insert(0, new ListItem("Seleccionar", ""));
        }
    }

    protected void DropDownListUnidadAcademica_DataBound(object sender, EventArgs e)
    {
        DropDownListUnidadAcademica.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    //protected void DropDownListModalidad_DataBound(object sender, EventArgs e)
    //{
    //    DropDownListModalidad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    //}

    protected void DropDownListProgramaAcad_DataBound(object sender, EventArgs e)
    {
        DropDownListProgramaAcad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListPlanEstudio_DataBound(object sender, EventArgs e)
    {
        DropDownListPlanEstudio.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListUnidadAprend_DataBound(object sender, EventArgs e)
    {
        DropDownListUnidadAprend.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListUnidadAcademica_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelZP.Text = DropDownListUnidadAcademica.SelectedValue.ToString();
        //ClearAndInsertItem(DropDownListProgramaAcad);
        ClearAndInsertItem(DropDownListPlanEstudio);
        ClearAndInsertItem(DropDownListUnidadAprend);
        ClearCheckPeriodo();
        divCheckPeriodo.Visible = false;

        if (DropDownListUnidadAcademica.SelectedIndex == 0)
        {
            //ClearAndInsertItem(DropDownListModalidad);
            ClearAndInsertItem(DropDownListProgramaAcad);
        }

        ClearLblCalcularEstadisticos();
    }

    //protected void DropDownListModalidad_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ClearAndInsertItem(DropDownListPlanEstudio);
    //    ClearAndInsertItem(DropDownListUnidadAprend);
    //    ClearLblCalcularEstadisticos();
    //    ClearCheckPeriodo();
    //    divCheckPeriodo.Visible = false;

    //    if (DropDownListModalidad.SelectedIndex == 0)
    //    {
    //        ClearAndInsertItem(DropDownListProgramaAcad);
    //    }
    //}

    protected void DropDownListProgramaAcad_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAndInsertItem(DropDownListUnidadAprend);
        ClearCheckPeriodo();
        divCheckPeriodo.Visible = false;

        if (DropDownListProgramaAcad.SelectedIndex == 0)
        {
            ClearAndInsertItem(DropDownListPlanEstudio);
        }

        ClearLblCalcularEstadisticos();
    }

    protected void DropDownListPlanEstudio_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearCheckPeriodo();
        divCheckPeriodo.Visible = false;

        if (DropDownListPlanEstudio.SelectedIndex == 0)
        {
            ClearAndInsertItem(DropDownListUnidadAprend);
        }

        ClearLblCalcularEstadisticos();
    }

    protected void DropDownListUnidadAprend_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListPeriodo.ClearSelection();
        if (DropDownListUnidadAprend.SelectedIndex == 0)
        {
        }
        else
        {
            GridViewOcupabilidad.DataSourceID = "SqlDataSourceGrupoPerioodo";
            GridViewOcupabilidad.DataBind();
            divGridOcupo.Visible = true;
            divCalculos.Visible = true;
            CalcularEstadisticosSql();

            divDetailGraf.Visible = true;
            divCheckPeriodo.Visible = true;
            
            //CaragarTablaWS_zp(LabelZP.Text);
            DataBindDropDownPeriodos();
        }
    }

    protected void DropDownListPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListPeriodo.SelectedIndex == 0)
        {
            GridViewOcupabilidad.DataSourceID = "SqlDataSourceGrupoPerioodo";
            GridViewOcupabilidad.DataBind();
        }
        else if (DropDownListPeriodo.SelectedIndex >= 1)
        {
            GridViewOcupabilidad.DataSourceID = "SqlDataSourcePEParImpar";
            GridViewOcupabilidad.DataBind();
        }

        divGridOcupo.Visible = true;
        divCalculos.Visible = true;
        CalcularEstadisticosSql();
        divDetailGraf.Visible = true;
        DataBindDropDownPeriodos();
    }

    protected void CheckBoxVigencia_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxPeriodo.Checked)
        {
            divDdlPeriodo.Style["display"] = "block";

        }
        else
        {
            divDdlPeriodo.Style["display"] = "none";
            DropDownListPeriodo.ClearSelection();
            GridViewOcupabilidad.DataSourceID = "SqlDataSourceGrupoPerioodo";
            GridViewOcupabilidad.DataBind();
            CalcularEstadisticosSql();
        }
    }

    protected void ClearCheckPeriodo()
    {
        divDdlPeriodo.Style["display"] = "none";
        DropDownListPeriodo.ClearSelection();
        CheckBoxPeriodo.Checked = false;

    }

    protected void CalcularEstadisticos()
    {
        List<double> valores = new List<double>();

        // obtener los valores de la columna "Promedio (ocupabilidad)"
        foreach (GridViewRow row in GridViewOcupabilidad.Rows)
        {
            double valor;
            if (double.TryParse(row.Cells[3].Text, out valor)) // Columna 3 = Promedio (ocupabilidad)
            {
                valores.Add(valor);
            }
        }

        if (valores.Count > 0)
        {
            // Media
            double media = valores.Average();

            // Varianza muestral
            double varianza = valores.Sum(x => Math.Pow(x - media, 2)) / (valores.Count - 1);

            // Desviación estándar
            double desviacion = Math.Sqrt(varianza);

            // Mostramos en labels
            lblMedia.Text = "Media: <b>" + media.ToString("0.00") + "</b>";
            lblVarianza.Text = "Varianza: <b>" + varianza.ToString("0.00") + "</b>";
            lblDesv.Text = "Desviación estándar: <b>" + desviacion.ToString("0.00") + "</b>";
        }
        else
        {
            lblMedia.Text = "No hay datos para calcular.";
        }
    }

    private string ObtenerEstadisticosSql(DropDownList ddlPeriodo)
    {
        string estadisticosSQL;

        if (ddlPeriodo.SelectedIndex >= 1)
        {
            estadisticosSQL = @"
            WITH Datos AS
            (
            SELECT hcd.PERIODO_ESCOLAR, (hcd.SECUENCIA+hcd.ID_ASIGNATURA) grupos, ALUMNOS
		            FROM HISTORIAL_CARGA_DOCENTE hcd
		            INNER JOIN PLANES_ESTUDIO pe
			            ON hcd.CLAVE_ZP = pe.CLAVE_ZP
			            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
			            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		            WHERE hcd.CLAVE_ZP = @clave
			            AND hcd.MODALIDAD = 1
			            AND hcd.ID_ASIGNATURA = @asignatura
                        AND PERIODO_ESCOLAR like  '%' + @periodo + ''
		            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, 
		            hcd.MODALIDAD, hcd.ID_ASIGNATURA, hcd.SECUENCIA, 
		            hcd.ALUMNOS
		    )
            SELECT 
                AVG((CAST(ALUMNOS AS FLOAT))) AS Media,
                VARP((CAST(ALUMNOS AS FLOAT)))  AS Varianza,
                STDEVP((CAST(ALUMNOS AS FLOAT))) AS DesviacionEstandar
            FROM Datos;";
        }
        else
        {
            estadisticosSQL = @"
            WITH Datos AS
            (
            SELECT hcd.PERIODO_ESCOLAR, (hcd.SECUENCIA+hcd.ID_ASIGNATURA) grupos, ALUMNOS
		            FROM HISTORIAL_CARGA_DOCENTE hcd
		            INNER JOIN PLANES_ESTUDIO pe
			            ON hcd.CLAVE_ZP = pe.CLAVE_ZP
			            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
			            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		            WHERE hcd.CLAVE_ZP = @clave
			            AND hcd.MODALIDAD = 1
			            AND hcd.ID_ASIGNATURA = @asignatura
		            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, 
		            hcd.MODALIDAD, hcd.ID_ASIGNATURA, hcd.SECUENCIA, 
		            hcd.ALUMNOS
		    )
            SELECT 
                AVG((CAST(ALUMNOS AS FLOAT)))  AS Media,
                VARP((CAST(ALUMNOS AS FLOAT)))  AS Varianza,
                STDEVP((CAST(ALUMNOS AS FLOAT))) AS DesviacionEstandar
            FROM Datos;";
        }

        return estadisticosSQL;
    }

    protected void CalcularEstadisticosSql()
    {
        string query = ObtenerEstadisticosSql(DropDownListPeriodo);

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@clave", DropDownListUnidadAcademica.SelectedValue);
            //cmd.Parameters.AddWithValue("@modalidad", DropDownListModalidad.SelectedValue);
            cmd.Parameters.AddWithValue("@asignatura", DropDownListUnidadAprend.SelectedValue);
            cmd.Parameters.AddWithValue("@periodo", DropDownListPeriodo.SelectedValue);

            decimal media, varianza, desviacion = 0;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                decimal.TryParse(dr["Media"].ToString(), out media);
                decimal.TryParse(dr["Varianza"].ToString(), out varianza);
                decimal.TryParse(dr["DesviacionEstandar"].ToString(), out desviacion);

                //Rango
                decimal rangoInferior = media - (2 * desviacion);
                decimal rangoSuperior = media + (2 * desviacion);

                decimal rano = (media - (2 * desviacion)) < 1 ? 1 : media - (2 * desviacion);

                lblMedia.Text = "Media: <b>" + Math.Round(media, 0).ToString("0") + "</b>";
                lblVarianza.Text = "Varianza: <b>" + Math.Round(varianza, 0).ToString("0") + "</b>";
                lblDesv.Text = "Desviación estándar: <b>" + Math.Round(desviacion, 0).ToString("0") + "</b>";
                lblRango.Text = "Rango sugerido de cupo: <b>" + Math.Round(rano, 0).ToString("0") + " mín. , " + Math.Round(rangoSuperior, 0).ToString("0") + " máx.</b>";
                //lblRango.Text = "Cupo mínimo sugerido: <b>" + rangoInferior.ToString("0") + "</b>";
            }
            con.Close();
        }
    }

    protected void ClearLblCalcularEstadisticos()
    {
        divGridOcupo.Visible = false;
        divCalculos.Visible = false;
        divDetailGraf.Visible = false;
        lblMedia.Text = string.Empty;
        lblVarianza.Text = string.Empty;
        lblDesv.Text = string.Empty;

    }

    public class ResultadoFila
    {
        public string Periodo { get; set; }
        public double Xi { get; set; }              // Promedio
        public double Desviacion { get; set; }      // xi - media
        public double Desviacion2 { get; set; }     // (xi - media)^2
    }

    private string ObtenerEstadisticosSqlDetail(DropDownList ddlPeriodo)
    {
        string estadisticosSQL;

        if (ddlPeriodo.SelectedIndex >= 1)
        {
            estadisticosSQL = @"
            SELECT hcd.PERIODO_ESCOLAR, hcd.SECUENCIA AS GRUPO, ALUMNOS
            FROM HISTORIAL_CARGA_DOCENTE hcd
            INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
            WHERE hcd.CLAVE_ZP = @clave
	            AND hcd.MODALIDAD = 1
	            AND hcd.ID_ASIGNATURA = @asignatura
                AND PERIODO_ESCOLAR like  '%' + @periodo + ''
            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, 
			            hcd.ID_ASIGNATURA, hcd.SECUENCIA, hcd.ALUMNOS
            ORDER BY hcd.PERIODO_ESCOLAR";
        }
        else
        {
            estadisticosSQL = @"
            SELECT hcd.PERIODO_ESCOLAR, hcd.SECUENCIA AS GRUPO, ALUMNOS
            FROM HISTORIAL_CARGA_DOCENTE hcd
            INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
            WHERE hcd.CLAVE_ZP = @clave
	            AND hcd.MODALIDAD = 1
	            AND hcd.ID_ASIGNATURA = @asignatura
            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, 
			            hcd.ID_ASIGNATURA, hcd.SECUENCIA, hcd.ALUMNOS
            ORDER BY hcd.PERIODO_ESCOLAR";
        }

        return estadisticosSQL;
    }

    private void CalcularEstadisticosSqlDetail()
    {
        //string connStr = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
        List<Tuple<string, double>> valores = new List<Tuple<string, double>>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = ObtenerEstadisticosSqlDetail(DropDownListPeriodo);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@clave", DropDownListUnidadAcademica.SelectedValue);
            //cmd.Parameters.AddWithValue("@modalidad", DropDownListModalidad.SelectedValue);
            cmd.Parameters.AddWithValue("@asignatura", DropDownListUnidadAprend.SelectedValue);
            cmd.Parameters.AddWithValue("@periodo", DropDownListPeriodo.SelectedValue);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string periodo = dr["PERIODO_ESCOLAR"].ToString();
                double promedio = Convert.ToDouble(dr["ALUMNOS"]);
                valores.Add(Tuple.Create(periodo, promedio));
            }
        }

        if (valores.Count == 0)
        {
            GridViewResultados.DataSource = null;
            GridViewResultados.DataBind();
            return;
        }

        // Media
        double media = valores.Average(v => v.Item2);

        // Desviaciones y varianza
        List<ResultadoFila> filas = new List<ResultadoFila>();
        double sumaDesv2 = 0;

        foreach (var item in valores)
        {
            double desviacion = item.Item2 - media;
            double desviacion2 = Math.Pow(desviacion, 2);
            sumaDesv2 += desviacion2;

            filas.Add(new ResultadoFila
            {
                Periodo = item.Item1,
                Xi = Math.Round(item.Item2, 2),
                Desviacion = Math.Round(desviacion, 2),
                Desviacion2 = Math.Round(desviacion2, 2)
            });
        }

        double varianza = valores.Count > 1 ? sumaDesv2 / (valores.Count) : 0;
        double desvEstandar = Math.Sqrt(varianza);

        // Fila resumen tipo Σ
        filas.Add(new ResultadoFila
        {
            Periodo = "Σ / Resumen",
            Xi = 0, // no aplica
            Desviacion = Math.Round(varianza, 2),   // varianza
            Desviacion2 = Math.Round(sumaDesv2, 2)  // suma de desviaciones²
        });

        GridViewResultados.DataSource = filas;
        GridViewResultados.DataBind();

        // Ajustar última fila con texto personalizado
        GridViewRow row = GridViewResultados.Rows[GridViewResultados.Rows.Count - 1];
        row.Cells[1].Text = "—";
        row.Cells[2].Text = "Var = " + Math.Round(varianza).ToString("F2");
        row.Cells[3].Text = "Σ = " + sumaDesv2.ToString("F2") + " | s = " + Math.Round(desvEstandar).ToString("F2");
    }

    protected void LinkButtonAddProgramAcad_Click(object sender, EventArgs e)
    {
        LabelModalDetail_claveZp.Text = DropDownListUnidadAcademica.SelectedItem.Text;
        //LabelModalDetail_modalidad.Text = DropDownListModalidad.SelectedItem.Text;
        LabelModalDetail_programaAcad.Text = DropDownListProgramaAcad.SelectedItem.Text;
        LabelModalDetail_planEst.Text = DropDownListPlanEstudio.SelectedValue;
        LabelModalDetail_unidadAcad.Text = DropDownListUnidadAprend.SelectedItem.Text;

        LabelModalDetail_periodoPar.Text = (DropDownListPeriodo.SelectedIndex >= 1) ? DropDownListPeriodo.SelectedItem.Text : string.Empty;

        bool checkSelect = (DropDownListPeriodo.SelectedIndex >= 1);
        divModalDetailPeriodo.Visible = checkSelect;

        CalcularEstadisticosSqlDetail();
        string scriptSMAP = "ShowModalAddProgramAcad();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", scriptSMAP, true);
    }

    public class DatosOcupabilidad
    {
        public string Periodo { get; set; }
        public double Promedio { get; set; }
        public int Grupos { get; set; }
        public int Alumnos { get; set; }
    }

    public class ResultadoOcupabilidad
    {
        public List<DatosOcupabilidad> Datos { get; set; }
        public double Media { get; set; }
        public double Varianza { get; set; }
        public double Desviacion { get; set; }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ObtenerDatosOcupabilidad(string clave, string asignatura, string periodo)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

        List<DatosOcupabilidad> lista = new List<DatosOcupabilidad>();
        int periodoInt = 0;
        int.TryParse(periodo, out periodoInt);

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string queryDatos;

            if (periodoInt >= 1)
            {
                queryDatos = @"
                SELECT hcd.PERIODO_ESCOLAR, 
                      (SELECT SUM(t1.ALUMNOS) 
                           FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
                                   FROM HISTORIAL_CARGA_DOCENTE
                                   WHERE CLAVE_ZP = hcd.CLAVE_ZP 
                                       AND MODALIDAD = hcd.MODALIDAD
                                       AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
                                       AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR						
                               ) AS t1
                       ) as Alumnos,
                       COUNT (DISTINCT SECUENCIA) as GRUPOS,
                       (SELECT ROUND(AVG(CAST(ALUMNOS AS FLOAT)), 0) 
			                FROM HISTORIAL_CARGA_DOCENTE
			                WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				                AND MODALIDAD = hcd.MODALIDAD
				                AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				                AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		                ) AS PROMEDIO 
                FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
                WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
                 AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
                 AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                 AND hcd.CLAVE_ZP = @clave
                 AND hcd.MODALIDAD = 1
                 AND hcd.ID_ASIGNATURA = @asignatura
                 AND PERIODO_ESCOLAR like '%' + @periodo + ''
                GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA    
                ORDER BY hcd.PERIODO_ESCOLAR";
            }
            else
            {
                queryDatos = @"
                SELECT hcd.PERIODO_ESCOLAR, 
                      (SELECT SUM(t1.ALUMNOS) 
                           FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
                                   FROM HISTORIAL_CARGA_DOCENTE
                                   WHERE CLAVE_ZP = hcd.CLAVE_ZP 
                                       AND MODALIDAD = hcd.MODALIDAD
                                       AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
                                       AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR						
                               ) AS t1
                       ) as Alumnos,
                       COUNT (DISTINCT SECUENCIA) as GRUPOS,
                       (SELECT ROUND(AVG(CAST(ALUMNOS AS FLOAT)), 0)
			                FROM HISTORIAL_CARGA_DOCENTE
			                WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				                AND MODALIDAD = hcd.MODALIDAD
				                AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				                AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		                ) AS PROMEDIO 
                FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
                WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
                 AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
                 AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                 AND hcd.CLAVE_ZP = @clave
                 AND hcd.MODALIDAD = 1
                 AND hcd.ID_ASIGNATURA = @asignatura
                GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA    
                ORDER BY hcd.PERIODO_ESCOLAR";
            }

            SqlCommand cmd = new SqlCommand(queryDatos, conn);
            cmd.Parameters.AddWithValue("@clave", clave);
            //cmd.Parameters.AddWithValue("@modalidad", modalidad);
            cmd.Parameters.AddWithValue("@asignatura", asignatura);
            if (periodoInt >= 1) cmd.Parameters.AddWithValue("@periodo", periodo);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new DatosOcupabilidad
                {
                    Periodo = dr["PERIODO_ESCOLAR"].ToString(),
                    Alumnos = Convert.ToInt32(dr["Alumnos"]),
                    Grupos = Convert.ToInt32(dr["Grupos"]),
                    Promedio = Convert.ToDouble(dr["Promedio"])
                });
            }
            conn.Close();
        }

        // Si no hay datos
        if (lista.Count == 0)
        {
            return new JavaScriptSerializer().Serialize(new
            {
                Datos = lista,
                Media = 0,
                Varianza = 0,
                Desviacion = 0
            });
        }

        // SEGUNDA CONSULTA: cálculos estadísticos reales basados en AVG(alumnos)
        List<double> listaPromedios = new List<double>();
        using (SqlConnection conn2 = new SqlConnection(connectionString))
        {
            string queryStats;

            if (periodoInt >= 1)
            {
                queryStats = @"
                SELECT 
                    ALUMNOS
                FROM HISTORIAL_CARGA_DOCENTE hcd
                INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	                AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	                AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                WHERE hcd.CLAVE_ZP = @clave
	                AND hcd.MODALIDAD = 1
	                AND hcd.ID_ASIGNATURA = @asignatura
                    AND PERIODO_ESCOLAR LIKE '%' + @periodo + ''
                GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, 
                         hcd.ID_ASIGNATURA, hcd.SECUENCIA, hcd.ALUMNOS
                ORDER BY hcd.PERIODO_ESCOLAR";
            }
            else
            {
                queryStats = @"
                SELECT 
                    ALUMNOS
                FROM HISTORIAL_CARGA_DOCENTE hcd
                INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	                AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	                AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                WHERE hcd.CLAVE_ZP = @clave
	                AND hcd.MODALIDAD = 1
	                AND hcd.ID_ASIGNATURA = @asignatura
                GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, 
                         hcd.ID_ASIGNATURA, hcd.SECUENCIA, hcd.ALUMNOS
                ORDER BY hcd.PERIODO_ESCOLAR";
            }

            SqlCommand cmd2 = new SqlCommand(queryStats, conn2);
            cmd2.Parameters.AddWithValue("@clave", clave);
            //cmd2.Parameters.AddWithValue("@modalidad", modalidad);
            cmd2.Parameters.AddWithValue("@asignatura", asignatura);
            if (periodoInt >= 1) cmd2.Parameters.AddWithValue("@periodo", periodo);

            conn2.Open();
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                listaPromedios.Add(Convert.ToDouble(dr2["ALUMNOS"]));
            }
            conn2.Close();
        }

        // 🔹 Calcular estadísticos basados en listaPromedios
        double media = listaPromedios.Average();
        double var = 0;
        foreach (double val in listaPromedios)
            var += Math.Pow(val - media, 2);
        var /= listaPromedios.Count;

        double desviacion = Math.Sqrt(var);

        ResultadoOcupabilidad resultado = new ResultadoOcupabilidad
        {
            Datos = lista,
            Media = media,
            Varianza = var,
            Desviacion = desviacion
        };

        return new JavaScriptSerializer().Serialize(resultado);
    }


    protected void LinkButtonGraf_Click(object sender, EventArgs e)
    {
        LabelModalGrafico_claveZp.Text = DropDownListUnidadAcademica.SelectedItem.Text;
        //LabelModalGrafico_modalidad.Text = DropDownListModalidad.SelectedItem.Text;
        LabelModalGrafico_programaAcad.Text = DropDownListProgramaAcad.SelectedItem.Text;
        LabelModalGrafico_planEst.Text = DropDownListPlanEstudio.SelectedValue;
        LabelModalGrafico_unidadAcad.Text = DropDownListUnidadAprend.SelectedItem.Text;

        LabelModalGrafico_periodoPar.Text = (DropDownListPeriodo.SelectedIndex >= 1) ? DropDownListPeriodo.SelectedItem.Text : string.Empty;

        bool checkSelect = (DropDownListPeriodo.SelectedIndex >= 1);
        divModalGraficoPeriodo.Visible = checkSelect;

        string javaScript2 = "mostrarGraficos(); ShowModalGraficos();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script2", javaScript2, true);
    }
    /*************************************************************/



    protected void LinkButtonCardEventosRiesgo_datos_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalDetalleEventos";
        LabelModalDetalleEventos_title.Text = "Eventos de riesgo registrados";
        LabelModalDetalleEventos_nombre.Text = "2020-2025";
        ShowModal(IdModal);
        DataBindGridViewDetalleEventos();
        detalleEstatusEventos();
    }
    public void detalleEstatusEventos()
    {
        string zp = LabelZP.Text;
        string cate = DropDownListDetalleEvento_categoria.SelectedValue.ToString();

        LabelDetalleEvento_pendientes.Text = Consultas.ConsultaS("select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where ID_EST_PETICION = 1 and pli.CLAVE_ZP like '"+ zp +"%' and ID_CAT_PETICION like '"+ cate +"%'");
        LabelDetalleEvento_atendiendo.Text = Consultas.ConsultaS("select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where ID_EST_PETICION = 2 and pli.CLAVE_ZP like '"+ zp +"%' and ID_CAT_PETICION like '"+ cate +"%'");
        LabelDetalleEvento_atendidas.Text = Consultas.ConsultaS("select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where ID_EST_PETICION = 3 and pli.CLAVE_ZP like '"+ zp +"%' and ID_CAT_PETICION like '"+ cate +"%'");
    }
    public void DataBindGridViewDetalleEventos()
    {
        string pe = LabelPE.Text;
        string zp = LabelZP.Text;
        string cate = DropDownListDetalleEvento_categoria.SelectedValue.ToString();

        string totalRows;

        string qry = "select pli.CLAVE_ZP, pet.ID_PETICION, pet_e.DESCRIPCION_PETICION, pet_d.DESCRIPCION_CAT_PETICION, pet.DESC_PETICION, pet.DESC_RESP_PETICION, " +
                    "FORMAT(pet.FECHA_PETICION,'d-MM-yyyy') FECHA_INICIO, " +
                    "FORMAT(pet.FECHA_RESP_PETICION,'d-MM-yyyy') FECHA_FIN " +
                    "from PETICIONES pet " +
                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                    "inner join CAT_CATEGORIA_PETICION pet_d on pet_d.ID_CAT_PETICION = pet.ID_CAT_PETICION " +
                    "inner join ESTATUS_PETICION pet_e on pet_e.ID_EST_PETICION = pet.ID_EST_PETICION " +
                    "where pli.CLAVE_ZP like '"+ zp +"%' and pet.ID_CAT_PETICION like '"+ cate +"%'";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlDataAdapter da = new SqlDataAdapter(qry, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    totalRows = dt.Rows.Count.ToString();

                    this.GridViewDetalleEventos.DataSource = dt;

                    GridViewDetalleEventos.DataBind();
                    GridViewDetalleEventos.PageIndex = 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            con.Close();
        }

    }
    protected void LinkButtonCardEventosAtendidos_datos_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButtonCardDetallePendientes_datos_Click(object sender, EventArgs e)
    {

    }
    protected void MostrarDetalleEventos_url_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string url = btn.CommandArgument;
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('"+ url +"', '_blank');", true);
    }
    
    
    
    private void DataBindGridViewUnidadesAcademicasNS()
    {
        string pe = LabelPE.Text;        

        string qryNS = "select * " +
                        "from( " +
                            "select dp.CLAVE_ZP, dp.LOGO, dp.DESCRIPCION_DP, (select COUNT(ID_PETICION) from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where pli.CLAVE_ZP = dp.CLAVE_ZP) PETICIONES " +
                            "from CAT_DEPENDENCIAS_POLITECNICAS dp " +
                            "where dp.ID_NIVEL_EST = 2 " +
                        ")datos " +
                        "order by PETICIONES desc, DESCRIPCION_DP asc";

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    this.GridViewUnidadesAcademicasNS.DataSource = dt;
                    GridViewUnidadesAcademicasNS.DataBind();
                    GridViewUnidadesAcademicasNS.PageIndex = 0;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }
    protected void GridViewUnidadesAcademicasNS_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewUnidadesAcademicasNS.PageIndex = e.NewPageIndex;
        this.DataBindGridViewUnidadesAcademicasNS();
    }
    protected void GridViewUnidadesAcademicasNS_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
    
    
    
    protected void LinkButtonUnidadAcademicaNS_seleccionar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
        
        string zp = commandArgs[0];
        string ua = commandArgs[1];

        AsignarValoresZP_seleccionada(zp, ua);

        GridViewRow rowGV = (GridViewRow)btn.NamingContainer;
        GridView gv = (GridView)rowGV.NamingContainer;

        foreach (GridViewRow row in gv.Rows)
        {
            LinkButton btnGV = ((LinkButton)row.FindControl("LinkButtonUnidadAcademicaNS_seleccionar"));
            btnGV.Text = "Mostrar";
            btnGV.CssClass = "btn btn-sm btn-outline-danger LoadingOverlay";
            row.BackColor = System.Drawing.Color.White;
        }


        btn.Text = "Seleccionada";
        btn.CssClass = "btn btn-sm btn-outline-success LoadingOverlay";
        rowGV.BackColor = System.Drawing.Color.LightGray;

    }
    private void AsignarValoresZP_seleccionada(string zp, string ua)
    {
        LabelZP.Text = zp;
        divAlertUnidadAcademica_seleccionada.Visible = true;
        LabelUnidadAcademicaSeleccionada_nombre.Text = ua;
    }
    protected void LinkButtonAlertUnidadAcademica_cerrar_Click(object sender, EventArgs e)
    {
        LimpiarValoresZP_seleccionada();
    }
    private void LimpiarValoresZP_seleccionada()
    {
        LabelZP.Text = string.Empty;
        LabelUnidadAcademicaSeleccionada_nombre.Text = string.Empty;
        divAlertUnidadAcademica_seleccionada.Visible = false;

        DataBindGridViewUnidadesAcademicasNS();
    }
    [WebMethod]
    public static List<object> chartColumnEventos_datos(string zp)
    {
        List<object> chartData = new List<object>();

        chartData.Add(new object[]
            {
                "DESCRIPCION", "TOTAL"
            });

        string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

        bool existEv = Consultas.ConsultaInt("select COUNT(ID_CAT_PETICION)total from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where pli.CLAVE_ZP like '"+ zp +"%'") == 0 ? false : true;

        string query = "select DESCRIPCION_CAT_PETICION DESCRIPCION, 0 TOTAL " +
                        "from CAT_CATEGORIA_PETICION " +
                        "group by DESCRIPCION_CAT_PETICION " +
                        "with rollup ";
        if (existEv == true)
        {
            query = "select cat_p.DESCRIPCION_CAT_PETICION DESCRIPCION, COUNT(pet.ID_CAT_PETICION) TOTAL " +
                    "from PETICIONES pet " +
                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                    "inner join CAT_CATEGORIA_PETICION cat_p on cat_p.ID_CAT_PETICION = pet.ID_CAT_PETICION " +
                    "where pli.CLAVE_ZP like '"+ zp +"%' " +
                    "group by cat_p.DESCRIPCION_CAT_PETICION " +
                    "with rollup ";
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
                            sdr["DESCRIPCION"],sdr["TOTAL"]
                        });

                    }

                }
                con.Close();
                return chartData;
            }
        }

    }
    [WebMethod]
    public static List<object> chartPieEventos_datos(string zp)
    {
        List<object> chartData = new List<object>();

        chartData.Add(new object[]
            {
                "name", "y", "color"
            });

        string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

        bool existEv = Consultas.ConsultaInt("select COUNT(ID_CAT_PETICION)total from PETICIONES inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO where pli.CLAVE_ZP like '"+ zp +"%'") == 0 ? false : true;

        string query = "select DESCRIPCION_PETICION DESCRIPCION, 0 TOTAL, " +
                        "case " +
                            "when DESCRIPCION_PETICION = 'Atendido' then '#5DC45C' " +
                            "when DESCRIPCION_PETICION = 'En Proceso' then '#F54927' " +
                            "when DESCRIPCION_PETICION = 'Pendiente' then '#F53533' " +
                        "else 'gray' end COLOR " +
                        "from ESTATUS_PETICION " +
                        "group by DESCRIPCION_PETICION " +
                        "with rollup";
        if (existEv == true)
        {
            query = "select pet_e.DESCRIPCION_PETICION DESCRIPCION, COUNT(pet.ID_PETICION) TOTAL, " +
                    "case " +
                        "when pet_e.DESCRIPCION_PETICION = 'Atendido' then '#5DC45C' " +
                        "when pet_e.DESCRIPCION_PETICION = 'En Proceso' then '#F54927' " +
                        "when pet_e.DESCRIPCION_PETICION = 'Pendiente' then '#F53533' " +
                    "else 'gray' end COLOR " +
                    "from PETICIONES pet " +
                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                    "inner join ESTATUS_PETICION pet_e on pet_e.ID_EST_PETICION = pet.ID_EST_PETICION " +
                    "where pli.CLAVE_ZP like '"+ zp +"%' " +
                    "group by pet_e.DESCRIPCION_PETICION " +
                    "with rollup";
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
                            sdr["DESCRIPCION"],sdr["TOTAL"],sdr["COLOR"]
                        });

                    }

                }
                con.Close();
                return chartData;
            }
        }

    }
    protected void GridViewDetalleEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridViewDetalleEventos.PageIndex = e.NewPageIndex;
        this.DataBindGridViewDetalleEventos();
    }
    protected void DropDownListDetalleEvento_categoria_DataBound(object sender, EventArgs e)
    {
        DropDownListDetalleEvento_categoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListDetalleEvento_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBindGridViewDetalleEventos();
        detalleEstatusEventos();
    }



    protected void DropDownListUnidadAcademica_resumen_SelectedIndexChanged(object sender, EventArgs e)
    {
        string zp = DropDownListUnidadAcademica_resumen.SelectedValue.ToString();
        string idPerfil = HiddenFieldUnidadesAcademicasNS_idPerfil.Value;

        DetalleModalPerfilAutoridad(zp);

        if (String.IsNullOrEmpty(zp) || String.IsNullOrEmpty(idPerfil))
        {
            DivDetallePerfil_seccion.Visible = false;
        }
        else
        {
            DivDetallePerfil_seccion.Visible = true;
            cargarDatosAutoridad();
        }        
    }
    protected void DropDownListUnidadAcademica_resumen_DataBound(object sender, EventArgs e)
    {
        DropDownListUnidadAcademica_resumen.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    private void cargarDatosAutoridad()
    {//sin_foto.jpg

        string zp = String.IsNullOrEmpty(LabelZP.Text) == true ? DropDownListUnidadAcademica_resumen.SelectedValue.ToString() : LabelZP.Text;
        string idPerfil = HiddenFieldUnidadesAcademicasNS_idPerfil.Value;

        string nombre = "Sin datos registrados";
        string cargo = "Sin datos registrados";
        string foto = "sin_foto.jpg";
        string inicio = "Sin datos registrados";
        string fin = "Sin datos registrados";
        string correo = "Sin datos registrados";
        string telefono = "Sin datos registrados";
        string observacion = "";

        string strClass = "alert alert-warning";
        string descripcion = "<strong>vacante</strong>";
        string descripcionAnio = "";

        bool exist = Consultas.ConsultaInt("select COUNT(*) total from AUTORIDADES_ZP where CLAVE_ZP = '"+ zp +"' and ID_PERFIL = '"+ idPerfil +"' and ESTATUS = '1'") != 0 ? true : false;

        if (exist == true)
        {
            int difAnio = Consultas.ConsultaInt("select DATEDIFF(year, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN) MESES from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '"+ idPerfil +"'");
            int difMes = Consultas.ConsultaInt("select DATEDIFF(month, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN) % 12 MESES from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '"+ idPerfil +"'");
            int anioAbs = Math.Abs(difAnio);

            string idUser = Consultas.ConsultaS("select ID_USER from AUTORIDADES_ZP where CLAVE_ZP = '"+ zp +"' and ID_PERFIL = '"+ idPerfil +"' and ESTATUS = '1'");
            
            string  pdf = Consultas.ConsultaS("select CONCAT(CLAVE_ZP,'_',(FORMAT(FECHA_INICIO,'yyyy')),'_',ID_PERFIL,'_',ID_USER,'.pdf') NOMBRAMIENTO from AUTORIDADES_ZP where CLAVE_ZP = '"+ zp +"' and ID_USER = '"+ idUser +"'");

            if (anioAbs >= 1)
            {
                descripcionAnio = anioAbs == 1 ? "<strong>"+ anioAbs +" año </strong>" : "<strong>"+ anioAbs +" años </strong>";
            }

            if (difMes >= 1)
            {
                if (anioAbs >= 1)
                {
                    descripcionAnio = anioAbs == 1 ? "" : "<strong>"+ (anioAbs - 1) +" años </strong>";
                }

                descripcion = difMes == 1 ? "<strong>"+ descripcionAnio + difMes +" mes</strong> restante. " : "<strong>"+ descripcionAnio + difMes +" meses</strong> restantes. ";
                strClass = "alert alert-success";
            }
            else
            {
                descripcion = difMes == 1 ? "vencido hace <strong>"+ descripcionAnio + Math.Abs(difMes) +" mes.</strong>" : "vencido hace <strong>"+ descripcionAnio + Math.Abs(difMes) +" meses.</strong>";
                strClass = "alert alert-danger";
            }

            nombre = Consultas.ConsultaS("select CONCAT(us.NOMBRE,' ',us.APELLIDO_PAT,' ',APELLIDO_MAT) NOMBRE " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");
            cargo = Consultas.ConsultaS("select per.DESCRIPCION " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");
            foto = Consultas.ConsultaS("select IIF(au.FOTO is null,'sin_foto.jpg', au.FOTO) FOTO " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");

            inicio = Consultas.ConsultaS("select FORMAT(au.FECHA_INICIO,'dddd dd MMMM, yyyy', 'es-ES') FECHA_I " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");
            fin = Consultas.ConsultaS("select FORMAT(au.FECHA_FIN,'dddd dd MMMM, yyyy', 'es-ES') FECHA_F " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");

            correo = Consultas.ConsultaS("select au.CORREO " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");
            telefono = Consultas.ConsultaS("select au.CELULAR " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");
            observacion = Consultas.ConsultaS("select au.OBSERVACION " +
                                                  "from AUTORIDADES_ZP au  " +
                                                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                                                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"'");

            foto = verificarFoto(foto) == true ? foto : "sin_foto.jpg";

            if (verificarNombramiento(pdf) == true)
            {
                string ua = Consultas.ConsultaS("select per.DESCRIPCION " +
                                                "from AUTORIDADES_ZP au " +
                                                "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                                                "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"' and ID_USER = '"+ idUser +"' and au.ESTATUS = '1'");
                string dp = Consultas.ConsultaS("select dp.DESCRIPCION_DP " +
                                                "from AUTORIDADES_ZP au " +
                                                "inner join CAT_DEPENDENCIAS_POLITECNICAS dp on dp.CLAVE_ZP = au.CLAVE_ZP " +
                                                "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '"+ idPerfil +"' and ID_USER = '"+ idUser +"' and au.ESTATUS = '1'");

                DivCardDatosAutoridades_nombramiento.Visible = true;

                string[] commandsArgs = { ua, dp , nombre, pdf};

                LinkButtonDatosAutoridad_pdf.CommandArgument = string.Join(",",commandsArgs);
            }
            else
            {
                DivCardDatosAutoridades_nombramiento.Visible = false;
                LinkButtonDatosAutoridad_pdf.CommandArgument = "";
            }

        }
        if (!String.IsNullOrEmpty(observacion))
        {
            fin = observacion.Substring(0, 3) == "pró" ? "prórroga" : fin;
        }

        string datos = "<div class='card-body profile-card pt-4 d-flex flex-column align-items-center'> " +
                        "<img src='public/img/Foto_perfil/autoridades/"+ foto +"' alt='Profile' class='rounded-circle'> " +
                        "<h2 style='text-align: center;'>"+ nombre +"</h2> " +
                        "<h3 style='text-align: center;'>"+ cargo +"</h3> " +
                         "<div class='"+ strClass +"' role='alert' style='text-align:center'> " +
                           ""+ descripcion +"" +
                            "<br><br><span class='text-secondary'>"+ observacion +"</span>" +
                         "</div> " +
                        "<div class='social-links mt-2'> " +
                            "<a href = '#' class='twitter'><i class='bi bi-twitter'></i></a> " +
                            "<a href = '#' class='facebook'><i class='bi bi-facebook'></i></a> " +
                            "<a href = '#' class='instagram'><i class='bi bi-instagram'></i></a> " +
                            "<a href = '#' class='linkedin'><i class='bi bi-linkedin'></i></a> " +
                        "</div> " +
                    "</div>";

        LabelDatosAutoridad_inicio.Text = inicio;
        LabelDatosAutoridad_fin.Text = fin;
        LabelDatosAutoridad_correo.Text = correo;
        LabelDatosAutoridad_telefono.Text = telefono;

        DivCardDatosAutoridades_perfil.InnerHtml = datos;
    }
    private bool verificarFoto(string nombreFoto)
    {
        string ruta = HttpContext.Current.Server.MapPath(@"~/public/img/Foto_perfil/autoridades/"+ nombreFoto +"");

        bool exists = System.IO.File.Exists(ruta);

        return exists;
    }
    private void DataBindGridViewUnidadesAcademicasNS_resumen()
    {
        string pe = LabelPE.Text;

        string qryNS = "select * " +
                        "from( " +
                            "select dp.CLAVE_ZP, dp.LOGO, dp.DESCRIPCION_DP, " +
                            "(select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and FECHA_FIN < (select FORMAT(GETDATE(),'yyyy-M-dd')) and ESTATUS = 1) VENCIDOS , " +
							"(select ((select COUNT(distinct ID_PERFIL) total from AUTORIDADES_ZP) - COUNT(ID_PERFIL)) TOTAL from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and estatus = 1) VACANTES " +
                            "from CAT_DEPENDENCIAS_POLITECNICAS dp " +
                            "where dp.ID_NIVEL_EST = 2 " +
                        ")datos " +
                        "order by VENCIDOS desc, DESCRIPCION_DP asc";

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    this.GridViewUnidadesAcademicasNS_resumen.DataSource = dt;
                    GridViewUnidadesAcademicasNS_resumen.DataBind();
                    GridViewUnidadesAcademicasNS_resumen.PageIndex = 0;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }
    protected void GridViewUnidadesAcademicasNS_resumen_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void GridViewUnidadesAcademicasNS_resumen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }

        int ven = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VENCIDOS"));
        int vac = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VACANTES"));

        string txtSuc = "<i class='bi bi-bell position-relative'> " +
                            "<span class='position-absolute top-100 start-100 translate-middle badge rounded-pill bg-success' style='font-size: xx-small;'> " +
                            ""+ ven +"" +
                            "<span class='visually-hidden'>unread messages</span> " +
                            "</span> " +
                        "</i>";
        string txtDan = "<i class='bi bi-bell position-relative'> " +
                            "<span class='position-absolute top-100 start-100 translate-middle badge rounded-pill bg-warning' style='font-size: xx-small;'> " +
                            ""+ ven +"" +
                            "<span class='visually-hidden'>unread messages</span> " +
                            "</span> " +
                        "</i>";
        string txtWar = "<i class='bi bi-bell position-relative'> " +
                            "<span class='position-absolute top-100 start-100 translate-middle badge rounded-pill bg-danger' style='font-size: xx-small;'> " +
                            ""+ ven +"" +
                            "<span class='visually-hidden'>unread messages</span> " +
                            "</span> " +
                        "</i>";

        if (ven >= 1) 
        { 
            e.Row.Cells[3].Text = txtDan; 
        } 
        else 
        {
            e.Row.Cells[3].Text = txtSuc;
            
            if (vac >= 1)
            {
                e.Row.Cells[3].Text = txtWar;
            }
        }

    }
    protected void LinkButtonUnidadAcademicaNS_resumen_seleccionar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });

        string zp = commandArgs[0];
        string nombreZp = commandArgs[1];

        LabelZP.Text = zp;

        AsignarValoresZP_resumen_seleccionada(nombreZp);

        GridViewRow rowGV = (GridViewRow)btn.NamingContainer;
        GridView gv = (GridView)rowGV.NamingContainer;

        foreach (GridViewRow row in gv.Rows)
        {
            LinkButton btnGV = ((LinkButton)row.FindControl("LinkButtonUnidadAcademicaNS_resumen_seleccionar"));
            btnGV.Text = "Mostrar";
            btnGV.CssClass = "btn btn-sm btn-outline-secondary LoadingOverlay";
            row.BackColor = System.Drawing.Color.White;
        }


        btn.Text = "Seleccionada";
        btn.CssClass = "btn btn-sm btn-outline-success LoadingOverlay";
        rowGV.BackColor = System.Drawing.Color.LightGray;
    }
    private void AsignarValoresZP_resumen_seleccionada(string ua)
    {
        string zp = LabelZP.Text;
        
        bool existDir = Consultas.ConsultaInt("select COUNT(*) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '11' and ESTATUS = '1'") >= 1 ? true : false;
        bool existAca = Consultas.ConsultaInt("select COUNT(*) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '12' and ESTATUS = '1'") >= 1 ? true : false;
        bool existSseis = Consultas.ConsultaInt("select COUNT(*) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '13' and ESTATUS = '1'") >= 1 ? true : false;
        bool existAdm = Consultas.ConsultaInt("select COUNT(*) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '14' and ESTATUS = '1'") >= 1 ? true : false;

        int tDir = Consultas.ConsultaInt("select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '11' and FECHA_FIN < (select FORMAT(GETDATE(),'yyyy-M-dd'))");
        int tAca = Consultas.ConsultaInt("select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '12' and FECHA_FIN < (select FORMAT(GETDATE(),'yyyy-M-dd'))");
        int tSseis = Consultas.ConsultaInt("select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '13' and FECHA_FIN < (select FORMAT(GETDATE(),'yyyy-M-dd'))");
        int tAdm = Consultas.ConsultaInt("select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%' and ID_PERFIL = '14' and FECHA_FIN < (select FORMAT(GETDATE(),'yyyy-M-dd'))");

        if (!String.IsNullOrEmpty(ua)) divAlertUnidadAcademicaResumen_seleccionada.Visible = true;

        LabelUnidadAcademicaResumenSeleccionada_nombre.Text = ua;
        LabelUnidadesAcademicasNS_resumenDireccion_total.Text ="";
        LabelUnidadesAcademicasNS_resumenAcademica_total.Text ="";
        LabelUnidadesAcademicasNS_resumenSSEIS_total.Text ="";
        LabelUnidadesAcademicasNS_resumenAdministracion_total.Text ="";

        LabelUnidadesAcademicasNS_resumenDireccion_obs.Text = "";
        LabelUnidadesAcademicasNS_resumenAcademica_obs.Text = "";
        LabelUnidadesAcademicasNS_resumenSSEIS_obs.Text = "";
        LabelUnidadesAcademicasNS_resumenAdministracion_obs.Text = "";

        LabelUnidadesAcademicasNS_resumenDireccion_estatus.Text = "Vacante";
        LabelUnidadesAcademicasNS_resumenDireccion_estatus.Attributes["class"] = "badge bg-danger small pt-1 fw-bold";
        DivUnidadesAcademicasNS_resumenDireccion_icon.Attributes["style"] = "background: #FC8686";
        UnidadesAcademicasNS_resumenDireccion_icon.Attributes["class"] = "bi bi-clock-history text-red";
        LabelUnidadesAcademicasNS_resumenAcademica_estatus.Text = "Vacante";
        LabelUnidadesAcademicasNS_resumenAcademica_estatus.Attributes["class"] = "badge bg-danger small pt-1 fw-bold";
        DivUnidadesAcademicasNS_resumenAcademica_icon.Attributes["style"] = "background: #FC8686";
        UnidadesAcademicasNS_resumenAcademica_icon.Attributes["class"] = "bi bi-clock-history text-red";
        LabelUnidadesAcademicasNS_resumenSSEIS_estatus.Text = "Vacante";
        LabelUnidadesAcademicasNS_resumenSSEIS_estatus.Attributes["class"] = "badge bg-danger small pt-1 fw-bold";
        DivUnidadesAcademicasNS_resumenSSEIS_icon.Attributes["style"] = "background: #FC8686";
        UnidadesAcademicasNS_resumenSSEIS_icon.Attributes["class"] = "bi bi-clock-history text-red";
        LabelUnidadesAcademicasNS_resumenAdministracion_estatus.Text = "Vacante";
        LabelUnidadesAcademicasNS_resumenAdministracion_estatus.Attributes["class"] = "badge bg-danger small pt-1 fw-bold";
        DivUnidadesAcademicasNS_resumenAdministracion_icon.Attributes["style"] = "background: #FC8686";
        UnidadesAcademicasNS_resumenAdministracion_icon.Attributes["class"] = "bi bi-clock-history text-red";

        if (String.IsNullOrEmpty(zp))
        {
            LabelUnidadesAcademicasNS_resumenDireccion_total.Text = tDir.ToString();
            LabelUnidadesAcademicasNS_resumenAcademica_total.Text = tAca.ToString();
            LabelUnidadesAcademicasNS_resumenSSEIS_total.Text = tSseis.ToString();
            LabelUnidadesAcademicasNS_resumenAdministracion_total.Text = tAdm.ToString();
        }
        if (existDir)//ffe599   warning
        {
            string obsDir = Consultas.ConsultaS("select SUBSTRING(au.OBSERVACION,0, CHARINDEX(' ',au.OBSERVACION)) OBSERVACION " +
                  "from AUTORIDADES_ZP au  " +
                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '11'");

            LabelUnidadesAcademicasNS_resumenDireccion_obs.Text = "<br><span class='badge border border-warning border-1 text-secondary' style='max-width: 150px;overflow-x: clip;'>"+ obsDir +"</span>";

            if (tDir == 0)
            {

                LabelUnidadesAcademicasNS_resumenDireccion_estatus.Text = "Vigente";
                LabelUnidadesAcademicasNS_resumenDireccion_estatus.Attributes["class"] = "badge bg-success small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenDireccion_icon.Attributes["style"] = "";
                UnidadesAcademicasNS_resumenDireccion_icon.Attributes["class"] = "bi bi-clock-history";
            }
            else
            {
                LabelUnidadesAcademicasNS_resumenDireccion_estatus.Text = "Vencido";
                LabelUnidadesAcademicasNS_resumenDireccion_estatus.Attributes["class"] = "badge bg-warning small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenDireccion_icon.Attributes["style"] = "background: #ffE599;";
                UnidadesAcademicasNS_resumenDireccion_icon.Attributes["class"] = "bi bi-clock-history text-yellow";
            }
        }
        if (existAca)
        {
            string obsAca = Consultas.ConsultaS("select SUBSTRING(au.OBSERVACION,0, CHARINDEX(' ',au.OBSERVACION)) OBSERVACION " +
                  "from AUTORIDADES_ZP au  " +
                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '12'");

            LabelUnidadesAcademicasNS_resumenAcademica_obs.Text = "<br><span class='badge border border-warning border-1 text-secondary' style='max-width: 150px;overflow-x: clip;'>"+ obsAca +"</span>";

            if (tAca == 0)
            {

                LabelUnidadesAcademicasNS_resumenAcademica_estatus.Text = "Vigente";
                LabelUnidadesAcademicasNS_resumenAcademica_estatus.Attributes["class"] = "badge bg-success small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenAcademica_icon.Attributes["style"] = "";
                UnidadesAcademicasNS_resumenAcademica_icon.Attributes["class"] = "bi bi-clock-history";
            }
            else
            {
                LabelUnidadesAcademicasNS_resumenAcademica_estatus.Text ="Vencido";
                LabelUnidadesAcademicasNS_resumenAcademica_estatus.Attributes["class"] = "badge bg-warning small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenAcademica_icon.Attributes["style"] = "background: #ffE599;";
                UnidadesAcademicasNS_resumenAcademica_icon.Attributes["class"] = "bi bi-clock-history text-yellow";
            }
        }
        if (existSseis)
        {
            string obsSseis = Consultas.ConsultaS("select SUBSTRING(au.OBSERVACION,0, CHARINDEX(' ',au.OBSERVACION)) OBSERVACION " +
                  "from AUTORIDADES_ZP au  " +
                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '13'");

            LabelUnidadesAcademicasNS_resumenSSEIS_obs.Text = "<br><span class='badge border border-warning border-1 text-secondary' style='max-width: 150px;overflow-x: clip;'>"+ obsSseis +"</span>";

            if (tSseis == 0)
            {
                LabelUnidadesAcademicasNS_resumenSSEIS_estatus.Text = "Vigente";
                LabelUnidadesAcademicasNS_resumenSSEIS_estatus.Attributes["class"] = "badge bg-success small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenSSEIS_icon.Attributes["style"] = "";
                UnidadesAcademicasNS_resumenSSEIS_icon.Attributes["class"] = "bi bi-clock-history";
            }
            else
            {
                LabelUnidadesAcademicasNS_resumenSSEIS_estatus.Text = "Vencido";
                LabelUnidadesAcademicasNS_resumenSSEIS_estatus.Attributes["class"] = "badge bg-warning small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenSSEIS_icon.Attributes["style"] = "background: #ffE599;";
                UnidadesAcademicasNS_resumenSSEIS_icon.Attributes["class"] = "bi bi-clock-history text-yellow";
            }
        }
        if (existAdm)
        {
            string obsAdm = Consultas.ConsultaS("select SUBSTRING(au.OBSERVACION,0, CHARINDEX(' ',au.OBSERVACION)) OBSERVACION " +
                  "from AUTORIDADES_ZP au  " +
                  "inner join USERS us on us.ID_USER = au.ID_USER " +
                  "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                  "where au.CLAVE_ZP = '"+ zp +"' and au.ID_PERFIL = '14'");

            LabelUnidadesAcademicasNS_resumenAdministracion_obs.Text = "<br><span class='badge border border-warning border-1 text-secondary' style='max-width: 150px;overflow-x: clip;'>"+ obsAdm +"</span>";

            if (tAdm == 0)
            {
                LabelUnidadesAcademicasNS_resumenAdministracion_estatus.Text = "Vigente";
                LabelUnidadesAcademicasNS_resumenAdministracion_estatus.Attributes["class"] = "badge bg-success small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenAdministracion_icon.Attributes["style"] = "";
                UnidadesAcademicasNS_resumenAdministracion_icon.Attributes["class"] = "bi bi-clock-history";
            }
            else
            {
                LabelUnidadesAcademicasNS_resumenAdministracion_estatus.Text = "Vencido";
                LabelUnidadesAcademicasNS_resumenAdministracion_estatus.Attributes["class"] = "badge bg-warning small pt-1 fw-bold";
                DivUnidadesAcademicasNS_resumenAdministracion_icon.Attributes["style"] = "background: #ffE599;";
                UnidadesAcademicasNS_resumenAdministracion_icon.Attributes["class"] = "bi bi-clock-history text-yellow";
            }
        }
    }
    protected void LinkButtonAlertUnidadAcademica_resumen_cerrar_Click(object sender, EventArgs e)
    {
        LimpiarValoresZP_resumen_seleccionada();
    }
    private void LimpiarValoresZP_resumen_seleccionada()
    {
        LabelZP.Text = string.Empty;
        LabelUnidadAcademicaResumenSeleccionada_nombre.Text = string.Empty;
        divAlertUnidadAcademicaResumen_seleccionada.Visible = false;

        DataBindGridViewUnidadesAcademicasNS_resumen();
        AsignarValoresZP_resumen_seleccionada("");
    }
    protected void LinkButtonUnidadesAcademicasNS_resumenDireccion_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalDetalleUnidadAdministrativa";
        HiddenFieldUnidadesAcademicasNS_idPerfil.Value = "11";

        ValidarCoberturaInformacion();
        ShowModal(IdModal);
    }
    protected void LinkButtonUnidadesAcademicasNS_resumenAcademica_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalDetalleUnidadAdministrativa";
        HiddenFieldUnidadesAcademicasNS_idPerfil.Value = "12";

        ValidarCoberturaInformacion();
        ShowModal(IdModal);
    }
    protected void LinkButtonUnidadesAcademicasNS_resumenSSEIS_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalDetalleUnidadAdministrativa";
        HiddenFieldUnidadesAcademicasNS_idPerfil.Value = "13";

        ValidarCoberturaInformacion();
        ShowModal(IdModal);
    }
    protected void LinkButtonUnidadesAcademicasNS_resumenAdministrativa_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalDetalleUnidadAdministrativa";
        HiddenFieldUnidadesAcademicasNS_idPerfil.Value = "14";

        ValidarCoberturaInformacion();
        ShowModal(IdModal);
    }

    private void DetalleModalPerfilAutoridad(string zp)
    {
        string idPerfil = HiddenFieldUnidadesAcademicasNS_idPerfil.Value;
        string unidadAdministrativa = "";

        switch (idPerfil)
        {
            case "11":
                unidadAdministrativa = "Dirección";
                break;
            case "12":
                unidadAdministrativa = "Subdirección Académica";
                break;
            case "13":
                unidadAdministrativa = "Subdirección de Servicios Educativos e Integración Social";
                break;
            case "14":
                unidadAdministrativa = "Subdirección Administrativa";
                break;
        }

        if (!String.IsNullOrEmpty(zp)) LabelModalDetalleUnidadAdministrativa_subtitle.Text = GetUaDesciption(zp);
        if (!String.IsNullOrEmpty(idPerfil)) LabelModalDetalleUnidadAdministrativa_title.Text = "Persona titular de la "+ unidadAdministrativa;
    }
    private void ValidarCoberturaInformacion()
    {
        string zp = LabelZP.Text;

        DivCardDatosAutoridades_nombramiento.Visible = false;

        if (String.IsNullOrEmpty(zp))
        {
            DropDownListUnidadAcademica_resumen.SelectedIndex = -1;

            LabelModalDetalleUnidadAdministrativa_title.Text = "Resúmen de vigencia de nombramientos vencidos";
            LabelModalDetalleUnidadAdministrativa_subtitle.Text = emptyZP_name;

            DivDetalleDropDownList_seccion.Visible = true;
            DivDetallePerfil_seccion.Visible = false;            
        }
        else
        {
            DetalleModalPerfilAutoridad(zp);

            DivDetalleDropDownList_seccion.Visible = false;
            DivDetallePerfil_seccion.Visible = true;
            cargarDatosAutoridad();
        }
    }



    protected void LinkButtonUnidadesAcademicasNS_resumen_nombramientos_Click(object sender, EventArgs e)
    {
        string zp = LabelZP.Text;
        string IdModal = "ModalResumenNombramientos";
        LabelModalResumenNombramientos_titulo.Text = "Resúmen de vigencia de nombramientos";
        LabelModalResumenNombramientos_subtitulo.Text = String.IsNullOrEmpty(zp) ==  true ? emptyZP_name : GetUaDesciption(zp);
        
        GridViewResumenNombramientos.EditIndex = -1;
        HiddenFieldNombramientosEstatus_edicion.Value = String.Empty;

        DataBingGridViewResumenNombramientos();
        ShowModal(IdModal);
    }

    private void DataBingGridViewResumenNombramientos()
    {
        string zp = LabelZP.Text;

        string qryNS = "select au.CLAVE_ZP, au.ID_PERFIL, au.ID_USER, dp.DESCRIPCION_DP, per.DESCRIPCION,us.APELLIDO_PAT,us.APELLIDO_MAT,us.NOMBRE, CONCAT(us.APELLIDO_PAT,' ',us.APELLIDO_MAT,' ',us.NOMBRE) NOMBRE_COMPLETO, FORMAT(au.FECHA_INICIO, 'dd/MM/yyyy', 'es-ES') FECHA_INICIO, " +
            "IIF(SUBSTRING(OBSERVACION,1,3) = 'pró','en prórroga',FORMAT(au.FECHA_FIN, 'dd/MM/yyyy', 'es-ES')) FECHA_FIN, " +
            "case " +
				"when (DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN)) >= 1 then 'VIGENTE'  " +
                "when (DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN)) <= 0 then 'VENCIDO'  " +
            "end NOMBRAMIENTO_EST," + 
            "(select CONCAT(CLAVE_ZP,'_',(FORMAT(FECHA_INICIO,'yyyy')),'_',ID_PERFIL,'_',ID_USER,'.pdf') NOMBRAMIENTO from AUTORIDADES_ZP where ID_USER= au.ID_USER) PDF " +
                        "from AUTORIDADES_ZP au " +
                            "inner join CAT_DEPENDENCIAS_POLITECNICAS dp on dp.CLAVE_ZP = au.CLAVE_ZP " +
                            "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                            "inner join USERS us on us.ID_USER = au.ID_USER " +
                        "where au.ESTATUS = 1 and au.CLAVE_ZP like '"+ zp +"%'" +
                        "order by dp.DESCRIPCION_DP asc";

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    this.GridViewResumenNombramientos.DataSource = dt;
                    GridViewResumenNombramientos.DataBind();
                    GridViewResumenNombramientos.PageIndex = 0;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }
    protected void LinkButtonModalResumenNombramientos_Excel_Click(object sender, EventArgs e)
    {
        string nameReport = "NombramientosNS_";
        string zp = LabelZP.Text;

        if (String.IsNullOrEmpty(zp))
        {
            zp = "IPN";
        }

        string qryNS = "select au.CLAVE_ZP, dp.DESCRIPCION_DP, per.DESCRIPCION, CONCAT(us.APELLIDO_PAT,' ',us.APELLIDO_MAT,' ',us.NOMBRE) NOMBRE, FORMAT(au.FECHA_INICIO, 'dd/MM/yyyy', 'es-ES') FECHA_INICIO, FORMAT(au.FECHA_FIN, 'dd/MM/yyyy', 'es-ES') FECHA_FIN, " +
                        "case " +
                            "when (DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN)) >= 1 then 'ACTIVO'  " +
                            "when (DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN)) <= 0 then 'VENCIDO'  " +
                        "end NOMBRAMIENTO_EST " +
                       "from AUTORIDADES_ZP au " +
                           "inner join CAT_DEPENDENCIAS_POLITECNICAS dp on dp.CLAVE_ZP = au.CLAVE_ZP " +
                           "inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL " +
                           "inner join USERS us on us.ID_USER = au.ID_USER " +
                       "where au.ESTATUS = 1 and au.CLAVE_ZP like '"+ zp +"%'" +
                       "order by dp.DESCRIPCION_DP asc";

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    XLWorkbook workbook = new XLWorkbook();

                    // Add worksheet with data
                    var worksheet = workbook.Worksheets.Add(dt, zp);

                    // Formattings Sheet
                    worksheet.Table(0).Theme = XLTableTheme.TableStyleLight20;
                    worksheet.Row(1).Style.Font.Bold = true;
                    worksheet.SheetView.FreezeRows(1);
                    worksheet.Columns().AdjustToContents(10.0, 50.0);

                    // Convert workbook into stream to download
                    var stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    // Download ExcelFile from Bynary Data
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + nameReport + zp + ".xlsx");
                    HttpContext.Current.Response.BinaryWrite(stream.ToArray());

                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }

    protected void GridViewResumenNombramientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void GridViewResumenNombramientos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string perfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();
        string nombramiento = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PDF"));//
        bool exists = verificarNombramiento(nombramiento);

        bool editar = HiddenFieldNombramientosEstatus_edicion.Value == "editar" ? true : false;

        switch (perfil)
        {
            case "432":

                if (editar == true)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;
 
                        e.Row.Cells[12].Visible = false;
                        e.Row.Cells[13].Visible = false;
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[6].Visible = false;

                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;

                        e.Row.Cells[12].Visible = false;
                        e.Row.Cells[13].Visible = false;
                    }
                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                    }
                }

                    break;
            default:
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;

                    e.Row.Cells[14].Visible = false;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;

                    e.Row.Cells[14].Visible = false;

                }
                break;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (exists == false)
            {
                e.Row.Cells[13].Text = "<i class='bi bi-file-earmark-x icon-orange'></i>";
            }
        }
    }


    protected void LinkButtonResumenNombramientos_editar_Click(object sender, EventArgs e)
    {

    }
    private bool verificarNombramiento(string nombrePdf)
    {
        string ruta = HttpContext.Current.Server.MapPath(@"~/public/src/nombramientos/autoridades/"+ nombrePdf +"");
        
        FrameModalVisualizarNombramiento_pdf.Attributes["src"] =  "";

        bool exists = System.IO.File.Exists(ruta);

        return exists;
    }
    protected void LinkButtonResumenNombramientos_pdf_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });

        string ua = commandArgs[0];
        string dp = commandArgs[1];
        string nombre = commandArgs[2];
        string pdf = commandArgs[3];

        //DataBingGridViewResumenNombramientos();
        pdf = verificarNombramiento(pdf) == true ? pdf : "sin_documento.pdf";

        LabelModalVisualizarNombramiento_titulo.Text = ua + " | " + dp;
        LabelModalVisualizarNombramiento_subtitulo.Text = nombre;

        string IdModal = "ModalVisualizarNombramiento";

        FrameModalVisualizarNombramiento_pdf.Attributes["src"] =  "~/public/src/nombramientos/autoridades/"+pdf;

        ShowModal(IdModal);
        
    }


    protected void GridViewResumenNombramientos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewResumenNombramientos.EditIndex = -1;
        HiddenFieldNombramientosEstatus_edicion.Value = String.Empty;

        DataBingGridViewResumenNombramientos();
    }

    protected void GridViewResumenNombramientos_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridViewResumenNombramientos.EditIndex = e.NewEditIndex;
        HiddenFieldNombramientosEstatus_edicion.Value = "editar";

        DataBingGridViewResumenNombramientos();
    }

    protected void GridViewResumenNombramientos_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        string zp = GridViewResumenNombramientos.Rows[e.RowIndex].Cells[1].Text;
        string idUser = GridViewResumenNombramientos.Rows[e.RowIndex].Cells[2].Text;
        string idPerfil = GridViewResumenNombramientos.Rows[e.RowIndex].Cells[3].Text;

        TextBox apat = GridViewResumenNombramientos.Rows[e.RowIndex].FindControl("TextBoxResumenNombramientos_apat") as TextBox;
        TextBox amat = GridViewResumenNombramientos.Rows[e.RowIndex].FindControl("TextBoxResumenNombramientos_amat") as TextBox;
        TextBox nombre = GridViewResumenNombramientos.Rows[e.RowIndex].FindControl("TextBoxResumenNombramientos_nombre") as TextBox;

        TextBox inicio = GridViewResumenNombramientos.Rows[e.RowIndex].FindControl("TextBoxResumenNombramientos_inicio") as TextBox;
        TextBox termino = GridViewResumenNombramientos.Rows[e.RowIndex].FindControl("TextBoxResumenNombramientos_termino") as TextBox;

        string format = "dd/MM/yyyy";
        DateTime dateI = DateTime.ParseExact(inicio.Text, format, CultureInfo.InvariantCulture);
        DateTime dateF = DateTime.ParseExact(termino.Text, format, CultureInfo.InvariantCulture);

        string feI = dateI.ToString("yyyy-MM-dd");
        string feF = dateF.ToString("yyyy-MM-dd");

        Consultas.miUpdate("update users set APELLIDO_PAT = '"+ apat.Text +"', APELLIDO_MAT = '"+ amat.Text +"', NOMBRE = '"+ nombre.Text +"' where ID_USER = '"+ idUser +"'");
        Consultas.miUpdate("update AUTORIDADES_ZP set FECHA_INICIO = '"+ feI +"', FECHA_FIN = '"+ feF +"' where CLAVE_ZP = '"+ zp +"' and ID_PERFIL = '"+ idPerfil +"' and ID_USER= '"+ idUser +"'");
          
        GridViewResumenNombramientos.EditIndex = -1;
        HiddenFieldNombramientosEstatus_edicion.Value = String.Empty;

        DataBingGridViewResumenNombramientos();
    }
    [WebMethod]
    public static List<object> chartBarNombramientos_datos(string zp)
    {
        List<object> chartData = new List<object>();

        chartData.Add(new object[]
            {
                "TITULAR", "INTERINO"
            });

        string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

        bool exist = Consultas.ConsultaInt("select COUNT(ID_USER) total from AUTORIDADES_ZP where CLAVE_ZP like '"+ zp +"%'") == 0 ? false : true;

        string query = "select SUM(TITULAR) TITULAR, SUM(INTERINO) INTERINO " +
                        "FROM( " +
                            "select 0 TITULAR, 0 INTERINO " +
                            "from AUTORIDADES_ZP " +
                        ")DATOS";

        if (exist == true)
        {
            query = "select SUM(TITULAR) TITULAR, SUM(INTERINO) INTERINO " +
                    "from( " +
                        "select dp.CLAVE_ZP, " +
                            "(select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and INTERINO = 0 and ESTATUS = 1) TITULAR, " +
                            "(select COUNT(CLAVE_ZP) total from AUTORIDADES_ZP where CLAVE_ZP = dp.CLAVE_ZP and INTERINO = 1 and ESTATUS = 1) INTERINO " +
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
                            sdr["TITULAR"],sdr["INTERINO"]
                        });

                    }

                }
                con.Close();
                return chartData;
            }
        }

    }
    [WebMethod]
    public static List<object> chartPieNombramientos_datos(string zp)
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