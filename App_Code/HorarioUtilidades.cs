using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de HorarioUtilidades
/// </summary>
public class HorarioUtilidades
{
    public HorarioUtilidades()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static TimeSpan CalcularHorasTrabajadas(TimeSpan? entrada, TimeSpan? salida)
    {
        if (entrada.HasValue && salida.HasValue && entrada.Value < salida.Value)
        {
            return salida.Value - entrada.Value;
        }
        else
        {
            return TimeSpan.Zero;
        }
    }

    // manejo de nulos en horas
    ////private static TimeSpan? ObtenerHora(string horaString)
    ////{
    ////    if (string.IsNullOrEmpty(horaString) || horaString.ToUpper() == "NULL")
    ////        return null;

    ////    TimeSpan hora;
    ////    if (TimeSpan.TryParse(horaString, out hora))
    ////        return hora;

    ////    return null;
    ////}

    // Calcular horas registradas por dia
    ////public static TimeSpan CalcularHorasRegistradas(DataRow[] filasHorario)
    ////{
    ////    TimeSpan horasTotales = TimeSpan.Zero;

    ////    foreach (DataRow fila in filasHorario)
    ////    {
    ////        TimeSpan? entrada = ObtenerHora(fila["entrada"].ToString());
    ////        TimeSpan? salida = ObtenerHora(fila["salida"].ToString());
    ////        horasTotales += CalcularHorasTrabajadas(entrada, salida);
    ////    }

    ////    return horasTotales;
    ////}

    //
    //public static TimeSpan horasRegistradas(string txtEntradaLu, string txtFinLu, string txtEntradaMa, string txtFinMa,
    //    string txtEntradaMi, string txtFinMi, string txtEntradaJu, string txtFinJu, string txtEntradaVi, string txtFinVi, string txtEntradaSa, string txtFinSa)
    //{
    //    TimeSpan horasTotales = TimeSpan.Zero;

    //    // ... código para calcular las horas totales ...            

    //    TimeSpan? lunesEntrada = string.IsNullOrEmpty(txtEntradaLu) ? (TimeSpan?)null : TimeSpan.Parse(txtEntradaLu);
    //    TimeSpan? lunesSalida = string.IsNullOrEmpty(txtFinLu) ? (TimeSpan?)null : TimeSpan.Parse(txtFinLu);
    //    horasTotales += CalcularHorasTrabajadas(lunesEntrada, lunesSalida);

    //    TimeSpan? martesEntrada = string.IsNullOrEmpty(txtEntradaMa) ? (TimeSpan?)null : TimeSpan.Parse(txtEntradaMa);
    //    TimeSpan? martesSalida = string.IsNullOrEmpty(txtFinMa) ? (TimeSpan?)null : TimeSpan.Parse(txtFinMa);
    //    horasTotales += CalcularHorasTrabajadas(martesEntrada, martesSalida);

    //    TimeSpan? miercolesEntrada = string.IsNullOrEmpty(txtEntradaMi) ? (TimeSpan?)null : TimeSpan.Parse(txtEntradaMi);
    //    TimeSpan? miercolesSalida = string.IsNullOrEmpty(txtFinMi) ? (TimeSpan?)null : TimeSpan.Parse(txtFinMi);
    //    horasTotales += CalcularHorasTrabajadas(miercolesEntrada, miercolesSalida);

    //    TimeSpan? juevesEntrada = string.IsNullOrEmpty(txtEntradaJu) ? (TimeSpan?)null : TimeSpan.Parse(txtEntradaJu);
    //    TimeSpan? juevesSalida = string.IsNullOrEmpty(txtFinJu) ? (TimeSpan?)null : TimeSpan.Parse(txtFinJu);
    //    horasTotales += CalcularHorasTrabajadas(juevesEntrada, juevesSalida);

    //    TimeSpan? viernesEntrada = string.IsNullOrEmpty(txtEntradaVi) ? (TimeSpan?)null : TimeSpan.Parse(txtEntradaVi);
    //    TimeSpan? viernesSalida = string.IsNullOrEmpty(txtFinVi) ? (TimeSpan?)null : TimeSpan.Parse(txtFinVi);
    //    horasTotales += CalcularHorasTrabajadas(viernesEntrada, viernesSalida);

    //    TimeSpan? sabadoEntrada = string.IsNullOrEmpty(txtEntradaSa) ? (TimeSpan?)null : TimeSpan.Parse(txtEntradaSa);
    //    TimeSpan? sabadoSalida = string.IsNullOrEmpty(txtFinSa) ? (TimeSpan?)null : TimeSpan.Parse(txtFinSa);
    //    horasTotales += CalcularHorasTrabajadas(sabadoEntrada, sabadoSalida);

