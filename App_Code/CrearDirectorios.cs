using System.IO;

/// <summary>
/// Descripción breve de CrearDirectorios
/// </summary>
public class CrearDirectorios
{
    public CrearDirectorios()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    // directorio docente
    public static string Guardar_archivoPDF(string NoEmpleado, params string[] carpetas)
    {
        // Directorio principal de documentos
        string directorio = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Archivos/Docente/";

        // Ruta base
        string pathString = System.IO.Path.Combine(directorio, NoEmpleado);

        // Crear el directorio base si no existe
        if (!Directory.Exists(pathString))
        {
            System.IO.Directory.CreateDirectory(pathString);
        }

        // Recorrer la lista de carpetas para crearlas
        foreach (string carpeta in carpetas)
        {
            pathString = System.IO.Path.Combine(pathString, carpeta);

            // Crear la carpeta si no existe
            if (!Directory.Exists(pathString))
            {
                System.IO.Directory.CreateDirectory(pathString);
            }
        }

        return pathString + "\\";
    }

    // crea directorio para algun perfil con sus subcarpetas
    public static string Guardar_carpeta(int idPerfil, params string[] carpetas)
    {
        string perfil = ObtenerPerfil(idPerfil);
        // Directorio principal de documentos
        string directorio = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Archivos/" + perfil + "/";

        // Ruta base
        string pathString = System.IO.Path.Combine(directorio);

        // Crear el directorio base si no existe
        if (!Directory.Exists(pathString))
        {
            System.IO.Directory.CreateDirectory(pathString);
        }

        // Recorrer la lista de carpetas para crearlas
        foreach (string carpeta in carpetas)
        {
            pathString = System.IO.Path.Combine(pathString, carpeta);

            // Crear la carpeta si no existe
            if (!Directory.Exists(pathString))
            {
                System.IO.Directory.CreateDirectory(pathString);
            }
        }

        return pathString + "\\";
    }

    private static string ObtenerPerfil(int idPerfil)
    {
        switch (idPerfil)
        {
            case 16: return "Docente";
            case 42: return "Coordinacion_EstructuraE";
            //agregar los perfiles necesarios
            default: return null;
        }
    }

}