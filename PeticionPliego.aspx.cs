using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PeticionPliego : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;
    //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;
    string CLAVEZP = HttpContext.Current.Request.Cookies["claveZP"].Value.ToString();
    string idPerfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (idPerfil == "19")
            {
                //labeldependencia.text = dependenciaipn.obtenernombredependencia(clavezp);
                //labelclavezp.text = clavezp;
                divSellectUA.Visible = false;
            }
            else
            {
                //labeldependencia.text = dependenciaipn.obtenernombredependencia(dropdownlistunidadacademica.selectedvalue);
                //labelclavezp.text = dropdownlistunidadacademica.selectedvalue;
                divSellectUA.Visible = true;
            }

            VisibleAlertAcciones();
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

            //VisibleAlertAcciones();

            //if (ViewState["pliegoTimeline"] != null && ViewState["peticionTimeline"] != null)
            //{
            //    //CargarTimeline(ViewState["pliegoTimeline"].ToString(), ViewState["peticionTimeline"].ToString());
            //    CargarTimeline(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
            //}


            if (idPerfil == "19")
            {
                LabelDependencia.Text = DependenciaIPN.ObtenerNombreDependencia(CLAVEZP);
                LabelClaveZP.Text = CLAVEZP;
                divSellectUA.Visible = false;
            }
            else
            {
                LabelDependencia.Text = DependenciaIPN.ObtenerNombreDependencia(DropDownListUnidadAcademica.SelectedValue);
                LabelClaveZP.Text = DropDownListUnidadAcademica.SelectedValue;
                divSellectUA.Visible = true;
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
        GenerateGridDocumentosPliegoUA(LabelClaveZP.Text);
        string scriptSMAP = "ShowModalSelectPliego();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", scriptSMAP, true);
    }

    private void showModalVerPdf()
    {
        string javaScript = "ShowModalVerPDF();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);
    }

    private void showModalAcciones()
    {
        string javaScript = "ShowModalAcciones();";
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

    protected void DropDownListUnidadAcademica_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListUnidadAcademica);
    }

    /// --- REGISTRO DE PETICION --- ///
    protected void DDLCategoriaPeticion_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListCategoria);
    }

    protected void DDLSubCategoriaPeticion_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListSubCat);
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

        string claveZP = LabelClaveZP.Text;
        string folioPliego = GridViewPliego.Rows[i].Cells[1].Text;

        LabelVisualizar.Text = "Pliego " + folioPliego;
        VerDocPliego(claveZP, folioPliego);
    }

    protected void LinkButtonSelectPetiPliegoPDF_Click(object sender, EventArgs e)
    {
        LabelVisualizar.Text = "Pliego " + LblFolioPliego.Text;
        VerDocPliego(LabelClaveZP.Text, LblFolioPliego.Text);
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
            LabelSubCategoriaGridResp.Text = string.Empty;
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

        if (DropDownListCategoria.SelectedIndex != 0)
        {
            if (DropDownListSubCat.SelectedIndex == 0)
            {
                MostrarAlerta("Seleccione un procedimiento.");
                return false;
            }
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
            string claveZP = LabelClaveZP.Text;

            string rutaArchivoMultiGral = CrearDirectorios.Crear_carpeta(claveZP, folio);
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

           
            string rutaArchivo = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + nombreArchivoGral;

            // Registrar pliego
            Consultas.miInsertPDes(InsertarPliego(),
                   new SqlParameter("@PLIEGO", idPliego),
                   new SqlParameter("@FOLIO", folio),
                   new SqlParameter("@RUTA", rutaArchivo), 
                   new SqlParameter("@FECHA", DateTime.Now),
                   new SqlParameter("@CLAVE", claveZP));

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

        // Validaciones
        if (!ValidarFormularioPeticion())
            return;

        int idPliego = 0;
        int idPeticion = ObtenerIdMaxPeticion();
        string rutaCarpeta = "";
        string rutaArchivo = "";

        // Si se sube archivo nuevo
        if (RadioButtonListPliego.SelectedValue == "nuevo")
        {
            if (!ValidarArchivoPDF(FileUploadPliego, LabelMensajePeticion))
                return;

            idPliego = ObtenerIdMaxPliego();
            string folio = "PLG-" + DateTime.Now.Year + "-" + idPliego.ToString("D3");
            string claveZP = LabelClaveZP.Text;

            rutaCarpeta = CrearDirectorios.Crear_carpeta(claveZP, folio);
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

            string rutaWeb = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + folio + ".pdf";

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
                    new SqlParameter("@CLAVE", claveZP),

                    new SqlParameter("@CATEGORIA", DropDownListCategoria.SelectedValue),
                    new SqlParameter("@ESTATUS", 1),
                    new SqlParameter("@IDPETICION", idPeticion),
                    new SqlParameter("@FECHAP", TextBoxFechaPeticion.Text),
                    new SqlParameter("@PETICION", TextBoxPeticion.Text),
                    new SqlParameter("@SUBCATEGORIA", DropDownListSubCat.SelectedValue)
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
                    new SqlParameter("@PETICION", TextBoxPeticion.Text),
                    new SqlParameter("@SUBCATEGORIA", DropDownListSubCat.SelectedValue)
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
        string insertPeticion = "INSERT INTO PETICIONES (ID_PLIEGO, ID_CAT_PETICION, ID_EST_PETICION, ID_PETICION, FECHA_PETICION, DESC_PETICION, ID_SUBCAT_PETICION) " +
            "VALUES(@PLIEGO, @CATEGORIA, @ESTATUS, @IDPETICION, @FECHAP, @PETICION, @SUBCATEGORIA)";

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
        DropDownListSubCat.ClearSelection();
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
        LabelVisualizar.Text = "Pliego " + LblFolioPliegoResp.Text;
        VerDocPliego(LabelClaveZP.Text, LblFolioPliegoResp.Text);
    }

    protected void GridViewPeticionResp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow row = e.Row;
            Button BtnSelectPeticion = (Button)row.FindControl("ButtonSelectPeticion");
            Label LblPeticionResp = (Label)row.FindControl("LabelPeticionConResp");

            //HttpUtility.HtmlDecode(
            string respuesta = e.Row.Cells[9].Text.Trim();

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
        LabelPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[7].Text; ;
        LabelCategoriaGridResp.Text = GridViewPeticionResp.Rows[i].Cells[4].Text; ;
        LabelFechaPeticionGridResp.Text = GridViewPeticionResp.Rows[i].Cells[6].Text; ;
        LabelSubCategoriaGridResp.Text = GridViewPeticionResp.Rows[i].Cells[5].Text; ;

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
        string query = @"SELECT ID_DOCUMENTO, TIPO_DOCUMENTO, FORMAT(FECHA_SUBIDA, 'dd/MM/yyyy') as FECHA_SUBIDA, ID_PLIEGO from DOCUMENTO_PETICION WHERE ID_PLIEGO = '" + pliego + @"' AND TIPO_DOCUMENTO = '" + tipoDoc + @"'";

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
        verPDF.Attributes["src"] = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + tipoDoc + "/" + nombreArchivo + ".pdf";
        verPDF.DataBind();

        showModalVerPdf();
    } 

    protected void LinkButtonVerDocRespPDF_Click(object sender, EventArgs e)
    {
        LinkButton S_B = (LinkButton)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewDocRespuesta.SelectedIndex = i;

        string claveZP = LabelClaveZP.Text;
        string folioPliego = LblFolioPliegoResp.Text;
        string idDoc = GridViewDocRespuesta.Rows[i].Cells[0].Text;
        string tipoDoc = "RespuestasDoc";
        string archivo = "Respuesta_Id_" + idDoc;

        LabelVisualizar.Text = "Documento respuesta";

        VerDocRespuesta(claveZP, folioPliego, tipoDoc, archivo);
    }

    protected void LinkButtonSelectRespDocPDF_Click(object sender, EventArgs e)
    {
        string claveZP = LabelClaveZP.Text;
        string folioPliego = LblFolioPliegoResp.Text;
        string idDoc = LabelIdDocumentoResp.Text;
        string tipoDoc = "RespuestasDoc";
        string archivo = "Respuesta_Id_" + idDoc;

        LabelVisualizar.Text = "Documento respuesta";

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

        if (String.IsNullOrEmpty(TextBoxFechaVigencia.Text))
        {
            MostrarAlerta("Ingrese la fecha de cumplimiento.");
            return false;
        }

        if (DateTime.Parse(TextBoxFechaVigencia.Text) < DateTime.Parse(LabelFechaPeticionGridResp.Text))
        {
            MostrarAlerta("La fecha de cumplimiento debe ser mayor a la fecha de la petición.");
            return false;
        }

        if (DateTime.Parse(TextBoxFechaVigencia.Text) < DateTime.Parse(TextBoxFechaRespuesta.Text))
        {
            MostrarAlerta("La fecha de cumplimiento debe ser mayor a la fecha de respuesta.");
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

        // Validaciones
        if (!ValidarFormularioRespuesta())
            return;
        
        int idPliego = Convert.ToInt32(LblIdPliegoResp.Text);
        int idDocumento = 0;
        string rutaCarpeta = "";
        string rutaArchivoFisico = "";

       
        // ======== NUEVO DOCUMENTO DE RESPUESTA ========
        if (RadioButtonListRespuesta.SelectedValue == "nuevo")
        {
            if (!ValidarArchivoPDF(FileUploadRespuesta, LabelMensajeResp))
                return;

            string folio = LblFolioPliegoResp.Text;
            string tipoDoc = "RespuestasDoc";

            idDocumento = ObtenerIdMaxDocumento();
            string claveZP = LabelClaveZP.Text;
            rutaCarpeta = CrearDirectorios.Crear_carpeta(claveZP, folio, tipoDoc);
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
                       
            string rutaArchivoWeb = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + tipoDoc + "/" + nombreArchivo;

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
                    new SqlParameter("@RESP", TextBoxRespuesta.Text),
                    new SqlParameter("@FECHACUMPLIMIENTO", TextBoxFechaVigencia.Text)
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
                    new SqlParameter("@RESP", TextBoxRespuesta.Text),
                    new SqlParameter("@FECHACUMPLIMIENTO", TextBoxFechaVigencia.Text)
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
        string updatePeticion = "UPDATE PETICIONES SET FECHA_RESP_PETICION = @FECHARESP, DESC_RESP_PETICION = @RESP, FECHA_CUMPLIMIENTO = @FECHACUMPLIMIENTO " +
            "WHERE ID_PETICION = @IDPETICION AND ID_PLIEGO = @PLIEGO";

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
        LabelSubCategoriaGridResp.Text = string.Empty;
        TextBoxFechaRespuesta.Text = string.Empty;
        TextBoxRespuesta.Text = string.Empty;
        TextBoxFechaVigencia.Text = string.Empty;
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

    protected void DropDownListPliego_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DdlPliego = DropDownListPliego;

        bool indexDDL = DdlPliego.SelectedIndex != 0;

        divViewPliego.Visible = indexDDL;

        
    }
    protected void LinkButtonPliego_Click(object sender, EventArgs e)
    {
        LabelVisualizar.Text = "Pliego " + DropDownListPliego.SelectedItem.Text;
        VerDocPliego(LabelClaveZP.Text, DropDownListPliego.SelectedItem.Text);
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

        LabelVisualizar.Text = "Documento respuesta";

        VerDocRespuesta(claveZP, folioPliego, tipoDoc, archivo);
    }

    protected void LinkButtonAccionesPLG_Click(object sender, EventArgs e)
    {
        LinkButton S_B = (LinkButton)sender;
        GridViewRow G_B = (GridViewRow)S_B.NamingContainer;
        int i = G_B.RowIndex;
        GridViewPliegoPeticion.SelectedIndex = i;

        string idPliego = GridViewPliegoPeticion.Rows[i].Cells[1].Text;
        string idPeticion = GridViewPliegoPeticion.Rows[i].Cells[3].Text;

        CargarTimeline(idPliego, idPeticion);
        showModalAcciones();
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

    /// --- REGISTRO DE ACCIONES DE RESPUESTA --- ///
    protected void DropDownListPliegoAccionResp_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListPliegoAccionResp);
    }

    protected void DropDownListRespuestaAccion_DataBound(object sender, EventArgs e)
    {
        DdlInsertItemZero(DropDownListRespuestaAccion);
    }

    protected void DropDownListRespuestaAccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlProgramAcadEspecDlt = DropDownListRespuestaAccion;

        bool indexDDL = ddlProgramAcadEspecDlt.SelectedIndex != 0;

        if (ddlProgramAcadEspecDlt.SelectedIndex != 0)
        {
            LabelFechaRespAction.Text = ObtenerFechaRespAction(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
            LabelFechaCumplimiento.Text = ObtenerFechaCumplimientoAction(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
        }

        VisibleAlertAcciones();
    }

    protected void VisibleAlertAcciones()
    {
        int accionPlan = ExisteActionPlan(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
        if (accionPlan == 0)
        {
            divAlertActionPlan.Visible = false;
            divAlertActionGestion.Visible = true;
            ButtonGuardarActionPlan.Enabled = true;
            ButtonGuardarActionGestion.Enabled = false;
            LabelIdDiagnostico.Text = string.Empty;
        }
        else
        {
            divAlertActionPlan.Visible = true;
            divAlertActionGestion.Visible = false;
            ButtonGuardarActionPlan.Enabled = false;
            ButtonGuardarActionGestion.Enabled = true;
            LabelIdDiagnostico.Text = ObtenerActionPlan(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
        }
    }

    private int ExisteActionPlan(string idPliego, string idPeticion)
    {
        int conteoAccionPlan = 0;
        conteoAccionPlan = Consultas.ConsultaIntDes("SELECT COUNT(*) FROM DIAGNOSTICO WHERE ID_PLIEGO = '" + idPliego + "' AND ID_PETICION = '" + idPeticion + "'");

        return conteoAccionPlan;
    }

    private string ObtenerActionPlan(string idPliego, string idPeticion)
    {
        string idAccionPlan;

        idAccionPlan = Consultas.ConsultaSDes("SELECT ID_DIAGNOSTICO FROM DIAGNOSTICO WHERE ID_PLIEGO = '" + idPliego + "' AND ID_PETICION = '" + idPeticion + "'");

        return idAccionPlan;
    }

    private bool ValidarFormularioActionPlan()
    {       
        if (DropDownListPliegoAccionResp.SelectedIndex == 0)
        {
            MostrarAlerta("Seleccione un pliego.");
            return false;
        }

        if (DropDownListRespuestaAccion.SelectedIndex == 0)
        {
            MostrarAlerta("Seleccione una respuesta.");
            return false;
        }

        if (String.IsNullOrEmpty(TextBoxFechaActionPlan.Text))
        {
            MostrarAlerta("Ingrese la fecha.");
            return false;
        }

        if (DateTime.Parse(TextBoxFechaActionPlan.Text) < DateTime.Parse(LabelFechaRespAction.Text))
        {
            MostrarAlerta("La fecha de la acción de diagnóstico debe ser mayor a la fecha de la respuesta.");
            return false;
        }

        if (String.IsNullOrEmpty(TextBoxActionPlan.Text))
        {
            MostrarAlerta("Ingrese la acción.");
            return false;
        }

        return true;
    }

    private bool ValidarFormularioActionGestion()
    {
        if (DropDownListPliegoAccionResp.SelectedIndex == 0)
        {
            MostrarAlerta("Seleccione un pliego.");
            return false;
        }

        if (DropDownListRespuestaAccion.SelectedIndex == 0)
        {
            MostrarAlerta("Seleccione una respuesta.");
            return false;
        }

        if (String.IsNullOrEmpty(TextBoxFechaActionGest.Text))
        {
            MostrarAlerta("Ingrese la fecha.");
            return false;
        }

        if (DateTime.Parse(TextBoxFechaActionGest.Text) < DateTime.Parse(LabelFechaRespAction.Text))
        {
            MostrarAlerta("La fecha de la acción de gestión debe ser mayor a la fecha de la respuesta.");
            return false;
        }

        if (String.IsNullOrEmpty(TextBoxActionGestion.Text))
        {
            MostrarAlerta("Ingrese la acción.");
            return false;
        }

        return true;
    }

    protected void ButtonGuardarActionPlan_Click(object sender, EventArgs e)
    {
        HtmlGenericControl DivConfirmCheck = modalConfirm.FindControl("DivConfirmCheck") as HtmlGenericControl;
        HtmlGenericControl DivConfirmError = modalConfirm.FindControl("DivConfirmError") as HtmlGenericControl;
        // Acceder al control LabelMensaje y establecer el texto
        Label LabelMensaje = modalConfirm.FindControl("LabelMensaje") as Label;

        // Validaciones
        if (!ValidarFormularioActionPlan())
            return;

        int idDiagnostico = 0;
        string rutaCarpeta = "";
        string rutaArchivoFisico = "";

        // ======== NUEVO DOCUMENTO DE RESPUESTA ========
        if (!ValidarArchivoPDF(FileUploadActionPlan, LabelMensajeActionPlan))
            return;

        string folio = DropDownListPliegoAccionResp.SelectedItem.Text;
        string tipoDoc = "AccionesRespuesta";
        string carpetaPeticion = "PTC-ID-" + DropDownListRespuestaAccion.SelectedValue;

        idDiagnostico = ObtenerIdMaxDiagnostico(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
        string claveZP = LabelClaveZP.Text;

        rutaCarpeta = CrearDirectorios.Crear_carpeta(claveZP, folio, tipoDoc, carpetaPeticion);
        string nombreArchivo = "Diagnostico_Id_" + idDiagnostico + ".pdf";

        rutaArchivoFisico = rutaCarpeta + nombreArchivo;

        try
        {
            FileUploadActionPlan.SaveAs(rutaArchivoFisico);
            MostrarMensaje(LabelMensajeActionPlan, "Archivo guardado correctamente", "alert-success");
        }
        catch
        {
            MostrarMensaje(LabelMensajeActionPlan, "Error al guardar archivo.", "text-danger");
            return;
        }

        string rutaArchivoWeb = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + tipoDoc + "/" + carpetaPeticion + "/" + nombreArchivo;

        // SQL con transacción
        string sql = @"
                BEGIN TRY
                    BEGIN TRANSACTION;
                    " + InsertarAccionDiagnostico() + @"
                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                END CATCH;";

        try
        {
            Consultas.miInsertPDes(sql,
                new SqlParameter("@PLIEGO", DropDownListPliegoAccionResp.SelectedValue),
                new SqlParameter("@IDPETICION", DropDownListRespuestaAccion.SelectedValue),
                new SqlParameter("@IDDIAGNOSTICO", idDiagnostico),
                new SqlParameter("@DIAGNOSTICO", TextBoxActionPlan.Text),
                new SqlParameter("@FECHADIAGNOSTICO", TextBoxFechaActionPlan.Text),
                new SqlParameter("@RUTA", rutaArchivoWeb)
            );

            // Agregar mensaje de éxito
            DivConfirmCheck.Visible = true;
            LabelMensaje.Text = "✅ Acción registrada correctamente.";
            MostrarMensaje(LabelMensajeActionPlan, "✅ Acción registrada correctamente.", "text-success");
            ClearAddActionPlan();
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
            MostrarMensaje(LabelMensajeActionPlan, "❎ Error al registrar la información.", "text-danger");
        }

        ShowModalConfirm();
        VisibleAlertAcciones();
    }

    private void ClearAddActionPlan()
    {
        TextBoxFechaActionPlan.Text = string.Empty;
        TextBoxActionPlan.Text = string.Empty;
    }

    private string ObtenerFechaRespAction(string idPliego, string idPeticion)
    {
        string idFecha = Consultas.ConsultaSDes("SELECT FORMAT(FECHA_RESP_PETICION, 'dd/MM/yyyy') as FECHA_RESP_PETICION FROM PETICIONES " +
            "WHERE ID_PLIEGO = '" + idPliego + "' AND ID_PETICION = '" + idPeticion + "' ");

        return idFecha;
    }

    private string ObtenerFechaCumplimientoAction(string idPliego, string idPeticion)
    {
        string idFecha = Consultas.ConsultaSDes("SELECT FORMAT(FECHA_CUMPLIMIENTO, 'dd/MM/yyyy') as FECHA_RESP_PETICION FROM PETICIONES " +
            "WHERE ID_PLIEGO = '" + idPliego + "' AND ID_PETICION = '" + idPeticion + "' ");

        return idFecha;
    }

    private string InsertarAccionDiagnostico()
    {
        string insertActionDiagnostico = "INSERT INTO DIAGNOSTICO (ID_PLIEGO, ID_PETICION, ID_DIAGNOSTICO, DESCRIPCION_DIAGNOSTICO, FECHA_DIAGNOSTICO, ARCHIVO_DIAGNOSTICO) " +
            "VALUES(@PLIEGO, @IDPETICION, @IDDIAGNOSTICO, @DIAGNOSTICO, @FECHADIAGNOSTICO, @RUTA)";

        return insertActionDiagnostico;
    }

    private int ObtenerIdMaxDiagnostico(string idPliego, string idPeticion)
    {
        int idMaxDiagnostico = 0;

        idMaxDiagnostico = Consultas.ConsultaIntDes("SELECT ISNULL(MAX(ID_DIAGNOSTICO), 0) + 1 FROM DIAGNOSTICO WHERE ID_PLIEGO = '" + idPliego + "' AND ID_PETICION = '" + idPeticion + "' ");

        return idMaxDiagnostico;
    }

    protected void ButtonGuardarActionGestion_Click(object sender, EventArgs e)
    {
        HtmlGenericControl DivConfirmCheck = modalConfirm.FindControl("DivConfirmCheck") as HtmlGenericControl;
        HtmlGenericControl DivConfirmError = modalConfirm.FindControl("DivConfirmError") as HtmlGenericControl;
        // Acceder al control LabelMensaje y establecer el texto
        Label LabelMensaje = modalConfirm.FindControl("LabelMensaje") as Label;

        // Validaciones
        if (!ValidarFormularioActionGestion())
            return;

        int idGestion = 0;
        string rutaCarpetaG = "";
        string rutaArchivoFisicoG = "";

        // ======== NUEVO DOCUMENTO DE RESPUESTA ========
        if (!ValidarArchivoPDF(FileUploadActionGestion, LabelMensajeActionGestion))
            return;

        string folio = DropDownListPliegoAccionResp.SelectedItem.Text;
        string tipoDoc = "AccionesRespuesta";
        string carpetaPeticion = "PTC-ID-" + DropDownListRespuestaAccion.SelectedValue;

        idGestion = ObtenerIdMaxGestion(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue, LabelIdDiagnostico.Text.Trim());
        string claveZP = LabelClaveZP.Text;

        rutaCarpetaG = CrearDirectorios.Crear_carpeta(claveZP, folio, tipoDoc, carpetaPeticion);
        string nombreArchivo = "Gestion_Id_" + idGestion + ".pdf";

        rutaArchivoFisicoG = rutaCarpetaG + nombreArchivo;

        try
        {
            FileUploadActionGestion.SaveAs(rutaArchivoFisicoG);
            MostrarMensaje(LabelMensajeActionGestion, "Archivo guardado correctamente", "alert-success");
        }
        catch
        {
            MostrarMensaje(LabelMensajeActionGestion, "Error al guardar archivo.", "text-danger");
            return;
        }

        string rutaArchivoWeb = "Archivos/UnidadAcademica/" + claveZP + "/" + folio + "/" + tipoDoc + "/" + carpetaPeticion + "/" + nombreArchivo;

        // SQL con transacción
        string sql = @"
                BEGIN TRY
                    BEGIN TRANSACTION;
                    " + InsertarAccionGestion() + @"
                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                END CATCH;";

        try
        {
            Consultas.miInsertPDes(sql,
                new SqlParameter("@PLIEGO", DropDownListPliegoAccionResp.SelectedValue),
                new SqlParameter("@IDPETICION", DropDownListRespuestaAccion.SelectedValue),
                new SqlParameter("@IDDIAGNOSTICO", LabelIdDiagnostico.Text.Trim()),
                new SqlParameter("@IDGESTION", idGestion),
                new SqlParameter("@GESTION", TextBoxActionGestion.Text),
                new SqlParameter("@FECHAGESTION", TextBoxFechaActionGest.Text),
                new SqlParameter("@RUTA", rutaArchivoWeb)
            );

            // Agregar mensaje de éxito
            DivConfirmCheck.Visible = true;
            LabelMensaje.Text = "✅ Acción registrada correctamente.";
            MostrarMensaje(LabelMensajeActionGestion, "✅ Acción registrada correctamente.", "text-success");
            ClearAddActionGestion();
        }
        catch 
        {
            // Rollback archivo físico
            try
            {
                if (File.Exists(rutaArchivoFisicoG))
                    File.Delete(rutaArchivoFisicoG);

                if (Directory.Exists(rutaCarpetaG))
                    Directory.Delete(rutaCarpetaG, true);
            }
            catch { }

            // Agregar mensaje de error
            DivConfirmError.Visible = true;
            LabelMensaje.Text = "❎ En este momento no podemos procesar su registro. Por favor, inténtelo de nuevo más tarde.";
            MostrarMensaje(LabelMensajeActionGestion, "❎ Error al registrar la información.", "text-danger");
        }

        ShowModalConfirm();
    }

    private void ClearAddActionGestion()
    {       
        TextBoxFechaActionGest.Text = string.Empty;
        TextBoxActionGestion.Text = string.Empty;
    }
    
    private string InsertarAccionGestion()
    {
        string insertActionGestion = "INSERT INTO GESTIONES (ID_PLIEGO, ID_PETICION, ID_DIAGNOSTICO, ID_GESTIONES, DESCRIPCION_GESTIONES, FECHA_GESTIONES, ARCHIVO_GESTIONES) " +
            "VALUES(@PLIEGO, @IDPETICION, @IDDIAGNOSTICO, @IDGESTION, @GESTION, @FECHAGESTION, @RUTA)";

        return insertActionGestion;
    }

    private int ObtenerIdMaxGestion(string idPliego, string idPeticion, string idDiagnostico)
    {
        int idMaxGestion = 0;

        idMaxGestion = Consultas.ConsultaIntDes("SELECT ISNULL(MAX(ID_GESTIONES), 0) + 1 FROM GESTIONES WHERE ID_PLIEGO = '" + idPliego + "' AND ID_PETICION = '" + idPeticion + "' AND ID_DIAGNOSTICO = '" + idDiagnostico + "'");

        return idMaxGestion;
    }

    protected void LinkButtonAcciones_Click(object sender, EventArgs e)
    {
        CargarTimeline(DropDownListPliegoAccionResp.SelectedValue, DropDownListRespuestaAccion.SelectedValue);
        showModalAcciones();
    }

    //private void CargarTimeline(string idPliego, string idPeticion)
    //{
    //    phTimeline.Controls.Clear();

    //    string query = "SELECT 'DIAGNOSTICO' AS ACCION, DESCRIPCION_DIAGNOSTICO AS DESCRIPCION, FORMAT(FECHA_DIAGNOSTICO, 'dd/MM/yyyy') AS FECHA, ARCHIVO_DIAGNOSTICO AS RUTA " +
    //                            "FROM DIAGNOSTICO WHERE ID_PLIEGO = @PLIEGO AND ID_PETICION = @PETICION " +
    //                        "UNION " +
    //                    "SELECT 'GESTION' AS ACCION, DESCRIPCION_GESTIONES AS DESCRIPCION, FORMAT(FECHA_GESTIONES, 'dd/MM/yyyy') AS FECHA, ARCHIVO_GESTIONES AS RUTA " +
    //                            "FROM GESTIONES WHERE ID_PLIEGO = @PLIEGO AND ID_PETICION = @PETICION";

    //    Debug.Write("consulta " + query);

    //    using (SqlConnection con = new SqlConnection(connectionString))
    //    using (SqlCommand cmd = new SqlCommand(query, con))
    //    {
    //        cmd.Parameters.AddWithValue("@PLIEGO", 5);
    //        cmd.Parameters.AddWithValue("@PETICION", 7);

    //        con.Open();
    //        SqlDataReader dr = cmd.ExecuteReader();

    //        Debug.WriteLine("cuenta" + dr.FieldCount);

    //        while (dr.Read())
    //        {
    //            string accion = dr["ACCION"].ToString();
    //            string descripcion = dr["DESCRIPCION"].ToString();
    //            string fecha = dr["FECHA"].ToString();
    //            string ruta = dr["RUTA"].ToString();

    //            //ITEM PRINCIPAL
    //            var divItem = new Literal();
    //            divItem.Text = "<div class='tracking-item'>";

    //            phTimeline.Controls.Add(divItem);

    //            // ICONO Y LinkButton
    //            Literal ltIconOpen = new Literal();
    //            ltIconOpen.Text = "<div class='tracking-icon status-intransit'><i class='fas fa-circle'></i>";

    //            phTimeline.Controls.Add(ltIconOpen);

    //            LinkButton btn = new LinkButton();
    //            btn.Text = "<i class='fas fa-file-pdf fa-2x fa-fw'></i>";
    //            btn.CssClass = "color-btn mb-2";
    //            btn.CommandArgument = ruta;             // RUTA PDF
    //            btn.Click += new EventHandler(VerPdfAccion_Click);

    //            phTimeline.Controls.Add(btn);

    //            Literal ltIconClose = new Literal();
    //            ltIconClose.Text = "</div>";

    //            phTimeline.Controls.Add(ltIconClose);

    //            // FECHA Y ACCION
    //            Literal ltFecha = new Literal();
    //            ltFecha.Text =
    //                "<div class='tracking-date'>" + accion +
    //                "<span>" + fecha + "</span></div>";

    //            phTimeline.Controls.Add(ltFecha);

    //            // DESCRIPCION
    //            Literal ltDesc = new Literal();
    //            ltDesc.Text =
    //                "<div class='tracking-content'>" + descripcion + "</div>";

    //            phTimeline.Controls.Add(ltDesc);

    //            // Cierre del item
    //            Literal ltClose = new Literal();
    //            ltClose.Text = "</div>";

    //            phTimeline.Controls.Add(ltClose);
    //        }
    //        dr.Close();
    //    }
    //}

    private void CargarTimeline(string idPliego, string idPeticion)
    {
        phTimeline.Controls.Clear();

        // Seguridad: convertir a int si se esperan números
        int pliegoInt = 0;
        int peticionInt = 0;
        int.TryParse(idPliego, out pliegoInt);
        int.TryParse(idPeticion, out peticionInt);

        string query =
            "SELECT 'DIAGNÓSTICO' AS ACCION, DESCRIPCION_DIAGNOSTICO AS DESCRIPCION, " +
                    "FORMAT(FECHA_DIAGNOSTICO, 'dd/MM/yyyy') AS FECHA, ARCHIVO_DIAGNOSTICO AS RUTA " +
                "FROM DIAGNOSTICO WHERE ID_PLIEGO = @PLIEGO AND ID_PETICION = @PETICION " +
            "UNION " +
            "SELECT 'GESTIÓN' AS ACCION, DESCRIPCION_GESTIONES AS DESCRIPCION, " +
                    "FORMAT(FECHA_GESTIONES, 'dd/MM/yyyy') AS FECHA, ARCHIVO_GESTIONES AS RUTA " +
                "FROM GESTIONES WHERE ID_PLIEGO = @PLIEGO AND ID_PETICION = @PETICION";

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@PLIEGO", pliegoInt);
                cmd.Parameters.AddWithValue("@PETICION", peticionInt);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.HasRows)
                    {
                        // Mostrar mensaje cuando no hay acciones
                        phTimeline.Controls.Add(new Literal { Text = "<div class='p-3 text-center text-muted'>No hay acciones registradas para este pliego/petición.</div>" });
                        return;
                    }

                    while (dr.Read())
                    {                        
                        string accion = dr["ACCION"].ToString();
                        string descripcion = dr["DESCRIPCION"].ToString();
                        string fecha = dr["FECHA"].ToString();
                        string ruta = dr["RUTA"].ToString();

                        string borderClase;
                        borderClase = (accion == "GESTIÓN") ? "border-gestion" : "border-diagnostico";

                        //ITEM PRINCIPAL
                        phTimeline.Controls.Add(new Literal { Text = "<div class='tracking-item fade-slide  " + borderClase + "'>" });

                        //ICONO ACCIÓN
                        //phTimeline.Controls.Add(new Literal { Text = "<div class='tracking-icon " + colorClase + "'>" + iconoAccion });

                        // ICONO Y LinkButton
                        phTimeline.Controls.Add(new Literal { Text = "<div class='tracking-icon status-intransit'>" });

                        phTimeline.Controls.Add(new Literal { Text = "<asp:UpdatePanel runat='server'><ContentTemplate>" });

                        //LinkButton btn = new LinkButton();
                        //btn.Text = "<i class='fas fa-file-pdf fa-2x fa-fw'></i>";
                        //btn.CssClass = "color-btn1 mb-2";
                        //btn.CommandArgument = ruta;             // RUTA PDF
                        //btn.Click += new EventHandler(VerPdfAccion_Click);

                        //phTimeline.Controls.Add(btn);

                        phTimeline.Controls.Add(new Literal
                        {
                            Text = "<span class='d-inline-block' tabindex='0' data-bs-toggle='popover' data-bs-placement='left' data-bs-custom-class='custom-popover' " +
                                        "data-bs-trigger='hover focus' data-bs-content='Ver documento'>"
                        });
                                                
                        HtmlAnchor a = new HtmlAnchor();
                        a.HRef = "javascript:void(0);";
                        a.InnerHtml = "<i class='fas fa-file-pdf fa-2x fa-fw'></i>";
                        a.Attributes["class"] = "color-btn1 mb-2";
                        a.Attributes["data-ruta"] = ruta;
                        a.Attributes["onclick"] = "VerPdfDesdeTimeline(this);";
                        phTimeline.Controls.Add(a);

                        phTimeline.Controls.Add(new Literal { Text = "</span>" });

                        phTimeline.Controls.Add(new Literal { Text = "</ContentTemplate></asp:UpdatePanel>" });

                        phTimeline.Controls.Add(new Literal { Text = "</div>" });

                        // FECHA Y ACCION
                        phTimeline.Controls.Add(new Literal
                        {
                            Text = "<div class='tracking-date'><span class='fw-bolder'>" + accion +
                                "</span><span>" + fecha + "</span></div>"
                        });

                        // DESCRIPCION
                        phTimeline.Controls.Add(new Literal { Text = "<div class='tracking-content'>" + descripcion + "</div>" });

                        // Cierre del item
                        phTimeline.Controls.Add(new Literal { Text = "</div>" });
                    }
                    dr.Close();

                } // using dr
            } // using cmd/con
        }
        catch (Exception ex)
        {
            // Mostrar mensaje de error simple en UI para debug (remover en producción)
            phTimeline.Controls.Add(new Literal { Text = "<div class='text-danger p-2'>Error al cargar timeline: " + HttpUtility.HtmlEncode(ex.Message) + "</div>" });

            // y loguear a Debug
            Debug.WriteLine("Error CargarTimeline: " + ex.ToString());
        }
    }

    //protected void VerPdfAccion_Click(object sender, EventArgs e)
    //{
    //    LinkButton btn = (LinkButton)sender;
    //    string ruta = btn.CommandArgument;

    //    //LblRutaPDF.Text = ruta; // O lo cargas en un iframe
    //    verPDF.Attributes["src"] = ruta; // O lo cargas en un iframe
    //    verPDF.DataBind();

    //    showModalVerPdf();
    //}
}