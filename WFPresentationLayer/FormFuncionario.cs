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
    public partial class FormFuncionario : Form
    {
        public FormFuncionario()
        {
            InitializeComponent();
            dataGridView1.DataSource = bll.GetData().Data;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }
        int idFuncionarioASerAtualizadoExcluido = 0;
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Funcionario result = (Funcionario)dataGridView1.SelectedRows[0].DataBoundItem;
            DataResponse<Funcionario> response = bll.GetByID(result.ID);
            if (response.Sucesso)
            {
                Funcionario funcionario = response.Data[0];
                idFuncionarioASerAtualizadoExcluido = funcionario.ID;
                txtNome.Text = funcionario.Nome;
                txtEmail.Text = funcionario.Email;
                txtCpf.Text = funcionario.CPF;
                txtTelefone.Text = funcionario.Telefone;
                dtpDataNascimento.Value = funcionario.DataNascimento;
            }
        }

        private FuncionarioBLL bll = new FuncionarioBLL();


        private void button1_Click(object sender, EventArgs e)
        {
            if (txtConfirmarSenha.Text != txtSenha.Text)
            {
                MessageBox.Show("Senha diferentes!");
                return;
            }

            Funcionario funcionario = new Funcionario();
            funcionario.CPF = txtCpf.Text;
            funcionario.DataNascimento = dtpDataNascimento.Value;
            funcionario.Email = txtEmail.Text;
            funcionario.Nome = txtNome.Text;
            funcionario.Senha = txtSenha.Text;
            funcionario.Telefone = txtTelefone.Text;

            Response response = bll.Insert(funcionario);
            if (response.Sucesso)
            {
                MessageBox.Show("Cadastrado com sucesso!");
                dataGridView1.DataSource = bll.GetData().Data;
            }
            else
            {
                MessageBox.Show(response.GetErrorMessage());
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !txtSenha.UseSystemPasswordChar;
        }

        private void btnMostrarSenha2_Click(object sender, EventArgs e)
        {
            txtConfirmarSenha.UseSystemPasswordChar = !txtConfirmarSenha.UseSystemPasswordChar;
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            Funcionario funcionario = new Funcionario();
            funcionario.ID = idFuncionarioASerAtualizadoExcluido;
            funcionario.CPF = txtCpf.Text;
            funcionario.DataNascimento = dtpDataNascimento.Value;
            funcionario.Email = txtEmail.Text;
            funcionario.Nome = txtNome.Text;
            funcionario.Senha = txtSenha.Text;
            funcionario.Telefone = txtTelefone.Text;
            funcionario.ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            Response response = bll.Update(funcionario);
            if (response.Sucesso)
            {
                MessageBox.Show("Funionário atualizado com sucesso.");
                dataGridView1.DataSource = bll.GetData().Data;
            }
            else
            {
                MessageBox.Show(response.GetErrorMessage());
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int funcionarioID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            Response response = bll.Delete(funcionarioID);
            if (response.Sucesso)
            {
                MessageBox.Show("Funcionário demitido com sucesso!");
                dataGridView1.DataSource = bll.GetData().Data;
            }
            else
            {
                MessageBox.Show(response.GetErrorMessage());
            }
        }
    }
}
