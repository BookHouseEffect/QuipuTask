using Notifications.Domain.Services;

namespace Notifications.AdminUI
{
    public partial class MainUI : Form
    {
        private readonly XmlReaderService _xmlReaderService;

        public MainUI(
            XmlReaderService xmlReaderService
            )
        {
            InitializeComponent();
        }

        private async void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                await _xmlReaderService.ReadXmlFile(openFileDialog1.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }
    }
}
