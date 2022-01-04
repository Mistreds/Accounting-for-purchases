using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Accounting_for_purchases
{
   public   class Setting
    {
        public  string Key { get; set; }
        public string Value { get; set; }
        [XmlIgnore]
        public static ObservableCollection<Setting> Settings { get; private set; }
        [XmlIgnore]
        public static string PathConnect { get { string path_connect = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AccountingForPurchases"; return path_connect; } }
        public Setting() {}
        public Setting(string Key, string value)
        {
            this.Key = Key;
            this.Value = value;
        }
        public Setting(string Key)
        {
            this.Key=Key;
            Value = "";
        }
        public static string GetDatabaseSetting()
        {
            var server = Settings.Where(p => p.Key == "server").FirstOrDefault();
            var user = Settings.Where(p => p.Key == "user").FirstOrDefault();
            var password = Settings.Where(p => p.Key == "password").FirstOrDefault();
            var database = Settings.Where(p => p.Key == "database").FirstOrDefault();
            return $"{server.Key}={server.Value};{user.Key}={user.Value};{password.Key}={password.Value};{database.Key}={database.Value}";
        }
        public static string GetServer()
        {
            return Settings.Where(p => p.Key == "server").FirstOrDefault().Value;
        }
        public static string GetUser()
        {
            return Settings.Where(p => p.Key == "user").FirstOrDefault().Value;
        }
        public static string GetPassword()
        {
            return Settings.Where(p => p.Key == "password").FirstOrDefault().Value;
        }
        public static string GetDatabase()
        {
            return  Settings.Where(p => p.Key == "database").FirstOrDefault().Value;
        }
        public static void SetDatabaseSetting(string server1, string user1, string password1,string database1)
        {
            var server = Settings.Where(p => p.Key == "server").FirstOrDefault();
            server.Value = server1;
            var user = Settings.Where(p => p.Key == "user").FirstOrDefault();
            user.Value= user1;
            var password = Settings.Where(p => p.Key == "password").FirstOrDefault();
            password.Value =password1;
            var database = Settings.Where(p => p.Key == "database").FirstOrDefault();
            database.Value = database1;
            File.Delete(PathConnect + @"\Setting.xml");
            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Setting>));
            using (FileStream fs = new FileStream(PathConnect + @"\Setting.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Settings);
            }
        }
        public static void InitSetting()
        {
            if (File.Exists($"{PathConnect}\\Setting.xml"))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Setting>));
                using (FileStream fs = new FileStream(PathConnect + @"\Setting.xml", FileMode.OpenOrCreate))
                {
                    Settings = (ObservableCollection<Setting>)formatter.Deserialize(fs);
                }
            }
            else
            {

                DirectoryInfo dirInfo = new DirectoryInfo(PathConnect);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                Settings = new ObservableCollection<Setting> { new Setting("server"), new Setting("user"), new Setting("password"), new Setting("database") };
                XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Setting>));
                using (FileStream fs = new FileStream(PathConnect + @"\Setting.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, Settings);

                    Console.WriteLine("Объект сериализован");
                }
            }
        }
    }
}
