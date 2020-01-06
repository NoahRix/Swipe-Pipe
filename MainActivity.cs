using System;
using System.Collections.Generic;
using Android.Gms.Ads;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CocosSharp;

namespace Swipe_Pipe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //Make fullscreen
            Window.AddFlags(WindowManagerFlags.Fullscreen);

            //Initiate ad
            string appid = "ca-app-pub-3542202851056774~5261967133";
            string inter_ad_id = "ca-app-pub-xxxxxxxxxxxxxxxxxxxxxxxxxxx";
            string int_ad_test = "ca-app-pub-3940256099942544/1033173712";
            string banner_1_id = "ca-app-pub-xxxxxxxxxxxxxxxxxxxxxxxxxxx";
            AdView adView1 = FindViewById<AdView>(Resource.Id.adView1);
            MobileAds.Initialize(this, appid);
            AdRequest adRequest1 = new AdRequest.Builder().Build();
            adView1.LoadAd(adRequest1);


            //Get game view
            CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);
            //+= operator sends the object CCGameView into LoadGame void.
            gameView.ViewCreated += LoadGame;
        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;
            if (gameView != null)
            {
                var ContentSearchPaths = new List<string>() { "Fonts", "Sounds" };
                CCSizeI viewSize = gameView.ViewSize;

                int width = Resources.DisplayMetrics.WidthPixels;
                int height = Resources.DisplayMetrics.HeightPixels;

                //Set dimensions 
                gameView.DesignResolution = new CCSizeI(width, height);

                gameView.ContentManager.SearchPaths = ContentSearchPaths;

                CCScene gameScene = new CCScene(gameView);

                gameScene.AddLayer(new GameLayer(this));
                gameView.RunWithScene(gameScene);
            }
        }
    }
}
