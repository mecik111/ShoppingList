using ShoppingList.Models;
using System.Xml;

namespace ShoppingList.Views
{
    public partial class ShoppingItemView : ContentView
    {
        public event EventHandler? ItemDeleted;
        public event EventHandler? ItemStatusChanged;
        public event EventHandler? ItemQuantityChanged;

        public ShoppingItemView()
        {
            InitializeComponent();
        }

        private void OnIncreaseClicked(object sender, EventArgs e)
        {
            if (BindingContext is ShoppingItem item)
            {
                item.Quantity++;
                ItemQuantityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDecreaseClicked(object sender, EventArgs e)
        {
            if (BindingContext is ShoppingItem item && item.Quantity > 1)
            {
                item.Quantity--;
                ItemQuantityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            ItemDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnIsBoughtChanged(object sender, CheckedChangedEventArgs e)
        {
            UpdateVisualState();
            ItemStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateVisualState()
        {
            if (BindingContext is ShoppingItem item)
            {
                if (item.IsBought)
                {
                    NameLabel.TextDecorations = TextDecorations.Strikethrough;
                    this.Opacity = 0.5;
                }
                else
                {
                    NameLabel.TextDecorations = TextDecorations.None;
                    this.Opacity = 1.0;
                }
            }
        }
    }

}
