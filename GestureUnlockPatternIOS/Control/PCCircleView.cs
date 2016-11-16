using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreGraphics;
using System.Runtime.CompilerServices;
using GestureUnlockPatternIOS.Interface;

namespace GestureUnlockPatternIOS.Control
{
    /// <summary>滑动手势的九个大圆
    /// </summary>
    public class PCCircleView : UIView
    {
        public ICircleViewDelegate Delegate;//代理

        public bool Clip { get; set; }//是否剪裁 default is true
        public CircleViewType Type { get; set; }//解锁类型
        private bool arrow ;
        /// <summary>是否有箭头 default is YES
        /// </summary>
        public bool Arrow
        {
            get { return arrow; }
            set
            {
                arrow = value;
                // 遍历子控件，改变其是否有箭头
                foreach (PCCircle item in Subviews)
                {
                    item.Arrow = arrow;
                }
            }
        }

        private List<PCCircle> circleSet;
        /// <summary>选中的圆的集合
        /// </summary>
        public List<PCCircle> CircleSet
        {
            get
            {
                if (circleSet == null)
                {
                    circleSet = new List<PCCircle>();
                }
                return circleSet;
            }
            set { circleSet = value; }
        }
        CGPoint CurrentPoint { get; set; }// 当前点
        bool HasClean { get; set; }//数组清空标志

        /// <summary>初始化方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="clip">是否剪裁</param>
        /// <param name="arrow">三角形箭头</param>
        public PCCircleView(CircleViewType type, bool clip, bool arrow) : base()
        {
            // 解锁视图准备
            LockViewPrepare();

            this.Type = type;
            this.Clip = clip;
            this.Arrow = arrow;
        }
        public PCCircleView() : base()
        {
            // 解锁视图准备
            LockViewPrepare();
        }

        public PCCircleView(NSCoder aDecoder) : base(aDecoder)
        {
            // 解锁视图准备
            LockViewPrepare();
        }

        /// <summary>解锁视图准备
        /// </summary>
        public void LockViewPrepare()
        {
            this.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width - PCCircleViewConst.CircleViewEdgeMargin * 2, UIScreen.MainScreen.Bounds.Size.Width - PCCircleViewConst.CircleViewEdgeMargin * 2);
            this.Center = new CGPoint(UIScreen.MainScreen.Bounds.Size.Width / 2, PCCircleViewConst.CircleViewCenterY);
            // 默认剪裁子控件
            Clip = true;
            // 默认有箭头
            Arrow = true;
            this.BackgroundColor = PCCircleViewConst.CircleBackgroundColor;

            for (int i = 0; i < 9; i++)
            {
                PCCircle circle = new PCCircle();
                circle.Type = CircleType.CircleTypeGesture;
                circle.Arrow = Arrow;
                AddSubview(circle);
            }
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            nfloat itemViewWH = PCCircleViewConst.CircleRadius * 2;
            nfloat marginValue = (Frame.Size.Width - 3 * itemViewWH) / 3.0f;

