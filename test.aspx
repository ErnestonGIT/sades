<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>


    <!-- Modal  NuevoNombramiento-->
    <div class="modal fade" id="ModalNuevoNombramiento" tabindex="-1" data-bs-backdrop="static">
        <div class="modal-dialog modal-fullscreen">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title titleModal" id="tittleModalNuevoNombramiento">Motor de búsqueda</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <!-- Modal -->
                <div id="DivModalNuevoNombramiento_body" runat="server" class="col-md-12 dashboard">
                    
                    <div class="card info-card customers-card">
                        <div class="filter" style="display:none">

                            <a class="icon" href="#" data-bs-toggle="dropdown"><i class="bi bi-three-dots"></i></a>
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                                <li class="dropdown-header text-start">
                                    <h6>Exportar</h6>
                                </li>
                                <li class="dropdown-item" runat="server" id="ModalNuevoNombramiento_dropdownItem">
                                    <i class="bi bi-filetype-xlsx iconExcel"></i><asp:Button ID="LinkButtonModalNuevoNombramiento_Excel" runat="server" Text="Excel" CssClass="btn btn-outline-default" Visible="true" />
                                </li>
                            </ul>

                        </div>
                        <div class="card-body">

                            <asp:UpdatePanel runat="server"><ContentTemplate>
                                <br />
                                <h5><asp:Label runat="server" ID="LabelModalNuevoNombramiento_titulo" Text="" CssClass="card-title"></asp:Label><span class="card-title"> | <asp:Label ID="LabelModalNuevoNombramiento_subtitulo" runat="server" Text=""></asp:Label></span></h5>
                                <br />

                                <div class="d-flex align-items-center">
                                    <div class="ps-3">
                                        <span class="text-success small pt-1 fw-bold">Pliego: </span>
                                            <asp:Label runat="server" ID="LabelModalNuevoNombramiento_text0" CssClass="text-muted small pt-2 ps-1"></asp:Label><br />
                                    </div>
                                </div>
                                <div class="mb-3">
                                    &nbsp;
                                </div>

                                <div class="card">

                                    <div class="row">
                                        <div class="mb-3">
                                            <label for="DropDownListNuevoNombramiento_ua" class="form-label">Unidad académica:</label>
                                            <asp:DropDownList ID="DropDownListNuevoNombramiento_ua" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropDownNuevoNombramiento_ua"
                                                DataValueField="CLAVE_ZP"
                                                DataTextField="DESCRIPCION_DP" 
                                                CssClass="form-select select-posicion" data-control="select2"
                                                OnDataBound="DropDownListNuevoNombramiento_ua_DataBound"
                                                OnSelectedIndexChanged="DropDownListNuevoNombramiento_ua_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSourceDropDownNuevoNombramiento_ua" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                SelectCommand="SELECT CLAVE_ZP, DESCRIPCION_DP FROM  CAT_DEPENDENCIAS_POLITECNICAS
                                                                WHERE ID_NIVEL_EST = 2 and CLAVE_ZP in (select distinct CLAVE_ZP from PLIEGO)
                                                                ORDER BY DESCRIPCION_DP ASC">
                                            </asp:SqlDataSource>
                                        </div>
                                        <div class="mb-3">
                                            <label for="DropDownListNuevoNombramiento_uad" class="form-label">Unidad administrativa:</label>
                                            <asp:DropDownList ID="DropDownListNuevoNombramiento_uad" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceDropDownNuevoNombramiento_uad"
                                                DataValueField="CLAVE_ZP"
                                                DataTextField="DESCRIPCION_DP" 
                                                CssClass="form-select select-posicion" data-control="select2"
                                                OnDataBound="DropDownListNuevoNombramiento_uad_DataBound"
                                                OnSelectedIndexChanged="DropDownListNuevoNombramiento_uad_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSourceDropDownNuevoNombramiento_uad" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDES %>"
                                                SelectCommand="select ID_PERFIL,
                                                                     case
                                                                         when DESCRIPCION like 'JEFE DEL %' then REPLACE(DESCRIPCION,'JEFE DEL ','')
                                                                         when DESCRIPCION like 'JEFE DE LA %' then REPLACE(DESCRIPCION,'JEFE DE LA ','')
                                                                         else DESCRIPCION
                                                                     end UNIDAD
                                                                from CAT_PERFILES
                                                                where CLAVE_ZP = @ZP or ID_PERFIL in(11,12,13,14)">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListNuevoNombramiento_ua" Name="ZP" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>

                                </div>

                            </ContentTemplate></asp:UpdatePanel>
                        </div>
                    </div>
                    
                </div><!-- End Modal NuevoNombramiento Card -->
      
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
        </div>
    </div>


        </div>
    </form>
</body>
</html>
