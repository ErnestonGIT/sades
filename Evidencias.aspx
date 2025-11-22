<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Evidencias.aspx.cs" Inherits="Evidencias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="section">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LabelZP" runat="server" Text="" Visible="false"></asp:Label>
                                <div class="row pt-2">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView ID="GridViewPliegos" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourcePliegos" 
                                            CssClass="table table-bordered dt-responsive" HeaderStyle-CssClass="table-primary text-center" OnRowDataBound="GridViewPliegos_RowDataBound" 
                                            PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                            <Columns>
                                                <asp:BoundField DataField="ID_PLIEGO" HeaderText="ID_PLIEGO" SortExpression="ID_PLIEGO" />
                                                <asp:BoundField DataField="FOLIO_PLIEGO" HeaderText="FOLIO " SortExpression="FOLIO_PLIEGO" />
                                                <asp:TemplateField HeaderText="ARCHIVO" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelRutaArchivoPliego" runat="server"  Text='<%# Eval("RUTA_ARCHIVO")%>' Visible="false"></asp:Label>
                                                        <asp:ImageButton ID="ImageButtonArchivoPliego" runat="server" ImageUrl="~/public/img/documento.png" Width="35px" Height="35px" OnClick="ImageButtonArchivoPliego_Click" />
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
                                                    <asp:Label runat="server" ID="mensaje" Text="No existen docentes" CssClass="alert alert-light" Width="90%"></asp:Label>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourcePliegos" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" CancelSelectOnNullParameter="False"
                                            SelectCommand="select ID_PLIEGO, FOLIO_PLIEGO, RUTA_ARCHIVO from PLIEGO where CLAVE_ZP = @ZP">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="LabelZP" Name="ZP" PropertyName="Text" />
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
                            <asp:Label ID="LabelIdPliego" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:GridView ID="GridViewPeticiones" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourcePeticiones" 
                                CssClass="table table-bordered dt-responsive" HeaderStyle-CssClass="table-primary text-center" OnRowDataBound="GridViewPeticiones_RowDataBound" 
                                PageSize="10" AllowPaging="true" PagerStyle-CssClass="pagination-ys" Style="border-collapse: collapse;">
                                <Columns>
                                    <asp:BoundField DataField="ID_CAT_PETICION" HeaderText="ID_CAT_PETICION" SortExpression="ID_CAT_PETICION" />
                                    <asp:BoundField DataField="ID_EST_PETICION" HeaderText="ID_EST_PETICION " SortExpression="ID_EST_PETICION" />
                                    <asp:BoundField DataField="ID_PETICION" HeaderText="ID_PETICION " SortExpression="ID_PETICION" />
                                    <asp:BoundField DataField="FECHA_PETICION" HeaderText="FECHA " SortExpression="FECHA_PETICION" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DESC_PETICION" HeaderText="PETICIÓN " SortExpression="DESC_PETICION" />
                                    <asp:TemplateField HeaderText="DIAGNÓSTICO" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1SelectDiagnosticoP" runat="server" ImageUrl="~/public/img/pendiente.png" Width="35px" Height="35px" OnClick="ImageButtonSelectDiagnosticoP_Click"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GESTIONES" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButtonSelectGestionesP" runat="server" ImageUrl="~/public/img/pendiente.png" Width="35px" Height="35px" OnClick="ImageButtonSelectGestionesP_Click" />
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
                                SelectCommand="select ID_CAT_PETICION, ID_EST_PETICION, ID_PETICION, FECHA_PETICION, DESC_PETICION
                                                from PETICIONES where ID_PLIEGO = @Pliego">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="LabelIdPliego" Name="Pliego" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Diagnostico-Gestiones -->
    <div class="modal fade" id="ModalDG" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalDGLabel" tabindex="-1">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h5 class="modal-title fw-bolder" id="ModalDGLabel">
                                <asp:Label ID="LabelTitDG" runat="server" Text=""></asp:Label>
                            </h5>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
             
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelDG" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="LabelId_PeticionDG" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="LabelTitD" runat="server" Text="Evidencias de cumplimiento" CssClass="fw-bolder"></asp:Label>
                            <asp:Panel ID="PanelAgregarDG" runat="server">
                                <asp:Panel ID="PanelAlertaErrorArchivo" runat="server" Visible="false">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <div class="alert alert-danger alert-dismissible" role="alert" >
                                                <div>
                                                    <asp:Label ID="LabelAlertaErrorArchivo" runat="server" Text="" CssClass="h6 fw-semibold"></asp:Label>
                                                </div>
                                                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TextBoxDescDG" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Image ID="imgLoaderImportarArchivo" runat="server" ImageUrl="~/public/img/loader.gif" Width="90px" Height="60px"/>
                                        <ajaxToolkit:AsyncFileUpload ID="AsyncFileUploadImportarArchivo" runat="server" CssClass="FileUploadClass form-control border-primary text-primary" ThrobberID="imgLoaderImportarArchivo" PersistFile="True" UploadingBackColor="#CCFFFF" CompleteBackColor="Transparent" UploaderStyle="Traditional"  ViewStateMode="Disabled" />
                                    </div>
                                </div>
                                <div class="row pt-3">
                                    <div class="col-md-12 text-center">
                                        <asp:Button ID="ButtonGuardarDG" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="ButtonGuardarDG_Click" />
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelVerDG" runat="server" CssClass="pt-2">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TextBoxVerDescDG" runat="server" TextMode="MultiLine" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6 text-center">
                                        <asp:ImageButton ID="ImageButtonVerArchivoDG" runat="server" ImageUrl="~/public/img/documento.png" Width="35px" Height="35px" />
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

    <!--Modal Gestiones-->
    <div class="modal fade" id="ModalVerDG" data-bs-backdrop="static" aria-hidden="true" aria-labelledby="ModalVerDGLabel" tabindex="-1">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <h5 class="modal-title fw-bolder" id="ModalVerDGLabel">
                                Gestiones - <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                            </h5>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
         
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelTitVerDG" runat="server" Text="Evidencias de cumplimiento" CssClass="fw-bolder"></asp:Label>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Aceptar</button>
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

    <script type="text/javascript">
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

        function ShowModalDG() {
            var myModal = document.getElementById('ModalDG');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

        function ShowModalGestiones() {
            var myModal = document.getElementById('ModalGestiones');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
        }

    </script>
                           
</asp:Content>

