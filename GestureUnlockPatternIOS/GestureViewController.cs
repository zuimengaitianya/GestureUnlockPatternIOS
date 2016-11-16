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
        /// <summary>���谴ť
        /// </summary>
        public UIButton resetBtn;
        /// <summary>��ʾLabel
        /// </summary>
        public PCLockLabel msgLabel;
        /// <summary>��������
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
                // ��������մ�ĵ�һ������
                PCCircleViewConst.ResetGesturePassWord();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = PCCircleViewConst.CircleViewBackgroundColor;
            // 1.������ͬ����������
            SetupSameUI();
            // 2.���治ͬ����������
            SetupDifferentUI(); 
        }

        /// <summary> ����UIBarButtonItem
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

        /// <summary>���治ͬ����������
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

        /// <summary> ������ͬ����������
        /// </summary>
        void SetupSameUI()
        {
            // �����������ұ߰�ť
            NavigationItem.RightBarButtonItem = ItemWithTitle("����",  (int)buttonTag.buttonTagReset);
            // ��������
            PCCircleView lockView = new PCCircleView();
            this.lockView = lockView;                                                   
            View.AddSubview(this.lockView);

            PCLockLabel msgLabelNew = new PCLockLabel(new CGRect(0, 0, PCCircleViewConst.kScreenW, 14));

            msgLabelNew.Center = new CGPoint(PCCircleViewConst.kScreenW / 2, lockView.Frame.GetMinY() - 30);
            this.msgLabel = msgLabelNew;
            View.AddSubview(msgLabel);

            lockView.Delegate = new MyCircleViewDelegate(NavigationController, resetBtn, msgLabel, InfoViewSelectedSubviewsSameAsCircleView); 
        }

        /// <summary>���������������
        /// </summary>
        void SetupSubViewsSettingVc()
        {
            lockView.Type = CircleViewType.CircleViewTypeSetting;
            Title = @"������������";
            msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextBeforeSet);
            PCCircleInfoView infoView = new PCCircleInfoView();
            infoView.Frame = new CGRect(0, 0, PCCircleViewConst.CircleRadius * 2 * 0.6, PCCircleViewConst.CircleRadius * 2 * 0.6);
            infoView.Center=new CGPoint(PCCircleViewConst.kScreenW / 2, msgLabel.Frame.GetMinY() - infoView.Frame.Height / 2 - 10);
            this.infoView = infoView;
            View.AddSubview(infoView);
        }

        /// <summary> ��½�����������
        /// </summary>
        void SetupSubViewsLoginVc()
        {
            lockView.Type = CircleViewType.CircleViewTypeLogin;
            // ͷ��
            UIImageView imageView = new UIImageView();
            imageView.Frame =new  CGRect(0, 0, 65, 65);
            imageView.Center = new CGPoint(PCCircleViewConst.kScreenW / 2, PCCircleViewConst.kScreenH / 5);
            imageView.Image = UIImage.FromFile("head");
            View.AddSubview(imageView);
            //��������������
            msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextLoginGesture);
            //������������
            UIButton leftBtn = new UIButton(UIButtonType.Custom);
            CreatButton(leftBtn, new CGRect(PCCircleViewConst.CircleViewEdgeMargin + 20, PCCircleViewConst.kScreenH - 60, PCCircleViewConst.kScreenW / 2, 20), "������������", UIControlContentHorizontalAlignment.Left, (int)buttonTag.buttonTagManager);
            // ��¼�����˻�
            UIButton rightBtn = new UIButton(UIButtonType.Custom);
            CreatButton(rightBtn, new CGRect(PCCircleViewConst.kScreenW / 2 - PCCircleViewConst.CircleViewEdgeMargin - 20, PCCircleViewConst.kScreenH - 60, PCCircleViewConst.kScreenW / 2, 20), "��¼�����˻�", UIControlContentHorizontalAlignment.Right, (int)buttonTag.buttonTagForget);
        }

        /// <summary>����UIButton
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
                    //(��������谴ť)
                    case (int)buttonTag.buttonTagReset:
                        // 1.���ذ�ť
                        resetBtn.Hidden = true;
                        // 2.infoViewȡ��ѡ��
                        InfoViewDeselectedSubviews();
                        // 3.msgLabel��ʾ���ָ�λ
                        msgLabel.ShowNormalMsg(PCCircleViewConst.gestureTextBeforeSet);
                        // 4.���֮ǰ�洢������
                        PCCircleViewConst.ResetGesturePassWord();
                        break;
                    //����˹����������밴ť
                    case (int)buttonTag.buttonTagManager:
                        break;
                    //����˵�¼�����˻���ť
                    case (int)buttonTag.buttonTagForget:
                        break;
                    default:
                        break;
                }
            }
        }

        #region infoViewչʾ����
        /// <summary>��infoView��Ӧ��ťѡ��
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

        /// <summary> ��infoView��Ӧ��ťȡ��ѡ��
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

        /// <summary>���߸�������4��ʱ��֪ͨ����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">���ƽ��</param>
        public override void CircleViewConnectCirclesLessThanNeedWithGesture(PCCircleView view, CircleViewType type, string gesture)
        {
            //��ȡ���Ʊ���ĵ�һ������
            string gestureOne = PCCircleViewConst.GetGesturePassWord();
            // ���Ƿ���ڵ�һ������
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

        /// <summary>���߸������ڻ����4������ȡ����һ����������ʱ֪ͨ����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">��һ���α��������</param>
        public override void CircleViewDidCompleteSetFirstGesture(PCCircleView view, CircleViewType type, string gesture)
        {
            msgLabel.ShowWarnMsg(PCCircleViewConst.gestureTextDrawAgain);
            if (action != null)
            {
                // infoViewչʾ��Ӧѡ�е�Բ
                action(view);
            }
        }

        /// <summary>��½������֤���������������ʱ�Ĵ�����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">��½ʱ����������</param>
        /// <param name="result"></param>
        public override void CircleViewDidCompleteSetSecondGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {
            //��������ƥ��
            if (equal)
            {
                msgLabel.ShowWarnMsg(PCCircleViewConst.gestureTextSetSuccess);
                navigationController.PopToRootViewController(true);
            }
            else
            {
                //�������Ʋ�ƥ��
                msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextDrawAgainError);
                resetBtn.Hidden = false;
            }
        }

        /// <summary>��½������֤���������������ʱ�Ĵ�����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">��½ʱ����������</param>
        /// <param name="result"></param>
        public override void CircleViewDidCompleteLoginGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {
            // ��ʱ��type��������� Login or verify
            if (type == CircleViewType.CircleViewTypeLogin)
            {
                if (equal)
                {
                    //��½�ɹ�
                    navigationController.PopToRootViewController(true);
                }
                else
                {
                    //�������
                    msgLabel.ShowWarnMsgAndShake(PCCircleViewConst.gestureTextGestureVerifyError);
                }
            }
            else if (type == CircleViewType.CircleViewTypeVerify)
            {
                if (equal)
                {
                    //"��֤�ɹ�����ת���������ƽ���"
                }
                else
                {
                    //ԭ���������������
                }
            }
        }
    }

    public enum GestureViewControllerType
    {
        /// <summary>��������
        /// </summary>
        GestureViewControllerTypeSetting = 1,
        /// <summary>��½����
        /// </summary>
        GestureViewControllerTypeLogin
    }

    public enum buttonTag
    {
        /// <summary>���谴ť
        /// </summary>
        buttonTagReset = 1,
        /// <summary>���ƹ���ť
        /// </summary>
        buttonTagManager,
        /// <summary>�������밴ť
        /// </summary>
        buttonTagForget
    }
}