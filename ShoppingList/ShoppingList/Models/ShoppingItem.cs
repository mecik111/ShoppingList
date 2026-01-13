using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingList.Models
{
    public class ShoppingItem : INotifyPropertyChanged
    {
        private string name = "";
        private string unit = "szt.";
        private int quantity = 1;
        private bool isBought;

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name
        {
            get => name;
            set { name = value; }
        }

        public string Unit
        {
            get => unit;
            set { unit = value; }
        }

        public int Quantity
        {
            get => quantity;
            set { quantity = value; OnPropertyChanged(); }
        }

        public bool IsBought
        {
            get => isBought;
            set { isBought = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}