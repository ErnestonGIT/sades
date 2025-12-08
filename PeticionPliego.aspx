<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PeticionPliego.aspx.cs" Inherits="PeticionPliego" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/ModalConfirm.ascx" TagPrefix="uc" TagName="ModalConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="public/css/StyleModules.css" rel="stylesheet" />

    <main id="main1" class="main">
        <div class="pagetitle">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <h1>Peticiones</h1>
                    <nav>
                        <ol class="breadcrumb">
                            <!-- <li class="breadcrumb-item"><a href="index">Inicio</a></li> -->
                            <li class="breadcrumb-item active">
                                <asp:Label ID="LabelDependencia" runat="server"></asp:Label>
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
                    <div class="card rounded-2">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="LabelClaveZP" runat="server" CssClass="d-none"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row mt-2">
                                <%--Guardar valor de Tab activa--%>
                                <asp:HiddenField ID="HiddenActiveTab" runat="server" />

                                <div class="row">
                                    <div class="col-xl-10 col-lg-10 col-md-10 col-sm-10 col-10">
                                        <!-- Pills Tabs -->
                                        <ul class="nav nav-pills mb-2 mt-1" id="pills-tab" role="tablist">
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
                                    </div>
                                    <%--Ver peticiones por pliego--%>
                                    <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 align-content-center">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="top" data-bs-custom-class="custom-popover"
                                                    data-bs-trigger="hover focus" data-bs-content="Ver peticiones">
                                                    <asp:LinkButton ID="LinkButtonVerPeticion" runat="server" CssClass="color-btn fw-semibold small md-chip specific mb-1 mx-1"
                                                        Text='<i class="fas fa-eye fa-lg fa-fw"></i> Peticiones'
                                                        OnClick="LinkButtonVerPeticion_Click">
                                                    </asp:LinkButton>
                                                </span>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="tab-content pt-2" id="myTabContentPliego">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LblTabSelection" runat="server" CssClass="d-none"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--Tab peticion--%>
                                    <div class="tab-pane fade show active" id="pills-addPeticion" role="tabpanel" aria-labelledby="home-tab">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="LblIdPliego" runat="server" CssClass="d-none"></asp:Label>
                                                <asp:Label ID="LblFolioPliego" runat="server" CssClass="d-none"></asp:Label>

                                                <div class="card shadow rounded-4 p-4">
                                                    <h4 class="mb-3 text-primary">
                                                        <i class="far fa-file-alt"></i>&nbsp;Registro de petición
                                                    </h4>

                                                    <!-- Mensaje de confirmación -->
                                                    <asp:Label ID="LabelMensajePeticion" runat="server" CssClass="fw-bold mt-3"></asp:Label>

                                                    <asp:ValidationSummary runat="server" ID="ValidationSummary1" CssClass="alert alert-danger alert-dismissible fade show fw-bold small"
                                                        DisplayMode="BulletList" ValidationGroup="AddPeticionPliego" HeaderText='<i class="fas fa-exclamation-triangle fa-fw"></i> Por favor verifique lo siguiente:'
                                                        ShowMessageBox="false" ShowSummary="True" EnableClientScript="true" />

                                                    <!-- Selección de pliego existente o nuevo -->
                                                    <div class="row">
                                                        <small class="fw-semibold small text-center">Indique si ya existe un archivo de <b>'Pliego'</b> cargado, relacionado a la petición</small>
                                                        <br />
                                                        <div class="row mb-3 text-center">
                                                            <div class="form-check form-check-inline">
                                                                <asp:RadioButtonList ID="RadioButtonListPliego" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                    CssClass="form-check form-check-inline FormatRadioButtonList fw-bold" OnSelectedIndexChanged="RadioButtonListPliego_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Si" Value="existente"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="nuevo"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="RFVRadioButtonListPliego" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="RadioButtonListPliego"
                                                                ErrorMessage="Seleccione una de las opciones de 'Si' o 'No' para el archivo del pliego."
                                                                Display="None" SetFocusOnError="True" ValidationGroup="AddPeticionPliego" />
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona pliego existente -->
                                                    <div class="row mb-3" id="divPliegoExistente" runat="server" visible="false">
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                                            <asp:Label ID="LabelPliego" runat="server" Text="Seleccione el pliego:" CssClass="form-label fw-bold"></asp:Label>

                                                            <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="right" data-bs-custom-class="custom-popover"
                                                                data-bs-trigger="hover focus" data-bs-content="De clic, para seleccionar un pliego"><%-- data-bs-title="De clic, para seleccionar un pliego" data-bs-content="Ver pliego" --%>
                                                                <asp:LinkButton ID="LinkButtonSelectPliego" runat="server" CssClass="color-btn small"
                                                                    Text='<i class="fas fa-folder fa-2x fa-fw"></i>' OnClick="LinkButtonSelectPliego_Click">
                                                                </asp:LinkButton>
                                                            </span>
                                                        </div>
                                                        <%--chips--%>
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6" id="divPliegoSelect" runat="server" visible="false">
                                                            <div class="row mx-auto">
                                                                <div class="col-xl-3 col-lg-2 col-md-2 col-sm-2 col-2">
                                                                    <%--Contenedor de chips--%>
                                                                    <asp:Label ID="Label1" runat="server" Text="Pliego:" CssClass="form-label fw-bold"></asp:Label>
                                                                </div>
                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4">
                                                                    <div id="MainContentDivsAddEspec" runat="server">
                                                                        <!-- Aquí se agregarán los divs (chips = badges)  -->
                                                                    </div>

                                                                </div>
                                                                <div class="col-xl-1 col-lg-1 col-md-1 col-sm-1 col-1">
                                                                    <asp:LinkButton ID="LinkButtonSelectPetiPliegoPDF" runat="server" CssClass="color-btn1 small" Text='<i class="far fa-file-pdf fa-2x fa-fw fa-pull-left" ></i>'
                                                                        OnClick="LinkButtonSelectPetiPliegoPDF_Click">
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona nuevo pliego -->
                                                    <div class="mb-3" id="divNuevoPliego" runat="server" visible="false">
                                                        <asp:Label ID="LabelArchivo" runat="server" Text="Subir archivo del pliego (.pdf):" CssClass="form-label fw-bold"></asp:Label>
                                                        <asp:FileUpload ID="FileUploadPliego" runat="server" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator ID="RFVFileUploadPliego" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="FileUploadPliego"
                                                            ErrorMessage="Cargue el archivo PDF del pliego."
                                                            Display="None" SetFocusOnError="True" ValidationGroup="AddPeticionPliego" />
                                                    </div>

                                                    <hr />

                                                    <!-- Datos de la petición -->
                                                    <div class="row">
                                                        <div class="col-md-6 mb-3">
                                                            <asp:Label ID="LabelCategoria" runat="server" Text="Categoría:" CssClass="form-label fw-bold"></asp:Label>
                                                            <asp:DropDownList ID="DropDownListCategoria" runat="server" CssClass="form-select" AutoPostBack="false" DataSourceID="SqlDataSourceDdlCategoriaPet"
                                                                DataTextField="DESCRIPCION_CAT_PETICION" DataValueField="ID_CAT_PETICION" OnDataBound="DDLCategoriaPeticion_DataBound">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSourceDdlCategoriaPet" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                                SelectCommand="SELECT ID_CAT_PETICION, DESCRIPCION_CAT_PETICION
