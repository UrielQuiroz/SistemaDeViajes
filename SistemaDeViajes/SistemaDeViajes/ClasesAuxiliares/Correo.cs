using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SistemaDeViajes.ClasesAuxiliares
{
    public class Correo
    {
        public static int EnviarCorreo(string nombreCorreo, string asunto, string contenido, string rutaError)
        {
            int rpta = 0;
            try
            {
                string correo = ConfigurationManager.AppSettings["correo"];
                string clave = ConfigurationManager.AppSettings["clave"];
                string servidor = ConfigurationManager.AppSettings["servidor"];
                int puerto = int.Parse(ConfigurationManager.AppSettings["puerto"]);

                //Datos del correo
                MailMessage mail = new MailMessage();
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = "<h1>" + contenido + "</h1>";
                mail.From = new MailAddress(correo);
                mail.To.Add(new MailAddress(nombreCorreo));

                //Envio de correo
                SmtpClient smtp = new SmtpClient();
                smtp.Host = servidor;
                smtp.EnableSsl = true;
                smtp.Port = puerto;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(correo, clave);
                smtp.Send(mail);
                rpta = 1;
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }
    }
}