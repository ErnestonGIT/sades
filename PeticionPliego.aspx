<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PeticionPliego.aspx.cs" Inherits="PeticionPliego" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--<%@ Register Src="~/ModalConfirm.ascx" TagPrefix="uc" TagName="ModalConfirm" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="public/css/StyleModules.css" rel="stylesheet" />

    <main id="main1" class="main">
        <div class="pagetitle">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <%--<h1>Catálogo de formación académica del docente</h1>--%>
                    <h1>Peticiones</h1>
                    <nav>
                        <ol class="breadcrumb">
                            <!-- <li class="breadcrumb-item"><a href="index">Inicio</a></li> -->
                            <li class="breadcrumb-item active">
                                <asp:Label ID="LabelDependencia" runat="server" Text=""></asp:Label>
                            </li>
                        </ol>
                    </nav>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- End Page Title -->
        <section class="section">
            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="row mt-3">
                                <!-- Pills Tabs -->
                                <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link active" id="addPeticion" data-bs-toggle="pill" data-bs-target="#pills-addPeticion" type="button" role="tab" aria-controls="pills-addPeticion"
                                            aria-selected="true">
                                            Petición</button>
                                    </li>
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link" id="addRespuesta" data-bs-toggle="pill" data-bs-target="#pills-addRespuesta" type="button" role="tab" aria-controls="pills-addRespuesta"
                                            aria-selected="false">
                                            Respuesta</button>
                                    </li>
                                </ul>
                                <div class="tab-content pt-2" id="myTabContentPliego">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LblTabSelection" runat="server" Text="" CssClass="hideGrid"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--Tab peticion--%>
                                    <div class="tab-pane fade show active" id="pills-addPeticion" role="tabpanel" aria-labelledby="home-tab">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>                                                
                                                <asp:Label ID="LblIdPliego" runat="server" Text="" CssClass="hideGrid"></asp:Label>
                                                <asp:Label ID="LblFolioPliego" runat="server" Text="" CssClass="hideGrid"></asp:Label>

                                                <div class="card shadow rounded-4 p-4">
                                                    <h4 class="mb-3 text-primary">
                                                        <i class="far fa-file-alt"></i> Registro de petición
                                                                <%--<i class="bi bi-file-earmark-text"></i>Registro de Petición--%>
                                                    </h4>

                                                    <!-- Selección de pliego existente o nuevo -->
                                                    <div class="row">
                                                        <small class="fw-semibold small text-center">Indique si ya existe un archivo de <b>'Pliego'</b> cargado relacionado a la petición</small>
                                                        <br />
                                                        <div class="row mb-3 text-center">
                                                            <div class="form-check form-check-inline">
                                                                <asp:RadioButtonList ID="RadioButtonListPliego" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                    CssClass="form-check form-check-inline FormatRadioButtonList" OnSelectedIndexChanged="RadioButtonListPliego_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Si" Value="existente"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="nuevo"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona pliego existente -->
                                                    <div class="row mb-3" id="divPliegoExistente" runat="server" visible="false">
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                                            <asp:Label ID="LabelPliego" runat="server" Text="Seleccione pliego:" CssClass="form-label fw-bold"></asp:Label>

                                                            <asp:LinkButton ID="LinkButtonSelectPliego" runat="server" CssClass="color-btn small"
                                                                Text='<i class="fas fa-folder fa-2x fa-fw"></i>' OnClick="LinkButtonSelectPliego_Click">
                                                            </asp:LinkButton>
                                                        </div>
                                                        <%--<asp:DropDownList ID="DropDownListPliego" runat="server" CssClass="form-select"></asp:DropDownList>--%>
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6" id="divPliegoSelect" runat="server" visible="false">
                                                            <div class="row">
                                                                <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2">
                                                                    <%--Contenedor de chips--%>
                                                                    <asp:Label ID="Label1" runat="server" Text="Pliego:" CssClass="form-label fw-bold"></asp:Label>
                                                                </div>
                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4">
                                                                    <div id="MainContentDivsAddEspec" runat="server">
                                                                        <!-- Aquí se agregarán los divs (chips = badges)  -->
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona nuevo pliego -->
                                                    <div class="mb-3" id="divNuevoPliego" runat="server" visible="false">
                                                        <asp:Label ID="LabelArchivo" runat="server" Text="Subir archivo del pliego (.pdf):" CssClass="form-label fw-bold"></asp:Label>
                                                        <asp:FileUpload ID="FileUploadPliego" runat="server" CssClass="form-control" />
                                                    </div>

                                                    <hr />

                                                    <!-- Datos de la petición -->
                                                    <div class="row">
                                                        <div class="col-md-6 mb-3">
                                                            <asp:Label ID="LabelCategoria" runat="server" Text="Categoría:" CssClass="form-label fw-bold"></asp:Label>
                                                            <asp:DropDownList ID="DropDownListCategoria" runat="server" CssClass="form-select" AutoPostBack="true" DataSourceID="SqlDataSourceDdlCategoriaPet"
                                                                DataTextField="DESCRIPCION_CAT_PETICION" DataValueField="ID_CAT_PETICION" OnDataBound="DDLCategoriaPeticion_DataBound">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSourceDdlCategoriaPet" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                SelectCommand="SELECT ID_CAT_PETICION, DESCRIPCION_CAT_PETICION
