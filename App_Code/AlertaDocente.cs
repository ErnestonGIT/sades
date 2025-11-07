using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de AlertaDocente
/// </summary>
public class AlertaDocente
{
    public AlertaDocente()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    //Excede Jornada Laboral por ZP
    public static int ObtenerDocenteExcedeJornadaLaboral(string numeroEmpleado, string claveZP, string periodo)
    {
        int excedeHorasLaborales = Consultas.ConsultaInt("SELECT COUNT(docente) as docentes from( " +
            "select distinct cd.NUMERO_EMPLEADO as docente from CARGA_DOCENTE as cd, RESUMEN_PLAZAS as rp where(cd.NUMERO_EMPLEADO = rp.NUMERO_EMPLEADO) " +
            "and  PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and rp.ID_TIPO_PLAZA not in (22) " +
            "AND(LUNES <> '' OR LUNES IS NULL) and cd.NUMERO_EMPLEADO not in (select distinct NUMERO_EMPLEADO from RESUMEN_PLAZAS " +
            "where ID_TIPO_PLAZA in ('22') ) group by cd.NUMERO_EMPLEADO having ISNULL(CAST(ISNULL(DATEDIFF(MINUTE, MIN(SUBSTRING(LUNES, 1, 5)), " +
            "MAX(SUBSTRING(LUNES, 7, 5))), 0) / 60.0 AS DECIMAL(20, 1)), 0) > 8 " +
            "UNION " +
            "select distinct cd.NUMERO_EMPLEADO as docente from CARGA_DOCENTE as cd, RESUMEN_PLAZAS as rp where(cd.NUMERO_EMPLEADO = rp.NUMERO_EMPLEADO) " +
            "and  PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and rp.ID_TIPO_PLAZA not in (22) " +
            "AND(MARTES <> '' OR MARTES IS NULL) and cd.NUMERO_EMPLEADO not in (select distinct NUMERO_EMPLEADO from RESUMEN_PLAZAS " +
            "where ID_TIPO_PLAZA in ('22') ) group by cd.NUMERO_EMPLEADO having ISNULL(CAST(ISNULL(DATEDIFF(MINUTE, MIN(SUBSTRING(MARTES, 1, 5)), " +
            "MAX(SUBSTRING(MARTES, 7, 5))), 0) / 60.0 AS DECIMAL(20, 1)), 0) > 8 " +
            "UNION " +
            "select distinct cd.NUMERO_EMPLEADO as docente from CARGA_DOCENTE as cd, RESUMEN_PLAZAS as rp where(cd.NUMERO_EMPLEADO = rp.NUMERO_EMPLEADO) " +
            "and  PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and rp.ID_TIPO_PLAZA not in (22) " +
            "AND(MIERCOLES <> '' OR MIERCOLES IS NULL) and cd.NUMERO_EMPLEADO not in (select distinct NUMERO_EMPLEADO from RESUMEN_PLAZAS " +
            "where ID_TIPO_PLAZA in ('22') ) group by cd.NUMERO_EMPLEADO having ISNULL(CAST(ISNULL(DATEDIFF(MINUTE, MIN(SUBSTRING(MIERCOLES, 1, 5)), " +
            "MAX(SUBSTRING(MIERCOLES, 7, 5))), 0) / 60.0 AS DECIMAL(20, 1)), 0) > 8 " +
            "UNION " +
            "select distinct cd.NUMERO_EMPLEADO as docente from CARGA_DOCENTE as cd, RESUMEN_PLAZAS as rp where(cd.NUMERO_EMPLEADO = rp.NUMERO_EMPLEADO) " +
            "and  PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and rp.ID_TIPO_PLAZA not in (22) " +
            "AND(JUEVES <> '' OR JUEVES IS NULL) and cd.NUMERO_EMPLEADO not in (select distinct NUMERO_EMPLEADO from RESUMEN_PLAZAS " +
            "where ID_TIPO_PLAZA in ('22') ) group by cd.NUMERO_EMPLEADO having ISNULL(CAST(ISNULL(DATEDIFF(MINUTE, MIN(SUBSTRING(JUEVES, 1, 5)), " +
            "MAX(SUBSTRING(JUEVES, 7, 5))), 0) / 60.0 AS DECIMAL(20, 1)), 0) > 8 " +
            "UNION " +
            "select distinct cd.NUMERO_EMPLEADO as docente from CARGA_DOCENTE as cd, RESUMEN_PLAZAS as rp where(cd.NUMERO_EMPLEADO = rp.NUMERO_EMPLEADO) " +
            "and  PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and rp.ID_TIPO_PLAZA not in (22) " +
            "AND(VIERNES <> '' OR VIERNES IS NULL) and cd.NUMERO_EMPLEADO not in (select distinct NUMERO_EMPLEADO from RESUMEN_PLAZAS " +
            "where ID_TIPO_PLAZA in ('22') ) group by cd.NUMERO_EMPLEADO having ISNULL(CAST(ISNULL(DATEDIFF(MINUTE, MIN(SUBSTRING(VIERNES, 1, 5)), " +
            "MAX(SUBSTRING(VIERNES, 7, 5))), 0) / 60.0 AS DECIMAL(20, 1)), 0) > 8 " +
            "UNION " +
            "select distinct cd.NUMERO_EMPLEADO as docente from CARGA_DOCENTE as cd, RESUMEN_PLAZAS as rp where(cd.NUMERO_EMPLEADO = rp.NUMERO_EMPLEADO) " +
            "and  PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and rp.ID_TIPO_PLAZA not in (22) " +
            "AND(SABADO <> '' OR SABADO IS NULL) and cd.NUMERO_EMPLEADO not in (select distinct NUMERO_EMPLEADO from RESUMEN_PLAZAS " +
            "where ID_TIPO_PLAZA in ('22') ) group by cd.NUMERO_EMPLEADO having ISNULL(CAST(ISNULL(DATEDIFF(MINUTE, MIN(SUBSTRING(SABADO, 1, 5)), " +
            "MAX(SUBSTRING(SABADO, 7, 5))), 0) / 60.0 AS DECIMAL(20, 1)), 0) > 8 " +
            ") as docentes where docente = '" + numeroEmpleado + "'");
        return excedeHorasLaborales;
    }

