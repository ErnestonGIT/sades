<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PeticionesAdmon.aspx.cs" Inherits="PeticionesAdmon" EnableEventValidation = "false" Buffer="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="contentPeticionesAdmon" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <style>

        .footer-card{
            text-align: right;
            padding-bottom: 0px;
        }
        .footer-card a{
            color:gray;
        }
        .btn-numero-empleado{
            width: 110px;
        }
        .iconExcel{
            color:green;
        }
        .iconPDF{
            color:darkred;
        }
        .icon-green{
            color:limegreen;
        }
        .icon-orange{
            color: #ff771d;
        }
        .icon-blue{
            color: royalblue;
        }
        .icon-red{
            color:red;
        }
        .back-red{
            background: #FA2F2F;
        }
        .text-yellow{
            color: #fc0;
        }
        .text-red{
            color: #cc3300;
        }

        .highcharts-figure-analista,
        .highcharts-data-table table {
            height: 250px;
            margin: 1em auto;
        }
        .containerChart{
            min-width: 260px;
            height:260px;
            
        }

        .highcharts-figure,
        .highcharts-data-table table {
            min-width: 90%;
            max-width: 95%;
            margin: 1em auto;
        }

        .highcharts-data-table table {
            font-family: Verdana, sans-serif;
            border-collapse: collapse;
            border: 1px solid #ebebeb;
            margin: 3px auto;
            text-align: center;
            width: 90%;
            max-width: 500px;
        }

        .highcharts-data-table caption {
            padding: 1em 0;
            font-size: 1.2em;
            color: #555;
        }

        .highcharts-data-table th {
            font-weight: 600;
            padding: 0.5em;
        }

        .highcharts-data-table td,
        .highcharts-data-table th,
        .highcharts-data-table caption {
            padding: 0.5em;
        }

        .highcharts-data-table thead tr,
        .highcharts-data-table tr:nth-child(even) {
            background: #f8f8f8;
        }

        .highcharts-data-table tr:hover {
            background: #f1f7ff;
        }

        #container-comportamiento {

            margin: 1em auto;
        }

        span.highcharts-subtitle{
            text-align:center;
        }

        .highcharts-container > svg > text {
            display: none;

        }

        .altura{
            min-height: 380px;
            max-height: 380px;
        }
        .marginChart{
            margin-top: -25px;
        }

        .columnTraslapeDetalleStyle{
            text-align: center;
            width: 105px;
            min-width: 105px;
        }
        .alturaChart{
            max-height:340px
        }

        .listBusqueda {
            list-style: none;
            background-color: #FFFFFF;
            max-height: 210px;
            text-align: inherit;
            overflow-y: scroll;
            margin-left: 0px;
            border-bottom: 1px solid #B5C6D4;
            border-left: 1px solid #B5C6D4;
            margin-top: 0px;
            cursor:pointer;
        }
        /****** Switch Button *****/
        .switch
        {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }
        
        .switch input
        {
            opacity: 0;
        }
        
        .slider
        {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: lightcoral;/*#ccc*/
            -webkit-transition: .4s;
            transition: .4s;
        }
        
        .slider:before
        {
            position: absolute;
            content: "";
            height: 16px;
            width: 16px;
            left: 4px;
            bottom: 4px;
            background-color: white;
            -webkit-transition: .4s;
            transition: .4s;
        }
        
        input:checked + .slider
        {
            background-color: #A3D69B;
        }
        
        input:focus + .slider
        {
            box-shadow: 0 0 1px red;
        }
        
        input:checked + .slider:before
        {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }
        
        /* Rounded sliders */
        .slider.round
        {
            border-radius: 34px;
        }
        
        .slider.round:before
        {
            border-radius: 50%;
        }

        .sliderPE
        {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #A3D69B;/*#ccc*/
            -webkit-transition: .4s;
            transition: .4s;
        }
        
        .sliderPE:before
        {
            position: absolute;
            content: "";
            height: 16px;
            width: 16px;
            left: 4px;
            bottom: 4px;
            background-color: white;
            -webkit-transition: .4s;
            transition: .4s;
        }
        
        input:checked + .sliderPE
        {
            background-color: #A3D69B;
        }
        
        input:focus + .sliderPE
        {
            box-shadow: 0 0 1px red;
        }
        
        input:checked + .sliderPE:before
        {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }
        
        .floatOne{
	        position:fixed;
	        width:40px;
	        height:40px;
	        top:102px;
	        right:0px;
	        background-color:#5A1236;
	        color:#FFF;
            border-bottom-left-radius:20px;
            border-top-left-radius:20px;
	        text-align:center;
	        box-shadow: 2px 2px 3px #999;
            z-index: 99998;
        }


        .floatIcon{
	        margin-top:12px;
        }

        .subt_card{
            color:#6c1d45;
        }
        .card-body-thin{
            padding-bottom: unset;
        }
        .subt_card span {
            color: #6c1d45;
            font-size: 14px;
            font-weight: 600;
        }
        .info-text{
            color: orange;
        }
        .custom-tooltip {
            --bs-tooltip-bg: var(--bs-secondary);
        }
        .custom-popover {
            --bs-popover-max-width: 200px;
            --bs-popover-border-color: var(--bs-secondary);
            --bs-popover-header-bg: var(--bs-secondary);
            --bs-popover-header-color: var(--bs-white);
            --bs-popover-body-padding-x: 1rem;
            --bs-popover-body-padding-y: .5rem;
        }
        .card-title-offCanvas {
            font-size: 20px;
            font-weight: 500;
            color: #012970;
            font-family: "Poppins", sans-serif;
        }
        .card-title-offCanvas span {
            color: #899bbd;
            font-size: 14px;
            font-weight: 400; 
        }
        .accordion-button:not(.collapsed),.accordion-button- {
            color: #FFF  !important;
            background-color: #32587a !important;
        }
        .accordion-button:hover {
            background-color: gray !important;
        }

        .dangerColor{

            --bs-btn-color: lightcoral;
            --bs-btn-border-color: lightcoral;
            --bs-btn-hover-color: #fff;
            --bs-btn-hover-bg: lightcoral;
            --bs-btn-hover-border-color: lightcoral;
            --bs-btn-focus-shadow-rgb: 220,53,69;
            --bs-btn-active-color: #fff;
            --bs-btn-active-bg: lightcoral;
            --bs-btn-active-border-color: lightcoral;
            --bs-btn-active-shadow: inset 0 3px 5px rgba(0, 0, 0, 0.125);
            --bs-btn-disabled-color: lightcoral;
            --bs-btn-disabled-bg: transparent;
            --bs-btn-disabled-border-color: lightcoral;
            --bs-gradient: none;
        }
        .titleSection{
            color: #5a1236;
            font-weight: bold;
        }

        .text-truncate {
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
        }
        .columnDiaSemanaStyle{
            text-align: center;
            max-width:105px;
        }        
    </style>
    
    <link rel="stylesheet" href="public/css/select2.min.css" />
    <link rel="stylesheet" href="public/css/select2-bootstrap-5-theme.min.css" />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <main id="main1" class="main">
                <div class="pagetitle">
                    <h1>Tablero </h1>
                    <nav>
                        <ol class="breadcrumb">
                            <!-- <li class="breadcrumb-item"><a href="index">Inicio</a></li> -->
                            <li class="breadcrumb-item"><a href="Dashboard.aspx">Inicio</a></li>
                            <li class="breadcrumb-item active">Garantias</li>
                            <li class="breadcrumb-item active"><asp:Label ID="LabelBreadCrumbZP_name" runat="server" CssClass="breadcrumb"></asp:Label></li>
                        </ol>
                    </nav>
                </div><!-- End Page Title -->

                <asp:Label ID="LabelPerfil" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelZP" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelPE" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelZPDesc" runat="server" Text="" style="display:none;"></asp:Label>

                <asp:HiddenField runat="server" ID="HiddenFieldPerfil_nivel"/>

                <asp:HiddenField runat="server" ID="HiddenFieldCollapsePlan_selected" Value="0"/>


                <section class="section dashboard">

                    <!-- PanelGarantias -->
                    <div id="divPanelGarantias" runat="server" visible="false">
                        <div class="accordion" id="divAccordionGarantias">

                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseContenido_filtroGarantias" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                                Resúmen de las unidades académicas
                              </button>
                            </h2>
                            <div id="collapseContenido_filtroGarantias" class="accordion-collapse collapse">
                              <div class="accordion-body">

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        


                                    </ContentTemplate>
                                </asp:UpdatePanel>                                      
                                
                              </div>
                            </div>
                          </div>

                        </div>

                        <br />
                    </div>

                </section>

            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script src="public/js/jquery/jquery-3.2.1.js"></script>
    <script src="public/js/jquery/select2.min.js"></script>
    <script src="public/js/jquery/select2es.js"></script>

    <script src="public/js/highcharts/highcharts.js"></script>
    <script src="public/js/highcharts/heatmap.js"></script>
    <script src="public/js/highcharts/drilldown.js"></script>
    <script src="public/js/highcharts/highcharts-more.js"></script>
    <script src="public/js/highcharts/solid-gauge.js"></script>
    <script src="public/js/highcharts/data.js"></script>
    <script src="public/js/highcharts/exporting.js"></script>
    <script src="public/js/highcharts/export-data.js"></script>
    <script src="public/js/highcharts/accessibility.js"></script>

    <script src="public/js/loadingOverlay/loadingoverlay.min.js"></script>
    <script src="public/js/sweetalert/sweetalert2.11.js"></script>

    
    <script>


        //There's a bug in Microsoft's Ajax script that stops the modal popups from working
        //This overrides the the code that causes the error
        Sys.UI.Point = function Sys$UI$Point(x, y) {

            x = Math.round(x);
            y = Math.round(y);

            var e = Function._validateParams(arguments, [
                { name: "x", type: Number, integer: true },
                { name: "y", type: Number, integer: true }
            ]);
            if (e) throw e;
            this.x = x;
            this.y = y;
        }

        $(document).ready(function () {

            LoadInitialFunctions();

            var prm = Sys.WebForms.PageRequestManager.getInstance();

            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    $(function () {
                        LoadInitialFunctions();
                    });
                });
            }

        })

        function IsPostBack() {
            var IsPostBack = Sys.WebForms.PageRequestManager.getInstance();
            return IsPostBack;
        }

        function LoadInitialFunctions() {

            verificarDatos();
            validarCollapse();
            infoToolStart();
            enableLoadingOverlay();
            hideLoadingOverlay();
            habilitarSelect2();

        }

        function verificarDatos() {

            var zp = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;
            var pe = document.getElementById('<%= LabelPE.ClientID %>').innerHTML;
            var ZPname = document.getElementById('<%= LabelZPDesc.ClientID %>').innerHTML;
            var perfil = document.getElementById('<% = LabelPerfil.ClientID%>').innerHTML.toString();

            console.log("\nzp: "+zp+"\npe: "+pe);
        }

        function hideLoadingOverlay() {
                LoadingOverlay("hide");
        }

        function enableLoadingOverlay() {
            $('[class*=LoadingOverlay]').click(function () {
                LoadingOverlay("show");
            });

            $('[class*=form-select]').change(function () {
                LoadingOverlay("show");
            });

            $('[class$=ddl]').change(function () {
                LoadingOverlay("show");
            });

        }

        function habilitarSelect2() {

            $.fn.select2.defaults.set('language', 'es');
            $("[class*=form-select]").select2({
                theme: 'bootstrap-5'
            });

            $("[id*=DropDownListUnidadAcademica_resumen]").select2({
                theme: 'bootstrap-5',
                dropdownParent: $('#ModalDetalleUnidadAdministrativa .modal-body')
            });

            $("[id*=DropDownListUnidadAdministrativa_resumen]").select2({
                theme: 'bootstrap-5',
                dropdownParent: $('#ModalDetalleUnidadAdministrativa .modal-body')
            });

        }

        function ShowOffCanvas(idOffCanvas) {
            const elementOffcanvas = document.getElementById(idOffCanvas)

            var bsOffcanvas = new bootstrap.Offcanvas(elementOffcanvas)
            bsOffcanvas.show()
        }

        function LoadingOverlay(action) {
            $.LoadingOverlay(action);
        }

        function ShowModal(idModal) {

            var LabelZP = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;
            var LabelPE = document.getElementById('<%= LabelPE.ClientID %>').innerHTML;

            console.log("hiddenUA: " + LabelUA);

            var myModal = document.getElementById(idModal);
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();

            switch (idModal) {
                case "ModalMapaCalorDetalle":
                    chartMapaCalor(LabelZP, LabelPE, 'container-mapa-calor');
                    break;
                case "ModalMapaCalorDetalleUA":
                    chartMapaCalorUA(LabelZP, LabelPE, LabelUA, 'container-mapa-calor-ua');
                    break;
            }
        }

        function onShowModal() {
            $(myModal).on('shown.bs.modal', function () {
            });
        }

        function onHideModal() {
            $(myModal).on('hidden.bs.modal', function () {

            });
        }

        function highChartOptions() {

            var lang_es = {
                months: [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Septiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
                weekdays: [
                    "Domingo",
                    "Lunes",
                    "Martes",
                    "Miércoles",
                    "Jueves",
                    "Viernes",
                    "Sábado"
                ],
                downloadJPEG: "Descargar JPEG",
                downloadPDF: "Descargar PDF",
                downloadPNG: "Descargar PNG",
                downloadXLS: "Descargar XLS",
                downloadCSV: "Descargar CSV",
                downloadSVG: "Descargar SVG",
                printChart: "Imprimir gráfico",
                resetZoom: "Resetear zoom",
                resetZoomTitle: "Resetear zoom",
                viewData: "Ver datos",
                hideData: "Ocultar datos",
                getWeekDays: function () {
                    return this.weekdays;
                },
                getMonths: function () {
                    return this.months;
                },
                getShortWeekDays: function () {
                    return this.weekdays.map(function (day) {
                        return day.substring(0, 3);
                    });
                },
                getShortMonths: function () {
                    return this.months.map(function (month) {
                        return month.substring(0, 3);
                    });
                }
            }

            Highcharts.setOptions({
                lang: {
                    months: lang_es.getMonths(),
                    weekdays: lang_es.getWeekDays(),
                    shortWeekdays: lang_es.getShortWeekDays(),
                    shortMonths: lang_es.getShortMonths(),
                    downloadJPEG: lang_es.downloadJPEG,
                    downloadPDF: lang_es.downloadPDF,
                    downloadPNG: lang_es.downloadPNG,
                    downloadSVG: lang_es.downloadSVG,
                    downloadXLS: lang_es.downloadXLS,
                    downloadCSV: lang_es.downloadCSV,
                    printChart: lang_es.printChart,
                    viewData: lang_es.viewData,
                    hideData: lang_es.hideData,
                    resetZoom: lang_es.resetZoom,
                    resetZoomTitle: lang_es.resetZoomTitle,
                }
                , exporting: {
                    buttons: {
                        contextButton: {
                            menuItems: [
                                'printChart',
                                'separator',
                                'downloadPNG',
                                'downloadJPEG',
                                'downloadPDF',
                                'downloadSVG',
                                'separator',
                                'downloadXLS',
                                'downloadCSV',
                                'viewData'
                            ]
                        }
                    }
                }
            });

        }

        function destroyHighChart(idContainer) {

            $("#" + idContainer).empty();

        }

        function validarCollapse() {

            $('.accordion-collapse').on('hide.bs.collapse', function (e) {

                let idCollapse = $(e.target).attr("ID");

                switch (idCollapse) {
                    case 'collapseContenido_filtroPlan':
                        $("[id*=HiddenFieldCollapsePlan_selected]").val("0");
                        break;
                    case 'collapseContenido_filtroEventos':
                        $("[id*=HiddenFieldCollapseEventos_selected]").val("0");
                        break;
                    case 'collapseContenido_filtroGarantias':
                        $("[id*=HiddenFieldCollapseGarantias_selected]").val("0");
                        break;
                }

            });

            $('.accordion-collapse').on('show.bs.collapse', function (e) {

                let idCollapse = $(e.target).attr("ID");

                switch (idCollapse) {
                    case 'collapseContenido_filtroPlan':
                        $("[id*=HiddenFieldCollapsePlan_selected]").val("1");
                        break;
                    case 'collapseContenido_filtroEventos':
                        $("[id*=HiddenFieldCollapseEventos_selected]").val("1");
                        break;
                    case 'collapseContenido_filtroGarantias':
                        $("[id*=HiddenFieldCollapseGarantias_selected]").val("1");
                        break;
                }
            });

            let collPlaSt = $("[id*=HiddenFieldCollapsePlan_selected]").val();
            let collEveSt = $("[id*=HiddenFieldCollapseEventos_selected]").val();
            let collDepSt = $("[id*=HiddenFieldCollapseGarantias_selected]").val();

            if (collPlaSt == "1") { ActivarCollapse("collapseContenido_filtroPlan"); }
            if (collEveSt == "1") { ActivarCollapse("collapseContenido_filtroEventos"); }
            if (collDepSt == "1") { ActivarCollapse("collapseContenido_filtroGarantias"); }

        }

        function ActivarCollapse(idCollapse) {

            var element = document.getElementById(idCollapse);
            var myCollapse = new bootstrap.Collapse(element);
            myCollapse.show();

        }

    </script>

</asp:Content>


