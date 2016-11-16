using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    public class PCCircle : UIView
    {
        private CircleState state;
        /// <summary>������״̬
        /// </summary>
        public CircleState State {
            get { return state; }
            set
            {
                state = value;
                SetNeedsDisplay();//�ػ����
            }
        }
        private nfloat angle;
        /// <summary> �Ƕ�
        /// </summary>
        public nfloat Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                SetNeedsDisplay();//�ػ����
            }
        }
        /// <summary>����
        /// </summary>
        public CircleType Type { get; set; }
        //�Ƿ��м�ͷ default is true
        public bool Arrow { get; set; } 
      
        /// <summary> �⻷��ɫ
        /// </summary>
        private UIColor outCircleColor
        {
            get {
                UIColor color;
                switch (this.State)
                {
                    case CircleState.CircleStateNormal:
                        color = PCCircleViewConst.CircleStateNormalOutsideColor;
                        break;
                    case CircleState.CircleStateSelected:
                        color = PCCircleViewConst.CircleStateSelectedOutsideColor;
                        break;
                    case CircleState.CircleStateError:
                        color = PCCircleViewConst.CircleStateErrorOutsideColor;
                        break;
                    case CircleState.CircleStateLastOneSelected:
                        color = PCCircleViewConst.CircleStateSelectedOutsideColor;
                        break;
                    case CircleState.CircleStateLastOneError:
                        color = PCCircleViewConst.CircleStateErrorOutsideColor;
                        break;
                    default:
                        color = PCCircleViewConst.CircleStateNormalOutsideColor;
                        break;
                }
                return color;
            }
        }
        /// <summary> ʵ��Բ��ɫ
        /// </summary>
        private UIColor inCircleColor
        {
            get
            {
                UIColor color= UIColor.Clear;
                switch (this.State)
                {
                    case CircleState.CircleStateNormal:
                        color = PCCircleViewConst.CircleStateNormalInsideColor;
                        break;
                    case CircleState.CircleStateSelected:
                        color = PCCircleViewConst.CircleStateSelectedInsideColor;
                        break;
                    case CircleState.CircleStateError:
                        color = PCCircleViewConst.CircleStateErrorInsideColor;
                        break;
                    case CircleState.CircleStateLastOneSelected:
                        color = PCCircleViewConst.CircleStateSelectedInsideColor;
                        break;
                    case CircleState.CircleStateLastOneError:
                        color = PCCircleViewConst.CircleStateErrorInsideColor;
                        break;
                    default:
                        color = PCCircleViewConst.CircleStateNormalInsideColor;
                        break;
                }
                return color;
            }
        }
        /// <summary> ��������ɫ
        /// </summary>
        private UIColor trangleColor
        {
            get
            {
                UIColor color=UIColor.Clear;
                switch (this.State)
                {
                    case CircleState.CircleStateNormal:
                        color = PCCircleViewConst.CircleStateNormalTrangleColor;
                        break;
                    case CircleState.CircleStateSelected:
                        color = PCCircleViewConst.CircleStateSelectedTrangleColor;
                        break;
                    case CircleState.CircleStateError:
                        color = PCCircleViewConst.CircleStateErrorTrangleColor;
                        break;
                    case CircleState.CircleStateLastOneSelected:
                        color = PCCircleViewConst.CircleStateNormalTrangleColor;
                        break;
                    case CircleState.CircleStateLastOneError:
                        color = PCCircleViewConst.CircleStateNormalTrangleColor;
                        break;
                    default:
                        color = PCCircleViewConst.CircleStateNormalTrangleColor;
                        break;
                }
                return color;
            }
        }

        public PCCircle() : base()
        {
            //�Ƿ��м�ͷ default is true
            Arrow = true;
            this.BackgroundColor = PCCircleViewConst.CircleBackgroundColor;
        }

        public PCCircle(NSCoder aDecoder) : base(aDecoder)
        {
            //�Ƿ��м�ͷ default is true
            Arrow = true;
            this.BackgroundColor = PCCircleViewConst.CircleBackgroundColor;
        }

        public override void Draw(CGRect rect)
        {
            CGContext ctx = UIGraphics.GetCurrentContext();
            nfloat radio = 0;
            CGRect circleRect = new CGRect(PCCircleViewConst.CircleEdgeWidth, PCCircleViewConst.CircleEdgeWidth, rect.Size.Width - 2 * PCCircleViewConst.CircleEdgeWidth, rect.Size.Height - 2 * PCCircleViewConst.CircleEdgeWidth);
            if (this.Type == CircleType.CircleTypeGesture)
            {
                radio = PCCircleViewConst.CircleRadio;
            }
            else if (this.Type == CircleType.CircleTypeInfo)
            {
                radio = 1;
            }

            // ��������ת
            this.TransFormCtx(ctx, rect);
            // ��Բ��
            this.DrawEmptyCircleWithContext(ctx, circleRect, outCircleColor);
            // ��ʵ��Բ
            this.DrawSolidCircleWithContext(ctx, rect, radio, inCircleColor);
            if (Arrow)
            {
                // �������μ�ͷ
                this.DrawTrangleWithContext(ctx, new CGPoint(rect.Size.Width / 2, 10), PCCircleViewConst.kTrangleLength, trangleColor);
            }
        }
      
        /// <summary> ��������ת
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="rect"></param>
        void TransFormCtx(CGContext ctx, CGRect rect)
        {
            nfloat translateXY = rect.Size.Width * .5f;
            //ƽ�� 
            ctx.TranslateCTM(translateXY, translateXY);
            ctx.RotateCTM(Angle);
            //��ƽ�ƻ���
            ctx.TranslateCTM(-translateXY, -translateXY);
        }

        /// <summary> ����Բ��
        /// </summary>
        /// <param name="ctx"> ͼ��������</param>
        /// <param name="rect">�滭��Χ</param>
        /// <param name="outCircleColor">������ɫ</param>
        void DrawEmptyCircleWithContext(CGContext ctx, CGRect rect,UIColor color)
        {
            CGPath circlePath = new CGPath();
            circlePath.AddEllipseInRect(rect);
            ctx.AddPath(circlePath);
            color.SetColor();
            ctx.SetLineWidth(PCCircleViewConst.CircleEdgeWidth);
            ctx.StrokePath();
            circlePath.Dispose();
        }
        /// <summary>��ʵ��Բ
        /// </summary>
        /// <param name="ctx">ͼ��������</param>
        /// <param name="rect">�滭��Χ</param>
        /// <param name="radio">ռ��Բ����</param>
        /// <param name="inCircleColor">������ɫ</param>
        void DrawSolidCircleWithContext(CGContext ctx, CGRect rect, nfloat radio, UIColor color)
        {
            CGPath circlePath = new CGPath();
            circlePath.AddEllipseInRect(new CGRect(rect.Size.Width / 2 * (1 - radio) + PCCircleViewConst.CircleEdgeWidth, rect.Size.Height / 2 * (1 - radio) + PCCircleViewConst.CircleEdgeWidth, rect.Size.Width * radio - PCCircleViewConst.CircleEdgeWidth * 2, rect.Size.Height * radio - PCCircleViewConst.CircleEdgeWidth * 2));
            color.SetColor();
            ctx.AddPath(circlePath);
            ctx.FillPath();
            circlePath.Dispose();
        }

        /// <summary>��������
        /// </summary>
        /// <param name="ctx">ͼ��������</param>
        /// <param name="point">����</param>
        /// <param name="length">�߳�</param>
        /// <param name="color">������ɫ</param>
        void DrawTrangleWithContext(CGContext ctx, CGPoint point, nfloat length, UIColor color)
        {
            CGPath trianglePathM = new CGPath();
            trianglePathM.MoveToPoint(point.X, point.Y);
            trianglePathM.AddLineToPoint(point.X - length / 2, point.Y + length / 2);
            trianglePathM.AddLineToPoint(point.X + length / 2, point.Y + length / 2);
            ctx.AddPath(trianglePathM);
            color.SetColor();
            ctx.FillPath();
            trianglePathM.Dispose();
        }
    }

    /// <summary>����Բ�ĸ���״̬
    /// </summary>
    public enum CircleState
    {
        /// <summary>��ͨ״̬�µ�Բ
        /// </summary>
        CircleStateNormal = 1,
        /// <summary>ѡ��״̬�µ�Բ
        /// </summary>
        CircleStateSelected,
        /// <summary>����״̬��ѡ�е�Բ
        /// </summary>
        CircleStateError,
        /// <summary>ѡ��״̬�µ����һ��Բ������״̬��ѡ�е�û������ͼ�꣩
        /// </summary>
        CircleStateLastOneSelected,
        /// <summary>����״̬��ѡ�е����һ��Բ������״̬�´����ѡ�е�û������ͼ�꣩
        /// </summary>
        CircleStateLastOneError
    }

    /// <summary>����Բ����;����
    /// </summary>
    public enum CircleType
    {
        /// <summary>����Բ
        /// </summary>
        CircleTypeInfo = 1,
        /// <summary>����Բ
        /// </summary>
        CircleTypeGesture
    }
}