FROM CAT_CATEGORIA_PETICION"></asp:SqlDataSource>
                                                        </div>
                                                        <div class="col-md-6 mb-3">
                                                            <asp:Label ID="LabelFechaPeticion" runat="server" Text="Fecha de petición:" CssClass="form-label fw-bold"></asp:Label>
                                                            <asp:TextBox ID="TextBoxFechaPeticion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="mb-3">
                                                        <asp:Label ID="LabelPeticion" runat="server" Text="Petición:" CssClass="form-label fw-bold"></asp:Label>
                                                        <asp:TextBox ID="TextBoxPeticion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                    </div>

                                                    <!-- Botón guardar -->
                                                    <div class="text-end">
                                                        <asp:Button ID="ButtonGuardar" runat="server" CssClass="btn btn-primary px-4" Text="Guardar Petición"
                                                            OnClick="ButtonGuardar_Click" />
                                                    </div>

                                                    <!-- Mensaje de confirmación -->
                                                    <asp:Label ID="LabelMensaje" runat="server" CssClass="text-success fw-bold mt-3"></asp:Label>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="ButtonGuardar" />

                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <%--Tap respuesta--%>
                                    <div class="tab-pane fade" id="pills-addRespuesta" role="tabpanel" aria-labelledby="profile-tab">
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>                                                
                                                  <asp:Label ID="LblIdPliegoResp" runat="server" Text="" CssClass="hideGrid"></asp:Label>
                                                <asp:Label ID="LblFolioPliegoResp" runat="server" Text="" CssClass="hideGrid"></asp:Label>
                                                <asp:Label ID="LabelIdPeticionGridResp" runat="server" Text="" CssClass="hideGrid"></asp:Label>
                                                <asp:Label ID="LabelIdDocumentoResp" runat="server" Text="" CssClass="hideGrid"></asp:Label>

                                                <div class="card shadow rounded-4 p-4">
                                                    <h4 class="mb-3 text-primary">
                                                        <i class="far fa-file-alt"></i> Registro de respuesta
                                                                <%--<i class="bi bi-file-earmark-text"></i>Registro de Petición--%>
                                                    </h4>

                                                    <!-- Seleccionar pliego -->
                                                    <div class="row mb-3" id="divPliegoRespuesta" runat="server" visible="true">
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                                            <asp:Label ID="Label4" runat="server" Text="Seleccione pliego:" CssClass="form-label fw-bold"></asp:Label>

                                                            <asp:LinkButton ID="LinkButtonSelectPliegoResp" runat="server" CssClass="color-btn small"
                                                                Text='<i class="fas fa-folder fa-2x fa-fw"></i>' OnClick="LinkButtonSelectPliegoResp_Click">
                                                            </asp:LinkButton>
                                                        </div>
                                                        <%--<asp:DropDownList ID="DropDownListPliego" runat="server" CssClass="form-select"></asp:DropDownList>--%>
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6" id="divPliegoSelectRespuesta" runat="server" visible="false">
                                                            <div class="row">
                                                                <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2">
                                                                    <%--Contenedor de chips--%>
                                                                    <asp:Label ID="Label5" runat="server" Text="Pliego:" CssClass="form-label fw-bold"></asp:Label>
                                                                </div>
                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4">
                                                                    <div id="MainContentDivsAddRespuesta" runat="server">
                                                                        <!-- Aquí se agregarán los divs (chips = badges)  -->
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Seleccionar petición -->
                                                    <div id="divGridPeticiones" class="mb-3" runat="server" visible="false">
                                                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 table-responsive">
                                                            <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder fw-bold">Peticiones.</h6>
                                                            <%--gridview peticiones del pliego--%>
                                                            <asp:GridView ID="GridViewPeticionResp" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGetPeticionResp"
                                                                CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                                                PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                                                <%--OnRowDataBound="GridViewLaboralT_RowDataBound">--%>
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID_PLIEGO" HeaderText="ID_PLIEGO" SortExpression="ID_PLIEGO" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                                                    <asp:BoundField DataField="ID_CAT_PETICION" HeaderText="ID_CAT_PETICION" SortExpression="ID_CAT_PETICION" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                                                    <asp:BoundField DataField="DESCRIPCION_CAT_PETICION" HeaderText="Categoría" SortExpression="DESCRIPCION_CAT_PETICION" />
                                                                    <asp:BoundField DataField="ID_PETICION" HeaderText="ID_PETICION" SortExpression="ID_PETICION" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                                                    <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha de petición" SortExpression="FECHA_PETICION" ItemStyle-HorizontalAlign="Center"/>
                                                                    <asp:BoundField DataField="DESC_PETICION" HeaderText="Petición" SortExpression="DESC_PETICION" />
                                                                    <asp:BoundField DataField="FECHA_RESP_PETICION" HeaderText="Fecha de respuesta" SortExpression="FECHA_RESP_PETICION" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="DESC_RESP_PETICION" HeaderText="Respuesta" SortExpression="DESC_RESP_PETICION" />
                                                                    <%--<asp:TemplateField HeaderText="Documento" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <span class="" tabindex="0" data-bs-toggle="popover" data-bs-placement="left" data-bs-custom-class="custom-popover"
                                                        data-bs-trigger="hover focus" data-bs-content="Ver RUAA">
                                                        <asp:LinkButton ID="LinkButtonVerPliego" runat="server" CssClass="color-btn mb-2" Text='<i class="fas fa-file-pdf fa-2x fa-fw"></i>'
                                                            OnClick="LinkButtonPliegoPDF_Click">                                                                                        
                                                        </asp:LinkButton>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="..." ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="ButtonSelectPeticion" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-dark btn-sm label-"
                                                                                OnClick="ButtonSelectPeticion_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div class="text-center">
                                                                        <asp:Label runat="server" ID="mensaje" Text="No hay pliegos existentes en la unidad académica" CssClass="alert alert-danger" Width="100%"></asp:Label>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                            <%------------------------------------datasource del gridview para peticiones pliego----------------------------------%>
                                                            <asp:SqlDataSource ID="SqlDataSourceGetPeticionResp" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                SelectCommand="SELECT  pE.ID_PLIEGO,pe.ID_CAT_PETICION,cp.DESCRIPCION_CAT_PETICION,pe.ID_PETICION,
		FORMAT(pe.FECHA_PETICION, 'dd/MM/yyyy') as FECHA_PETICION,pe.DESC_PETICION,FORMAT(pe.FECHA_RESP_PETICION, 'dd/MM/yyyy') as FECHA_RESP_PETICION,PE.DESC_RESP_PETICION
	FROM PETICIONES_POR_UA pe, CAT_CATEGORIA_PETICION cp
	WHERE pe.ID_CAT_PETICION = cp.ID_CAT_PETICION
		AND pe.ID_PLIEGO = @pliego">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="LblIdPliegoResp" Name="pliego" PropertyName="Text" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <%--Campos seleccionados--%>
                                                    <div class="row mb-3">                                                        
                                                        <span class="col-md-2 col-form-label">Petición:</span>
                                                        <div class="col-md-4 col-form-label">
                                                            <asp:Label ID="LabelPeticionGridResp" runat="server" CssClass="fw-bold" Text=""></asp:Label>
                                                        </div>
                                                        <span class="col-md-2 col-form-label">Categoría:</span>
                                                        <div class="col-md-4 col-form-label">
                                                            <asp:Label ID="LabelCategoriaGridResp" runat="server" CssClass="fw-bold" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                    <!-- Campos de respuesta -->
                                                    <div class="row mb-3">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="Fecha de respuesta:" CssClass="form-label fw-bold"></asp:Label>
                                                            <%--<label class="fw-bold">Fecha de Respuesta</label>--%>
                                                            <asp:TextBox ID="TextBoxFechaRespuesta" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="mb-3">
                                                           <asp:Label ID="Label10" runat="server" Text="Respuesta:" CssClass="form-label fw-bold"></asp:Label>
                                                        <%--<label class="fw-bold">Respuesta</label>--%>
                                                        <asp:TextBox ID="TextBoxRespuesta" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                                                    </div>

                                                    
                                                    <!-- Selección de documento existente o nuevo relacionado  -->
                                                    <div class="row">
                                                        <small class="fw-semibold small text-center">Indique si ya existe un archivo cargado relacionado a la respuesta</small>
                                                        <br />
                                                        <div class="row mb-3 text-center">
                                                            <div class="form-check form-check-inline">
                                                                <asp:RadioButtonList ID="RadioButtonListRespuesta" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                    CssClass="form-check form-check-inline FormatRadioButtonList" OnSelectedIndexChanged="RadioButtonListRespuesta_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Si" Value="existente"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="nuevo"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <!-- Si selecciona Documento existente -->
                                                    <div class="row mb-3" id="divDocRespExistente" runat="server" visible="false">
                                                        <div class="col-xl-5 col-lg-5 col-md-5 col-sm-5 col-5">
                                                            <asp:Label ID="LabelDocRespuesta" runat="server" Text="Seleccione el archivo:" CssClass="form-label fw-bold"></asp:Label>

                                                            <asp:LinkButton ID="LinkButtonSelectDocResp" runat="server" CssClass="color-btn small"
                                                                Text='<i class="fas fa-folder fa-2x fa-fw"></i>' OnClick="LinkButtonSelectDocResp_Click">
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div class="col-xl-7 col-lg-7 col-md-7 col-sm-7 col-7" id="divDocRespSelect" runat="server" visible="false">
                                                            <div class="row">
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-3">
                                                                    <%--Contenedor de chips--%>
                                                                    <asp:Label ID="Label7" runat="server" Text="Documento: " CssClass="form-label fw-bold"></asp:Label>

                                                                </div>
                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4">
                                                                    <asp:Label ID="Label6" runat="server" Text="" CssClass="md-chip specific mb-1 mx-1">Seleccionado <i class="far fa-check-square"></i></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona nuevo pliego -->
                                                    <div class="mb-3" id="divNuevoDocResp" runat="server" visible="false">
                                                        <asp:Label ID="LabelArchivoRespuesta" runat="server" Text="Subir archivo de respuesta:" CssClass="form-label fw-bold"></asp:Label>
                                                        <asp:FileUpload ID="FileUploadRespuesta" runat="server" CssClass="form-control" />
                                                    </div>

                                                                                                        
                                                    <div class="text-end">
                                                        <asp:Button ID="ButtonGuardarRespuesta" runat="server" Text="Guardar Respuesta" CssClass="btn btn-success" 
                                                        OnClick="ButtonGuardarRespuesta_Click" />
                                                    </div>

                                                    <div class="mt-3">
                                                        <asp:Label ID="LabelMensajeResp" runat="server" CssClass="fw-bold"></asp:Label>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                             <Triggers>
                                                <asp:PostBackTrigger ControlID="ButtonGuardarRespuesta" />

                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </main>


    <%-- Modal select pliego --%>
    <div class="modal fade" id="modalSelectPliego" tabindex="-1" role="dialog" data-bs-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<h5 class="modal-title" id="exampleModalLabel2">Eliminar archivo de RUAA</h5>--%>
                    <%--<i class="far fa-check-square fa-2x" style="color: #198754;"></i>--%>
                    <i class="far fa-check-square fa-lg fa-fw" style="color: #012970;"></i>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 table-responsive">
                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder fw-bold">Pliegos.</h6>
                                    <%--gridview laboral T docentes--%>
                                    <asp:GridView ID="GridViewPliego" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                        <%--OnRowDataBound="GridViewLaboralT_RowDataBound">--%>
                                        <Columns>
                                            <asp:BoundField DataField="ID_PLIEGO" HeaderText="ID_PLIEGO" SortExpression="ID_PLIEGO" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                            <asp:BoundField DataField="FOLIO_PLIEGO" HeaderText="Folio pliego" SortExpression="FOLIO_PLIEGO" />
                                            <asp:TemplateField HeaderText="Documento" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <span class="" tabindex="0" data-bs-toggle="popover" data-bs-placement="left" data-bs-custom-class="custom-popover"
                                                        data-bs-trigger="hover focus" data-bs-content="Ver RUAA">
                                                        <asp:LinkButton ID="LinkButtonVerPliego" runat="server" CssClass="color-btn mb-2" Text='<i class="fas fa-file-pdf fa-2x fa-fw"></i>'
                                                            OnClick="LinkButtonPliegoPDF_Click">                                                                                        
                                                        </asp:LinkButton>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="..." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="ButtonSelectPliego" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-dark btn-sm label-"
                                                        OnClick="ButtonSelectPliego_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="text-center">
                                                <asp:Label runat="server" ID="mensaje" Text="No hay pliegos existentes en la unidad académica" CssClass="alert alert-danger" Width="100%"></asp:Label>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer text-center">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="ButtonCancelAddPlan" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                                data-bs-dismiss="modal" />
                            <%--<asp:Button ID="ButtonAceptAddPlan" runat="server" Text="Aceptar" CssClass="btn btn-primary hideGrid"
                                OnClick="ButtonAceptAddPlan_Click" />
                            <button type="button" class="btn btn-primary" onclick="verificarPlanEstudio()">Aceptar</button>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal select doc resp --%>
    <div class="modal fade" id="modalSelectDocRespuesta" tabindex="-1" role="dialog" data-bs-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<h5 class="modal-title" id="exampleModalLabel2">Eliminar archivo de RUAA</h5>--%>
                    <%--<i class="far fa-check-square fa-2x" style="color: #198754;"></i>--%>
                    <i class="far fa-check-square fa-lg fa-fw" style="color: #012970;"></i>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 table-responsive">
                                    <h6 class="h6 mb-2 mt-1 text-dark font-weight-bolder fw-bold">Documentos de respuesta.</h6>
                                    <%--gridview laboral T docentes--%>
                                    <asp:GridView ID="GridViewDocRespuesta" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                        <%--OnRowDataBound="GridViewLaboralT_RowDataBound">--%>
                                        <Columns>
                                            <asp:BoundField DataField="ID_DOCUMENTO" HeaderText="ID_DOCUMENTO" SortExpression="ID_DOCUMENTO" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                            <asp:BoundField DataField="TIPO_DOCUMENTO" HeaderText="TIPO_DOCUMENTO" SortExpression="TIPO_DOCUMENTO" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                            <asp:BoundField DataField="ID_PLIEGO" HeaderText="ID_PLIEGO" SortExpression="ID_PLIEGO" ItemStyle-CssClass="hideGrid" HeaderStyle-CssClass="hideGrid" />
                                            <asp:BoundField DataField="FECHA_SUBIDA" HeaderText="Fecha" SortExpression="FECHA_SUBIDA" />
                                            <asp:TemplateField HeaderText="Documento" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <span class="" tabindex="0" data-bs-toggle="popover" data-bs-placement="left" data-bs-custom-class="custom-popover"
                                                        data-bs-trigger="hover focus" data-bs-content="Ver RUAA">
                                                        <asp:LinkButton ID="LinkButtonVerDocResp" runat="server" CssClass="color-btn mb-2" Text='<i class="fas fa-file-pdf fa-2x fa-fw"></i>'
                                                            OnClick="LinkButtonVerDocRespPDF_Click">                                                                                        
                                                        </asp:LinkButton>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="..." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="ButtonSelectDocRespuesta" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-dark btn-sm label-"
                                                        OnClick="ButtonSelectDocRespuesta_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="text-center">
                                                <asp:Label runat="server" ID="mensaje" Text="No hay documentos de respuesta relacionados al pliego" CssClass="alert alert-danger" Width="100%"></asp:Label>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer text-center">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button1" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                                data-bs-dismiss="modal" />
                            <%--<asp:Button ID="ButtonAceptAddPlan" runat="server" Text="Aceptar" CssClass="btn btn-primary hideGrid"
                                OnClick="ButtonAceptAddPlan_Click" />
                            <button type="button" class="btn btn-primary" onclick="verificarPlanEstudio()">Aceptar</button>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--Modal Ver PDF--%>
    <div class="modal fade  bd-example-modal-lg" id="modalVerPDF" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel1">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LabelVisualizar" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="ratio ratio-16x9">
                                <iframe id="verPDF" runat="server" type="application/pdf" loading="lazy" allow="fullscreen"></iframe>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <div class=" text-center">
                        <button class="btn btn-secondary" type="button" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--scripts--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>


    <%--Modals--%>
    <script>
        function ShowModalSelectPliego() {
            var myModal = document.getElementById('modalSelectPliego');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function HideModalSelectPliego() {
            var myModal = document.getElementById('modalSelectPliego');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.hide();
        }

        function ShowModalSelectDocRespuesta() {
            var myModal = document.getElementById('modalSelectDocRespuesta');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function HideModalSelectDocRespuesta() {
            var myModal = document.getElementById('modalSelectDocRespuesta');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.hide();
        }

        function ShowModalVerPDF() {

            var myModal = document.getElementById('modalVerPDF');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }
    </script>

<%--    <script type="text/javascript">
        function inicializarEventosTabs() {
            var lblTipo = document.getElementById("<%= LblTabSelection.ClientID %>");
            if (!lblTipo) return;

            // Cuando se hace clic en "Petición"
            document.getElementById("addPeticion").addEventListener("click", function () {
                lblTipo.innerText = "1";
            });

            // Cuando se hace clic en "Respuesta"
            document.getElementById("addRespuesta").addEventListener("click", function () {
                lblTipo.innerText = "2";
            });
        }

        // Se ejecuta al cargar la página
        document.addEventListener("DOMContentLoaded", inicializarEventosTabs);

        // Si hay UpdatePanel, se reancla tras cada postback parcial
        if (typeof (Sys) !== "undefined") {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(inicializarEventosTabs);
        }
    </script>--%>

 <%--   <script type="text/javascript">
    function inicializarEventosTabs() {
        var lblTipo = document.getElementById("<%= LblTabSelection.ClientID %>");
        var hdnTipo = document.getElementById("<%= hdnTipo.ClientID %>");

        if (!lblTipo || !hdnTipo) return;

        // 1️⃣ Al cargar la página, establece la primera tab como activa
        lblTipo.innerText = "1";
        hdnTipo.value = "1";

        // 2️⃣ Evento al hacer clic en "Petición"
        document.getElementById("addPeticion").addEventListener("click", function () {
            lblTipo.innerText = "1";
            hdnTipo.value = "1";
        });

        // 3️⃣ Evento al hacer clic en "Respuesta"
        document.getElementById("addRespuesta").addEventListener("click", function () {
            lblTipo.innerText = "2";
            hdnTipo.value = "2";
        });
    }

    // Ejecutar al cargar la página
    document.addEventListener("DOMContentLoaded", inicializarEventosTabs);

    // Si hay UpdatePanel, volver a inicializar después de un postback parcial
    if (typeof (Sys) !== "undefined") {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(inicializarEventosTabs);
    }
</script>--%>




</asp:Content>
