using System.Reflection;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;
using Foundation;
using QuickLayout.Core.ViewModels;
using ObjCRuntime;
using Cirrious.FluentLayouts;

namespace QuickLayout.Touch.Views
{
    [Register("DetailsView")]
    public class DetailsView : MvxViewController
    {
        public override void ViewDidLoad()
        {
            var scrollView = new UIScrollView()
                {
                    BackgroundColor = UIColor.White,
                    ShowsHorizontalScrollIndicator = false, 
                    AutoresizingMask = UIViewAutoresizing.FlexibleHeight,
                };
            View = scrollView;
            scrollView.TranslatesAutoresizingMaskIntoConstraints = true;
            base.ViewDidLoad();

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            var set = this.CreateBindingSet<DetailsView, DetailsViewModel>();

            foreach (var propertyInfo in typeof(DetailsViewModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (propertyInfo.PropertyType != typeof (string))
                    continue;

                var introLabel = new UILabel
                {
                    Text = propertyInfo.Name + ":",
                    TranslatesAutoresizingMaskIntoConstraints = false,
                };
                Add(introLabel);
                var textField = new UITextField
                    {
                        BorderStyle = UITextBorderStyle.RoundedRect,
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        BackgroundColor = UIColor.LightGray
                    };
                Add(textField);
                var label = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    BackgroundColor = UIColor.Yellow
                };
                Add(label);

                set.Bind(label).To(propertyInfo.Name);
                set.Bind(textField).To(propertyInfo.Name);
            }
            set.Apply();

            var constraints = View.VerticalStackPanelConstraints(
                                                   new Margins(20, 10, 20, 10, 5, 5),
                                                   View.Subviews);
            View.AddConstraints(constraints);
        }
    }
}