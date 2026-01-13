using System.Xml.Linq;
using ShoppingList.Models;

namespace ShoppingList.Views
{
    public partial class MainPage : ContentPage
    {
        private List<ShoppingItem> items = new();
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "shopping_data.xml");

        public MainPage()
        {
            InitializeComponent();
            LoadData();
            RenderList();
        }

        private void OnAddItemClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewItemNameEntry.Text))
            {
                DisplayAlert("B³¹d", "Podaj nazwê produktu.", "OK");
                return;
            }

            if (!int.TryParse(NewItemQuantityEntry.Text, out int quantity) || quantity < 1)
            {
                DisplayAlert("B³¹d", "Podaj prawid³ow¹ iloœæ (minimum 1).", "OK");
                return;
            }

            var newItem = new ShoppingItem
            {
                Name = NewItemNameEntry.Text,
                Unit = string.IsNullOrWhiteSpace(NewItemUnitEntry.Text) ? "szt." : NewItemUnitEntry.Text,
                Quantity = quantity,
                IsBought = false
            };

            items.Add(newItem);

            NewItemNameEntry.Text = string.Empty;
            NewItemUnitEntry.Text = "szt.";
            NewItemQuantityEntry.Text = "1";

            SaveData();
            RenderList();
        }

        private void RenderList()
        {
            ItemsStackLayout.Children.Clear();

            var sortedItems = items.OrderBy(i => i.IsBought).ToList();

            foreach (var item in sortedItems)
            {
                var view = new ShoppingItemView
                {
                    BindingContext = item
                };

                view.ItemDeleted += (s, e) =>
                {
                    items.Remove(item);
                    SaveData();
                    RenderList();
                };

                view.ItemStatusChanged += (s, e) =>
                {
                    SaveData();
                    RenderList();
                };

                view.ItemQuantityChanged += (s, e) =>
                {
                    SaveData();
                };

                ItemsStackLayout.Add(view);
            }
        }

        private void SaveData()
        {
            try
            {
                var doc = new XDocument(
                    new XElement("ShoppingList",
                        items.Select(i => new XElement("Item",
                            new XAttribute("Id", i.Id),
                            new XElement("Name", i.Name),
                            new XElement("Unit", i.Unit),
                            new XElement("Quantity", i.Quantity),
                            new XElement("IsBought", i.IsBought)
                        ))
                    )
                );

                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving XML: {ex.Message}");
            }
        }

        private void LoadData()
        {
            if (!File.Exists(filePath))
                return;

            try
            {
                var doc = XDocument.Load(filePath);
                items = doc.Descendants("Item").Select(x => new ShoppingItem
                {
                    Id = x.Attribute("Id")?.Value ?? Guid.NewGuid().ToString(),
                    Name = x.Element("Name")?.Value ?? string.Empty,
                    Unit = x.Element("Unit")?.Value ?? "szt.",
                    Quantity = int.Parse(x.Element("Quantity")?.Value ?? "1"),
                    IsBought = bool.Parse(x.Element("IsBought")?.Value ?? "false")
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML: {ex.Message}");
            }
        }
    }
}