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

namespace App.Android
{
     public class ItemSelectedListener
     {
          private List<string> mYears = new List<string>() { "none" };
          private List<string> mParts = new List<string>() { "none" };
          private string make = "make", year = "year", part = "part";
          public AdapterListener mListener = null;

          public interface AdapterListener
          {
               void search(string make, string year, string part);
          }

          public void setListener(AdapterListener listener)
          {
               mListener = listener;
          }

          public void onAttach(Activity activity) {
               onAttach(activity);
        
               // This makes sure that the container activity has implemented
               // the callback interface. If not, it throws an exception
               try {
                    mListener = (AdapterListener) activity;
               } catch (Exception e) {
                    throw new Exception(activity.ToString()
                         + " must implement OnHeadlineSelectedListener");
               }
          }
     }
}