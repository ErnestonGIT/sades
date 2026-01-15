using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NotificarDatos : System.Web.UI.Page
{

    string emptyZP_name = "Instituto Politécnico Nacional";
    string constr = ConfigurationManager.ConnectionStrings["ConnectionDES"].ConnectionString;

    string perfil = HttpContext.Current.Request.Cookies["Tipo"].Value.ToString();
    string idUser = HttpContext.Current.Request.Cookies["id_usuario"].Value.ToString();
    string chP = HttpContext.Current.Request.Cookies["chP"].Value.ToString();

    string zp = HttpContext.Current.Request.Cookies["claveZP"].Value.ToString(); //HttpContext.Current.Request.QueryString["zp"];
    string pe = HttpContext.Current.Request.Cookies["pe"].Value.ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!String.IsNullOrEmpty(chP))
        {
            if (!IsPostBack)
            {

                LabelPerfil.Text = perfil;
                LabelZP.Text = zp;
                LabelPE.Text = pe;
                LabelZPDesc.Text = GetUaDesciption(zp);

                validarContenido(perfil);

            }
            else
            {

            }
        }
    }
    public void ShowModal(string idModal)
    {
        string script = "ShowModal('" + idModal + "');";
        ScriptManager.RegisterStartupScript(this, GetType(), "script", script, true);
    }
    private string GetUaDesciption(string zp)
    {
        return Consultas.ConsultaS("SELECT DESCRIPCION_DP FROM CAT_DEPENDENCIAS_POLITECNICAS WHERE CLAVE_ZP = '"+ zp +"'");
    }
    private void validarContenido(string perfil)
    {
        switch (perfil)
        {
            case "7":

                break;
            case "19":

                break;
            case "43":
                break;
            case "432"://super administrador
                mostrarPanelAdministrador(true);
                break;
        }
    }
    public void mostrarPanelAdministrador(bool data)
    {
        string nivel = HiddenFieldPerfil_nivel.Value;
        HiddenFieldCollapseNotificarDatos_selected.Value  = "1";
        divPanelNotificarDatos.Visible = data;
    }

    protected void LinkButtonNotificarDatos_enviar_Click(object sender, EventArgs e)
    {
        string em = TextBoxNotificarDatos_correo.Text;
        string us = TextBoxNotificarDatos_usuario.Text;
        string ps = TextBoxNotificarDatos_password.Text;

        if(!String.IsNullOrEmpty(em) && !String.IsNullOrEmpty(us) && !String.IsNullOrEmpty(ps))
        {
            enviarCorreoAsignacion(em, us, ps);
        }
        else
        {
            LabelEnvioCorreo_estatus.Text = "Debera proporcionar todos los datos para enviar el correo.";
        }
        
    }

    private void limpiarCamposAlta()
    {
        TextBoxNotificarDatos_correo.Text = string.Empty;
        TextBoxNotificarDatos_usuario.Text = string.Empty;
        TextBoxNotificarDatos_password.Text = string.Empty;
        LabelEnvioCorreo_estatus.Text = string.Empty;
    }

    public bool enviarCorreoAsignacion(string destino, string us, string ps)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        var mail = new MailMessage();
        var smtp = new SmtpClient();

        string from = "saiee.i.p.n.m.x@gmail.com";
        string fromAlias = "sades@ipn.mx";
        string password = "tjzq ixji anst dccv";

        string html = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n    <title></title>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n    <style type=\"text/css\">\r\n        @media screen {\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local(\\'Lato Regular\\'), local(\\'Lato-Regular\\'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format(\\'woff\\');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local(\\'Lato Bold\\'), local(\\'Lato-Bold\\'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format(\\'woff\\');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: italic;\r\n                font-weight: 400;\r\n                src: local(\\'Lato Italic\\'), local(\\'Lato-Italic\\'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format(\\'woff\\');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: \\'Lato\\';\r\n                font-style: italic;\r\n                font-weight: 700;\r\n                src: local(\\'Lato Bold Italic\\'), local(\\'Lato-BoldItalic\\'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format(\\'woff\\');\r\n            }\r\n        }\r\n\r\n        /* CLIENT-SPECIFIC STYLES */\r\n        body,\r\n        table,\r\n        td,\r\n        a {\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n        }\r\n\r\n        table,\r\n        td {\r\n            mso-table-lspace: 0pt;\r\n            mso-table-rspace: 0pt;\r\n        }\r\n\r\n        img {\r\n            -ms-interpolation-mode: bicubic;\r\n        }\r\n\r\n        /* RESET STYLES */\r\n        img {\r\n            border: 0;\r\n            height: auto;\r\n            line-height: 100%;\r\n            outline: none;\r\n            text-decoration: none;\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse !important;\r\n        }\r\n\r\n        body {\r\n            height: 100% !important;\r\n            margin: 0 !important;\r\n            padding: 0 !important;\r\n            width: 100% !important;\r\n        }\r\n\r\n        /* iOS BLUE LINKS */\r\n        a[x-apple-data-detectors] {\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }\r\n\r\n        /* MOBILE STYLES */\r\n        @media screen and (max-width:600px) {\r\n            h1 {\r\n                font-size: 32px !important;\r\n                line-height: 32px !important;\r\n            }\r\n        }\r\n\r\n        /* ANDROID CENTER FIX */\r\n        div[style*=\"margin: 16px 0;\"] {\r\n            margin: 0 !important;\r\n        }\r\n\r\n    </style>\r\n</head>\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n        <!-- LOGO -->\r\n        <tr>\r\n            <td bgcolor=\"#872456\" align=\"center\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#872456\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\" style=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n" +
            "<h1 style=\"font-size: 40px; font-weight: 400; margin: 2;\">Alta de usuario!</h1>" +
            "<img src=\"http://148.204.112.186:8081/public/img/sadesCorreo.webp\" width=\"125\" height=\"120\" style=\"display: block; border: 0px;\" />\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 40px 30px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n" +
            "<p style=\"margin: 0;\">Bienvenido al Sistema de Apoyo para la Dirección de Educación Superior <strong>SADES</strong>.</p>\r\n                            <br>\r\n                            " +
            "<p style=\"margin: 0;\">Con el objetivo de que se de seguimiento a las acciones dentro del SADES, " +
            "le han sido generados los datos necesario para que pueda acceder al sistema.</p><br></td></tr> " +
            "<tr> " +
            "<td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 0px 30px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\"> " +
            "<table style=\"width: 85%\" border=\"1\" align=\"center\"> " +
                "<tr > " +
                    "<th >Usuario</th> " +
                    "<th >Contraseña</th> " +
                "</tr> " +
                "<tr align=\"center\"> " +
                    "<td >"+ us +"</td> " +
                    "<td >"+ ps +"</td> " +
                "</tr> " +
            "</table> " +
            "</td> " +
            "</tr> " +
            "<tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\">\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                <tr>\r\n                                    <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 40px 30px 10px 30px;\"> " +
            "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> " +
                "<tr> " +
                    "<td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#872456\"><a href=\"http://148.204.112.186:8081/\" target=\"_blank\" style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #872456; display: inline-block;\"> " +
                            "SADES | Ingresar</a> " +
                        "</td> " +
                "</tr> " +
            "</table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <img src=\"https://ipn.mx/assets/files/imageninstitucional/img/identidad/logotipos/portada-vertical.jpg\" height=\"200\" style=\"display: block; border: 0px;\" />\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#f4f4f4\" align=\"left\" style=\"padding: 0px 30px 30px 30px; color: #666666; font-family: \\'Lato\\', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 18px;\">\r\n                            <br>\r\n                            <p style=\"margin: 0;\">Secretaría Académica | Instituto Politécnico Nacional <a href=\"https://www.ipn.mx/seacademica/\" target=\"_blank\" style=\"color: #111111; font-weight: 700;\">(SECACADEMICA)</a>.</p>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";

        try
        {
            List<string> destinos = destino.Split(',').ToList();

            foreach (var item in destinos)
            {
                mail.To.Add(item);
            }

            mail.From = new MailAddress(fromAlias);

            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Subject = "SADES - Alta de usuario.";
            mail.Body = html;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            smtp.Host = ("smtp.gmail.com");
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(from, password);
            smtp.EnableSsl = true;

            smtp.Send(mail);

            limpiarCamposAlta();

            return true;

        }
        catch (Exception e)
        {
            LabelEnvioCorreo_estatus.Text = e.ToString();
            return false;
        }

    }
}