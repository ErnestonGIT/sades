using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de EstadosSAIEE
/// </summary>
public class EstadosSAIEE
{
    public EstadosSAIEE()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static class Estatus
    {
        // PERIODO ESCOLAR NO HABILITADO
        public const int PeriodoNoHabilitado = 0;
        // HABILITADO PARA LA UNIDAD ACADÉMICA DE INICIAR CON LA PLANEACIÓN DE LA ESTRUCTURA EDUCATIVA
        public const int HabilitadoParaPlaneacionEE = 1;
        // EN PROCESO DE REVISIÓN POR PARTE DE LA COORDINACIÓN DE ESTRUCTURA EDUCATIVA
        public const int ProcesoRevisionCEE = 2;
        // HABILITADO PARA MODIFICAR SOLO POR LA COORDINACIÓN DE ESTRUCTURA EDUCATIVA
        public const int HabilitadoModificarCEE = 3;
        // CONCLUSIÓN DEL REGISTRO DE LA ESTRUCTURA EDUCATIVA
        public const int ConclusionRegistroEE = 4;
        // REGISTRO DE ACTIVIDADES COMPLEMENTARIAS
        public const int RegistroActividadesComplementarias = 5;
        // EMISIÓN DE LA RUAA
        public const int EmisionRUAA = 6;
        //PROCESO DE REGISTRO DE LA ESTRUCTURA EDUCATIVA CONCLUIDO
        public const int ProcesoConcluidoEE = 7;
    }

    public static class EstatusEstructura
    {
        // PERIODO ESCOLAR NO HABILITADO
        public const int PeriodoNoHabilitado = 0;
        // HABILITADO A LA UNIDAD ACADEMICA PARA INICIAR CON LA PLANEACION DE SU ESTRUCTURA EDUCATIVA
        public const int HabilitadoParaPlaneacionEE = 1;
        // EN PROCESO DE REVISION POR PARTE DE LA COORDINACION DE ESTRUCTURA EDUCATIVA
        public const int ProcesoRevisionCEE = 2;
        // VALIDADO POR LA COORDINACION DE ESCTRUCTURA EDUCATIVA
        public const int ValidadoCEE = 3;
        // AJUSTES EN LA OFERTA POR CEE
        public const int AjusteOfertaCEE = 4;
        // HABILITADO PARA LA IMPRESION DE RUAA
        public const int EmisionRUAA = 5;
    }

    public static class MovicoAlta
    {
        // ALTA DEFINITIVA
        public const int AltaDefinitiva = 10;
        // ALTA COMO SERVIDOR PÚBLICO DE CARRERA TITULAR
        public const int AltaServidorPublico = 11;
        // ALTA POR MOVIMIENTO LATERAL
        public const int MovimientoLateral = 12;
        // ALTA INTERINA LIMITADA
        public const int AltaInternaLimit = 20;
        // NOMBRAMIENTO TEMPORAL BAJO EL AMPARO DEL ARTÍCULO
        public const int NombramientoTempAmparo = 23;
        // ALTA PROVISIONAL
        public const int AltaProvisional = 95;
        // ALTA DE CONFIANZA
        public const int AltaConfianza = 96;
    }
}