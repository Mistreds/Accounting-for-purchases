using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Accounting_for_purchases.ViewModel
{
    

   public class AdminViewModel:BaseViewModel
    {
        private ObservableCollection<Model.Employee> _employees;
        public ObservableCollection<Model.Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
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
        private ObservableCollection<Model.Store> _storeTable;
        public ObservableCollection<Model.Store> StoreTable
        {
            get => _storeTable;
            set
            {
                _storeTable = value;
                OnPropertyChanged();
            }
        }
        public AdminViewModel()
        {
            using (var db = new ConnectDB())
            {
                Employees = new ObservableCollection<Model.Employee>(db.Employee.Include(p=>p.Store));
                Store = new ObservableCollection<Model.Store>(db.Store.ToList());
                StoreTable = new ObservableCollection<Model.Store>(db.Store.Where(p=>p.Id!=1).ToList());
            }
        }
        public ICommand SaveEmp => new RelayCommand(() => { using (var db = new ConnectDB()) {

                db.UpdateRange(Employees);
                db.SaveChanges();
            
            
            } });
        public ICommand DelEmp => new DelegateCommand<Model.Employee>(DeleteEmp);
        private void DeleteEmp(Model.Employee emp)
        {
      
            if(emp.Id==0)
            {
                Employees.Remove(emp);
                return;
            }
            using (var db=new ConnectDB())
            {
                if (MessageBox.Show($"Вы действительно хотите удалить сотрудника {emp.Fio}, все его заказы будут удалены ?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {

                    db.Remove(emp);
                    db.SaveChanges();
                        Employees.Remove(emp);
                    

                }
               
            }
        }
        public ICommand CanselEmp => new RelayCommand(() => {
            using (var db = new ConnectDB())
            {

                Employees = new ObservableCollection<Model.Employee>(db.Employee.Include(p => p.Store));


            }
        });
        public ICommand SaveStore => new RelayCommand(() => {
            using (var db = new ConnectDB())
            {

                db.UpdateRange(StoreTable);
                db.SaveChanges();
                Store = new ObservableCollection<Model.Store>(db.Store.ToList());

            }
        });
        public ICommand DelStore => new DelegateCommand<Model.Store>(DeleteStore);
        private void DeleteStore(Model.Store store)
        {
            if (store == null)
                return;
            if(store.Id==0)
            {
                StoreTable.Remove(store);
                return;
            }
            using (var db = new ConnectDB())
            {
                if (MessageBox.Show($"Вы действительно хотите удалить магазин {store.Name} ?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {

                    foreach(var em in db.Employee.Where(p=>p.StoreId==store.Id))
                    {
                        var emp=Employees.Where(p => p.Id == em.Id).FirstOrDefault();
                        emp.StoreId=1;
                        db.Update(emp);
                        db.SaveChanges();
                    }
                    db.Remove(store);
                    db.SaveChanges();
                    Store.Remove(store);
                    StoreTable.Remove(store);


                }
              
            }
        }
        public ICommand CanselStore => new RelayCommand(() => {
            using (var db = new ConnectDB())
            {

                StoreTable = new ObservableCollection<Model.Store>(db.Store.Where(p => p.Id != 1).ToList());


            }
        });
    }
}
