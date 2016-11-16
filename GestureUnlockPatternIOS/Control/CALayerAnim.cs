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
        /// <summary> 摇动
        /// </summary>
        public void Shake()
        {
            CAKeyFrameAnimation kfa = CAKeyFrameAnimation.FromKeyPath(@"transform.translation.x");
            nfloat s = 5;
            //kfa.Values = "@[@(-s),@(0),@(s),@(0),@(-s),@(0),@(s),@(0)]";
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