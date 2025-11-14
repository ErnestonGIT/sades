using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;//using System.Web.UI.WebControls;
using System.Web.UI.WebControls;


public partial class Garantias : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
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
        HiddenFieldCollapseGarantias_selected.Value  = "1";
        divPanelGarantias.Visible = data;

        ActualizarEstadisticas();
    }
    public void mostrarPanelEnlace(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;

        HiddenFieldCollapseGarantias_selected.Value  = "1";
        divPanelGarantias.Visible = data;

        if (!String.IsNullOrEmpty(zp))
        {
            DropDownListRegistrarGarnatia_ua_SelectCommand();

            DropDownListRegistrarGarnatia_ua.SelectedValue = zp;
            LabelBreadCrumbZP_name.Text = LabelZPDesc.Text;
            DropDownListRegistrarGarnatia_ua.Enabled = false;
        }

        ActualizarEstadisticas();
    }
    public void mostrarPanelAdministrador(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseGarantias_selected.Value  = "1";
        divPanelGarantias.Visible = data;
        ActualizarEstadisticas();
    }

    public void ActualizarEstadisticas()
    {
        string zp = LabelZP.Text;
        LabelBreadCrumbZP_name.Text = String.IsNullOrEmpty(zp) == true ? emptyZP_name : GetUaDesciption(zp);

        string totalPendientes = Consultas.ConsultaS("select count(pet.ID_PETICION) total " +
                                                    "from PETICIONES_POR_UA pet " +
                                                    "inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO " +
                                                    "where pet.ID_PETICION not in (select distinct ID_PETICION from GARANTIA_PETICION) and pli.CLAVE_ZP like '"+ zp +"%'");

        string totalConcluidas = Consultas.ConsultaS("select count(distinct ID_PETICION) total from GARANTIA_PETICION where ESTATUS = 1  and CLAVE_ZP like '"+ zp +"%'");

        string totalCategorias = Consultas.ConsultaS("select COUNT(distinct gar.ID_PETICION) AS 'value',cat_pet.DESCRIPCION_CAT_PETICION AS 'name' " +
                                                    "from GARANTIA_PETICION gar " +
                                                    "inner join CAT_CATEGORIA_PETICION cat_pet on cat_pet.ID_CAT_PETICION = gar.ID_CAT_PETICION " +
                                                    "inner join PLIEGO pli on pli.ID_PLIEGO = gar.ID_PLIEGO " +
                                                    "where pli.CLAVE_ZP like '"+ zp +"%' " +
                                                    "group by cat_pet.DESCRIPCION_CAT_PETICION " +
                                                    "for JSON PATH ");


        HiddenFieldGraficoPieCategorias_datos.Value = totalCategorias;
        LabelGarantiasPendientes_total.Text = totalPendientes == "0" ? "0": totalPendientes;
        LabelGarantiasConcluidas_total.Text = totalConcluidas == "0" ? "0": totalConcluidas;
    }

    protected void DropDownListRegistrarGarnatia_ua_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_ua.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListRegistrarGarnatia_ua_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelZP.Text = DropDownListRegistrarGarnatia_ua.SelectedValue.ToString();

        RestaurarDropDownListGarantias(1);
        ActualizarEstadisticas();
    }
    protected void DropDownListRegistrarGarnatia_ua_SelectCommand()
    {
        SqlDataSourceDropDownRegistrarGarnatia_ua.SelectCommand = "SELECT CLAVE_ZP, DESCRIPCION_DP FROM  CAT_DEPENDENCIAS_POLITECNICAS WHERE ID_NIVEL_EST = 2 ORDER BY DESCRIPCION_DP ASC";
        DropDownListRegistrarGarnatia_ua.DataBind();
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
        DataBindDropDownListRegistrarGarantia_peticion();
    }

    protected void DropDownListRegistrarGarnatia_peticion_DataBound(object sender, EventArgs e)
    {
        DropDownListRegistrarGarnatia_peticion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }
    protected void DropDownListRegistrarGarnatia_peticion_SelectedIndexChanged(object sender, EventArgs e)
    {
        MostrarAlertPeticion();
        DataBindDropDownListRegistrarGarantia_peticion();

    }
    public void DataBindDropDownListRegistrarGarantia_peticion()
    {
        string andListId = "";
        string stringListId = HiddenFieldDivPeticiones_selected.Value;
        string categoriaId = DropDownListRegistrarGarnatia_categoria.SelectedValue.ToString();
        string pliegoId = DropDownListRegistrarGarnatia_pliego.SelectedValue.ToString();

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
                    this.DropDownListRegistrarGarnatia_peticion.DataSource = ds;
                    this.DropDownListRegistrarGarnatia_peticion.DataValueField = "ID_PETICION";
                    this.DropDownListRegistrarGarnatia_peticion.DataTextField = "DESC_PETICION";
                    this.DropDownListRegistrarGarnatia_peticion.DataBind();

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
            DataBindDropDownListRegistrarGarantia_peticion();
        }
    }
    private List<int> AgregarListaPeticionesId()
    {
        string peticionId = DropDownListRegistrarGarnatia_peticion.SelectedValue.ToString();
        string peticionAct = HiddenFieldDivPeticiones_selected.Value;

        List<int> intListId = new List<int>();

        if (!String.IsNullOrEmpty(peticionId))
        {
            int peticionIdInt = Convert.ToInt32(DropDownListRegistrarGarnatia_peticion.SelectedValue);

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


    private void RestaurarDropDownListGarantias(int nivel)
    {
        switch (nivel)
        {
            case 0:
                DropDownListRegistrarGarnatia_ua.DataBind();
                ClearAndInsertItem(DropDownListRegistrarGarnatia_pliego);
                ClearAndInsertItem(DropDownListRegistrarGarnatia_categoria);
                ClearAndInsertItem(DropDownListRegistrarGarnatia_peticion);
                break;
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

        HiddenFieldDivPeticiones_selected.Value = string.Empty;
        DivContenidoPeticiones_seleccionadas.InnerHtml = string.Empty;
        LabelFileUpload_estatus.Text = string.Empty;
        TextBoxRegistrarGarnatia_descripcion.Text = string.Empty;
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
    protected void ButtonRegistrarGarnatia_guardar_Click(object sender, EventArgs e)
    {

        if (!FileUploadRegistrarGarnatia_evidencia.HasFile)
        {
            LabelFileUpload_estatus.Text = "Debe subir un documento de evidencia.";
            return;
        }

        if (FileUploadRegistrarGarnatia_evidencia.HasFile)
        {
            GuardarDocumentoGarantias();
        }
        else
        {
            LabelFileUpload_estatus.Text = "Deberá seleccionar un archivo.";
        }
    }
    private string verificarRutaGarantias()
    {
        string zp = LabelZP.Text;
        string ruta = HttpContext.Current.Server.MapPath(@"~/public/src/garantias/"+ zp +"/");

        if (!Directory.Exists(ruta))
        {
            Directory.CreateDirectory(ruta);
        }

        return ruta;

    }
    private void GuardarDocumentoGarantias()
    {
        int sigGarantia = Consultas.ConsultaInt("select COUNT(ID_GARANTIA) +1 SIGUIENTE from GARANTIA_PETICION");
        string folio = "GAR-" + DateTime.Now.Year + "-" + sigGarantia + "-" + Guid.NewGuid().ToString().Substring(0, 3);
        string rutaDoc = verificarRutaGarantias();

        try
        {
            string zp = LabelZP.Text;
            string tipoDoc = Path.GetExtension(FileUploadRegistrarGarnatia_evidencia.PostedFile.FileName);
            string nombreDoc = folio + tipoDoc;

            rutaDoc = Path.Combine(rutaDoc, nombreDoc);

            if (File.Exists(rutaDoc))
            {
                string nuevoNombreDoc = Path.GetFileNameWithoutExtension(nombreDoc) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(nombreDoc);
                rutaDoc = Path.Combine(rutaDoc, nuevoNombreDoc);
            }

            FileUploadRegistrarGarnatia_evidencia.SaveAs(rutaDoc);
            LabelFileUpload_estatus.Text = "El archivo se cargo correctramente: " + nombreDoc;

            rutaDoc = "/public/src/garantias/"+ zp +"/"+ nombreDoc +"";

            InsertarGarantia(rutaDoc, tipoDoc);
        }
        catch (Exception ex)
        {
            LabelFileUpload_estatus.Text = "Error al cargar el archivo: " + ex.Message;
        }

    }
    private void InsertarGarantia(string rutaDocumento, string tipoDoc)
    {
        string zp = LabelZP.Text;
        string pliegoId = DropDownListRegistrarGarnatia_pliego.SelectedValue.ToString();
        string categoriaId = DropDownListRegistrarGarnatia_categoria.SelectedValue.ToString();
        string peticionId = HiddenFieldDivPeticiones_selected.Value;
        string peticionDesc = TextBoxRegistrarGarnatia_descripcion.Text;
        int idGarantia = ObtenerIdPeticion_siguiente();

        int documentoId = InsertarEvidenciaGarantia(rutaDocumento, tipoDoc);

        if (!String.IsNullOrEmpty(peticionId))
        {

            List<string> stringId = peticionId.Split(',').ToList();

            foreach (var id in stringId)
            {
                int intId = Convert.ToInt32(id);
                Consultas.miInsert("insert into GARANTIA_PETICION (ID_GARANTIA, CLAVE_ZP, ID_PLIEGO, ID_CAT_PETICION, ID_PETICION, DESC_GARANTIA, ID_DOCUMENTO) values('"+ idGarantia +"', '"+ zp +"','"+ pliegoId +"','"+ categoriaId +"','"+ intId +"','"+ peticionDesc +"','"+ documentoId +"')");
            }

        }

        RestaurarDropDownListGarantias(0);
        ActualizarEstadisticas();
        HiddenFieldMensajeRegistroExitoso_estatus.Value = "1";

    }
    private int InsertarEvidenciaGarantia(string rutaDocumento, string tipoDocumento)
    {
        int DocGarantiaId = Consultas.ConsultaInt("INSERT INTO DOCUMENTO_GARANTIA (TIPO_DOCUMENTO, RUTA_DOCUMENTO) OUTPUT INSERTED.ID_DOCUMENTO VALUES ('"+ tipoDocumento +"','"+ rutaDocumento +"')");

        return DocGarantiaId;
    }


    protected void LinkButtonPeticionId_eliminar_Click(object sender, EventArgs e)
    {
        string idPeticion = HiddenFieldPeticionEliminar_id.Value;
        EliminarListaPeticionesId(idPeticion);
    }

    protected void LinkButtonResumenGarantias_Click(object sender, EventArgs e)
    {
        string IdModal = "ModalResumenGarantias";

        ShowModal(IdModal);
        DataBingGridViewResumenGarantias();
    }
    private void DataBingGridViewResumenGarantias()
    {
        string zp = LabelZP.Text;

        string qryNS = "select IIF(pli.FOLIO_PLIEGO is null, CONCAT('PLG-',pli.CLAVE_ZP,'-',pli.ID_PLIEGO), pli.FOLIO_PLIEGO) FOLIO_PLIEGO, " +
		                        "cat_pet.DESCRIPCION_CAT_PETICION, pet.DESC_PETICION, " +
                                "gar.DESC_GARANTIA, " +
		                        "doc.RUTA_DOCUMENTO " +
                            "from GARANTIA_PETICION gar " +
                            "inner join PLIEGO pli on pli.ID_PLIEGO = gar.ID_PLIEGO " +
                            "inner join CAT_CATEGORIA_PETICION  cat_pet on cat_pet.ID_CAT_PETICION = gar.ID_CAT_PETICION " +
                            "inner join PETICIONES_POR_UA pet on pet.ID_PETICION = gar.ID_PETICION " +
                            "inner join DOCUMENTO_GARANTIA doc on doc.ID_DOCUMENTO = gar.ID_DOCUMENTO " +
                            "where gar.CLAVE_ZP like '"+ zp +"%'";

        using (SqlConnection con = new SqlConnection(constr))
        {

            using (SqlDataAdapter da = new SqlDataAdapter(qryNS, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    this.GridViewResumenGarantias.DataSource = dt;
                    GridViewResumenGarantias.DataBind();
                    GridViewResumenGarantias.PageIndex = 0;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            con.Close();
        }
    }
    protected void LinkButtonModalResumenGarantias_Excel_Click(object sender, EventArgs e)
    {

    }

    protected void GridViewResumenGarantias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void GridViewResumenGarantias_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void GridViewResumenGarantias_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }

    protected void GridViewResumenGarantias_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void LinkButtonResumenGarantias_pdf_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
        string pdf = commandArgs[0];

        LabelModalVisualizarEvidenciaGarantia_titulo.Text = "";
        LabelModalVisualizarEvidenciaGarantia_subtitulo.Text = "";

        string IdModal = "ModalVisualizarEvidenciaGarantia";

        FrameModalVisualizarEvidenciaGarantia_pdf.Attributes["src"] =  "~"+pdf;

        ShowModal(IdModal);
    }
}