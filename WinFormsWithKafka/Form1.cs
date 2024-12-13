using KafkaNet;
using KafkaNet.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsWithKafka
{
    public partial class Form1 : Form
    {
        string txtMessage = string.Empty;
        Uri uri = new Uri("http://localhost:9092");
        string topic = "chat-message";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            txtMessage = textBox1.Text;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            txtMessage = textBox1.Text;
            if (txtMessage == string.Empty)
            {
                MessageBox.Show("Please Enter Message", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string payload = txtMessage.Trim();
            var sendMessage = new Thread(() => {
                
                KafkaNet.Protocol.Message msg = new KafkaNet.Protocol.Message(payload);
                var options = new KafkaOptions(uri);
                var router = new BrokerRouter(options);
                var client = new Producer(router);
                client.SendMessageAsync(topic, new List<KafkaNet.Protocol.Message> { msg }).Wait();
            });
            sendMessage.Start();
        }
    }
}
