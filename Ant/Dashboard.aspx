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
                <asp:HiddenField runat="server" ID="HiddenFieldCollapsePlan_selected" Value="1"/>
                <asp:HiddenField runat="server" ID="HiddenFieldGruposTotales_mapaGeneral" Value="1"/>
                <asp:HiddenField runat="server" ID="HiddenFieldFiltro_ua"/>

                <asp:Label id="LabeltotalLun" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalMar" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalMie" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalJue" runat="server" style="display:none;"></asp:Label>
                <asp:Label id="LabeltotalVie" runat="server" style="display:none;"></asp:Label>

                <section class="section dashboard">

                    <!-- PanelInicial -->
                    <div id="divPanelInicial" runat="server" visible="false">
                        <div class="accordion" id="divAccordionPlan">

                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseContenido_filtroPlan" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                                Filtro de búsqueda
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
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-12 mb-2">
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
                                                                </div>
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
                        AND ID_MODALIDAD = @idModalidadd">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListModalidad" Name="idModalidadd" PropertyName="SelectedValue" />
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

                                                            <div id="divCheckVigenciaFuncion" runat="server" class="row mb-3" visible="true">
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
		(
			(SELECT SUM(t1.ALUMNOS) 
				FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
						FROM HISTORIAL_CARGA_DOCENTE
						WHERE CLAVE_ZP = hcd.CLAVE_ZP 
							AND MODALIDAD = hcd.MODALIDAD
							AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
							AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR
					) AS t1
			) / COUNT (DISTINCT SECUENCIA)
		) as PROMEDIO 
	FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
	WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
		AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
		AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		AND hcd.CLAVE_ZP = @UA
		AND hcd.MODALIDAD = @modalidad
		AND hcd.ID_ASIGNATURA = @unidadAprend
		AND pe.ID_PLAN_ESTUDIO = @planEstudio
	GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA
	ORDER BY hcd.PERIODO_ESCOLAR">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListModalidad" Name="modalidad" PropertyName="SelectedValue" />
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
		(
			(SELECT SUM(t1.ALUMNOS) 
				FROM (SELECT DISTINCT SECUENCIA, ALUMNOS 
						FROM HISTORIAL_CARGA_DOCENTE
						WHERE CLAVE_ZP = hcd.CLAVE_ZP 
							AND MODALIDAD = hcd.MODALIDAD
							AND ID_ASIGNATURA = hcd.ID_ASIGNATURA
							AND PERIODO_ESCOLAR = hcd.PERIODO_ESCOLAR
					) AS t1
			) / COUNT (DISTINCT SECUENCIA)
		) as PROMEDIO 
	FROM HISTORIAL_CARGA_DOCENTE hcd, PLANES_ESTUDIO pe
	WHERE hcd.CLAVE_ZP = pe.CLAVE_ZP
		AND hcd.ID_ASIGNATURA = pe.ID_ASIGNATURA
		AND hcd.ID_ESPECIALIDAD = pe.ID_ESPECIALIDAD
		AND hcd.CLAVE_ZP = @UA
		AND hcd.MODALIDAD = @modalidad
		AND hcd.ID_ASIGNATURA = @unidadAprend
		AND pe.ID_PLAN_ESTUDIO = @planEstudio
                                                AND PERIODO_ESCOLAR like  '%' + @periodo + ''
	GROUP BY hcd.PERIODO_ESCOLAR, hcd.CLAVE_ZP, hcd.MODALIDAD, hcd.ID_ASIGNATURA
	ORDER BY hcd.PERIODO_ESCOLAR">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="DropDownListUnidadAcademica" Name="UA" PropertyName="SelectedValue" />
                                                                            <asp:ControlParameter ControlID="DropDownListModalidad" Name="modalidad" PropertyName="SelectedValue" />
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

                </section>

            </main>
        </ContentTemplate>
    </asp:UpdatePanel>

     <!-- Modal Filtro Detalle estadisticos-->
    <div class="modal fade" id="ModalUnidadesAcademicas" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-lg">
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
                                    <span class="text-success small pt-1 fw-bold">Modalidad: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_modalidad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Programa académico: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_programaAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Plan de estudio: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_planEst" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Unidad de aprendizaje: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_unidadAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Periodos: </span><span
                                        class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalDetail_periodoPar" runat="server"></asp:Label></span>
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalGraficos" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
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
                                    <span class="text-success small pt-1 fw-bold">Modalidad: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_modalidad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Programa académico: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_programaAcad" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Plan de estudio: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_planEst" runat="server"></asp:Label></span>
                                    <br />
                                    <span class="text-success small pt-1 fw-bold">Unidad de aprendizaje: </span><span
                                                class="text-blak medium pt-1 fw-bold"><asp:Label ID="LabelModalGrafico_unidadAcad" runat="server"></asp:Label></span>
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
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </ContentTemplate>
            </asp:UpdatePanel>

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

            var labelZP = document.getElementById('<%= LabelZP.ClientID %>').innerHTML;
            var labelPE = document.getElementById('<%= LabelPE.ClientID %>').innerHTML;
            var ZPname = document.getElementById('<%= LabelZPDesc.ClientID %>').innerHTML;
            var perfil = document.getElementById('<% = LabelPerfil.ClientID%>').innerHTML.toString();

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
            
            validarCollapse();
            infoToolStart();
            enableLoadingOverlay();
            habilitarSelect2();
            hideLoadingOverlay();            
        
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
        }

        function habilitarSelect2() {

            $.fn.select2.defaults.set('language', 'es');
            $("[class*=form-select]").select2({
                theme: 'bootstrap-5'
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

        function validarCollapse() {

            $('.accordion-collapse').on('hide.bs.collapse', function (e) {

                let idCollapse = $(e.target).attr("ID");

                switch (idCollapse) {
                    case 'collapseContenido_filtroPlan':
                        $("[id*=HiddenFieldCollapsePlan_selected]").val("0");
                        break;
                }

            });

            $('.accordion-collapse').on('show.bs.collapse', function (e) {

                let idCollapse = $(e.target).attr("ID");

                switch (idCollapse) {
                    case 'collapseContenido_filtroPlan':
                        $("[id*=HiddenFieldCollapsePlan_selected]").val("1");
                        break;
                }
            });

            let collPlaSt = $("[id*=HiddenFieldCollapsePlan_selected]").val();

            if (collPlaSt == "1") { ActivarCollapse("collapseContenido_filtroPlan"); }

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
            var modalidad = $("#<%=DropDownListModalidad.ClientID%>").val();
            var asignatura = $("#<%=DropDownListUnidadAprend.ClientID%>").val();

            var ddlPeriodo = $("#<%=DropDownListPeriodo.ClientID%>");
            var periodo = ddlPeriodo.length > 0 ? ddlPeriodo.val() : "";  // si no existe, manda vacío
                        
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/ObtenerDatosOcupabilidad",
                data: JSON.stringify({ clave: clave, modalidad: modalidad, asignatura: asignatura, periodo : periodo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var res = JSON.parse(response.d);
                    var categorias = res.Datos.map(x => x.Periodo);
                    var promedios = res.Datos.map(x => x.Promedio);
                    var grupos = res.Datos.map(x => x.Grupos);
                    var alumnos = res.Datos.map(x => x.Alumnos);

                    // 🔹 Umbrales de ±1σ  (promedio-(2*desviacion))
                    var limiteInferior = res.Media - (2 * res.Desviacion);
                    var limiteSuperior = res.Media + (2 * res.Desviacion);

                    // 🔹 Transformar promedios en objetos con colores
                    var promediosColoreados = res.Datos.map(x => {
                        return {
                            y: x.Promedio,
                            color: x.Promedio < limiteInferior ? 'red' : '#007bff',
                            marker: {
                                radius: x.Promedio < limiteInferior ? 8.0 : 5.0
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
                    Media = ${res.Media}<br/>
                    Varianza = ${res.Varianza}<br/>
                    Desviación estándar = ${res.Desviacion}<br/>                    
                    Rango esperado = [${(res.Media - (2 * res.Desviacion)).toFixed(2)}, ${(res.Media + (2 * res.Desviacion)).toFixed(2)}]
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
                            data: Array(promedios.length).fill(res.Media),
                            marker: { enabled: false }
                        }, {
                            name: '+1σ',
                            type: 'line',
                            dashStyle: 'Dot',
                            data: Array(promedios.length).fill(limiteSuperior),
                            marker: { enabled: false },
                            color: 'green'
                        }, {
                            name: '-1σ',
                            type: 'line',
                            dashStyle: 'Dot',
                            data: Array(promedios.length).fill(limiteInferior),
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
                        "Media: " + res.Media +
                        " | Varianza: " + res.Varianza +
                        " | Desviación estándar: " + res.Desviacion
                    );
                }
            });
        }
    </script>

</asp:Content>
