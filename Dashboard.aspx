<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" EnableEventValidation = "false" Buffer="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="contentDashboard" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
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
                            <li class="breadcrumb-item active">Tablero</li>
                            <li class="breadcrumb-item active"><asp:Label ID="LabelTableroZP_name" runat="server" CssClass="breadcrumb"></asp:Label></li>
                        </ol>
                    </nav>
                </div><!-- End Page Title -->

                <asp:Label ID="LabelPerfil" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelZP" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelPE" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelZPDesc" runat="server" Text="" style="display:none;"></asp:Label>

                <asp:HiddenField runat="server" ID="HiddenFieldPerfil_nivel"/>

                <asp:HiddenField runat="server" ID="HiddenFieldCollapsePlan_selected" Value="0"/>
                <asp:HiddenField runat="server" ID="HiddenFieldCollapseEventos_selected" Value="0"/>
                <asp:HiddenField runat="server" ID="HiddenFieldCollapseDependencias_selected" Value="0"/>

                <asp:HiddenField runat="server" ID="HiddenFieldGruposTotales_mapaGeneral" Value="0"/>
                <asp:HiddenField runat="server" ID="HiddenFieldFiltro_ua"/>
                <asp:HiddenField runat="server" ID="HiddenFieldUnidadesAcademicasNS_idPerfil"/>
                
                <asp:HiddenField runat="server" ID="HiddenFieldNombramientosEstatus_edicion"/>

                <asp:Label id="LabeltotalLun" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalMar" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalMie" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalJue" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalVie" runat="server" style="display:none;"></asp:Label>

                <section class="section dashboard">

                    <!-- PanelDependencias -->
                    <div id="divPanelDependencias" runat="server" visible="false">
                        <div class="accordion" id="divAccordionDependencias">

                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseContenido_filtroDependencias" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                                Resúmen de las unidades académicas
                              </button>
                            </h2>
                            <div id="collapseContenido_filtroDependencias" class="accordion-collapse collapse">
                              <div class="accordion-body">

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        
                                        <div runat="server" id="DivDetalleDependencias_resumen" visible="true">
                                            <div class="card info-card revenue-card">
                                                <div class="filter">
                                                    <a class="icon show" href="#" data-bs-toggle="dropdown" aria-expanded="true"><i class="bi bi-three-dots"></i></a>
                                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow" style="position: absolute; inset: 0px 0px auto auto; margin: 0px; transform: translate(0px, 30px);" data-popper-placement="bottom-end">
                                                    <li class="dropdown-header text-start">
                                                        <h6>Resúmen</h6>
                                                    </li>

                                                    <li>
                                                        <asp:LinkButton runat="server" ID="LinkButton2" CssClass="dropdown-item LoadingOverlay" Text="Nombramientos" OnClick="LinkButtonUnidadesAcademicasNS_resumen_nombramientos_Click"></asp:LinkButton>
                                                    </li>
                                                    </ul>
                                                </div>
                                                <div class="card-body">
                                                    <h5 class="card-title">
                                                        Resúmen de nombramientos
                                                    </h5>
                                                    <div class="row col-xl-12 mt-3">
                                                        <div class="col-xl-4">
                                                        <div class="card info-card">
                                                            <div class="card-body">
                                                                <figure class="highcharts-figure">
                                                                    <div id="container-pie-nombramientos" style="max-height: 220px;"></div>
                                                                </figure>
                                                            </div>                                                            
                                                        </div>
                                                        </div>
                                                        <div class="col-xl-8">
                                                            <div class="card info-card">
                                                                <div class="card-body">
                                                                    <figure class="highcharts-figure">
                                                                        <div id="container-bar-nombramientos" style="max-height: 220px;"></div>
                                                                    </figure>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="row col-xl-12 mt-3" runat="server" id="DivDetalleDependencias_seccion" visible="false">
                                            <div class="col-xl-6">
                                                <div runat="server" id="divAlertUnidadAcademicaResumen_seleccionada" visible="false">

                                                    <div class="alert alert-warning alert-dismissible fade show" role="alert">
                                                      Unidad académica seleccionada: <strong><asp:Label runat="server" ID="LabelUnidadAcademicaResumenSeleccionada_nombre"></asp:Label></strong>
                                                        <asp:LinkButton runat="server" ID="LinkButtonAlertUnidadAcademicaResumen_cerrar" CssClass="btn-close LoadingOverlay" OnClick="LinkButtonAlertUnidadAcademica_resumen_cerrar_Click"
                                                            aria-label="Close"
                                                            data-bs-toggle="popover" data-bs-placement="right"
                                                            data-bs-custom-class="custom-popover"
                                                            data-bs-trigger="hover focus"
                                                            data-bs-title="Cancelar filtro"
                                                            data-bs-content="Elimina el filtro por unidad académica, mostrando la información general."><!-- -->

                                                        </asp:LinkButton>

                                                    </div>

                                                </div>
                                                <section class="section profile">

                                                    <!-- UnidadesAcademicasNS_resumen -->
                                                    <div class="card info-card revenue-card">
                                                        <div class="filter" style="display:none">
                                                            <a class="icon show" href="#" data-bs-toggle="dropdown" aria-expanded="true"><i class="bi bi-three-dots"></i></a>
                                                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow" style="position: absolute; inset: 0px 0px auto auto; margin: 0px; transform: translate(0px, 30px);" data-popper-placement="bottom-end">
                                                            <li class="dropdown-header text-start">
                                                                <h6>Resúmen</h6>
                                                            </li>

                                                            <li>
                                                                
                                                            </li>
                                                            </ul>
                                                        </div>
                                                        <div class="card-body">
                                                            <h5 class="card-title">
                                                                Unidades académicas
                                                                <i class="bi bi-info-circle info-text"
                                                                    data-bs-toggle="popover" data-bs-placement="right"
                                                                    data-bs-custom-class="custom-popover"
                                                                    data-bs-trigger="hover focus"
                                                                    data-bs-title="Unidades académicas"
                                                                    data-bs-content="Muestra la relación de unidades académicas de nivel superior.">
                                                                </i> 
                                                            </h5>
                                                            <table>
                                                                <thead>
                                                                    <tr>
                                                                    <th style="width:20%"></th>
                                                                    <th style="text-align:center;padding-inline: 15px;width:20%">Unidad Académica</th>
                                                                        <th style="text-align:center;padding-inline: 15px;width:20%">Nombramientos vencidos</th>
                                                                    <th style="text-align:center;padding-inline: 15px;width:30%">Datos</th>
                                                                    </tr>
                                                                </thead>
                                                            </table>

                                                            <div style="overflow-y: scroll; max-height: 320px; width: 100%;">
                                                                <asp:GridView ID="GridViewUnidadesAcademicasNS_resumen" runat="server"
                                                                    AutoGenerateColumns="False" ShowHeader="false"
                                                                        CssClass="table table-borderless"
                                                                            PagerStyle-CssClass="pagination-ys"
                                                                                PageSize="100" AllowPaging="false" 
                                                                                    OnPageIndexChanging="GridViewUnidadesAcademicasNS_resumen_PageIndexChanging" 
                                                                                        OnRowDataBound="GridViewUnidadesAcademicasNS_resumen_RowDataBound">
                                            
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CLAVE_ZP" HeaderText="CLAVE_ZP"/>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                <img src='<%# Eval("LOGO") %>' alt="Logo" width="50" height="50" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField DataField="DESCRIPCION_DP" HeaderText="UNIDAD ACADËMICA" ItemStyle-Width="150px"/>
                                                                        <asp:BoundField DataField="VENCIDOS" HeaderText="NOMBRAMIENTOS VENCIDOS" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center"/>

                                                                        <asp:TemplateField HeaderText="Detalles" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton runat="server" ID="LinkButtonUnidadAcademicaNS_resumen_seleccionar" CommandArgument = '<%# Eval("CLAVE_ZP") + "," + Eval("DESCRIPCION_DP") %>' CssClass="btn btn-sm btn-outline-secondary LoadingOverlay" OnClick="LinkButtonUnidadAcademicaNS_resumen_seleccionar_Click">Mostrar</asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>                                                
                                              
                                                                    </Columns>

                                                                    <EmptyDataTemplate>
                                                                        <div class="text-center">
                                                                            <asp:Label runat="server" ID="Label4" Text="<br><br><br> No se han asignado unidades académicas de nivel superior a su perfil de analista <br><br><br>" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                                        </div>
                                                                    </EmptyDataTemplate>

                                                                </asp:GridView>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </section>

                                            </div>

                                            <div class="col-xl-6">
                                                <section class="section contact">
                                                    <div class="row">

                                                        <div class="col-lg-12">
                                                            <h5 class="card-title">Vigencia de nombramientos</h5>
                                                        </div>

                                                        <div class="col-lg-6">
                                                            <div class="card info-card revenue-card">
                                                                <div class="card-body">
                                                                    <h5 class="card-title">Dirección</h5>
                                                                    <div class="d-flex align-items-center">
                                                                    <div runat="server" id="DivUnidadesAcademicasNS_resumenDireccion_icon" class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                        <i runat="server" id="UnidadesAcademicasNS_resumenDireccion_icon" class="bi bi-clock-history"></i>
                                                                    </div>
                                                                    <div class="ps-3">
                                                                        <h6><asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenDireccion_total"></asp:Label></h6>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenDireccion_estatus" Visible="true"></asp:Label>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenDireccion_obs" Visible="true"></asp:Label>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card-footer footer-card">
                                                                    <asp:LinkButton ID="LinkButtonUnidadesAcademicasNS_resumenDireccion" runat="server" OnClick="LinkButtonUnidadesAcademicasNS_resumenDireccion_Click" CssClass="LoadingOverlay">
                                                                        <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-lg-6">
                                                            <div class="card info-card revenue-card">
                                                                <div class="card-body">
                                                                    <h5 class="card-title">Subdirección Académica</h5>
                                                                    <div class="d-flex align-items-center">
                                                                    <div runat="server" id="DivUnidadesAcademicasNS_resumenAcademica_icon" class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                        <i runat="server" id="UnidadesAcademicasNS_resumenAcademica_icon" class="bi bi-clock-history"></i>
                                                                    </div>
                                                                    <div class="ps-3">
                                                                        <h6><asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenAcademica_total"></asp:Label></h6>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenAcademica_estatus" Visible="true"></asp:Label>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenAcademica_obs" Visible="true"></asp:Label>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card-footer footer-card">
                                                                    <asp:LinkButton ID="LinkButtonUnidadesAcademicasNS_resumenAcademica" runat="server" OnClick="LinkButtonUnidadesAcademicasNS_resumenAcademica_Click" CssClass="LoadingOverlay">
                                                                        <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-lg-6">
                                                            <div class="card info-card revenue-card">
                                                                <div class="card-body">
                                                                    <h5 class="card-title">Subdirección de Servicios<span><br />| Educativos e Integración Social</span></h5>
                                                                    <div class="d-flex align-items-center">
                                                                    <div runat="server" id="DivUnidadesAcademicasNS_resumenSSEIS_icon" class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                        <i runat="server" id="UnidadesAcademicasNS_resumenSSEIS_icon" class="bi bi-clock-history"></i>
                                                                    </div>
                                                                    <div class="ps-3">
                                                                        <h6><asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenSSEIS_total"></asp:Label></h6>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenSSEIS_estatus" Visible="true"></asp:Label>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenSSEIS_obs" Visible="true"></asp:Label>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card-footer footer-card">
                                                                    <asp:LinkButton ID="LinkButtonUnidadesAcademicasNS_resumenSSEIS" runat="server" OnClick="LinkButtonUnidadesAcademicasNS_resumenSSEIS_Click" CssClass="LoadingOverlay">
                                                                        <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-lg-6">
                                                            <div class="card info-card revenue-card">
                                                                <div class="card-body">
                                                                    <h5 class="card-title">Subdirección Administrativa</h5>
                                                                    <div class="d-flex align-items-center">
                                                                    <div runat="server" id="DivUnidadesAcademicasNS_resumenAdministracion_icon" class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                        <i runat="server" id="UnidadesAcademicasNS_resumenAdministracion_icon" class="bi bi-clock-history"></i>
                                                                    </div>
                                                                    <div class="ps-3">
                                                                        <h6><asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenAdministracion_total"></asp:Label></h6>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenAdministracion_estatus" Visible="true"></asp:Label>
                                                                        <asp:Label runat="server" ID="LabelUnidadesAcademicasNS_resumenAdministracion_obs" Visible="true"></asp:Label>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card-footer footer-card">
                                                                    <asp:LinkButton ID="LinkButtonUnidadesAcademicasNS_resumenAdministrativa" runat="server" OnClick="LinkButtonUnidadesAcademicasNS_resumenAdministrativa_Click" CssClass="LoadingOverlay">
                                                                        <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </section>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>                                      
                                
                              </div>
                            </div>
                          </div>

                        </div>

                        <br />
                    </div>

                    <!-- PanelInicial -->
                    <div id="divPanelInicial" runat="server" visible="false">
                        <div class="accordion" id="divAccordionPlan">

                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseContenido_filtroPlan" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                                Ocupabilidad de grupos de la Unidad Académica
                              </button>
                            </h2>
                            <div id="collapseContenido_filtroPlan" class="accordion-collapse collapse">
                              <div class="accordion-body">

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        
                                        <section class="section">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="card">
                                                        <div class="card-body">
                                                            <%--FILTROS--%>
                                                            <div class="row mb-3">
                                                                <%--Dropdown unidad academica--%>
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2">
                                                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Unidad académica: </h6>
                                                                    <asp:DropDownList ID="DropDownListUnidadAcademica" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropUA"
                                                                        DataTextField="DESCRIPCION_DP" DataValueField="CLAVE_ZP" CssClass="form-select" data-control="select2"
                                                                        OnDataBound="DropDownListUnidadAcademica_DataBound"
                                                                        OnSelectedIndexChanged="DropDownListUnidadAcademica_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="SqlDataSourceDropUA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="SELECT CLAVE_ZP, DESCRIPCION_DP FROM  CAT_DEPENDENCIAS_POLITECNICAS
                        WHERE CLAVE_UA IS NOT NULL
                        AND ID_NIVEL_EST = 2
                        ORDER BY CLAVE_UA"></asp:SqlDataSource>
                                                                </div>
                                                                <%--Dropdown modalidad--%>
                                                                <%--<div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2">
                                                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Modalidad: </h6>
                                                                    <asp:DropDownList ID="DropDownListModalidad" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropModalidad"
                                                                        DataTextField="DESCRIPCION" DataValueField="ID_MODALIDAD" CssClass="form-select"
                                                                        OnDataBound="DropDownListModalidad_DataBound"
                                                                        OnSelectedIndexChanged="DropDownListModalidad_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="SqlDataSourceDropModalidad" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="SELECT DISTINCT cm.ID_MODALIDAD, cm.DESCRIPCION FROM CAT_MODALIDADES cm, PROGRAMAS_ACADEMICOS cp
                        WHERE cm.ID_MODALIDAD = cp.ID_MODALIDAD
                        AND cp.CLAVE_ZP = @UA">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </div>--%>
                                                                <%--Dropdown programa académico--%>
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2">
                                                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Programa académico: </h6>
                                                                    <asp:DropDownList ID="DropDownListProgramaAcad" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropPrograAcad"
                                                                        DataTextField="DESCRIPCION" DataValueField="ID_PROGRAMA_ACAD" CssClass="form-select"
                                                                        OnDataBound="DropDownListProgramaAcad_DataBound"
                                                                        OnSelectedIndexChanged="DropDownListProgramaAcad_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="SqlDataSourceDropPrograAcad" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="SELECT ID_PROGRAMA_ACAD, DESCRIPCION FROM PROGRAMAS_ACADEMICOS
                        WHERE CLAVE_ZP =@UA 
                        AND ID_MODALIDAD = 1">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <%--<asp:ControlParameter ControlID="DropDownListModalidad" Name="idModalidadd" PropertyName="SelectedValue" />--%>
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                                <%--Dropdown plan de estudios--%>
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2">
                                                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Plan de estudios: </h6>
                                                                    <asp:DropDownList ID="DropDownListPlanEstudio" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropPlanEstud"
                                                                        DataTextField="ID_PLAN_ESTUDIO" DataValueField="ID_PLAN_ESTUDIO" CssClass="form-select"
                                                                        OnDataBound="DropDownListPlanEstudio_DataBound"
                                                                        OnSelectedIndexChanged="DropDownListPlanEstudio_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="SqlDataSourceDropPlanEstud" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="SELECT DISTINCT ID_PLAN_ESTUDIO FROM PLANES_ESTUDIO
                        WHERE CLAVE_ZP = @UA
                        AND ID_PROGRAMA_ACAD = @programaAcad">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListProgramaAcad" Name="programaAcad" PropertyName="SelectedValue" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                                <%--Dropdown unidad de aprendizaje--%>
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2" id="divEspecialidad" runat="server" visible="true">
                                                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Unidad de aprendizaje: </h6>
                                                                    <asp:DropDownList ID="DropDownListUnidadAprend" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropEspecialidad"
                                                                        DataTextField="DESCRIPCION" DataValueField="ID_ASIGNATURA" CssClass="form-select"
                                                                        OnDataBound="DropDownListUnidadAprend_DataBound"
                                                                        OnSelectedIndexChanged="DropDownListUnidadAprend_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="SqlDataSourceDropEspecialidad" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="select ID_ASIGNATURA, DESCRIPCION from PLANES_ESTUDIO
                        where CLAVE_ZP = @UA 
                        and ID_PROGRAMA_ACAD = @programaAcad
                        and ID_PLAN_ESTUDIO = @planEstudio
                        order by DESCRIPCION">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListProgramaAcad" Name="programaAcad" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListPlanEstudio" Name="planEstudio" PropertyName="SelectedValue" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                                <%--Dropdow periodo par/impar--%>
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2" id="divDdlPeriodo" runat="server" style="display: none;">
                                                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Periodo: </h6>
                                                                    <asp:DropDownList ID="DropDownListPeriodo" runat="server" AutoPostBack="true" CssClass="form-select border-primary"
                                                                        OnSelectedIndexChanged="DropDownListPeriodo_SelectedIndexChanged">
                                                                        <asp:ListItem Text="Seleccionar" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="Impar" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Par" Value="2"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div id="divCheckPeriodo" runat="server" class="row mb-3" visible="false">
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2">
                                                                    <asp:CheckBox ID="CheckBoxPeriodo" runat="server" AutoPostBack="true" CssClass="ChkBoxClassSize"
                                                                        OnCheckedChanged="CheckBoxVigencia_CheckedChanged" />
                                                                    <label for="CheckBoxVigencia" class="small">Mostrar filtro periodo</label>
                                                                </div>
                                                            </div>
                                   
                                                            <%--GridVies--%>
                                                            <div id="divGridOcupo" runat="server" class="row" visible="false">
                                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                                                                    <div class="table-responsive modal-dialog-scrollable">

                                                                        <asp:GridView ID="GridViewOcupabilidad" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGrupoPerioodo"
                                                                            CssClass="table table-striped table-bordered small" PagerStyle-CssClass="pagination-ys" PageSize="20"
                                                                            AllowPaging="false" AlternatingRowStyle-CssClass="alt" SelectedRowStyle-CssClass="alt2"
                                                                            Style="border-collapse: collapse;">
                                                                            <HeaderStyle CssClass="table-primary text-center" HorizontalAlign="Center" />
                                                                            <AlternatingRowStyle CssClass="alt" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Periodo escolar" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="LinkButtonGVocupabilidad_pe" runat="server" 
                                                                                            CommandArgument='<%# Eval("PERIODO_ESCOLAR")%>' 
                                                                                            OnClick="LinkButtonGVocupabilidad_pe_Click" 
                                                                                            CssClass="btn btn-secondary btn-sm LoadingOverlay" 
                                                                                            data-bs-toggle="popover" data-bs-placement="left"
                                                                                            data-bs-custom-class="custom-popover"
                                                                                            data-bs-trigger="hover focus"
                                                                                            data-bs-title="Mapa de calor"
                                                                                            data-bs-content="Mostrará el mapa de calor de la ocupabilidad por periodo escolar, de los grupos de la unidad de aprendizaje.">
                                                                                                <i class="bi bi-bar-chart-steps" style="font-style: normal;">
                                                                                                    &nbsp;&nbsp;&nbsp;<%# Eval("PERIODO_ESCOLAR")%>
                                                                                                </i>
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Alumnos" HeaderText="Total de alumnos" ReadOnly="True"
                                                                                    SortExpression="Alumnos" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="GRUPOS" HeaderText="Total de grupos" ReadOnly="True"
                                                                                    SortExpression="GRUPOS" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="PROMEDIO" HeaderText="Promedio (Cupo por grupo)" ReadOnly="True"
                                                                                    SortExpression="PROMEDIO" ItemStyle-HorizontalAlign="Center" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div class="text-center alert alert-warning px-auto py-auto">
                                                                                    <asp:Label runat="server" ID="LblSinRegistros_mensaje"
                                                                                        Text="No se encontraron datos de la unidad de aprendizaje."
                                                                                        CssClass="fw-bold" Width="90%"></asp:Label>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>

                                                                    </div>
                                                                    <%-------------- datasource del gridview para obtencion de datos de unidades de aprendizaje/grupos*alumno------------------%>
                                                                    <asp:SqlDataSource ID="SqlDataSourceGrupoPerioodo" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="SELECT hcd.PERIODO_ESCOLAR,
		(SELECT SUM(t1.ALUMNOS) 
			FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
					FROM HISTORIAL_CARGA_DOCENTE
					WHERE CLAVE_ZP = hcd.CLAVE_ZP 
						AND MODALIDAD = hcd.MODALIDAD
						AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
						AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR						
				) AS t1
		) as Alumnos,
		COUNT (DISTINCT SECUENCIA) as GRUPOS,
		(SELECT ROUND(AVG(CAST(ALUMNOS AS FLOAT)), 0)   
			FROM HISTORIAL_CARGA_DOCENTE
			WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				AND MODALIDAD = hcd.MODALIDAD
				AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		) AS PROMEDIO 
	FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
	WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
		AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
		AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		AND hcd.CLAVE_ZP = @UA
		AND hcd.MODALIDAD = 1
		AND hcd.ID_ASIGNATURA = @unidadAprend
		AND pe.ID_PLAN_ESTUDIO = @planEstudio
	GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA
	ORDER BY hcd.PERIODO_ESCOLAR">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <%--<asp:ControlParameter ControlID="DropDownListModalidad" Name="modalidad" PropertyName="SelectedValue" />--%>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAprend" Name="unidadAprend" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListPlanEstudio" Name="planEstudio" PropertyName="SelectedValue" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                    <%-------------- datasource del gridview para obtencion de datos de unidades de aprendizaje/grupos*alumno------------------%>
                                                                    <asp:SqlDataSource ID="SqlDataSourcePEParImpar" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                        SelectCommand="SELECT hcd.PERIODO_ESCOLAR,
		(SELECT SUM(t1.ALUMNOS) 
			FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
					FROM HISTORIAL_CARGA_DOCENTE
					WHERE CLAVE_ZP = hcd.CLAVE_ZP 
						AND MODALIDAD = hcd.MODALIDAD
						AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
						AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR						
				) AS t1
		) as Alumnos,
		COUNT (DISTINCT SECUENCIA) as GRUPOS,
		(SELECT ROUND(AVG(CAST(ALUMNOS AS FLOAT)), 0)   
			FROM HISTORIAL_CARGA_DOCENTE
			WHERE CLAVE_ZP = hcd.CLAVE_ZP 
				AND MODALIDAD = hcd.MODALIDAD
				AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
				AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR 
		) AS PROMEDIO 
	FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
	WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
		AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
		AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		AND hcd.CLAVE_ZP = @UA
		AND hcd.MODALIDAD = 1
		AND hcd.ID_ASIGNATURA = @unidadAprend
		AND pe.ID_PLAN_ESTUDIO = @planEstudio
                                                AND PERIODO_ESCOLAR like  '%' + @periodo + ''
	GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA
	ORDER BY hcd.PERIODO_ESCOLAR">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <%--<asp:ControlParameter ControlID="DropDownListModalidad" Name="modalidad" PropertyName="SelectedValue" />--%>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAprend" Name="unidadAprend" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListPlanEstudio" Name="planEstudio" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListPeriodo" Name="periodo" PropertyName="SelectedValue" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>


                                                                </div>
                                                            </div>

                                                            <br />

                                                            <%--Calculos estadisticos--%>
                                                            <div id="divCalculos" runat="server" class="row" visible="false">
                                                                <asp:Label ID="lblMedia" runat="server" Text="" CssClass=""></asp:Label><br />
                                                                <asp:Label ID="lblVarianza" runat="server" Text=""></asp:Label><br />
                                                                <asp:Label ID="lblDesv" runat="server" Text=""></asp:Label>
                                                                <asp:Label ID="lblRango" runat="server" Text=""></asp:Label>
                                                            </div>

                                                            <div id="divDetailGraf" runat="server" class="row mt-2" visible="false">
                                                                <%--Detalles estadisticos--%>
                                                                <div class="d-grid gap-2 col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 mt-2">
                                                                    <asp:LinkButton ID="LinkButtonAddProgramAcad" runat="server" CssClass="btn btn-primary LoadingOverlay"
                                                                        Text='<i class="fas fa-th-list fa-sm fa-fw"></i> Detalles estadisticos'
                                                                        OnClick="LinkButtonAddProgramAcad_Click">
                                                                    </asp:LinkButton>
                                                                </div>
                                                                <div class="d-grid gap-2 col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 mt-2">
                                                                    <asp:LinkButton ID="LinkButtonGraf" runat="server" CssClass="btn btn-primary LoadingOverlay"
                                                                        Text='<i class="fas fa-th-list fa-sm fa-fw"></i> Gráficos'
                                                                        OnClick="LinkButtonGraf_Click">
                                                                    </asp:LinkButton>
                                                                </div>

                                                                <div class="d-grid gap-2 col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 mt-2">

                                                                    <div class="d-grid gap-2 ">
                                                                        <div class="input-group flex-nowrap">
                                                                            <span class="input-group-text btn btn-primary" id="addonFiltro_pe"
                                                                                data-bs-toggle="popover" data-bs-placement="left"
                                                                                data-bs-custom-class="custom-popover"
                                                                                data-bs-trigger="hover focus"
                                                                                data-bs-title="Mapa de calor general"
                                                                                data-bs-content="Mostrará el mapa de calor de la ocupabilidad por periodo escolar, de todos los grupos de la unidad académica.">
                                                                                <i class="bi bi-bar-chart-steps" style="font-style: normal;">&nbsp;&nbsp;&nbsp;Mapa de calor</i></span>

                                                                            <asp:DropDownList id="DropDownFiltro_periodo" OnSelectedIndexChanged="DropDownFiltro_periodo_SelectedIndexChanged"
                                                                                AutoPostBack="true"
                                                                                CssClass="form-select"
                                                                                runat="server">

                                                                            </asp:DropDownList> 

                                                                        </div>
                                                                    </div>

                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </section>

                                    </ContentTemplate>
                                </asp:UpdatePanel>                                      
                                
                              </div>
                            </div>
                          </div>

                        </div>

                        <br />
                        
                    </div>

                    <!-- PanelEventos -->
                    <div id="divPanelEventos" runat="server" visible="false">
                        <div class="accordion" id="divAccordionEventos">

                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseContenido_filtroEventos" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                                Resúmen de los eventos de riesgo 
                              </button>
                            </h2>
                            <div id="collapseContenido_filtroEventos" class="accordion-collapse collapse">
                              <div class="accordion-body">

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        
                                        <section class="section">
                                            <div class="row">
                                                <div class="col-12 col-lg-6" runat="server" id="divAlertUnidadAcademica_seleccionada" visible="false">

                                                    <div class="alert alert-warning alert-dismissible fade show" role="alert">
                                                      Unidad académica seleccionada: <strong><asp:Label runat="server" ID="LabelUnidadAcademicaSeleccionada_nombre"></asp:Label></strong>
                                                        <asp:LinkButton runat="server" ID="LinkButtonAlertUnidadAcademica_cerrar" CssClass="btn-close LoadingOverlay" OnClick="LinkButtonAlertUnidadAcademica_cerrar_Click"
                                                            aria-label="Close"
                                                            data-bs-toggle="popover" data-bs-placement="right"
                                                            data-bs-custom-class="custom-popover"
                                                            data-bs-trigger="hover focus"
                                                            data-bs-title="Cancelar filtro"
                                                            data-bs-content="Elimina el filtro por unidad académica, mostrando la información general."><!-- -->

                                                        </asp:LinkButton>

                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-12 col-lg-9">
                                                        <figure class="highcharts-figure">
                                                            <div id="container-column-eventos" style="max-height: 240px;"></div>
                                                        </figure>
                                                    </div>
                                                    <div class="col-12 col-lg-3">
                                                        <!-- EventosRiesgo -->
                                                        <div id="DivCardEventosRiesgo" runat="server" class="col-12" visible="false">
                                                            <div class="card info-card revenue-card">
            
                                                                <div class="filter" style="display:none">
                                                                    <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                                                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                                                        <li class="dropdown-header text-start">
                                                                            <h6>Opción</h6>
                                                                        </li>
                                                                        <asp:LinkButton ID="LinkButtonCardEventosRiesgo_buscar" runat="server"><li class="dropdown-item">Búsqueda</li></asp:LinkButton>
                                                                    </ul>
                                                                </div>
            
                                                                <div class="card-body">
                                                                    <h5 class="card-title">
                                                                        Eventos de riesgo
                                                                        <i class="bi bi-info-circle info-text"
                                                                            data-bs-toggle="popover" data-bs-placement="right"
                                                                            data-bs-custom-class="custom-popover"
                                                                            data-bs-trigger="hover focus"
                                                                            data-bs-title="Eventos de riesgo"
                                                                            data-bs-content="Muestra el total de eventos de riesgo registrados.">
                                                                        </i> 
                                                                        <p><span>| Total de </span></p>
                                                                    </h5>
            
                                                                    <div class="d-flex align-items-center">
                                                                        <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                            <i class="bi bi-layout-text-window-reverse icon-green"></i>
                                                                        </div>
                                                                        <div class="ps-3">
                                                                            <h6><asp:Label ID="LabelCardEventosRiesgo_total" runat="server"></asp:Label></h6>
                                                                            <span class="text-success small pt-1 fw-bold"></span>
                                                                            <span class="text-muted small pt-2 ps-1 optionDet"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card-footer footer-card">
                                                                    <asp:LinkButton ID="LinkButtonCardEventosRiesgo_datos" runat="server" OnClick="LinkButtonCardEventosRiesgo_datos_Click" CssClass="LoadingOverlay">
                                                                        <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="col-12" style="display:block">
                                                    <div class="row">

                                                        <div class="col-12 col-lg-6">
                                                            <!-- UnidadesAcademicasNS -->
                                                            <div class="card info-card revenue-card">
                                                                <div class="card-body">
                                                                    <h5 class="card-title">
                                                                        Unidades académicas
                                                                        <i class="bi bi-info-circle info-text"
                                                                            data-bs-toggle="popover" data-bs-placement="right"
                                                                            data-bs-custom-class="custom-popover"
                                                                            data-bs-trigger="hover focus"
                                                                            data-bs-title="Unidades académicas"
                                                                            data-bs-content="Muestra la relación de unidades académicas con eventos de riesgo registrados.">
                                                                        </i> 
                                                                    </h5>
                                                                    <table>
                                                                        <thead>
                                                                          <tr>
                                                                            <th style="width:20%"></th>
                                                                            <th style="text-align:center;padding-inline: 15px;width:20%">Unidad Académica</th>
                                                                              <th style="text-align:center;padding-inline: 15px;width:20%">Peticiones</th>
                                                                            <th style="text-align:center;padding-inline: 15px;width:30%">Datos</th>
                                                                          </tr>
                                                                        </thead>
                                                                    </table>

                                                                    <div style="overflow-y: scroll; max-height: 320px; width: 100%;">
                                                                        <asp:GridView ID="GridViewUnidadesAcademicasNS" runat="server"
                                                                            AutoGenerateColumns="False" ShowHeader="false"
                                                                                CssClass="table table-borderless"
                                                                                    PagerStyle-CssClass="pagination-ys"
                                                                                        PageSize="100" AllowPaging="false" 
                                                                                            OnPageIndexChanging="GridViewUnidadesAcademicasNS_PageIndexChanging" 
                                                                                                OnRowDataBound="GridViewUnidadesAcademicasNS_RowDataBound">
                                            
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CLAVE_ZP" HeaderText="CLAVE_ZP"/>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <ItemTemplate>
                                                                                        <img src='<%# Eval("LOGO") %>' alt="Logo" width="50" height="50" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:BoundField DataField="DESCRIPCION_DP" HeaderText="UNIDAD ACADËMICA"/>
                                                                                <asp:BoundField DataField="PETICIONES" HeaderText="PETICIONES"/>

                                                                                <asp:TemplateField HeaderText="Detalles" ItemStyle-CssClass="align-center">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton runat="server" ID="LinkButtonUnidadAcademicaNS_seleccionar" CommandArgument = '<%# Eval("CLAVE_ZP") +","+  Eval("DESCRIPCION_DP")%>' CssClass="btn btn-sm btn-outline-danger LoadingOverlay" OnClick="LinkButtonUnidadAcademicaNS_seleccionar_Click">Mostrar</asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>                                                
                                                                            </Columns>

                                                                            <EmptyDataTemplate>
                                                                                <div class="text-center">
                                                                                    <asp:Label runat="server" ID="Label4" Text="<br><br><br> No se han asignado unidades académicas de nivel superior a su perfil de analista <br><br><br>" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                                                </div>
                                                                            </EmptyDataTemplate>

                                                                        </asp:GridView>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-12 col-lg-6">
                                                            <div class="card info-card revenue-card">
                                                                <div class="card-body">
                                                                    <h5 class="card-title">
                                                                        Porcentaje de atención
                                                                        <i class="bi bi-info-circle info-text"
                                                                            data-bs-toggle="popover" data-bs-placement="right"
                                                                            data-bs-custom-class="custom-popover"
                                                                            data-bs-trigger="hover focus"
                                                                            data-bs-title="Porcentaje de atención"
                                                                            data-bs-content="Muestra el porcentahe de atención para los eventos registrados.">
                                                                        </i> 
                                                                    </h5>
                                                                    <figure class="highcharts-figure">
                                                                        <div id="container-pie-eventos" style="max-height: 320px;"></div>
                                                                    </figure>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="row col-12">

                                                    <!-- EventosAtendidos -->
                                                    <div id="DivCardEventosAtendidos" runat="server" class="col-4" visible="false">
                                                        <div class="card info-card revenue-card">
            
                                                            <div class="filter" style="display:none">
                                                                <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                                                                <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                                                    <li class="dropdown-header text-start">
                                                                        <h6>Opción</h6>
                                                                    </li>
                                                                    <asp:LinkButton ID="LinkButtonCardEventosEtendidos_buscar" runat="server"><li class="dropdown-item">Búsqueda</li></asp:LinkButton>
                                                                </ul>
                                                            </div>
            
                                                            <div class="card-body">
                                                                <h5 class="card-title">
                                                                    Eventos atendidos
                                                                    <i class="bi bi-info-circle info-text"
                                                                        data-bs-toggle="popover" data-bs-placement="right"
                                                                        data-bs-custom-class="custom-popover"
                                                                        data-bs-trigger="hover focus"
                                                                        data-bs-title="Eventos atendidos"
                                                                        data-bs-content="Muestra el porcentaje de los eventos que se han atendido.">
                                                                    </i> 
                                                                    <p><span>| Porcentaje de </span></p>
                                                                </h5>
            
                                                                <div class="d-flex align-items-center">
                                                                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                        <i class="bi bi-layout-text-window-reverse icon-green"></i>
                                                                    </div>
                                                                    <div class="ps-3">
                                                                        <h6><asp:Label ID="LabelCardEventosAtendidos_total" runat="server"></asp:Label></h6>
                                                                        <span class="text-success small pt-1 fw-bold"></span>
                                                                        <span class="text-muted small pt-2 ps-1 optionDet"></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="card-footer footer-card">
                                                                <asp:LinkButton ID="LinkButtonCardEventosAtendidos_datos" runat="server" OnClick="LinkButtonCardEventosAtendidos_datos_Click">
                                                                    <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- DetallePendientes -->
                                                    <div id="DivCardDetallePendientes" runat="server" class="col-4" visible="false">
                                                        <div class="card info-card revenue-card">
            
                                                            <div class="filter" style="display:none">
                                                                <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                                                                <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                                                    <li class="dropdown-header text-start">
                                                                        <h6>Opción</h6>
                                                                    </li>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server"><li class="dropdown-item">Búsqueda</li></asp:LinkButton>
                                                                </ul>
                                                            </div>
            
                                                            <div class="card-body">
                                                                <h5 class="card-title">
                                                                    Eventos por atender
                                                                    <i class="bi bi-info-circle info-text"
                                                                        data-bs-toggle="popover" data-bs-placement="right"
                                                                        data-bs-custom-class="custom-popover"
                                                                        data-bs-trigger="hover focus"
                                                                        data-bs-title="Eventos por atender"
                                                                        data-bs-content="Muestra el porcentaje de los eventos que se encuentran pendientes por atender.">
                                                                    </i> 
                                                                    <p><span>| Porcentaje de </span></p>
                                                                </h5>
            
                                                                <div class="d-flex align-items-center">
                                                                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                                                                        <i class="bi bi-layout-text-window-reverse icon-green"></i>
                                                                    </div>
                                                                    <div class="ps-3">
                                                                        <h6><asp:Label ID="LabelCardDetallePendientes_total" runat="server"></asp:Label></h6>
                                                                        <span class="text-success small pt-1 fw-bold"></span>
                                                                        <span class="text-muted small pt-2 ps-1 optionDet"></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="card-footer footer-card">
                                                                <asp:LinkButton ID="LinkButtonCardDetallePendientes_datos" runat="server" OnClick="LinkButtonCardDetallePendientes_datos_Click">
                                                                    <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </section>

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


    <!-- Modal  ResumenNombramientos-->
    <div class="modal fade" id="ModalResumenNombramientos" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title titleModal" id="tittleModalResumenNombramientos">Motor de búsqueda</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <!-- Modal -->
                <div id="DivModalResumenNombramientos_body" runat="server" class="col-md-12 dashboard">
                    
                    <div class="card info-card customers-card">
                        <div class="filter" style="display:block">

                            <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                <li class="dropdown-header text-start">
                                    <h6>Exportar</h6>
                                </li>
                                <li class="dropdown-item" runat="server" id="ModalResumenNombramientos_dropdownItem">
                                    <i class="bi bi-filetype-xlsx iconExcel"></i><asp:Button ID="LinkButtonModalResumenNombramientos_Excel" runat="server" Text="Excel" CssClass="btn btn-outline-default" Visible="true" OnClick="LinkButtonModalResumenNombramientos_Excel_Click" />
                                </li>
                            </ul>

                        </div>
                        <div class="card-body">

                            <asp:UpdatePanel runat="server"><ContentTemplate>
                                <br />
                                <h5><asp:Label runat="server" ID="LabelModalResumenNombramientos_titulo" Text="" CssClass="card-title"></asp:Label><span class="card-title"> | <asp:Label ID="LabelModalResumenNombramientos_subtitulo" runat="server" Text=""></asp:Label></span></h5>
                                <br />

                                <div class="card" style="overflow-y: scroll; max-height: 360px; width: 100%;">

                                    <asp:GridView ID="GridViewResumenNombramientos" runat="server"
                                        AutoGenerateColumns="False" ShowHeader="true"
                                            CssClass="table table-sm table-bordered table-striped table-responsive" HeaderStyle-CssClass=" bg-gradient bg-primary-light text-gray-100 text-center"
                                                PagerStyle-CssClass="pagination-ys"
                                                    PageSize="1000" AllowPaging="false" 
                                                        OnPageIndexChanging="GridViewResumenNombramientos_PageIndexChanging" 
                                                            OnRowDataBound="GridViewResumenNombramientos_RowDataBound" 
                                                                OnRowCancelingEdit="GridViewResumenNombramientos_RowCancelingEdit" 
                                                                    OnRowEditing="GridViewResumenNombramientos_RowEditing" 
                                                                        OnRowUpdating="GridViewResumenNombramientos_RowUpdating">
                                            
                                        <Columns>
                                            <asp:TemplateField HeaderText="NÚM." ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="CLAVE_ZP" HeaderText="ZONA PAGADORA" ReadOnly="true"/>
                                            <asp:BoundField DataField="ID_USER" HeaderText="USUARIO" ReadOnly="true"/>
                                            <asp:BoundField DataField="ID_PERFIL" HeaderText="PERFIL" ReadOnly="true"/>

                                            <asp:BoundField DataField="DESCRIPCION_DP" HeaderText="UNIDAD ACADÉMICA"  ReadOnly="true"/>
                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="UNIDAD ADMINISTRATIVA"  ReadOnly="true"/>
                                            <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="NOMBRE"  ReadOnly="true"/>
                                            
                                            <asp:TemplateField HeaderText="APELLIDO PATERNO">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelResumenNombramientos_apat" runat="server" Text='<%#Eval("APELLIDO_PAT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxResumenNombramientos_apat" runat="server" Text='<%#Eval("APELLIDO_PAT") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="APELLIDO MATERNO">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelResumenNombramientos_amat" runat="server" Text='<%#Eval("APELLIDO_MAT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxResumenNombramientos_amat" runat="server" Text='<%#Eval("APELLIDO_MAT") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NOMBRE">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelResumenNombramientos_nombre" runat="server" Text='<%#Eval("NOMBRE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxResumenNombramientos_nombre" runat="server" Text='<%#Eval("NOMBRE") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="INICIO">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelResumenNombramientos_inicio" runat="server" Text='<%#Eval("FECHA_INICIO") %>' ></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxResumenNombramientos_inicio" runat="server" 
                                                        Text='<%# Eval("FECHA_INICIO", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TÉRMINO">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelResumenNombramientos_termino" runat="server" Text='<%#Eval("FECHA_FIN") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxResumenNombramientos_termino" runat="server"  
                                                        Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("FECHA_FIN")) %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NOMBRAMIENTO_EST" HeaderText="ESTATUS"  ReadOnly="true"/>

                                            <asp:TemplateField HeaderText="NOMBRAMIENTO" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="LinkButtonResumenNombramientos_pdf" CommandArgument = '<%# Eval("DESCRIPCION") + "," + Eval("DESCRIPCION_DP") + "," + Eval("NOMBRE_COMPLETO") + "," + Eval("PDF") %>' CssClass="btn btn-sm btn-outline-secondary LoadingOverlay" OnClick="LinkButtonResumenNombramientos_pdf_Click"><i class="bi bi-file-earmark-pdf iconPDF"> </i>Ver</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                
                                            <asp:TemplateField HeaderText="DATOS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="ButtonResumenNombramientos_Edit" runat="server" Text="Editar" CommandName="Edit" CssClass="btn btn-sm btn-outline-warning LoadingOverlay"/>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="ButtonResumenNombramientos_Update" runat="server" Text="Actualizar" CommandName="Update" CssClass="btn btn-sm btn-outline-danger LoadingOverlay mb-2"/>
                                                    <asp:Button ID="ButtonResumenNombramientos_Cancel" runat="server" Text="Cancelar" CommandName="Cancel" CssClass="btn btn-sm btn-outline-secondary LoadingOverlay"/>
                                                </EditItemTemplate>
                                            </asp:TemplateField>                                            
                                              
                                        </Columns>

                                        <EmptyDataTemplate>
                                            <div class="text-center">
                                                <asp:Label runat="server" ID="Label4" Text="<br><br><br> No se han asignado unidades académicas de nivel superior a su perfil de analista <br><br><br>" CssClass="alert alert-light" Width="90%"></asp:Label>
                                            </div>
                                        </EmptyDataTemplate>

                                    </asp:GridView>
                                </div>

                            </ContentTemplate></asp:UpdatePanel>
                        </div>
                    </div>
                    
                </div><!-- End Modal ResumenNombramientos Card -->
      
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
        </div>
    </div>

    <!-- Modal  DetalleEventos-->
    <div class="modal fade" id="ModalDetalleEventos" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title titleModal" id="tittleModalDetalleEventos">Motor de búsqueda</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <!-- Modal -->
                <div id="DivModalDetalleEventos_body" runat="server" class="col-md-12 dashboard">
                    
                    <div class="card info-card customers-card">
                        <div class="filter" style="display:none">

                            <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                <li class="dropdown-header text-start">
                                    <h6>Exportar</h6>
                                </li>
                                <li class="dropdown-item" runat="server" id="ModalDetalleEventos_dropdownItem" Visible="false">
                                    <i class="bi bi-filetype-xlsx iconExcel"></i><asp:Button ID="LinkButtonModalDetalleEventos_Excel" runat="server" Text="Excel" CssClass="btn btn-outline-default" Visible="true" />
                                </li>
                            </ul>

                        </div>
                        <div class="card-body">

                            <asp:UpdatePanel runat="server"><ContentTemplate>
                                <br />
                                <h5><asp:Label runat="server" ID="LabelModalDetalleEventos_title" Text="" CssClass="card-title"></asp:Label><span class="card-title"> | <asp:Label ID="LabelModalDetalleEventos_nombre" runat="server" Text=""></asp:Label></span></h5>
                                <br />
   
                                <div class="card-body row">

                                    <div class="info-card">
                                        <asp:Panel runat="server">
                                            <asp:UpdatePanel runat="server" >
                                                <ContentTemplate>
                                                
                                                    <div class="col-12 table-responsive">
                                                        <h5 class="card-title">Relación de eventos con riesgo registrados</h5>
                                                        <asp:GridView ID="GridViewDetalleEventos" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-bordered table-striped table-responsive" HeaderStyle-CssClass=" bg-gradient bg-primary-light text-gray-100 text-center" 
                                                            PagerStyle-CssClass="pagination-ys LoadingOverlay" 
                                                                PageSize="10" AllowPaging="true" OnPageIndexChanging="GridViewDetalleEventos_PageIndexChanging">
                            
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="NÚM." ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:BoundField DataField="DESCRIPCION_PETICION" HeaderText="ESTATUS" ItemStyle-CssClass="unidadOverflow"/>
                                                                <asp:BoundField DataField="DESCRIPCION_CAT_PETICION" HeaderText="CATEGORIA" ItemStyle-CssClass="unidadOverflow"/>
                                                                <asp:BoundField DataField="DESC_PETICION" HeaderText="PETICION" ItemStyle-CssClass="unidadOverflow"/>
                                                                <asp:BoundField DataField="DESC_RESP_PETICION" HeaderText="TRATAMIENTO" ItemStyle-CssClass="unidadOverflow"/>
                                                                <asp:BoundField DataField="FECHA_INICIO" HeaderText="INICIO"/>
                                                                <asp:BoundField DataField="FECHA_FIN" HeaderText="FIN"/>
