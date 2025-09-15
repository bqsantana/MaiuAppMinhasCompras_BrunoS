using MaiuAppMinhasCompras_BrunoS.Models;
using SQLite;

//teste
namespace MaiuAppMinhasCompras_BrunoS.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }
        public Task<int> Insert(Produto p) {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p) {
            string sql = "Update Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id = ?";
            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id);
        }
        public Task<int> Delete(int id) {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll() {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q) {
            string sql = "SELECT * from Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }
        public Task<List<Produto>> ProdutosCategoria(string c, string q)
        {
            string sql = "SELECT * FROM Produto WHERE categoria LIKE '%" + c + "%' and " +
                            "descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }
        public Task<List<ItemRelatorioCategoria>> ObterRelatorioCategoria()
        {
            string sql = "SELECT categoria, sum(preco * quantidade) as total " +
                            "FROM Produto GROUP BY categoria";

            return _conn.QueryAsync<ItemRelatorioCategoria>(sql);
        }
    }
}
