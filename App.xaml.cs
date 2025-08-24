using MaiuAppMinhasCompras_BrunoS.Helpers;

namespace MaiuAppMinhasCompras_BrunoS
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if(_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite.db3"
                        );
                    _db = new SQLiteDatabaseHelper(path);
                }
                return _db;
            }
        }
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();

            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
