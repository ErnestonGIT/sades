using AjaxControlToolkit;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Evidencias : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LabelZP.Text = Request.Cookies["claveZP"].Value;
        }
        else
        {

        }
    }

    protected String Exist_EvidenciaCumplimiento(String IDPLIEGO, String IDPETICION)
    {
        return Consultas.ConsultaS("select CONCAT(ID_PLIEGO,ID_PETICION) as evidencia from EVIDENCIAS_CUMPLIMIENTO " +
                                    "where ID_PLIEGO = '" + IDPLIEGO + "' and ID_PETICION = '" + IDPETICION + "' ");
    }

    protected String Exist_EvidenciaDiagnostico(String IDPLIEGO, String IDPETICION)
    {
        return Consultas.ConsultaS("select DESC_DIAGNOSTICO from EVIDENCIAS_CUMPLIMIENTO " +
                                    "where ID_PLIEGO = '" + IDPLIEGO + "' and ID_PETICION = '" + IDPETICION + "' ");
    }

    protected String Exist_EvidenciaGestiones(String IDPLIEGO, String IDPETICION)
    {
        return Consultas.ConsultaS("select DESC_GESTIONES from EVIDENCIAS_CUMPLIMIENTO " +
                                    "where ID_PLIEGO = '" + IDPLIEGO + "' and ID_PETICION = '" + IDPETICION + "' ");
    }

    protected void GridViewPliegos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String RutaArchivoPliego = ((Label)e.Row.Cells[2].FindControl("LabelRutaArchivoPliego")).Text;
            ImageButton ArchivoPliego = ((ImageButton)e.Row.Cells[2].FindControl("ImageButtonArchivoPliego"));
            Image NoArchivoPliego = ((Image)e.Row.Cells[2].FindControl("ImageNoArchivoPliego"));

            if (String.IsNullOrEmpty(RutaArchivoPliego))
            {
                ArchivoPliego.Visible = false;
                NoArchivoPliego.Visible = true;
            }
            else
            {
                ArchivoPliego.Visible = true;
                NoArchivoPliego.Visible = false;
            }
        }
    }

    protected void GridViewPeticiones_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int CellDiagnostico = 5;
            int CellGestiones = 6;

            String idpliego = LabelIdPliego.Text; ;
            String idpeticion = e.Row.Cells[2].Text; ;

            //Diagnostico
            if (String.IsNullOrEmpty(Exist_EvidenciaDiagnostico(idpliego, idpeticion)))
            {
                ((ImageButton)e.Row.Cells[CellDiagnostico].FindControl("ImageButton1SelectDiagnosticoP")).ImageUrl = "~/public/img/pendiente.png";
            }
            else
            {
                ((ImageButton)e.Row.Cells[CellDiagnostico].FindControl("ImageButton1SelectDiagnosticoP")).ImageUrl = "~/public/img/completado.png";
            }

            //Gestiones
            if (String.IsNullOrEmpty(Exist_EvidenciaGestiones(idpliego, idpeticion)))
            {
                ((ImageButton)e.Row.Cells[CellGestiones].FindControl("ImageButtonSelectGestionesP")).ImageUrl = "~/public/img/pendiente.png";
            }
            else
            {
                ((ImageButton)e.Row.Cells[CellGestiones].FindControl("ImageButtonSelectGestionesP")).ImageUrl = "~/public/img/completado.png";
            }
        }
    }

    protected String Dir_Doc_Base(String rutabase_gen, String IdInterinato)
    {
        return rutabase_gen + IdInterinato + "\\";
    }

    protected void ImageButtonArchivoPliego_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewPliegos.SelectedIndex = i;

        Random rnd = new Random();
        int numa = rnd.Next(1, 999);
        verPDF.Attributes["src"] = ((Label)GridViewPliegos.Rows[i].Cells[2].FindControl("LabelRutaArchivoPliego")).Text + "?v=" + numa.ToString();
        verPDF.DataBind();

        string javaScriptHDocD2 = "ShowModalVerArchivo();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ButtonSelectPliego_Click(object sender, EventArgs e)
    {
        Button S_B = (Button)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewPliegos.SelectedIndex = i;

        LabelIdPliego.Text = GridViewPliegos.Rows[i].Cells[0].Text;
        LabelFolioPliego.Text = GridViewPliegos.Rows[i].Cells[1].Text;

        string javaScriptHDocD2 = "ShowModalDetallesPliego();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonSelectDiagnosticoP_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewPeticiones.SelectedIndex = i;

        LabelTitDG.Text = "Diagnóstico";
        LabelDG.Text = "D";
        LabelId_PeticionDG.Text = GridViewPeticiones.Rows[i].Cells[2].Text;

        if (String.IsNullOrEmpty(Exist_EvidenciaDiagnostico(LabelIdPliego.Text, LabelId_PeticionDG.Text)))
        {
            PanelAgregarDG.Visible = true;
            PanelVerDG.Visible = false;
        }
        else
        {
            PanelAgregarDG.Visible = false;
            PanelVerDG.Visible = true;
        }

            string javaScriptHDocD2 = "ShowModalDG();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonSelectGestionesP_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewPeticiones.SelectedIndex = i;

        LabelTitDG.Text = "Gestiones";
        LabelDG.Text = "G";

        LabelId_PeticionDG.Text = GridViewPeticiones.Rows[i].Cells[2].Text;

        string javaScriptHDocD2 = "ShowModalDG();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ButtonGuardarDG_Click(object sender, EventArgs e)
    {
        //if (AsyncFileUploadImportarArchivo.IsUploading)
        //{
        //    SubirArchivo(AsyncFileUploadImportarArchivo, LabelIdPliego.Text, LabelId_PeticionDG.Text, LabelZP.Text, LabelFolioPliego.Text, LabelDG.Text, TextBoxDG.Text);
        //}
        String rutabase = "Archivos\\UnidadAcademica\\";

        if (AsyncFileUploadImportarArchivo.HasFile)
        {
            if (AsyncFileUploadImportarArchivo.FileName.Contains(".pdf"))
            {
                try
                {
                    String ClaveZP = LabelZP.Text;
                    String FolioPliego = LabelFolioPliego.Text;
                    String IdPeticion = LabelId_PeticionDG.Text;
                    String DG = LabelDG.Text;
                    String IdPliego = LabelIdPliego.Text;
                    String DescDG = TextBoxDescDG.Text;

                    string directorio = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + rutabase;

                    string carpeta1 = System.IO.Path.Combine(directorio, ClaveZP);
                    System.IO.Directory.CreateDirectory(carpeta1);

                    string carpeta2 = System.IO.Path.Combine(carpeta1, FolioPliego);
                    System.IO.Directory.CreateDirectory(carpeta2);

                    string NombreArchivoPDF = FolioPliego + "_" + IdPeticion + "_" + DG + ".pdf";
                    
                    string dirArchivo = System.IO.Path.Combine(carpeta2, NombreArchivoPDF);
                    string dirArchivoMin = System.IO.Path.Combine(rutabase, ClaveZP, FolioPliego, NombreArchivoPDF);

                    AsyncFileUploadImportarArchivo.SaveAs(dirArchivo);

                    if (DG == "D")
                    {
                        if(String.IsNullOrEmpty(Exist_EvidenciaCumplimiento(IdPliego, IdPeticion)))
                        {
                            Consultas.miInsert("INSERT INTO EVIDENCIAS_CUMPLIMIENTO (ID_PLIEGO, ID_PETICION, DESC_DIAGNOSTICO ,ARCHIVO_DIAGNOSTICO) VALUES ('" + IdPliego + "', '" + IdPeticion + "', '" + DescDG + "', '" + dirArchivoMin + "')");
                        }
                        else
                        {
                            Consultas.miUpdate("UPDATE EVIDENCIAS_CUMPLIMIENTO SET DESC_DIAGNOSTICO = '" + DescDG + "', ARCHIVO_DIAGNOSTICO = '" + dirArchivoMin + "' WHERE ID_PLIEGO = '" + IdPliego + "' and ID_PETICION = '" + IdPeticion + "' ");
                        }  
                    }
                    else if (DG == "G")
                    {
                        if (String.IsNullOrEmpty(Exist_EvidenciaCumplimiento(IdPliego, IdPeticion)))
                        {
                            Consultas.miInsert("INSERT INTO EVIDENCIAS_CUMPLIMIENTO (ID_PLIEGO, ID_PETICION, DESC_GESTIONES, ARCHIVO_GESTIONES) VALUES ('" + IdPliego + "', '" + IdPeticion + "', '" + DescDG + "', '" + dirArchivoMin + "')");
                        }
                        else
                        {
                            Consultas.miUpdate("UPDATE EVIDENCIAS_CUMPLIMIENTO SET DESC_GESTIONES = '" + DescDG + "', ARCHIVO_GESTIONES = '" + dirArchivoMin + "' WHERE ID_PLIEGO = '" + IdPliego + "' and ID_PETICION = '" + IdPeticion + "' ");
                        }
                    }

                }
                catch (Exception)
                {
                    PanelAlertaErrorArchivo.Visible = true;
                    LabelAlertaErrorArchivo.Text = "Existe algún error.Por favor subir de nuevo el archivo.";
                }
            }
            else
            {
                PanelAlertaErrorArchivo.Visible = true;
                LabelAlertaErrorArchivo.Text = "La extensión del archivo es incorrecta, solo permite archivos de PDF (.pdf)";
            }
        }
        else
        {
            PanelAlertaErrorArchivo.Visible = true;
            LabelAlertaErrorArchivo.Text = "Por favor seleccione algún archivo";
        }

        AsyncFileUploadImportarArchivo.ClearAllFilesFromPersistedStore();
    }

}