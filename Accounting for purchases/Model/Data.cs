using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting_for_purchases.Model
{

    public class Sprav : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _product;
        public string Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }
        private int _wholesale;
        public int Wholesale
        {
            get => _wholesale;
            set
            {
                _wholesale = value;
                OnPropertyChanged();
            }
        }
        private int _retail;
        public int Retail
        {
            get => _retail;
            set
            {
                _retail = value;

                OnPropertyChanged();
            }
        }
        public Sprav() { }
        public Sprav(string name)
        {
            Product= name;
            Wholesale = 0;
            Retail = 0;

        }
    }
    public class Product : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private int _sprav_id;
        public int SpravId
        {
            get => _sprav_id;
            set
            {
                _sprav_id = value;
                OnPropertyChanged();
            }
        }
        private Sprav _sprav;
        public Sprav Sprav
        {
            get => _sprav;
            set
            {
                _sprav = value;
                OnPropertyChanged();
            }
        }
        private int _Order_id;
        public int OrderId
        {
            get => _Order_id;
            set
            {
                _Order_id = value;
                OnPropertyChanged();
            }
        }
        private Order _Order;
        public Order Order
        {
            get => _Order;
            set
            {
                _Order = value;
                OnPropertyChanged();
            }
        }
        private int _discount;
        public int Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        private int _cost_for_client;
        public int CostForClient
        {
            get
            {

                return _cost_for_client;

            }
            set
            {
                _cost_for_client = value;
                OnPropertyChanged();
            }

        }
        private int _profit;
        public int Profit
        {
            get
            {

                return _profit;
            }
            set
            {
                _profit = value;
                OnPropertyChanged();
            }
        }
        private void UpdateCost()
        {
            CostForClient = Count * (Sprav.Retail - Discount);
            Profit = CostForClient - Count * Sprav.Wholesale;
        }
        public void Update(Product product)
        {
            this.Count = product.Count;
            this.Discount = product.Discount;
            UpdateCost();
        }
        public Product() { }
        public Product(Sprav sprav)
        {
            this.Sprav = sprav;
            this.Count = 1;
            this.Discount = 0;
            this.SpravId = sprav.Id;
            UpdateCost();
        }
        public Product prod(Product prod)
        {
            var prods = prod;
            prod.Sprav = null;
            return prods;
        }
        public Product(int sprav, int disc, int cost, int prof,int count)
        {
            this.SpravId = sprav;
            var db = new ConnectDB();
            this.Sprav = db.Sprav.Where(p => p.Id == sprav).FirstOrDefault();
            this.Discount = disc;
            this.CostForClient = cost;
            this.Profit = prof;
            this.Count = count;

        }
        public Product( int disc, int cost, int prof, int count)
        {
            
            var db = new ConnectDB();
            this.Sprav = new Sprav("Итого");
            this.Discount = disc;
            this.CostForClient = cost;
            this.Profit = prof;
            this.Count = count;

        }


    }
    public class Order:BaseViewModel
    {
        private int _id;
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Product> _product;
        public ObservableCollection<Product> Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }
        public Order() { }
       
        public Order(ObservableCollection<Product> Product)
        {
            this.Date = DateTime.Now;
            this.Product = Product;            
        }
        public void UpdateTime()
        {
            this.Date = DateTime.Now;
        }
    }

}
