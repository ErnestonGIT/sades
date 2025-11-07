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
        RestaurarDropDownListGarantias(1);
    }

    protected void DropDownListRegistrarGarnatia_pliego_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_pliego.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_pliego_SelectedIndexChanged(object sender, EventArgs e)
    {
        RestaurarDropDownListGarantias(2);
    }

    protected void DropDownListRegistrarGarnatia_categoria_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_categoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        RestaurarDropDownListGarantias(3);
    }

    protected void DropDownListRegistrarGarnatia_peticion_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_peticion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListRegistrarGarnatia_peticion_SelectedIndexChanged(object sender, EventArgs e)
    {
        string peticionId = DropDownListRegistrarGarnatia_peticion.SelectedValue.ToString();
        string peticionDe = DropDownListRegistrarGarnatia_peticion.SelectedItem.Text;

        string peticionA = HiddenFieldDivPeticiones_selected.Value;

        List<int> peticionIdList = new List<int>();
        List<string> peticionDeList = new List<string>();
        
        if (!String.IsNullOrEmpty(peticionId))
        {
            int peticionIdInt = Convert.ToInt32(DropDownListRegistrarGarnatia_peticion.SelectedValue);

            //peticion.Add(peticionId,peticionDe);
            peticionIdList.Add(peticionIdInt);
            peticionDeList.Add(peticionDe);

        }

        LabelBreadCrumbZP_name.Text = peticionDeList[0];

        if (peticionIdList.Count >= 1)
        {
            string contenido = "";

            foreach (string peti in peticionDeList)
            {
                contenido += "<div class='col-auto'> " +
                                "<div class='alert alert-warning alert-dismissible fade show' role='alert'> " +
                                    ""+ peti +"" +
                                    "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button> " +
                                "</div> " +
                            "</div>";
                //Console.WriteLine($"ID: {obj.Id}, Nombre: {obj.Name}");
            }

            DivContenidoPeticiones_seleccionadas.InnerHtml = contenido;
        }
    }

    

    private void RestaurarDropDownListGarantias(int nivel)
    {
        switch (nivel)
        {
            case 1:
                ClearAndInsertItem(DropDownListRegistrarGarnatia_pliego);
                ClearAndInsertItem(DropDownListRegistrarGarnatia_categoria);
                ClearAndInsertItem(DropDownListRegistrarGarnatia_peticion);
                break;
            case 2:
                ClearAndInsertItem(DropDownListRegistrarGarnatia_categoria);
                ClearAndInsertItem(DropDownListRegistrarGarnatia_peticion);
                break;
            case 3:
                ClearAndInsertItem(DropDownListRegistrarGarnatia_peticion);
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
}