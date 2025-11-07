<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html lang="en">
    <head runat="server">
        <meta charset="utf-8">
        <meta content="width=device-width, initial-scale=1.0" name="viewport">

        <title>SADES</title>
        <meta content="" name="description">
        <meta content="" name="keywords">

        <!-- Favicons -->
        <link rel="icon" sizes="192x192" href="public/img/logo-ipn1.png">

        <!-- Google Fonts -->
        <link href="https://fonts.gstatic.com" rel="preconnect">
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i|Nunito:300,300i,400,400i,600,600i,700,700i|Poppins:300,300i,400,400i,500,500i,600,600i,700,700i" rel="stylesheet">

        <!-- Vendor CSS Files -->
        <link href="public/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
        <link href="public/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
        <link href="public/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
        <link href="public/vendor/quill/quill.snow.css" rel="stylesheet">
        <link href="public/vendor/quill/quill.bubble.css" rel="stylesheet">
        <link href="public/vendor/remixicon/remixicon.css" rel="stylesheet">
        <link href="public/vendor/simple-datatables/style.css" rel="stylesheet">

        <!-- Template Main CSS File -->
        <link href="public/css/style.css" rel="stylesheet">

        <%--Cabeceras IPN--%>
        <!-- Meta para la cabecera Content Security Policy-->
        <meta name="apple-mobile-web-app-capable" content="yes">
        <!--<meta http-equiv="Content-Security-Policy" content="default-src *; script-src 'unsafe-inline' 'unsafe-eval'">-->
        <!-- icon in the highest resolution we need it for -->
        <style type="text/css">svg:not(:root).svg-inline--fa{overflow:visible}.svg-inline--fa{display:inline-block;font-size:inherit;height:1em;overflow:visible;vertical-align:-.125em}.svg-inline--fa.fa-lg{vertical-align:-.225em}.svg-inline--fa.fa-w-1{width:.0625em}.svg-inline--fa.fa-w-2{width:.125em}.svg-inline--fa.fa-w-3{width:.1875em}.svg-inline--fa.fa-w-4{width:.25em}.svg-inline--fa.fa-w-5{width:.3125em}.svg-inline--fa.fa-w-6{width:.375em}.svg-inline--fa.fa-w-7{width:.4375em}.svg-inline--fa.fa-w-8{width:.5em}.svg-inline--fa.fa-w-9{width:.5625em}.svg-inline--fa.fa-w-10{width:.625em}.svg-inline--fa.fa-w-11{width:.6875em}.svg-inline--fa.fa-w-12{width:.75em}.svg-inline--fa.fa-w-13{width:.8125em}.svg-inline--fa.fa-w-14{width:.875em}.svg-inline--fa.fa-w-15{width:.9375em}.svg-inline--fa.fa-w-16{width:1em}.svg-inline--fa.fa-w-17{width:1.0625em}.svg-inline--fa.fa-w-18{width:1.125em}.svg-inline--fa.fa-w-19{width:1.1875em}.svg-inline--fa.fa-w-20{width:1.25em}.svg-inline--fa.fa-pull-left{margin-right:.3em;width:auto}.svg-inline--fa.fa-pull-right{margin-left:.3em;width:auto}.svg-inline--fa.fa-border{height:1.5em}.svg-inline--fa.fa-li{width:2em}.svg-inline--fa.fa-fw{width:1.25em}.fa-layers svg.svg-inline--fa{bottom:0;left:0;margin:auto;position:absolute;right:0;top:0}.fa-layers{display:inline-block;height:1em;position:relative;text-align:center;vertical-align:-.125em;width:1em}.fa-layers svg.svg-inline--fa{-webkit-transform-origin:center center;transform-origin:center center}.fa-layers-counter,.fa-layers-text{display:inline-block;position:absolute;text-align:center}.fa-layers-text{left:50%;top:50%;-webkit-transform:translate(-50%,-50%);transform:translate(-50%,-50%);-webkit-transform-origin:center center;transform-origin:center center}.fa-layers-counter{background-color:#ff253a;border-radius:1em;-webkit-box-sizing:border-box;box-sizing:border-box;color:#fff;height:1.5em;line-height:1;max-width:5em;min-width:1.5em;overflow:hidden;padding:.25em;right:0;text-overflow:ellipsis;top:0;-webkit-transform:scale(.25);transform:scale(.25);-webkit-transform-origin:top right;transform-origin:top right}.fa-layers-bottom-right{bottom:0;right:0;top:auto;-webkit-transform:scale(.25);transform:scale(.25);-webkit-transform-origin:bottom right;transform-origin:bottom right}.fa-layers-bottom-left{bottom:0;left:0;right:auto;top:auto;-webkit-transform:scale(.25);transform:scale(.25);-webkit-transform-origin:bottom left;transform-origin:bottom left}.fa-layers-top-right{right:0;top:0;-webkit-transform:scale(.25);transform:scale(.25);-webkit-transform-origin:top right;transform-origin:top right}.fa-layers-top-left{left:0;right:auto;top:0;-webkit-transform:scale(.25);transform:scale(.25);-webkit-transform-origin:top left;transform-origin:top left}.fa-lg{font-size:1.33333em;line-height:.75em;vertical-align:-.0667em}.fa-xs{font-size:.75em}.fa-sm{font-size:.875em}.fa-1x{font-size:1em}.fa-2x{font-size:2em}.fa-3x{font-size:3em}.fa-4x{font-size:4em}.fa-5x{font-size:5em}.fa-6x{font-size:6em}.fa-7x{font-size:7em}.fa-8x{font-size:8em}.fa-9x{font-size:9em}.fa-10x{font-size:10em}.fa-fw{text-align:center;width:1.25em}.fa-ul{list-style-type:none;margin-left:2.5em;padding-left:0}.fa-ul>li{position:relative}.fa-li{left:-2em;position:absolute;text-align:center;width:2em;line-height:inherit}.fa-border{border:solid .08em #eee;border-radius:.1em;padding:.2em .25em .15em}.fa-pull-left{float:left}.fa-pull-right{float:right}.fa.fa-pull-left,.fab.fa-pull-left,.fal.fa-pull-left,.far.fa-pull-left,.fas.fa-pull-left{margin-right:.3em}.fa.fa-pull-right,.fab.fa-pull-right,.fal.fa-pull-right,.far.fa-pull-right,.fas.fa-pull-right{margin-left:.3em}.fa-spin{-webkit-animation:fa-spin 2s infinite linear;animation:fa-spin 2s infinite linear}.fa-pulse{-webkit-animation:fa-spin 1s infinite steps(8);animation:fa-spin 1s infinite steps(8)}@-webkit-keyframes fa-spin{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);transform:rotate(360deg)}}@keyframes fa-spin{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);transform:rotate(360deg)}}.fa-rotate-90{-webkit-transform:rotate(90deg);transform:rotate(90deg)}.fa-rotate-180{-webkit-transform:rotate(180deg);transform:rotate(180deg)}.fa-rotate-270{-webkit-transform:rotate(270deg);transform:rotate(270deg)}.fa-flip-horizontal{-webkit-transform:scale(-1,1);transform:scale(-1,1)}.fa-flip-vertical{-webkit-transform:scale(1,-1);transform:scale(1,-1)}.fa-flip-horizontal.fa-flip-vertical{-webkit-transform:scale(-1,-1);transform:scale(-1,-1)}:root .fa-flip-horizontal,:root .fa-flip-vertical,:root .fa-rotate-180,:root .fa-rotate-270,:root .fa-rotate-90{-webkit-filter:none;filter:none}.fa-stack{display:inline-block;height:2em;position:relative;width:2em}.fa-stack-1x,.fa-stack-2x{bottom:0;left:0;margin:auto;position:absolute;right:0;top:0}.svg-inline--fa.fa-stack-1x{height:1em;width:1em}.svg-inline--fa.fa-stack-2x{height:2em;width:2em}.fa-inverse{color:#fff}.sr-only{border:0;clip:rect(0,0,0,0);height:1px;margin:-1px;overflow:hidden;padding:0;position:absolute;width:1px}.sr-only-focusable:active,.sr-only-focusable:focus{clip:auto;height:auto;margin:0;overflow:visible;position:static;width:auto}</style>
        <!-- Iconos bootstrap -->
        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
         <%--1er cabecera--%>
        <link rel="stylesheet" href="public/ipn/estilo-compresion.min.css"> 


        <style>
            .icon-no-border {
              border: 0;
            }
        </style>

    </head>
    <body>
        <header class="d-none d-md-block">
            <nav class="navbar fixed-top" role="navigation" id="barraGobmx2">
                <div class="container u-noPaddingContainer">
                    <a class="navbar-brand" style="padding-left: 8px;" href="https://www.gob.mx/">
                        <img src="public/ipn/logob.svg" height="29" alt="Página de inicio, Gobierno de México">
                    </a>
                    <div class="text-rigth barraGobmx-enlaces2">
                       <a href="https://mivacuna.salud.gob.mx/" title="Registro para vacunación" class="nav-link">
                            Registro para vacunación
                        </a>  
                        <a href="https://coronavirus.gob.mx/" title="Informacion sobre COVID-19" class="nav-link">
                            Informacion sobre COVID-19
                        </a>                    
                        <a href="https://www.gob.mx/tramites" title="Trámites" class="nav-link">
                            Trámites
                        </a>
                        <a href="https://www.gob.mx/gobierno" title="Gobierno" class="nav-link">
                            Gobierno
                        </a>
                        <a href="https://www.gob.mx/segob/en" title="English" class="nav-link">
                            English
                        </a>

                        <a href="https://www.gob.mx/busqueda">
                        <span class="sr-only nav-link">Búsqueda</span><svg class="svg-inline--fa fa-search fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="search" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M505 442.7L405.3 343c-4.5-4.5-10.6-7-17-7H372c27.6-35.3 44-79.7 44-128C416 93.1 322.9 0 208 0S0 93.1 0 208s93.1 208 208 208c48.3 0 92.7-16.4 128-44v16.3c0 6.4 2.5 12.5 7 17l99.7 99.7c9.4 9.4 24.6 9.4 33.9 0l28.3-28.3c9.4-9.4 9.4-24.6.1-34zM208 336c-70.7 0-128-57.2-128-128 0-70.7 57.2-128 128-128 70.7 0 128 57.2 128 128 0 70.7-57.2 128-128 128z"></path></svg><!-- <i class="fas fa-search"></i> -->
                        </a>
                    </div>
                </div>
            </nav>
        </header>
        <div class="pace  pace-inactive">
            <div class="pace-progress" data-progress-text="100%" data-progress="99" style="transform: translate3d(100%, 0px, 0px);">
                <div class="pace-progress-inner">
                </div>
            </div>
            <div class="pace-activity">
            </div>
        </div>
          <!-- Encabezado principal -->
        <div class="u-noPaddingContainer contenedorGobierno">
            <div class="container">
                <div class="row no-gutters">
                    <div class="col-md-12">
                        <div class="d-inline-block ipnLogo-enlace">
                            <a href="https://www.gob.mx/sep" class="">
                                <img src="public/ipn/pleca-educacion.svg" alt=" Pleca de Gobierno " class="plecaGob gob">
                            </a>
                            <div class="d-inline-block ipnLogo-enlace">
                                <a href="https://www.ipn.mx/" class="">
                                    <img src="public/ipn/logo_ipn_guinda.svg" alt=" Logo IPN " class="plecaIPN">
                                    <p class="d-inline-block tituloLogo">
                                        Instituto Politécnico Nacional <br>
                                        <span class="sm">"La Técnica al Servicio de la Patria"</span>
                                    </p>
                                </a>   
                            </div>
                        </div>
                    </div>
                </div>
                <div class="utileriasIpn">
                    <ul class="barra-enlaces">
                        <li>
                            <a href="https://www.ipn.mx/directorio-telefonico.html">DIRECTORIO</a> |
                        </li>
                        <li>
                            <a href="https://www.ipn.mx/correo-electronico.html">CORREO</a> |
                        </li>
                        <li>
                            <a href="https://www.ipn.mx/calendario-academico.html">CALENDARIO</a> |
                        </li>
                        <li>
                            <a href="https://www.ipn.mx/transparencia/">TRANSPARENCIA</a> |
                        </li>
                        <li>
                            <a href="https://www.ipn.mx/proteccion-datos-personales/">PROTECCIÓN DE DATOS</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <main>
            <div class="container">
                <section class="section register d-flex flex-column align-items-center justify-content-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-5 col-md-6 d-flex flex-column align-items-center justify-content-center">
                                <div class="d-flex justify-content-center py-2">
                                    <a href="#" class="logo d-flex align-items-center w-auto">
                                    <!-- <img src="/img/logo.png" alt=""> -->
                                    <span class="d-lg-block">SADES IPN</span>
                                    </a>
                                </div><!-- End Logo -->

                                <div class="card mb-3">
                                    <div class="card-body">
                                        <div class="pt-2 pb-2">
                                            <h5 class="card-title text-center pb-0 fs-4">Iniciar sesión</h5>
                                            <p class="text-center small"></p>
                                        </div>
                                        <form runat="server" class="row g-3 needs-validation" novalidate>

                                            <asp:HiddenField runat="server" ID="HiddenField_error"/>

                                            <asp:Login ID="Login" runat="server" OnAuthenticate="Login_Authenticate" CssClass="login100-form validate-form" >
						                        <LayoutTemplate>
                                                    <div class="col-12">
                                                        <label for="yourUsername" class="form-label">Usuario</label>
                                                            <div class="input-group has-validation">
                                                            <%--<input type="text" name="username" class="form-control" id="yourUsername" required>--%>
                                                                <asp:TextBox ID="Username" runat="server" CssClass="form-control" required ></asp:TextBox>
                                                                <%--<span class="input-group-text" id="inputGroupPrepend">@ipn.mx</span>--%>
                                                                <div class="invalid-feedback">Ingresa tu usuario.</div>
                                                            </div>
                                                    </div>
                                                    <div class="col-12">
                                                        <label for="yourPassword" class="form-label">Contraseña</label>
                                                        <div class="input-group has-validation">   
                                                            <%--<input type="password" name="password" class="form-control" id="yourPassword" required>--%>
                                                            <asp:TextBox ID="Password" runat="server" class="form-control" ClientIDMode="Static" TextMode="Password" required></asp:TextBox>
                                                            <button id="show_password" class="btn btn-primary" type="button" onclick="mostrarPassword()"> <span class="bi bi-eye-slash"></span> </button>
                                                            <div class="invalid-feedback">Ingresa tu password.</div>
                                                        </div>
                                                    </div>
                                                    <p></p>
                                                    <div id="DivAlert_login" class="col-12 alert alert-danger" style="display:none">
                                                        <asp:Label runat="server" ID="FailureText"></asp:Label>
                                                    </div>

                                                    <div class="col-12">
                                                        <div class="form-check">
                                                            <input class="form-check-input" type="checkbox" name="remember" value="true" id="rememberMe">
                                                            <label class="form-check-label" for="rememberMe">Recordarme</label>
                                                        </div>
                                                    </div>

                                                    <div class="col-12">
                                                        <%--<button class="btn btn-primary w-100" type="submit">Iniciar Sesión</button>--%>
                                                        <asp:button ID="BtnLogin" runat="server" text="Iniciar Sesión" class="btn btn-primary w-100" CommandName="Login" ValidationGroup="Login" />
                                                    </div>
                                                </LayoutTemplate>
                                            </asp:Login>
                                            <div class="col-12">
                                                <!-- <p class="small mb-0 ">¿No tienes cuenta institucional? <a href="#">Solicita una cuenta local</a></p> -->
                                            </div>
                                            <asp:Panel ID="Login_Perfil" runat="server" Visible="false" CssClass="p-t-30">
                                                <asp:Label ID="Label1" runat="server" Text="Elegir perfil para iniciar sesión:" CssClass=" fs-20"></asp:Label>

                                                <asp:GridView ID="GridViewPerfiles" runat="server" AutoGenerateColumns="False" 
                                                    DataKeyNames="ID_Perfil"  HeaderStyle-HorizontalAlign="Center"
							                        DataSourceID="SqlDataSourcePerfiles" class="table table-striped" 
                                                    PagerStyle-CssClass="pagination-sm" ShowHeader="false"
                                                    onselectedindexchanged="GridViewPerfiles_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="ID_Perfil" HeaderText="ID_Perfil" ReadOnly="True" 
                                                        SortExpression="ID_Perfil" Visible="false" />
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Perfil"  
                                                        SortExpression="DESCRIPCION" HeaderStyle-CssClass="text-center"/>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:Button ID="ButtonPR" CssClass="btn btn-primary" runat="server" Text="Seleccionar" CommandName="select" CausesValidation="false" OnClientClick="this.disabled=true" UseSubmitBehavior="False" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                </asp:GridView>
                                                <asp:SqlDataSource ID="SqlDataSourcePerfiles" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:ConnectionDES %>" >
                                                </asp:SqlDataSource>
                                            </asp:Panel>
                                        </form> 
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </main><!-- End #main -->
        <footer class="footer">
            <div class="copyright">
                Desarrollado por <strong>
                <span>IPN - Dirección de Educación Superior.</span>
                </strong>
            </div>
            <div class="credits">
                <img src="public/img/pie-ipn.png" class="img-fluid" height="50%" width="50%">
            </div>
        </footer><!-- End Footer -->

        <!-- Vendor JS Files -->
        <script src="public/vendor/apexcharts/apexcharts.min.js"></script>
        <script src="public/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="public/vendor/chart.js/chart.min.js"></script>
        <script src="public/vendor/echarts/echarts.min.js"></script>
        <script src="public/vendor/quill/quill.min.js"></script>
        <script src="public/vendor/simple-datatables/simple-datatables.js"></script>
        <script src="public/vendor/tinymce/tinymce.min.js"></script>
        <script src="public/vendor/php-email-form/validate.js"></script>

        <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
        <script src="public/js/jquery/jquery-3.2.1.js"></script>
        <script src="public/js/sweetalert/sweetalert2.11.js"></script>

        <!-- Template Main JS File -->
        <script src="public/js/main.js"></script>

        <script>

            $(document).ready(function () {
                validLogIn();
            });

            function validLogIn() {
                let legend = "";
                let icon = "";

                let errorLogIn = $("[id*='HiddenField_error']").val();
                console.log("error: "+errorLogIn);
                if (errorLogIn != "") {

                    switch (errorLogIn) {
                        case "1":
                        case "2":
                            legend = "Error al ingresar al sistema";
                            icon = '<i class="bi bi-x-circle" style="color: lightcoral;"></i>';
                            break;
                        case "3":
                            legend = 'Perfil deshabilitado';
                            icon = '<i class="bi bi-person-fill-slash" style="color: lightcoral;"></i>';
                            break;
                        case "4":
                            legend = 'Perfil habilitado';
                            icon = '<i class="bi bi-x-circle" style="color: lightcoral;"></i>';
                            break;
                    }

                    $("#DivAlert_login").show();

                    const Toast = Swal.mixin({
                        toast: true,
                        position: "center",
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.onmouseenter = Swal.stopTimer;
                            toast.onmouseleave = Swal.resumeTimer;
                        }
                    });
                    Toast.fire({
                        //icon: "error",
                        //title: legend,
                        iconHtml: icon,
                        html: legend,
                        customClass: {
                            icon: 'icon-no-border'
                        }
                    });

                    setTimeout(function () {
                        $("#DivAlert_login").hide("slow");
                    }, 6000);
                }
            }

            //function mostrarPassword(){
      //    var tipo = document.getElementById("Password");
      //    if(tipo.type == "password"){
      //        tipo.type = "text";
      //    }else{
      //        tipo.type = "password";
      //    }
      //  }

      function mostrarPassword() {
          var tipo = document.getElementById("Password");
          var boton = document.getElementById("show_password");
          if (tipo.type === "password") {
              tipo.type = "text";
              boton.innerHTML = '<span class="bi bi-eye"></span>';
          } else {
              tipo.type = "password";
              boton.innerHTML = '<span class="bi bi-eye-slash"></span>';
          }
      }
        </script>
    </body>
</html>