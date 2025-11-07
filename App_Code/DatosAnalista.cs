using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DatosUser
/// </summary>
public class DatosAnalista
{
    public DatosAnalista()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static string AsignacionZP(string idUser, string pe)
    {
        string zonas = Consultas.ConsultaS("select case when STRING_AGG(CLAVE_ZP,',') is null then '0' else STRING_AGG(CLAVE_ZP,',') end CLAVE_ZP from ASIGNACION_ZP where ID_USUARIO = '" + idUser +"' and PERIODO_ESCOLAR = '"+ pe +"' and ESTATUS = 1");
        return zonas;
    }
    public static string totalAsignacionZP(string idUser, string pe)
    {
        string zonas = Consultas.ConsultaS("select count(CLAVE_ZP) CLAVE_ZP from ASIGNACION_ZP where ID_USUARIO = '" + idUser +"' and PERIODO_ESCOLAR = '"+ pe +"' and ESTATUS = 1");
        return zonas;
    }

    public static string arrayAsignacionZP(string idUser, string pe)
    {
        string zonas = Consultas.ConsultaS("select case when STRING_AGG(dp.NOMBRE_CORTO,',') is null then 'Sin asignaciones' else STRING_AGG(dp.NOMBRE_CORTO,',') end CLAVE_ZP from ASIGNACION_ZP asig inner join CAT_DEPENDENCIAS_POLITECNICAS dp on dp.CLAVE_ZP = asig.CLAVE_ZP where ID_USUARIO = '" + idUser + "' and PERIODO_ESCOLAR = '" + pe + "' and ESTATUS = 1");
        return zonas;
    }

}