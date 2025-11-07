using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de TechoPresupuestal
/// </summary>
public class TechoPresupuestal
{
    public TechoPresupuestal()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    //Solo cuando está en estatus 2
    public static void UpdateTechoPSolicitado(string claveZP)
    {
        Consultas.miUpdate("UPDATE TECH " +
                                "SET TECH.TP_SOLICITADO = techT.Techo " +
                            "FROM TECHO_PRESUPUESTAL_ZP TECH " +
                                "INNER JOIN " +
                                    "(SELECT techTemp.CLAVE_ZP, techTemp.Techo FROM " +
                                        "(SELECT CLAVE_ZP, CEILING(SUM(cd.HORAS_ASIGNADAS)) as Techo " +
                                            "FROM CARGA_DOCENTE cd " +
                                            "WHERE cd.CLAVE_ZP = '" + claveZP + "' " +
                                                "AND cd.PERIODO_ESCOLAR = (SELECT p.PE_ACTUAL FROM PARAMETROS p " +
                                                    "WHERE p.CLAVE_ZP = cd.CLAVE_ZP) " +
                                                "AND INTERINATO = 1 AND(INCIDENCIA != 1 OR INCIDENCIA IS NULL) " +
                                            "GROUP BY CLAVE_ZP " +
                                        ") AS techTemP " +
                                    ") techT " +
                            "ON TECH.CLAVE_ZP = techT.CLAVE_ZP");        
    }

    //Solo cuando está en estatus 3
    public static void UpdateTechoPAutorizado(string claveZP)
    {
        Consultas.miUpdate("UPDATE TECH " +
                                "SET TECH.TP_AUTORIZADO = techT.Techo " +
                            "FROM TECHO_PRESUPUESTAL_ZP TECH " +
                                "INNER JOIN " +
                                    "(SELECT techTemp.CLAVE_ZP, techTemp.Techo FROM " +
                                        "(SELECT CLAVE_ZP, CEILING(SUM(cd.HORAS_ASIGNADAS)) as Techo " +
                                            "FROM CARGA_DOCENTE cd " +
                                            "WHERE cd.CLAVE_ZP = '" + claveZP + "' " +
                                                "AND cd.PERIODO_ESCOLAR = (SELECT p.PE_ACTUAL FROM PARAMETROS p " +
                                                    "WHERE p.CLAVE_ZP = cd.CLAVE_ZP) " +
                                                "AND INTERINATO = 1 AND(INCIDENCIA != 1 OR INCIDENCIA IS NULL) " +
                                            "GROUP BY CLAVE_ZP " +
                                        ") AS techTemP " +
                                    ") techT " +
                            "ON TECH.CLAVE_ZP = techT.CLAVE_ZP");        
    }
}