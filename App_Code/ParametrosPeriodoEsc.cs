using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ParametrosPeriodoEsc
/// </summary>
public class ParametrosPeriodoEsc
{
    public ParametrosPeriodoEsc()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static int ObtenerPeriodoActual(string claveZP)
    {
        // Consultar la cantidad de puestos del usuario con el ID_USER especificado        
        int periodoActual = Consultas.ConsultaInt("SELECT PE_ACTUAL FROM PARAMETROS WHERE CLAVE_ZP = '" + claveZP + "'");
        return periodoActual;
    }

    public static int ObtenerPeriodoSiguiente(string claveZP)
    {
        // Consultar la cantidad de puestos del usuario con el ID_USER especificado        
        int periodoSiguiente = Consultas.ConsultaInt("SELECT PE_SIGUIENTE FROM PARAMETROS WHERE CLAVE_ZP = '" + claveZP + "'");
        return periodoSiguiente;
    }

    public static int ObtenerPeriodoAnterior(string claveZP)
    {
        // Consultar la cantidad de puestos del usuario con el ID_USER especificado        
        int periodoAnterior = Consultas.ConsultaInt("SELECT DISTINCT TOP (1) PERIODO_ESCOLAR from HISTORIAL_CARGA_DOCENTE WHERE CLAVE_ZP = '" + claveZP + "' " +
            "AND PERIODO_ESCOLAR NOT IN (SELECT PE_ACTUAL FROM PARAMETROS WHERE CLAVE_ZP = '" + claveZP + "')ORDER BY PERIODO_ESCOLAR DESC");
        return periodoAnterior;
    }

    public static int ObtenerEstatusPeriodoActual(string claveZP)
    {
        // Consultar la cantidad de puestos del usuario con el ID_USER especificado        
        int estatusPeriodoActual = Consultas.ConsultaInt("SELECT ESTATUS_PE_ACTUAL FROM PARAMETROS WHERE CLAVE_ZP = '" + claveZP + "'");
        return estatusPeriodoActual;
    }

    public static int ObtenerEstatusPeriodoSiguiente(string claveZP)
    {
        // Consultar la cantidad de puestos del usuario con el ID_USER especificado        
        int estatusPeriodoSiguiente = Consultas.ConsultaInt("SELECT ESTATUS_PE_SIGUIENTE FROM PARAMETROS WHERE CLAVE_ZP = '" + claveZP + "'");
        return estatusPeriodoSiguiente;
    }
}