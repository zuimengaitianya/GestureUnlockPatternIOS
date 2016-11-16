using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    public class PCCircle : UIView
    {
        private CircleState state;
        /// <summary>所处的状态
        /// </summary>
        public CircleState State {
            get { return state; }
            set
            {
                state = value;
                SetNeedsDisplay();//重绘操作
            }
        }
        private nfloat angle;
        /// <summary> 角度
        /// </summary>
        public nfloat Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                SetNeedsDisplay();//重绘操作
            }
        }
        /// <summary>类型
        /// </summary>
        public CircleType Type { get; set; }
        //是否有箭头 default is true
        public bool Arrow { get; set; } 
      
        /// <summary> 外环颜色
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
        /// <summary> 实心圆颜色
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
        /// <summary> 三角形颜色
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
            //是否有箭头 default is true
            Arrow = true;
            this.BackgroundColor = PCCircleViewConst.CircleBackgroundColor;
        }

        public PCCircle(NSCoder aDecoder) : base(aDecoder)
        {
            //是否有箭头 default is true
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

            // 上下文旋转
            this.TransFormCtx(ctx, rect);
            // 画圆环
            this.DrawEmptyCircleWithContext(ctx, circleRect, outCircleColor);
            // 画实心圆
            this.DrawSolidCircleWithContext(ctx, rect, radio, inCircleColor);
            if (Arrow)
            {
                // 画三角形箭头
                this.DrawTrangleWithContext(ctx, new CGPoint(rect.Size.Width / 2, 10), PCCircleViewConst.kTrangleLength, trangleColor);
            }
        }
      
        /// <summary> 上下文旋转
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="rect"></param>
        void TransFormCtx(CGContext ctx, CGRect rect)
        {
            nfloat translateXY = rect.Size.Width * .5f;
            //平移 
            ctx.TranslateCTM(translateXY, translateXY);
            ctx.RotateCTM(Angle);
            //再平移回来
            ctx.TranslateCTM(-translateXY, -translateXY);
        }

        /// <summary> 画外圆环
        /// </summary>
        /// <param name="ctx"> 图形上下文</param>
        /// <param name="rect">绘画范围</param>
        /// <param name="outCircleColor">绘制颜色</param>
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
        /// <summary>画实心圆
        /// </summary>
        /// <param name="ctx">图形上下文</param>
        /// <param name="rect">绘画范围</param>
        /// <param name="radio">占大圆比例</param>
        /// <param name="inCircleColor">绘制颜色</param>
        void DrawSolidCircleWithContext(CGContext ctx, CGRect rect, nfloat radio, UIColor color)
        {
            CGPath circlePath = new CGPath();
            circlePath.AddEllipseInRect(new CGRect(rect.Size.Width / 2 * (1 - radio) + PCCircleViewConst.CircleEdgeWidth, rect.Size.Height / 2 * (1 - radio) + PCCircleViewConst.CircleEdgeWidth, rect.Size.Width * radio - PCCircleViewConst.CircleEdgeWidth * 2, rect.Size.Height * radio - PCCircleViewConst.CircleEdgeWidth * 2));
            color.SetColor();
            ctx.AddPath(circlePath);
            ctx.FillPath();
            circlePath.Dispose();
        }

        /// <summary>画三角形
        /// </summary>
        /// <param name="ctx">图形上下文</param>
        /// <param name="point">顶点</param>
        /// <param name="length">边长</param>
        /// <param name="color">绘制颜色</param>
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

    /// <summary>单个圆的各种状态
    /// </summary>
    public enum CircleState
    {
        /// <summary>普通状态下的圆
        /// </summary>
        CircleStateNormal = 1,
        /// <summary>选中状态下的圆
        /// </summary>
        CircleStateSelected,
        /// <summary>错误状态下选中的圆
        /// </summary>
        CircleStateError,
        /// <summary>选中状态下的最后一个圆（这种状态下选中的没有三角图标）
        /// </summary>
        CircleStateLastOneSelected,
        /// <summary>错误状态下选中的最后一个圆（这种状态下错误的选中的没有三角图标）
        /// </summary>
        CircleStateLastOneError
    }

    /// <summary>单个圆的用途类型
    /// </summary>
    public enum CircleType
    {
        /// <summary>导航圆
        /// </summary>
        CircleTypeInfo = 1,
        /// <summary>手势圆
        /// </summary>
        CircleTypeGesture
    }
}