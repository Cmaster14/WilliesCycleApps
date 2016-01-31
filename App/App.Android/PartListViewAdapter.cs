
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
using App.Portable;
using Android.Graphics;

namespace App.Android

{
	public class PartListViewAdapter : BaseAdapter<Part>
	{
		List<Part> items;
		Activity context;

		public PartListViewAdapter(Activity context, List<Part> items)

			: base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Part this [int position] {
			get { return items [position]; }
		}
		public override int Count
		{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items [position];
			var view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate (Resource.Layout.PartListView, null);
			var nameText = view.FindViewById<TextView> (Resource.Id.Name);
			nameText.Text = item.PartName;
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			nameText.SetTypeface (f, TypefaceStyle.Normal);
			string price = "$" + item.Price;
			var priceText = view.FindViewById<TextView> (Resource.Id.Price);
			priceText.Text = price;
			priceText.SetTypeface (f, TypefaceStyle.Normal);
			return view;
		}
	}
}

