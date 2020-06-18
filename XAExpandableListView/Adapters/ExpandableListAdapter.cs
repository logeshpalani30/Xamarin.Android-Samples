using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace XAExpandableListView.Adapters
{
    public class ExpandableListAdapter : BaseExpandableListAdapter
    {
        public Activity context;
        public List<string> headerList;
        public Dictionary<string, List<ExpandableModel>> childList;

        public ExpandableListAdapter(Activity contextactivity, List<string> headerlist, Dictionary<string, List<ExpandableModel>> childlist)
        {
            context = contextactivity;
            headerList = headerlist;
            childList = childlist;
        }

        public override int GroupCount
        {
            get
            {
                return headerList.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return childList[headerList[groupPosition]][childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return childList[headerList[groupPosition]].Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var textChild = (ExpandableModel)GetChild(groupPosition, childPosition);
            convertView = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.expand_list_chlid, null);
            var txtHeader = (TextView)convertView.FindViewById(Resource.Id.txtChildText);
            var mtxtorImage = (TextView)convertView.FindViewById(Resource.Id.textorimage);
            var mImageortxt = (ImageView)convertView.FindViewById(Resource.Id.imageortext);

            txtHeader.Text = textChild.Name;
            txtHeader.SetTextColor(Android.Graphics.Color.ParseColor("#ff000000"));

            if (textChild.Value == "Done")
            {
                mtxtorImage.Visibility = ViewStates.Visible;
                txtHeader.SetTextColor(Android.Graphics.Color.ParseColor("#ff000000"));
                mImageortxt.Visibility = ViewStates.Gone;
            }
            else if (textChild.Value == "Pending")
            {
                mImageortxt.Visibility = ViewStates.Visible;
                txtHeader.SetTextColor(Android.Graphics.Color.ParseColor("#585858"));
                mtxtorImage.Visibility = ViewStates.Gone;
            }
            else if (textChild.Value == null)
            {
                mImageortxt.Visibility = ViewStates.Gone;
                txtHeader.SetTextColor(Android.Graphics.Color.ParseColor("#ff000000"));
                mtxtorImage.Visibility = ViewStates.Gone;
            }
            return convertView;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return headerList[groupPosition];
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var textHead = (string)GetGroup(groupPosition);
            convertView = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.list_view_header, null);
            var txtView = (TextView)convertView.FindViewById(Resource.Id.txtHeaderText);
            var imageSource = (ImageView)convertView.FindViewById(Resource.Id.imagearrow);
            txtView.Text = textHead;

            if (isExpanded == true)
                imageSource.SetImageResource(Resource.Drawable.up_arrow_one);
            else
                imageSource.SetImageResource(Resource.Drawable.down_arrow_one);
            return convertView;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}
