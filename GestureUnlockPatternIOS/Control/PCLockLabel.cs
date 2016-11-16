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
            //视图初始化
            ViewPrepare();
        }

        public PCLockLabel(NSCoder aDecoder) : base(aDecoder)
        {
            //视图初始化
            ViewPrepare();
        }


        public void ViewPrepare()
        {
            Font = UIFont.SystemFontOfSize(14.0f);
            TextAlignment = UITextAlignment.Center;
        }

        /// <summary> 普通提示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowNormalMsg(string msg)
        {
            Text = msg;
            TextColor = PCCircleViewConst.textColorNormalState;
        }
        /// <summary>警示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowWarnMsg(string msg)
        {
            Text = msg;
            TextColor = PCCircleViewConst.textColorWarningState;
        }
        /// <summary>警示信息(shake)
        /// </summary>
        /// <param name="msg"></param>
        public void ShowWarnMsgAndShake(string msg)
        {
            Text = msg;
            TextColor = PCCircleViewConst.textColorWarningState;

            //添加一个shake动画
            Shake();
        }

        /// <summary> 摇动
        /// </summary>
        private void Shake()
        {
            CAKeyFrameAnimation kfa = CAKeyFrameAnimation.FromKeyPath(@"transform.translation.x");
            nfloat s = 5;
            NSObject[] ns = new NSObject[1];
            ns[0]=FromObject("@[@(-s),@(0),@(s),@(0),@(-s),@(0),@(s),@(0)]");
            kfa.Values = ns;
            //时长
            kfa.Duration = 0.3f;
            ////重复
            kfa.RepeatCount = 2;
            ////移除
            kfa.RemovedOnCompletion = true;
            this.Layer.AddAnimation(kfa, @"shake");
        }
    }
}