using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;

public partial class Bitacora : System.Web.UI.Page
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

                validarPerfil(perfil);

            }
            else
            {


            }
        }


    }
    protected void validarPerfil(string idPerfil)
    {
        switch (idPerfil)
        {
            case "432":
                mostrarPanelInicial(true);
                break;
            default:
                Response.Redirect("Dashboard.aspx");
                break;
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


    protected void GridViewOffCanvasUnidadesAcademicas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {

    }

    protected void LinkButtonVaciar_bitacora_Click(object sender, EventArgs e)
    {
        Consultas.Sentencia("DELETE from bitacora");
    }
}