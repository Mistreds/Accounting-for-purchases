using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Accounting_for_purchases
{
   public class MainViewModel:BaseViewModel
    {
        public static ViewModel.AdminViewModel AdminViewModel { get; set; }
        private UserControl _mainControl;
        public UserControl MainControl
        {
            get => _mainControl;
           set
            {
                _mainControl = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<UserControl> Control;
        private ObservableCollection<Model.Sprav> MainDirect;

        private ObservableCollection<Model.Sprav> _direct;
        public ObservableCollection<Model.Sprav> Direct
        {
            get => _direct;
            set
            {
                _direct = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Model.Store> _store;
        public ObservableCollection<Model.Store> Store
        {
            get => _store;
            set
            {
                _store = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Model.Sprav> _direct1;
        public ObservableCollection<Model.Sprav> Direct1
        {
            get => _direct1;
            set
            {
                _direct1 = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Model.Product> _product_list;
        public ObservableCollection<Model.Product> ProductList
        {
            get => _product_list;
            set
            {
                _product_list = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date_1;
        public DateTime Date1
        {
            get => _date_1;
            set
            {
                _date_1 = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date_2;
        public DateTime Date2
        {
            get => _date_2;
            set
            {
                _date_2 = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Model.Product> _product_rep;
        public ObservableCollection<Model.Product> ProductRep
        {
            get => _product_rep;
            set
            {
                _product_rep = value;
                OnPropertyChanged();
            }
        }
        private int _current_page;
        public int CurrentPage
        {
            get => _current_page;
            set
            {
                _current_page = value;
                OnPropertyChanged();
            }
        }
        private int _all_price;
        public int AllPrice
        {
            get => _all_price;
            set
            {
                _all_price = value;
                OnPropertyChanged();
            }
        }
        private static MainViewModel main { get; set; }
        public static MainViewModel SelMain()
        {
            return main;
        }
        public Model.Employee Employee { get; private set; }
        private string _find_prod_text;
        public string FindProdText
        {
            get => _find_prod_text;
            set
            {
                _find_prod_text = value;
                OnPropertyChanged();
            }
        }
        private int _store_id;
        public int StoreId
        {
            get => _store_id;
            set
            {
                _store_id = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel(Model.Employee employee)
        {
            StoreId = 0;
            FindProdText = "";
            main = this;
            Employee = employee;
            if(employee.IsAdmin)
            {
                AdminViewModel = new ViewModel.AdminViewModel();
                Control = new ObservableCollection<UserControl> { new View.Order(), new View.Directory(), new View.Report(), new View.Employees(AdminViewModel), new View.Stores(AdminViewModel) };

            }
                
            else
                Control = new ObservableCollection<UserControl> { new View.Order(), new View.Directory(), new View.Report()};
            Date1 = DateTime.Now;
            Date2 = DateTime.Now;
            ProductList = new ObservableCollection<Model.Product>();
            using(var db=new ConnectDB())
            {
               
                Direct = new ObservableCollection<Model.Sprav>(db.Sprav.ToList());
                Direct1 = new ObservableCollection<Model.Sprav>(db.Sprav.ToList());
                MainDirect = new ObservableCollection<Model.Sprav>(db.Sprav.ToList());
            }
            SaveOrder = new DelegateCommand(AddOrder);
            CanselOrder = new DelegateCommand(CanselOrderCommand);
            MainControl = Control[0];
            CurrentPage = 1;
        }

        #region Navigation
        public ICommand OpenOrder => new RelayCommand(() => //Открыть страницу с заказом
        {
                MainControl = Control[0];
                using (var db = new ConnectDB())
                {
                    Direct1 = new ObservableCollection<Model.Sprav>(MainDirect);
                }
                SaveOrder = new DelegateCommand(AddOrder);
                CanselOrder = new DelegateCommand(CanselOrderCommand);
            CurrentPage = 1;

        });

        public ICommand OpenReport => new RelayCommand(() =>//Открыть окно с отчетом
        {
            MainControl = Control[2];
            Store = AdminViewModel.Store;
            Store.Insert(0, new Model.Store { Id = 0, Name = "Все магазины" });
            SaveOrder = new DelegateCommand(GetReport);
            CurrentPage = 3;

        });
        public ICommand OpenDirectory => new RelayCommand(() =>//Открыть окно со справочником
        {
            MainControl = Control[1];
            var db = new ConnectDB();

            SaveOrder = new DelegateCommand(SaveOrderCommand);
            CanselOrder = new DelegateCommand(CanselDirect);
            CurrentPage = 2;
        });
        public ICommand OpenEmployee => new RelayCommand(() => {

            MainControl = Control[3];
            CurrentPage = 4;
        });
        public ICommand OpenStores => new RelayCommand(() => {

            MainControl = Control[4];
            CurrentPage = 5;
        });
        #endregion
        #region ICommand
        public ICommand AddProdInOrder => new DelegateCommand<Model.Sprav>(AddInOrder);
        public ICommand UpdateProduct => new DelegateCommand<Model.Product>(UpdateProductCommand);
        public ICommand DeleteProduct => new DelegateCommand<Model.Product>(DeleteProductCommand);
        private ICommand _save_order;
        public ICommand SaveOrder
        {
            get => _save_order;
            set
            {
                _save_order = value;
                OnPropertyChanged();
            }
        }
        private ICommand _cansel_order;
        public ICommand CanselOrder
        {
            get => _cansel_order;
            set
            {
                _cansel_order = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region DelegateCommand
        public void AddInOrder(Model.Sprav sprav)//Добавить товар из справочника в заказ
        {
            if (sprav == null)
                return;
            Model.Product product = new Model.Product(sprav);
            ProductList.Add(product);
            GetAllPrice();
        }
       
        private  void UpdateProductCommand(Model.Product prod)//Обновить товар в заказе
        {
            if (prod == null)
                return;
            prod.Update(prod);
            
        }
        
        private void DeleteProductCommand(Model.Product prod)//Удалить товар из заказа
        {
            if (prod == null)
                return;
            ProductList.Remove(prod);

        }
        public void GetAllPrice()
        {
            AllPrice = 0;
            foreach (var prod in ProductList)
            {
                AllPrice += prod.CostForClient;
            }
        }
        private void GetReport()//Сформировать отчет
        {
            using (var db = new ConnectDB())
            {
                Date1 = new DateTime(Date1.Year, Date1.Month, Date1.Day, 0, 0, 0);
                Date2 = new DateTime(Date2.Year, Date2.Month, Date2.Day, 23, 59, 59);
                ProductRep = new ObservableCollection<Model.Product>();
                var query1 = db.Product.Include(p => p.Order).Include(p => p.Sprav).Include(p => p.Order.Employee).ThenInclude(p => p.Store).Where(p => p.Order.Date >= Date1 && p.Order.Date <= Date2 && (p.Order.Employee.StoreId == StoreId || StoreId == 0)).GroupBy(p => p.SpravId).Select(grp => new
                {
                    SpravId = grp.Key,
                    discount = grp.Sum(t => t.Discount),
                    cost_client = grp.Sum(t => t.CostForClient),
                    profit = grp.Sum(t => t.Profit),
                    count = grp.Sum(t => t.Count),
                    
                }).ToList();
                if(StoreId!=0)
                {
                    foreach (var a in query1)
                    {
                        ProductRep.Add(new Model.Product(a.SpravId, a.discount, a.cost_client, a.profit, a.count,Store.Where(p=>p.Id==StoreId).Select(p=>p.Name).FirstOrDefault()));
                    }

                   
                }
                else
                {
                    foreach(var store in Store)
                    {
                        if (!db.Product.Include(p => p.Order).Include(p => p.Sprav).Include(p => p.Order.Employee).ThenInclude(p => p.Store).Where(p => p.Order.Date >= Date1 && p.Order.Date <= Date2 && (p.Order.Employee.StoreId == store.Id)).Any())
                            continue;
                        var query_store = db.Product.Include(p => p.Order).Include(p => p.Sprav).Include(p => p.Order.Employee).ThenInclude(p => p.Store).Where(p => p.Order.Date >= Date1 && p.Order.Date <= Date2 && (p.Order.Employee.StoreId == store.Id)).GroupBy(p => p.SpravId).Select(grp => new
                        {
                            SpravId = grp.Key,
                            discount = grp.Sum(t => t.Discount),
                            cost_client = grp.Sum(t => t.CostForClient),
                            profit = grp.Sum(t => t.Profit),
                            count = grp.Sum(t => t.Count),

                        }).ToList();
                        foreach (var a in query_store)
                        {
                            ProductRep.Add(new Model.Product(a.SpravId, a.discount, a.cost_client, a.profit, a.count,store.Name));
                        }
                        ProductRep.Add(new Model.Product(query_store.Sum(p => p.discount), query_store.Sum(p => p.cost_client), query_store.Sum(p => p.profit), query_store.Sum(p => p.count), store.Name));
                    }
                }
                if (query1.Count != 0)
                {
                    ProductRep.Add(new Model.Product(query1.Sum(p => p.discount), query1.Sum(p => p.cost_client), query1.Sum(p => p.profit), query1.Sum(p => p.count)));
                }
            }
            
        }
        private void AddOrder()//Добавить заказ
        {
            
            using (var db=new ConnectDB())
            {
                if(ProductList.Count==0)
                {
                    MessageBox.Show("Заказ пустой", "Ошибка");
                }
                Model.Order order = new Model.Order(new ObservableCollection<Model.Product>(ProductList.Select(p=>p.prod(p)).ToList()), Employee.Id);
                db.Order.Add(order);
                db.SaveChanges();
                ProductList = new ObservableCollection<Model.Product>();
            }
            
            
        }
        
        private void SaveOrderCommand()//Сохранить справочник
        {
            using(var db=new ConnectDB())
            {
                db.UpdateRange(Direct);
                db.SaveChanges();
                if(FindProdText=="")
                    db.RemoveRange(db.Sprav.Where(p => !Direct.Select(s=> s.Id).Contains(p.Id)));
                db.SaveChanges();
                MainDirect = new ObservableCollection<Model.Sprav>(db.Sprav.ToList());
            }
        }
        private void CanselDirect()//Отменить изменения в справочнике
        {
            using (var db = new ConnectDB())
            {
                Direct = new ObservableCollection<Model.Sprav>(MainDirect);
                FindProdText = "";
            }
        }
        private void CanselOrderCommand()//Отменить изменения в заказе
        {
            ProductList = new ObservableCollection<Model.Product>();
        }
        public ICommand FindProd => new RelayCommand(() => { Direct = new ObservableCollection<Model.Sprav>(MainDirect.Where(p =>p.Product.ToLower().Contains(FindProdText))); });
        public ICommand DownloadOldBase => new RelayCommand(() => { AddFromOldDb.AddFromDb(); });
       
        #endregion

    }
}
