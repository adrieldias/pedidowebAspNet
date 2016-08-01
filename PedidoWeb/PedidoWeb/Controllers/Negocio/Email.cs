using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace PedidoWeb.Controllers.Negocio
{
    public class Email
    {
        public string Destinatario { get; set; }
        public string[] CopiaCarbono { get; set; }
        public string Remetente { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
        public string SMTP { get; set; }
        public int Porta { get; set; }
        public string Senha { get; set; }
        public bool Ssl { get; set; }
        public string ImagemPath { get; set; }
        public string LogoCRC { get; set; }
        public string LogoMaisSaber { get; set; }
        public string logoRodape { get; set; }
        public bool TemImagem { get; set; }
        public void Enviar()
        {            
            try
            {
                // imagem
                if (TemImagem)
                {

                    AlternateView av = AlternateView.CreateAlternateViewFromString(Mensagem, null, System.Net.Mime.MediaTypeNames.Text.Html);

                    if (!string.IsNullOrEmpty(ImagemPath))
                    {
                        byte[] reader = File.ReadAllBytes(ImagemPath);
                        MemoryStream image1 = new MemoryStream(reader);
                        LinkedResource imgAcop = new LinkedResource(image1, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                        imgAcop.ContentId = "companyLogo";
                        imgAcop.ContentType = new ContentType("image/jpg");
                        av.LinkedResources.Add(imgAcop);
                    }

                    if(!string.IsNullOrEmpty(LogoCRC))
                    {
                        byte[] imgCrc = File.ReadAllBytes(LogoCRC);
                        MemoryStream imagemLogo = new MemoryStream(imgCrc);
                        LinkedResource crcImage = new LinkedResource(imagemLogo, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                        crcImage.ContentId = "crc";
                        crcImage.ContentType = new ContentType("image/jpg");
                        av.LinkedResources.Add(crcImage);
                    }

                    if(!string.IsNullOrEmpty(LogoMaisSaber))
                    {
                        byte[] imgSaber = File.ReadAllBytes(LogoMaisSaber);
                        MemoryStream imagemSaber = new MemoryStream(imgSaber);
                        LinkedResource maisSaberImage = new LinkedResource(imagemSaber, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                        maisSaberImage.ContentId = "saber";
                        maisSaberImage.ContentType = new ContentType("image/jpg");
                        av.LinkedResources.Add(maisSaberImage);
                    }
                    
                    if(!string.IsNullOrEmpty(logoRodape))
                    {
                        byte[] imgRodape = File.ReadAllBytes(logoRodape);
                        MemoryStream imagemRodape = new MemoryStream(imgRodape);
                        LinkedResource rodapeImage = new LinkedResource(imagemRodape, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                        rodapeImage.ContentId = "rodape";
                        rodapeImage.ContentType = new ContentType("image/jpg");
                        av.LinkedResources.Add(rodapeImage);
                    }

                    
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(Remetente, Destinatario);

                    //if(CopiaCarbono.Count() > 0)
                    //{                        
                    //    foreach(var c in CopiaCarbono)
                    //    {
                    //        message.Bcc.Add(c);
                    //    }                        
                    //}

                    message.Subject = Assunto;
                    message.AlternateViews.Add(av);
                    

                    //ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                    //AlternateView alternate = AlternateView.CreateAlternateViewFromString(Mensagem, mimeType);
                    //message.AlternateViews.Add(alternate);

                    // fim imagem
                    SmtpClient mailer = new SmtpClient(SMTP, Porta);
                    mailer.Credentials = new NetworkCredential(Remetente, Senha);
                    mailer.EnableSsl = Ssl;
                    mailer.Send(message);
                }
                else
                {
                    var mensagem = new MailMessage(Remetente, Destinatario, Assunto, Mensagem);
                    mensagem.IsBodyHtml = true;
                    SmtpClient mailer = new SmtpClient(SMTP, Porta);
                    mailer.Credentials = new NetworkCredential(Remetente, Senha);
                    mailer.EnableSsl = Ssl;
                    mailer.Send(mensagem);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}