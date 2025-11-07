using System.IO;
using System.Web.UI.WebControls;


/// <summary>
/// Descripción breve de LabelData
/// </summary>
public class LabelData
{
    public LabelData()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static object LabelFiltro_valor { get; private set; }

    public static string StringLabel(Label nombre)
    {

        string valor = nombre.Text; 

        return valor;
    }
}