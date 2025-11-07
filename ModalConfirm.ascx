<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalConfirm.ascx.cs" Inherits="ModalConfirm" %>

<style>
    .my-icon-check {
        color: forestgreen;
    }

    .my-icon-error {
        color: crimson;
    }
</style>

<div class="modal fade" id="modalConfirm" tabindex="-1">
    <%--class="alert alert-info alert-dismissible fade show"--%>
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header ">
                <%--<h5 class="modal-title">¡Atención!</h5>--%>
                <%--<i class="bi bi-info"></i>--%>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div id="DivConfirmCheck" runat="server" class="text-center" visible="false">
                            <i class="fas fa-check-circle fa-2x my-icon-check"></i>
                        </div>
                        <div id="DivConfirmError" runat="server" class="text-center" visible="false">
                            <i class="fas fa-times-circle fa-2x my-icon-error"></i>
                        </div>
                        <div class="text-center">
                            <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
    </div>
</div>