<%--                                                                <asp:TemplateField HeaderText="DOCUMENTO">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="LinkButtonGrupoDisponible_grupo" CommandArgument = '<%# Eval("URL_DOCUMENTO")%>' CssClass="btn btn-sm btn-outline-success LoadingOverlay" OnClick="MostrarDetalleEventos_url_Click" >Abrir</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>

                                                                </Columns>

                                                            <EmptyDataTemplate>
                                                                <div class="text-center">
                                                                    <asp:Label runat="server" ID="mensaje" Text="<br> No se encontraron registros !!! <br>" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </div>    

                                </div>

                            </ContentTemplate></asp:UpdatePanel>
                        </div>
                    </div>
                    
                </div><!-- End Modal DetalleEventos Card -->
      
            </div>
            <asp:UpdatePanel runat="server" >
                <ContentTemplate>

            <div class="modal-footer">

                <div class="row align-items-center">

                    <div class="mb-3 col-12 col-lg-5 row">
                        <div class="col-12 col-lg-3">
                            <label for="DropDownListDetalleEvento_categoria" class="col-form-label">Categoria</label>
                        </div>
                        <div class="col-12 col-lg-9">
                            <%--Dropdown Categorias--%>
                            <asp:DropDownList ID="DropDownListDetalleEvento_categoria" runat="server" AutoPostBack="true" DataSourceID="SqlDataSource_cate"
                                DataTextField="DESCRIPCION_CAT_PETICION" DataValueField="ID_CAT_PETICION" CssClass="form-control ddl"
                                OnDataBound="DropDownListDetalleEvento_categoria_DataBound"
                                OnSelectedIndexChanged="DropDownListDetalleEvento_categoria_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_cate" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                SelectCommand="select * from CAT_CATEGORIA_PETICION">
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="mb-3 col-12 col-lg-7 row">
                        <div class="mb-2 col-auto">
                            <input type="radio" class="btn-check" name="detalleEvento_estatus" id="estatus_p" autocomplete="off">
                            <label class="btn btn-outline-secondary text-sm-center" for="estatus_p">Pendientes
                                <asp:Label runat="server" ID="LabelDetalleEvento_pendientes" CssClass="badge text-bg-danger"></asp:Label>
                            </label>                    
                        </div>
                        <div class="mb-2 col-auto">
                            <input type="radio" class="btn-check" name="detalleEvento_estatus" id="estatus_e" autocomplete="off">
                            <label class="btn btn-outline-secondary text-sm-center" for="estatus_e">En proceso
                                <asp:Label runat="server" ID="LabelDetalleEvento_atendiendo" CssClass="badge text-bg-warning"></asp:Label>
                            </label>                    
                        </div>
                        <div class="mb-2 col-auto">
                            <input type="radio" class="btn-check" name="detalleEvento_estatus" id="estatus_a" autocomplete="off">
                            <label class="btn btn-outline-secondary text-sm-center" for="estatus_a">Atendidas
                                <asp:Label runat="server" ID="LabelDetalleEvento_atendidas" CssClass="badge text-bg-success"></asp:Label>
                            </label>                    
                        </div>
                    </div>

                </div>
                
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        </div>
    </div> 

     <!-- Modal Filtro Detalle estadisticos-->
    <div class="modal fade" id="ModalUnidadesAcademicas" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-lg  modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<h5 class="modal-title titleModal" id="titleModalUnidadesAcademicas">Selección de unidades</h5>--%>
                </div>
                <div class="modal-body">
                    <div class="d-flex align-items-center mb-2">
                        <div class="ps-3">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <span class="text-success small pt-1 fw-bold">Unidad académica: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_claveZp" runat="server"></asp:Label></span>
                                    <br />
                                    <%--<span class="text-success small pt-1 fw-bold">Modalidad: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_modalidad" runat="server"></asp:Label></span>
                                    <br />--%>
                                    <span class="text-success small pt-1 fw-bold">Programa académico: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_programaAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Plan de estudio: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_planEst" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Unidad de aprendizaje: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_unidadAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <div id="divModalDetailPeriodo" runat="server" visible="false">
                                        <span class="text-success small pt-1 fw-bold">Periodos: </span><span
                                            class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_periodoPar" runat="server"></asp:Label></span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridViewResultados" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                    <asp:BoundField DataField="Xi" HeaderText="Promedio" DataFormatString="{0:F2}" />
                                    <asp:BoundField DataField="Desviacion" HeaderText="Desviación" DataFormatString="{0:F2}" />
                                    <asp:BoundField DataField="Desviacion2" HeaderText="Desviación²" DataFormatString="{0:F2}" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalGraficos" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered  modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">Gráficos estadísticos</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="d-flex align-items-center mb-2">
                        <div class="ps-3">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <span class="text-success small pt-1 fw-bold">Unidad académica: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_claveZp" runat="server"></asp:Label></span>
                                    <br />
                                    <%--<span class="text-success small pt-1 fw-bold">Modalidad: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_modalidad" runat="server"></asp:Label></span>
                                    <br />--%>
                                    <span class="text-success small pt-1 fw-bold">Programa académico: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_programaAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Plan de estudio: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_planEst" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Unidad de aprendizaje: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_unidadAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <div id="divModalGraficoPeriodo" runat="server" visible="false">
                                        <span class="text-success small pt-1 fw-bold">Periodos: </span><span
                                            class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_periodoPar" runat="server"></asp:Label></span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                   
                    <div id="container-line" style="height: 400px;"></div>
                    <hr />
                    <div id="container-hist" style="height: 400px;"></div>
                    <hr />
                    <div id="container-scatter" style="height: 400px;"></div>
                    <div id="resumen" class="mt-3 fw-bold"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Mapa calor Detalle -->
    <div class="modal fade" id="ModalMapaCalorDetalle" tabindex="-1" data-bs-backdrop="static">
      <div class="modal-dialog modal-xl">
        <div class="modal-content" id="contentModalMapaCalorDetalle">
          <div class="modal-header">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
            
                    <h5 class="modal-title titleModal"><asp:Label ID="tittleModalMapaCalorDetalle" runat="server"></asp:Label></h5>
                    
                </ContentTemplate>
            </asp:UpdatePanel>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>

          </div>
          <div class="modal-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <div class="d-flex align-items-center mb-2">
                        <div class="ps-3">
                            <h6></h6>
                            <span class="text-success small pt-1 fw-bold">Periodo escolar: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalle_pe" runat="server"></asp:Label></span>
                            <br />
                            <span class="text-success small pt-1 fw-bold">Unidad académica: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalle_zp" runat="server"></asp:Label></span>
                            <br />
                        </div>
                    </div>

                    <!-- Mapa Calor detalle -->
                    <div id="DivMapaCalorDetalle" runat="server" class="col-md-12">
                        
                        <div class="card info-card customers-card">
                        
                            <div class="card-body">
                                <div id="container-mapa-calor"></div>
                                <div class="row">
                                    <table style="text-align: center; font-weight:500">
                                        <tr>
                                            <td style="width:100px"><span id="total">Total de grupos por dia</span></td>
                                            <td><span id="totalLun" ></span></td>
                                            <td><span id="totalMar" ></span></td>
                                            <td><span id="totalMie" ></span></td>
                                            <td><span id="totalJue" ></span></td>
                                            <td><span id="totalVie" ></span></td>
                                            <td style="width:85px"><asp:Label id="totalFin" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div><!-- End detalle Mapa de calor Card -->

                </ContentTemplate>
            </asp:UpdatePanel>

          </div>
          <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Modal Mapa calor Detalle UA -->
    <div class="modal fade" id="ModalMapaCalorDetalleUA" tabindex="-1" data-bs-backdrop="static">
      <div class="modal-dialog modal-xl">
        <div class="modal-content" id="contentModalMapaCalorDetalleUA">
          <div class="modal-header">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>   
                    <h5 class="modal-title titleModal"><asp:Label ID="tittleModalMapaCalorDetalleUA" runat="server"></asp:Label></h5>
                </ContentTemplate>
            </asp:UpdatePanel>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="d-flex align-items-center mb-2">
                        <div class="ps-3">
                            <h6></h6>
                            <span class="text-success small pt-1 fw-bold">Periodo escolar: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalleUA_pe" runat="server"></asp:Label></span>
                            <br />
                            <span class="text-success small pt-1 fw-bold">Unidad académica: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalleUA_zp" runat="server"></asp:Label></span>
                            <br />
                            <span class="text-success small pt-1 fw-bold">Modalidad: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalleUA_mod" runat="server"></asp:Label></span>
                            <br />
                            <span class="text-success small pt-1 fw-bold">Programa académico: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalleUA_pac" runat="server"></asp:Label></span>
                            <br />
                            <span class="text-success small pt-1 fw-bold">Plan de estudio: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalleUA_pes" runat="server"></asp:Label></span>
                            <br />
                            <span class="text-success small pt-1 fw-bold">Unidad de aprendizaje: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalMapaCalorDetalleUA_ua" runat="server"></asp:Label></span>
                        </div>

                    </div>

                    <!-- Mapa Calor detalle -->
                    <div id="DivMapaCalorDetalleUA" runat="server" class="col-md-12">
                        
                        <div class="card info-card customers-card">
                        
                            <div class="card-body">
                                <div id="container-mapa-calor-ua"></div>
                                <div class="row">
                                    <table style="text-align: center; font-weight:500">
                                        <tr>
                                            <td style="width:100px"><span id="totalUA">Total de grupos por dia</span></td>
                                            <td><span id="totalLunUA"></span></td>
                                            <td><span id="totalMarUA"></span></td>
                                            <td><span id="totalMieUA"></span></td>
                                            <td><span id="totalJueUA"></span></td>
                                            <td><span id="totalVieUA"></span></td>
                                            <td style="width:85px"><asp:Label id="totalFinUA" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div><!-- End detalle Mapa de calor Card -->
                </ContentTemplate>
            </asp:UpdatePanel>

          </div>
          <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal  DetalleUnidadAdministrativa-->
    <div class="modal fade" id="ModalDetalleUnidadAdministrativa" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title titleModal" id="tittleModalDetalleUnidadAdministrativa">Motor de búsqueda</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <!-- Modal -->
                <div id="DivModalDetalleUnidadAdministrativa_body" runat="server" class="col-md-12 dashboard">
                    
                    <div class="card info-card customers-card">
                        <div class="filter" style="display:none">

                            <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                <li class="dropdown-header text-start">
                                    <h6>Exportar</h6>
                                </li>
                                <li class="dropdown-item" runat="server" id="ModalDetalleUnidadAdministrativa_dropdownItem" Visible="false">
                                    <i class="bi bi-filetype-xlsx iconExcel"></i><asp:Button ID="LinkButtonModalDetalleUnidadAdministrativa_Excel" runat="server" Text="Excel" CssClass="btn btn-outline-default" Visible="true" />
                                </li>
                            </ul>

                        </div>
                        <div class="card-body">

                            <asp:UpdatePanel runat="server"><ContentTemplate>
                                <br />
                                <h5><asp:Label runat="server" ID="LabelModalDetalleUnidadAdministrativa_title" Text="" CssClass="card-title"></asp:Label><span class="card-title"> | <asp:Label ID="LabelModalDetalleUnidadAdministrativa_subtitle" runat="server" Text=""></asp:Label></span></h5>
                                <br />
   
                                <div class="card-body row">

                                    <div class="info-card">

                                            <div class="row gy-4">
                                                <div class="row col-xl-12 mt-3" runat="server" id="DivDetalleDropDownList_seccion" visible="false">

                                                    <%--Dropdown UnidadAcademica_resumen--%>
                                                    <div class="col-xl-12 mb-2">
                                                        <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder">Unidad académica: </h6>
                                                        <asp:DropDownList ID="DropDownListUnidadAcademica_resumen" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceUnidadAcademica_resumen"
                                                            DataTextField="DESCRIPCION_DP" DataValueField="CLAVE_ZP" CssClass="form-select" data-control="select2"
                                                            OnDataBound="DropDownListUnidadAcademica_resumen_DataBound"
                                                            OnSelectedIndexChanged="DropDownListUnidadAcademica_resumen_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSourceUnidadAcademica_resumen" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                            SelectCommand="SELECT CLAVE_ZP,IIF(VENCIDOS > 0, CONCAT(DESCRIPCION_DP,' -> ','SIN VIGENCIA'), DESCRIPCION_DP) DESCRIPCION_DP, VENCIDOS 
                                                                        from ( SELECT dp.CLAVE_ZP, dp.DESCRIPCION_DP, IIF(aut.VENCIDOS is null, 0, aut.VENCIDOS) VENCIDOS  
                                                            FROM  CAT_DEPENDENCIAS_POLITECNICAS dp left join (SELECT CLAVE_ZP, ID_PERFIL, COUNT(CLAVE_ZP) VENCIDOS 
                                                            FROM AUTORIDADES_ZP WHERE ID_PERFIL = @idp AND FECHA_FIN < (select FORMAT(GETDATE(),'yyyy-M-dd')) and ESTATUS = '1' 
                                                            group by CLAVE_ZP, ID_PERFIL) aut on aut.CLAVE_ZP = dp.CLAVE_ZP WHERE dp.CLAVE_ZP IS NOT NULL AND dp.ID_NIVEL_EST = 2 )datos 
                                                            GROUP BY CLAVE_ZP, DESCRIPCION_DP, VENCIDOS 
                                                            HAVING VENCIDOS > 0
                                                            ORDER BY VENCIDOS desc " >
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="HiddenFieldUnidadesAcademicasNS_idPerfil" Name="idp" PropertyName="Value" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>

                                                </div>
                                                <div class="row col-xl-12 mt-3" runat="server" id="DivDetallePerfil_seccion" visible="false">
                                                    <div class="col-xl-6">

                                                        <section class="section profile">
                                                            <div class="card" runat="server" id="DivCardDatosAutoridades_perfil">

                                                            </div>
                                                        </section>
                                                        <section class="section contact">
                                                            <div runat="server" id="DivCardDatosAutoridades_nombramiento" visible="false">
                                                                <div class="col-lg-12"> 
                                                                    <div class="info-box card"> 
                                                                    <i class="bi bi-filetype-pdf" style="color:forestgreen"></i> 
                                                                    <h3>Nombramiento</h3> 
                                                                    <p><asp:Label runat = "server" ID="LabelDatosAutoridad_pdf"></asp:Label></p> 
                                                                    <div class="card-footer footer-card"> 
                                                                        <asp:LinkButton ID="LinkButtonDatosAutoridad_pdf" runat="server" OnClick="LinkButtonResumenNombramientos_pdf_Click" CssClass="LoadingOverlay"> 
                                                                            <h5>Detalle <i class="bi bi-arrow-right-circle-fill"></i></h5> 
                                                                        </asp:LinkButton> 
                                                                    </div> 
                                                                    </div> 
                                                                </div>
                                                            </div>
                                                        </section>

                                                    </div>

                                                    <div class="col-xl-6">
                                                        <section class="section contact">
                                                            <div class="row">
                                                                <div class="col-lg-6">
                                                                    <div class="info-box card">
                                                                    <i class="bi bi-clock" style="color:forestgreen"></i>
                                                                    <h3>Inicio de nombramiento</h3>
                                                                    <p><asp:Label runat="server" ID="LabelDatosAutoridad_inicio"></asp:Label></p>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-6">
                                                                    <div class="info-box card">
                                                                    <i class="bi bi-clock" style="color:coral"></i>
                                                                    <h3>Término de nombramiento</h3>
                                                                    <p><asp:Label runat="server" ID="LabelDatosAutoridad_fin"></asp:Label></p>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-6">
                                                                    <div class="info-box card">
                                                                    <i class="bi bi-envelope"></i>
                                                                    <h3>Correo electrónico</h3>
                                                                    <p><asp:Label runat="server" ID="LabelDatosAutoridad_correo"></asp:Label></p>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-6">
                                                                    <div class="info-box card">
                                                                    <i class="bi bi-telephone"></i>
                                                                    <h3>Teléfono</h3>
                                                                    <p><asp:Label runat="server" ID="LabelDatosAutoridad_telefono"></asp:Label></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </section>
                                                    </div>
                                                </div>

                                            </div>

                                    </div>    

                                </div>

                            </ContentTemplate></asp:UpdatePanel>
                        </div>
                    </div>
                    
                </div><!-- End Modal DetalleUnidadAdministrativa Card -->
      
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
        </div>
    </div>

    <!-- Modal  VisualizarNombramiento-->
    <div class="modal fade" id="ModalVisualizarNombramiento" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-fullscreen">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title titleModal" id="tittleModalVisualizarNombramiento">Motor de búsqueda</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <!-- Modal -->
                <div id="DivModalVisualizarNombramiento_body" runat="server" class="col-md-12 dashboard">
                    
                    <div class="card info-card customers-card">
                        <div class="filter" style="display:none">

                            <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                <li class="dropdown-header text-start">
                                    <h6>Exportar</h6>
                                </li>
                                <li class="dropdown-item" runat="server" id="ModalVisualizarNombramiento_dropdownItem">
                                    <i class="bi bi-filetype-xlsx iconExcel"></i><asp:Button ID="LinkButtonModalVisualizarNombramiento_Excel" runat="server" Text="Excel" CssClass="btn btn-outline-default" Visible="true" />
                                </li>
                            </ul>

                        </div>
                        <div class="card-body">

                            <asp:UpdatePanel runat="server"><ContentTemplate>
                                <br />
                                <h5><asp:Label runat="server" ID="LabelModalVisualizarNombramiento_titulo" Text="" CssClass="card-title"></asp:Label><span class="card-title"> | <asp:Label ID="LabelModalVisualizarNombramiento_subtitulo" runat="server" Text=""></asp:Label></span></h5>
                                <br />

                                <div class="card">

                                    <iframe runat="server" id="FrameModalVisualizarNombramiento_pdf" style="min-width:100%" height="900" src="">

                                    </iframe>

                                </div>

                            </ContentTemplate></asp:UpdatePanel>
                        </div>
                    </div>
                    
                </div><!-- End Modal VisualizarNombramiento Card -->
      
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
        </div>
    </div>
    
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
            chartPieEventos("container-pie-eventos");
            chartColumnEventos("container-column-eventos");
            chartBarNombramientos("container-bar-nombramientos");
            chartPieNombramientos("container-pie-nombramientos");
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
            var LabelUA = document.getElementById('<%= HiddenFieldFiltro_ua.ClientID %>').value;

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

        function chartMapaCalor(zp, pe, container) {

            $("#totalLun").addClass("spinner-border");
            $("#totalMar").addClass("spinner-border");
            $("#totalMie").addClass("spinner-border");
            $("#totalJue").addClass("spinner-border");
            $("#totalVie").addClass("spinner-border");

            $.ajax({
                cache: false,
                type: "POST",
                url: "Dashboard.aspx/GetChartMapaCalor",
                data: JSON.stringify({ zp: zp, pe: pe }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (chartData) {

                    destroyHighChart(container);
                    highChartOptions();

                    var categories = [];
                    var h1 = [];
                    var h2 = [];
                    var h3 = [];
                    var h4 = [];
                    var h5 = [];
                    var h6 = [];
                    var h7 = [];
                    var h8 = [];
                    var h9 = [];
                    var h10 = [];
                    var h11 = [];
                    var h12 = [];
                    var h13 = [];
                    var h14 = [];
                    var h15 = [];
                    var h16 = [];
                    var h17 = [];
                    var h18 = [];
                    var h19 = [];
                    var h20 = [];
                    var h21 = [];
                    var h22 = [];
                    var h23 = [];
                    var h24 = [];
                    var h25 = [];
                    var h26 = [];
                    var h27 = [];
                    var h28 = [];
                    var h29 = [];
                    var h30 = [];

                    //console.log(response.d);
                    // Convertir los datos devueltos del servidor en el formato requerido por Highcharts
                    var rawData = chartData.d.slice(1);
                    $.each(rawData, function (index, value) {
                        categories.push(value[0]);
                        h1.push(parseInt(value[1]));
                        h2.push(parseInt(value[2]));
                        h3.push(parseInt(value[3]));
                        h4.push(parseInt(value[4]));
                        h5.push(parseInt(value[5]));
                        h6.push(parseInt(value[6]));
                        h7.push(parseInt(value[7]));
                        h8.push(parseInt(value[8]));
                        h9.push(parseInt(value[9]));
                        h10.push(parseInt(value[10]));
                        h11.push(parseInt(value[11]));
                        h12.push(parseInt(value[12]));
                        h13.push(parseInt(value[13]));
                        h14.push(parseInt(value[14]));
                        h15.push(parseInt(value[15]));
                        h16.push(parseInt(value[16]));
                        h17.push(parseInt(value[17]));
                        h18.push(parseInt(value[18]));
                        h19.push(parseInt(value[19]));
                        h20.push(parseInt(value[20]));
                        h21.push(parseInt(value[21]));
                        h22.push(parseInt(value[22]));
                        h23.push(parseInt(value[23]));
                        h24.push(parseInt(value[24]));
                        h25.push(parseInt(value[25]));
                        h26.push(parseInt(value[26]));
                        h27.push(parseInt(value[27]));
                        h28.push(parseInt(value[28]));
                        h29.push(parseInt(value[29]));
                        h30.push(parseInt(value[30]));

                    });

                    var lun = [h1[0], h2[0], h3[0], h4[0], h5[0], h6[0], h7[0], h8[0], h9[0], h10[0], h11[0], h12[0], h13[0], h14[0], h15[0], h16[0], h17[0], h18[0], h19[0], h20[0], h21[0], h22[0], h23[0], h24[0], h25[0], h26[0], h27[0], h28[0], h29[0], h30[0]];
                    var mar = [h1[1], h2[1], h3[1], h4[1], h5[1], h6[1], h7[1], h8[1], h9[1], h10[1], h11[1], h12[1], h13[1], h14[1], h15[1], h16[1], h17[1], h18[1], h19[1], h20[1], h21[1], h22[1], h23[1], h24[1], h25[1], h26[1], h27[1], h28[1], h29[1], h30[1]];
                    var mie = [h1[2], h2[2], h3[2], h4[2], h5[2], h6[2], h7[2], h8[2], h9[2], h10[2], h11[2], h12[2], h13[2], h14[2], h15[2], h16[2], h17[2], h18[2], h19[2], h20[2], h21[2], h22[2], h23[2], h24[2], h25[2], h26[2], h27[2], h28[2], h29[2], h30[2]];
                    var jue = [h1[3], h2[3], h3[3], h4[3], h5[3], h6[3], h7[3], h8[3], h9[3], h10[3], h11[3], h12[3], h13[3], h14[3], h15[3], h16[3], h17[3], h18[3], h19[3], h20[3], h21[3], h22[3], h23[3], h24[3], h25[3], h26[3], h27[3], h28[3], h29[3], h30[3]];
                    var vie = [h1[4], h2[4], h3[4], h4[4], h5[4], h6[4], h7[4], h8[4], h9[4], h10[4], h11[4], h12[4], h13[4], h14[4], h15[4], h16[4], h17[4], h18[4], h19[4], h20[4], h21[4], h22[4], h23[4], h24[4], h25[4], h26[4], h27[4], h28[4], h29[4], h30[4]];

                    let totalLun = lun.reduce((a, b) => a + b, 0);
                    let totalMar = mar.reduce((a, b) => a + b, 0);
                    let totalMie = mie.reduce((a, b) => a + b, 0);
                    let totalJue = jue.reduce((a, b) => a + b, 0);
                    let totalVie = vie.reduce((a, b) => a + b, 0);

                    //$("#totalLun").text(totalLun);
                    //$("#totalMar").text(totalMar);
                    //$("#totalMie").text(totalMie);
                    //$("#totalJue").text(totalJue);
                    //$("#totalVie").text(totalVie);

                    // Substring template helper for the responsive labels
                    Highcharts.Templating.helpers.substr = (s, from, length) =>
                        s.substr(from, length);

                    // Create the chart
                    Highcharts.chart(container, {

                        chart: {
                            type: 'heatmap',
                            marginTop: 40,
                            marginBottom: 80,
                            plotBorderWidth: 1,
                            height: 600,
                            events: {
                                load: function () {
                                    $("#totalLun").removeClass("spinner-border");
                                    $("#totalMar").removeClass("spinner-border");
                                    $("#totalMie").removeClass("spinner-border");
                                    $("#totalJue").removeClass("spinner-border");
                                    $("#totalVie").removeClass("spinner-border");

                                    $("#totalLun").text($("[id*='LabeltotalLun']").text());
                                    $("#totalMar").text($("[id*='LabeltotalMar']").text());
                                    $("#totalMie").text($("[id*='LabeltotalMie']").text());
                                    $("#totalJue").text($("[id*='LabeltotalJue']").text());
                                    $("#totalVie").text($("[id*='LabeltotalVie']").text());
                                }
                            }
                        },


                        title: {
                            text: 'Ocupación de grupos',
                            style: {
                                fontSize: '1em'
                            }
                        },

                        xAxis: {
                            categories: ['Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes'],
                            opposite: true,
                            labels: {
                                autoRotation: false
                            }
                        },

                        yAxis: {
                            categories: ['07:00-07:30', '07:30-08:00', '08:00-08:30', '08:30-09:00', '09:00-09:30', '09:30-10:00', '10:00-10:30', '10:30-11:00', '11:00-11:30', '11:30-12:00', '12:00-12:30', '12:30-13:00', '13:00-13:30', '13:30-14:00', '14:00-14:30', '14:30-15:00', '15:00-15:30', '15:30-16:00', '16:00-16:30', '16:30-17:00', '17:00-17:30', '17:30-18:00', '18:00-18:30', '18:30-19:00', '19:00-19:30', '19:30-20:00', '20:00-20:30', '20:30-21:00', '21:00-21:30', '21:30-22:00']
                            ,
                            title: null,
                            reversed: true
                        },

                        accessibility: {
                            point: {
                                descriptionFormat: '{(add index 1)}. ' +
                                    '{series.xAxis.categories.(x)} sales ' +
                                    '{series.yAxis.categories.(y)}, {value}.'
                            }
                        },

                        colorAxis: {
                            min: 0,
                            stops: [
                                [0, '#3FEA59'],
                                [0.5, '#fffbbc'],
                                [0.9, '#c4463a'],
                                [1, '#EA3F3F']
                            ]
                        },

                        legend: {
                            align: 'right',
                            layout: 'vertical',
                            margin: 0,
                            verticalAlign: 'top',
                            y: 50,
                            symbolHeight: 400
                        },

                        tooltip: {
                            format: '<b>{series.xAxis.categories.(point.x)}</b> <br>' +
                                '<b>{point.value}</b> grupos<br>' +
                                '<b>{series.yAxis.categories.(point.y)}</b>'
                        },
                        series: [{
                            name: 'Mapa de calor de la ocupación por día',
                            borderWidth: 1,
                            data: [
                                [0, 0, lun[0]], [0, 1, lun[1]], [0, 2, lun[2]], [0, 3, lun[3]], [0, 4, lun[4]], [0, 5, lun[5]], [0, 6, lun[6]], [0, 7, lun[7]], [0, 8, lun[8]], [0, 9, lun[9]], [0, 10, lun[10]], [0, 11, lun[11]], [0, 12, lun[12]], [0, 13, lun[13]], [0, 14, lun[14]], [0, 15, lun[15]], [0, 16, lun[16]], [0, 17, lun[17]], [0, 18, lun[18]], [0, 19, lun[19]], [0, 20, lun[20]], [0, 21, lun[21]], [0, 22, lun[22]], [0, 23, lun[23]], [0, 24, lun[24]], [0, 25, lun[25]], [0, 26, lun[26]], [0, 27, lun[27]], [0, 28, lun[28]], [0, 29, lun[29]],
                                [1, 0, mar[0]], [1, 1, mar[1]], [1, 2, mar[2]], [1, 3, mar[3]], [1, 4, mar[4]], [1, 5, mar[5]], [1, 6, mar[6]], [1, 7, mar[7]], [1, 8, mar[8]], [1, 9, mar[9]], [1, 10, mar[10]], [1, 11, mar[11]], [1, 12, mar[12]], [1, 13, mar[13]], [1, 14, mar[14]], [1, 15, mar[15]], [1, 16, mar[16]], [1, 17, mar[17]], [1, 18, mar[18]], [1, 19, mar[19]], [1, 20, mar[20]], [1, 21, mar[21]], [1, 22, mar[22]], [1, 23, mar[23]], [1, 24, mar[24]], [1, 25, mar[25]], [1, 26, mar[26]], [1, 27, mar[27]], [1, 28, mar[28]], [1, 29, mar[29]],
                                [2, 0, mie[0]], [2, 1, mie[1]], [2, 2, mie[2]], [2, 3, mie[3]], [2, 4, mie[4]], [2, 5, mie[5]], [2, 6, mie[6]], [2, 7, mie[7]], [2, 8, mie[8]], [2, 9, mie[9]], [2, 10, mie[10]], [2, 11, mie[11]], [2, 12, mie[12]], [2, 13, mie[13]], [2, 14, mie[14]], [2, 15, mie[15]], [2, 16, mie[16]], [2, 17, mie[17]], [2, 18, mie[18]], [2, 19, mie[19]], [2, 20, mie[20]], [2, 21, mie[21]], [2, 22, mie[22]], [2, 23, mie[23]], [2, 24, mie[24]], [2, 25, mie[25]], [2, 26, mie[26]], [2, 27, mie[27]], [2, 28, mie[28]], [2, 29, mie[29]],
                                [3, 0, jue[0]], [3, 1, jue[1]], [3, 2, jue[2]], [3, 3, jue[3]], [3, 4, jue[4]], [3, 5, jue[5]], [3, 6, jue[6]], [3, 7, jue[7]], [3, 8, jue[8]], [3, 9, jue[9]], [3, 10, jue[10]], [3, 11, jue[11]], [3, 12, jue[12]], [3, 13, jue[13]], [3, 14, jue[14]], [3, 15, jue[15]], [3, 16, jue[16]], [3, 17, jue[17]], [3, 18, jue[18]], [3, 19, jue[19]], [3, 20, jue[20]], [3, 21, jue[21]], [3, 22, jue[22]], [3, 23, jue[23]], [3, 24, jue[24]], [3, 25, jue[25]], [3, 26, jue[26]], [3, 27, jue[27]], [3, 28, jue[28]], [3, 29, jue[29]],
                                [4, 0, vie[0]], [4, 1, vie[1]], [4, 2, vie[2]], [4, 3, vie[3]], [4, 4, vie[4]], [4, 5, vie[5]], [4, 6, vie[6]], [4, 7, vie[7]], [4, 8, vie[8]], [4, 9, vie[9]], [4, 10, vie[10]], [4, 11, vie[11]], [4, 12, vie[12]], [4, 13, vie[13]], [4, 14, vie[14]], [4, 15, vie[15]], [4, 16, vie[16]], [4, 17, vie[17]], [4, 18, vie[18]], [4, 19, vie[19]], [4, 20, vie[20]], [4, 21, vie[21]], [4, 22, vie[22]], [4, 23, vie[23]], [4, 24, vie[24]], [4, 25, vie[25]], [4, 26, vie[26]], [4, 27, vie[27]], [4, 28, vie[28]], [4, 29, vie[29]]
                            ],
                            dataLabels: {
                                enabled: false,
                                color: '#000000'
                            }
                        }],

                        responsive: {
                            rules: [{
                                condition: {
                                    maxWidth: 500
                                },
                                chartOptions: {
                                    yAxis: {
                                        labels: {
                                            format: '{substr value 0 1}'
                                        }
                                    }
                                }
                            }]
                        }

                    });

                },
                error: function (chartData) {
                    //console.log(chartData);
                }
            });
        }

        function chartMapaCalorUA(zp, pe, ua, container) {

            $("#totalLunUA").addClass("spinner-border");
            $("#totalMarUA").addClass("spinner-border");
            $("#totalMieUA").addClass("spinner-border");
            $("#totalJueUA").addClass("spinner-border");
            $("#totalVieUA").addClass("spinner-border");

            $.ajax({
                cache: false,
                type: "POST",
                url: "Dashboard.aspx/GetChartMapaCalorUA",
                data: JSON.stringify({ zp: zp, pe: pe, ua: ua }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (chartData) {

                    destroyHighChart(container);
                    highChartOptions();

                    var categories = [];
                    var h1 = [];
                    var h2 = [];
                    var h3 = [];
                    var h4 = [];
                    var h5 = [];
                    var h6 = [];
                    var h7 = [];
                    var h8 = [];
                    var h9 = [];
                    var h10 = [];
                    var h11 = [];
                    var h12 = [];
                    var h13 = [];
                    var h14 = [];
                    var h15 = [];
                    var h16 = [];
                    var h17 = [];
                    var h18 = [];
                    var h19 = [];
                    var h20 = [];
                    var h21 = [];
                    var h22 = [];
                    var h23 = [];
                    var h24 = [];
                    var h25 = [];
                    var h26 = [];
                    var h27 = [];
                    var h28 = [];
                    var h29 = [];
                    var h30 = [];

                    var lun = [];
                    var mar = [];
                    var mie = [];
                    var jue = [];
                    var vie = [];

                    //console.log(response.d);
                    var rawData = chartData.d.slice(1);
                    $.each(rawData, function (index, value) {
                        categories.push(value[0]);
                        h1.push(parseInt(value[1]));
                        h2.push(parseInt(value[2]));
                        h3.push(parseInt(value[3]));
                        h4.push(parseInt(value[4]));
                        h5.push(parseInt(value[5]));
                        h6.push(parseInt(value[6]));
                        h7.push(parseInt(value[7]));
                        h8.push(parseInt(value[8]));
                        h9.push(parseInt(value[9]));
                        h10.push(parseInt(value[10]));
                        h11.push(parseInt(value[11]));
                        h12.push(parseInt(value[12]));
                        h13.push(parseInt(value[13]));
                        h14.push(parseInt(value[14]));
                        h15.push(parseInt(value[15]));
                        h16.push(parseInt(value[16]));
                        h17.push(parseInt(value[17]));
                        h18.push(parseInt(value[18]));
                        h19.push(parseInt(value[19]));
                        h20.push(parseInt(value[20]));
                        h21.push(parseInt(value[21]));
                        h22.push(parseInt(value[22]));
                        h23.push(parseInt(value[23]));
                        h24.push(parseInt(value[24]));
                        h25.push(parseInt(value[25]));
                        h26.push(parseInt(value[26]));
                        h27.push(parseInt(value[27]));
                        h28.push(parseInt(value[28]));
                        h29.push(parseInt(value[29]));
                        h30.push(parseInt(value[30]));

                    });

                    lun = [h1[0], h2[0], h3[0], h4[0], h5[0], h6[0], h7[0], h8[0], h9[0], h10[0], h11[0], h12[0], h13[0], h14[0], h15[0], h16[0], h17[0], h18[0], h19[0], h20[0], h21[0], h22[0], h23[0], h24[0], h25[0], h26[0], h27[0], h28[0], h29[0], h30[0]];
                    mar = [h1[1], h2[1], h3[1], h4[1], h5[1], h6[1], h7[1], h8[1], h9[1], h10[1], h11[1], h12[1], h13[1], h14[1], h15[1], h16[1], h17[1], h18[1], h19[1], h20[1], h21[1], h22[1], h23[1], h24[1], h25[1], h26[1], h27[1], h28[1], h29[1], h30[1]];
                    mie = [h1[2], h2[2], h3[2], h4[2], h5[2], h6[2], h7[2], h8[2], h9[2], h10[2], h11[2], h12[2], h13[2], h14[2], h15[2], h16[2], h17[2], h18[2], h19[2], h20[2], h21[2], h22[2], h23[2], h24[2], h25[2], h26[2], h27[2], h28[2], h29[2], h30[2]];
                    jue = [h1[3], h2[3], h3[3], h4[3], h5[3], h6[3], h7[3], h8[3], h9[3], h10[3], h11[3], h12[3], h13[3], h14[3], h15[3], h16[3], h17[3], h18[3], h19[3], h20[3], h21[3], h22[3], h23[3], h24[3], h25[3], h26[3], h27[3], h28[3], h29[3], h30[3]];
                    vie = [h1[4], h2[4], h3[4], h4[4], h5[4], h6[4], h7[4], h8[4], h9[4], h10[4], h11[4], h12[4], h13[4], h14[4], h15[4], h16[4], h17[4], h18[4], h19[4], h20[4], h21[4], h22[4], h23[4], h24[4], h25[4], h26[4], h27[4], h28[4], h29[4], h30[4]];

                    //let totalLun = lun.reduce((a, b) => a + b, 0);
                    //let totalMar = mar.reduce((a, b) => a + b, 0);
                    //let totalMie = mie.reduce((a, b) => a + b, 0);
                    //let totalJue = jue.reduce((a, b) => a + b, 0);
                    //let totalVie = vie.reduce((a, b) => a + b, 0);

                    // Substring template helper for the responsive labels
                    Highcharts.Templating.helpers.substr = (s, from, length) =>
                        s.substr(from, length);

                    Highcharts.chart(container, {

                        plotOptions: {
                            series: {
                                events: {
                                    click: function (event) {
                                        console.log('Series clicked:', this.name);
                                        console.log('Point clicked:', '{ series.xAxis.categories.(x) } sales' || event.point.category, event.point.y);
                                    }
                                }
                            }
                        },

                        chart: {
                            type: 'heatmap',
                            marginTop: 40,
                            marginBottom: 80,
                            plotBorderWidth: 1,
                            height: 600,
                            events: {
                                load: function () {
                                    $("#totalLunUA").removeClass("spinner-border");
                                    $("#totalMarUA").removeClass("spinner-border");
                                    $("#totalMieUA").removeClass("spinner-border");
                                    $("#totalJueUA").removeClass("spinner-border");
                                    $("#totalVieUA").removeClass("spinner-border");

                                    $("#totalLunUA").text($("[id*='LabeltotalLun']").text());
                                    $("#totalMarUA").text($("[id*='LabeltotalMar']").text());
                                    $("#totalMieUA").text($("[id*='LabeltotalMie']").text());
                                    $("#totalJueUA").text($("[id*='LabeltotalJue']").text());
                                    $("#totalVieUA").text($("[id*='LabeltotalVie']").text());
                                }
                            }
                        },


                        title: {
                            text: 'Ocupación de grupos',
                            style: {
                                fontSize: '1em'
                            }
                        },

                        xAxis: {
                            categories: ['Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes'],
                            opposite: true,
                            labels: {
                                autoRotation: false
                            }
                        },

                        yAxis: {
                            categories: ['07:00-07:30', '07:30-08:00', '08:00-08:30', '08:30-09:00', '09:00-09:30', '09:30-10:00', '10:00-10:30', '10:30-11:00', '11:00-11:30', '11:30-12:00', '12:00-12:30', '12:30-13:00', '13:00-13:30', '13:30-14:00', '14:00-14:30', '14:30-15:00', '15:00-15:30', '15:30-16:00', '16:00-16:30', '16:30-17:00', '17:00-17:30', '17:30-18:00', '18:00-18:30', '18:30-19:00', '19:00-19:30', '19:30-20:00', '20:00-20:30', '20:30-21:00', '21:00-21:30', '21:30-22:00']
                            ,
                            title: null,
                            reversed: true
                        },

                        accessibility: {
                            point: {
                                descriptionFormat: '{(add index 1)}. ' +
                                    '{series.xAxis.categories.(x)} sales ' +
                                    '{series.yAxis.categories.(y)}, {value}.'
                            }
                        },

                        colorAxis: {
                            min: 0,
                            stops: [
                                [0, '#3FEA59'],
                                [0.5, '#fffbbc'],
                                [0.9, '#c4463a'],
                                [1, '#EA3F3F']
                            ]
                        },

                        legend: {
                            align: 'right',
                            layout: 'vertical',
                            margin: 0,
                            verticalAlign: 'top',
                            y: 50,
                            symbolHeight: 400
                        },

                        tooltip: {
                            format: '<b>{series.xAxis.categories.(point.x)}</b> <br>' +
                                '<b>{point.value}</b> grupos<br>' +
                                '<b>{series.yAxis.categories.(point.y)}</b>'
                        },
                        series: [{
                            name: 'Mapa de calor de la ocupación por día',
                            borderWidth: 1,
                            data: [
                                [0, 0, lun[0]], [0, 1, lun[1]], [0, 2, lun[2]], [0, 3, lun[3]], [0, 4, lun[4]], [0, 5, lun[5]], [0, 6, lun[6]], [0, 7, lun[7]], [0, 8, lun[8]], [0, 9, lun[9]], [0, 10, lun[10]], [0, 11, lun[11]], [0, 12, lun[12]], [0, 13, lun[13]], [0, 14, lun[14]], [0, 15, lun[15]], [0, 16, lun[16]], [0, 17, lun[17]], [0, 18, lun[18]], [0, 19, lun[19]], [0, 20, lun[20]], [0, 21, lun[21]], [0, 22, lun[22]], [0, 23, lun[23]], [0, 24, lun[24]], [0, 25, lun[25]], [0, 26, lun[26]], [0, 27, lun[27]], [0, 28, lun[28]], [0, 29, lun[29]],
                                [1, 0, mar[0]], [1, 1, mar[1]], [1, 2, mar[2]], [1, 3, mar[3]], [1, 4, mar[4]], [1, 5, mar[5]], [1, 6, mar[6]], [1, 7, mar[7]], [1, 8, mar[8]], [1, 9, mar[9]], [1, 10, mar[10]], [1, 11, mar[11]], [1, 12, mar[12]], [1, 13, mar[13]], [1, 14, mar[14]], [1, 15, mar[15]], [1, 16, mar[16]], [1, 17, mar[17]], [1, 18, mar[18]], [1, 19, mar[19]], [1, 20, mar[20]], [1, 21, mar[21]], [1, 22, mar[22]], [1, 23, mar[23]], [1, 24, mar[24]], [1, 25, mar[25]], [1, 26, mar[26]], [1, 27, mar[27]], [1, 28, mar[28]], [1, 29, mar[29]],
                                [2, 0, mie[0]], [2, 1, mie[1]], [2, 2, mie[2]], [2, 3, mie[3]], [2, 4, mie[4]], [2, 5, mie[5]], [2, 6, mie[6]], [2, 7, mie[7]], [2, 8, mie[8]], [2, 9, mie[9]], [2, 10, mie[10]], [2, 11, mie[11]], [2, 12, mie[12]], [2, 13, mie[13]], [2, 14, mie[14]], [2, 15, mie[15]], [2, 16, mie[16]], [2, 17, mie[17]], [2, 18, mie[18]], [2, 19, mie[19]], [2, 20, mie[20]], [2, 21, mie[21]], [2, 22, mie[22]], [2, 23, mie[23]], [2, 24, mie[24]], [2, 25, mie[25]], [2, 26, mie[26]], [2, 27, mie[27]], [2, 28, mie[28]], [2, 29, mie[29]],
                                [3, 0, jue[0]], [3, 1, jue[1]], [3, 2, jue[2]], [3, 3, jue[3]], [3, 4, jue[4]], [3, 5, jue[5]], [3, 6, jue[6]], [3, 7, jue[7]], [3, 8, jue[8]], [3, 9, jue[9]], [3, 10, jue[10]], [3, 11, jue[11]], [3, 12, jue[12]], [3, 13, jue[13]], [3, 14, jue[14]], [3, 15, jue[15]], [3, 16, jue[16]], [3, 17, jue[17]], [3, 18, jue[18]], [3, 19, jue[19]], [3, 20, jue[20]], [3, 21, jue[21]], [3, 22, jue[22]], [3, 23, jue[23]], [3, 24, jue[24]], [3, 25, jue[25]], [3, 26, jue[26]], [3, 27, jue[27]], [3, 28, jue[28]], [3, 29, jue[29]],
                                [4, 0, vie[0]], [4, 1, vie[1]], [4, 2, vie[2]], [4, 3, vie[3]], [4, 4, vie[4]], [4, 5, vie[5]], [4, 6, vie[6]], [4, 7, vie[7]], [4, 8, vie[8]], [4, 9, vie[9]], [4, 10, vie[10]], [4, 11, vie[11]], [4, 12, vie[12]], [4, 13, vie[13]], [4, 14, vie[14]], [4, 15, vie[15]], [4, 16, vie[16]], [4, 17, vie[17]], [4, 18, vie[18]], [4, 19, vie[19]], [4, 20, vie[20]], [4, 21, vie[21]], [4, 22, vie[22]], [4, 23, vie[23]], [4, 24, vie[24]], [4, 25, vie[25]], [4, 26, vie[26]], [4, 27, vie[27]], [4, 28, vie[28]], [4, 29, vie[29]]
                            ],
                            dataLabels: {
                                enabled: false,
                                color: '#000000'
                            }
                        }],

                        responsive: {
                            rules: [{
                                condition: {
                                    maxWidth: 500
                                },
                                chartOptions: {
                                    yAxis: {
                                        labels: {
                                            format: '{substr value 0 1}'
                                        }
                                    }
                                }
                            }]
                        }
                    });

                },
                error: function (chartData) {
                    //console.log(chartData);
                }
            });
        }

        function chartColumnEventos(container) {

            var zp = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;

            $.ajax({
                cache: false,
                type: "POST",
                url: "Dashboard.aspx/chartColumnEventos_datos",
                data: JSON.stringify({ zp: zp }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (chartData) {

                    destroyHighChart(container);
                    highChartOptions();

                    var categories = [];
                    var data = [];

                    var rawData = chartData.d.slice(1);

                    $.each(rawData, function (index, value) {

                        categories.push(value[0]);
                        data.push(parseInt(value[1]));
                        
                    });

                    categories.pop();
                    let totalEv = data.pop();
                    $("[id*=LabelCardEventosRiesgo_total]").text(totalEv);

                    // Substring template helper for the responsive labels
                    Highcharts.Templating.helpers.substr = (s, from, length) =>
                        s.substr(from, length);

                    // Create the chart
                    Highcharts.chart(container, {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: ''
                        },
                        subtitle: {
                            text:
                                ''
                        },
                        xAxis: {
                            categories: categories,
                            crosshair: true,
                            accessibility: {
                                description: 'Countries'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: ''
                            }
                        },
                        tooltip: {
                            valueSuffix: ''
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: [
                            {
                                name: 'Eventos',
                                data: data
                            }
                        ]
                    });


                },
                error: function (chartData) {
                    //console.log(chartData);
                }
            });
        }

        function chartPieEventos(container) {

            var zp = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;

            $.ajax({
                cache: false,
                type: "POST",
                url: "Dashboard.aspx/chartPieEventos_datos",
                data: JSON.stringify({ zp: zp}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (chartData) {

                    destroyHighChart(container);
                    highChartOptions();

                    var categories = [];
                    var data = [];
                    var colors = [];
                    var dataFormat = [];

                    var rawData = chartData.d.slice(1);

                    $.each(rawData, function (index, value) {

                        categories.push(value[0]);
                        data.push(parseInt(value[1]));
                        colors.push(value[2]);

                        dataFormat.push({ name: value[0], y: value[1], color: value[2] });

                    });

                    dataFormat.pop();
                    categories.pop();
                    let totalEv = data.pop();

                    // Substring template helper for the responsive labels
                    Highcharts.Templating.helpers.substr = (s, from, length) =>
                        s.substr(from, length);

                    // Create the chart
                    Highcharts.chart(container, {
                        chart: {
                            type: 'pie',
                            custom: {},
                            events: {
                                render() {
                                    const chart = this,
                                        series = chart.series[0];
                                    let customLabel = chart.options.chart.custom.label;

                                    if (!customLabel) {
                                        customLabel = chart.options.chart.custom.label =
                                            chart.renderer.label(
                                                'Total<br/>' +
                                                '<strong>'+ totalEv +'</strong>'
                                            )
                                                .css({
                                                    color:
                                                        'var(--highcharts-neutral-color-100, #000)',
                                                    textAnchor: 'middle'
                                                })
                                                .add();
                                    }

                                    const x = series.center[0] + chart.plotLeft,
                                        y = series.center[1] + chart.plotTop -
                                            (customLabel.attr('height') / 2);

                                    customLabel.attr({
                                        x,
                                        y
                                    });
                                    // Set font size based on chart diameter
                                    customLabel.css({
                                        fontSize: `${series.center[2] / 12}px`
                                    });
                                }
                            }
                        },
                        accessibility: {
                            point: {
                                valueSuffix: '%'
                            }
                        },
                        title: {
                            text: 'Atención de eventos de riesgo'
                        },
                        subtitle: {
                            text: ''
                        },
                        tooltip: {
                            formatter: function () {
                                // 'this' refers to the point object in the formatter function
                                let value = this.y; // Raw value of the point
                                let percentage = Highcharts.numberFormat(this.percentage, 1) + ' %'; // Formatted percentage

                                return '<b>' + this.point.name + '</b><br/>' +
                                    'Eventos: ' + value + '<br/>' +
                                    'Porcentaje: ' + percentage;
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        plotOptions: {
                            series: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                borderRadius: 8,
                                dataLabels: [{
                                    enabled: true,
                                    distance: 20,
                                    format: '{point.name}'
                                }, {
                                    enabled: true,
                                    distance: -15,
                                    format: '{point.percentage:.0f}%',
                                    style: {
                                        fontSize: '0.9em'
                                    }
                                }],
                                showInLegend: true
                            }
                        },
                        series: [{
                            name: '',
                            colorByPoint: true,
                            innerSize: '75%',
                            data: dataFormat
                        }]
                    });


                },
                error: function (chartData) {
                    //console.log(chartData);
                }
            });
        }

        function chartBarNombramientos(container) {

            var zp = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;

            $.ajax({
                cache: false,
                type: "POST",
                url: "Dashboard.aspx/chartBarNombramientos_datos",
                data: JSON.stringify({ zp: zp}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (chartData) {

                    destroyHighChart(container);
                    highChartOptions();

                    var data = [];

                    var rawData = chartData.d.slice(1);

                    $.each(rawData, function (index, value) {

                        data.push(parseInt(value[0]));
                        data.push(parseInt(value[1]));

                    });

                    // Substring template helper for the responsive labels
                    Highcharts.Templating.helpers.substr = (s, from, length) =>
                        s.substr(from, length);

                    // Create the chart
                    Highcharts.chart(container, {
                        chart: {
                            type: 'bar'
                        },
                        title: {
                            text: '',
                            align: 'left'
                        },
                        xAxis: {
                            categories: [
                                'Nombramientos'
                            ],
                            visible: false
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: ''
                            }
                        },
                        legend: {
                            reversed: true
                        },
                        tooltip: {
                            useHTML: true,
                            formatter: function () {
                                return '<b>' + this.key + '</b><br/><br/><span style="color:' + this.series.color + '">\u25CF</span> ' +
                                    this.series.name + ': <b>' + this.y + '</b><br/><br/>Total: <b>' + this.point.stackTotal + '</b>';
                            }
                            /*    format: '<b>{key}</b><br/><br/>{series.color}{series.name}: {y}<br/>' +
                                    'Total: {point.stackTotal}'*/
                        },
                        plotOptions: {
                            series: {
                                stacking: 'normal',
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        exporting: { enabled: false },
                        series: [{
                            name: 'Interinato',
                            data: [data[1]],
                            color: "#FDD835"
                        }, {
                            name: 'Titulares',
                            data: [data[0]],
                            color: "#43A047"
                        }]
                    });


                },
                error: function (chartData) {
                    //console.log(chartData);
                }
            });
        }

        function chartPieNombramientos(container) {

            var zp = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;

            $.ajax({
                cache: false,
                type: "POST",
                url: "Dashboard.aspx/chartPieNombramientos_datos",
                data: JSON.stringify({ zp: zp }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (chartData) {

                    destroyHighChart(container);
                    highChartOptions();

                    var data = [];

                    var rawData = chartData.d.slice(1);

                    $.each(rawData, function (index, value) {
                        
                        data.push(parseInt(value[0]));
                        data.push(parseInt(value[1]));
                        data.push(parseInt(value[2]));

                    });

                    // Substring template helper for the responsive labels
                    Highcharts.Templating.helpers.substr = (s, from, length) =>
                        s.substr(from, length);

                    // Create the chart
                    Highcharts.chart(container, {
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie'
                        },
                        title: {
                            text: ''
                        },
                        tooltip: {
                            useHTML: true,
                            formatter: function () {
                                return '<b>' + this.series.name + '</b><br/><br/><span style="color:' + this.color + '">\u25CF</span> ' +
                                    this.key + ': <b>' + this.y + '</b>';
                            }
                        },
                        accessibility: {
                            point: {
                                valueSuffix: '%'
                            }
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: [{
                                    enabled: false,
                                    distance: 20
                                }, {
                                    enabled: true,
                                    distance: -20,
                                    format: '{point.percentage:.1f}%',
                                    style: {
                                        fontSize: '.7em',
                                        color: "#080808",
                                        textOutline: 'none',
                                        opacity: 0.7
                                    },
                                    filter: {
                                        operator: '>',
                                        property: 'percentage',
                                        value: 10
                                    }
                                }],
                                showInLegend: true
                            }
                        },
                        exporting: { enabled: false },
                        series: [{
                            type: 'pie',
                            name: 'Nombramientos',
                            data: [
                                {
                                    name: "Vigentes",
                                    y: data[0],
                                    color: "#43A047"
                                },
                                {
                                    name: 'Vencidos',
                                    y: data[1],
                                    sliced: true,
                                    selected: true,
                                    color: "#FDD835"
                                },
                                {
                                    name: 'Vacantes',
                                    y: data[2],
                                    color: "#E53935"
                                }
                            ]
                        }]
                    });


                },
                error: function (chartData) {
                    //console.log(chartData);
                }
            });

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
                    case 'collapseContenido_filtroDependencias':
                        $("[id*=HiddenFieldCollapseDependencias_selected]").val("0");
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
                    case 'collapseContenido_filtroDependencias':
                        $("[id*=HiddenFieldCollapseDependencias_selected]").val("1");
                        break;
                }
            });

            let collPlaSt = $("[id*=HiddenFieldCollapsePlan_selected]").val();
            let collEveSt = $("[id*=HiddenFieldCollapseEventos_selected]").val();
            let collDepSt = $("[id*=HiddenFieldCollapseDependencias_selected]").val();

            if (collPlaSt == "1") { ActivarCollapse("collapseContenido_filtroPlan"); }
            if (collEveSt == "1") { ActivarCollapse("collapseContenido_filtroEventos"); }
            if (collDepSt == "1") { ActivarCollapse("collapseContenido_filtroDependencias"); }

        }

        function ActivarCollapse(idCollapse) {

            var element = document.getElementById(idCollapse);
            var myCollapse = new bootstrap.Collapse(element);
            myCollapse.show();

        }

    </script>

     <%--scripts modals--%>
    <script>
        function ShowModalAddProgramAcad() {
            var myModal = document.getElementById('ModalUnidadesAcademicas');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function ShowModalGraficos() {
            var myModal = document.getElementById('modalGraficos');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }
    </script>

    <script>
        function mostrarGraficos() {
            // Obtén los valores de los dropdowns ASP.NET
            var clave = $("#<%=DropDownListUnidadAcademica.ClientID%>").val();
            var asignatura = $("#<%=DropDownListUnidadAprend.ClientID%>").val();

            var ddlPeriodo = $("#<%=DropDownListPeriodo.ClientID%>");
            var periodo = ddlPeriodo.length > 0 ? ddlPeriodo.val() : "";  // si no existe, manda vacío
                        
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/ObtenerDatosOcupabilidad",
                data: JSON.stringify({ clave: clave, asignatura: asignatura, periodo : periodo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var res = JSON.parse(response.d);
                    var categorias = res.Datos.map(x => x.Periodo);
                    var promedios = res.Datos.map(x => x.Promedio);
                    var grupos = res.Datos.map(x => x.Grupos);
                    var alumnos = res.Datos.map(x => x.Alumnos);

                    // 🔹 Umbrales de ±1σ  (promedio-(2*desviacion))
                    var limiteInferior = (res.Media - (2 * res.Desviacion)) < 1 ? 1 : res.Media - (2 * res.Desviacion);
                    var limiteSuperior = res.Media + (2 * res.Desviacion);

                    // 🔹 Transformar promedios en objetos con colores
                    var promediosColoreados = res.Datos.map(x => {
                        return {
                            y: x.Promedio,
                            color: Math.round(x.Promedio) < Math.round(limiteInferior) ? 'red' : '#007bff',
                            marker: {
                                radius: Math.round(x.Promedio) < Math.round(limiteInferior) ? 8.0 : 5.0
                            }
                        };
                    });

                    // 1. Histórico con banda ±σ
                    Highcharts.chart('container-line', {
                        chart: { type: 'line' },
                        title: { text: 'Histórico de capacidad por grupo' },
                        xAxis: { categories: categorias },
                        yAxis: { title: { text: 'Alumnos por grupo' } },
                        tooltip: {
                            shared: true,
                            formatter: function () {
                                if (this.points.some(p => p.series.name === 'Media histórica')) {
                                    return `
                    <b>Estadísticos:</b><br/>
                    Media = ${Math.round(res.Media)}<br/>
                    Varianza = ${Math.round(res.Varianza)}<br/>
                    Desviación estándar = ${Math.round(res.Desviacion)}<br/>                    
                    Rango esperado = [${Math.round(limiteInferior).toFixed(0)}, ${Math.round(limiteSuperior).toFixed(0)}]
                `;
                                } else {
                                    return this.points.map(p => `<b>${p.series.name}:</b> ${p.y}`).join('<br/>');
                                }
                            }
                        },
                        series: [{
                            name: 'Promedio por periodo',
                            data: promediosColoreados,
                            dataLabels: {
                                enabled: true
                            }
                        }, {
                            name: 'Media histórica',
                            type: 'line',
                            dashStyle: 'Dash',
                            data: Array(promedios.length).fill(Math.round(res.Media)),
                            marker: { enabled: false }
                        }, {
                            name: '+1σ',
                            type: 'line',
                            dashStyle: 'Dot',
                            data: Array(promedios.length).fill(Math.round(limiteSuperior)),
                            marker: { enabled: false },
                            color: 'green'
                        }, {
                            name: '-1σ',
                            type: 'line',
                            dashStyle: 'Dot',
                            data: Array(promedios.length).fill(Math.round(limiteInferior)),
                            marker: { enabled: false },
                            color: 'red'
                        }]
                    });

                    // 2. Histograma
                    var freq = {};
                    promedios.forEach(v => { freq[v] = (freq[v] || 0) + 1; });
                    var histData = Object.keys(freq).map(k => [parseFloat(k), freq[k]]);

                    Highcharts.chart('container-hist', {
                        chart: { type: 'column' },
                        title: { text: 'Histograma de capacidad por grupo' },
                        xAxis: { title: { text: 'Cupo promedio por grupo' } },
                        yAxis: { title: { text: 'Frecuencia' } },
                        series: [{
                            name: 'Frecuencia',
                            data: histData,
                            colorByPoint: true
                        }]
                    });

                    // 3. Dispersión alumnos vs grupos
                    //var scatterData = res.Datos.map(x => [x.Grupos, x.Alumnos]);
                    var scatterData = res.Datos.map(x => ({
                        x: x.Grupos,
                        y: x.Alumnos,
                        periodo: x.Periodo
                    }));

                    Highcharts.chart('container-scatter', {
                        chart: { type: 'scatter', zoomType: 'xy' },
                        //Diagrama de dispersión    N° de alumnos vs N° de grupos (para ver tendencia). 
                        title: { text: 'Núm de alumnos vs núm de grupos' },
                        xAxis: { title: { text: 'Número de grupos' } },
                        yAxis: { title: { text: 'Número de alumnos' } },
                         plotOptions: {
                            scatter: {
                                marker: {
                                    radius: 10.5,
                                    symbol: 'circle',
                                    states: {
                                        hover: {
                                            enabled: true,
                                            lineColor: 'rgb(100,100,100)'
                                        }
                                    }
                                },
                                states: {
                                    hover: {
                                        marker: {
                                            enabled: false
                                        }
                                    }
                                },
                                jitter: {
                                    x: 1,
                                    y: 1
                                }
                            }
                        },
                        tooltip: {
                            useHTML: true,
                            pointFormat: 'Núm. de grupos: <b>{point.x}</b> <br/> Núm. de alumnos: <b>{point.y}</b> <br/> Periodo: <b>{point.periodo}</b>'
                        },
                        series: [{
                            name: 'Relación alumnos vs grupos',
                            data: scatterData,
                            colorByPoint: true
                        }]
                    });

                    // Resumen estadístico
                    $("#resumen").html(
                        "Media: " + Math.round(res.Media) +
                        " | Varianza: " + Math.round(res.Varianza) +
                        " | Desviación estándar: " + Math.round(res.Desviacion)
                    );
                }
            });
        }
    </script>

</asp:Content>
