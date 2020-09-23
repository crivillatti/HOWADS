using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CadastroWifi
{
    public partial class frmCadastro : Form
    {
        public frmCadastro()
        {
            InitializeComponent();
        }
        //Núcleo da Conexão com o Banco de Dados MySQL
        private MySqlConnectionStringBuilder connCoreDB() 
        {
            MySqlConnectionStringBuilder enderecoDB = new MySqlConnectionStringBuilder();
            enderecoDB.Server = "localhost";
            enderecoDB.Database = "Base1";
            enderecoDB.UserID = "root";
            enderecoDB.Password = "";
            return enderecoDB;
        }
        //Carregamento da aplicação no formulário inicial
        private void frmCadastro_Load(object sender, EventArgs e)
        {
            atualizarGrid();
        }
        //início da estrutura base para atualizar o Grid de Usuarios
        private void atualizarGrid()
        {
            //Conexáo com MySQL
            MySqlConnectionStringBuilder enderecoBD = connCoreDB();
            MySqlConnection connectionDB = new MySqlConnection(enderecoBD.ToString());

            try
            {
                connectionDB.Open();

                MySqlCommand comandoMySql = connectionDB.CreateCommand();
                comandoMySql.CommandText = "SELECT * FROM usuario WHERE idStatus = 0"; // Pesquisa todos da tabela nomeUsuario
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                DataGridUsuarios.Rows.Clear(); //Limpa o Grid

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)DataGridUsuarios.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetInt32(0);//ID
                    row.Cells[1].Value = reader.GetString(1);//NOME
                    row.Cells[2].Value = reader.GetString(3);//CPF
                    row.Cells[3].Value = reader.GetString(2);//DATA EXPIRAÇÃO
                    row.Cells[4].Value = reader.GetInt32(4);//STATUS ATIVO
                    DataGridUsuarios.Rows.Add(row);//ADICIONA A LINHA NA TABELA
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Não foi possivel conectar-se com a Base de Dados!");
            }
        }
        //Fim da estrutura de atualização do Grid
        //Fim do carregamento da aplicação


        //Início das funções dos botões do programa

        //Botão do botão Salvar
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            //Conexão com MySQL
            MySqlConnectionStringBuilder enderecoBD = connCoreDB();
            MySqlConnection connectionDB = new MySqlConnection(enderecoBD.ToString());
            try
            {
                //Abre conexão com a base
                connectionDB.Open();
                Console.WriteLine("Conexão aberta com a base de dados!");

                MySqlCommand comandoMySql = connectionDB.CreateCommand();
                //Insere dados na tabela usuario
                comandoMySql.CommandText = "INSERT INTO usuario (idNome, idCPF, idDtExpira)" + 
                    "VALUES('" + txtNome.Text + "', '" + txmCpf.Text + "', '" + txmDtExpira.Text + "')";
                comandoMySql.ExecuteNonQuery();

                //Fecha Conexão com a base
                connectionDB.Close();
                //Exibe na tela a mensagem de retorno
                MessageBox.Show("Cadastro Realizado!");
                Console.WriteLine("Conexão Com a Base de dados Encerrada!");
                atualizarGrid();

            }
            catch (Exception ex)
            {
                //Exibe na tela a mensagem de retorno
                Console.WriteLine(ex.Message);
                MessageBox.Show("Erro!");
            }
            txtNome.Clear();
            txmCpf.Clear();
            txmDtExpira.Clear();
        }

        //Função do botão Cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNome.Clear();
            txmCpf.Clear();
            txmDtExpira.Clear();
            atualizarGrid();
        }

        //Função do botão Excluir
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            //Conexão com MySQL
            MySqlConnectionStringBuilder enderecoBD = connCoreDB();
            MySqlConnection connectionDB = new MySqlConnection(enderecoBD.ToString());
            try
            {
                //Abre conexão com a base
                connectionDB.Open();
                Console.WriteLine("Conexão aberta com a base de dados!");

                MySqlCommand comandoMySql = connectionDB.CreateCommand();
                //"Deleta" dados da tabela usuario
                comandoMySql.CommandText = "UPDATE usuario SET idStatus = 1 WHERE idNome = '" + txtNome.Text + "'";
                comandoMySql.ExecuteNonQuery();

                //Fecha Conexão com a base
                connectionDB.Close();
                //Exibe na tela a mensagem de retorno
                MessageBox.Show("Usuário Excluído com Sucesso!");
                Console.WriteLine("Conexão Com a Base de dados Encerrada!");
                atualizarGrid();

            }
            catch (Exception ex)
            {
                //Exibe na tela a mensagem de retorno
                Console.WriteLine(ex.Message);
                MessageBox.Show("Erro!");
            }
            txtNome.Clear();
            txmCpf.Clear();
            txmDtExpira.Clear();
        }



        //Função clique no Grid
        private void DataGridUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                DataGridUsuarios.CurrentRow.Selected = true;
                txtId.Text = DataGridUsuarios.Rows[e.RowIndex].Cells["ColumnID"].FormattedValue.ToString();
                txtNome.Text = DataGridUsuarios.Rows[e.RowIndex].Cells["ColumnNome"].FormattedValue.ToString();
                txmCpf.Text = DataGridUsuarios.Rows[e.RowIndex].Cells["ColumnCPF"].FormattedValue.ToString();
                txmDtExpira.Text = DataGridUsuarios.Rows[e.RowIndex].Cells["ColumnDtExpira"].FormattedValue.ToString();
            }
            else
            {
                Console.WriteLine();
                txtNome.Clear();
                txmCpf.Clear();
                txmDtExpira.Clear();
                MessageBox.Show("Falha!");

            }
        }

        //Função botão Atualizar
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            //Conexão com MySQL
            MySqlConnectionStringBuilder enderecoBD = connCoreDB();
            MySqlConnection connectionDB = new MySqlConnection(enderecoBD.ToString());
            try
            {
                //Abre conexão com a base
                connectionDB.Open();
                Console.WriteLine("Conexão aberta com a base de dados!");

                MySqlCommand comandoMySql = connectionDB.CreateCommand();
                //Atualiza dados da tabela usuario
                comandoMySql.CommandText = "UPDATE usuario SET idNome = '" + txtNome.Text + "', " +
                    "idCpf = '" + txmCpf.Text + "', " + "idDtExpira = '" + txmDtExpira.Text + 
                    "' WHERE idUsuario = " + txtId.Text + "";
                comandoMySql.ExecuteNonQuery();

                //Fecha Conexão com a base
                connectionDB.Close();
                //Exibe na tela a mensagem de retorno
                MessageBox.Show("Cadastro Atualizado!");
                Console.WriteLine("Conexão Com a Base de dados Encerrada!");
                atualizarGrid();

            }
            catch (Exception ex)
            {
                //Exibe na tela a mensagem de retorno
                Console.WriteLine(ex.Message);
                MessageBox.Show("Erro!");
            }
            txtNome.Clear();
            txmCpf.Clear();
            txmDtExpira.Clear();
        }
    }

}
