using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Garantias : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    string emptyZP_name = "Instituto Politécnico Nacional";

    string perfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();
    string idUser = HttpContext.Current.Request.Cookies["id_usuario"].Value.ToString();
    string chP = HttpContext.Current.Request.Cookies["chP"].Value.ToString();
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
                LabelZP.Text = string.Empty;
                mostrarPanelDirector(true);
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
        HiddenFieldCollapseGarantias_selected.Value  = "1";
        divPanelGarantias.Visible = data;
    }
    public void mostrarPanelAdministrador(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseGarantias_selected.Value  = "1";
        divPanelGarantias.Visible = data;
    }

    protected void DropDownListRegistrarGarnatia_ua_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_ua.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_ua_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListRegistrarGarnatia_pliego_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_pliego.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_pliego_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListRegistrarGarnatia_categoria_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_categoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListRegistrarGarnatia_peticion_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_peticion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_peticion_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}