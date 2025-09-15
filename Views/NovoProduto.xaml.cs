using MaiuAppMinhasCompras_BrunoS.Models;

namespace MaiuAppMinhasCompras_BrunoS.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    { 
		try
		{
			Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text),
				Categoria = pkr_categoria.SelectedItem.ToString()
			};

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso!", "Registro Inserido", "OK");
			await Navigation.PopAsync();  

		} catch(Exception ex)
		{
			DisplayAlert("Erro", ex.Message, "OK");
		}
    }
}