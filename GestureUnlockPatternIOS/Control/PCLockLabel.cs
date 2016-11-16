using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    public class PCLockLabel : UILabel
    {
        public PCLockLabel(CGRect frame):base(frame)
        {
            //��ͼ��ʼ��
            ViewPrepare();
        }

        public PCLockLabel(NSCoder aDecoder) : base(aDecoder)
        {
            //��ͼ��ʼ��
            ViewPrepare();
        }


        public void ViewPrepare()
        {
            Font = UIFont.SystemFontOfSize(14.0f);
            TextAlignment = UITextAlignment.Center;
        }

        /// <summary> ��ͨ��ʾ��Ϣ
        /// </summary>
        /// <param name="msg"></param>
        public void ShowNormalMsg(string msg)
        {
            Text = msg;
            TextColor = PCCircleViewConst.textColorNormalState;
        }
        /// <summary>��ʾ��Ϣ
        /// </summary>
        /// <param name="msg"></param>
        public void ShowWarnMsg(string msg)
        {
            Text = msg;
            TextColor = PCCircleViewConst.textColorWarningState;
        }
        /// <summary>��ʾ��Ϣ(shake)
        /// </summary>
        /// <param name="msg"></param>
        public void ShowWarnMsgAndShake(string msg)
        {
            Text = msg;
            TextColor = PCCircleViewConst.textColorWarningState;

            //���һ��shake����
            Shake();
        }

        /// <summary> ҡ��
        /// </summary>
        private void Shake()
        {
            CAKeyFrameAnimation kfa = CAKeyFrameAnimation.FromKeyPath(@"transform.translation.x");
            nfloat s = 5;
            NSObject[] ns = new NSObject[1];
            ns[0]=FromObject("@[@(-s),@(0),@(s),@(0),@(-s),@(0),@(s),@(0)]");
            kfa.Values = ns;
            //ʱ��
            kfa.Duration = 0.3f;
            ////�ظ�
            kfa.RepeatCount = 2;
            ////�Ƴ�
            kfa.RemovedOnCompletion = true;
            this.Layer.AddAnimation(kfa, @"shake");
        }
    }
}