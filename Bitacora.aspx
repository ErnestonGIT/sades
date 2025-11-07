<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bitacora.aspx.cs" Inherits="Bitacora" EnableEventValidation = "false" Buffer="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="contentBitacora" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
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
                    <h1>Bitácora </h1>
                    <nav>
                        <ol class="breadcrumb">
                            <!-- <li class="breadcrumb-item"><a href="index">Inicio</a></li> -->
                            <li class="breadcrumb-item"><a href="Dashboard.aspx">Inicio</a></li>
                            <li class="breadcrumb-item active">Bitácora</li>
                            <li class="breadcrumb-item active"><asp:Label ID="LabelZP_name" runat="server" CssClass="breadcrumb"></asp:Label></li>
                        </ol>
                    </nav>
                </div><!-- End Page Title -->

                <asp:Label ID="LabelPerfil" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelZP" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelPE" runat="server" Text="" style="display:none;"></asp:Label>
                <asp:Label ID="LabelZPDesc" runat="server" Text="" style="display:none;"></asp:Label>
                
                <asp:HiddenField runat="server" ID="HiddenFieldPerfil_nivel"/>
                <asp:HiddenField runat="server" ID="HiddenFieldCollapseBitacora_selected" Value="1"/>

                <section class="section dashboard">

                    <!-- PanelInicial -->
                    <div id="divPanelInicial" runat="server" visible="false">
                        <div class="accordion" id="divAccordionPlan">

                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseContenido_bitacora" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                                Bitácora de acciones en el sistema
                              </button>
                            </h2>
                            <div id="collapseContenido_bitacora" class="accordion-collapse collapse">
                              <div class="accordion-body">
                                <div class="row-cols-1" style="margin-block:5px">
                                    <asp:LinkButton runat="server" ID="LinkButtonVaciar_bitacora" CssClass="btn btn-outline-danger" OnClick="LinkButtonVaciar_bitacora_Click">Vaciar bitácora</asp:LinkButton>
                                </div>
                                <div class="col-md-12 table-responsive">

                                    <asp:GridView ID="GridViewOffCanvasUnidadesAcademicas" runat="server"
                                        AutoGenerateColumns="False" AllowSorting="true"                                                                        
                                            CssClass="table table-bordered table-striped table-responsive" HeaderStyle-CssClass=" bg-gradient bg-primary-light text-gray-100 text-center"  
                                                PagerStyle-CssClass="pagination-ys LoadingOverlay"
                                                    PageSize="10" AllowPaging="true" DataSourceID="SqlDataSourceUAS" 
                                                        OnPageIndexChanging="GridViewOffCanvasUnidadesAcademicas_PageIndexChanging">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Núm" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCIÓN"/>
                                            <asp:BoundField DataField="IP" HeaderText="IP"/>
                                            <asp:BoundField DataField="FECHA" HeaderText="FECHA"/>
                                            <asp:BoundField DataField="HORA" HeaderText="HORA" ItemStyle-Width="20%"/>

                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="text-center">
                                                <asp:Label runat="server" ID="LabelOffCanvasUnidadesAcademicas_mensaje" Text="<br><br><br> No se encontraron registros <br><br><br>" CssClass="alert alert-light" Width="90%"></asp:Label>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                    <asp:SqlDataSource ID="SqlDataSourceUAS" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" 
                                            SelectCommand="select ID_USER, ID_PERFIL, TIPO_MOVIMIENTO, DESCRIPCION, IP, FORMAT(FECHA_MOVIMIENTO, 'dd-MM-yyyy') FECHA,
	                                                        FORMAT(FECHA_MOVIMIENTO, 'yyyy-MM-dd') ORDEN,
	                                                        STRING_AGG(FORMAT(FECHA_MOVIMIENTO, 'HH:mm'), CHAR(10)) HORA
                                                        from BITACORA
                                                        group by ID_USER, ID_PERFIL, TIPO_MOVIMIENTO, DESCRIPCION, IP,FORMAT(FECHA_MOVIMIENTO, 'dd-MM-yyyy'), FORMAT(FECHA_MOVIMIENTO, 'yyyy-MM-dd')
                                                        order by CAST(FORMAT(FECHA_MOVIMIENTO, 'yyyy-MM-dd') AS date) desc">
                                    </asp:SqlDataSource>
                                </div>


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

            var myModal = document.getElementById(idModal);
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();

            switch (idModal) {

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

        function validarCollapse() {

            $('.accordion-collapse').on('hide.bs.collapse', function (e) {

                let idCollapse = $(e.target).attr("ID");

                switch (idCollapse) {
                    case 'collapseContenido_bitacora':
                        $("[id*=HiddenFieldCollapseBitacora_selected]").val("0");
                        break;
                }

            });

            $('.accordion-collapse').on('show.bs.collapse', function (e) {

                let idCollapse = $(e.target).attr("ID");

                switch (idCollapse) {
                    case 'collapseContenido_bitacora':
                        $("[id*=HiddenFieldCollapseBitacora_selected]").val("1");
                        break;
                }
            });

            let collPlaSt = $("[id*=HiddenFieldCollapseBitacora_selected]").val();

            if (collPlaSt == "1") { ActivarCollapse("collapseContenido_bitacora"); }

        }

        function ActivarCollapse(idCollapse) {

            var element = document.getElementById(idCollapse);
            var myCollapse = new bootstrap.Collapse(element);
            myCollapse.show();

        }

    </script>
</asp:Content>
