using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SimplePosDesktop.Data;

namespace SimplePosDesktop;

public partial class MainWindow : Window
{
    // Products loaded from DB
    private List<Product> _products = new();
    // Simple cart: list of selected products (we'll add quantity later)
    private List<Product> _cart = new();

    // Controls from XAML
    private ListBox? _productsListBox;
    private ListBox? _cartListBox;
    private TextBlock? _totalTextBlock;

    public MainWindow()
    {
        InitializeComponent();

        // 1) Ensure database & seed data exist
        using (var db = new PosDbContext())
        {
            db.Database.EnsureCreated();
            _products = db.Products
                          .OrderBy(p => p.Name)
                          .ToList();
        }

        // 2) Find controls by x:Name from MainWindow.axaml
        _productsListBox = this.FindControl<ListBox>("ProductsListBox");
        _cartListBox = this.FindControl<ListBox>("CartListBox");
        _totalTextBlock = this.FindControl<TextBlock>("TotalTextBlock");

        // 3) Fill UI
        RefreshProductList();
        RefreshCart();
    }

    // -----------------------------
    //  PRODUCT LIST
    // -----------------------------
    private void RefreshProductList()
    {
        if (_productsListBox == null)
            return;

        _productsListBox.ItemsSource = null;
        _productsListBox.ItemsSource = _products;
    }

    // -----------------------------
    //  CART + TOTAL
    // -----------------------------
    private void RefreshCart()
    {
        if (_cartListBox != null)
        {
            _cartListBox.ItemsSource = null;
            _cartListBox.ItemsSource = _cart;
        }

        double total = 0;
        foreach (var p in _cart)
            total += p.Price;

        if (_totalTextBlock != null)
        {
            _totalTextBlock.Text = $"${total:F2}";
        }
    }

    // -----------------------------
    //  BUTTON: ADD TO CART
    // -----------------------------
    private void OnAddToCartClick(object? sender, RoutedEventArgs e)
    {
        if (_productsListBox?.SelectedItem is Product selected)
        {
            _cart.Add(selected);
            RefreshCart();
        }
        else
        {
            ShowMessage("Please select a product to add.");
        }
    }

    // -----------------------------
    //  BUTTON: CHECKOUT
    // -----------------------------
    private void OnCheckoutClick(object? sender, RoutedEventArgs e)
    {
        if (_cart.Count == 0)
        {
            ShowMessage("Cart is empty.");
            return;
        }

        double total = 0;
        foreach (var p in _cart)
            total += p.Price;

        // Later we will save Sale + SaleItems into DB here
        ShowMessage($"Payment complete.\nTotal: ${total:F2}\nThank you!");

        _cart.Clear();
        RefreshCart();
    }

    // -----------------------------
    //  SIMPLE MESSAGE WINDOW
    // -----------------------------
    private async void ShowMessage(string message)
    {
        var dialog = new Window
        {
            Width = 320,
            Height = 180,
            Title = "Message",
            Content = new TextBlock
            {
                Text = message,
                Margin = new Avalonia.Thickness(20),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            },
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        await dialog.ShowDialog(this);
    }
}