    //    return horasTotales;
    //}

    //public static TimeSpan horasRegistradas(params string[] horarios)
    //{
    //    TimeSpan horasTotales = TimeSpan.Zero;

    //    string[] diasSemana = { "Lu", "Ma", "Mi", "Ju", "Vi", "Sa" };

    //    foreach (string dia in diasSemana)
    //    {
    //        string entradaText = horarios.FirstOrDefault(x => x.StartsWith("txtEntrada" + dia)) ?? "";
    //        string salidaText = horarios.FirstOrDefault(x => x.StartsWith("txtFin" + dia)) ?? "";

    //        TimeSpan? entrada = string.IsNullOrEmpty(entradaText) ? (TimeSpan?)null : TimeSpan.Parse(entradaText);
    //        TimeSpan? salida = string.IsNullOrEmpty(salidaText) ? (TimeSpan?)null : TimeSpan.Parse(salidaText);

    //        horasTotales += CalcularHorasTrabajadas(entrada, salida);
    //    }

    //    return horasTotales;
    //}

    public static TimeSpan horasRegistradas(params string[] horarios)
    {
        TimeSpan horasTotales = TimeSpan.Zero;

        for (int i = 0; i < horarios.Length; i += 2)
        {
            TimeSpan? entrada = string.IsNullOrEmpty(horarios[i]) ? (TimeSpan?)null : TimeSpan.Parse(horarios[i]);
            TimeSpan? salida = string.IsNullOrEmpty(horarios[i + 1]) ? (TimeSpan?)null : TimeSpan.Parse(horarios[i + 1]);

            horasTotales += CalcularHorasTrabajadas(entrada, salida);
        }

        return horasTotales;
    }


    public static TimeSpan ObtenerHorasDocente(string idUser, string claveZP)
    {
        // Implementa tu lógica aquí
        string horasTotalString = Consultas.ConsultaS("SELECT SUM(HORAS) AS HORASTOTAL FROM CAPITAL_HUMANO_PLAZAS ch, USERS u " +
        "WHERE ch.NUMERO_EMPLEADO = u.NUMERO_EMPLEADO AND u.ID_USER = '" + idUser + "' AND ZONA_PAGADORA = '" + claveZP + "'");

        int horasTotal;
        if (int.TryParse(horasTotalString, out horasTotal))
        {
            TimeSpan horasTotalTimeSpan = TimeSpan.FromHours(horasTotal);
            return horasTotalTimeSpan;
        }

        return TimeSpan.Zero;
    }

    private static TimeSpan ObtenerHoraInicio(string horario)
    {
        // Crear una nueva variable para manipular la cadena sin afectar la variable de iteración original
        string horarioIni = horario;
        // Eliminar comillas alrededor del horario
        horarioIni = horarioIni.Trim('\'');

        string[] partes = horarioIni.Split('-');

        if (partes.Length == 2)
        {
            return TimeSpan.Parse(partes[0]); //REVISAR
        }
        return TimeSpan.Zero;
    }

    private static TimeSpan ObtenerHoraFin(string horario)
    {
        // Crear una nueva variable para manipular la cadena sin afectar la variable de iteración original
        string horarioFin = horario;
        // Eliminar comillas alrededor del horario
        horarioFin = horarioFin.Trim('\'');

        string[] partes = horarioFin.Split('-');
        if (partes.Length == 2)
        {
            return TimeSpan.Parse(partes[1]);
        }
        return TimeSpan.Zero;
    }

