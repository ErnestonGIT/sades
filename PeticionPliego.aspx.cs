using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PeticionPliego : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
    //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;
    string CLAVEZP = HttpContext.Current.Request.Cookies["claveZP"].Value.ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LabelDependencia.Text = DependenciaIPN.ObtenerNombreDependencia(CLAVEZP);
          
        }
        else
        {
            MainContentDivsAddEspec.Controls.Clear();
            GenerateDivsForEspecifico(LblFolioPliego.Text, LblIdPliego.Text, MainContentDivsAddEspec);
            MainContentDivsAddRespuesta.Controls.Clear();
            GenerateDivsForEspecifico(LblFolioPliegoResp.Text, LblIdPliegoResp.Text, MainContentDivsAddRespuesta);

        }

    }

    // Función para mostrar una alerta en el cliente
    private void MostrarAlerta(string mensaje)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('" + mensaje + "');", true);
    }

    private void MostrarMensaje(Label lblMensaje, string texto, string clase)
    {
        lblMensaje.Text = texto;
        lblMensaje.CssClass = clase + " fw-bold";
    }

    /// --- REGISTRO DE PETICION --- ///
    protected void DDLCategoriaPeticion_DataBound(object sender, EventArgs e)
    {
        DropDownListCategoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void RadioButtonListPliego_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdBtnPliego = RadioButtonListPliego;

        divPliegoExistente.Visible = (rdBtnPliego.SelectedValue == "existente");
        divNuevoPliego.Visible = (rdBtnPliego.SelectedValue == "nuevo");

        LblIdPliego.Text = (rdBtnPliego.SelectedValue == "nuevo") ? string.Empty: LblIdPliego.Text;
        LblFolioPliego.Text = (rdBtnPliego.SelectedValue == "nuevo") ? string.Empty : LblFolioPliego.Text;
        divPliegoSelect.Visible = !(rdBtnPliego.SelectedValue == "existente");
    }

    // -----  SELECCIONAR PLIEGO UA
    protected void LinkButtonSelectPliego_Click(object sender, EventArgs e)
    {       
        modalGridPliegosUA();
        LblTabSelection.Text = "1";
    }

    private void modalGridPliegosUA()
    {
        GenerateGridDocumentosPliegoUA(CLAVEZP);
        string scriptSMAP = "ShowModalSelectPliego();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", scriptSMAP, true);
    }

    private void GenerateGridDocumentosPliegoUA(string claveZP)
    {
        string query = @"SELECT ID_PLIEGO, FOLIO_PLIEGO FROM PLIEGO WHERE CLAVE_ZP = '" + claveZP + @"'";

        // Ejecuta la consulta manualmente
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            conn.Open();
            da.Fill(dt);
        }

        //ViewState["GridViewPliego"] = dt;
        GridViewPliego.DataSource = dt;
        GridViewPliego.DataBind();
    }

    protected void VerDocPliego(string claveZP, string folio)
    {
        verPDF.Attributes["src"] = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + folio + ".pdf";
        verPDF.DataBind();

        string javaScript = "ShowModalVerPDF();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);
    }

    protected void LinkButtonPliegoPDF_Click(object sender, EventArgs e)
    {
        LinkButton S_B = (LinkButton)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewPliego.SelectedIndex = i;

        string claveZP = CLAVEZP;
        string folioPliego = GridViewPliego.Rows[i].Cells[1].Text;

        LabelVisualizar.Text = "Visualizar Pliego";

        VerDocPliego(claveZP, folioPliego);
    }

    protected void ButtonSelectPliego_Click(object sender, EventArgs e)
    {
        Button S_B = (Button)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewPliego.SelectedIndex = i;

        //estilo de fila seleccionada
        GridViewPliego.SelectedRow.BackColor = Color.FromName("#00BFA5");
        GridViewPliego.SelectedRow.Font.Bold = true;

        string id = GridViewPliego.Rows[i].Cells[0].Text;
        string folioPliego = GridViewPliego.Rows[i].Cells[1].Text;

        if (LblTabSelection.Text == "1")
        {
            LblIdPliego.Text = id;
            LblFolioPliego.Text = folioPliego;
            GenerateDivsForEspecifico(LblFolioPliego.Text, LblIdPliego.Text, MainContentDivsAddEspec);
            divPliegoSelect.Visible = true;
            divPliegoSelectRespuesta.Visible = false;
        }
        else if(LblTabSelection.Text == "2")
        {
            LblIdPliegoResp.Text = id;
            LblFolioPliegoResp.Text = folioPliego;
            GenerateDivsForEspecifico(LblFolioPliegoResp.Text, LblIdPliegoResp.Text, MainContentDivsAddRespuesta);
            divPliegoSelect.Visible = false;
            divPliegoSelectRespuesta.Visible = true;
            divGridPeticiones.Visible = true;
        }

        string javaScript2 = "HideModalSelectPliego();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script2", javaScript2, true);
    }

    // generar chips -- sigue perteneciendo a select uas
    private void GenerateDivsForEspecifico(string folio, string idPliego, Control MainContentDiv)
    {
        MainContentDiv.Controls.Clear();

        Panel divContainer = new Panel
        {
            CssClass = "md-chip specific mb-1 mx-1"
        };

        Label label = new Label
        {
            Text = folio, // Usar el nombre corto en lugar de la clave
            CssClass = "label-class"
        };
        label.Attributes["data-clave"] = idPliego;

        divContainer.Controls.Add(new Literal { Text = "<span>" });
        divContainer.Controls.Add(label);
        divContainer.Controls.Add(new Literal { Text = "</span>" });
        MainContentDiv.Controls.Add(divContainer);
    }

    protected void ButtonGuardar_Click(object sender, EventArgs e)
    {
        int idPliego = 0;
        int idPeticion = ObtenerIdMaxPeticion();

        if (RadioButtonListPliego.SelectedIndex == -1)
        {
            MostrarAlerta("Seleccione una de las opciones de si existe o no un archivo de pliego relacionado a la petición, para poder continuar");
            return;
        }

        if (RadioButtonListPliego.SelectedValue == "existente")
        {
            if ((String.IsNullOrEmpty(LblIdPliego.Text)) && (String.IsNullOrEmpty(LblFolioPliego.Text)))
            {
                MostrarAlerta("Seleccione un pliego existente para poder continuar");
                return;
            }
        }

        // Si es nuevo pliego
        if (RadioButtonListPliego.SelectedValue == "nuevo")
        {
            if (!FileUploadPliego.HasFile)
            {
                MostrarMensaje(LabelMensaje, "Debe subir el archivo PDF del pliego.", "text-danger");
                return;
            }

            //string folio = "PLG-" + DateTime.Now.Year + "-" + Guid.NewGuid().ToString().Substring(0, 4);
            string folio = "PLG-" + DateTime.Now.Year + "-" + Guid.NewGuid().ToString().Substring(0, 3);

            string rutaArchivoMultiGral = CrearDirectorios.Crear_carpeta(CLAVEZP, folio);
            string nombreArchivoGral =  folio + ".pdf";

            try
            {
                string nomArchivo = rutaArchivoMultiGral + nombreArchivoGral;

                // Guardar el archivo físicamente en el servidor
                FileUploadPliego.SaveAs(nomArchivo);
                MostrarMensaje(LabelMensaje, "Archivo guardado correctamente", "alert-success");               
            }
            catch (Exception ex)
            {
                MostrarMensaje(LabelMensaje, "Error al guardar archivo", "alert-danger");
            }

            idPliego = ObtenerIdMaxPliego();
            string rutaArchivo = "Archivos/UnidadAcademica/" + CLAVEZP + "/" + folio + "/" + nombreArchivoGral;

            // Registrar pliego
            Consultas.miInsertPDes(InsertarPliego(),
                   new SqlParameter("@PLIEGO", idPliego),
                   new SqlParameter("@FOLIO", folio),
                   new SqlParameter("@RUTA", rutaArchivo), 
                   new SqlParameter("@FECHA", DateTime.Now),
                   new SqlParameter("@CLAVE", CLAVEZP));
        }
        else
        {
            // Si selecciona un pliego existente
            idPliego = Convert.ToInt32(LblIdPliego.Text);
        }

        // Registrar petición
        Consultas.miInsertPDes(InsertarPeticion(),
                   new SqlParameter("@PLIEGO", idPliego),
                   new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
                   new SqlParameter("@ESTATUS", 1), // 1 = Pendiente
                   new SqlParameter("@IDPETICION", idPeticion),
                   new SqlParameter("@FECHA", TextBoxFechaPeticion.Text),
                   new SqlParameter("@PETICION", TextBoxPeticion.Text));

        MostrarMensaje(LabelMensaje, "✅ Petición registrada correctamente.", "text-success");
        ClearAddPeticion();
    }

    private string InsertarPeticion()
    {
        string insertPeticion = "INSERT INTO PETICIONES_POR_UA (ID_PLIEGO, ID_CAT_PETICION, ID_EST_PETICION, ID_PETICION, FECHA_PETICION, DESC_PETICION) " +
            "VALUES(@PLIEGO, @CATEGORIA, @ESTATUS, @IDPETICION, @FECHA, @PETICION)";

        return insertPeticion;
    }

    private string InsertarPliego()
    {
        string insertPliego = "INSERT INTO PLIEGO (ID_PLIEGO, FOLIO_PLIEGO, RUTA_ARCHIVO, FECHA_CARGA, CLAVE_ZP) " +
            "VALUES(@PLIEGO, @FOLIO, @RUTA, @FECHA, @CLAVE)";

        return insertPliego;
    }

    private int ObtenerIdMaxPeticion()
    {
        int idMaxPeticion = 0;

        idMaxPeticion = Consultas.ConsultaIntDes("SELECT ISNULL(MAX(ID_PETICION), 0) + 1 FROM PETICIONES_POR_UA");

        return idMaxPeticion;
    }

    private int ObtenerIdMaxPliego()
    {
        int idMaxPeticion = 0;

        idMaxPeticion = Consultas.ConsultaIntDes("SELECT ISNULL(MAX(ID_PLIEGO), 0) + 1 FROM PLIEGO");

        return idMaxPeticion;
    }

    private void ClearAddPeticion()
    {
        DropDownListCategoria.ClearSelection();
        TextBoxFechaPeticion.Text = string.Empty;
        TextBoxPeticion.Text = string.Empty;
    }

    /// --- REGISTRO DE RESPUESTA --- ///
    // -----  SELECCIONAR PLIEGO UA
    protected void LinkButtonSelectPliegoResp_Click(object sender, EventArgs e)
    {
        modalGridPliegosUA();
        LblTabSelection.Text = "2";
    }

    protected void ButtonSelectPeticion_Click(object sender, EventArgs e)
    {
        Button S_B = (Button)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewPeticionResp.SelectedIndex = i;

        //estilo de fila seleccionada
        GridViewPeticionResp.SelectedRow.BackColor = Color.FromName("#00BFA5");
        GridViewPeticionResp.SelectedRow.Font.Bold = true;

        LabelIdPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[3].Text;
        LabelPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[5].Text; ;
        LabelCategoriaGridResp.Text = GridViewPeticionResp.Rows[i].Cells[2].Text; ;
    }

    protected void RadioButtonListRespuesta_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdBtnDocRespuesta = RadioButtonListRespuesta;

        divDocRespExistente.Visible = (rdBtnDocRespuesta.SelectedValue == "existente");
        divNuevoDocResp.Visible = (rdBtnDocRespuesta.SelectedValue == "nuevo");

        divDocRespSelect.Visible = !(rdBtnDocRespuesta.SelectedValue == "existente");
    }

    // -----  SELECCIONAR DOC RESPUESTA
    protected void LinkButtonSelectDocResp_Click(object sender, EventArgs e)
    {
        string docRespuesta = "1";
        GenerateGridDocumentosRespPliegoUA(LblIdPliegoResp.Text, docRespuesta);

        string scriptSMAP = "ShowModalSelectDocRespuesta();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", scriptSMAP, true);
    }

    private void GenerateGridDocumentosRespPliegoUA(string pliego, string tipoDoc)
    {
        string query = @"SELECT ID_DOCUMENTO, TIPO_DOCUMENTO, FECHA_SUBIDA, ID_PLIEGO from DOCUMENTO_PETICION WHERE ID_PLIEGO = '" + pliego + @"' AND TIPO_DOCUMENTO = '" + tipoDoc + @"'";

        // Ejecuta la consulta manualmente
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            conn.Open();
            da.Fill(dt);
        }

        GridViewDocRespuesta.DataSource = dt;
        GridViewDocRespuesta.DataBind();
    }

    protected void VerDocRespuesta(string claveZP, string folio, string tipoDoc, string nombreArchivo)
    {
        verPDF.Attributes["src"] = "Archivos/UnidadAcademica/" + CLAVEZP + "/" + folio + "/" + tipoDoc + "/" + nombreArchivo + ".pdf";
            //"Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + folio + ".pdf";
        verPDF.DataBind();

        string javaScript = "ShowModalVerPDF();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);
    } 

    protected void LinkButtonVerDocRespPDF_Click(object sender, EventArgs e)
    {
        LinkButton S_B = (LinkButton)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewDocRespuesta.SelectedIndex = i;

        string claveZP = CLAVEZP;
        string folioPliego = GridViewPliego.Rows[i].Cells[1].Text;
        string idDoc = GridViewPliego.Rows[i].Cells[0].Text;
        string tipoDoc = "RespuestasDoc";
        string archivo = "Respuesta_Id_" + idDoc;

        LabelVisualizar.Text = "Visualizar documento respuesta";

        VerDocRespuesta(claveZP, folioPliego, tipoDoc, archivo);
    }

    protected void ButtonSelectDocRespuesta_Click(object sender, EventArgs e)
    {
        Button S_B = (Button)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewDocRespuesta.SelectedIndex = i;

        //estilo de fila seleccionada
        GridViewDocRespuesta.SelectedRow.BackColor = Color.FromName("#00BFA5");
        GridViewDocRespuesta.SelectedRow.Font.Bold = true;

        LabelIdDocumentoResp.Text = GridViewDocRespuesta.Rows[i].Cells[0].Text; ;
       
        divDocRespSelect.Visible = true;

        string javaScript2 = "HideModalSelectDocRespuesta();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script2", javaScript2, true);
    }

    protected void ButtonGuardarRespuesta_Click(object sender, EventArgs e)
    {
        int idPliego = 0;
        int idDocumento = 0;
       
        // Si es nuevo doc respuesta
        if (RadioButtonListRespuesta.SelectedValue == "nuevo")
        {
            if (!FileUploadRespuesta.HasFile)
            {
                MostrarMensaje(LabelMensajeResp, "Debe subir un archivo PDF con la respuesta.", "text-danger");
                return;
            }

            string extension = Path.GetExtension(FileUploadRespuesta.FileName).ToLower();
            if (extension != ".pdf")
            {
                MostrarMensaje(LabelMensajeResp, "Solo se permiten archivos PDF.", "text-danger");
                return;
            }

            string folio = LblFolioPliegoResp.Text;
            string tipoDoc = "RespuestasDoc";


            idDocumento = ObtenerIdMaxDocumento();
            string idDoc = Convert.ToString(idDocumento);

            string rutaArchivoMultiGral = CrearDirectorios.Crear_carpeta(CLAVEZP, folio, tipoDoc);
            string nombreArchivoGral = "Respuesta_Id_" + idDoc + ".pdf";

            try
            {
                string nomArchivo = rutaArchivoMultiGral + nombreArchivoGral;

                // Guardar el archivo
                FileUploadRespuesta.SaveAs(nomArchivo);
                MostrarMensaje(LabelMensajeResp, "Archivo guardado correctamente", "alert-success");
            }
            catch (Exception ex)
            {
                MostrarMensaje(LabelMensajeResp, "Error al guardar archivo", "alert-danger");
            }

            idPliego = Convert.ToInt32(LblIdPliegoResp.Text);
            string rutaArchivo = "Archivos/UnidadAcademica/" + CLAVEZP + "/" + folio + "/" + tipoDoc + "/" + nombreArchivoGral;

            //Registrar documento respuesta
            Consultas.miInsertPDes(InsertarDocumento(),
                   new SqlParameter("@IDDOC", idDocumento),
                   new SqlParameter("@TIPODOC", 1),
                   new SqlParameter("@RUTA", rutaArchivo),
                   new SqlParameter("@FECHA", DateTime.Now),
                   new SqlParameter("@PLIEGO", idPliego));
        }
        else
        {
            // Si selecciona un pliego existente
            idDocumento = Convert.ToInt32(LabelIdDocumentoResp.Text);
            idPliego = Convert.ToInt32(LblIdPliegoResp.Text);
        }

        //Registrar relacion doc peticion
        Consultas.miInsertPDes(InsertarVinculoPeticionRespDocumento(),
            new SqlParameter("@PET", LabelIdPeticionGridResp.Text),
            new SqlParameter("@DOC", idDocumento),
            new SqlParameter("@PLIEGO", idPliego));


        //Actualizar petición
        Consultas.miUpdatePPDes(UpdateInfoPeticion(),
            new SqlParameter("@FECHA", TextBoxFechaRespuesta.Text),
            new SqlParameter("@RESP", TextBoxRespuesta.Text),
            new SqlParameter("@IDPETICION", LabelIdPeticionGridResp.Text),
            new SqlParameter("@PLIEGO", idPliego));

        MostrarMensaje(LabelMensajeResp, "✅ Respuesta registrada correctamente.", "text-success");
        ClearAddRespuesta();        
    }

    private string InsertarDocumento()
    {
        string insertDoc = "INSERT INTO DOCUMENTO_PETICION (ID_DOCUMENTO, TIPO_DOCUMENTO, RUTA_ARCHIVO, FECHA_SUBIDA, ID_PLIEGO) " +
            "VALUES(@IDDOC, @TIPODOC, @RUTA, @FECHA, @PLIEGO)";

        return insertDoc;
    }

    private string InsertarVinculoPeticionRespDocumento()
    {
        string inserRelacionDOcPeticion = "INSERT INTO VINCULAR_PETICION_DOCUMENTO(ID_PETICION, ID_DOCUMENTO, ID_PLIEGO) VALUES (@PET, @DOC, @PLIEGO)";

        return inserRelacionDOcPeticion;
    }

    private string UpdateInfoPeticion()
    {
        string updatePeticion = "UPDATE PETICIONES_POR_UA SET FECHA_RESP_PETICION = @FECHA, DESC_RESP_PETICION = @RESP WHERE ID_PETICION = @IDPETICION AND ID_PLIEGO = @PLIEGO";

        return updatePeticion;
    }

    private int ObtenerIdMaxDocumento()
    {
        int idMaxPeticion = 0;

        idMaxPeticion = Consultas.ConsultaIntDes("SELECT ISNULL(MAX(ID_DOCUMENTO), 0) + 1 FROM DOCUMENTO_PETICION");

        return idMaxPeticion;
    }   
  
    private void ClearAddRespuesta()
    {
        LabelIdPeticionGridResp.Text = string.Empty;
        LabelPeticionGridResp.Text = string.Empty;
        LabelCategoriaGridResp.Text = string.Empty;
        TextBoxFechaRespuesta.Text = string.Empty;
        TextBoxRespuesta.Text = string.Empty;
        RadioButtonListRespuesta.ClearSelection();
        divNuevoDocResp.Visible = false;
    }
}