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
using System.Web.UI.HtmlControls;
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
            LabelClaveZP.Text = CLAVEZP;
        }
        else
        {
            InfoToolStart();

            MainContentDivsAddEspec.Controls.Clear();
            GenerateDivsForEspecifico(LblFolioPliego.Text, LblIdPliego.Text, MainContentDivsAddEspec);
            MainContentDivsAddRespuesta.Controls.Clear();
            GenerateDivsForEspecifico(LblFolioPliegoResp.Text, LblIdPliegoResp.Text, MainContentDivsAddRespuesta);

            string tabId = HiddenActiveTab.Value;

            if (!string.IsNullOrEmpty(tabId))
            {
                string script =
                    "var el = document.querySelector('button[data-bs-target=\"" + tabId + "\"]');" +
                    "if(el){ var tab = new bootstrap.Tab(el); tab.show(); }";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "RestoreTabScript", script, true);
            }
        }
    }

    // toolTip
    protected void InfoToolStart()
    {
        string script = "infoToolStart();";
        ScriptManager.RegisterStartupScript(this, GetType(), "string", script, true);
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

    protected void DdlInsertItemZero(DropDownList ddlId)
    {
        ddlId.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    // show modals
    private void modalGridPliegosUA()
    {
        GenerateGridDocumentosPliegoUA(CLAVEZP);
        string scriptSMAP = "ShowModalSelectPliego();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", scriptSMAP, true);
    }

    private void showModalVerPdf()
    {
        string javaScript = "ShowModalVerPDF();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);
    }

    // Mostrar modal de confirmación de acción
    protected void ShowModalConfirm()
    {
        string javaScript1 = "ShowModalConfirm();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script1", javaScript1, true);
    }

    private bool ValidarArchivoPDF(FileUpload fileUploadPdf, Label lblMsg)
    {
        if (!fileUploadPdf.HasFile)
        {
            MostrarMensaje(lblMsg, "Debe subir un archivo PDF.", "text-danger");
            return false;
        }

        string ext = Path.GetExtension(fileUploadPdf.FileName).ToLower();
        if (ext != ".pdf")
        {
            MostrarMensaje(lblMsg, "Solo se permiten archivos PDF.", "text-danger");
            return false;
        }

        const int MaxBytes = 2 * 1024 * 1024;
        if (fileUploadPdf.PostedFile.ContentLength > MaxBytes)
        {
            MostrarMensaje(lblMsg, "El archivo debe ser de un tamaño máximo de 2 MB.", "text-danger");
            return false;
        }

        return true;
    }

    /// --- REGISTRO DE PETICION --- ///
    protected void DDLCategoriaPeticion_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListCategoria);
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

        showModalVerPdf();
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

    protected void LinkButtonSelectPetiPliegoPDF_Click(object sender, EventArgs e)
    {
        LabelVisualizar.Text = "Visualizar Pliego";
        VerDocPliego(CLAVEZP, LblFolioPliego.Text);
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
            LabelIdPeticionGridResp.Text = string.Empty;
            LabelPeticionGridResp.Text = string.Empty;
            LabelCategoriaGridResp.Text = string.Empty;
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
            CssClass = "md-chip specific mb-1 mx-1 fw-bold"
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

    private bool ValidarFormularioPeticion()
    {
        if (RadioButtonListPliego.SelectedIndex == -1)
        {
            MostrarAlerta("Seleccione si existe o no un archivo de pliego.");
            return false;
        }

        if (RadioButtonListPliego.SelectedValue == "existente" &&
            string.IsNullOrEmpty(LblIdPliego.Text))
        {
            MostrarAlerta("Seleccione un pliego existente.");
            return false;
        }

        if (DropDownListCategoria.SelectedIndex == 0)
        {
            MostrarAlerta("Seleccione una categoría.");
            return false;
        }

        if (string.IsNullOrEmpty(TextBoxFechaPeticion.Text))
        {
            MostrarAlerta("Ingrese la fecha de la petición.");
            return false;
        }

        return true;
    }

    protected void ButtonGuardar_Click1NoSeUSa(object sender, EventArgs e)
    {
        if (!ValidarFormularioPeticion())
            return;

        int idPliego = 0;
        int idPeticion = ObtenerIdMaxPeticion();

        //if (RadioButtonListPliego.SelectedIndex == -1)
        //{
        //    MostrarAlerta("Seleccione una de las opciones de si existe o no un archivo de pliego relacionado a la petición, para poder continuar");
        //    return;
        //}

        //if (RadioButtonListPliego.SelectedValue == "existente")
        //{
        //    if (String.IsNullOrEmpty(LblIdPliego.Text) && String.IsNullOrEmpty(LblFolioPliego.Text))
        //    {
        //        MostrarAlerta("Seleccione un pliego existente para poder continuar");
        //        return;
        //    }
        //}

        //if (DropDownListCategoria.SelectedIndex == 0)
        //{
        //    MostrarAlerta("Seleccione una categoría para poder continuar");
        //    return;
        //}

        //if (String.IsNullOrEmpty(TextBoxFechaPeticion.Text))
        //{
        //    MostrarAlerta("Ingrese la fecha de la petición para continuar");
        //    return;
        //}

        // Si es nuevo pliego
        if (RadioButtonListPliego.SelectedValue == "nuevo")
        {
            if (!ValidarArchivoPDF(FileUploadPliego, LabelMensajePeticion))
                return;

            if (!FileUploadPliego.HasFile)
            {
                MostrarMensaje(LabelMensajePeticion, "Debe subir el archivo PDF del pliego.", "text-danger");
                return;
            }

            string extension = Path.GetExtension(FileUploadPliego.FileName).ToLower();
            if (extension != ".pdf")
            {
                MostrarMensaje(LabelMensajePeticion, "Solo se permiten archivos PDF.", "text-danger");
                return;
            }

            //Tamaño del archivo.
            int fileSize = FileUploadPliego.PostedFile.ContentLength;
            // tamaño maximo permitido (2MB)
            const int TwoMegaBytesInBytes = 2 * 1024 * 1024;
            
            //if (fileSize > 2100000)
            if (fileSize > TwoMegaBytesInBytes)
            {
                MostrarMensaje(LabelMensajePeticion, "Los archivos deben ser de un tamaño inferior o igual a 2 MB.", "text-danger");
                return;
            }

            idPliego = ObtenerIdMaxPliego();
            //string folio = "PLG-" + DateTime.Now.Year + "-" + Guid.NewGuid().ToString().Substring(0, 3);
            string folio = "PLG-" + DateTime.Now.Year + "-" + idPliego.ToString("D3");

            string rutaArchivoMultiGral = CrearDirectorios.Crear_carpeta(CLAVEZP, folio);
            string nombreArchivoGral =  folio + ".pdf";

            try
            {
                string nomArchivo = rutaArchivoMultiGral + nombreArchivoGral;

                // Guardar el archivo físicamente en el servidor
                FileUploadPliego.SaveAs(nomArchivo);
                MostrarMensaje(LabelMensajePeticion, "Archivo guardado correctamente", "alert-success");               
            }
            catch (Exception ex)
            {
                MostrarMensaje(LabelMensajePeticion, "Error al guardar archivo", "alert-danger");
            }

           
            string rutaArchivo = "Archivos/UnidadAcademica/" + CLAVEZP + "/" + folio + "/" + nombreArchivoGral;

            // Registrar pliego
            Consultas.miInsertPDes(InsertarPliego(),
                   new SqlParameter("@PLIEGO", idPliego),
                   new SqlParameter("@FOLIO", folio),
                   new SqlParameter("@RUTA", rutaArchivo), 
                   new SqlParameter("@FECHA", DateTime.Now),
                   new SqlParameter("@CLAVE", CLAVEZP));

            // Registrar petición
            Consultas.miInsertPDes(InsertarPeticion(),
                       new SqlParameter("@PLIEGO", idPliego),
                       new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
                       new SqlParameter("@ESTATUS", 1), // 1 = Pendiente
                       new SqlParameter("@IDPETICION", idPeticion),
                       new SqlParameter("@FECHA", TextBoxFechaPeticion.Text),
                       new SqlParameter("@PETICION", TextBoxPeticion.Text));

            MostrarMensaje(LabelMensajePeticion, "✅ Petición registrada correctamente.", "text-success");
            ClearAddPeticion();
        }
        else
        {
            // Si selecciona un pliego existente
            idPliego = Convert.ToInt32(LblIdPliego.Text);

            // Registrar petición
            //Consultas.miInsertPDes(InsertarPeticion(),
            //           new SqlParameter("@PLIEGO", idPliego),
            //           new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
            //           new SqlParameter("@ESTATUS", 1), // 1 = Pendiente
            //           new SqlParameter("@IDPETICION", idPeticion),
            //           new SqlParameter("@FECHA", TextBoxFechaPeticion.Text),
            //           new SqlParameter("@PETICION", TextBoxPeticion.Text));

            MostrarMensaje(LabelMensajePeticion, "✅ Petición registrada correctamente.", "text-success");
            ClearAddPeticion();
        }

        DropDownListPliego.DataBind();
        //// Registrar petición
        //Consultas.miInsertPDes(InsertarPeticion(),
        //           new SqlParameter("@PLIEGO", idPliego),
        //           new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
        //           new SqlParameter("@ESTATUS", 1), // 1 = Pendiente
        //           new SqlParameter("@IDPETICION", idPeticion),
        //           new SqlParameter("@FECHA", TextBoxFechaPeticion.Text),
        //           new SqlParameter("@PETICION", TextBoxPeticion.Text));

        //MostrarMensaje(LabelMensaje, "✅ Petición registrada correctamente.", "text-success");
        //ClearAddPeticion();
    }

    protected void ButtonGuardar_Click(object sender, EventArgs e)
    {
        HtmlGenericControl DivConfirmCheck = modalConfirm.FindControl("DivConfirmCheck") as HtmlGenericControl;
        HtmlGenericControl DivConfirmError = modalConfirm.FindControl("DivConfirmError") as HtmlGenericControl;
        // Acceder al control LabelMensaje y establecer el texto
        Label LabelMensaje = modalConfirm.FindControl("LabelMensaje") as Label;

        int idPliego = 0;
        int idPeticion = ObtenerIdMaxPeticion();
        string rutaCarpeta = "";
        string rutaArchivo = "";

        // Validaciones
        if (!ValidarFormularioPeticion())
            return;

        // Si se sube archivo nuevo
        if (RadioButtonListPliego.SelectedValue == "nuevo")
        {
            if (!ValidarArchivoPDF(FileUploadPliego, LabelMensajePeticion))
                return;

            idPliego = ObtenerIdMaxPliego();
            string folio = "PLG-" + DateTime.Now.Year + "-" + idPliego.ToString("D3");

            rutaCarpeta = CrearDirectorios.Crear_carpeta(CLAVEZP, folio);
            rutaArchivo = rutaCarpeta + folio + ".pdf";

            try
            {
                FileUploadPliego.SaveAs(rutaArchivo);
                MostrarMensaje(LabelMensajePeticion, "Archivo guardado correctamente", "alert-success");
            }
            catch
            {
                MostrarMensaje(LabelMensajePeticion, "Error al guardar archivo.", "text-danger");
                return;
            }

            string rutaWeb = "Archivos/UnidadAcademica/" + CLAVEZP + "/" + folio + "/" + folio + ".pdf";

            // SQL con transacción
            string sql = @"
		        BEGIN TRY
			        BEGIN TRANSACTION;

			        " + InsertarPliego() + @"
			        " + InsertarPeticion() + @"

			        COMMIT TRANSACTION;
		        END TRY
		        BEGIN CATCH
			        ROLLBACK TRANSACTION;
		        END CATCH;";

            try
            {
                Consultas.miInsertPDes(sql,
                    new SqlParameter("@PLIEGO", idPliego),
                    new SqlParameter("@FOLIO", folio),
                    new SqlParameter("@RUTA", rutaWeb),
                    new SqlParameter("@FECHA", DateTime.Now),
                    new SqlParameter("@CLAVE", CLAVEZP),

                    new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
                    new SqlParameter("@ESTATUS", 1),
                    new SqlParameter("@IDPETICION", idPeticion),
                    new SqlParameter("@FECHAP", TextBoxFechaPeticion.Text),
                    new SqlParameter("@PETICION", TextBoxPeticion.Text)
                );

                // Agregar mensaje de éxito
                DivConfirmCheck.Visible = true;
                LabelMensaje.Text = "✅ Petición registrada correctamente.";              
                MostrarMensaje(LabelMensajePeticion, "✅ Petición registrada correctamente.", "text-success");
                ClearAddPeticion();
            }
            catch
            {
                // Eliminar archivo y carpeta si algo falló
                try
                {
                    if (File.Exists(rutaArchivo))
                    { File.Delete(rutaArchivo); }

                    if (Directory.Exists(rutaCarpeta))
                    { Directory.Delete(rutaCarpeta, true); }
                }
                catch { }

                // Agregar mensaje de error
                DivConfirmError.Visible = true;
                LabelMensaje.Text = "❎ En este momento no podemos procesar su registro. Por favor, inténtelo de nuevo más tarde.";
                MostrarMensaje(LabelMensajePeticion, "❎ Error al registrar la información.", "text-danger");
            }
        }
        else
        {
            // Pliego existente
            idPliego = Convert.ToInt32(LblIdPliego.Text);

            try
            {
                string sql = @"
			        BEGIN TRY
				        BEGIN TRANSACTION;
				        " + InsertarPeticion() + @"
				        COMMIT;
			        END TRY
			        BEGIN CATCH
				        ROLLBACK;
			        END CATCH;";

                Consultas.miInsertPDes(sql,
                    new SqlParameter("@PLIEGO", idPliego),
                    new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
                    new SqlParameter("@ESTATUS", 1),
                    new SqlParameter("@IDPETICION", idPeticion),
                    new SqlParameter("@FECHAP", TextBoxFechaPeticion.Text),
                    new SqlParameter("@PETICION", TextBoxPeticion.Text)
                );

                // Agregar mensaje de éxito
                DivConfirmCheck.Visible = true;
                LabelMensaje.Text = "✅ Petición registrada correctamente.";
                MostrarMensaje(LabelMensajePeticion, "✅ Petición registrada correctamente.", "text-success");
                ClearAddPeticion();
            }
            catch
            {
                // Agregar mensaje de error
                DivConfirmError.Visible = true;
                LabelMensaje.Text = "❎ En este momento no podemos procesar su registro. Por favor, inténtelo de nuevo más tarde.";
                MostrarMensaje(LabelMensajePeticion, "❎ Error al registrar petición.", "text-danger");
            }
        }

        DropDownListPliego.DataBind();
        ShowModalConfirm();
    }

    private string InsertarPeticion()
    {
        string insertPeticion = "INSERT INTO PETICIONES (ID_PLIEGO, ID_CAT_PETICION, ID_EST_PETICION, ID_PETICION, FECHA_PETICION, DESC_PETICION) " +
            "VALUES(@PLIEGO, @CATEGORIA, @ESTATUS, @IDPETICION, @FECHAP, @PETICION)";

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

        idMaxPeticion = Consultas.ConsultaIntDes("SELECT ISNULL(MAX(ID_PETICION), 0) + 1 FROM PETICIONES");

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

    protected void LinkButtonSelectRespPliegoPDF_Click(object sender, EventArgs e)
    {
        LabelVisualizar.Text = "Visualizar Pliego";
        VerDocPliego(CLAVEZP, LblFolioPliegoResp.Text);
    }

    protected void GridViewPeticionResp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow row = e.Row;
            Button BtnSelectPeticion = (Button)row.FindControl("ButtonSelectPeticion");
            Label LblPeticionResp = (Label)row.FindControl("LabelPeticionConResp");

            //HttpUtility.HtmlDecode(
            string respuesta = e.Row.Cells[7].Text.Trim();

            //if (String.IsNullOrEmpty(respuesta))
            if (respuesta == "&nbsp;" || string.IsNullOrWhiteSpace(respuesta))
            {
                BtnSelectPeticion.Visible = true;
                LblPeticionResp.Visible = false;
            }
            else
            {
                BtnSelectPeticion.Visible = false;
                LblPeticionResp.Visible = true;
                //LblPeticionResp.Text = "Esta petición ya tiene respuesta";
                LblPeticionResp.Text = "La petición no se puede seleccionar, porque ya tiene respuesta";
            }
        }
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

        LabelIdPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[2].Text;
        LabelPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[5].Text; ;
        LabelCategoriaGridResp.Text = GridViewPeticionResp.Rows[i].Cells[3].Text; ;
        LabelFechaPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[4].Text; ;

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
        string docRespuesta = "1"; // CAT_TIPO_DOC_PETICION
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
        verPDF.DataBind();

        showModalVerPdf();
    } 

    protected void LinkButtonVerDocRespPDF_Click(object sender, EventArgs e)
    {
        LinkButton S_B = (LinkButton)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewDocRespuesta.SelectedIndex = i;

        string claveZP = CLAVEZP;
        string folioPliego = GridViewPliego.Rows[i].Cells[1].Text;
        string idDoc = GridViewDocRespuesta.Rows[i].Cells[0].Text;
        string tipoDoc = "RespuestasDoc";
        string archivo = "Respuesta_Id_" + idDoc;

        LabelVisualizar.Text = "Visualizar documento respuesta";

        VerDocRespuesta(claveZP, folioPliego, tipoDoc, archivo);
    }

    protected void LinkButtonSelectRespDocPDF_Click(object sender, EventArgs e)
    {
        string claveZP = CLAVEZP;
        string folioPliego = LblFolioPliegoResp.Text;
        string idDoc = LabelIdDocumentoResp.Text;
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

    private bool ValidarFormularioRespuesta()
    {
        if (String.IsNullOrEmpty(LblIdPliegoResp.Text))
        {
            MostrarAlerta("Seleccione un pliego existente.");
            return false;
        }

        if (String.IsNullOrEmpty(LabelPeticionGridResp.Text) && String.IsNullOrEmpty(LabelCategoriaGridResp.Text))
        {
            MostrarAlerta("Seleccione una petición.");
            return false;
        }

        if (String.IsNullOrEmpty(TextBoxFechaRespuesta.Text))
        {
            MostrarAlerta("Ingrese la fecha de la respuesta.");
            return false;
        }

        if (DateTime.Parse(TextBoxFechaRespuesta.Text) < DateTime.Parse(LabelFechaPeticionGridResp.Text))
        {
            MostrarAlerta("La fecha de la respuesta debe ser mayor a la fecha de la petición.");
            return false;
        }

        if (String.IsNullOrEmpty(TextBoxRespuesta.Text))
        {
            MostrarAlerta("Ingrese la respuesta.");
            return false;
        }

        if (RadioButtonListRespuesta.SelectedIndex == -1)
        {
            MostrarAlerta("Seleccione si existe o no un archivo relacionado a la respuesta.");
            return false;
        }

        if (RadioButtonListRespuesta.SelectedValue == "existente" &&
            String.IsNullOrEmpty(LabelIdDocumentoResp.Text))
        {
            MostrarAlerta("Seleccione un archivo de respuesta existente.");
            return false;
        }

        return true;
    }

    protected void ButtonGuardarRespuesta_ClickNoSeUsa(object sender, EventArgs e)
    {
        if (!ValidarFormularioRespuesta())
            return;

        int idPliego = 0;
        int idDocumento = 0;

        //if (String.IsNullOrEmpty(LblIdPliegoResp.Text) && String.IsNullOrEmpty(LblFolioPliegoResp.Text))
        //if (String.IsNullOrEmpty(LblIdPliegoResp.Text))
        //{
        //    MostrarAlerta("Seleccione un pliego existente para poder continuar");
        //    return;
        //}

        //if (String.IsNullOrEmpty(LabelPeticionGridResp.Text) && String.IsNullOrEmpty(LabelCategoriaGridResp.Text))
        //{
        //    MostrarAlerta("Seleccione una petición para poder continuar");
        //    return;
        //}

        //if (String.IsNullOrEmpty(TextBoxFechaRespuesta.Text))
        //{
        //    MostrarAlerta("Ingrese la fecha de la respuesta para continuar");
        //    return;
        //}

        //if (String.IsNullOrEmpty(TextBoxRespuesta.Text))
        //{
        //    MostrarAlerta("Ingrese la respuesta para continuar");
        //    return;
        //}

        //if (RadioButtonListRespuesta.SelectedIndex == -1)
        //{
        //    MostrarAlerta("Seleccione una de las opciones de si existe o no un archivo de respuesta relacionado, para poder continuar");
        //    return;
        //}

        //if (RadioButtonListRespuesta.SelectedValue == "existente")
        //{
        //    if (String.IsNullOrEmpty(LabelIdDocumentoResp.Text))
        //    {
        //        MostrarAlerta("Seleccione el archivo de respuesta para poder continuar");
        //        return;
        //    }
        //}
                      

        // Si es nuevo doc respuesta
        if (RadioButtonListRespuesta.SelectedValue == "nuevo")
        {
            if (!ValidarArchivoPDF(FileUploadRespuesta, LabelMensajeResp))
                return;

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

            //Tamaño del archivo.
            int fileSize = FileUploadRespuesta.PostedFile.ContentLength;
            // tamaño maximo permitido (2MB)
            const int TwoMegaBytesInBytes = 2 * 1024 * 1024;

            //if (fileSize > 2100000)
            if (fileSize > TwoMegaBytesInBytes)
            {
                MostrarMensaje(LabelMensajeResp, "Los archivos deben ser de un tamaño inferior o igual a 2 MB.", "text-danger");
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
        else
        {
            // Si selecciona un pliego existente
            idDocumento = Convert.ToInt32(LabelIdDocumentoResp.Text);
            idPliego = Convert.ToInt32(LblIdPliegoResp.Text);

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
        GridViewPeticionResp.DataBind();
    }

    protected void ButtonGuardarRespuesta_Click(object sender, EventArgs e)
    {
        HtmlGenericControl DivConfirmCheck = modalConfirm.FindControl("DivConfirmCheck") as HtmlGenericControl;
        HtmlGenericControl DivConfirmError = modalConfirm.FindControl("DivConfirmError") as HtmlGenericControl;
        // Acceder al control LabelMensaje y establecer el texto
        Label LabelMensaje = modalConfirm.FindControl("LabelMensaje") as Label;

        int idPliego = Convert.ToInt32(LblIdPliegoResp.Text);
        int idDocumento = 0;
        string rutaCarpeta = "";
        string rutaArchivoFisico = "";

        // Validaciones
        if (!ValidarFormularioRespuesta())
            return;

        // ======== NUEVO DOCUMENTO DE RESPUESTA ========
        if (RadioButtonListRespuesta.SelectedValue == "nuevo")
        {
            if (!ValidarArchivoPDF(FileUploadRespuesta, LabelMensajeResp))
                return;

            string folio = LblFolioPliegoResp.Text;
            string tipoDoc = "RespuestasDoc";

            idDocumento = ObtenerIdMaxDocumento();

            rutaCarpeta = CrearDirectorios.Crear_carpeta(CLAVEZP, folio, tipoDoc);
            string nombreArchivo = "Respuesta_Id_" + idDocumento + ".pdf";

            rutaArchivoFisico = rutaCarpeta + nombreArchivo;

            try
            {
                FileUploadRespuesta.SaveAs(rutaArchivoFisico);
                MostrarMensaje(LabelMensajeResp, "Archivo guardado correctamente", "alert-success");
            }
            catch
            {
                MostrarMensaje(LabelMensajeResp, "Error al guardar archivo.", "text-danger");
                return;
            }
                       
            string rutaArchivoWeb = "Archivos/UnidadAcademica/" + CLAVEZP + "/" + folio + "/" + tipoDoc + "/" + nombreArchivo;

            // SQL con transacción
            string sql = @"
                BEGIN TRY
                    BEGIN TRANSACTION;

                    " + InsertarDocumento() + @"
                    " + InsertarVinculoPeticionRespDocumento() + @"
                    " + UpdateInfoPeticion() + @"

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                END CATCH;";

            try
            {
                Consultas.miInsertPDes(sql,
                    new SqlParameter("@IDDOC", idDocumento),
                    new SqlParameter("@TIPODOC", 1),
                    new SqlParameter("@RUTA", rutaArchivoWeb),
                    new SqlParameter("@FECHA", DateTime.Now),
                    new SqlParameter("@PLIEGO", idPliego),

                    new SqlParameter("@IDPETICION", LabelIdPeticionGridResp.Text),
                    new SqlParameter("@FECHARESP", TextBoxFechaRespuesta.Text),
                    new SqlParameter("@RESP", TextBoxRespuesta.Text)
                );

                // Agregar mensaje de éxito
                DivConfirmCheck.Visible = true;
                LabelMensaje.Text = "✅ Respuesta registrada correctamente.";
                MostrarMensaje(LabelMensajeResp, "✅ Respuesta registrada correctamente.", "text-success");
                ClearAddRespuesta();
            }
            catch
            {
                // Rollback archivo físico
                try
                {
                    if (File.Exists(rutaArchivoFisico))
                        File.Delete(rutaArchivoFisico);

                    if (Directory.Exists(rutaCarpeta))
                        Directory.Delete(rutaCarpeta, true);
                }
                catch { }

                // Agregar mensaje de error
                DivConfirmError.Visible = true;
                LabelMensaje.Text = "❎ En este momento no podemos procesar su registro. Por favor, inténtelo de nuevo más tarde.";
                MostrarMensaje(LabelMensajeResp, "❎ Error al registrar la información.", "text-danger");
            }
        }
        else
        {
            // ======== USAR DOCUMENTO EXISTENTE ========
            idDocumento = Convert.ToInt32(LabelIdDocumentoResp.Text);

            string sql = @"
                BEGIN TRY
                    BEGIN TRANSACTION;
                    " + InsertarVinculoPeticionRespDocumento() + @"
                    " + UpdateInfoPeticion() + @"
                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                END CATCH;";

            try
            {
                Consultas.miInsertPDes(sql,
                    new SqlParameter("@IDPETICION", LabelIdPeticionGridResp.Text),
                    new SqlParameter("@IDDOC", idDocumento),
                    new SqlParameter("@PLIEGO", idPliego),

                    new SqlParameter("@FECHA", TextBoxFechaRespuesta.Text),
                    new SqlParameter("@RESP", TextBoxRespuesta.Text)
                );

                DivConfirmCheck.Visible = true;
                LabelMensaje.Text = "✅ Respuesta registrada correctamente.";
                MostrarMensaje(LabelMensajeResp, "✅ Respuesta registrada correctamente.", "text-success");
                ClearAddRespuesta();
            }
            catch
            {
                // Agregar mensaje de error
                DivConfirmError.Visible = true;
                LabelMensaje.Text = "❎ En este momento no podemos procesar su registro. Por favor, inténtelo de nuevo más tarde.";
                MostrarMensaje(LabelMensajeResp, "❎ Error al registrar respuesta.", "text-danger");
            }
        }

        GridViewPeticionResp.DataBind();
        ShowModalConfirm();
    }

    private string InsertarDocumento()
    {
        string insertDoc = "INSERT INTO DOCUMENTO_PETICION (ID_DOCUMENTO, TIPO_DOCUMENTO, RUTA_ARCHIVO, FECHA_SUBIDA, ID_PLIEGO) " +
            "VALUES(@IDDOC, @TIPODOC, @RUTA, @FECHA, @PLIEGO)";

        return insertDoc;
    }

    private string InsertarVinculoPeticionRespDocumento()
    {
        string inserRelacionDOcPeticion = "INSERT INTO VINCULAR_PETICION_DOCUMENTO(ID_PETICION, ID_DOCUMENTO, ID_PLIEGO) VALUES (@IDPETICION, @IDDOC, @PLIEGO)";

        return inserRelacionDOcPeticion;
    }

    private string UpdateInfoPeticion()
    {
        string updatePeticion = "UPDATE PETICIONES SET FECHA_RESP_PETICION = @FECHARESP, DESC_RESP_PETICION = @RESP WHERE ID_PETICION = @IDPETICION AND ID_PLIEGO = @PLIEGO";

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

    /// --- detail peticiones --- ///
    protected void LinkButtonVerPeticion_Click(object sender, EventArgs e)
    {        
        string script = "ShowModalVerPeticiones();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", script, true);
    }

    protected void DropDownListPliego_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListPliego);
    }

    protected void LinkButtonVeDocRespuesta_Click(object sender, EventArgs e)
    {
        LinkButton S_B = (LinkButton)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewPliegoPeticion.SelectedIndex = i;

        string claveZP = LabelClaveZP.Text;
        string folioPliego = DropDownListPliego.SelectedItem.Text;
        string idDoc = GridViewPliegoPeticion.Rows[i].Cells[4].Text;
        string tipoDoc = "RespuestasDoc";
        string archivo = "Respuesta_Id_" + idDoc;

        LabelVisualizar.Text = "Visualizar documento respuesta";

        VerDocRespuesta(claveZP, folioPliego, tipoDoc, archivo);
    }

    protected void GridViewPliegoPeticion_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow row = e.Row;
            LinkButton LnkBtnVeDocRespuesta = (LinkButton)row.FindControl("LinkButtonVeDocRespuesta");
            Label LblNoExistDoc = (Label)row.FindControl("LabelNoExistDoc");
            Label LblEstatus = (Label)row.FindControl("LblEstatus");

            string idDoc = e.Row.Cells[4].Text.Trim();
            string estatusPeticion = e.Row.Cells[5].Text.Trim();

            if (idDoc == "&nbsp;" || string.IsNullOrWhiteSpace(idDoc))
            {
                LnkBtnVeDocRespuesta.Visible = false;
                LblNoExistDoc.Visible = true;
                LblNoExistDoc.Text = "No se asignado documento";
            }
            else
            {
                LnkBtnVeDocRespuesta.Visible = true;
                LblNoExistDoc.Visible = false;
            }

            switch (estatusPeticion)
            {
                case "1":
                    LblEstatus.CssClass = "badge bg-warning fw-bolder text-dark";
                    break;
                case "2":
                    LblEstatus.CssClass = "badge bg-info fw-bolder text-dark";
                    break;
                case "3":
                    LblEstatus.CssClass = "badge bg-success fw-bolder text-dark";
                    break;
                case "4":
                    LblEstatus.CssClass = "badge bg-danger fw-bolder text-dark";
                    break;
            }
        }
    }
}