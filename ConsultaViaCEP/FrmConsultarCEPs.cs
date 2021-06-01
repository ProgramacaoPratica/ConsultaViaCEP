using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultaViaCEP
{
    public partial class FrmConsultarCEPs : Form
    {
        public FrmConsultarCEPs()
        {
            InitializeComponent();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCEP.Text))
            {
                MessageBox.Show("Informe um CEP válido...", this.Text, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                string strURL = string.Format("https://viacep.com.br/ws/{0}/json/", txtCEP.Text.Trim());

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = client.GetAsync(strURL).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            Resultado res = JsonConvert.DeserializeObject<Resultado>(result);

                            txtEstado.Text = res.UF;
                            txtCidade.Text = res.Localidade;
                            txtBairro.Text = res.Bairro;
                            txtRua.Text = res.Logradouro;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.Text, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtCEP.Text = string.Empty;
            txtEstado.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtRua.Text = string.Empty;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
