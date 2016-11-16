using GestureUnlockPatternIOS.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;

namespace GestureUnlockPatternIOS
{
    public class ViewController : UIViewController, IUIAlertViewDelegate
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(false, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = @"手势解锁";
            View.BackgroundColor = UIColor.White;
            UIButton btn1 = new UIButton();
            btn1.Frame = new CoreGraphics.CGRect(106, 100, 208, 34);
            btn1.SetTitle ("设置手势密码", UIControlState.Normal);
            btn1.TitleColor(UIControlState.Normal);
            btn1.BackgroundColor = UIColor.Purple;
            btn1.Tag = 1;
            btn1.TouchDown += Btn1_TouchDown;
            View.AddSubview(btn1);

            UIButton btn2 = new UIButton();
            btn2.Frame = new CoreGraphics.CGRect(106, 200, 208, 34);
            btn2.SetTitle("登陆手势密码", UIControlState.Normal);
            btn2.TitleColor(UIControlState.Normal);
            btn2.BackgroundColor = UIColor.Red;
            btn2.Tag = 2;
            btn2.TouchDown += Btn1_TouchDown;
            View.AddSubview(btn2);

            UIButton btn3 = new UIButton();
            btn3.Frame = new CoreGraphics.CGRect(106, 300, 208, 34);
            btn3.SetTitle("验证手势密码", UIControlState.Normal);
            btn3.TitleColor(UIControlState.Normal);
            btn3.BackgroundColor = UIColor.Red;
            btn3.Tag = 3;
            btn3.TouchDown += Btn1_TouchDown;
            View.AddSubview(btn3);

            UIButton btn4 = new UIButton();
            btn4.Frame = new CoreGraphics.CGRect(106, 400, 208, 34);
            btn4.SetTitle("修改手势密码", UIControlState.Normal);
            btn4.TitleColor(UIControlState.Normal);
            btn4.BackgroundColor = UIColor.Red;
            btn4.Tag = 4;
            btn4.TouchDown += Btn1_TouchDown;
            View.AddSubview(btn4);
        }

        private void Btn1_TouchDown(object sender, EventArgs e)
        {
            BtnClick((UIButton)sender);
        }

        void BtnClick(UIButton sender)
        {
            switch (sender.Tag)
            {
                case 1:
                    GestureViewController gestureVc1 = new GestureViewController();
                    gestureVc1.type = GestureViewControllerType.GestureViewControllerTypeSetting;
                    NavigationController.PushViewController(gestureVc1, true);
                    break;
                case 2:
                    if (PCCircleViewConst.GetGesturePassWord().Length > 0)
                    {
                        GestureViewController gestureVc2 = new GestureViewController();
                        gestureVc2.type = GestureViewControllerType.GestureViewControllerTypeLogin;
                        NavigationController.PushViewController(gestureVc2, true);
                    }
                    else
                    {
                        UIAlertView alerView = new UIAlertView("提示", "暂未设置手势密码，是否前往设置", new MyUIAlertViewDelegate(NavigationController), "取消", "设置");
                        alerView.Show();
                    }
                    break;
                case 3:
                    GestureVerifyViewController gestureVerifyVc1 = new GestureVerifyViewController();
                    NavigationController.PushViewController(gestureVerifyVc1, true);
                    break;
                case 4:
                    GestureVerifyViewController gestureVerifyVc2 = new GestureVerifyViewController();
                    gestureVerifyVc2.isToSetNewGesture = true;
                    NavigationController.PushViewController(gestureVerifyVc2, true);
                    break;
                default:
                    break;
            }
        }
    }
    public class MyUIAlertViewDelegate : UIAlertViewDelegate
    {
        UINavigationController navigationController;
        public MyUIAlertViewDelegate(UINavigationController navigationController)
        {
            this.navigationController = navigationController;
        }
        public override void Clicked(UIAlertView alertview, nint buttonIndex)
        {
            if (buttonIndex == 1)
            {
                GestureViewController gestureVc = new GestureViewController();
                gestureVc.type = GestureViewControllerType.GestureViewControllerTypeSetting;
                navigationController.PushViewController(gestureVc, true);
            }
        }
    }
}