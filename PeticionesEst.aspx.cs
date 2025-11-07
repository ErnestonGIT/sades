using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PeticionesEst : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void DropDownListUnidadAcademica_DataBound(object sender, EventArgs e)
    {
        DropDownListUnidadAcademica.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    protected void DDLCategoriaPeticion_DataBound(object sender, EventArgs e)
    {
        DDLCategoriaPeticion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
    }

    // Mostrar modal de confirmación de acción
    protected void ShowModalConfirm()
    {
        string javaScript1 = "ShowModalConfirm();";
        ScriptManager.RegisterStartupScript(this, GetType(), "script1", javaScript1, true);
    }

    protected void ButtonAddPeticion_Click(object sender, EventArgs e)
    {
        HtmlGenericControl DivConfirmCheck = modalConfirm.FindControl("DivConfirmCheck") as HtmlGenericControl;
        HtmlGenericControl DivConfirmError = modalConfirm.FindControl("DivConfirmError") as HtmlGenericControl;
        // Acceder al control LabelMensaje y establecer su texto
        Label LabelMensaje = modalConfirm.FindControl("LabelMensaje") as Label;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                int claveZp = Convert.ToInt32(DropDownListUnidadAcademica.SelectedValue);
                int categoria = Convert.ToInt32(DDLCategoriaPeticion.SelectedValue);
                int estatus = 1; //pendiente
                int idPeticion = obtenerIdMaxPeticion();

                string fechaPeticion = TextBoxDatePeticion.Text;
                DateTime datePeticion = Convert.ToDateTime(fechaPeticion);

                string descPeticion = TextBoxPeticion.Text;
                string fechaRespuesta = TextBoxDateAccSol.Text;
                DateTime dateRespuesta = Convert.ToDateTime(fechaRespuesta);
                 
                string descRespuesta = TextBoxRespuesta.Text;
                
                Consultas.miInsertP(InsertarPeticion(),
                    new SqlParameter("@claveZp", claveZp),
                    new SqlParameter("@idCategoria", categoria),
                    new SqlParameter("@idEstatus", estatus),
                    new SqlParameter("@idPeticion", idPeticion),
                    new SqlParameter("@fechaPeticion", datePeticion),
                    new SqlParameter("@descPeticion", descPeticion),
                    new SqlParameter("@fechaRespuesta", dateRespuesta),
                    new SqlParameter("@descRespuesta", descRespuesta));

                // Agregar mensaje de éxito
                DivConfirmCheck.Visible = true;
                LabelMensaje.Text = "Los datos se han insertado correctamente.";

                ClearAddPeticion();

            }
            catch (Exception ex)
            {
                // Si ocurre un error, deshacemos todos los cambios
                transaction.Rollback();
                Console.WriteLine("Error: " + ex.Message);
                // Agregar mensaje de error
                DivConfirmError.Visible = true;
                LabelMensaje.Text = "Ha ocurrido un error al insertar. Por favor, inténtelo de nuevo más tarde.";
            }
            ShowModalConfirm();
        }
    }

    private string InsertarPeticion()
    {
        string insertPeticion = "INSERT INTO PETICIONES_POR_UA (CLAVE_ZP,ID_CAT_PETICION,ID_EST_PETICION,ID_PETICION,FECHA_PETICION,DESC_PETICION,FECHA_RESP_PETICION,DESC_RESP_PETICION) " +
            "VALUES(@claveZp, @idCategoria, @idEstatus, @idPeticion, @fechaPeticion, @descPeticion, @fechaRespuesta, @descRespuesta)";

        return insertPeticion;
    }

    private int obtenerIdMaxPeticion ()
    {
        int idMaxPeticion = 0;

        idMaxPeticion = Consultas.ConsultaInt("SELECT MAX(ID_PETICION) + 1 FROM PETICIONES_POR_UA");

        return idMaxPeticion;
    }

    private void ClearAddPeticion()
    {
        DropDownListUnidadAcademica.ClearSelection();
        DDLCategoriaPeticion.ClearSelection();
        TextBoxDatePeticion.Text = string.Empty;
        TextBoxPeticion.Text = string.Empty;
        TextBoxDateAccSol.Text = string.Empty;
        TextBoxRespuesta.Text = string.Empty;
    }
}