using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CargaReglamentariaDocente
/// </summary>
public class CargaReglamentariaDocente
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringSAIEE"].ConnectionString;

    public CargaReglamentariaDocente()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    //Zonas pagadoras docente
    public static DataTable ObtenerZonasPagadorasDocente(string numeroEmpleado)
    {       
        DataTable query = Consultas.miDataTable("SELECT DISTINCT ch.ZONA_PAGADORA, cd.DESCRIPCION_DP, cd.ID_NIVEL_EST " +
            "FROM CAPITAL_HUMANO_PLAZAS ch, CAT_DEPENDENCIAS_POLITECNICAS cd " +
            "WHERE ch.ZONA_PAGADORA = cd.CLAVE_ZP AND NUMERO_EMPLEADO = '" + numeroEmpleado + "'");
        return query;
    }

    //Zonas pagadoras historial docente
    public static DataTable ObtenerZonasPagadorasDocenteHist(string numeroEmpleado, string periodo)
    {
        DataTable query = Consultas.miDataTable("SELECT DISTINCT hcd.CLAVE_ZP, cd.DESCRIPCION_DP FROM HISTORIAL_CARGA_DOCENTE hcd, " +
            "CAT_DEPENDENCIAS_POLITECNICAS cd WHERE hcd.CLAVE_ZP = cd.CLAVE_ZP AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND PERIODO_ESCOLAR = '" + periodo + "'");
        return query;
    }

    //Horas base docente por zona
    public static int ObtenerHorasBaseDocente(string numeroEmpleado, string claveZP)
    {
        int horasBase = Consultas.ConsultaInt("SELECT ISNULL(SUM(horas),0) as horasB FROM CAPITAL_HUMANO_PLAZAS " +
            "WHERE NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND ZONA_PAGADORA = '" + claveZP + "'");
        return horasBase;
    }

    //Horas minimas y maximas docente por zona
    public class CargaEmpleado
    {
        public int CargaMinima { get; set; }
        public int CargaMaxima { get; set; }

        public CargaEmpleado(int cargaMinima, int cargaMaxima)
        {
            CargaMinima = cargaMinima;
            CargaMaxima = cargaMaxima;
        }
    }

    public static CargaEmpleado ObtenerHorasMinMax(string numeroEmpleado, string clavezp, string nivel, string connection)
    {
        using (SqlConnection conn = new SqlConnection(connection))
        {
            using (SqlCommand cmd = new SqlCommand("DOCENTE_HORAS_MINIMAS_MAXIMAS", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ne", numeroEmpleado);
                cmd.Parameters.AddWithValue("@zp", clavezp);
                cmd.Parameters.AddWithValue("@NivelE", nivel);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int cargaMinima = reader.GetInt32(reader.GetOrdinal("CARGA_MINIMA"));
                        int cargaMaxima = reader.GetInt32(reader.GetOrdinal("CARGA_MAXIMA"));

                        return new CargaEmpleado(cargaMinima, cargaMaxima);
                    }
                }
            }
        }

        return new CargaEmpleado(0, 0); // Valores por defecto si no se encuentra registro
    }

    //Horas interinas docente por zona
    public static float ObtenerHorasInterinasDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasInterinasActual = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt FROM CARGA_DOCENTE as cd " +
            "WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' AND cd.NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND cd.INTERINATO = '1' ");
        return horasInterinasActual;
    }

    //Horas interinas escolarizadas docente por zona
    public static float ObtenerHorasInterinasEscolarizadaDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasInterinasEActual = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt FROM CARGA_DOCENTE as cd " +
            "WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' AND cd.NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND cd.INTERINATO = '1' AND cd.MODALIDAD = 1");
        return horasInterinasEActual;
    }

    //Horas interinas no escolarizadas docente por zona
    public static float ObtenerHorasInterinasNoEscolarizadaDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasInterinasNEActual = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt FROM CARGA_DOCENTE as cd " +
            "WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' AND cd.NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND cd.INTERINATO = '1' AND cd.MODALIDAD = 2");
        return horasInterinasNEActual;
    }

    //Horas base frente a grupo total docente por zona
    public static float ObtenerHFGBaseDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasFGBase = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HfgBase FROM CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND(INTERINATO = '0' OR INTERINATO IS NULL)");
        return horasFGBase;
    }

    //Horas base frente a grupo  escolarizada docente por zona
    public static float ObtenerHFGBaseEscolarizadaDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasFGBase = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HfgBase FROM CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND(INTERINATO = '0' OR INTERINATO IS NULL) AND MODALIDAD = 1");
        return horasFGBase;
    }

    //Horas base frente grupo no escolarizada docente por zona
    public static float ObtenerHFGBaseNoEscolarizadaDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasFGBase = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HfgBase FROM CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND(INTERINATO = '0' OR INTERINATO IS NULL) AND MODALIDAD = 2");
        return horasFGBase;
    }

    //Horas frente a grupo asignadas docente por zona (hrs base  +  hrs interinas)
    public static string ObtenerHFGAsignadasDocente(string numeroEmpleado, string claveZP, string periodo)
    {
        string horasHFGAsignadas = Consultas.ConsultaS("SELECT (SELECT ISNULL(SUM(HORAS_ASIGNADAS), 0) FROM CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND INTERINATO = '1') + " +
            "(SELECT ISNULL(SUM(HORAS_ASIGNADAS), 0) FROM CARGA_DOCENTE WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' " +
            "AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND(INTERINATO = '0' OR INTERINATO IS NULL)) AS TotalHoras; ");
        return horasHFGAsignadas;
    }

    //Horas interinas docente por zona historial
    public static float ObtenerHorasInterinasDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasInterinasHist = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt FROM HISTORIAL_CARGA_DOCENTE as cd " +
            "WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' AND cd.NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND cd.INTERINATO = '1' ");
        return horasInterinasHist;
    }

    //Horas interinas escolarizadas docente por zona historial
    public static float ObtenerHrsInterinasEscolarizadaDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasInterinasEActual = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt FROM HISTORIAL_CARGA_DOCENTE as cd " +
            "WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' AND cd.NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND cd.INTERINATO = '1' AND cd.MODALIDAD = 1");
        return horasInterinasEActual;
    }

    //Horas interinas no escolarizadas docente por zona historial
    public static float ObtenerHrsInterinasNoEscolarizadaDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasInterinasNEActual = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt FROM HISTORIAL_CARGA_DOCENTE as cd " +
            "WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' AND cd.NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND cd.INTERINATO = '1' AND cd.MODALIDAD = 2");
        return horasInterinasNEActual;
    }

    //Horas base frente a grupo total docente por zona historial
    public static float ObtenerHFGBaseDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasFGBase = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HfgBase FROM HISTORIAL_CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND(INTERINATO = '0' OR INTERINATO IS NULL)");
        return horasFGBase;
    }

    //Horas base frente a grupo  escolarizada docente por zona historial
    public static float ObtenerHFGBaseEscolarizadaDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasFGBase = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HfgBase FROM HISTORIAL_CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND(INTERINATO = '0' OR INTERINATO IS NULL) AND MODALIDAD = 1");
        return horasFGBase;
    }

    //Horas base frente grupo no escolarizada docente por zona historial
    public static float ObtenerHFGBaseNoEscolarizadaDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        float horasFGBase = Consultas.ConsultaFloat("SELECT ISNULL(SUM(HORAS_ASIGNADAS),0) as HfgBase FROM HISTORIAL_CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' " +
            "AND(INTERINATO = '0' OR INTERINATO IS NULL) AND MODALIDAD = 2");
        return horasFGBase;
    }

    //Horas frente a grupo asignadas docente por zona (hrs base  +  hrs interinas) historial
    public static string ObtenerHFGAsignadasDocenteHist(string numeroEmpleado, string claveZP, string periodo)
    {
        string horasHFGAsignadas = Consultas.ConsultaS("SELECT (SELECT ISNULL(SUM(HORAS_ASIGNADAS), 0) FROM HISTORIAL_CARGA_DOCENTE " +
            "WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND INTERINATO = '1') + " +
            "(SELECT ISNULL(SUM(HORAS_ASIGNADAS), 0) FROM HISTORIAL_CARGA_DOCENTE WHERE PERIODO_ESCOLAR = '" + periodo + "' AND CLAVE_ZP = '" + claveZP + "' " +
            "AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND(INTERINATO = '0' OR INTERINATO IS NULL)) AS TotalHoras; ");
        return horasHFGAsignadas;
    }

    //Zonas interinas de otras unidades academicas
    public static DataTable ObtenerZPInterinasDocente(int zonasP, string numeroEmpleado, string periodo1, string periodo2, string claveZP1, string claveZP2)
    {
        DataTable query = null;
        if (zonasP == 1)
        {
            query = Consultas.miDataTable("SELECT DISTINCT cd.CLAVE_ZP, cdp.DESCRIPCION_DP, cdp.ID_NIVEL_EST " +
                "FROM CARGA_DOCENTE cd, CAT_DEPENDENCIAS_POLITECNICAS cdp WHERE cd.CLAVE_ZP = cdp.CLAVE_ZP " +
                "AND PERIODO_ESCOLAR IN ('" + periodo1 + "') " +
                "AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND cd.CLAVE_ZP NOT IN ('" + claveZP1 + "')");
        }
        else if (zonasP >= 2)
        {

            query = Consultas.miDataTable("SELECT DISTINCT cd.CLAVE_ZP, cdp.DESCRIPCION_DP, cdp.ID_NIVEL_EST " +
                "FROM CARGA_DOCENTE cd, CAT_DEPENDENCIAS_POLITECNICAS cdp WHERE cd.CLAVE_ZP = cdp.CLAVE_ZP " +
                "AND PERIODO_ESCOLAR IN ('" + periodo1 + "', '" + periodo2 + "') " +
                "AND NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND cd.CLAVE_ZP NOT IN ('" + claveZP1 + "', '" + claveZP2 + "')");
        }
        return query;
    }
    
    //Indicador personal Profe
    public static double IndicadorPersonalProfe(double HrsMax, double hrsAsig)
    {
        double IP = (hrsAsig * 100) / HrsMax;

        if (IP > 100)
        {
            return 100;
        }
        else
        {
            return Math.Truncate(IP);
        }
    }
}