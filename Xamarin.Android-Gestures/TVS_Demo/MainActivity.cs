using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static Android.Views.GestureDetector;
using static Android.Views.View;

namespace TVS_Demo
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, IOnTouchListener, IOnGestureListener
    {
        private TextView txtGestureView;
        private readonly int SWIPE_MIN_DISTANCE = 120;
        private static int SWIPE_MAX_OFF_PATH = 250;
        private static int SWIPE_THRESHOLD_VELOCITY = 200;

        private int imageIndex = 0;

        private GestureDetector gestureDetector;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            txtGestureView = FindViewById<TextView>(Resource.Id.imageView);

            gestureDetector = new GestureDetector(this);

            txtGestureView.SetOnTouchListener(this);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            Toast.MakeText(this, "On Touch", ToastLength.Short).Show();

            return gestureDetector.OnTouchEvent(e);
        }

        public bool OnDown(MotionEvent e)
        {
            Toast.MakeText(this, "On Down", ToastLength.Short).Show();

            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            bool result = false;

            try
            {
                float diffY = e2.GetY() - e1.GetY();

                float diffX = e2.GetX() - e1.GetX();

                if (Math.Abs(diffX) > Math.Abs(diffY))
                {
                    if (Math.Abs(diffX) > SWIPE_THRESHOLD_VELOCITY && Math.Abs(velocityX) > SWIPE_THRESHOLD_VELOCITY)
                    {
                        if (diffX > 0)
                        {
                            //onSwipeRight();
                            if (imageIndex > 0)
                            {
                                imageIndex--;
                            }
                            txtGestureView.Text = "Swiped Right";
                        }
                        else
                        {
                            if (imageIndex < 28)
                            {
                                imageIndex++;
                            }

                            //onSwipeLeft();
                            txtGestureView.Text = "Swiped Left";
                        }


                        result = true;
                    }
                }
                else
                if (Math.Abs(diffY) > SWIPE_THRESHOLD_VELOCITY && Math.Abs(velocityY) > SWIPE_THRESHOLD_VELOCITY)
                {
                    if (diffY > 0)
                    {
                        //onSwipeBottom();
                        txtGestureView.Text = "Swiped Bottom";
                    }
                    else
                    {
                        //onSwipeTop();
                        txtGestureView.Text = "Swiped Top";

                    }
                    result = true;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return result;
        }

        public void OnLongPress(MotionEvent e)
        {
            Toast.MakeText(this, "On Long Press", ToastLength.Short).Show();
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            Toast.MakeText(this, "On Scroll", ToastLength.Short).Show();

            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
            Toast.MakeText(this, "On Show Press", ToastLength.Short).Show();

        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            Toast.MakeText(this, "On Single Tab Up", ToastLength.Short).Show();

            return true;
        }
    }
}

