using Notifications.Core.Interfaces;
using Notifications.Core.Models;
using System.Net.Mail;
using System.Text;

namespace Notifications.Domain.Services
{
    public class MailRenderer : IMailRenderer
    {
        public MailMessage RenderMail(AdvancedNotificationModel model)
        {
            string content = model.TemplateData != null && model.TemplateData.Count > 0 ?
                ReplaceMacro(model.Content, model.TemplateData) : model.Content;

            var mailMessage = new MailMessage
            {
                Sender = new MailAddress(model.Sender),
                Subject = model.Subject,
                Body = content,
                IsBodyHtml = model.IsContentHtml,
            };

            foreach (var recipient in model.Recipients)
            {
                mailMessage.To.Add(new MailAddress(recipient));
            }

            return mailMessage;
        }

        private string ReplaceMacro(string value, Dictionary<string, string> data)
        {
            StringBuilder sb = new(value);
            foreach (KeyValuePair<string, string> entry in data)
            {
                sb.Replace($"{{{{{entry.Key}}}}}", entry.Value);
            }
            return sb.ToString();
        }
    }
}
