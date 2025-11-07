<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PeticionesEst.aspx.cs" Inherits="PeticionesEst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/ModalConfirm.ascx" TagPrefix="uc" TagName="ModalConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" />
    <!---------------------->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2-bootstrap-5-theme@1.3.0/dist/select2-bootstrap-5-theme.min.css" />

     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <main id="main1" class="main">
                <div class="pagetitle">
                    <%--<h1>Catálogo de formación académica del docente</h1>--%>
                    <h1>Registrar peticiones</h1>
                    <%--<nav>
                        <ol class="breadcrumb">
                            <!-- <li class="breadcrumb-item"><a href="index">Inicio</a></li> -->
                            <li class="breadcrumb-item active">
                                <asp:Label ID="LabelDependencia" runat="server" Text=""></asp:Label>
                            </li>
                        </ol>
                    </nav>--%>
                </div>                <!-- End Page Title -->
                <section class="section">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <!-- General Form Elements -->
                                    <div class="mt-4">
                                        <form>
                                            <%--ddl unidad academica--%>
                                            <div class="row mb-3 mx-auto">
                                                <label for="DropDownListUnidadAcademica" class="col-xl-3 col-lg-3 col-md-3 col-sm-12 col-12 col-form-label">Unidad académica:</label>
                                                <div class="col-xl-5 col-lg-5 col-md-5 col-sm-12 col-12">
                                                    <asp:DropDownList ID="DropDownListUnidadAcademica" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropUA"
                                                        DataTextField="DESCRIPCION_DP" DataValueField="CLAVE_ZP" CssClass="form-select border-primary" data-control="select2"
                                                        OnDataBound="DropDownListUnidadAcademica_DataBound">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSourceDropUA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                        SelectCommand="SELECT CLAVE_ZP, DESCRIPCION_DP FROM  CAT_DEPENDENCIAS_POLITECNICAS
WHERE CLAVE_UA IS NOT NULL
AND ID_NIVEL_EST = 2
ORDER BY CLAVE_UA"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <%--ddl categorias--%>
                                            <div class="row mb-3 mx-auto">
                                                <label for="DDLCategoriaPeticion" class="col-xl-3 col-lg-3 col-md-3 col-sm-12 col-12 col-form-label">Categoría:</label>
                                                <div class="col-xl-5 col-lg-5 col-md-5 col-sm-12 col-12">
                                                    <asp:DropDownList ID="DDLCategoriaPeticion" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDdlCategoriaPet"
                                                        DataTextField="DESCRIPCION_CAT_PETICION" DataValueField="ID_CAT_PETICION" CssClass="form-select border-primary"
                                                        OnDataBound="DDLCategoriaPeticion_DataBound">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSourceDdlCategoriaPet" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                        SelectCommand="SELECT ID_CAT_PETICION, DESCRIPCION_CAT_PETICION
FROM CAT_CATEGORIA_PETICION"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <%--Fecha peticion--%>
                                            <div id="divFechaPeticion" runat="server" class="row mb-3 mx-auto">
                                                <label for="TextBoxDatePeticion" class="col-xl-3 col-lg-3 col-md-3 col-sm-12 col-12 col-form-label">Fecha de petición:</label>
                                                <div class="col-xl-5 col-lg-5 col-md-5 col-sm-12 col-12">
                                                    <asp:TextBox ID="TextBoxDatePeticion" runat="server" class="form-control" Text="" type="date"></asp:TextBox>
                                                    <%--<input type="date" class="form-control">--%>
                                                </div>
                                            </div>
                                            <%--tXt Peticion--%>
                                            <div class="row mb-3 mx-auto">
                                                <label for="TextBoxPeticion" class="col-xl-3 col-lg-3 col-md-3 col-sm-12 col-12 col-form-label">Petición:</label>
                                                <div class="col-xl-8 col-lg-8 col-md-8 col-sm-12 col-12">
                                                    <asp:TextBox ID="TextBoxPeticion" runat="server" class="form-control border-primary" Text="" TextMode="MultiLine" Rows="3" placeholder="Ingrese la petición"></asp:TextBox>
                                                </div>
                                            </div>
                                            <%--Fecha solucion--%>
                                            <div id="divFechaAccSol" runat="server" class="row mb-3 mx-auto">
                                                <label for="TextBoxDateAccSol" class="col-xl-3 col-lg-3 col-md-3 col-sm-12 col-12 col-form-label">Fecha de acción/solución:</label>
                                                <div class="col-xl-5 col-lg-5 col-md-5 col-sm-12 col-12">
                                                    <asp:TextBox ID="TextBoxDateAccSol" runat="server" class="form-control" Text="" type="date"></asp:TextBox>
                                                    <%--<input type="date" class="form-control">--%>
                                                </div>
                                            </div>
                                            <%--txt accion solucion--%>
                                            <div class="row mb-3 mx-auto">
                                                <label for="TextBoxRespuesta" class="col-xl-3 col-lg-3 col-md-3 col-sm-12 col-12 col-form-label">Acción/Solución:</label>
                                                <div class="col-xl-8 col-lg-8 col-md-8 col-sm-12 col-12">
                                                    <asp:TextBox ID="TextBoxRespuesta" runat="server" class="form-control" Text="" TextMode="MultiLine" Rows="3" placeholder="Ingrese la acción o solución"></asp:TextBox>
                                                </div>
                                            </div>
                                            

                                            <div class="row mb-3 mx-auto">
                                                <%--<label class="col-sm-2 col-form-label"></label>--%>
                                                <div class="text-center">
                                                    <asp:Button ID="ButtonAddPeticion" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="ButtonAddPeticion_Click"/>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                </main>
            </ContentTemplate>
         </asp:UpdatePanel>

    <%-------------MODALS---------------%>
    <%-- Modal de confirmación --%>
    <uc:ModalConfirm runat="server" ID="modalConfirm" />

    <%--scripts--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.full.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/i18n/es.js"></script>

 
     <%--aplicar select2 en los dropdowlists--%>
    <script type="text/javascript">
        $(document).ready(function () {
            const themeBtp5 = "bootstrap-5";
            const cssClassCont = "form-select border-primary";
            const dimension = '100%';

            $(function () {
                //para establecer el lenguaje en español
                $.fn.select2.defaults.set('language', 'es');

                $("[id*=DropDownListUnidadAcademica]").select2({
                    theme: themeBtp5,
                    containerCssClass: cssClassCont,
                    width: dimension,
                });               
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();

            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    $(function () {
                        $("[id*=DropDownListUnidadAcademica]").select2({
                            theme: themeBtp5,
                            containerCssClass: cssClassCont,
                            width: dimension,
                        });                        
                    });
                });
            }
        });
    </script>

    <%--scripts modals--%>
    <script>
        function ShowModalConfirm() {
            var myModal = document.getElementById('modalConfirm');
            var modal = bootstrap.Modal.getOrCreateInstance(myModal);
            modal.show();
            //myModal.style.zIndex = '1100'
        }
        </script>
</asp:Content>