using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DependenciaIPN
/// </summary>
public class DependenciaIPN
{
    public DependenciaIPN()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static string ObtenerNombreDependencia(string claveZP)
    {
        string dependencia = Consultas.ConsultaS("select DESCRIPCION_DP from CAT_DEPENDENCIAS_POLITECNICAS where CLAVE_ZP = '" + claveZP + "'");
        return dependencia;
    }

    public static int ObtenerClaveUADependencia(string claveZP)
    {
        int claveUA = 0;
        string consulta = "SELECT ISNULL(CLAVE_UA, 0) as CLAVE_UA from CAT_DEPENDENCIAS_POLITECNICAS WHERE CLAVE_ZP = @claveZP";

        claveUA = Consultas.ConsultaIntP(consulta, new SqlParameter("@claveZP", claveZP));
        return claveUA;
    }

    public static int ObtenerNivelEstructura(string claveZP)
    {
        int nivelEst = 0;
        // Consultar el nivel de la claveZP(zona pagadora)
        string consulta = "SELECT DISTINCT ID_NIVEL_EST FROM CAT_PERFILES WHERE CLAVE_ZP = @claveZP";

        nivelEst = Consultas.ConsultaIntP(consulta, new SqlParameter("@claveZP", claveZP));
        return nivelEst;
    }
}