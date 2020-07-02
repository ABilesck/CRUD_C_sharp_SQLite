using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using prjAlan.model;
using System.Data;

namespace prjAlan.dao
{
    class DaoProduto
    {
        private static SQLiteConnection connection;

        public DaoProduto()
        {

        }

        private static SQLiteConnection DbConnection()
        {
            connection = new SQLiteConnection("" +
                "Data Source=c:\\dados\\Produtos.sqlite; Version=3;");
            connection.Open();
            return connection;
        }

        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(@"c:\dados\Cadastro.sqlite");
            }
            catch
            {
                throw;
            }
        }

        public static void CriarTabelaSQlite()
        {
            try
            {
                var cmd = DbConnection().CreateCommand();
                cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS Produtos (id integer PRIMARY KEY AUTOINCREMENT," +
                " Nome Varchar(50), descricao text)";
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetProdutos()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var cmd = DbConnection().CreateCommand();
                cmd.CommandText = "SELECT id, Nome, descricao FROM Produtos";
                da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetProdutoByNome(string id)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var cmd = DbConnection().CreateCommand();
                cmd.CommandText = "SELECT Id, Nome, Descricao FROM Produtos Where Nome LIKE '" + id + "'";
                da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Inserir(Produto produto)
        {
            try
            {
                var cmd = DbConnection().CreateCommand();
                cmd.CommandText = "INSERT INTO Produtos(Nome, descricao) values (@nome, @descricao)";
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Alterar(Produto produto)
        {
            try
            {
                var cmd = new SQLiteCommand(DbConnection());
                cmd.CommandText = "UPDATE Produtos SET Nome=@Nome, descricao=@descricao WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", produto.Id);
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Deletar(int Id)
        {
            try
            {
                var cmd = new SQLiteCommand(DbConnection());
                cmd.CommandText = "DELETE FROM Produtos Where Id=@Id";
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
