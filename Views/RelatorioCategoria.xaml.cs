using MaiuAppMinhasCompras_BrunoS.Models;
using System.Collections.ObjectModel;
using MaiuAppMinhasCompras_BrunoS.Views;

namespace MaiuAppMinhasCompras_BrunoS.Views;

public partial class RelatorioCategoria : ContentPage

{
    ObservableCollection<ItemRelatorioCategoria> lista = new ObservableCollection<ItemRelatorioCategoria>();
    public List<ItemRelatorioCategoria> itensRelatorio { get; set; }
    public RelatorioCategoria(List<ItemRelatorioCategoria> itensRelatorio2)
	{
		InitializeComponent();
        itensRelatorio = itensRelatorio2;

        itensRelatorio.ForEach(i => lista.Add(i));
        BindingContext = this;
        lst_produtos.ItemsSource = lista;
    }
}