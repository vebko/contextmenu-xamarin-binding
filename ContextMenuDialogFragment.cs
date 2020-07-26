using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Yalantis.Contextmenu.Lib;
using Com.Yalantis.Contextmenu.Lib.Interfaces;
using Android.Support.V4.App;


namespace ContextMenuXamarinAndroid
{
    class ContextMenuDialogFragment : DialogFragment, IOnItemClickListener, IOnItemLongClickListener
    {

        public static String TAG = typeof(ContextMenuDialogFragment).Name;
        private static String BUNDLE_MENU_PARAMS = "BUNDLE_MENU_PARAMS";

        private LinearLayout mWrapperButtons;
        private LinearLayout mWrapperText;
        private MenuAdapter mDropDownMenuAdapter;
        private IOnMenuItemClickListener mItemClickListener;
        private IOnMenuItemLongClickListener mItemLongClickListener;
        private MenuParams mMenuParams;

        public static int STYLE_NO_FRAME = 2;

        public static ContextMenuDialogFragment NewInstance(MenuParams menuParams)
        {
            ContextMenuDialogFragment fragment = new ContextMenuDialogFragment();
            Bundle args = new Bundle();
            args.PutParcelable(BUNDLE_MENU_PARAMS, menuParams);
            fragment.Arguments = args;
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetStyle(StyleNoFrame, Resource.Style.MenuFragmentStyle);
            if (Arguments != null)
            {
                mMenuParams = (MenuParams)Arguments.GetParcelable(BUNDLE_MENU_PARAMS);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_menu, container, false);

            rootView.SetFitsSystemWindows(mMenuParams.FitsSystemWindow);
            ((ViewGroup)rootView).SetClipToPadding(mMenuParams.ClipToPadding);
            initViews(rootView);
            Dialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
            initDropDownMenuAdapter();

            Handler h = new Handler();
            Action myAction = () =>
            {
                // your code that you want to delay here
                mDropDownMenuAdapter.MenuToggle();
            };
            h.PostDelayed(myAction, mMenuParams.AnimationDelay);

            if (mMenuParams.ClosableOutside)
            {
                rootView.FindViewById<RelativeLayout>(Resource.Id.root).Click += (sender, args) =>
                {
                    if (IsAdded)
                    {
                        DismissAllowingStateLoss();
                    }
                };
            }

            return rootView;
        }

        private void initViews(View view)
        {
            mWrapperButtons = view.FindViewById<LinearLayout>(Resource.Id.wrapper_buttons);
            mWrapperText = view.FindViewById<LinearLayout>(Resource.Id.wrapper_text);
        }

        private void initDropDownMenuAdapter()
        {
            mDropDownMenuAdapter = new MenuAdapter(Activity, mWrapperButtons, mWrapperText,
                    mMenuParams.MenuObjects, mMenuParams.ActionBarSize);
            mDropDownMenuAdapter.SetOnItemClickListener(this);
            mDropDownMenuAdapter.SetOnItemLongClickListener(this);
            mDropDownMenuAdapter.SetAnimationDuration(mMenuParams.AnimationDuration);
        }

        private void close()
        {
            Handler h = new Handler();
            Action myAction = () =>
            {
                DismissAllowingStateLoss();
            };
            h.PostDelayed(myAction, mMenuParams.AnimationDelay);
        }

        public void setItemLongClickListener(IOnMenuItemLongClickListener itemLongClickListener)
        {
            this.mItemLongClickListener = itemLongClickListener;
        }

        public void setItemClickListener(IOnMenuItemClickListener itemClickListener)
        {
            this.mItemClickListener = itemClickListener;
        }

        void IOnItemClickListener.OnClick(View v)
        {
            if (mItemClickListener != null)
            {
                mItemClickListener.OnMenuItemClick(v, ((ViewGroup)v.Parent).IndexOfChild(v));
            }
            close();
        }

        void IOnItemLongClickListener.OnLongClick(View v)
        {
            if (mItemLongClickListener != null)
            {
                mItemLongClickListener.OnMenuItemLongClick(v, ((ViewGroup)v.Parent).IndexOfChild(v));
            }
            close();
        }
    }
}