    //Carga menor a la mínima por ZP
    public static int ObtenerDocenteCargaMenorMinima(string numeroEmpleado, string claveZP, string periodo)
    {
        int cargaMenorMinima = Consultas.ConsultaInt("WITH HorasTotales AS (SELECT cd.NUMERO_EMPLEADO, ISNULL(SUM(CAST(ROUND(( " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.LUNES, 1, 5), SUBSTRING(cd.LUNES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MARTES, 1, 5), SUBSTRING(cd.MARTES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MIERCOLES, 1, 5), SUBSTRING(cd.MIERCOLES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.JUEVES, 1, 5), SUBSTRING(cd.JUEVES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.VIERNES, 1, 5), SUBSTRING(cd.VIERNES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.SABADO, 1, 5), SUBSTRING(cd.SABADO, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.DOMINGO, 1, 5), SUBSTRING(cd.DOMINGO, 7, 5)), 0)) / 60.0, 2, 1) AS DECIMAL(20, 1))), 0) AS HRS_TOT " +
            "FROM CARGA_DOCENTE cd WHERE cd.PERIODO_ESCOLAR = '"+ periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' " +
            "AND(cd.INTERINATO = '0' OR cd.INTERINATO IS NULL) GROUP BY cd.NUMERO_EMPLEADO ), " +
            "EmpleadosFiltrados AS(SELECT RP.NUMERO_EMPLEADO, CASE WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) <= 26 THEN SUM(RP.HFG_MIN) " +
            "WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) > 26 THEN 26 WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) <= 24 THEN SUM(RP.HFG_MIN) " +
            "WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) > 24 THEN 24 END AS HFG_MIN FROM RESUMEN_PLAZAS RP JOIN USERS U " +
            "ON RP.NUMERO_EMPLEADO = U.NUMERO_EMPLEADO WHERE RP.CLAVE_ZP = '" + claveZP + "' GROUP BY RP.NUMERO_EMPLEADO, RP.ID_NIVEL_EST, U.NOMBRE, " +
            "U.APELLIDO_PAT, U.APELLIDO_MAT HAVING CASE WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) <= 26 THEN SUM(RP.HFG_MIN) " +
            "WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) > 26 THEN 26 WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) <= 24 THEN SUM(RP.HFG_MIN) " +
            "WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) > 24 THEN 24 END > 0 ) " +
            "SELECT COUNT(EF.NUMERO_EMPLEADO) AS empleados FROM EmpleadosFiltrados EF JOIN HorasTotales HT ON EF.NUMERO_EMPLEADO = HT.NUMERO_EMPLEADO " +
            "WHERE EF.NUMERO_EMPLEADO = '"+ numeroEmpleado + "' AND EF.HFG_MIN > HT.HRS_TOT; ");
        return cargaMenorMinima;
    }
    
    //Carga menor a la máxima por ZP
    public static int ObtenerDocenteCargaMenorMaxima(string numeroEmpleado, string claveZP, string periodo)
    {
        int cargaMenorMaxima = Consultas.ConsultaInt("WITH HorasTotales AS (SELECT cd.NUMERO_EMPLEADO, ISNULL(SUM(CAST(ROUND(( " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.LUNES, 1, 5), SUBSTRING(cd.LUNES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MARTES, 1, 5), SUBSTRING(cd.MARTES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MIERCOLES, 1, 5), SUBSTRING(cd.MIERCOLES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.JUEVES, 1, 5), SUBSTRING(cd.JUEVES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.VIERNES, 1, 5), SUBSTRING(cd.VIERNES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.SABADO, 1, 5), SUBSTRING(cd.SABADO, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.DOMINGO, 1, 5), SUBSTRING(cd.DOMINGO, 7, 5)), 0) ) / 60.0, 2, 1) AS DECIMAL(20, 1))), 0) AS HRS_TOT " +
            "FROM CARGA_DOCENTE cd WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' " +
            "AND(cd.INTERINATO = '0' OR cd.INTERINATO IS NULL) GROUP BY cd.NUMERO_EMPLEADO ), " +
            "EmpleadosFiltrados AS(SELECT RP.NUMERO_EMPLEADO, CASE WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) <= 26 THEN SUM(RP.HFG_MIN) " +
            "WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) > 26 THEN 26 WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) <= 24 THEN SUM(RP.HFG_MIN) " +
            "WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) > 24 THEN 24 END AS HFG_MIN, CASE WHEN RP.ID_NIVEL_EST = 1 " +
            "AND SUM(RP.HFG_MAX) <= 26 THEN SUM(RP.HFG_MAX) WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MAX) > 26 THEN 26 WHEN RP.ID_NIVEL_EST = 2 " +
            "AND SUM(RP.HFG_MAX) <= 24 THEN SUM(RP.HFG_MAX) WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MAX) > 24 THEN 24 END AS HFG_MAX " +
            "FROM RESUMEN_PLAZAS RP WHERE RP.CLAVE_ZP = '" + claveZP + "' GROUP BY RP.NUMERO_EMPLEADO, RP.ID_NIVEL_EST HAVING CASE " +
            "WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) <= 26 THEN SUM(RP.HFG_MIN) WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MIN) > 26 THEN 26 " +
            "WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) <= 24 THEN SUM(RP.HFG_MIN) WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MIN) > 24 THEN 24 " +
            "END IS NOT NULL AND CASE WHEN RP.ID_NIVEL_EST = 1 AND SUM(RP.HFG_MAX) <= 26 THEN SUM(RP.HFG_MAX) WHEN RP.ID_NIVEL_EST = 1 " +
            "AND SUM(RP.HFG_MAX) > 26 THEN 26 WHEN RP.ID_NIVEL_EST = 2 AND SUM(RP.HFG_MAX) <= 24 THEN SUM(RP.HFG_MAX) WHEN RP.ID_NIVEL_EST = 2 " +
            "AND SUM(RP.HFG_MAX) > 24 THEN 24 END IS NOT NULL ) " +
            "SELECT COUNT(EF.NUMERO_EMPLEADO) AS empleados FROM EmpleadosFiltrados EF JOIN HorasTotales HT ON EF.NUMERO_EMPLEADO = HT.NUMERO_EMPLEADO " +
            "WHERE EF.NUMERO_EMPLEADO = '"+ numeroEmpleado + "'  AND HT.HRS_TOT >= EF.HFG_MIN AND HT.HRS_TOT < EF.HFG_MAX; ");
        return cargaMenorMaxima;
    }

    public static int HrsCalidadE_NivelEst(String Nivel_Est)
    {
        if (Nivel_Est == "1")
        {
            return 26;
        }
        if (Nivel_Est == "2")
        {
            return 24;
        }
        else
        {
            return 0;
        }
    }

    //Docentes que exceden las horas de calidad académica
    public static int ObtenerDocenteExcedeCalidadAcademica(string numeroEmpleado, string claveZP, string periodo, int hrsCalidad)
    {
        int excedeCalidadEducativa = Consultas.ConsultaInt("WITH HorasTotales AS (SELECT cd.NUMERO_EMPLEADO, ISNULL(SUM(CAST(ROUND(( " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.LUNES, 1, 5), SUBSTRING(cd.LUNES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MARTES, 1, 5), SUBSTRING(cd.MARTES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MIERCOLES, 1, 5), SUBSTRING(cd.MIERCOLES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.JUEVES, 1, 5), SUBSTRING(cd.JUEVES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.VIERNES, 1, 5), SUBSTRING(cd.VIERNES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.SABADO, 1, 5), SUBSTRING(cd.SABADO, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.DOMINGO, 1, 5), SUBSTRING(cd.DOMINGO, 7, 5)), 0) ) / 60.0, 2, 1) AS DECIMAL(20, 1))), 0) AS HRS_TOT " +
            "FROM CARGA_DOCENTE cd JOIN PLANES_ESTUDIO pe ON cd.CLAVE_ZP = pe.CLAVE_ZP AND cd.ID_ASIGNATURA = pe.ID_ASIGNATURA " +
            "AND cd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD WHERE cd.PERIODO_ESCOLAR = '" + periodo + "' AND cd.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd.NUMERO_EMPLEADO IS NOT NULL GROUP BY cd.NUMERO_EMPLEADO ) " +
            "SELECT COUNT(*) as empleados FROM HorasTotales WHERE NUMERO_EMPLEADO = '" + numeroEmpleado + "' AND HRS_TOT > " + hrsCalidad + ";");
        return excedeCalidadEducativa;
    }

    //Docentes con traslape en horario
    public static int ObtenerDocenteTraslapeHorario(string numeroEmpleado, string claveZP, string periodo)
    {
        int traslapeHorario = Consultas.ConsultaInt("SELECT COUNT(docenteH) as Num_GruposTraslape FROM(SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH " +
            "FROM CARGA_DOCENTE as cd_a INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.LUNES, 1, 5) < SUBSTRING(cd_b.LUNES, 7, 5) " +
            "AND SUBSTRING(cd_a.LUNES, 7, 5) > SUBSTRING(cd_b.LUNES, 1, 5) or(SUBSTRING(cd_a.LUNES, 1, 5) = SUBSTRING(cd_b.LUNES, 1, 5) " +
            "AND SUBSTRING(cd_a.LUNES, 7, 5) = SUBSTRING(cd_b.LUNES, 7, 5)))) AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.LUNES <> '' or cd_a.LUNES is null) GROUP BY cd_a.LUNES, cd_a.CLAVE_ZP, " +
            "cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.LUNES) > 1 UNION SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH FROM CARGA_DOCENTE as cd_a " +
            "INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.MARTES, 1, 5) < SUBSTRING(cd_b.MARTES, 7, 5) " +
            "AND SUBSTRING(cd_a.MARTES, 7, 5) > SUBSTRING(cd_b.MARTES, 1, 5) or(SUBSTRING(cd_a.MARTES, 1, 5) = SUBSTRING(cd_b.MARTES, 1, 5) " +
            "AND SUBSTRING(cd_a.MARTES, 7, 5) = SUBSTRING(cd_b.MARTES, 7, 5)))) AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.MARTES <> '' or cd_a.MARTES is null) GROUP BY cd_a.MARTES, " +
            "cd_a.CLAVE_ZP,cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.MARTES) > 1 UNION SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH " +
            "FROM CARGA_DOCENTE as cd_a INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.MIERCOLES, 1, 5) < SUBSTRING(cd_b.MIERCOLES, 7, 5) " +
            "AND SUBSTRING(cd_a.MIERCOLES, 7, 5) > SUBSTRING(cd_b.MIERCOLES, 1, 5) or(SUBSTRING(cd_a.MIERCOLES, 1, 5) = SUBSTRING(cd_b.MIERCOLES, 1, 5) " +
            "AND SUBSTRING(cd_a.MIERCOLES, 7, 5) = SUBSTRING(cd_b.MIERCOLES, 7, 5)))) AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.MIERCOLES <> '' or cd_a.MIERCOLES is null) GROUP BY cd_a.MIERCOLES, " +
            "cd_a.CLAVE_ZP,cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.MIERCOLES) > 1 UNION SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH " +
            "FROM CARGA_DOCENTE as cd_a INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.JUEVES, 1, 5) < SUBSTRING(cd_b.JUEVES, 7, 5) " +
            "AND SUBSTRING(cd_a.JUEVES, 7, 5) > SUBSTRING(cd_b.JUEVES, 1, 5) or(SUBSTRING(cd_a.JUEVES, 1, 5) = SUBSTRING(cd_b.JUEVES, 1, 5) " +
            "AND SUBSTRING(cd_a.JUEVES, 7, 5) = SUBSTRING(cd_b.JUEVES, 7, 5))))  AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.JUEVES <> '' or cd_a.JUEVES is null) GROUP BY cd_a.JUEVES, " +
            "cd_a.CLAVE_ZP,cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.JUEVES) > 1 UNION SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH " +
            "FROM CARGA_DOCENTE as cd_a INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.VIERNES, 1, 5) < SUBSTRING(cd_b.VIERNES, 7, 5) " +
            "AND SUBSTRING(cd_a.VIERNES, 7, 5) > SUBSTRING(cd_b.VIERNES, 1, 5) or(SUBSTRING(cd_a.VIERNES, 1, 5) = SUBSTRING(cd_b.VIERNES, 1, 5) " +
            "AND SUBSTRING(cd_a.VIERNES, 7, 5) = SUBSTRING(cd_b.VIERNES, 7, 5)))) AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.VIERNES <> '' or cd_a.VIERNES is null) GROUP BY cd_a.VIERNES, " + 
            "cd_a.CLAVE_ZP,cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.VIERNES) > 1 UNION SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH " +
            "FROM CARGA_DOCENTE as cd_a INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.SABADO, 1, 5) < SUBSTRING(cd_b.SABADO, 7, 5) " +
            "AND SUBSTRING(cd_a.SABADO, 7, 5) > SUBSTRING(cd_b.SABADO, 1, 5) or(SUBSTRING(cd_a.SABADO, 1, 5) = SUBSTRING(cd_b.SABADO, 1, 5) " +
            "AND SUBSTRING(cd_a.SABADO, 7, 5) = SUBSTRING(cd_b.SABADO, 7, 5)))) AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.SABADO <> '' or cd_a.SABADO is null) GROUP BY cd_a.SABADO, " +
            "cd_a.CLAVE_ZP,cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.SABADO) > 1 UNION SELECT distinct cd_a.NUMERO_EMPLEADO as docenteH " +
            "FROM CARGA_DOCENTE as cd_a INNER JOIN CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.DOMINGO, 1, 5) < SUBSTRING(cd_b.DOMINGO, 7, 5) " +
            "AND SUBSTRING(cd_a.DOMINGO, 7, 5) > SUBSTRING(cd_b.DOMINGO, 1, 5) or(SUBSTRING(cd_a.DOMINGO, 1, 5) = SUBSTRING(cd_b.DOMINGO, 1, 5) " +
            "AND SUBSTRING(cd_a.DOMINGO, 7, 5) = SUBSTRING(cd_b.DOMINGO, 7, 5)))) AND(cd_a.CLAVE_ZP = cd_b.CLAVE_ZP " +
            "AND cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO AND cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) AND cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "AND cd_a.PERIODO_ESCOLAR = '" + periodo + "' AND(cd_a.DOMINGO <> '' or cd_a.DOMINGO is null) GROUP BY cd_a.DOMINGO, " +
            "cd_a.CLAVE_ZP,cd_a.NUMERO_EMPLEADO HAVING COUNT(cd_a.DOMINGO) > 1 ) as docentes where docenteH = '" + numeroEmpleado + "' ");
        return traslapeHorario;
    }

    //Docentes con traslape en otra unidad académica
    public static int ObtenerDocenteTraslapeHorarioUA(string numeroEmpleado, string claveZP, string periodo)
    {
        int traslapeHorarioUA = Consultas.ConsultaInt("SELECT COUNT(docenteH) as Num_GruposTraslape from(select distinct cd_a.NUMERO_EMPLEADO as docenteH  " +
            "from CARGA_DOCENTE as cd_a inner join CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.LUNES, 1, 5) < SUBSTRING(cd_b.LUNES, 7, 5) " +
            "and SUBSTRING(cd_a.LUNES, 7, 5) > SUBSTRING(cd_b.LUNES, 1, 5) or(SUBSTRING(cd_a.LUNES, 1, 5) = SUBSTRING(cd_b.LUNES, 1, 5) " +
            "and SUBSTRING(cd_a.LUNES, 7, 5) = SUBSTRING(cd_b.LUNES, 7, 5)))) and(cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO " +
            "and cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) and cd_a.NUMERO_EMPLEADO in (select distinct NUMERO_EMPLEADO " +
            "from CARGA_DOCENTE as cd_1 where PERIODO_ESCOLAR = '" + periodo + "' and NUMERO_EMPLEADO is not null and numero_empleado in " +
            "(select distinct NUMERO_EMPLEADO from CARGA_DOCENTE where CLAVE_ZP = '" + claveZP + "') and cd_a.CLAVE_ZP = '" + claveZP + "' " +
            "and cd_b.CLAVE_ZP not in ('" + claveZP + "') group by NUMERO_EMPLEADO having count(distinct CLAVE_ZP) >= 2) " +
            "and cd_a.PERIODO_ESCOLAR = '" + periodo + "' and(cd_a.LUNES <> '' or cd_a.LUNES is null) group by cd_a.LUNES, cd_a.NUMERO_EMPLEADO " +
            "having COUNT(cd_a.LUNES) > 1 UNION select distinct cd_a.NUMERO_EMPLEADO as docenteH from CARGA_DOCENTE as cd_a inner join " +
            "CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.MARTES, 1, 5) < SUBSTRING(cd_b.MARTES, 7, 5) and SUBSTRING(cd_a.MARTES, 7, 5) > " +
            "SUBSTRING(cd_b.MARTES, 1, 5) or(SUBSTRING(cd_a.MARTES, 1, 5) = SUBSTRING(cd_b.MARTES, 1, 5) " +
            "and SUBSTRING(cd_a.MARTES, 7, 5) = SUBSTRING(cd_b.MARTES, 7, 5)))) and(cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO " +
            "and cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) and cd_a.NUMERO_EMPLEADO in (select distinct NUMERO_EMPLEADO from CARGA_DOCENTE as cd_1 " +
            "where PERIODO_ESCOLAR = '" + periodo + "' and NUMERO_EMPLEADO is not null and numero_empleado in (select distinct NUMERO_EMPLEADO " +
            "from CARGA_DOCENTE where CLAVE_ZP = '" + claveZP + "') and cd_a.CLAVE_ZP = '" + claveZP + "' and cd_b.CLAVE_ZP not in ('" + claveZP + "') " +
            "group by NUMERO_EMPLEADO having count(distinct CLAVE_ZP) >= 2) and cd_a.PERIODO_ESCOLAR = '" + periodo + "' " +
            "and(cd_a.MARTES <> '' or cd_a.MARTES is null) group by cd_a.MARTES, cd_a.NUMERO_EMPLEADO having COUNT(cd_a.MARTES) > 1 UNION " +
            "select distinct cd_a.NUMERO_EMPLEADO as docenteH  from CARGA_DOCENTE as cd_a inner join CARGA_DOCENTE as cd_b " +
            "on((SUBSTRING(cd_a.MIERCOLES, 1, 5) < SUBSTRING(cd_b.MIERCOLES, 7, 5) and SUBSTRING(cd_a.MIERCOLES, 7, 5) > SUBSTRING(cd_b.MIERCOLES, 1, 5) " +
            "or(SUBSTRING(cd_a.MIERCOLES, 1, 5) = SUBSTRING(cd_b.MIERCOLES, 1, 5) and SUBSTRING(cd_a.MIERCOLES, 7, 5) = SUBSTRING(cd_b.MIERCOLES, 7, 5)))) " +
            "and(cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO and cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) " +
            "and cd_a.NUMERO_EMPLEADO in (select distinct NUMERO_EMPLEADO from CARGA_DOCENTE as cd_1 where PERIODO_ESCOLAR = '" + periodo + "' " +
            "and NUMERO_EMPLEADO is not null and numero_empleado in (select distinct NUMERO_EMPLEADO from CARGA_DOCENTE where CLAVE_ZP = '" + claveZP + "') " +
            "and cd_a.CLAVE_ZP = '" + claveZP + "' and cd_b.CLAVE_ZP not in ('" + claveZP + "') group by NUMERO_EMPLEADO having count(distinct CLAVE_ZP) >= 2) " +
            "and cd_a.PERIODO_ESCOLAR = '" + periodo + "' and(cd_a.MIERCOLES <> '' or cd_a.MIERCOLES is null) group by cd_a.MIERCOLES, cd_a.NUMERO_EMPLEADO " +
            "having COUNT(cd_a.MIERCOLES) > 1 UNION select distinct cd_a.NUMERO_EMPLEADO as docenteH  from CARGA_DOCENTE as cd_a inner join " +
            "CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.JUEVES, 1, 5) < SUBSTRING(cd_b.JUEVES, 7, 5) and SUBSTRING(cd_a.JUEVES, 7, 5) > " +
            "SUBSTRING(cd_b.JUEVES, 1, 5) or(SUBSTRING(cd_a.JUEVES, 1, 5) = SUBSTRING(cd_b.JUEVES, 1, 5) " +
            "and SUBSTRING(cd_a.JUEVES, 7, 5) = SUBSTRING(cd_b.JUEVES, 7, 5)))) and(cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO " +
            "and cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) and cd_a.NUMERO_EMPLEADO in (select distinct NUMERO_EMPLEADO from CARGA_DOCENTE as cd_1 " +
            " where PERIODO_ESCOLAR = '" + periodo + "' and NUMERO_EMPLEADO is not null and numero_empleado in (select distinct NUMERO_EMPLEADO " +
            "from CARGA_DOCENTE where CLAVE_ZP = '" + claveZP + "') and cd_a.CLAVE_ZP = '" + claveZP + "' and cd_b.CLAVE_ZP not in ('" + claveZP + "') " +
            "group by NUMERO_EMPLEADO having count(distinct CLAVE_ZP) >= 2) and cd_a.PERIODO_ESCOLAR = '" + periodo + "' and(cd_a.JUEVES <> '' " +
            "or cd_a.JUEVES is null) group by cd_a.JUEVES, cd_a.NUMERO_EMPLEADO having COUNT(cd_a.JUEVES) > 1 UNION " +
            "select distinct cd_a.NUMERO_EMPLEADO as docenteH  from CARGA_DOCENTE as cd_a inner join CARGA_DOCENTE as cd_b " +
            "on((SUBSTRING(cd_a.VIERNES, 1, 5) < SUBSTRING(cd_b.VIERNES, 7, 5) and SUBSTRING(cd_a.VIERNES, 7, 5) > SUBSTRING(cd_b.VIERNES, 1, 5) " +
            "or(SUBSTRING(cd_a.VIERNES, 1, 5) = SUBSTRING(cd_b.VIERNES, 1, 5) and SUBSTRING(cd_a.VIERNES, 7, 5) = SUBSTRING(cd_b.VIERNES, 7, 5)))) " +
            "and(cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO and cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) and cd_a.NUMERO_EMPLEADO in " +
            "(select distinct NUMERO_EMPLEADO from CARGA_DOCENTE as cd_1 where PERIODO_ESCOLAR = '" + periodo + "' and NUMERO_EMPLEADO is not null " +
            "and numero_empleado in (select distinct NUMERO_EMPLEADO from CARGA_DOCENTE where CLAVE_ZP = '" + claveZP + "') " +
            "and cd_a.CLAVE_ZP = '" + claveZP + "' and cd_b.CLAVE_ZP not in ('" + claveZP + "') group by NUMERO_EMPLEADO having " +
            "count(distinct CLAVE_ZP) >= 2) and cd_a.PERIODO_ESCOLAR = '" + periodo + "' and(cd_a.VIERNES <> '' or cd_a.VIERNES is null) " +
            "group by cd_a.VIERNES, cd_a.NUMERO_EMPLEADO having COUNT(cd_a.VIERNES) > 1 UNION select distinct cd_a.NUMERO_EMPLEADO as docenteH  " +
            "from CARGA_DOCENTE as cd_a inner join CARGA_DOCENTE as cd_b on((SUBSTRING(cd_a.SABADO, 1, 5) < SUBSTRING(cd_b.SABADO, 7, 5) " +
            "and SUBSTRING(cd_a.SABADO, 7, 5) > SUBSTRING(cd_b.SABADO, 1, 5) or(SUBSTRING(cd_a.SABADO, 1, 5) = SUBSTRING(cd_b.SABADO, 1, 5) " +
            "and SUBSTRING(cd_a.SABADO, 7, 5) = SUBSTRING(cd_b.SABADO, 7, 5)))) and(cd_a.NUMERO_EMPLEADO = cd_b.NUMERO_EMPLEADO " +
            "and cd_a.PERIODO_ESCOLAR = cd_b.PERIODO_ESCOLAR) and cd_a.NUMERO_EMPLEADO in (select distinct NUMERO_EMPLEADO from CARGA_DOCENTE as cd_1 " +
            "where PERIODO_ESCOLAR = '" + periodo + "' and NUMERO_EMPLEADO is not null and numero_empleado in (select distinct NUMERO_EMPLEADO " +
            "from CARGA_DOCENTE where CLAVE_ZP = '" + claveZP + "') and cd_a.CLAVE_ZP = '" + claveZP + "' and cd_b.CLAVE_ZP not in ('" + claveZP + "') " +
            "group by NUMERO_EMPLEADO having count(distinct CLAVE_ZP) >= 2) and cd_a.PERIODO_ESCOLAR = '" + periodo + "' and(cd_a.SABADO <> '' " +
            "or cd_a.SABADO is null) group by cd_a.SABADO, cd_a.NUMERO_EMPLEADO having COUNT(cd_a.SABADO) > 1 ) as docentes " +
            "WHERE docenteH = '" + numeroEmpleado + "'");
        return traslapeHorarioUA;
    }

    //Docentes que exceden las 40 horas de pago
    public static int ObtenerDocenteExcede40(string numeroEmpleado, string claveZP, string periodo)
    {
        int excede40 = Consultas.ConsultaInt("SELECT COUNT(empleados) as empleados FROM (select RP.NUMERO_EMPLEADO as empleados " +
            "from RESUMEN_PLAZAS as RP where CLAVE_ZP = '" + claveZP + "' GROUP BY RP.NUMERO_EMPLEADO,RP.ID_NIVEL_EST " +
            "having (SUM(HORAS) + (select ISNULL(SUM(HORAS_ASIGNADAS),0) as HrsInt from CARGA_DOCENTE as cd " +
            "where cd.PERIODO_ESCOLAR = '" + periodo + "' and  cd.NUMERO_EMPLEADO = RP.NUMERO_EMPLEADO and CLAVE_ZP = '" + claveZP + "' " +
            "and cd.INTERINATO = '1')) > 40 ) as empleadoss " +
            "where empleados = '" + numeroEmpleado + "'");
        return excede40;
    }

    //Diferencia de horarios y horas de UA
    public static int ObtenerDocenteDiferenciaHorario(string numeroEmpleado, string claveZP, string periodo)
    {
        //int diferenciaHorario = Consultas.ConsultaInt("SELECT COUNT(grupos) as Num_Grupos FROM (select distinct CONCAT(cd.CLAVE_ZP, " +
        //    "cd.ID_ASIGNATURA, cd.SECUENCIA) as grupos from CARGA_DOCENTE as cd, PLANES_ESTUDIO as pe " +
        //    "where cd.PERIODO_ESCOLAR = '" + periodo + "' and cd.CLAVE_ZP = '" + claveZP + "' and (cd.ADJUNTIA is null or cd.ADJUNTIA = '' " +
        //    "or cd.ADJUNTIA = '0') and (cd.CLAVE_ZP = pe.CLAVE_ZP AND cd.ID_ASIGNATURA = pe.ID_ASIGNATURA and cd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD)  " +
        //    "group by cd.CLAVE_ZP, cd.ID_ASIGNATURA, cd.SECUENCIA, cd.ID_ESPECIALIDAD, pe.HORAS_SEMANA, pe.DESCRIPCION " +
        //    "having (ISNULL( SUM( CAST(ROUND((ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.LUNES,1,5), SUBSTRING(cd.LUNES,7,5)),0) + " +
        //    "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MARTES,1,5), SUBSTRING(cd.MARTES,7,5)),0) +  ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MIERCOLES, 1, 5), " +
        //    "SUBSTRING(cd.MIERCOLES, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.JUEVES, 1, 5), SUBSTRING(cd.JUEVES, 7, 5)), 0) + " +
        //    "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.VIERNES, 1, 5), SUBSTRING(cd.VIERNES, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, " +
        //    "SUBSTRING(cd.SABADO, 1, 5), SUBSTRING(cd.SABADO, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.DOMINGO, 1, 5), " +
        //    "SUBSTRING(cd.DOMINGO, 7, 5)), 0)) / 60.0 , 2, 1) AS DECIMAL(20,1)) ) ,0) ) <> pe.HORAS_SEMANA) as gruposs " +
        //    "where Num_Grupos = '" + numeroEmpleado + "'");

        int diferenciaHorario = Consultas.ConsultaInt("SELECT COUNT(NUMERO_EMPLEADO) FROM ( select cd.CLAVE_ZP, cd.ID_ASIGNATURA, cd.SECUENCIA, " +
            "cd.ID_ESPECIALIDAD, cd.NUMERO_EMPLEADO, cd.HORAS_ASIGNADAS from CARGA_DOCENTE as cd where cd.PERIODO_ESCOLAR = '" + periodo + "' " +
            "and cd.CLAVE_ZP = '" + claveZP + "' and(cd.ADJUNTIA is null or cd.ADJUNTIA = '' or cd.ADJUNTIA = '0') group by cd.CLAVE_ZP, " +
            "cd.ID_ASIGNATURA, cd.SECUENCIA, cd.ID_ESPECIALIDAD, cd.HORAS_ASIGNADAS, cd.NUMERO_EMPLEADO having(ISNULL(SUM(CAST(ROUND((" +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.LUNES, 1, 5), SUBSTRING(cd.LUNES, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MARTES, 1, 5), " +
            "SUBSTRING(cd.MARTES, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.MIERCOLES, 1, 5), SUBSTRING(cd.MIERCOLES, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.JUEVES, 1, 5), SUBSTRING(cd.JUEVES, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.VIERNES, 1, 5), " +
            "SUBSTRING(cd.VIERNES, 7, 5)), 0) + ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.SABADO, 1, 5), SUBSTRING(cd.SABADO, 7, 5)), 0) + " +
            "ISNULL(DATEDIFF(MINUTE, SUBSTRING(cd.DOMINGO, 1, 5), SUBSTRING(cd.DOMINGO, 7, 5)), 0)) / 60.0, 2, 1) AS DECIMAL(20, 1))), 0)) <> " +
            "cd.HORAS_ASIGNADAS) as T1 WHERE NUMERO_EMPLEADO = '" + numeroEmpleado + "'");
        return diferenciaHorario;
    }
}