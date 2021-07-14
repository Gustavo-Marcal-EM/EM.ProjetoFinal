using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EM.Repository;
using EM.Domain;

namespace EM.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            preencheGrid();
            sexoComboBox.SelectedIndex = 0;
            matriculaTextBox.Focus();
        }
        private void preencheGrid()
        {
            try
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = new RepositorioAluno().GetAll();
                dataGridView1.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErroPreencheGrid", MessageBoxButtons.OK);
            }
        }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
        private void matriculaTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 08)
            {
                e.Handled = true;
            }
        }
        private void nomeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != 08 && e.KeyChar != 32)
            {
                e.Handled = true;
            }
        }
        private void cpfTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 08)
            {
                e.Handled = true;
            }
        }
        private void pesquisaTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != 08 && e.KeyChar != 32)
            {
                e.Handled = true;
            }
        }
        private bool _IsEditando;
        public bool IsEditando
        {
            get
            {
                return _IsEditando;
            }
            set
            {
                _IsEditando = value;
                buttonAdicionar.Text = _IsEditando ? "Modificar" : "Adicionar";
                buttonLimpar.Text = _IsEditando ? "Cancelar" : "Limpar";
                matriculaTextBox.Enabled = !_IsEditando;
            }
        }
        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            Aluno aluno = new Aluno();
            RepositorioAluno repositorioAluno = new RepositorioAluno();

            if (buttonAdicionar.Text.Equals("Modificar"))
            {
                if (nomeTextBox.Text == string.Empty)
                {
                    MessageBox.Show("Selecione o aluno na tabela para editar!");
                }
                else
                {
                    if (!ValidaCPF.IsCpf(cpfTextBox.Text.ToString()))
                    {
                        MessageBox.Show("CPF informado é inválido");
                    }
                    else
                    {
                        try
                        {
                            aluno.Matricula = Convert.ToInt32(matriculaTextBox.Text);
                            aluno.Nome = nomeTextBox.Text.ToString();
                            aluno.CPF = cpfTextBox.Text.ToString();
                            aluno.Nascimento = Convert.ToDateTime(nascimentoMaskedTextBox.Text).Date;
                            aluno.Sexo = (EnumeradorSexo)sexoComboBox.SelectedIndex;
                            
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erro na conversão modificar");
                        }

                        DialogResult dr = MessageBox.Show("Confirme para editar o aluno", "Confirmação de edição", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dr == DialogResult.Yes)
                        {
                            repositorioAluno.Update(aluno);
                            preencheGrid();
                            matriculaTextBox.Text = "";
                            nomeTextBox.Text = "";
                            cpfTextBox.Text = "";
                            sexoComboBox.SelectedIndex = 0;
                            nascimentoMaskedTextBox.Text = "";
                            IsEditando = !IsEditando;
                        }
                    }
                }
            }
            else
            {
                if (matriculaTextBox.Text == string.Empty || nomeTextBox.Text == string.Empty ||
                    nascimentoMaskedTextBox.Text.Trim(' ', '/', '_') == "" && !nascimentoMaskedTextBox.MaskCompleted)
                {
                    MessageBox.Show("Preencha os campos obrigatórios(*)", "Erro ao adicionar", MessageBoxButtons.OK);
                }
                else
                {
                    if (!ValidaCPF.IsCpf(cpfTextBox.Text.ToString()))
                    {
                        MessageBox.Show("CPF informado é inválido");
                    }
                    else
                    {
                        aluno.CPF = cpfTextBox.Text;
                        aluno.Nome = nomeTextBox.Text;
                        try
                        {
                            aluno.Matricula = Convert.ToInt32(matriculaTextBox.Text);
                            aluno.Nascimento = Convert.ToDateTime(nascimentoMaskedTextBox.Text);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erro1", MessageBoxButtons.OK);
                        }

                        aluno.Sexo = (EnumeradorSexo)sexoComboBox.SelectedIndex;
                        if (ValidaMatricula() && ValidaNome() && ValidaUnicidadeCPF())
                        {
                            try
                            {
                                repositorioAluno.Add(aluno);
                                preencheGrid();
                                MessageBox.Show("Aluno inserido com sucesso!", "Inserir", MessageBoxButtons.OK);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erro3", MessageBoxButtons.OK);
                            }
                        }
                    }
                }
            }
        }
        private void buttonLimpar_Click(object sender, EventArgs e)
        {
            if (buttonLimpar.Text == "Cancelar")
            {
                matriculaTextBox.Text = "";
                nomeTextBox.Text = "";
                cpfTextBox.Text = "";
                sexoComboBox.SelectedIndex = 0;
                nascimentoMaskedTextBox.Text = "";
                pesquisaTextBox.Text = "";
                matriculaTextBox.Focus();
                IsEditando = !IsEditando;
            }
            else
            {
                foreach (Control myControl in groupBox1.Controls)
                {
                    if (myControl as TextBox == null)
                    {
                    }
                    else
                    {
                        ((TextBox)myControl).Text = "";
                        sexoComboBox.SelectedIndex = 0;
                        nascimentoMaskedTextBox.Text = "";
                        pesquisaTextBox.Text = "";
                    }
                }
                matriculaTextBox.Focus();
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsEditando)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    matriculaTextBox.Text = row.Cells[0].Value.ToString();
                    nomeTextBox.Text = row.Cells[1].Value.ToString();
                    cpfTextBox.Text = row.Cells[2].Value.ToString();
                    nascimentoMaskedTextBox.Text = row.Cells[3].Value.ToString();
                    if (row.Cells[4].Value.ToString() == "Masculino")
                        sexoComboBox.SelectedIndex = 0;
                    else
                        sexoComboBox.SelectedIndex = 1;
                }
            }
        }
        private void buttonEditar_Click(object sender, EventArgs e)
        {
            IsEditando = !IsEditando;
            matriculaTextBox.Text = "";
            nomeTextBox.Text = "";
            cpfTextBox.Text = "";
            sexoComboBox.SelectedIndex = 0;
            nascimentoMaskedTextBox.Text = "";
            matriculaTextBox.Focus();

        }
        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Confirme para excluir o aluno", "Confirmação de exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                Aluno aluno = new Aluno();
                try
                {
                    int matricula = (int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value;
                    aluno = new RepositorioAluno().GetByMatricula(matricula);
                    new RepositorioAluno().Remove(aluno);
                    preencheGrid();
                    MessageBox.Show("Aluno excluído com sucesso!", "Excluir", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao excluir", MessageBoxButtons.OK);
                }
            }
        }
        private void buttonPesquisar_Click(object sender, EventArgs e)
        {
            string termoPesquisado = pesquisaTextBox.Text;
            if (Int32.TryParse(termoPesquisado, out int stringMatricula))
            {
                var matchingMatricula = new RepositorioAluno().GetByMatricula(stringMatricula);
                try
                {
                    dataGridView1.DataSource = new BindingSource();
                    dataGridView1.DataSource = matchingMatricula.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK); 
                }
            }
            else
            {
                var matchingNames = new RepositorioAluno().GetByContendoNoNome(termoPesquisado);
                try
                {
                    dataGridView1.DataSource = new BindingSource();
                    dataGridView1.DataSource = matchingNames.ToList();
                    new RepositorioAluno().GetByContendoNoNome(termoPesquisado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao Pesquisar"); 
                }
            }
            
            
            
            

            

        }
        private void pesquisaTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        public static class ValidaCPF
        {
            public static bool IsCpf(string cpf)
            {
                if (string.IsNullOrEmpty(cpf))
                {
                    return true;
                }
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;
                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");
                if (cpf.Length != 11)
                    return false;
                tempCpf = cpf.Substring(0, 9);
                soma = 0;

                try
                {
                    for (int i = 0; i < 9; i++)
                        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                }
                catch(FormatException fex)
                {
                    MessageBox.Show(fex.Message);
                    return false;
                }
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCpf += digito;
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito += resto.ToString();
                return cpf.EndsWith(digito);
            }
        }
        public bool ValidaMatricula()
        {
            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                int col = 0;
                int matricula = Convert.ToInt32(dataGridView1.Rows[rows].Cells[col].Value);
                try
                {
                    if (Convert.ToInt32(matriculaTextBox.Text) == matricula)
                    {
                        MessageBox.Show("Matricula já existente no Banco", "Erro ao validar matricula", MessageBoxButtons.OK);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return true;
        }
        public bool ValidaNome()
        {
            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                int col = 1;
                string nome = (string)dataGridView1.Rows[rows].Cells[col].Value;
                try
                {
                    if (nomeTextBox.Text == nome)
                    {
                        MessageBox.Show("Nome já existente no Banco", "Erro ao validar nome", MessageBoxButtons.OK);
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return true;
        }
        public bool ValidaUnicidadeCPF()
        {
            if (string.IsNullOrEmpty(cpfTextBox.Text))
            {
                return true;
            }
            else
            {
                for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                {
                    int col = 2;
                    string cpf = dataGridView1.Rows[rows].Cells[col].Value.ToString();
                    try
                    {
                        if (cpfTextBox.Text == cpf)
                        {
                            MessageBox.Show("CPF já existente no Banco", "Erro", MessageBoxButtons.OK);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            return true;
        }
    }
}
