using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Android.Graphics;
using System.IO;

namespace App.Android
{
     [Activity(Label = "LicensureActivity")]
     public class LicensureActivity : Activity
     {
          protected override void OnCreate(Bundle savedInstanceState)
          {
               base.OnCreate(savedInstanceState);

               WebView view = new WebView(this);
               string html;
               using (var sr = new StreamReader(Assets.Open("licensure.html")))
               {
                    html = sr.ReadToEnd();
               }
               view.LoadData(html, "text/html", "UTF-8");
               view.SetBackgroundColor(Color.Transparent);
               SetContentView(view);

          }
     }
}