using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using System.Linq;
using Android.Graphics;
using App2.Fragments;

namespace App2
{
    [Activity(Label = "ActivityWithFragments")]
    public class ActivityWithFragments : Activity
    {
        GestureDetector gestureDetector;
        GestureListener gestureListener;

        ListView menuListView;
        ImageButton menuButton;
        MenuListAdapterClass objAdapterMenu;

        int intDisplayWidth;
        bool isSingleTapFired = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.testLayout1);
            FnInitialization();
            TapEvent();
            FnBindMenu(); //find definition in below stepsù


            // Begin the transaction

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            // Replace the contents of the container with the new fragment
            ft.Replace(Resource.Id.Frame_Content, new FragmentBlue());
            
            ft.Commit();
        }
        void FnInitialization()
        {
            //gesture initialization
            gestureListener = new GestureListener();
            gestureListener.LeftEvent += GestureLeft; //find definition in below steps
            gestureListener.RightEvent += GestureRight;
            gestureListener.SingleTapEvent += SingleTap;
            gestureDetector = new GestureDetector(this, gestureListener);

            menuListView = FindViewById<ListView>(Resource.Id.menuListView);
            menuButton = FindViewById<ImageButton>(Resource.Id.imageButton1);

            //changed sliding menu width to 3/4 of screen width 
            Display display = this.WindowManager.DefaultDisplay;
            var point = new Point();
            display.GetSize(point);
            intDisplayWidth = point.X;
            intDisplayWidth = intDisplayWidth - (intDisplayWidth / 3);
            using (var layoutParams = menuListView.LayoutParameters)
            {
                layoutParams.Width = intDisplayWidth;
                layoutParams.Height = ViewGroup.LayoutParams.MatchParent;
                menuListView.LayoutParameters = layoutParams;
            }
        }
        void TapEvent()
        {
            //title bar menu icon
            menuButton.Click += delegate (object sender, EventArgs e)
            {
                if (!isSingleTapFired)
                {
                    FnToggleMenu();  //find definition in below steps
                    isSingleTapFired = false;
                }
            };          
        }
        //toggling the left menu
        void FnToggleMenu()
        {
            Console.WriteLine(menuListView.IsShown);
            if (menuListView.IsShown)
            {
                menuListView.Animation = new TranslateAnimation(0f, -menuListView.MeasuredWidth, 0f, 0f);
                menuListView.Animation.Duration = 300;
                menuListView.Visibility = ViewStates.Gone;
            }
            else
            {
                menuListView.Visibility = ViewStates.Visible;
                menuListView.RequestFocus();
                menuListView.Animation = new TranslateAnimation(-menuListView.MeasuredWidth, 0f, 0f, 0f);//starting edge of layout 
                menuListView.Animation.Duration = 300;
            }
        }
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            gestureDetector.OnTouchEvent(ev);
            return base.DispatchTouchEvent(ev);
        }
        void GestureLeft()
        {
            if (!menuListView.IsShown)
                FnToggleMenu();
            isSingleTapFired = false;
        }
        void GestureRight()
        {
            if (menuListView.IsShown)
                FnToggleMenu();
            isSingleTapFired = false;
        }
        void SingleTap()
        {
            if (menuListView.IsShown)
            {
                FnToggleMenu();
                isSingleTapFired = true;
            }
            else
            {
                isSingleTapFired = false;
            }
        }
        void FnBindMenu()
        {
            string[] strMnuText = { "Red", "Blue", "Green"};
            int[] strMnuUrl = { Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon };
            if (objAdapterMenu != null)
            {
                objAdapterMenu.actionMenuSelected -= FnMenuSelected;
                objAdapterMenu = null;
            }
            objAdapterMenu = new MenuListAdapterClass(this, strMnuText, strMnuUrl);
            objAdapterMenu.actionMenuSelected += FnMenuSelected;
            menuListView.Adapter = objAdapterMenu;
        }
        void FnMenuSelected(string strMenuText)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            switch (strMenuText)
            {
                case "Red":
                    // Replace the contents of the container with the new fragment
                    ft.Replace(Resource.Id.Frame_Content, new FragmentRed());
                    ft.Commit();
                    break;
                case "Blue":
                    // Replace the contents of the container with the new fragment
                    ft.Replace(Resource.Id.Frame_Content, new FragmentBlue());
                    ft.Commit();
                    break;
                case "Green":
                    // Replace the contents of the container with the new fragment
                    ft.Replace(Resource.Id.Frame_Content, new FragmentGreen());
                    ft.Commit();
                    break;
                default:
                    throw new InvalidOperationException("Should not happend");
            }
        }
    }
}

