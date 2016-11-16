using CoreGraphics;
using Foundation;
using GestureUnlockPatternIOS.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS
{
    public class GestureVerifyViewController : UIViewController
    {
        public bool isToSetNewGesture { get; set; }
        /// <summary>文字提示Label
        /// </summary>
        public PCLockLabel msgLabel;

        public GestureVerifyViewController() : base()
        {
            this.View.BackgroundColor = PCCircleViewConst.CircleViewBackgroundColor;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(false, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //在加载视图后进行任何其他设置
            Title = @"验证手势解锁";
            PCCircleView lockView = new PCCircleView();
            //lockView.Delegate = this;
            lockView.Type = CircleViewType.CircleViewTypeVerify;
            View.AddSubview(lockView);

            msgLabel = new PCLockLabel(new CGRect(0, 0, PCCircleViewConst.kScreenW, 14));
            msgLabel.Center = new CGPoint(PCCircleViewConst.kScreenW / 2, lockView.Frame.GetMinY() - 30);
            msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextOldGesture);
            View.AddSubview(msgLabel);
        }

        public void CircleViewDidCompleteLoginGesture(CircleViewType type, NSString gesture,bool equal)
        {
            if (type == CircleViewType.CircleViewTypeVerify)
            {
                //验证成功
                if (equal)
                {
                    if (isToSetNewGesture)
                    {
                        GestureViewController gestureVc = new GestureViewController();
                        gestureVc.type = GestureViewControllerType.GestureViewControllerTypeSetting;
                        NavigationController.PushViewController(gestureVc, true);
                    }
                    else
                    {
                        NavigationController.PopToRootViewController(true);
                    }
                }
                else
                {
                    //密码错误！
                    msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextGestureVerifyError);
                }
            }
        }
    }
}