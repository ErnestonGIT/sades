using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DatosUser
/// </summary>
public class DatosUser
{
    public DatosUser()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static string ObtenerNumeroEmpleado(string idUser)
    {
        string numEmpleado = Consultas.ConsultaS("select NUMERO_EMPLEADO from USERS where ID_USER = '" + idUser + "'");
        return numEmpleado;
    }

    public static string ObtenerNombreUser(string claveZP, string idUser)
    {
        string nombre = Consultas.ConsultaS("SELECT DISTINCT CONCAT(u.NOMBRE, ' ', u.APELLIDO_PAT, ' ', u.APELLIDO_MAT) AS NOMBRE FROM USERS u " +
            "INNER JOIN CAPITAL_HUMANO_PLAZAS ch ON u.NUMERO_EMPLEADO = ch.NUMERO_EMPLEADO WHERE ch.ZONA_PAGADORA = '" + claveZP + "' AND u.ID_USER = '" + idUser + "'");
        return nombre;
    }

    public static int ObtenerIdMotivoAlta(string idUser)
    {
        int idMotivo = Consultas.ConsultaInt("SELECT ID_MOTIVO_ALTA FROM CAPITAL_HUMANO_PLAZAS ch, USERS u WHERE ch.NUMERO_EMPLEADO = u.NUMERO_EMPLEADO AND u.ID_USER = '" + idUser + "'");
        return idMotivo;
    }

    public static string ObtenerNombreUserSinZP(string idUser)
    {
        string nombre = Consultas.ConsultaS("SELECT DISTINCT CONCAT(u.NOMBRE, ' ', u.APELLIDO_PAT, ' ', u.APELLIDO_MAT) AS NOMBRE FROM USERS u WHERE u.ID_USER = '" + idUser + "'");
        return nombre;
    }

    public static int ObtenerNivelEstPerfil(string perfil)
    {
        int nivelEst = 0;
        // Consultar el nivel dei user(perfil)
        string consulta = "SELECT DISTINCT ID_NIVEL_EST FROM CAT_PERFILES WHERE ID_PERFIL = @idPerfil";

        nivelEst = Consultas.ConsultaIntP(consulta, new SqlParameter("@idPerfil", perfil));
        return nivelEst;
    }
}