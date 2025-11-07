using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Color = System.Drawing.Color;
using System.Collections.Generic;
using System.Web.Services;//using ClosedXML.Excel;
using System.Web.Script.Serialization;

using System.Linq;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Office.Word;
using System.Globalization;
using System.Threading;

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
                LabelZPDesc.Text = GetUaDesciption();
                mostrarPanelInicial(true);

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
    private string GetUaDesciption()
    {
        return Consultas.ConsultaS("SELECT DESCRIPCION_DP FROM CAT_DEPENDENCIAS_POLITECNICAS WHERE CLAVE_ZP = '" + LabelZP.Text + "'");
    }
    public void mostrarPanelInicial(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        divPanelInicial.Visible = data;

    }

    
    public void DataBindDropDownPeriodos()
    {
        string zp = LabelZP.Text =DropDownListUnidadAcademica.SelectedValue.ToString();
        string tipo = DropDownListPeriodo.SelectedValue.ToString();
        string whr = "";

        if (!String.IsNullOrEmpty(tipo))
        {
            whr = "where RIGHT(PERIODO_ESCOLAR,1) = '"+ tipo +"'";
        }

        string query = "select * from( " +
                            "select distinct PERIODO_ESCOLAR from CARGA_DOCENTE where CLAVE_ZP = '" + zp + "' " +
                            "union " +
                            "select distinct PERIODO_ESCOLAR from HISTORIAL_CARGA_DOCENTE where CLAVE_ZP = '" + zp + "' " +
                        ")datos " +
                        "" +
                        ""+ whr +" "+
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

        string tb = Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from CARGA_DOCENTE where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "CARGA_DOCENTE" : "HISTORIAL_CARGA_DOCENTE";

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

        tb = Consultas.ConsultaS("select COUNT(distinct PERIODO_ESCOLAR) tot from CARGA_DOCENTE where PERIODO_ESCOLAR = '" + pe + "'") == "1" ? "CARGA_DOCENTE" : "HISTORIAL_CARGA_DOCENTE";

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

        LabeltotalLun.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and LUNES != '' and ID_ASIGNATURA = '"+ asi +"'");
        LabeltotalMar.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and MARTES != '' and ID_ASIGNATURA = '"+ asi +"'");
        LabeltotalMie.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and MIERCOLES != '' and ID_ASIGNATURA = '"+ asi +"'");
        LabeltotalJue.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and JUEVES != '' and ID_ASIGNATURA = '"+ asi +"'");
        LabeltotalVie.Text = Consultas.ConsultaS("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from " + tb + " where PERIODO_ESCOLAR ='" + pe + "' and CLAVE_ZP = '" + zp + "' and VIERNES != '' and ID_ASIGNATURA = '"+ asi +"'");
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

        lu = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from "+ tb +" where ID_ASIGNATURA = '"+ ua +"' and CLAVE_ZP = '"+ zp +"' and PERIODO_ESCOLAR = '"+ pe +"' and (LUNES is not null and LUNES != '')") == 0 ? lu : "select 1 as DIA, cd.LUNES HORARIO, count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h1 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h1 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h1 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h1 +"', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h2 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h2 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h2 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h2 +"', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h3 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h3 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h3 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h3 +"', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h4 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h4 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h4 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h4 +"', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h5 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h5 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h5 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h5 +"', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h6 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h6 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h6 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h6 +"', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h7 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h7 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h7 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h7 +"', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h8 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h8 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h8 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h8 +"', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h9 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h9 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h9 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h9 +"', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h10 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h10 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h10 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h10 +"', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h11 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h11 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h11 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h11 +"', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h12 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h12 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h12 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h12 +"', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h13 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h13 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h13 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h13 +"', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h14 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h14 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h14 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h14 +"', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h15 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h15 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h15 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h15 +"', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h16 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h16 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h16 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h16 +"', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h17 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h17 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h17 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h17 +"', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h18 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h18 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h18 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h18 +"', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h19 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h19 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h19 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h19 +"', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h20 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h20 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h20 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h20 +"', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h21 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h21 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h21 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h21 +"', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h22 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h22 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h22 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h22 +"', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h23 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h23 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h23 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h23 +"', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h24 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h24 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h24 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h24 +"', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h25 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h25 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h25 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h25 +"', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h26 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h26 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h26 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h26 +"', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h27 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h27 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h27 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h27 +"', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h28 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h28 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h28 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h28 +"', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h29 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h29 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h29 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h29 +"', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.LUNES, 1, 5) < SUBSTRING('"+ h30 +"', 7, 5) and SUBSTRING(cd.LUNES, 7, 5) > SUBSTRING('"+ h30 +"', 1, 5) or (SUBSTRING(cd.LUNES, 1, 5) = SUBSTRING('"+ h30 +"', 1, 5) and SUBSTRING(cd.LUNES, 7, 5) = SUBSTRING('"+ h30 +"', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, LUNES FROM "+ tb +") as cd where cd.ID_ASIGNATURA = '"+ ua +"' and cd.CLAVE_ZP = '"+ zp +"' and cd.PERIODO_ESCOLAR = '"+ pe +"' and (cd.LUNES is not null and cd.LUNES != '')group by cd.LUNES ";
        ma = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from "+ tb +" where ID_ASIGNATURA = '"+ ua +"' and CLAVE_ZP = '"+ zp +"' and PERIODO_ESCOLAR = '"+ pe +"' and (MARTES is not null and MARTES != '')") == 0 ? ma : "union select 2 as DIA, cd.MARTES HORARIO, count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h1 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h1 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h1 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h1 +"', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h2 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h2 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h2 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h2 +"', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h3 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h3 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h3 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h3 +"', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h4 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h4 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h4 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h4 +"', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h5 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h5 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h5 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h5 +"', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h6 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h6 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h6 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h6 +"', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h7 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h7 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h7 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h7 +"', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h8 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h8 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h8 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h8 +"', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h9 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h9 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h9 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h9 +"', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h10 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h10 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h10 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h10 +"', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h11 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h11 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h11 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h11 +"', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h12 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h12 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h12 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h12 +"', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h13 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h13 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h13 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h13 +"', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h14 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h14 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h14 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h14 +"', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h15 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h15 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h15 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h15 +"', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h16 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h16 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h16 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h16 +"', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h17 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h17 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h17 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h17 +"', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h18 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h18 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h18 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h18 +"', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h19 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h19 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h19 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h19 +"', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h20 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h20 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h20 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h20 +"', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h21 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h21 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h21 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h21 +"', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h22 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h22 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h22 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h22 +"', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h23 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h23 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h23 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h23 +"', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h24 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h24 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h24 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h24 +"', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h25 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h25 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h25 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h25 +"', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h26 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h26 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h26 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h26 +"', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h27 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h27 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h27 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h27 +"', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h28 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h28 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h28 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h28 +"', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h29 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h29 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h29 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h29 +"', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.MARTES, 1, 5) < SUBSTRING('"+ h30 +"', 7, 5) and SUBSTRING(cd.MARTES, 7, 5) > SUBSTRING('"+ h30 +"', 1, 5) or (SUBSTRING(cd.MARTES, 1, 5) = SUBSTRING('"+ h30 +"', 1, 5) and SUBSTRING(cd.MARTES, 7, 5) = SUBSTRING('"+ h30 +"', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, MARTES FROM "+ tb +") as cd where cd.ID_ASIGNATURA = '"+ ua +"' and cd.CLAVE_ZP = '"+ zp +"' and cd.PERIODO_ESCOLAR = '"+ pe +"' and (cd.MARTES is not null and cd.MARTES != '')group by cd.MARTES ";
        mi = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from "+ tb +" where ID_ASIGNATURA = '"+ ua +"' and CLAVE_ZP = '"+ zp +"' and PERIODO_ESCOLAR = '"+ pe +"' and (MIERCOLES is not null and MIERCOLES != '')") == 0 ? mi : "union select 3 as DIA, cd.MIERCOLES HORARIO, count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h1 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h1 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h1 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h1 +"', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h2 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h2 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h2 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h2 +"', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h3 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h3 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h3 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h3 +"', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h4 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h4 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h4 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h4 +"', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h5 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h5 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h5 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h5 +"', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h6 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h6 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h6 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h6 +"', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h7 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h7 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h7 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h7 +"', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h8 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h8 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h8 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h8 +"', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h9 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h9 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h9 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h9 +"', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h10 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h10 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h10 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h10 +"', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h11 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h11 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h11 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h11 +"', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h12 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h12 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h12 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h12 +"', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h13 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h13 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h13 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h13 +"', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h14 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h14 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h14 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h14 +"', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h15 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h15 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h15 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h15 +"', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h16 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h16 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h16 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h16 +"', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h17 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h17 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h17 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h17 +"', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h18 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h18 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h18 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h18 +"', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h19 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h19 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h19 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h19 +"', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h20 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h20 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h20 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h20 +"', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h21 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h21 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h21 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h21 +"', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h22 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h22 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h22 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h22 +"', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h23 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h23 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h23 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h23 +"', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h24 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h24 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h24 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h24 +"', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h25 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h25 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h25 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h25 +"', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h26 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h26 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h26 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h26 +"', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h27 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h27 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h27 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h27 +"', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h28 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h28 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h28 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h28 +"', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h29 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h29 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h29 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h29 +"', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.MIERCOLES, 1, 5) < SUBSTRING('"+ h30 +"', 7, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) > SUBSTRING('"+ h30 +"', 1, 5) or (SUBSTRING(cd.MIERCOLES, 1, 5) = SUBSTRING('"+ h30 +"', 1, 5) and SUBSTRING(cd.MIERCOLES, 7, 5) = SUBSTRING('"+ h30 +"', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, MIERCOLES FROM "+ tb +") as cd where cd.ID_ASIGNATURA = '"+ ua +"' and cd.CLAVE_ZP = '"+ zp +"' and cd.PERIODO_ESCOLAR = '"+ pe +"' and (cd.MIERCOLES is not null and cd.MIERCOLES != '')group by cd.MIERCOLES ";
        ju = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from "+ tb +" where ID_ASIGNATURA = '"+ ua +"' and CLAVE_ZP = '"+ zp +"' and PERIODO_ESCOLAR = '"+ pe +"' and (JUEVES is not null and JUEVES != '')") == 0 ? ju : "union select 4 as DIA, cd.JUEVES HORARIO, count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h1 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h1 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h1 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h1 +"', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h2 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h2 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h2 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h2 +"', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h3 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h3 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h3 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h3 +"', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h4 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h4 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h4 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h4 +"', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h5 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h5 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h5 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h5 +"', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h6 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h6 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h6 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h6 +"', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h7 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h7 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h7 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h7 +"', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h8 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h8 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h8 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h8 +"', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h9 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h9 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h9 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h9 +"', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h10 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h10 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h10 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h10 +"', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h11 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h11 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h11 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h11 +"', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h12 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h12 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h12 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h12 +"', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h13 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h13 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h13 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h13 +"', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h14 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h14 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h14 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h14 +"', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h15 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h15 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h15 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h15 +"', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h16 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h16 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h16 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h16 +"', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h17 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h17 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h17 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h17 +"', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h18 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h18 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h18 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h18 +"', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h19 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h19 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h19 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h19 +"', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h20 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h20 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h20 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h20 +"', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h21 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h21 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h21 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h21 +"', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h22 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h22 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h22 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h22 +"', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h23 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h23 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h23 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h23 +"', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h24 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h24 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h24 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h24 +"', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h25 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h25 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h25 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h25 +"', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h26 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h26 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h26 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h26 +"', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h27 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h27 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h27 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h27 +"', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h28 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h28 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h28 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h28 +"', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h29 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h29 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h29 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h29 +"', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.JUEVES, 1, 5) < SUBSTRING('"+ h30 +"', 7, 5) and SUBSTRING(cd.JUEVES, 7, 5) > SUBSTRING('"+ h30 +"', 1, 5) or (SUBSTRING(cd.JUEVES, 1, 5) = SUBSTRING('"+ h30 +"', 1, 5) and SUBSTRING(cd.JUEVES, 7, 5) = SUBSTRING('"+ h30 +"', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, JUEVES FROM "+ tb +") as cd where cd.ID_ASIGNATURA = '"+ ua +"' and cd.CLAVE_ZP = '"+ zp +"' and cd.PERIODO_ESCOLAR = '"+ pe +"' and (cd.JUEVES is not null and cd.JUEVES != '')group by cd.JUEVES ";
        vi = Consultas.ConsultaInt("select COUNT(distinct SECUENCIA+ID_ASIGNATURA) total from "+ tb +" where ID_ASIGNATURA = '"+ ua +"' and CLAVE_ZP = '"+ zp +"' and PERIODO_ESCOLAR = '"+ pe +"' and (VIERNES is not null and VIERNES != '')") == 0 ? vi : "union select 5 as DIA, cd.VIERNES HORARIO, count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h1 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h1 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h1 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h1 +"', 7, 5)))then 1 else null end) h1,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h2 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h2 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h2 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h2 +"', 7, 5)))then 1 else null end) h2,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h3 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h3 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h3 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h3 +"', 7, 5)))then 1 else null end) h3,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h4 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h4 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h4 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h4 +"', 7, 5)))then 1 else null end) h4,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h5 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h5 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h5 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h5 +"', 7, 5)))then 1 else null end) h5,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h6 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h6 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h6 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h6 +"', 7, 5)))then 1 else null end) h6,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h7 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h7 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h7 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h7 +"', 7, 5)))then 1 else null end) h7,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h8 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h8 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h8 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h8 +"', 7, 5)))then 1 else null end) h8,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h9 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h9 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h9 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h9 +"', 7, 5)))then 1 else null end) h9,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h10 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h10 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h10 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h10 +"', 7, 5)))then 1 else null end) h10,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h11 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h11 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h11 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h11 +"', 7, 5)))then 1 else null end) h11,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h12 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h12 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h12 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h12 +"', 7, 5)))then 1 else null end) h12,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h13 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h13 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h13 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h13 +"', 7, 5)))then 1 else null end) h13,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h14 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h14 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h14 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h14 +"', 7, 5)))then 1 else null end) h14,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h15 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h15 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h15 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h15 +"', 7, 5)))then 1 else null end) h15,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h16 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h16 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h16 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h16 +"', 7, 5)))then 1 else null end) h16,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h17 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h17 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h17 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h17 +"', 7, 5)))then 1 else null end) h17,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h18 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h18 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h18 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h18 +"', 7, 5)))then 1 else null end) h18,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h19 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h19 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h19 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h19 +"', 7, 5)))then 1 else null end) h19,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h20 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h20 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h20 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h20 +"', 7, 5)))then 1 else null end) h20,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h21 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h21 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h21 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h21 +"', 7, 5)))then 1 else null end) h21,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h22 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h22 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h22 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h22 +"', 7, 5)))then 1 else null end) h22,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h23 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h23 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h23 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h23 +"', 7, 5)))then 1 else null end) h23,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h24 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h24 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h24 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h24 +"', 7, 5)))then 1 else null end) h24,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h25 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h25 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h25 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h25 +"', 7, 5)))then 1 else null end) h25,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h26 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h26 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h26 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h26 +"', 7, 5)))then 1 else null end) h26,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h27 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h27 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h27 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h27 +"', 7, 5)))then 1 else null end) h27,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h28 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h28 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h28 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h28 +"', 7, 5)))then 1 else null end) h28,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h29 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h29 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h29 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h29 +"', 7, 5)))then 1 else null end) h29,count(case when (SUBSTRING(cd.VIERNES, 1, 5) < SUBSTRING('"+ h30 +"', 7, 5) and SUBSTRING(cd.VIERNES, 7, 5) > SUBSTRING('"+ h30 +"', 1, 5) or (SUBSTRING(cd.VIERNES, 1, 5) = SUBSTRING('"+ h30 +"', 1, 5) and SUBSTRING(cd.VIERNES, 7, 5) = SUBSTRING('"+ h30 +"', 7, 5)))then 1 else null end) h30 from (SELECT DISTINCT SECUENCIA, ID_ASIGNATURA, CLAVE_ZP, PERIODO_ESCOLAR, VIERNES FROM "+ tb +") as cd where cd.ID_ASIGNATURA = '"+ ua +"' and cd.CLAVE_ZP = '"+ zp +"' and cd.PERIODO_ESCOLAR = '"+ pe +"' and (cd.VIERNES is not null and cd.VIERNES != '')group by cd.VIERNES ";

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

    protected void DropDownListModalidad_DataBound(object sender, EventArgs e)
    {
        DropDownListModalidad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

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
        ClearAndInsertItem(DropDownListProgramaAcad);
        ClearAndInsertItem(DropDownListPlanEstudio);
        ClearAndInsertItem(DropDownListUnidadAprend);
        ClearCheckPeriodo();

        if (DropDownListUnidadAcademica.SelectedIndex == 0)
        {
            ClearAndInsertItem(DropDownListModalidad);
        }

        ClearLblCalcularEstadisticos();
        DataBindDropDownPeriodos();
    }

    protected void DropDownListModalidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAndInsertItem(DropDownListPlanEstudio);
        ClearAndInsertItem(DropDownListUnidadAprend);
        ClearLblCalcularEstadisticos();
        ClearCheckPeriodo();

        if (DropDownListModalidad.SelectedIndex == 0)
        {
            ClearAndInsertItem(DropDownListProgramaAcad);
        }
    }

    protected void DropDownListProgramaAcad_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAndInsertItem(DropDownListUnidadAprend);
        ClearCheckPeriodo();

        if (DropDownListProgramaAcad.SelectedIndex == 0)
        {
            ClearAndInsertItem(DropDownListPlanEstudio);
        }

        ClearLblCalcularEstadisticos();
    }

    protected void DropDownListPlanEstudio_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearCheckPeriodo();
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
        }
    }

    protected void DropDownListPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListUnidadAprend.SelectedIndex == 0)
        {
            //GridViewOcupabilidad.DataSourceID = "SqlDataSourceGrupoPerioodo";
            //GridViewOcupabilidad.DataBind();
        }
        else if (DropDownListUnidadAprend.SelectedIndex >= 1)
        {
            GridViewOcupabilidad.DataSourceID = "SqlDataSourcePEParImpar";
            GridViewOcupabilidad.DataBind();
            divGridOcupo.Visible = true;
            divCalculos.Visible = true;
            //CalcularEstadisticos();
            CalcularEstadisticosSql();
            //CalcularEstadisticosSqlDetail();
            divDetailGraf.Visible = true;
        }
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
            SELECT hcd.PERIODO_ESCOLAR, (hcd.SECUENCIA+hcd.ID_ASIGNATURA) grupos, ALUMNOS, 
	            (SELECT AVG(alumnos) 
			            FROM HISTORIAL_CARGA_DOCENTE
			            WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				            AND MODALIDAD = hcd.MODALIDAD
				            AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				            AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		            ) AS PROMEDIO
		            FROM HISTORIAL_CARGA_DOCENTE hcd
		            INNER JOIN PLANES_ESTUDIO pe
			            ON hcd.CLAVE_ZP = pe.CLAVE_ZP
			            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
			            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		            WHERE hcd.CLAVE_ZP = @clave
			            AND hcd.MODALIDAD = @modalidad
			            AND hcd.ID_ASIGNATURA = @asignatura
                        AND PERIODO_ESCOLAR like  '%' + @periodo + ''
		            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, 
		            hcd.MODALIDAD, hcd.ID_ASIGNATURA, hcd.SECUENCIA, 
		            hcd.ALUMNOS
		    )
            SELECT 
                ROUND(AVG((CAST(PROMEDIO AS DECIMAL(10, 2)))),0)  AS Media,
                ROUND(VARP((CAST(PROMEDIO AS DECIMAL(10, 2)))),0)  AS Varianza,
                ROUND(STDEVP((CAST(PROMEDIO AS DECIMAL(10, 2)))),0) AS DesviacionEstandar
            FROM Datos;";
        }
        else
        {
            estadisticosSQL = @"
            WITH Datos AS
            (
            SELECT hcd.PERIODO_ESCOLAR, (hcd.SECUENCIA+hcd.ID_ASIGNATURA) grupos, ALUMNOS, 
	            (SELECT AVG(alumnos) 
			            FROM HISTORIAL_CARGA_DOCENTE
			            WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				            AND MODALIDAD = hcd.MODALIDAD
				            AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				            AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		            ) AS PROMEDIO
		            FROM HISTORIAL_CARGA_DOCENTE hcd
		            INNER JOIN PLANES_ESTUDIO pe
			            ON hcd.CLAVE_ZP = pe.CLAVE_ZP
			            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
			            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		            WHERE hcd.CLAVE_ZP = @clave
			            AND hcd.MODALIDAD = @modalidad
			            AND hcd.ID_ASIGNATURA = @asignatura
		            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, 
		            hcd.MODALIDAD, hcd.ID_ASIGNATURA, hcd.SECUENCIA, 
		            hcd.ALUMNOS
		    )
            SELECT 
                ROUND(AVG((CAST(PROMEDIO AS DECIMAL(10, 2)))),0)  AS Media,
                ROUND(VARP((CAST(PROMEDIO AS DECIMAL(10, 2)))),0)  AS Varianza,
                ROUND(STDEVP((CAST(PROMEDIO AS DECIMAL(10, 2)))),0) AS DesviacionEstandar
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
            cmd.Parameters.AddWithValue("@modalidad", DropDownListModalidad.SelectedValue);
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

                lblMedia.Text = "Media: <b>" + media.ToString("0.00") + "</b>";
                lblVarianza.Text = "Varianza: <b>" + varianza.ToString("0.00") + "</b>";
                lblDesv.Text = "Desviación estándar: <b>" + desviacion.ToString("0.00") + "</b>";

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
            SELECT hcd.PERIODO_ESCOLAR, hcd.SECUENCIA AS GRUPO,
		            (SELECT AVG(alumnos) 
			            FROM HISTORIAL_CARGA_DOCENTE
			            WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				            AND MODALIDAD = hcd.MODALIDAD
				            AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				            AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		            ) AS PROMEDIO
            FROM HISTORIAL_CARGA_DOCENTE hcd
            INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
            WHERE hcd.CLAVE_ZP = @clave
	            AND hcd.MODALIDAD = @modalidad
	            AND hcd.ID_ASIGNATURA = @asignatura
                AND PERIODO_ESCOLAR like  '%' + @periodo + ''
            GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, 
			            hcd.ID_ASIGNATURA, hcd.SECUENCIA, hcd.ALUMNOS
            ORDER BY hcd.PERIODO_ESCOLAR";
        }
        else
        {
            estadisticosSQL = @"
            SELECT hcd.PERIODO_ESCOLAR, hcd.SECUENCIA AS GRUPO,
		            (SELECT AVG(alumnos) 
			            FROM HISTORIAL_CARGA_DOCENTE
			            WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				            AND MODALIDAD = hcd.MODALIDAD
				            AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				            AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		            ) AS PROMEDIO
            FROM HISTORIAL_CARGA_DOCENTE hcd
            INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	            AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	            AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
            WHERE hcd.CLAVE_ZP = @clave
	            AND hcd.MODALIDAD = @modalidad
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
            cmd.Parameters.AddWithValue("@modalidad", DropDownListModalidad.SelectedValue);
            cmd.Parameters.AddWithValue("@asignatura", DropDownListUnidadAprend.SelectedValue);
            cmd.Parameters.AddWithValue("@periodo", DropDownListPeriodo.SelectedValue);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string periodo = dr["PERIODO_ESCOLAR"].ToString();
                double promedio = Convert.ToDouble(dr["PROMEDIO"]);
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

        double varianza = valores.Count > 1 ? sumaDesv2 / (valores.Count - 1) : 0;
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
        LabelModalDetail_modalidad.Text = DropDownListModalidad.SelectedItem.Text;
        LabelModalDetail_programaAcad.Text = DropDownListProgramaAcad.SelectedItem.Text;
        LabelModalDetail_planEst.Text = DropDownListPlanEstudio.SelectedValue;
        LabelModalDetail_unidadAcad.Text = DropDownListUnidadAprend.SelectedItem.Text;

        LabelModalDetail_periodoPar.Text = (DropDownListPeriodo.SelectedIndex >= 1) ? DropDownListPeriodo.SelectedItem.Text : string.Empty;

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
    public static string ObtenerDatosOcupabilidad(string clave, string modalidad, string asignatura, string periodo)
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
                       (
                           (SELECT SUM(t1.ALUMNOS) 
                               FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
                                       FROM HISTORIAL_CARGA_DOCENTE
                                       WHERE CLAVE_ZP = hcd.CLAVE_ZP 
                                           AND MODALIDAD = hcd.MODALIDAD
                                           AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
                                           AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR
                               ) AS t1
                           ) / COUNT (DISTINCT SECUENCIA)
                       ) as PROMEDIO 
                FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
                WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
                 AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
                 AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                 AND hcd.CLAVE_ZP = @clave
                 AND hcd.MODALIDAD = @modalidad
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
                       (
                           (SELECT SUM(t1.ALUMNOS) 
                               FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
                                       FROM HISTORIAL_CARGA_DOCENTE
                                       WHERE CLAVE_ZP = hcd.CLAVE_ZP 
                                           AND MODALIDAD = hcd.MODALIDAD
                                           AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
                                           AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR
                               ) AS t1
                           ) / COUNT (DISTINCT SECUENCIA)
                       ) as PROMEDIO 
                FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
                WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
                 AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
                 AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                 AND hcd.CLAVE_ZP = @clave
                 AND hcd.MODALIDAD = @modalidad
                 AND hcd.ID_ASIGNATURA = @asignatura
                GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA    
                ORDER BY hcd.PERIODO_ESCOLAR";
            }

            SqlCommand cmd = new SqlCommand(queryDatos, conn);
            cmd.Parameters.AddWithValue("@clave", clave);
            cmd.Parameters.AddWithValue("@modalidad", modalidad);
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

        // ⚠ Si no hay datos
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

        // 🔹 SEGUNDA CONSULTA: cálculos estadísticos reales basados en AVG(alumnos)
        List<double> listaPromedios = new List<double>();
        using (SqlConnection conn2 = new SqlConnection(connectionString))
        {
            string queryStats;

            if (periodoInt >= 1)
            {
                queryStats = @"
                SELECT 
                    (SELECT AVG(alumnos) 
                     FROM HISTORIAL_CARGA_DOCENTE
                     WHERE CLAVE_ZP = hcd.CLAVE_ZP 
                         AND MODALIDAD = hcd.MODALIDAD
                         AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
                         AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR) AS PROMEDIO
                FROM HISTORIAL_CARGA_DOCENTE hcd
                INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	                AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	                AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                WHERE hcd.CLAVE_ZP = @clave
	                AND hcd.MODALIDAD = @modalidad
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
                    (SELECT AVG(alumnos) 
                     FROM HISTORIAL_CARGA_DOCENTE
                     WHERE CLAVE_ZP = hcd.CLAVE_ZP 
                         AND MODALIDAD = hcd.MODALIDAD
                         AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
                         AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR) AS PROMEDIO
                FROM HISTORIAL_CARGA_DOCENTE hcd
                INNER JOIN PLANES_ESTUDIO pe ON hcd.CLAVE_ZP = pe.CLAVE_ZP
	                AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
	                AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
                WHERE hcd.CLAVE_ZP = @clave
	                AND hcd.MODALIDAD = @modalidad
	                AND hcd.ID_ASIGNATURA = @asignatura
                GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, 
                         hcd.ID_ASIGNATURA, hcd.SECUENCIA, hcd.ALUMNOS
                ORDER BY hcd.PERIODO_ESCOLAR";
            }

            SqlCommand cmd2 = new SqlCommand(queryStats, conn2);
            cmd2.Parameters.AddWithValue("@clave", clave);
            cmd2.Parameters.AddWithValue("@modalidad", modalidad);
            cmd2.Parameters.AddWithValue("@asignatura", asignatura);
            if (periodoInt >= 1) cmd2.Parameters.AddWithValue("@periodo", periodo);

            conn2.Open();
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                listaPromedios.Add(Convert.ToDouble(dr2["PROMEDIO"]));
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
            Media = Math.Round(media, 0),
            Varianza = Math.Round(var, 0),
            Desviacion = Math.Round(desviacion, 0)
        };

        return new JavaScriptSerializer().Serialize(resultado);
    }


    protected void LinkButtonGraf_Click(object sender, EventArgs e)
    {
        LabelModalGrafico_claveZp.Text = DropDownListUnidadAcademica.SelectedItem.Text;
        LabelModalGrafico_modalidad.Text = DropDownListModalidad.SelectedItem.Text;
        LabelModalGrafico_programaAcad.Text = DropDownListProgramaAcad.SelectedItem.Text;
        LabelModalGrafico_planEst.Text = DropDownListPlanEstudio.SelectedValue;
        LabelModalGrafico_unidadAcad.Text = DropDownListUnidadAprend.SelectedItem.Text;

        string javaScript2 = "mostrarGraficos(); ShowModalGraficos();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script2", javaScript2, true);
    }
    /*************************************************************/

}