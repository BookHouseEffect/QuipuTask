using Notifications.Core.Interfaces;
using Notifications.Core.Models;
using System.Xml.Linq;

namespace Notifications.Domain.Services
{
    public class XmlReaderService(
        IQueueSender queueSender
        )
    {
        private readonly IQueueSender _queueSender = queueSender;

        public async Task ReadXmlFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("File does not exists.");
            }

            XDocument manifest = XDocument.Load(filename);

            foreach (XElement client in manifest.Descendants("client"))
            {
                foreach (XElement template in client.Descendants("Template")) {

                    try
                    {
                        var model = new NotificationModel()
                        {
                            ClientId = Guid.Parse(client.Attribute("Id").Value),
                            TemplateId = Guid.Parse(template.Attribute("Id").Value),
                            Recipients = [],
                            TemplateData = [],
                        };

                        foreach (XElement recipient in template.Descendants("recipient"))
                        {
                            model.Recipients.Add(recipient.Value);
                        }

                        foreach (XElement data in template.Descendants("data"))
                        {
                            model.TemplateData.Add(
                                data.Attribute("key").Value,
                                data.Value
                            );
                        }

                        await _queueSender.SendNotificationData(model);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
