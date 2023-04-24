using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using SQLite;
using System.Collections.Generic;
using System.IO;

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "mydb.db3");
            db = new SQLiteConnection(path);
            db.CreateTable<Person>();
            //db.Insert(new Person { LastName = "", FirstName = "", Specialty = "Информационные системы и программирование", Group = "320", Email = "" });

            students = db.Table<Person>().ToList();

            adapter = new ArrayAdapter<Person>(
                this,
                Android.Resource.Layout.SimpleListItem1,
                students);

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = adapter;

            var buttonClearDatabase = FindViewById<Button>(Resource.Id.staff);
            buttonClearDatabase.Click += ButtonClearDatabase_Click;
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