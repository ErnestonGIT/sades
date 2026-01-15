<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Validacion.aspx.cs" Inherits="Validacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
    .expand-panel {
        position: absolute;
        left: 0;
        width: 100%;
        z-index: 100;
        background: #ffffff;
        margin-top: 20px;
        padding: 5px;
        box-sizing: border-box;
        border: 1px solid #ddd;
    }

    .row-relative {
        position: relative;
    }
</style>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="loader" style="position: fixed; text-align: center; vertical-align: middle; height: auto; width: auto; top: 0; bottom: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                <asp:Label ID="LabelLoader" runat="server" Text="Cargando..." Style="color: #FFFFFF;"></asp:Label>
                <br />
                <img src="public/img/loader/sean_tiffonnet_loader_360learning-450-600.gif" alt="Cargando..." class="img-fluid" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="pagetitle">
        <div class="row">
            <div class="col-md-6">
                <h1>Validar petición</h1>
            </div>
            <div class="col-md-6 ">
            </div>
        </div>    
    </div><!-- End Page Title -->

    <section class="section">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LabelZP" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="LabelIdPerfil" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="LabelId_Nivel_Est" runat="server" Text="" Visible="false"></asp:Label>
                                <div class="row pt-2">
                                    <div class="col-md-12">
                                        <asp:Label ID="LabelInst1" runat="server" Text="Selecciona la unidad académica" CssClass="h6 fw-semibold"></asp:Label>
                                    </div>
                                </div>
                                <div class="row pt-2">
                                    <div class="col-md-6">
                                        <asp:Label ID="LabelTitUA" runat="server" Text="Unidad Académica" CssClass="h6 fw-bolder"></asp:Label>
                                        <asp:DropDownList ID="DropDownListUA" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropUA"
                                            DataTextField="DESCRIPCION_DP" DataValueField="CLAVE_ZP" CssClass="form-select border-primary" 
                                            OnDataBound="DropDownListUA_DataBound" OnSelectedIndexChanged="DropDownListUA_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSourceDropUA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                            SelectCommand="select CLAVE_ZP, DESCRIPCION_DP from CAT_DEPENDENCIAS_POLITECNICAS where ID_NIVEL_EST = @NivelE order by DESCRIPCION_DP ">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="LabelId_Nivel_Est" Name="NivelE" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="SqlDataSourceDropUA_UA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                            SelectCommand="select CLAVE_ZP, DESCRIPCION_DP from CAT_DEPENDENCIAS_POLITECNICAS where ID_NIVEL_EST = @NivelE and CLAVE_ZP = @ZP order by DESCRIPCION_DP ">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="LabelId_Nivel_Est" Name="NivelE" PropertyName="Text" />
                                                <asp:ControlParameter ControlID="LabelZP" Name="ZP" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="row pt-2" runat="server" visible="false" id="GridPliegos">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView ID="GridViewPliegos" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourcePliegos" 
                                            CssClass="table table-bordered" HeaderStyle-CssClass="table-primary text-center" OnRowDataBound="GridViewPliegos_RowDataBound" 
                                            PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                            <Columns>
                                                <asp:BoundField DataField="ID_PLIEGO" HeaderText="ID_PLIEGO" SortExpression="ID_PLIEGO" />
                                                <asp:BoundField DataField="FOLIO_PLIEGO" HeaderText="FOLIO " SortExpression="FOLIO_PLIEGO" />
                                                <asp:TemplateField HeaderText="ARCHIVO" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelRutaArchivoPliego" runat="server"  Text='<%# Eval("RUTA_ARCHIVO")%>' Visible="false"></asp:Label>
                                                        <asp:ImageButton ID="ImageButtonArchivoPliego" runat="server" ImageUrl="~/public/img/documento.png" Width="35px" Height="35px" OnClick="ImageButtonArchivoPliego_Click" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-custom-class="custom-tooltip" data-bs-trigger="hover focus" ToolTip="Visualizar pliego" />
                                                        <asp:Image ID="ImageNoArchivoPliego" runat="server" CssClass=" ri-file-forbid-fill text-danger fa-2x" Visible="false"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="ButtonSelectPliego" runat="server" Text="Seleccionar" CausesValidation="false" CssClass="btn btn-primary" OnClick="ButtonSelectPliego_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="text-center">
                                                    <asp:Label runat="server" ID="mensaje" Text="No existen pliegos" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourcePliegos" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" CancelSelectOnNullParameter="False"
                                            SelectCommand="select ID_PLIEGO, FOLIO_PLIEGO, RUTA_ARCHIVO from PLIEGO where CLAVE_ZP = @ZP">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="DropDownListUA" Name="ZP" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!--Modal Detalles Pliego-->
    <div class="modal fade" id="ModalDetallesPliego" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalDetallesPliegoLabel" tabindex="-1">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel2_2" runat="server">
                        <ContentTemplate>
                            <h5 class="modal-title fw-bolder" id="ModalDetallesPliegoLabel">
                                Pliego - <asp:Label ID="LabelFolioPliego" runat="server" Text=""></asp:Label>
                            </h5>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <div class="row pt-2">
                                <div class="col-md-12">
                                    <asp:Label ID="LabelInst2" runat="server" Text="Selecciona ''Estatus'' para validar o ''Detalles'' para visualizar los datos de las peticiones." CssClass="h6 fw-semibold"></asp:Label>
                                </div>
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-12 table-responsive">
                                    <asp:Label ID="LabelIdPliego" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:GridView ID="GridViewPeticiones" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourcePeticiones" 
                                            CssClass="table table-bordered" HeaderStyle-CssClass="table-primary text-center" OnRowDataBound="GridViewPeticiones_RowDataBound" 
                                            PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                            <Columns>
                                                <asp:BoundField DataField="ID_CAT_PETICION" HeaderText="ID_CAT_PETICION" SortExpression="ID_CAT_PETICION" />
                                                <asp:BoundField DataField="ID_EST_PETICION" HeaderText="ID_EST_PETICION " SortExpression="ID_EST_PETICION" />
                                                <asp:BoundField DataField="ID_PETICION" HeaderText="ID_PETICION " SortExpression="ID_PETICION" />
                                                <asp:BoundField DataField="FECHA_PETICION" HeaderText="FECHA " SortExpression="FECHA_PETICION" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="DESC_PETICION" HeaderText="PETICIÓN " SortExpression="DESC_PETICION" />
                                                <asp:BoundField DataField="DESCRIPCION_CAT_PETICION" HeaderText="CATEGORÍA" SortExpression="DESCRIPCION_CAT_PETICION" />
                                                <asp:BoundField DataField="DESCRIPCION_SUBCAT_PETICION" HeaderText="SUBCATEGORÍA" SortExpression="DESCRIPCION_SUBCAT_PETICION" />
                                                <asp:TemplateField HeaderText="ESTATUS" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButtonEstatus" runat="server" ImageUrl="~/public/img/peticion-negra.png" Width="35px" Height="35px" OnClick="ImageButtonEstatus_Click" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-custom-class="custom-tooltip" data-bs-trigger="hover focus" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DETALLES" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButtonSelectDetalles" runat="server" ImageUrl="~/public/img/evidencia.png" Width="35px" Height="35px" OnClick="ImageButtonSelectDetalles_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="text-center">
                                                    <asp:Label runat="server" ID="mensaje" Text="No existen docentes" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourcePeticiones" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" CancelSelectOnNullParameter="False"
                                            SelectCommand="select p.ID_CAT_PETICION, ID_EST_PETICION, ID_PETICION, FECHA_PETICION, DESC_PETICION, cp.DESCRIPCION_CAT_PETICION, sp.DESCRIPCION_SUBCAT_PETICION
                                                            from PETICIONES as p, CAT_CATEGORIA_PETICION cp, CAT_SUBCATEGORIA_PETICION as sp
                                                            where p.ID_CAT_PETICION = cp.ID_CAT_PETICION and (p.ID_CAT_PETICION = sp.ID_CAT_PETICION and p.ID_SUBCAT_PETICION = sp.ID_SUBCAT_PETICION) 
                                                            and ID_PLIEGO = @Pliego order by FECHA_PETICION">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="LabelIdPliego" Name="Pliego" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!--Modal Estatus-->
    <div class="modal fade" id="ModalEstatus" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalEstatusLabel" tabindex="-1">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h5 class="modal-title fw-bolder" id="ModalEstatusLabel">
                                Estatus - Petición
                            </h5>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
    
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelIdEstatus" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="LabelIdPeticion" runat="server" Text="" Visible="false"></asp:Label>
                            <div class="row pt-2">
                                <div class="col-md-12">
                                    <asp:Label ID="LabelInst3" runat="server" Text="Clic en ''Validar'' para Atender la petición seleccionada." CssClass="h6 fw-semibold"></asp:Label>
                                </div>
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-4">
                                </div>
                                <div class="col-md-4 text-center">
                                    <div class="d-grid gap-2 mt-3">
                                        <asp:Button ID="ButtonValidar" runat="server" Text="Validar" CssClass="btn btn-primary" OnClick="ButtonValidar_Click"/>
                                    </div>
                                    
                                </div>
                                <div class="col-md-4">
                                </div>
                            </div>
                            <div class="row pt-3">
                                <div class="col-md-6">
                                    <h6 class="fw-bolder">Categoría: </h6>
                                    <asp:Label ID="LabelCategoriaP" runat="server" Text="" CssClass="h6"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="fw-bolder">Subcategoría: </h6>
                                    <asp:Label ID="LabelSubCategoriaP" runat="server" Text="" CssClass="h6"></asp:Label>
                                </div>
                            </div>

                            <asp:Panel ID="PanelEstatus" runat="server">
                                <div class="container py-4">
                                    <div class="main-timeline-4 text-white">
                                        <div class="timeline-4 left-4 arrowL-red" runat="server" id="TCardEstatus">
                                            <div class="card color-rojo-custom" runat="server" id="CCardEstatus" >
                                                <div class="card-body p-4">
                                                    <div class="row mb-3">
                                                        <div class="col-md-6">
                                                            <i class="ri-draft-line fa-2x"></i>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="LabelNomEstatus" runat="server" Text="Hola" CssClass="h5"></asp:Label>
                                                        </div>  
                                                    </div>
                                                    <h6>
                                                        <asp:Label ID="LabelFechaP" runat="server" Text=""></asp:Label>
                                                    </h6>
                                                    <p class="text-justify">
                                                        <asp:Label ID="LabelPeticionP" runat="server" Text=""></asp:Label>
                                                    </p>
                                                    <div class="float-end">
                                                        <asp:Button ID="ButtonDetallesP" runat="server" Text="Detalles" CssClass="btn btn-outline-primary" OnClick="ButtonDetallesP_Click"/>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelEstatusC" runat="server" Visible="false">
                                <div class="container py-5">
                                    <div class="main-timeline-4 text-white">
                                        <div class="timeline-4 left-4 arrowL-red" runat="server" id="RojoE">
                                            <div class="card color-rojo-custom">
                                                <div class="card-body p-4">
                                                    <i class="fas fa-brain fa-2x mb-3"></i>
                                                    <h4>7:45PM</h4>
                                                    <p class="small text-white-50 mb-4">May 21</p>
                                                    <p>Lorem ipsum dolor sit amet, quo ei simul congue exerci, ad nec admodum perfecto
                                                        mnesarchum, vim ea mazim fierent detracto. Ea quis iuvaret expetendis his, te elit voluptua
                                                        dignissim
                                                        per, habeo iusto primis ea eam.
                                                    </p>
                                                    <h6 class="badge bg-body-tertiary text-black mb-0">New</h6>
                                                    <h6 class="badge bg-body-tertiary text-black mb-0">Admin</h6>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="timeline-4 right-4 arrowR-yellow " runat="server" id="AmarilloE">
                                            <div class="card color-amarillo-custom">
                                                <div class="card-body p-4">
                                                    <i class="fas fa-camera fa-2x mb-3"></i>
                                                    <h4>8:00 AM</h4>
                                                    <p class="small text-white-50 mb-4">May 18</p>
                                                    <p>Lorem ipsum dolor sit amet, quo ei simul congue exerci, ad nec admodum perfecto
                                                        mnesarchum, vim ea mazim fierent detracto. Ea quis iuvaret expetendis his, te elit voluptua
                                                        dignissim
                                                        per, habeo iusto primis ea eam.
                                                  </p>
                                                  <h6 class="badge bg-body-tertiary text-black mb-0">New</h6>
                                                  <h6 class="badge bg-body-tertiary text-black mb-0">Admin</h6>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="timeline-4 left-4 arrowL-green" runat="server" id="VerdeE">
                                            <div class="card color-verde-custom">
                                                <div class="card-body p-4">
                                                    <i class="fas fa-campground fa-2x mb-3"></i>
                                                    <h4>7:25 PM</h4>
                                                    <p class="small text-white-50 mb-4">May 6</p>
                                                    <p>Lorem ipsum dolor sit amet, quo ei simul congue exerci, ad nec admodum perfecto
                                                        mnesarchum, vim ea mazim fierent detracto. Ea quis iuvaret expetendis his, te elit voluptua
                                                        dignissim
                                                        per, habeo iusto primis ea eam.
                                                    </p>
                                                    <h6 class="badge bg-body-tertiary text-black mb-0">New</h6>
                                                    <h6 class="badge bg-body-tertiary text-black mb-0">Admin</h6>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="timeline-4 right-4 arrowR-black" runat="server" id="NegroE">
                                            <div class="card color-negro-custom">
                                                <div class="card-body p-4">
                                                    <i class="fas fa-sun fa-2x mb-3"></i>
                                                    <h4>3:55 PM</h4>
                                                    <p class="small text-white-50 mb-4">Apr 26</p>
                                                    <p>Lorem ipsum dolor sit amet, quo ei simul congue exerci, ad nec admodum perfecto
                                                        mnesarchum, vim ea mazim fierent detracto. Ea quis iuvaret expetendis his, te elit voluptua
                                                        dignissim
                                                        per, habeo iusto primis ea eam.
                                                    </p>
                                                    <h6 class="badge bg-body-tertiary text-black mb-0">New</h6>
                                                    <h6 class="badge bg-body-tertiary text-black mb-0">Admin</h6>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!--Modal Detalles Peticion-->
    <div class="modal fade" id="ModalDetallesPeticion" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalDetallesPeticionLabel" tabindex="-1">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <h5 class="modal-title fw-bolder" id="ModalDetallesPeticionLabel">
                                Detalle - Petición
                            </h5>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
    
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelIdEstatusDet" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="LabelIdPeticionDet" runat="server" Text="" Visible="false"></asp:Label>
                            <div class="row pt-2">
                                <div class="col-md-4">
                                    <h6 class="fw-bolder">Fecha: </h6>
                                    <asp:Label ID="LabelFecDP" runat="server" Text="" CssClass="h6"></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>
                                <div class="col-md-8">
                                    <h6 class="fw-bolder">Petición: </h6>
                                    <asp:Label ID="LabelPeticionDP" runat="server" Text="" CssClass="h6"></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>  
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-6">
                                    <h6 class="fw-bolder">Categoría: </h6>
                                    <asp:Label ID="LabelCategoriaDP" runat="server" Text="" CssClass="h6"></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>
                                <div class="col-md-6">
                                    <h6 class="fw-bolder">Subcategoría: </h6>
                                    <asp:Label ID="LabelSubCategoriaDP" runat="server" Text="" CssClass="h6"></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>  
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-12">
                                    <h6 class="fw-bolder">Respuesta </h6>
                                </div>
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-4">
                                    <h6 class="fw-bolder">Fecha: </h6>
                                    <asp:Label ID="LabelFecResp" runat="server" Text=""></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>
                                <div class="col-md-8">
                                    <h6 class="fw-bolder">Descripción: </h6>
                                    <asp:Label ID="LabelDescResp" runat="server" Text=""></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-6">
                                    <h6 class="fw-bolder">Fecha Compromiso: </h6>
                                    <asp:Label ID="LabelFechaComproDet" runat="server" Text=""></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>
                                <div class="col-md-6">
                                    <h6 class="fw-bolder">Asignación: </h6>
                                    <asp:Label ID="LabelAsignacion" runat="server" Text=""></asp:Label>
                                    <hr class="mt-1 mb-1" />
                                </div>
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-12 table-responsive">
                                    <h6 class="fw-bolder">Garantías: </h6>
                                    <asp:GridView ID="GridViewGarantias" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGaratias" 
                                        CssClass="table table-bordered " HeaderStyle-CssClass="table-primary text-center" OnRowDataBound="GridViewGarantias_RowDataBound" 
                                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                        <Columns>
                                            <asp:BoundField DataField="ID_GARANTIA" HeaderText="ID_GARANTIA" SortExpression="ID_GARANTIA" />
                                            <asp:BoundField DataField="ID_DOCUMENTO" HeaderText="ID_DOCUMENTO" SortExpression="ID_DOCUMENTO" />
                                            <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="Fecha" SortExpression="FECHA_REGISTRO" DataFormatString="{0:dd/MM/yyyy}"/>
                                            <asp:BoundField DataField="DESC_GARANTIA" HeaderText="Garantía" SortExpression="DESC_GARANTIA" />
                                            <asp:TemplateField HeaderText="Archivo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelRutaArchivoGarantia" runat="server"  Text='<%# Eval("RUTA_DOCUMENTO")%>' Visible="false"></asp:Label>
                                                    <asp:ImageButton ID="ImageButtonArchivoGarantia" runat="server" ImageUrl="~/public/img/documento.png" Width="35px" Height="35px" OnClick="ImageButtonArchivoGarantia_Click" />
                                                    <asp:Image ID="ImageNoArchivoGarantia" runat="server" CssClass=" ri-file-forbid-fill text-danger fa-2x" Visible="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="text-center">
                                                <asp:Label runat="server" ID="mensaje" Text="No existen garantías" CssClass="alert alert-light" Width="90%"></asp:Label>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSourceGaratias" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" CancelSelectOnNullParameter="False"
                                        SelectCommand="select ID_GARANTIA, gp.ID_DOCUMENTO, gp.FECHA_REGISTRO, DESC_GARANTIA,RUTA_DOCUMENTO 
                                                        from GARANTIA_PETICION as gp, DOCUMENTO_GARANTIA as dg
                                                        where gp.ID_DOCUMENTO = dg.ID_DOCUMENTO AND ID_PLIEGO = @Pliego AND ID_PETICION = @Peticion ">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="LabelIdPliego" Name="Pliego" PropertyName="Text" />
                                            <asp:ControlParameter ControlID="LabelIdPeticionDet" Name="Peticion" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="row pt-2">
                                <div class="col-md-12 table-responsive">
                                    <h6 class="fw-bolder">Diagnóstico - Gestiones: </h6>
                                    <asp:GridView ID="GridViewDG" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceDG" OnRowDataBound="GridViewDG_RowDataBound" 
                                        CssClass="table table-bordered" HeaderStyle-CssClass="table-primary text-center" OnRowCommand="GridViewDG_RowCommand">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnExpand" runat="server" CommandName="Expandir" CommandArgument='<%# Container.DataItemIndex %>'>+</asp:LinkButton>
                                                    <asp:Panel ID="pnlDetalles" runat="server" Visible="False" CssClass="expand-panel" >
                                                        <div class="row pt-2">
                                                            <div class="col-md-12">
                                                                <asp:Label ID="LabelIdDiagnostico" runat="server" Text='<%# Eval("ID_DIAGNOSTICO")%>' Visible="false"></asp:Label>
                                                                <div class="row pt-2">
                                                                    <div class="col-md-12 table-responsive">
                                                                        <h6 class="fw-bolder">Gestiones: </h6>
                                                                        <asp:GridView ID="GridViewGestiones" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGestiones" 
                                                                            CssClass="table table-bordered w-100" HeaderStyle-CssClass="table-primary text-center" OnRowDataBound="GridViewGestiones_RowDataBound" 
                                                                            PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID_GESTIONES" HeaderText="ID_GESTIONES" SortExpression="ID_GESTIONES" />
                                                                                <asp:BoundField DataField="FECHA_GESTIONES" HeaderText="Fecha" SortExpression="FECHA_GESTIONES" DataFormatString="{0:dd/MM/yyyy}"/>
                                                                                <asp:BoundField DataField="DESCRIPCION_GESTIONES" HeaderText="Gestión" SortExpression="DESCRIPCION_GESTIONES" />
                                                                                <asp:TemplateField HeaderText="Archivo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="LabelRutaArchivoGestiones" runat="server"  Text='<%# Eval("ARCHIVO_GESTIONES")%>' Visible="false"></asp:Label>
                                                                                        <asp:ImageButton ID="ImageButtonArchivoGestiones" runat="server" ImageUrl="~/public/img/documento.png" Width="35px" Height="35px" OnClick="ImageButtonArchivoGestiones_Click" />
                                                                                        <asp:Image ID="ImageNoArchivoGestiones" runat="server" CssClass=" ri-file-forbid-fill text-danger fa-2x" Visible="false"/>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div class="text-center">
                                                                                    <asp:Label runat="server" ID="mensaje" Text="No existen gestiones" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                        <asp:SqlDataSource ID="SqlDataSourceGestiones" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" CancelSelectOnNullParameter="False"
                                                                            SelectCommand="select ID_GESTIONES, FECHA_GESTIONES, DESCRIPCION_GESTIONES, ARCHIVO_GESTIONES from GESTIONES
                                                                                            where ID_PLIEGO = @Pliego AND ID_PETICION = @Peticion and ID_DIAGNOSTICO = @Diagnostico ">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="LabelIdPliego" Name="Pliego" PropertyName="Text" />
                                                                                <asp:ControlParameter ControlID="LabelIdPeticionDet" Name="Peticion" PropertyName="Text" />
                                                                                <asp:ControlParameter ControlID="LabelIdDiagnostico" Name="Diagnostico" PropertyName="Text" />
                                                                            </SelectParameters>
                                                                        </asp:SqlDataSource>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID_DIAGNOSTICO" HeaderText="ID_DIAGNOSTICO" />
                                            <asp:BoundField DataField="FECHA_DIAGNOSTICO" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"/>
                                            <asp:BoundField DataField="DESCRIPCION_DIAGNOSTICO" HeaderText="Diagnóstico" />
                                            <asp:TemplateField HeaderText="Archivo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelRutaArchivoDiagnostico" runat="server"  Text='<%# Eval("ARCHIVO_DIAGNOSTICO")%>' Visible="false"></asp:Label>
                                                    <asp:ImageButton ID="ImageButtonArchivoDiagnostico" runat="server" ImageUrl="~/public/img/documento.png" Width="35px" Height="35px" OnClick="ImageButtonArchivoDiagnostico_Click" />
                                                    <asp:Image ID="ImageNoArchivoDiagnostico" runat="server" CssClass=" ri-file-forbid-fill text-danger fa-2x" Visible="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSourceDG" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" CancelSelectOnNullParameter="False"
                                        SelectCommand="select ID_PLIEGO, ID_PETICION, ID_DIAGNOSTICO, DESCRIPCION_DIAGNOSTICO, FECHA_DIAGNOSTICO, ARCHIVO_DIAGNOSTICO 
                                                        from DIAGNOSTICO WHERE ID_PLIEGO =@Pliego AND ID_PETICION = @Peticion ">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="LabelIdPliego" Name="Pliego" PropertyName="Text" />
                                            <asp:ControlParameter ControlID="LabelIdPeticionDet" Name="Peticion" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!--Modal MSGValida-->
    <div class="modal fade" id="ModalMSGValida" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalMSGValidaLabel" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <div class="row pt-2">
                                <div class="col-md-12 text-justify">
                                    <asp:Label ID="LabelValidaMSG" runat="server" Text="¿Está de acuerdo en dar por concluida la petición?" CssClass="h5 fw-bolder"></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <div class="row pt-2">
                        <div class="col-md-6 text-center">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="ButtonValidaMSG" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="ButtonValidaMSG_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-6 text-center">
                            <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Cancelar</button>
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
    </div>

    <!--Modal Ver archivo-->
    <div class="modal fade" id="ModalVerArchivo" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalVerArchivoLabel" tabindex="-1">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel54" runat="server">
                        <ContentTemplate>
                            <h5 class="modal-title" id="ModalVerArchivoLabel">Ver archivo</h5>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel56" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelA" runat="server" Text=""></asp:Label>
                            <div class="text-center">
                                <iframe id="verPDF" runat="server"  type="application/pdf" class="Visualizar" ></iframe>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <link href="public/css/TimelineE.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2-bootstrap-5-theme@1.3.0/dist/select2-bootstrap-5-theme.min.css" />

    <!-- Scripts -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.full.min.js"></script>

    <!-- Libreria español -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/i18n/es.js"></script>
    <script type="text/javascript" src="https://unpkg.com/default-passive-events"></script>


    <%--aplicar select2 en los dropdowlists--%>
    <script type="text/javascript">
        $(document).ready(function () {

            $(function () {
                $.fn.select2.defaults.set('language', 'es');
                $("[id*=DropDownListUA]").select2({
                    theme: "bootstrap-5",
                    containerCssClass: "form-select border-primary",
                    width: '100%',
                });
                //$("[id*=DropDownListSecuencia]").select2({
                //    theme: "bootstrap-5",
                //    containerCssClass: "form-select border-primary",
                //    width: '100%',
                //});
                //$("[id*=DropDownListGUAGrupo]").select2({
                //    theme: "bootstrap-5",
                //    containerCssClass: "form-select border-primary",
                //    width: '100%',
                //});
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    $(function () {
                        $.fn.select2.defaults.set('language', 'es');
                        $("[id*=DropDownListUA]").select2({
                            theme: "bootstrap-5",
                            containerCssClass: "form-select border-primary",
                            width: '100%',
                        });
                        //$("[id*=DropDownListSecuencia]").select2({
                        //    theme: "bootstrap-5",
                        //    containerCssClass: "form-select border-primary",
                        //    width: '100%',
                        //});
                        //$("[id*=DropDownListGUAGrupo]").select2({
                        //    theme: "bootstrap-5",
                        //    containerCssClass: "form-select border-primary",
                        //    width: '100%',
                        //});
                    });
                });
            }
        });

        $(document).ready(function () {

            infoToolStart();

        });

        //function infoToolStart() {
        //    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
        //    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => {
        //    const tooltip = new bootstrap.Tooltip(tooltipTriggerEl);

        //        // Agregar un evento click al botón para cerrar el popover manualmente
        //        tooltipTriggerEl.addEventListener('click', () => {
        //            tooltip.hide();
        //        });

        //        return tooltip;
        //    });

        //    const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
        //    const popoverList = [...popoverTriggerList].map(popoverTriggerEl => {
        //    const popover = new bootstrap.Popover(popoverTriggerEl);

        //        // Agregar un evento click al botón para cerrar el popover manualmente
        //        popoverTriggerEl.addEventListener('click', () => {
        //            popover.hide();
        //        });

        //        return popover;

        //    });
        //}   

        function ShowModalVerArchivo() {
            var myModal = document.getElementById('ModalVerArchivo');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function ShowModalDetallesPliego() {
            var myModal = document.getElementById('ModalDetallesPliego');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function ShowModalDetallesPeticion() {
            var myModal = document.getElementById('ModalDetallesPeticion');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function ShowModalEstatus() {
            var myModal = document.getElementById('ModalEstatus');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function ShowModalMSGValida() {
            var myModal = document.getElementById('ModalMSGValida');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function HideModalMSGValida() {
            var myModal = document.getElementById('ModalMSGValida');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.hide();
        }
    </script>
</asp:Content>

