using CoreGraphics;
using Foundation;
using GestureUnlockPatternIOS.Control;
using GestureUnlockPatternIOS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS
{
    public class GestureViewController: UIViewController, ICircleViewDelegate
    {
        public GestureViewControllerType type { get; set; }
        /// <summary>重设按钮
        /// </summary>
        public UIButton resetBtn;
        /// <summary>提示Label
        /// </summary>
        public PCLockLabel msgLabel;
        /// <summary>解锁界面
        /// </summary>
        public PCCircleView lockView;

        public PCCircleInfoView infoView;

        public GestureViewController():base()
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (type == GestureViewControllerType.GestureViewControllerTypeLogin)
            {
                NavigationController.SetNavigationBarHidden(true, animated);
            }
            else
            {
                // 进来先清空存的第一个密码
                PCCircleViewConst.ResetGesturePassWord();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = PCCircleViewConst.CircleViewBackgroundColor;
            // 1.界面相同部分生成器
            SetupSameUI();
            // 2.界面不同部分生成器
            SetupDifferentUI(); 
        }

        /// <summary> 创建UIBarButtonItem
        /// </summary>
        /// <param name="title"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public UIBarButtonItem ItemWithTitle(string title,int tag)
        { 
            UIButton button = new UIButton(UIButtonType.Custom);
            button.SetTitle(title, UIControlState.Normal);
            button.TouchUpInside += Btn_TouchUpInside;
            button.Frame = new CGRect(new CGPoint(0, 0), new CGSize(100, 20));
            button.SetTitleColor(UIColor.Black, UIControlState.Normal);
            button.TitleLabel.Font = UIFont.SystemFontOfSize(17);
            button.Tag = tag;
            button.Hidden = true;
            resetBtn = button;
            return new UIBarButtonItem(button);
        }

        /// <summary>界面不同部分生成器
        /// </summary>
        void SetupDifferentUI()
        {
            switch (type)
            {
                case GestureViewControllerType.GestureViewControllerTypeSetting:
                    SetupSubViewsSettingVc();
                    break;
                case GestureViewControllerType.GestureViewControllerTypeLogin:
                    SetupSubViewsLoginVc();
                    break;
                default:
                    break;
            }
        }

        /// <summary> 界面相同部分生成器
        /// </summary>
        void SetupSameUI()
        {
            // 创建导航栏右边按钮
            NavigationItem.RightBarButtonItem = ItemWithTitle("重设",  (int)buttonTag.buttonTagReset);
            // 解锁界面
            PCCircleView lockView = new PCCircleView();
            this.lockView = lockView;                                                   
            View.AddSubview(this.lockView);

            PCLockLabel msgLabelNew = new PCLockLabel(new CGRect(0, 0, PCCircleViewConst.kScreenW, 14));

            msgLabelNew.Center = new CGPoint(PCCircleViewConst.kScreenW / 2, lockView.Frame.GetMinY() - 30);
            this.msgLabel = msgLabelNew;
            View.AddSubview(msgLabel);

            lockView.Delegate = new MyCircleViewDelegate(NavigationController, resetBtn, msgLabel, InfoViewSelectedSubviewsSameAsCircleView); 
        }

        /// <summary>设置手势密码界面
        /// </summary>
        void SetupSubViewsSettingVc()
        {
            lockView.Type = CircleViewType.CircleViewTypeSetting;
            Title = @"设置手势密码";
            msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextBeforeSet);
            PCCircleInfoView infoView = new PCCircleInfoView();
            infoView.Frame = new CGRect(0, 0, PCCircleViewConst.CircleRadius * 2 * 0.6, PCCircleViewConst.CircleRadius * 2 * 0.6);
            infoView.Center=new CGPoint(PCCircleViewConst.kScreenW / 2, msgLabel.Frame.GetMinY() - infoView.Frame.Height / 2 - 10);
            this.infoView = infoView;
            View.AddSubview(infoView);
        }

        /// <summary> 登陆手势密码界面
        /// </summary>
        void SetupSubViewsLoginVc()
        {
            lockView.Type = CircleViewType.CircleViewTypeLogin;
            // 头像
            UIImageView imageView = new UIImageView();
            imageView.Frame =new  CGRect(0, 0, 65, 65);
            imageView.Center = new CGPoint(PCCircleViewConst.kScreenW / 2, PCCircleViewConst.kScreenH / 5);
            imageView.Image = UIImage.FromFile("head");
            View.AddSubview(imageView);
            //请输入手势密码
            msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextLoginGesture);
            //管理手势密码
            UIButton leftBtn = new UIButton(UIButtonType.Custom);
            CreatButton(leftBtn, new CGRect(PCCircleViewConst.CircleViewEdgeMargin + 20, PCCircleViewConst.kScreenH - 60, PCCircleViewConst.kScreenW / 2, 20), "管理手势密码", UIControlContentHorizontalAlignment.Left, (int)buttonTag.buttonTagManager);
            // 登录其他账户
            UIButton rightBtn = new UIButton(UIButtonType.Custom);
            CreatButton(rightBtn, new CGRect(PCCircleViewConst.kScreenW / 2 - PCCircleViewConst.CircleViewEdgeMargin - 20, PCCircleViewConst.kScreenH - 60, PCCircleViewConst.kScreenW / 2, 20), "登录其他账户", UIControlContentHorizontalAlignment.Right, (int)buttonTag.buttonTagForget);
        }

        /// <summary>创建UIButton
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="frame"></param>
        /// <param name="title"></param>
        /// <param name="alignment"></param>
        /// <param name="tag"></param>
        public void CreatButton(UIButton btn, CGRect frame, string title, UIControlContentHorizontalAlignment alignment, int tag)
        {
            btn.Frame = frame;
            btn.Tag = tag;
            btn.SetTitle(title, UIControlState.Normal);
            btn.SetTitleColor(UIColor.White, UIControlState.Normal);
            btn.HorizontalAlignment = alignment;
            btn.TitleLabel.Font = UIFont.SystemFontOfSize(14.0f);
            btn.TouchUpInside += Btn_TouchUpInside;
            View.AddSubview(btn);
        }

        private void Btn_TouchUpInside(object sender, EventArgs e)
        {
           var btn= sender as UIButton;
            if (btn != null)
            {
                switch (btn.Tag)
                {
                    //(点击了重设按钮)
                    case (int)buttonTag.buttonTagReset:
                        // 1.隐藏按钮
                        resetBtn.Hidden = true;
                        // 2.infoView取消选中
                        InfoViewDeselectedSubviews();
                        // 3.msgLabel提示文字复位
                        msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextBeforeSet);
                        // 4.清除之前存储的密码
                        PCCircleViewConst.ResetGesturePassWord();
                        break;
                    //点击了管理手势密码按钮
                    case (int)buttonTag.buttonTagManager:
                        break;
                    //点击了登录其他账户按钮
                    case (int)buttonTag.buttonTagForget:
                        break;
                    default:
                        break;
                }
            }
        }

        #region infoView展示方法
        /// <summary>让infoView对应按钮选中
        /// </summary>
        /// <param name="circleView"></param>
        void InfoViewSelectedSubviewsSameAsCircleView(PCCircleView circleView)
        {
            foreach (PCCircle circle in circleView.Subviews)
            {
                if (circle.State == CircleState.CircleStateSelected || circle.State == CircleState.CircleStateLastOneSelected)
                {
                    foreach (PCCircle infoCircle in infoView.Subviews)
                    {
                        if (infoCircle.Tag == circle.Tag)
                        {
                            infoCircle.State = CircleState.CircleStateSelected;
                        }
                    }
                }
            }
        }

        /// <summary> 让infoView对应按钮取消选中
        /// </summary>
        void InfoViewDeselectedSubviews()
        {
            foreach (PCCircle pcCircle in infoView.Subviews)
            {
                pcCircle.State = CircleState.CircleStateNormal;
            }
        } 
        #endregion
    }

    public class MyCircleViewDelegate : CircleViewDelegate
    {
        UIButton resetBtn;
        PCLockLabel msgLabel;
        Action<PCCircleView> action;
        UINavigationController navigationController;
        public MyCircleViewDelegate(UINavigationController navigationController, UIButton resetBtn, PCLockLabel msgLabel, Action<PCCircleView> action)
        {
            this.navigationController = navigationController;
            this.resetBtn = resetBtn;
            this.msgLabel = msgLabel;
            this.action = action;
        }

        /// <summary>连线个数少于4个时，通知代理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">手势结果</param>
        public override void CircleViewConnectCirclesLessThanNeedWithGesture(PCCircleView view, CircleViewType type, string gesture)
        {
            //获取手势保存的第一个密码
            string gestureOne = PCCircleViewConst.GetGesturePassWord();
            // 看是否存在第一个密码
            if (gestureOne.Length >0)
            {
                resetBtn.Hidden = false;
                msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextDrawAgainError);
            }
            else
            {
                msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextConnectLess);
            }
        }

        /// <summary>连线个数多于或等于4个，获取到第一个手势密码时通知代理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">第一个次保存的密码</param>
        public override void CircleViewDidCompleteSetFirstGesture(PCCircleView view, CircleViewType type, string gesture)
        {
            msgLabel.ShowWarnMsg(PCCircleViewConst.gestureTextDrawAgain);
            if (action != null)
            {
                // infoView展示对应选中的圆
                action(view);
            }
        }

        /// <summary>登陆或者验证手势密码输入完成时的代理方法
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">登陆时的手势密码</param>
        /// <param name="result"></param>
        public override void CircleViewDidCompleteSetSecondGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {
            //两次手势匹配
            if (equal)
            {
                msgLabel.ShowWarnMsg(PCCircleViewConst.gestureTextSetSuccess);
                navigationController.PopToRootViewController(true);
            }
            else
            {
                //两次手势不匹配
                msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextDrawAgainError);
                resetBtn.Hidden = false;
            }
        }

        /// <summary>登陆或者验证手势密码输入完成时的代理方法
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">登陆时的手势密码</param>
        /// <param name="result"></param>
        public override void CircleViewDidCompleteLoginGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {
            // 此时的type有两种情况 Login or verify
            if (type == CircleViewType.CircleViewTypeLogin)
            {
                if (equal)
                {
                    //登陆成功
                    navigationController.PopToRootViewController(true);
                }
                else
                {
                    //密码错误
                    msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextGestureVerifyError);
                }
            }
            else if (type == CircleViewType.CircleViewTypeVerify)
            {
                if (equal)
                {
                    //"验证成功，跳转到设置手势界面"
                }
                else
                {
                    //原手势密码输入错误
                }
            }
        }
    }

    public enum GestureViewControllerType
    {
        /// <summary>设置手势
        /// </summary>
        GestureViewControllerTypeSetting = 1,
        /// <summary>登陆手势
        /// </summary>
        GestureViewControllerTypeLogin
    }

    public enum buttonTag
    {
        /// <summary>重设按钮
        /// </summary>
        buttonTagReset = 1,
        /// <summary>手势管理按钮
        /// </summary>
        buttonTagManager,
        /// <summary>忘记密码按钮
        /// </summary>
        buttonTagForget
    }
}