            for (int index = 0; index < Subviews.Length; index++)
            {
                int row = index % 3;
                int col = index / 3;
                nfloat x = marginValue * row + row * itemViewWH + marginValue / 2;
                nfloat y = marginValue * col + col * itemViewWH + marginValue / 2;
                CGRect frame = new CGRect(x, y, itemViewWH, itemViewWH);
                // 设置tag -> 密码记录的单元
                Subviews[index].Tag = index + 1;
                Subviews[index].Frame = frame;
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            GestureEndResetMembers(obj);
            CurrentPoint = new CGPoint(0, 0);
            UITouch touch = (UITouch)touches.AnyObject;
            CGPoint point = touch.LocationInView(this);

            foreach (PCCircle circle in Subviews)
            {
                if (circle.Frame.Contains(point))
                {
                    circle.State = CircleState.CircleStateSelected;
                    CircleSet.Add(circle);
                }
            }
            if (CircleSet.Count > 0)
            {
                //数组中最后一个对象的处理
                CircleSetLastObjectWithState(CircleState.CircleStateLastOneSelected);
                SetNeedsDisplay();
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            CurrentPoint = new CGPoint(0, 0);
            UITouch touch = (UITouch)touches.AnyObject;
            CGPoint point = touch.LocationInView(this);

            foreach (PCCircle circle in Subviews)
            {
                if (circle.Frame.Contains(point))
                {
                    if (!CircleSet.Contains(circle))
                    {
                        CircleSet.Add(circle);
                        // move过程中的连线（包含跳跃连线的处理）
                        CalAngleAndconnectTheJumpedCircle();
                    }
                }
                else
                {
                    CurrentPoint = point;
                }
            }

            foreach (PCCircle circle in CircleSet)
            {
                circle.State = CircleState.CircleStateSelected;
                // 如果是登录或者验证原手势密码，就改为对应的状态 （加上这句没有三角箭头）
                //if (Type != CircleViewType.CircleViewTypeSetting)
                //{
                //    circle.State = CircleState.CircleStateLastOneSelected;
                //}
            }
            if (CircleSet.Count > 0)
            {
                // 数组中最后一个对象的处理
                CircleSetLastObjectWithState(CircleState.CircleStateLastOneSelected);
                SetNeedsDisplay();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            HasClean= false;
            //手势密码
            string gesture = GetGestureResultFromCircleSet(CircleSet);
            nfloat length = gesture.Length;
            if (length == 0)
            {
                return;
            }

            // 手势绘制结果处理
            switch (Type)
            {
                case CircleViewType.CircleViewTypeSetting:
                    GestureEndByTypeSettingWithGesture(gesture, length);
                    break;
                case CircleViewType.CircleViewTypeLogin:
                    GestureEndByTypeLoginWithGesture(gesture, length);
                    break;
                case CircleViewType.CircleViewTypeVerify:
                    GestureEndByTypeVerifyWithGesture(gesture, length);
                    break;
                default:
                    GestureEndByTypeSettingWithGesture(gesture, length);
                    break;
            }
            // 手势结束后是否错误回显重绘，取决于是否延时清空数组和状态复原
            ErrorToDisplay();
        }

        /// <summary>是否错误回显重绘
        /// </summary>
        public void ErrorToDisplay() {
            if (GetCircleState() == CircleState.CircleStateError || GetCircleState() == CircleState.CircleStateLastOneError)
            {
                //此处应该延时 以显示红色线条错误
                //参数说明 
                //dueTime：调用 callback 之前延迟的时间量（以毫秒为单位）。指定 Timeout.Infinite 以防止计时器开始计时。指定零 (0) 以立即启动计时器。
                //Period：调用 callback 的时间间隔（以毫秒为单位）。指定 Timeout.Infinite 可以禁用定期终止。
                System.Threading.Timer tt = new System.Threading.Timer(new System.Threading.TimerCallback(GestureEndResetMembers),obj, PCCircleViewConst.kdisplayTime, System.Threading.Timeout.Infinite);
            }
            else
            {
                GestureEndResetMembers(obj);
            }
        }

        public static object obj = new object();

        /// <summary>手势结束时的清空操作
        /// </summary>
        public void GestureEndResetMembers(object obj)
        {
            // 保证线程安全
            lock (obj)
            {
                BeginInvokeOnMainThread(() =>
                {
                    if (!HasClean)
                    {
                        // 手势完毕，选中的圆回归普通状态
                        ChangeCircleInCircleSetWithState(CircleState.CircleStateNormal);
                        // 清空数组
                        CircleSet.Clear();
                        // 清空方向
                        ResetAllCirclesDirect();
                        // 完成之后改变clean的状态
                        HasClean = true;
                    }
                });
            }
        }
        // 获取当前选中圆的状态
        CircleState GetCircleState() {
            return CircleSet.First().State;
        }
        /// <summary> 清空所有子控件的方向
        /// </summary>
        void ResetAllCirclesDirect()
        {
            foreach (PCCircle item in Subviews)
            {
                item.State = 0;
            }
        }

        /// <summary>对数组中最后一个对象的处理
        /// </summary>
        /// <param name="state"></param>
        public void CircleSetLastObjectWithState(CircleState state)
        {
            CircleSet.Last().State = state;
        }

        /// <summary> 解锁类型：设置 手势路径的处理
        /// </summary>
        /// <param name="gesture">手势密码</param>
        /// <param name="length">手势长度</param>
        public void GestureEndByTypeSettingWithGesture(string gesture, nfloat length)
        {
            var myCircleViewDelegate = Delegate as MyCircleViewDelegate;

            // 连接少于最少个数 （<4个）
            if (length < PCCircleViewConst.CircleSetCountLeast)
            {
                // 1.通知代理
                if (myCircleViewDelegate != null)
                {
                    myCircleViewDelegate.CircleViewConnectCirclesLessThanNeedWithGesture(this, Type, gesture);
                }
                // 2.改变状态为error
                ChangeCircleInCircleSetWithState(CircleState.CircleStateError);
            }
            else// 连接多于最少个数 （>=4个）
            {
                string gestureOne = PCCircleViewConst.GetGesturePassWord();
                // 接收并存储第一个密码
                if (gestureOne.Length < PCCircleViewConst.CircleSetCountLeast)
                {
                    // 记录第一次密码
                    PCCircleViewConst.SaveGesturePassWord(gesture);
                    // 通知代理
                    if (myCircleViewDelegate != null)
                    {
                        myCircleViewDelegate.CircleViewDidCompleteSetFirstGesture(this, Type, gesture);
                    }
                }
                else
                {
                    // 接受第二个密码并与第一个密码匹配，一致后存储起来  // 匹配两次手势
                    bool equal = gesture.Equals(PCCircleViewConst.GetGesturePassWord());
                    // 通知代理
                    // 通知代理
                    if (myCircleViewDelegate != null)
                    {
                        myCircleViewDelegate.CircleViewDidCompleteSetSecondGesture(this, Type, gesture,equal);
                    }

                    if (equal)
                    {
                        // 一致，存储密码
                        PCCircleViewConst.SaveGesturePassWord(gesture);
                    }
                    else
                    {
                        // 不一致，重绘回显
                        ChangeCircleInCircleSetWithState(CircleState.CircleStateError);
                    }
                }
            }
        }

        /// <summary>解锁类型：登陆 手势路径的处理
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="length"></param>
        public void GestureEndByTypeLoginWithGesture(string gesture, nfloat length)
        {
            string password = PCCircleViewConst.GetGesturePassWord();
            bool equal = gesture.Equals(password);
            // 通知代理
            var myCircleViewDelegate = Delegate as MyCircleViewDelegate;
            if (myCircleViewDelegate != null)
            {
                myCircleViewDelegate.CircleViewDidCompleteLoginGesture(this,Type,gesture,equal);
            }

            if (!equal)
            {
                // 不一致，重绘回显
                ChangeCircleInCircleSetWithState(CircleState.CircleStateError);
            }
        }

        /// <summary>解锁类型：验证 手势路径的处理
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="length"></param>
        public void GestureEndByTypeVerifyWithGesture(string gesture, nfloat length)
        {
            GestureEndByTypeLoginWithGesture(gesture , length);
        }

        /// <summary>改变选中数组CircleSet子控件状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeCircleInCircleSetWithState(CircleState state)
        {
            for (int i = 0; i < CircleSet.Count; i++)
            {
                CircleSet[i].State = state;
                // 如果是错误状态，那就将最后一个按钮特殊处理
                if (state == CircleState.CircleStateError)
                {
                    if (i == CircleSet.Count - 1)
                    {
                        CircleSet[i].State = CircleState.CircleStateLastOneError;
                    }
                }
            }
            SetNeedsDisplay();
        }

        /// <summary>将circleSet数组解析遍历，拼手势密码字符串
        /// </summary>
        /// <param name="circleSet"></param>
        /// <returns></returns>
        public string GetGestureResultFromCircleSet(List<PCCircle> CircleSet)
        {
            StringBuilder gesture = new StringBuilder();
            // 遍历取tag拼字符串
            foreach (var item in CircleSet)
            {
                gesture.Append(item.Tag);
            }
            return gesture.ToString();
        }

        /// <summary>drawRect
        /// </summary>
        /// <param name="rect"></param>
        public override void Draw(CGRect rect)
        {
            // 如果没有任何选中按钮， 直接retrun
            if (CircleSet == null || CircleSet.Count == 0)
                return;
            UIColor color=UIColor.Clear;
            if (GetCircleState() == CircleState.CircleStateError)
            {
                color = PCCircleViewConst.CircleConnectLineErrorColor;
            }
            else
            {
                color = PCCircleViewConst.CircleConnectLineNormalColor;
            }
            // 绘制图案
            ConnectCirclesInRect(rect, color);
        }

        /// <summary> 连线绘制图案(以设定颜色绘制)  将选中的圆形以color颜色链接起来
        /// </summary>
        /// <param name="rect">图形上下文</param>
        /// <param name="color">连线颜色</param>
        public void ConnectCirclesInRect(CGRect rect, UIColor color)
        {
            //获取上下文
            CGContext ctx = UIGraphics.GetCurrentContext();
            // 添加路径
            ctx.AddRect(rect);
            //是否剪裁
            ClipSubviewsWhenConnectInContext(ctx, Clip);
            //剪裁上下文
            ctx.EOClip();
            // 遍历数组中的circle
            for (int index = 0; index < CircleSet.Count; index++)
            {
                // 取出选中按钮
                PCCircle circle = CircleSet[index];
                // 起点按钮
                if (index == 0)
                {
                    ctx.MoveTo(circle.Center.X, circle.Center.Y);
                }
                else
                {
                    // 全部是连线
                    ctx.AddLineToPoint(circle.Center.X, circle.Center.Y);
                }
            }

            // 连接最后一个按钮到手指当前触摸得点
            if (!CurrentPoint.Equals(new CGPoint(0, 0)))
            {
                foreach (var item in Subviews)
                {
                    if (GetCircleState() == CircleState.CircleStateError || GetCircleState() == CircleState.CircleStateLastOneError)
                    {
                        // 如果是错误的状态下不连接到当前点
                    }
                    else
                    {
                        ctx.AddLineToPoint(CurrentPoint.X, CurrentPoint.Y);
                    }
                }
            }
             
            //线条转角样式
            ctx.SetLineCap(CGLineCap.Round);
            ctx.SetLineJoin(CGLineJoin.Round);
            // 设置绘图的属性
            ctx.SetLineWidth(PCCircleViewConst.CircleConnectLineWidth);
            // 线条颜色
            color.SetColor();
            //渲染路径
            ctx.StrokePath();
        }
        
        /// <summary> 是否剪裁子控件
        /// </summary>
        /// <param name="ctx">图形上下文</param>
        /// <param name="clip">是否剪裁</param>
        void ClipSubviewsWhenConnectInContext(CGContext ctx, bool clip)
        {
            if (clip)
            {
                // 遍历所有子控件
                foreach (PCCircle circle in Subviews)
                {
                    // 确定"剪裁"的形状
                    ctx.AddEllipseInRect(circle.Frame);
                }
            }
        }


        /// <summary>每添加一个圆，就计算一次方向
        /// </summary>
        public void CalAngleAndconnectTheJumpedCircle()
        {
            if (CircleSet == null || CircleSet.Count <= 1)
                return;
            //取出最后一个对象
            PCCircle lastOne = CircleSet.Last();
            //倒数第二个
            PCCircle lastTwo = CircleSet[CircleSet.Count - 2];
            //计算倒数第二个的位置
            nfloat last_1_x = lastOne.Center.X;
            nfloat last_1_y = lastOne.Center.Y;
            nfloat last_2_x = lastTwo.Center.X;
            nfloat last_2_y = lastTwo.Center.Y;

            // 1.计算角度（反正切函数）
            nfloat angle =(nfloat)(Math.Atan2(last_1_y - last_2_y, last_1_x - last_2_x) + Math.PI/2);
            lastTwo.Angle = angle;

            // 2.处理跳跃连线
            CGPoint center = CenterPointWithPointOne(lastOne.Center, lastTwo.Center);
            PCCircle centerCircle = EnumCircleSetToFindWhichSubviewContainTheCenterPoint(center);
            if (centerCircle != null)
            {
                // 把跳过的圆加到数组中，它的位置是倒数第二个
                if (!CircleSet.Contains(centerCircle))
                {
                    CircleSet.Insert(CircleSet.Count - 1, centerCircle);
                }
            }
        }

        /// <summary>提供两个点，返回一个它们的中点
        /// </summary>
        /// <param name="pointOne"></param>
        /// <param name="pointTwo"></param>
        /// <returns></returns>
        public CGPoint CenterPointWithPointOne(CGPoint pointOne, CGPoint pointTwo)
        {
           nfloat x1 = pointOne.X > pointTwo.X ? pointOne.X : pointTwo.X;
           nfloat x2 = pointOne.X < pointTwo.X ? pointOne.X : pointTwo.X;
           nfloat y1 = pointOne.Y > pointTwo.Y ? pointOne.Y : pointTwo.Y;
           nfloat y2 = pointOne.Y < pointTwo.Y ? pointOne.Y : pointTwo.Y;
           return new CGPoint((x1 + x2) / 2, (y1 + y2) / 2);
        }

        /// <summary> 给一个点，判断这个点是否被圆包含，如果包含就返回当前圆，如果不包含返回的是NULL
        /// 
        /// </summary>
        /// <param name="point">前点</param>
        /// <returns>点所在的圆</returns>
        public PCCircle EnumCircleSetToFindWhichSubviewContainTheCenterPoint(CGPoint point)
        {
            PCCircle centerCircle=null;
            foreach (PCCircle circle in Subviews)
            {
                if (circle.Frame.Contains(point))
                {
                    centerCircle = circle;
                }
            }

            if (centerCircle!=null && !CircleSet.Contains(centerCircle))
            {
                // 这个circle的角度和倒数第二个circle的角度一致
                centerCircle.Angle = CircleSet[CircleSet.Count - 2].Angle;
            }
            return centerCircle; // 注意：NULL，就是当前点不在圆内
        } 
    }

    /// <summary>手势密码界面用途类型
    /// </summary>
    public enum CircleViewType
    {
        /// <summary>设置手势密码
        /// </summary>
        CircleViewTypeSetting = 1,
        /// <summary>登陆手势密码
        /// </summary>
        CircleViewTypeLogin,
        /// <summary>验证旧手势密码
        /// </summary>
        CircleViewTypeVerify,    
    }
}