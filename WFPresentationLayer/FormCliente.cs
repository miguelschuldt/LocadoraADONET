using BusinessLogicalLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFPresentationLayer
{
    public partial class FormCliente : Form
    {
        ClienteBLL bll = new ClienteBLL();
        public FormCliente()
        {
            InitializeComponent();
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.DataSource = bll.GetData().Data;
        }
        int idClienteASerAtualizadoExcluido = 0;

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Cliente result = (Cliente)dataGridView1.SelectedRows[0].DataBoundItem;
            DataResponse<Cliente> response = bll.GetByID(result.ID);
            if (response.Sucesso)
            {
                Cliente cliente = response.Data[0];
                idClienteASerAtualizadoExcluido = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                txtNome.Text = cliente.Nome;
                txtCPF.Text = cliente.CPF;
                txtEmail.Text = cliente.Email;
                dtpDataNascimento.Value = cliente.DataNascimento;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente()
            {
                Nome = txtNome.Text,
                CPF = txtCPF.Text,
                Email = txtEmail.Text,
                DataNascimento = dtpDataNascimento.Value
            };
            bll = new ClienteBLL();
            Response response = bll.Insert(cliente);
            if (response.Sucesso)
            {
                MessageBox.Show("Cliente cadastrado com sucesso!");
                dataGridView1.DataSource = bll.GetData().Data;
            }
            else
            {
                MessageBox.Show(response.GetErrorMessage());
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente();
            cliente.ID = idClienteASerAtualizadoExcluido;
            cliente.Nome = txtNome.Text;
            cliente.Email = txtEmail.Text;
            cliente.CPF = txtCPF.Text;
            cliente.DataNascimento = dtpDataNascimento.Value;


            Response response = bll.Update(cliente);
            if (response.Sucesso)
            {
                MessageBox.Show("Cliente atualizado com sucesso!");
                dataGridView1.DataSource = bll.GetData().Data;
            }
            else
            {
                MessageBox.Show(response.GetErrorMessage());
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            Response response = bll.Delete(idClienteASerAtualizadoExcluido);
            if (response.Sucesso)
            {
                MessageBox.Show("Cliente excluído com sucesso!");
                dataGridView1.DataSource = bll.GetData().Data;
            }
            else
            {
                MessageBox.Show(response.GetErrorMessage());
            }
        }
    }
}
