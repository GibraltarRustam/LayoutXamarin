using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using SQLite;
using System.Collections.Generic;
using System.IO;
using ZXing.Mobile;
using System;
using System.Net.NetworkInformation;

namespace LayoutXamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListView listView;
        private ArrayAdapter<Person> adapter;
        private List<Person> students;
        private string path;
        private SQLiteConnection db;

        MobileBarcodeScanner scanner;
        TextView textView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "mydb.db3");
            db = new SQLiteConnection(path);
            db.CreateTable<Person>();
            db.Insert(new Person { LastName = "Булат", FirstName = "Даукаев", Specialty = "Информационные системы и программирование", Group = "320", Email = "dauckaev.bulat@mail.ru" });

            students = db.Table<Person>().ToList();

            adapter = new ArrayAdapter<Person>(
                this,
                Android.Resource.Layout.SimpleListItem1,
                students);

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = adapter;

            var buttonClearDatabase = FindViewById<Button>(Resource.Id.staff);
            buttonClearDatabase.Click += ButtonClearDatabase_Click;

            MobileBarcodeScanner.Initialize(Application);
            SetContentView(Resource.Layout.activity_main);
            scanner = new MobileBarcodeScanner();
            var btn = FindViewById<Button>(Resource.Id.category);
            //textView = FindViewById<TextView>(Resource.Id.textView1);
            btn.Click += (o, e) =>
            {
                RunScan();
            };
        }

        private async void RunScan()
        {

            scanner.UseCustomOverlay = false;
            scanner.TopText = "Поместите штрих-код в видоискатель камеры для егоканирования.Штрих - код сканируется автоматически.";
            scanner.BottomText = "Старайтесь избегать теней и бликов. Держите устройствопримерно на 15 см от штрих - кода.";
            var result = await scanner.Scan(new MobileBarcodeScanningOptions
            {
                AutoRotate =
            false
            });
            HandleScanResult(result);
        }
        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";
            decimal _s = 0;
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                msg = "Найден штрих-код: " + result.Text;
                var code_value = result.Text;
                try
                {
                    char[] delimiters = { Convert.ToChar("&") };
                    string[] parts;
                    parts = code_value.Split(delimiters);
                    System.Collections.IEnumerator iPart = parts.GetEnumerator();
                    while (iPart.MoveNext())
                    {
                        if (iPart.Current.ToString().IndexOf("s=") == 0)
                        {
                            if (decimal.TryParse(iPart.Current.ToString().Replace("s=",
                            string.Empty), System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out _s) == false)
                            {
                                throw new Exception(
                            "Ошибка преобразования суммы чека.");
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    Toast.MakeText(this, err.Message, ToastLength.Long).Show();
                    return;
                }
                if (_s > 0)
                {
                    textView.Text = "Общая сумма покупки: " +
                    _s.ToString("#0.00") + " руб.";
                }
                else
                {
                    msg = "Ошибка разбора данных чека!\r\n" + msg;
                    Toast.MakeText(this, msg, ToastLength.Short).Show();
                }
            }
            else
            {
                msg = "Сканирование отменено!";
                Toast.MakeText(this, msg, ToastLength.Short).Show();
            }
        }





        private void ButtonClearDatabase_Click(object sender, System.EventArgs e)
        {
            db.DeleteAll<Person>();
            students.Clear();
            adapter.NotifyDataSetChanged();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}