    //Verificar si hay traslapes y si el horario tiene una hora de diferencia
    public static bool VerificarTraslapeYDiferencia(DataTable horarioOtraClaveZP, params string[] nuevosHorarios)
    {
        foreach (DataRow row in horarioOtraClaveZP.Rows)
        {
            for (int i = 0; i < nuevosHorarios.Length; i++)
            {
                if (!string.IsNullOrEmpty(nuevosHorarios[i]) && nuevosHorarios[i] != "NULL" && row[i + 1].ToString() != "NULL")
                {
                    TimeSpan nuevoHorarioInicio = ObtenerHoraInicio(nuevosHorarios[i]);
                    TimeSpan nuevoHorarioFin = ObtenerHoraFin(nuevosHorarios[i]);
                    TimeSpan horarioOtraClaveZPInicio = ObtenerHoraInicio(row[i + 1].ToString());
                    TimeSpan horarioOtraClaveZPFin = ObtenerHoraFin(row[i + 1].ToString());

                    // Verificar traslape
                    if (nuevoHorarioInicio < horarioOtraClaveZPFin && nuevoHorarioFin > horarioOtraClaveZPInicio)
                    {
                        return true; // Hay traslape
                    }

                    // Verificar diferencia de horas (1 hr)
                    if ((nuevoHorarioInicio - horarioOtraClaveZPFin).Duration() < TimeSpan.FromHours(1) ||
                        (horarioOtraClaveZPInicio - nuevoHorarioFin).Duration() < TimeSpan.FromHours(1))
                    {
                        return true; // No hay al menos una hora de diferencia
                    }
                }
            }
        }
        return false; // No hay traslape ni falta de diferencia de horas
    }

    // Función para validar el horario (rango) 
    public static bool ValidarHorario(string horaInicio, string horaFin, string[] horariosInicio, string[] horariosFin)
    {
        TimeSpan inicio = TimeSpan.Parse(horaInicio);
        TimeSpan fin = TimeSpan.Parse(horaFin);

        // Validar los horarios proporcionados
        for (int i = 0; i < horariosInicio.Length; i++)
        {
            if (!EstaEnRango(horariosInicio[i], inicio, fin) || !EstaEnRango(horariosFin[i], inicio, fin))
            {
                return false;
            }
        }

        return true;
    }

    // Función auxiliar para verificar si una hora está en un rango
    private static bool EstaEnRango(string hora, TimeSpan inicio, TimeSpan fin)
    {
        if (!string.IsNullOrEmpty(hora))
        {
            TimeSpan horaSeleccionada = TimeSpan.Parse(hora);
            return horaSeleccionada >= inicio && horaSeleccionada <= fin;
        }
        return true;  // Si no hay hora, no hay violación del rango
    }

    public static bool ExcedeHorasPorDia(params string[] horarios)
    {
        foreach (var horarioOriginal in horarios)
        {
            // Crear una nueva variable para manipular la cadena sin afectar la variable de iteración original
            string horario = horarioOriginal;

            if (!string.IsNullOrEmpty(horario) && horario.ToUpper() != "NULL")
            {
                // Eliminar comillas alrededor del horario
                horario = horario.Trim('\'');

                // Dividir la cadena del horario en las partes de inicio y fin
                string[] partesHorario = horario.Split('-');

                if (partesHorario.Length == 2)
                {
                    // Intentar convertir las partes en TimeSpan manualmente
                    TimeSpan inicio, fin;

                    if (TryParseHorario(partesHorario[0], out inicio) && TryParseHorario(partesHorario[1], out fin))
                    {
                        // Calcular la duración
                        TimeSpan duracion = fin - inicio;

                        if (duracion.TotalHours > 8)
                        {
                            Debug.WriteLine("Excede las 8 horas: " + duracion.TotalHours + " horas");
                            return true; // Excede las 8 horas
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Conversión fallida para horario: " + horario);
                        // La cadena no es válida para TimeSpan

                        // Imprimir partes individuales para depurar
                        Debug.WriteLine("Parte 1: " + partesHorario[0]);
                        Debug.WriteLine("Parte 2: " + partesHorario[1]);

                        return true;
                    }
                }
                else
                {
                    Debug.WriteLine("Formato de horario no válido: " + horario);
                    // La cadena no tiene el formato esperado
                    return true;
                }
            }
        }
        return false; // No excede las 8 horas en ningún día
    }

    private static bool TryParseHorario(string input, out TimeSpan resultado)
    {
        resultado = TimeSpan.Zero;
        string[] partes = input.Split(':');

        if (partes.Length == 2)
        {
            int horas, minutos;

            if (int.TryParse(partes[0], out horas) && int.TryParse(partes[1], out minutos))
            {
                resultado = new TimeSpan(horas, minutos, 0);
                return true;
            }
        }

        return false;
    }

    //para conteo  de horas
    public static TimeSpan ObtenerDuracionHorarioTxtB(string horaInicio, string horaFin)
    {
        if (!string.IsNullOrEmpty(horaInicio) && !string.IsNullOrEmpty(horaFin))
        {
            TimeSpan inicio = TimeSpan.Parse(horaInicio);
            TimeSpan fin = TimeSpan.Parse(horaFin);
            return fin - inicio;
        }
        return TimeSpan.Zero;
    }
}