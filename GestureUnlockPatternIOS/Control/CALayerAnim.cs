using CoreAnimation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    public class CALayerAnim 
    {
        /// <summary> ҡ��
        /// </summary>
        public void Shake()
        {
            CAKeyFrameAnimation kfa = CAKeyFrameAnimation.FromKeyPath(@"transform.translation.x");
            nfloat s = 5;
            //kfa.Values = "@[@(-s),@(0),@(s),@(0),@(-s),@(0),@(s),@(0)]";
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