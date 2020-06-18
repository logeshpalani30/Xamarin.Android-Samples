using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using XAExpandableListView.Adapters;

namespace XAExpandableListView
{
    [Activity(Label = "Expanadable ListView", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ExpandableListAdapter listAdapter;
        private ExpandableListView expandableList;
        private List<string> listHeaderData;
        private Dictionary<string, List<ExpandableModel>> listChildData;
        private List<ExpandableModel> groupData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            expandableList = FindViewById(Resource.Id.expandList) as ExpandableListView;

            LoadData();

            listAdapter = new ExpandableListAdapter(this, listHeaderData, listChildData);
            expandableList.SetAdapter(listAdapter);
        }

        private void LoadData()
        {
            listHeaderData = new List<string>();
            listChildData = new Dictionary<string, List<ExpandableModel>>();
            groupData = new List<ExpandableModel>();

            listHeaderData.Add("Idea Generation");
            listHeaderData.Add("Verbal abilities");
            listHeaderData.Add("Quantitive abilities");
            listHeaderData.Add("Memory");
            listHeaderData.Add("Perceptual abilities");

            groupData.Add(new ExpandableModel() { Name = "Fluency of Ideas", Value = "Done" });
            groupData.Add(new ExpandableModel() { Name = "Originality", Value = "Pending" });
            groupData.Add(new ExpandableModel() { Name = "Problem Sensitivity", Value = null });
            groupData.Add(new ExpandableModel() { Name = "Mathamatical Reasoning", Value = "Pending" });
            groupData.Add(new ExpandableModel() { Name = "Problem Sensitivity", Value = null });
            groupData.Add(new ExpandableModel() { Name = "Speed of closure", Value = null });
            groupData.Add(new ExpandableModel() { Name = "Number Facility", Value = null });
            groupData.Add(new ExpandableModel() { Name = "Speed of closure", Value = null });
            groupData.Add(new ExpandableModel() { Name = "Flexiblity of closure", Value = null });


            for (int i = 0; i < listHeaderData.Count; i++)
            {
                listChildData.Add(listHeaderData[i], groupData);
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
