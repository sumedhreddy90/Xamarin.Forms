using System.Collections.Generic;
using CoreGraphics;
using UIKit;
using System.Linq;

namespace Xamarin.Forms.Platform.iOS
{
	public static class ViewExtensions
	{
		public static IEnumerable<Page> GetParentPages(this Page target)
		{
			var result = new List<Page>();
			var parent = target.RealParent as Page;
			while (!Application.IsApplicationOrNull(parent))
			{
				result.Add(parent);
				parent = parent.RealParent as Page;
			}

			return result;
		}

        public static UIView ToiOS(this Xamarin.Forms.View view, CGRect size)
        {
            if (Platform.GetRenderer(view) == null)
                Platform.SetRenderer(view, Platform.CreateRenderer(view));

            var vRenderer = Platform.GetRenderer(view);

            vRenderer.NativeView.Frame = size;

            vRenderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            vRenderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            vRenderer.Element?.Layout(size.ToRectangle());

            var nativeView = vRenderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;
        }

		internal static T FindParentOfType<T>(this VisualElement element)
		{
			var navPage = element.GetParentsPath()
										.OfType<T>()
										.FirstOrDefault();
			return navPage;
		}

		internal static IEnumerable<Element> GetParentsPath(this VisualElement self)
		{
			Element current = self;

			while (!Application.IsApplicationOrNull(current.RealParent))
			{
				current = current.RealParent;
				yield return current;
			}
		}
	}
}