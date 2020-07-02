using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using prjAlan.model;
using prjAlan.dao;

namespace prjAlan.view
{
    public partial class FrmProdutos : Form
    {
        operacao operacao;
        public FrmProdutos()
        {
            InitializeComponent();
        }

        private void ExibirDados()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DaoProduto.GetProdutos();
                dgvDados.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro:" + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmProdutos_Load(object sender, EventArgs e)
        {
            try
            {
                DaoProduto.CriarBancoSQLite();
                DaoProduto.CriarTabelaSQlite();
                ExibirDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro:" + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            operacao = operacao.incluir;
            HabilitarCampos();

        }

        private void HabilitarCampos()
        {
            btnIncluir.Enabled = false;
            btnAlterar.Enabled = false;
            btnPesquisar.Enabled = false;
            btnExcluir.Enabled = false;
            btnCancelar.Enabled = true;
            btnGravar.Enabled = true;
            txtCod.Enabled = false;
            txtNome.Enabled = true;
            txtDescricao.Enabled = true;
            txtNome.Focus();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            operacao = operacao.alterar;
            HabilitarCampos();
            txtNome.Text = dgvDados.Rows[dgvDados.SelectedRows[0].Index].Cells[1].Value.ToString();
            txtDescricao.Text = dgvDados.Rows[dgvDados.SelectedRows[0].Index].Cells[2].Value.ToString();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            operacao = operacao.pesquisar;
            HabilitarCampos();
            txtDescricao.Enabled = false;
            //txtNome.Enabled = false;
            //txtCod.Enabled = true;
            //txtCod.Focus();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {            
            if(MessageBox.Show("Deseja excluir o produto selecionado?", "Atenção",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    int codigo = Convert.ToInt32(dgvDados.Rows[dgvDados.SelectedRows[0].Index].Cells[0].Value.ToString());
                    DaoProduto.Deletar(codigo);
                    ExibirDados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro:" + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DesabilitarCampos()
        {
            btnIncluir.Enabled = true;
            btnAlterar.Enabled = true;
            btnPesquisar.Enabled = true;
            btnExcluir.Enabled = true;
            btnCancelar.Enabled = false;
            btnGravar.Enabled = false;
            txtCod.Enabled = false;
            txtNome.Enabled = false;
            txtDescricao.Enabled = false;

            txtNome.Text = "";
            txtDescricao.Text = "";
            txtCod.Text = "";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DesabilitarCampos();
            FrmProdutos_Load(null, null);
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            if(operacao == operacao.incluir)
            {
                Produto pro = new Produto();
                pro.Nome = txtNome.Text;
                pro.Descricao = txtDescricao.Text;

                DaoProduto.Inserir(pro);
                ExibirDados();
                DesabilitarCampos();
                //MessageBox.Show("Parabens, nao fez mais que sua orbrigacao", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(operacao == operacao.alterar)
            {
                Produto pro = new Produto();
                pro.Id = Convert.ToInt32(dgvDados.Rows[dgvDados.SelectedRows[0].Index].Cells[0].Value.ToString());
                pro.Nome = txtNome.Text;
                pro.Descricao = txtDescricao.Text;

                DaoProduto.Alterar(pro);
                ExibirDados();
                DesabilitarCampos();

            }
            else if(operacao == operacao.pesquisar)
            {
                try
                {
                    //int codigo = Convert.ToInt32(txtCod.Text);
                    DataTable dt = new DataTable();
                    dt = DaoProduto.GetProdutoByNome("%" + txtNome.Text + "%");
                    dgvDados.DataSource = dt;
                    DesabilitarCampos();
                    btnCancelar.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro:" + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmProdutos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja fechar o programa?",
                "Atenção", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }

    public enum operacao
    {
        incluir, alterar, pesquisar
    }
}
