using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Com.Yalantis.Contextmenu.Lib;
using Android.Graphics.Drawables;
using Android.App;
using Android.Support.V7.App;
using Com.Yalantis.Contextmenu.Lib.Interfaces;

namespace ContextMenuXamarinAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMenuItemClickListener
    {

        private Button btn;

        private ContextMenuDialogFragment mMenuDialogFragment;
        private Android.Support.V4.App.FragmentManager fragmentManager;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btn = FindViewById<Button>(Resource.Id.btnClick);

            fragmentManager = SupportFragmentManager;

            btn.Click += (sender, args) =>
            {
                if (fragmentManager.FindFragmentByTag(ContextMenuDialogFragment.TAG) == null)
                {
                    mMenuDialogFragment.Show(fragmentManager, ContextMenuDialogFragment.TAG);
                }
            };

            initMenuFragment();

        }


        private void initMenuFragment()
        {
            MenuParams menuParams = new MenuParams();
            //menuParams.ActionBarSize = (int)Resources.GetDimension(Resource.Dimension.tool_bar_height);
            menuParams.ActionBarSize = 150;
            menuParams.MenuObjects = getMenuObjects();
            menuParams.ClosableOutside = true;
            mMenuDialogFragment = ContextMenuDialogFragment.NewInstance(menuParams);
            mMenuDialogFragment.setItemClickListener(this);
            //mMenuDialogFragment.setItemLongClickListener(this);
        }

        private List<MenuObject> getMenuObjects()
        {
            // You can use any [resource, bitmap, drawable, color] as image:
            // item.setResource(...)
            // item.setBitmap(...)
            // item.setDrawable(...)
            // item.setColor(...)
            // You can set image ScaleType:
            // item.setScaleType(ScaleType.FIT_XY)
            // You can use any [resource, drawable, color] as background:
            // item.setBgResource(...)
            // item.setBgDrawable(...)
            // item.setBgColor(...)
            // You can use any [color] as text color:
            // item.setTextColor(...)
            // You can set any [color] as divider color:
            // item.setDividerColor(...)

            List<MenuObject> menuObjects = new List<MenuObject>();

            MenuObject close = new MenuObject();
            close.Resource = Resource.Drawable.icn_close;

            MenuObject send = new MenuObject("Send message");
            send.Resource = Resource.Drawable.icn_1;

            MenuObject like = new MenuObject("Like profile");
            Bitmap b = BitmapFactory.DecodeResource(Resources, Resource.Drawable.icn_2);
            like.Bitmap = b;

            MenuObject addFr = new MenuObject("Add to friends");
            BitmapDrawable bd = new BitmapDrawable(Resources, BitmapFactory.DecodeResource(Resources, Resource.Drawable.icn_3));
            addFr.Drawable = bd;

            MenuObject addFav = new MenuObject("Add to favorites");
            addFav.Resource = Resource.Drawable.icn_4;

            MenuObject block = new MenuObject("Block user");
            block.Resource = Resource.Drawable.icn_5;

            menuObjects.Add(close);
            menuObjects.Add(send);
            menuObjects.Add(like);
            menuObjects.Add(addFr);
            menuObjects.Add(addFav);
            menuObjects.Add(block);
            return menuObjects;
        }

        public void OnMenuItemClick(View p0, int p1)
        {
            //throw new NotImplementedException();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}