FROM CAT_CATEGORIA_PETICION"></asp:SqlDataSource>
                                                            <asp:RequiredFieldValidator ID="RFVDropDownListCategoria" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="DropDownListCategoria"
                                                                ErrorMessage="Seleccione una opción de categoría."
                                                                Display="None" SetFocusOnError="True" ValidationGroup="AddPeticionPliego" />
                                                        </div>
                                                        <div class="col-md-6 mb-3">
                                                            <asp:Label ID="LabelFechaPeticion" runat="server" Text="Fecha de petición:" CssClass="form-label fw-bold"></asp:Label>
                                                            <asp:TextBox ID="TextBoxFechaPeticion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFVTextBoxFechaPeticion" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="TextBoxFechaPeticion"
                                                                ErrorMessage="Ingrese la fecha de la petición."
                                                                Display="None" SetFocusOnError="True" ValidationGroup="AddPeticionPliego" />
                                                        </div>
                                                    </div>

                                                    <div class="mb-3">
                                                        <asp:Label ID="LabelPeticion" runat="server" Text="Petición:" CssClass="form-label fw-bold"></asp:Label>
                                                        <asp:TextBox ID="TextBoxPeticion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFVTextBoxPeticion" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="TextBoxPeticion"
                                                            ErrorMessage="Ingrese la petición."
                                                            Display="None" SetFocusOnError="True" ValidationGroup="AddPeticionPliego" />
                                                    </div>

                                                    <!-- Botón guardar -->
                                                    <div class="text-end">
                                                        <asp:Button ID="ButtonGuardar" runat="server" CssClass="btn btn-primary px-4" Text="Guardar Petición"
                                                            OnClick="ButtonGuardar_Click" ValidationGroup="AddPeticionPliego" />
                                                       <%-- <asp:Button ID="ButtonLimpiar" runat="server" CssClass="btn btn-danger px-4" Text="Limpiar"
                                                            />--%>
                                                    </div>

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
                                                <asp:Label ID="LblIdPliegoResp" runat="server" CssClass="d-none"></asp:Label>
                                                <asp:Label ID="LblFolioPliegoResp" runat="server" CssClass="d-none"></asp:Label>
                                                <asp:Label ID="LabelIdPeticionGridResp" runat="server" CssClass="d-none"></asp:Label>
                                                <asp:Label ID="LabelIdDocumentoResp" runat="server" CssClass="d-none"></asp:Label>
                                                <asp:Label ID="LabelFechaPeticionGridResp" runat="server" CssClass="d-none"></asp:Label>

                                                <div class="card shadow rounded-4 p-4">
                                                    <h4 class="mb-3 text-primary">
                                                        <i class="far fa-file-alt"></i>&nbsp;Registro de respuesta
                                                                <%--<i class="bi bi-file-earmark-text"></i>Registro de Petición--%>
                                                    </h4>

                                                    <div class="mt-3">
                                                        <asp:Label ID="LabelMensajeResp" runat="server" CssClass="fw-bold"></asp:Label>
                                                    </div>
                                                    <asp:ValidationSummary runat="server" ID="ValidationSummary2" CssClass="alert alert-danger alert-dismissible fade show fw-bold small"
                                                        DisplayMode="BulletList" ValidationGroup="AddRespuestaPliego" HeaderText='<i class="fas fa-exclamation-triangle fa-fw"></i> Por favor verifique lo siguiente:'
                                                        ShowMessageBox="false" ShowSummary="True" EnableClientScript="true" />

                                                    <!-- Seleccionar pliego -->
                                                    <div class="row mb-3">
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                                            <asp:Label ID="Label4" runat="server" Text="Seleccione el pliego:" CssClass="form-label fw-bold"></asp:Label>

                                                            <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="right" data-bs-custom-class="custom-popover"
                                                                data-bs-trigger="hover focus" data-bs-content="De clic, para seleccionar un pliego">
                                                                <asp:LinkButton ID="LinkButtonSelectPliegoResp" runat="server" CssClass="color-btn small"
                                                                    Text='<i class="fas fa-folder fa-2x fa-fw"></i>' OnClick="LinkButtonSelectPliegoResp_Click">
                                                                </asp:LinkButton>
                                                            </span>
                                                        </div>

                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6" id="divPliegoSelectRespuesta" runat="server" visible="false">
                                                            <div class="row mx-auto">
                                                                <div class="col-xl-3 col-lg-2 col-md-2 col-sm-2 col-2">
                                                                    <%--Contenedor de chips--%>
                                                                    <asp:Label ID="Label5" runat="server" Text="Pliego:" CssClass="form-label fw-bold"></asp:Label>
                                                                </div>
                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4">
                                                                    <div id="MainContentDivsAddRespuesta" runat="server">
                                                                        <!-- Aquí se agregarán los divs (chips = badges)  -->
                                                                    </div>
                                                                </div>
                                                                <div class="col-xl-1 col-lg-1 col-md-1 col-sm-1 col-1">
                                                                    <asp:LinkButton ID="LinkButtonSelectRespPliegoPDF" runat="server" CssClass="color-btn1 small" Text='<i class="far fa-file-pdf fa-2x fa-fw fa-pull-left" ></i>'
                                                                        OnClick="LinkButtonSelectRespPliegoPDF_Click">
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Tabla de Selección de petición -->
                                                    <div id="divGridPeticiones" class="mb-3" runat="server" visible="false">
                                                        <div class="table-responsive">
                                                            <h6 class="h6 mb-2 mt-1 text-dark fw-bold">Peticiones.</h6>

                                                            <asp:GridView ID="GridViewPeticionResp" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGetPeticionResp"
                                                                CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                                                PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;"
                                                                OnRowDataBound="GridViewPeticionResp_RowDataBound">
                                                                <Columns>
                                                                    <%--Columnas ocultas--%> 
                                                                    <asp:BoundField DataField="ID_PLIEGO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                    <asp:BoundField DataField="ID_CAT_PETICION" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                    <asp:BoundField DataField="ID_PETICION" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                    <%--Columnas visibles--%> 
                                                                    <asp:BoundField DataField="DESCRIPCION_CAT_PETICION" HeaderText="Categoría" SortExpression="DESCRIPCION_CAT_PETICION" />
                                                                    <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha de petición" SortExpression="FECHA_PETICION" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="DESC_PETICION" HeaderText="Petición" SortExpression="DESC_PETICION" />
                                                                    <asp:BoundField DataField="FECHA_RESP_PETICION" HeaderText="Fecha de respuesta" SortExpression="FECHA_RESP_PETICION" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="DESC_RESP_PETICION" HeaderText="Respuesta" SortExpression="DESC_RESP_PETICION" />

                                                                    <asp:TemplateField HeaderText="..." ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="ButtonSelectPeticion" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-dark btn-sm"
                                                                                OnClick="ButtonSelectPeticion_Click" />

                                                                            <asp:Label ID="LabelPeticionConResp" runat="server" Text="" CssClass="small fw-bold btn-outline-secondary" Visible="false"></asp:Label>
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
	FROM PETICIONES pe, CAT_CATEGORIA_PETICION cp
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
                                                            <asp:Label ID="LabelPeticionGridResp" runat="server" CssClass="fw-bold"></asp:Label>
                                                        </div>
                                                        <span class="col-md-2 col-form-label">Categoría:</span>
                                                        <div class="col-md-4 col-form-label">
                                                            <asp:Label ID="LabelCategoriaGridResp" runat="server" CssClass="fw-bold"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <!-- Campos de respuesta -->
                                                    <div class="row mb-3">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="Fecha de respuesta:" CssClass="form-label fw-bold"></asp:Label>
                                                            <%--<label class="fw-bold">Fecha de Respuesta</label>--%>
                                                            <asp:TextBox ID="TextBoxFechaRespuesta" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFVTextBoxFechaRespuesta" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="TextBoxFechaRespuesta"
                                                                ErrorMessage="Ingrese la fecha de la respuesta."
                                                                Display="None" SetFocusOnError="True" ValidationGroup="AddRespuestaPliego" />
                                                        </div>
                                                    </div>

                                                    <div class="mb-3">
                                                        <asp:Label ID="Label10" runat="server" Text="Respuesta:" CssClass="form-label fw-bold"></asp:Label>
                                                        <%--<label class="fw-bold">Respuesta</label>--%>
                                                        <asp:TextBox ID="TextBoxRespuesta" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                                                         <asp:RequiredFieldValidator ID="RFVTextBoxRespuesta" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="TextBoxRespuesta"
                                                            ErrorMessage="Ingrese la respuesta."
                                                            Display="None" SetFocusOnError="True" ValidationGroup="AddRespuestaPliego" />
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
                                                            <asp:RequiredFieldValidator ID="RFVRadioButtonListRespuesta" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="RadioButtonListRespuesta"
                                                                ErrorMessage="Seleccione una de las opciones de 'Si' o 'No' para el archivo de respuesta."
                                                                Display="None" SetFocusOnError="True" ValidationGroup="AddRespuestaPliego" />
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona Documento existente -->
                                                    <div class="row mb-3" id="divDocRespExistente" runat="server" visible="false">
                                                        <div class="col-xl-5 col-lg-5 col-md-5 col-sm-5 col-5">
                                                            <asp:Label ID="LabelDocRespuesta" runat="server" Text="Seleccione el archivo:" CssClass="form-label fw-bold"></asp:Label>

                                                            <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="right" data-bs-custom-class="custom-popover"
                                                                data-bs-trigger="hover focus" data-bs-content="De clic, para seleccionar el archivo">
                                                                <asp:LinkButton ID="LinkButtonSelectDocResp" runat="server" CssClass="color-btn small"
                                                                    Text='<i class="fas fa-folder fa-2x fa-fw"></i>' OnClick="LinkButtonSelectDocResp_Click">
                                                                </asp:LinkButton>
                                                            </span>
                                                        </div>
                                                        <div class="col-xl-7 col-lg-7 col-md-7 col-sm-7 col-7" id="divDocRespSelect" runat="server" visible="false">
                                                            <div class="row mx-auto">
                                                                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-3">
                                                                    <%--Contenedor de chips--%>
                                                                    <asp:Label ID="Label7" runat="server" Text="Documento: " CssClass="form-label fw-bold"></asp:Label>

                                                                </div>
                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4">
                                                                    <asp:Label ID="Label6" runat="server" Text="" CssClass="md-chip specific mb-1 mx-1 fw-bold">Seleccionado <i class="far fa-check-square text-success"></i></asp:Label>
                                                                </div>
                                                                <div class="col-xl-1 col-lg-1 col-md-1 col-sm-1 col-1">
                                                                    <asp:LinkButton ID="LinkButtonSelectRespDocPDF" runat="server" CssClass="color-btn1 small" Text='<i class="far fa-file-pdf fa-2x fa-fw fa-pull-left" ></i>'
                                                                        OnClick="LinkButtonSelectRespDocPDF_Click">
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Si selecciona nuevo pliego -->
                                                    <div class="mb-3" id="divNuevoDocResp" runat="server" visible="false">
                                                        <asp:Label ID="LabelArchivoRespuesta" runat="server" Text="Subir archivo de respuesta:" CssClass="form-label fw-bold"></asp:Label>
                                                        <asp:FileUpload ID="FileUploadRespuesta" runat="server" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator ID="RFVFileUploadRespuesta" runat="server" CssClass="text-danger small fw-bold" ControlToValidate="FileUploadRespuesta"
                                                            ErrorMessage="Cargue el archivo PDF de la respuesta."
                                                            Display="None" SetFocusOnError="True" ValidationGroup="AddRespuestaPliego" />
                                                    </div>

                                                    <div class="text-end">
                                                        <asp:Button ID="ButtonGuardarRespuesta" runat="server" Text="Guardar Respuesta" CssClass="btn btn-success"
                                                            OnClick="ButtonGuardarRespuesta_Click" ValidationGroup="AddRespuestaPliego" />
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

    <%-------------MODALS---------------%>
    <%-- Modal de confirmación --%>
    <uc:ModalConfirm runat="server" ID="modalConfirm" />

    <%-- Modal select pliego --%>
    <div class="modal fade" id="modalSelectPliego" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<i class="far fa-check-square fa-2x" style="color: #198754;"></i>--%>
                    <i class="far fa-check-square fa-lg fa-fw" style="color: #012970;"></i>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                        <ContentTemplate>

                             <!-- Tabla Plegos -->
                            <div class="row">
                                <div class="table-responsive">
                                    <h6 class="h6 mb-2 mt-1 text-dark fw-bold">Pliegos.</h6>

                                    <asp:GridView ID="GridViewPliego" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                        <Columns>
                                            <%--Columnas ocultas--%> 
                                            <asp:BoundField DataField="ID_PLIEGO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <%--Columnas visibles--%> 
                                            <asp:BoundField DataField="FOLIO_PLIEGO" HeaderText="Folio pliego" SortExpression="FOLIO_PLIEGO" />
                                            <%--Documento--%> 
                                            <asp:TemplateField HeaderText="Documento" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="left" data-bs-custom-class="custom-popover"
                                                        data-bs-trigger="hover focus" data-bs-content="Ver pliego">
                                                        <asp:LinkButton ID="LinkButtonVerPliego" runat="server" CssClass="color-btn mb-2" Text='<i class="fas fa-file-pdf fa-2x fa-fw"></i>'
                                                            OnClick="LinkButtonPliegoPDF_Click">                                                                                        
                                                        </asp:LinkButton>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="..." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="ButtonSelectPliego" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-dark btn-sm"
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
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="ButtonCancelAddPlan" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                                data-bs-dismiss="modal" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal select doc resp --%>
    <div class="modal fade" id="modalSelectDocRespuesta" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<i class="far fa-check-square fa-2x" style="color: #198754;"></i>--%>
                    <i class="far fa-check-square fa-lg fa-fw" style="color: #012970;"></i>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>

                            <!-- Tabla Doc de Respuestas -->
                            <div class="row">
                                <div class="table-responsive">
                                    <h6 class="h6 mb-2 mt-1 text-dark fw-bold">Documentos de respuesta.</h6>

                                    <asp:GridView ID="GridViewDocRespuesta" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                        <Columns>
                                            <%--Columnas ocultas--%> 
                                            <asp:BoundField DataField="ID_DOCUMENTO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="TIPO_DOCUMENTO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="ID_PLIEGO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <%--Columnas visibles--%> 
                                            <asp:BoundField DataField="FECHA_SUBIDA" HeaderText="Fecha" SortExpression="FECHA_SUBIDA" />
                                            <%--Documento--%> 
                                            <asp:TemplateField HeaderText="Documento" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="left" data-bs-custom-class="custom-popover"
                                                        data-bs-trigger="hover focus" data-bs-content="Ver documento de respuesta">
                                                        <asp:LinkButton ID="LinkButtonVerDocResp" runat="server" CssClass="color-btn mb-2" Text='<i class="fas fa-file-pdf fa-2x fa-fw"></i>'
                                                            OnClick="LinkButtonVerDocRespPDF_Click">                                                                                        
                                                        </asp:LinkButton>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="..." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="ButtonSelectDocRespuesta" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-dark btn-sm"
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
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button1" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                                data-bs-dismiss="modal" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--Modal Ver PDF--%>
    <div class="modal fade" id="modalVerPDF" tabindex="-1" aria-labelledby="exampleModalLabel1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel1">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LabelVisualizar" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
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
                        <button class="btn btn-secondary" type="button" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--visualizar peticiones--%>
    <div class="modal fade" id="modalVerPeticiones" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="exampleModalLabel2">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel2">Peticiones </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>

                            <div class="row mb-3">
                                <div class="col-xl-2 col-lg-2 col-md-2 col-sm-12 col-12">
                                    <asp:Label ID="Label2" runat="server" Text="Pliego:" CssClass="form-label fw-bold"></asp:Label>
                                </div>
                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">
                                    <asp:DropDownList ID="DropDownListPliego" runat="server" CssClass="form-select" AutoPostBack="true" DataSourceID="SqlDataSourcePliego"
                                        DataTextField="FOLIO_PLIEGO" DataValueField="ID_PLIEGO" OnDataBound="DropDownListPliego_DataBound">
                                    </asp:DropDownList>

                                    <asp:SqlDataSource ID="SqlDataSourcePliego" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                        SelectCommand="SELECT ID_PLIEGO, FOLIO_PLIEGO FROM PLIEGO
