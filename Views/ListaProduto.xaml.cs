using MaiuAppMinhasCompras_BrunoS.Models;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MaiuAppMinhasCompras_BrunoS.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	private List<Produto> todosProdutos = new List<Produto>();
	public List<ItemRelatorioCategoria> itensRelatorio = new List<ItemRelatorioCategoria>();
	private CancellationTokenSource _tokenPesquisa;
    private string pesquisa = "";

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
	{
		try
        {
            pkr_search_categoria.SelectedIndex = 0;
            todosProdutos = await App.Db.GetAll();

            lista.Clear();
            todosProdutos.ForEach(i => lista.Add(i));
        }
		catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		} catch (Exception ex)
		{
			DisplayAlert("Erro", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		pesquisa = e.NewTextValue?.ToLower() ?? "";
		string q = pesquisa;

		_tokenPesquisa?.Cancel();
		_tokenPesquisa = new CancellationTokenSource();

		try
		{
			await Task.Delay(TimeSpan.FromMilliseconds(300), _tokenPesquisa.Token);

            lst_produtos.IsRefreshing = true;

            var resultados = await App.Db.Search(q);


            MainThread.BeginInvokeOnMainThread(() =>
				{
					lista.Clear();
                    resultados.ForEach(i => lista.Add(i));
                });
		}
		catch (TaskCanceledException)
		{
			//busca cancelada após o usuário digitar novamente
		}
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
			MenuItem selecionado = sender as MenuItem;

			Produto p = selecionado.BindingContext as Produto;

			bool confirm = await DisplayAlert(
				"Tem Certeza?", $"Remover {p.Descricao}", "Sim", "Não");
			if (confirm)
			{
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            todosProdutos = await App.Db.GetAll();

            lista.Clear();
            todosProdutos.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        } finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void pkr_search_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(pkr_search_categoria.SelectedItem.ToString() != "Todos")
        {
            try
            {
                string c = pkr_search_categoria.SelectedItem.ToString();

                var resultados = await App.Db.ProdutosCategoria(c, pesquisa);


                MainThread.BeginInvokeOnMainThread(() =>
                {
                    lista.Clear();
                    resultados.ForEach(i => lista.Add(i));
                });
            }
            catch (TaskCanceledException)
            {
                //busca cancelada após o usuário digitar novamente
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }
        else
        {
            try
            {

                var resultados = await App.Db.Search(pesquisa);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    lista.Clear();
                    resultados.ForEach(i => lista.Add(i));
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        try
        {
            itensRelatorio = await App.Db.ObterRelatorioCategoria();

            var paginaRelatorio = new RelatorioCategoria(itensRelatorio);

            paginaRelatorio.itensRelatorio = itensRelatorio;

            Navigation.PushAsync(paginaRelatorio);
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}