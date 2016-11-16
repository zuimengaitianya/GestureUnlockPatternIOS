using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    /// <summary>������ͼ
    /// </summary>
    public class PCCircleInfoView: UIView
    {
        public PCCircleInfoView():base()
        {
            // ������ͼ׼��
            LockViewPrepare();
        }

        public PCCircleInfoView(NSCoder aDecoder) : base(aDecoder)
        {
            // ������ͼ׼��
            LockViewPrepare();
        }

        /// <summary>������ͼ׼��
        /// </summary>
        private void LockViewPrepare()
        {
            BackgroundColor = PCCircleViewConst.CircleBackgroundColor;
            for (int i = 0; i < 9; i++)
            {
                PCCircle circle = new PCCircle();
                circle.Type = CircleType.CircleTypeInfo;
                AddSubview(circle);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            nfloat itemViewWH = PCCircleViewConst.CircleInfoRadius * 2;
            nfloat marginValue = (Frame.Size.Width - 3 * itemViewWH) / 3.0f;

            for (int index = 0; index < Subviews.Length; index++)
            {
                int row = index % 3;
                int col = index / 3;
                nfloat x = marginValue * row + row * itemViewWH + marginValue / 2;
                nfloat y = marginValue * col + col * itemViewWH + marginValue / 2;
                CGRect frame = new CGRect(x, y, itemViewWH, itemViewWH);
                // ����tag -> �����¼�ĵ�Ԫ
                Subviews[index].Tag = index + 1;
                Subviews[index].Frame = frame;
            }
        }
    }
}