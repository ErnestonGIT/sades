using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Validacion : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LabelIdPerfil.Text = Request.Cookies["Tipo"].Value;
            LabelId_Nivel_Est.Text = Consultas.ConsultaS("select ID_NIVEL_EST from CAT_PERFILES where ID_PERFIL = '" + LabelIdPerfil.Text + "'");

            LabelZP.Text = Request.Cookies["claveZP"].Value;
            if(String.IsNullOrEmpty(LabelZP.Text))
            {
                DropDownListUA.DataSourceID = "SqlDataSourceDropUA";
                DropDownListUA.DataBind();
            }
            else
            {
                DropDownListUA.DataSourceID = "SqlDataSourceDropUA_UA";
                DropDownListUA.DataBind();
            }

        }
        else
        {
          
        }
    }

    protected void DropDownListUA_DataBound(object sender, EventArgs e)
    {
        DropDownListUA.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DropDownListUA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListUA.SelectedIndex == 0)
        {
            GridPliegos.Visible = false;
        }
        else
        {
            GridPliegos.Visible = true;
        }
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
            int CellEstatus = 7;
            //int CellDetalles = 8;

            String Estatus = e.Row.Cells[1].Text;

            if(Estatus == "1")
            {
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ImageUrl = "~/public/img/peticion-roja.png";
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ToolTip = "Pendiente";
            }
            else if(Estatus == "2")
            {
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ImageUrl = "~/public/img/peticion-amarilla.png";
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ToolTip = "En proceso";
            }
            else if (Estatus == "3")
            {
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ImageUrl = "~/public/img/peticion-verde.png";
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ToolTip = "Atendido";
            }
            else if (Estatus == "4")
            {
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ImageUrl = "~/public/img/peticion-negra.png";
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ToolTip = "Vencido";
            }
            else
            {
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ImageUrl = "";
                ((ImageButton)e.Row.Cells[CellEstatus].FindControl("ImageButtonEstatus")).ToolTip = "Sin estatus";
            }
        }
    }

    protected void GridViewGarantias_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
    }

    protected void GridViewDG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.CssClass = "row-relative";
        }
    }

    protected void GridViewDG_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Expandir")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewDG.Rows[index];

            Panel pnl = row.FindControl("pnlDetalles") as Panel;
            LinkButton btn = row.FindControl("btnExpand") as LinkButton;

            pnl.Visible = !pnl.Visible;

            // Cambiar ícono
            btn.Text = pnl.Visible ? "-" : "+";
        }
    }

    protected void GridViewGestiones_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            
        }
    }

    //Funciones
    protected String UnidadAsignada(string idpeticion)
    {
        String DescUnidad = Consultas.ConsultaS("select DESC_UNIDAD from ASIGNACION_PETICION where ID_PETICION = '"+ idpeticion + "' ");

        if(String.IsNullOrEmpty(DescUnidad))
        {
            return "Sin asignar";
        }
        else
        {
            return DescUnidad;
        }

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

        GridViewPeticiones.DataBind();

        string javaScriptHDocD2 = "ShowModalDetallesPliego();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonEstatus_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewPeticiones.SelectedIndex = i;

        LabelFechaP.Text = GridViewPeticiones.Rows[i].Cells[3].Text;
        LabelIdPeticion.Text = GridViewPeticiones.Rows[i].Cells[2].Text;
        LabelPeticionP.Text = GridViewPeticiones.Rows[i].Cells[4].Text;
        LabelIdEstatus.Text = GridViewPeticiones.Rows[i].Cells[1].Text;
        LabelNomEstatus.Text = Consultas.ConsultaS("select DESCRIPCION_PETICION from ESTATUS_PETICION where ID_EST_PETICION = '"+ LabelIdEstatus.Text + "'");

        LabelCategoriaP.Text = GridViewPeticiones.Rows[i].Cells[5].Text;
        LabelSubCategoriaP.Text = GridViewPeticiones.Rows[i].Cells[6].Text;

        if (LabelIdEstatus.Text == "1")
        {
            TCardEstatus.Attributes.Add("class", "timeline-4 left-4 arrowL-red");
            CCardEstatus.Attributes.Add("class", "card color-rojo-custom");

            ButtonValidar.Enabled = false;
        }
        else if (LabelIdEstatus.Text == "2")
        {
            TCardEstatus.Attributes.Add("class", "timeline-4 left-4 arrowL-yellow");
            CCardEstatus.Attributes.Add("class", "card color-amarillo-custom");

            ButtonValidar.Enabled = true;
        }
        else if (LabelIdEstatus.Text == "3")
        {
            TCardEstatus.Attributes.Add("class", "timeline-4 left-4 arrowL-green");
            CCardEstatus.Attributes.Add("class", "card color-verde-custom");

            ButtonValidar.Enabled = false;
        }
        else if (LabelIdEstatus.Text == "4")
        {
            TCardEstatus.Attributes.Add("class", "timeline-4 left-4 arrowL-black");
            CCardEstatus.Attributes.Add("class", "card color-negro-custom");

            ButtonValidar.Enabled = false;
        }
        else
        {
            ButtonValidar.Enabled = false;
        }


        string javaScriptHDocD2 = "ShowModalEstatus();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonSelectDetalles_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewPeticiones.SelectedIndex = i;

        LabelIdPeticionDet.Text = GridViewPeticiones.Rows[i].Cells[2].Text;
        LabelIdEstatusDet.Text = GridViewPeticiones.Rows[i].Cells[1].Text;

        LabelFecDP.Text = GridViewPeticiones.Rows[i].Cells[3].Text;
        LabelPeticionDP.Text = GridViewPeticiones.Rows[i].Cells[4].Text;

        LabelCategoriaDP.Text = GridViewPeticiones.Rows[i].Cells[5].Text;
        LabelSubCategoriaDP.Text = GridViewPeticiones.Rows[i].Cells[6].Text;

        LabelFecResp.Text = Consultas.ConsultaS("select CONVERT(varchar,FECHA_RESP_PETICION,103) as FECHA_RESP from PETICIONES where ID_PLIEGO = '" + LabelIdPliego.Text + "' and ID_PETICION = '" + LabelIdPeticionDet.Text + "' ");
        LabelDescResp.Text = Consultas.ConsultaS("select DESC_RESP_PETICION from PETICIONES where ID_PLIEGO = '" + LabelIdPliego.Text + "' and ID_PETICION = '" + LabelIdPeticionDet.Text + "' ");
        LabelFechaComproDet.Text = Consultas.ConsultaS("select CONVERT(varchar,FECHA_CUMPLIMIENTO,103) as FECHA_RESP from PETICIONES where ID_PLIEGO = '" + LabelIdPliego.Text + "' and ID_PETICION = '" + LabelIdPeticionDet.Text + "' ");
        LabelAsignacion.Text = UnidadAsignada(LabelIdPeticionDet.Text);

        string javaScriptHDocD2 = "ShowModalDetallesPeticion();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ButtonValidar_Click(object sender, EventArgs e)
    {

        string javaScriptHDocD2 = "ShowModalMSGValida();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ButtonValidaMSG_Click(object sender, EventArgs e)
    {
        Consultas.miUpdate("UPDATE PETICIONES SET ID_EST_PETICION = 3 WHERE ID_PLIEGO = '"+ LabelIdPliego.Text + "' AND ID_PETICION = '"+ LabelIdPeticion.Text + "'");

        LabelNomEstatus.Text = Consultas.ConsultaS("select DESCRIPCION_PETICION from ESTATUS_PETICION where ID_EST_PETICION = '3'");

        TCardEstatus.Attributes.Add("class", "timeline-4 left-4 arrowL-green");
        CCardEstatus.Attributes.Add("class", "card color-verde-custom");

        ButtonValidar.Enabled = false;

        GridViewPeticiones.DataBind();

        string javaScriptHDocD2 = "HideModalMSGValida();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonArchivoGarantia_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewGarantias.SelectedIndex = i;

        String RutaArchivoGarantia = ((Label)GridViewGarantias.Rows[i].Cells[4].FindControl("LabelRutaArchivoGarantia")).Text.Remove(0, 1);

        Random rnd = new Random();
        int numa = rnd.Next(1, 999);
        verPDF.Attributes["src"] = RutaArchivoGarantia + "?v=" + numa.ToString();
        verPDF.DataBind();

        string javaScriptHDocD2 = "ShowModalVerArchivo();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonArchivoDiagnostico_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int i = G_B.RowIndex;
        GridViewDG.SelectedIndex = i;

        Random rnd = new Random();
        int numa = rnd.Next(1, 999);
        verPDF.Attributes["src"] = ((Label)GridViewDG.Rows[i].Cells[3].FindControl("LabelRutaArchivoDiagnostico")).Text + "?v=" + numa.ToString();
        verPDF.DataBind();

        string javaScriptHDocD2 = "ShowModalVerArchivo();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ImageButtonArchivoGestiones_Click(object sender, EventArgs e)
    {
        ImageButton S_B = (ImageButton)sender;
        GridViewRow G_B = (GridViewRow)(S_B.Parent.Parent);
        int indexDiag= G_B.RowIndex;

        GridView GG = (GridView)G_B.FindControl("GridViewGestiones");
        GridViewRow filaGG = (GridViewRow)S_B.NamingContainer;
        int indexGG = filaGG.RowIndex;

        Random rnd = new Random();
        int numa = rnd.Next(1, 999);
        verPDF.Attributes["src"] = ((Label)filaGG.FindControl("LabelRutaArchivoGestiones")).Text + "?v=" + numa.ToString();
        verPDF.DataBind();

        string javaScriptHDocD2 = "ShowModalVerArchivo();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }

    protected void ButtonDetallesP_Click(object sender, EventArgs e)
    {
        LabelIdPeticionDet.Text = LabelIdPeticion.Text;
        LabelIdEstatusDet.Text = LabelIdEstatus.Text;

        LabelFecDP.Text = LabelFechaP.Text;
        LabelPeticionDP.Text = LabelPeticionP.Text;

        LabelCategoriaDP.Text = LabelCategoriaP.Text;
        LabelSubCategoriaDP.Text = LabelSubCategoriaP.Text;

        LabelFecResp.Text = Consultas.ConsultaS("select CONVERT(varchar,FECHA_RESP_PETICION,103) as FECHA_RESP from PETICIONES where ID_PLIEGO = '" + LabelIdPliego.Text + "' and ID_PETICION = '" + LabelIdPeticionDet.Text + "' ");
        LabelDescResp.Text = Consultas.ConsultaS("select DESC_RESP_PETICION from PETICIONES where ID_PLIEGO = '" + LabelIdPliego.Text + "' and ID_PETICION = '" + LabelIdPeticionDet.Text + "' ");
        LabelFechaComproDet.Text = Consultas.ConsultaS("select CONVERT(varchar,FECHA_CUMPLIMIENTO,103) as FECHA_RESP from PETICIONES where ID_PLIEGO = '" + LabelIdPliego.Text + "' and ID_PETICION = '" + LabelIdPeticionDet.Text + "' ");
        LabelAsignacion.Text = UnidadAsignada(LabelIdPeticionDet.Text);

        string javaScriptHDocD2 = "ShowModalDetallesPeticion();";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", javaScriptHDocD2, true);
    }
}