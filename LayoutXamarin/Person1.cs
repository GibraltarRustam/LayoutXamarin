using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LayoutXamarin
{
    public class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Specialty { get; set; }
        public string Group { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {Specialty} {Email} ({Group})";
        }
    }



}