WHERE CLAVE_ZP = @claveZP">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="LabelClaveZP" Name="claveZP" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>

                            <!-- Tabla Peticiones -->
                            <div class="mb-3" runat="server">
                                <div class="table-responsive">
                                    <h6 class="h6 mb-2 mt-1 text-dark fw-bold">Peticiones</h6>

                                    <asp:GridView ID="GridViewPliegoPeticion" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGetPLGPeticionResp"
                                        CssClass="table table-striped table-bordered small" HeaderStyle-CssClass="table-primary text-center"
                                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;"
                                        OnRowDataBound="GridViewPliegoPeticion_RowDataBound">
                                        <Columns>
                                            <%--Columnas ocultas--%> 
                                            <asp:BoundField DataField="CLAVE_ZP" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="ID_PLIEGO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="ID_CAT_PETICION" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="ID_PETICION" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="ID_DOCUMENTO" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <asp:BoundField DataField="ID_EST_PETICION" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                            <%--Columnas visibles--%> 
                                            <asp:BoundField DataField="DESCRIPCION_CAT_PETICION" HeaderText="Categoría" SortExpression="DESCRIPCION_CAT_PETICION" />
                                            <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha de petición" SortExpression="FECHA_PETICION" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="DESC_PETICION" HeaderText="Petición" SortExpression="DESC_PETICION" />
                                            <asp:BoundField DataField="FECHA_RESP_PETICION" HeaderText="Fecha de respuesta" SortExpression="FECHA_RESP_PETICION" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="DESC_RESP_PETICION" HeaderText="Respuesta" SortExpression="DESC_RESP_PETICION" />
                                            <asp:TemplateField HeaderText="Estatus" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblEstatus" runat="server" Text='<%# Eval("DESCRIPCION_PETICION") %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Documento--%> 
                                            <asp:TemplateField HeaderText="Documento de respuesta" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-placement="left" data-bs-custom-class="custom-popover"
                                                        data-bs-trigger="hover focus" data-bs-content="Ver documento">
                                                        <asp:LinkButton ID="LinkButtonVeDocRespuesta" runat="server" CssClass="color-btn mb-2" Text='<i class="fas fa-file-pdf fa-2x fa-fw"></i>'
                                                            OnClick="LinkButtonVeDocRespuesta_Click">                                                                                        
                                                        </asp:LinkButton>
                                                    </span>
                                                    <asp:Label ID="LabelNoExistDoc" runat="server" Text="" CssClass="small fw-bold" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <%--  <EmptyDataTemplate>
                                            <div class="text-center">
                                                <asp:Label runat="server" ID="mensaje" Text="No hay pliegos existentes en la unidad académica" CssClass="alert alert-danger" Width="100%"></asp:Label>
                                            </div>
                                        </EmptyDataTemplate>--%>
                                    </asp:GridView>
                                    <%------------------------------------datasource del gridview para peticiones pliego----------------------------------%>
                                    <asp:SqlDataSource ID="SqlDataSourceGetPLGPeticionResp" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                        SelectCommand="SELECT pl.CLAVE_ZP, pe.ID_PLIEGO,pe.ID_CAT_PETICION,cp.DESCRIPCION_CAT_PETICION,pe.ID_PETICION, 
		pe.ID_EST_PETICION, ep.DESCRIPCION_PETICION,
		FORMAT(pe.FECHA_PETICION, 'dd/MM/yyyy') as FECHA_PETICION,pe.DESC_PETICION,
		FORMAT(pe.FECHA_RESP_PETICION, 'dd/MM/yyyy') as FECHA_RESP_PETICION,PE.DESC_RESP_PETICION,
		(SELECT vp.ID_DOCUMENTO FROM VINCULAR_PETICION_DOCUMENTO vp
			WHERE vp.ID_PLIEGO = pe.ID_PLIEGO 
				AND vp.ID_PETICION = pe.ID_PETICION
		) AS ID_DOCUMENTO
	FROM PETICIONES pe, CAT_CATEGORIA_PETICION cp, PLIEGO pl, ESTATUS_PETICION ep
	WHERE pe.ID_CAT_PETICION = cp.ID_CAT_PETICION
		AND pe.ID_PLIEGO = pl.ID_PLIEGO
		AND pe.ID_EST_PETICION = ep.ID_EST_PETICION
		AND pe.ID_PLIEGO = @pliego">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="DropDownListPliego" Name="pliego" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                     <%--SELECT pl.CLAVE_ZP, pe.ID_PLIEGO,pe.ID_CAT_PETICION,cp.DESCRIPCION_CAT_PETICION,pe.ID_PETICION, 
		FORMAT(pe.FECHA_PETICION, 'dd/MM/yyyy') as FECHA_PETICION,pe.DESC_PETICION,
		FORMAT(pe.FECHA_RESP_PETICION, 'dd/MM/yyyy') as FECHA_RESP_PETICION,PE.DESC_RESP_PETICION,
		(SELECT vp.ID_DOCUMENTO FROM VINCULAR_PETICION_DOCUMENTO vp
			WHERE vp.ID_PLIEGO = pe.ID_PLIEGO 
				AND vp.ID_PETICION = pe.ID_PETICION
		) AS ID_DOCUMENTO
	FROM PETICIONES pe, CAT_CATEGORIA_PETICION cp, PLIEGO pl
	WHERE pe.ID_CAT_PETICION = cp.ID_CAT_PETICION
		AND pe.ID_PLIEGO = pl.ID_PLIEGO
		AND pe.ID_PLIEGO = @pliego--%>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--scripts--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script src="public/js/TooltipPopover.js"></script>

    <%--Modals--%>
    <script>
         //Blocked aria-hidden on an element because its descendant retained focus
        //Forzar la perdida del foco (blur) en el modal
        $('.modal').on('hide.bs.modal', function () {
            document.activeElement.blur();
        });

        function ShowModalConfirm() {
            var myModal = document.getElementById('modalConfirm');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
            //myModal.style.zIndex = '1100'
        }

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
             myModal.style.zIndex = '1100'
        }

        function ShowModalVerPeticiones() {

            var myModal = document.getElementById('modalVerPeticiones');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }
    </script>

   <%-- <script type="text/javascript">
    // Guardar la pestaña activa antes del postback
    document.addEventListener("DOMContentLoaded", function () {
        // Detectar clic en cualquier tab
        const tabButtons = document.querySelectorAll('button[data-bs-toggle="pill"]');
        tabButtons.forEach(btn => {
            btn.addEventListener('shown.bs.tab', function (e) {
                // localStorage.setItem('activeTab', e.target.getAttribute('data-bs-target'));
                sessionStorage.setItem('activeTab', e.target.getAttribute('data-bs-target'));
            });
        });

        // Restaurar la pestaña activa después del postback
        // const activeTab = localStorage.getItem('activeTab');
        const activeTab = sessionStorage.getItem('activeTab');
        if (activeTab) {
            const tabTrigger = document.querySelector(`button[data-bs-target="${activeTab}"]`);
            if (tabTrigger) {
                const tab = new bootstrap.Tab(tabTrigger);
                tab.show();
            }
        }
    });
</script>--%>
   <script type="text/javascript">
    function saveActiveTab(tabId) {
        document.getElementById("<%= HiddenActiveTab.ClientID %>").value = tabId;
    }

    document.addEventListener("DOMContentLoaded", function () {
        var tabButtons = document.querySelectorAll('button[data-bs-toggle="pill"]');

        for (var i = 0; i < tabButtons.length; i++) {
            tabButtons[i].addEventListener('shown.bs.tab', function (e) {
                var id = e.target.getAttribute('data-bs-target');
                saveActiveTab(id);
            });
        }
    });
</script>

 <%--  <script>
    function QuitarFocusAntesPostback() {
        document.activeElement.blur();
    }
</script>--%>

</asp:Content>
