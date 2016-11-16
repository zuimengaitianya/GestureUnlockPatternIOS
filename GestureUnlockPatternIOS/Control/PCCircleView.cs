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
    /// <summary>�������ƵľŸ���Բ
    /// </summary>
    public class PCCircleView : UIView
    {
        public ICircleViewDelegate Delegate;//����

        public bool Clip { get; set; }//�Ƿ���� default is true
        public CircleViewType Type { get; set; }//��������
        private bool arrow ;
        /// <summary>�Ƿ��м�ͷ default is YES
        /// </summary>
        public bool Arrow
        {
            get { return arrow; }
            set
            {
                arrow = value;
                // �����ӿؼ����ı����Ƿ��м�ͷ
                foreach (PCCircle item in Subviews)
                {
                    item.Arrow = arrow;
                }
            }
        }

        private List<PCCircle> circleSet;
        /// <summary>ѡ�е�Բ�ļ���
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
        CGPoint CurrentPoint { get; set; }// ��ǰ��
        bool HasClean { get; set; }//������ձ�־

        /// <summary>��ʼ������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="clip">�Ƿ����</param>
        /// <param name="arrow">�����μ�ͷ</param>
        public PCCircleView(CircleViewType type, bool clip, bool arrow) : base()
        {
            // ������ͼ׼��
            LockViewPrepare();

            this.Type = type;
            this.Clip = clip;
            this.Arrow = arrow;
        }
        public PCCircleView() : base()
        {
            // ������ͼ׼��
            LockViewPrepare();
        }

        public PCCircleView(NSCoder aDecoder) : base(aDecoder)
        {
            // ������ͼ׼��
            LockViewPrepare();
        }

        /// <summary>������ͼ׼��
        /// </summary>
        public void LockViewPrepare()
        {
            this.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width - PCCircleViewConst.CircleViewEdgeMargin * 2, UIScreen.MainScreen.Bounds.Size.Width - PCCircleViewConst.CircleViewEdgeMargin * 2);
            this.Center = new CGPoint(UIScreen.MainScreen.Bounds.Size.Width / 2, PCCircleViewConst.CircleViewCenterY);
            // Ĭ�ϼ����ӿؼ�
            Clip = true;
            // Ĭ���м�ͷ
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
                // ����tag -> �����¼�ĵ�Ԫ
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
                //���������һ������Ĵ���
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
                        // move�����е����ߣ�������Ծ���ߵĴ���
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
                // ����ǵ�¼������֤ԭ�������룬�͸�Ϊ��Ӧ��״̬ ���������û�����Ǽ�ͷ��
                //if (Type != CircleViewType.CircleViewTypeSetting)
                //{
                //    circle.State = CircleState.CircleStateLastOneSelected;
                //}
            }
            if (CircleSet.Count > 0)
            {
                // ���������һ������Ĵ���
                CircleSetLastObjectWithState(CircleState.CircleStateLastOneSelected);
                SetNeedsDisplay();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            HasClean= false;
            //��������
            string gesture = GetGestureResultFromCircleSet(CircleSet);
            nfloat length = gesture.Length;
            if (length == 0)
            {
                return;
            }

            // ���ƻ��ƽ������
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
            // ���ƽ������Ƿ��������ػ棬ȡ�����Ƿ���ʱ��������״̬��ԭ
            ErrorToDisplay();
        }

        /// <summary>�Ƿ��������ػ�
        /// </summary>
        public void ErrorToDisplay() {
            if (GetCircleState() == CircleState.CircleStateError || GetCircleState() == CircleState.CircleStateLastOneError)
            {
                //�˴�Ӧ����ʱ ����ʾ��ɫ��������
                //����˵�� 
                //dueTime������ callback ֮ǰ�ӳٵ�ʱ�������Ժ���Ϊ��λ����ָ�� Timeout.Infinite �Է�ֹ��ʱ����ʼ��ʱ��ָ���� (0) ������������ʱ����
                //Period������ callback ��ʱ�������Ժ���Ϊ��λ����ָ�� Timeout.Infinite ���Խ��ö�����ֹ��
                System.Threading.Timer tt = new System.Threading.Timer(new System.Threading.TimerCallback(GestureEndResetMembers),obj, PCCircleViewConst.kdisplayTime, System.Threading.Timeout.Infinite);
            }
            else
            {
                GestureEndResetMembers(obj);
            }
        }

        public static object obj = new object();

        /// <summary>���ƽ���ʱ����ղ���
        /// </summary>
        public void GestureEndResetMembers(object obj)
        {
            // ��֤�̰߳�ȫ
            lock (obj)
            {
                BeginInvokeOnMainThread(() =>
                {
                    if (!HasClean)
                    {
                        // ������ϣ�ѡ�е�Բ�ع���ͨ״̬
                        ChangeCircleInCircleSetWithState(CircleState.CircleStateNormal);
                        // �������
                        CircleSet.Clear();
                        // ��շ���
                        ResetAllCirclesDirect();
                        // ���֮��ı�clean��״̬
                        HasClean = true;
                    }
                });
            }
        }
        // ��ȡ��ǰѡ��Բ��״̬
        CircleState GetCircleState() {
            return CircleSet.First().State;
        }
        /// <summary> ��������ӿؼ��ķ���
        /// </summary>
        void ResetAllCirclesDirect()
        {
            foreach (PCCircle item in Subviews)
            {
                item.State = 0;
            }
        }

        /// <summary>�����������һ������Ĵ���
        /// </summary>
        /// <param name="state"></param>
        public void CircleSetLastObjectWithState(CircleState state)
        {
            CircleSet.Last().State = state;
        }

        /// <summary> �������ͣ����� ����·���Ĵ���
        /// </summary>
        /// <param name="gesture">��������</param>
        /// <param name="length">���Ƴ���</param>
        public void GestureEndByTypeSettingWithGesture(string gesture, nfloat length)
        {
            var myCircleViewDelegate = Delegate as MyCircleViewDelegate;

            // �����������ٸ��� ��<4����
            if (length < PCCircleViewConst.CircleSetCountLeast)
            {
                // 1.֪ͨ����
                if (myCircleViewDelegate != null)
                {
                    myCircleViewDelegate.CircleViewConnectCirclesLessThanNeedWithGesture(this, Type, gesture);
                }
                // 2.�ı�״̬Ϊerror
                ChangeCircleInCircleSetWithState(CircleState.CircleStateError);
            }
            else// ���Ӷ������ٸ��� ��>=4����
            {
                string gestureOne = PCCircleViewConst.GetGesturePassWord();
                // ���ղ��洢��һ������
                if (gestureOne.Length < PCCircleViewConst.CircleSetCountLeast)
                {
                    // ��¼��һ������
                    PCCircleViewConst.SaveGesturePassWord(gesture);
                    // ֪ͨ����
                    if (myCircleViewDelegate != null)
                    {
                        myCircleViewDelegate.CircleViewDidCompleteSetFirstGesture(this, Type, gesture);
                    }
                }
                else
                {
                    // ���ܵڶ������벢���һ������ƥ�䣬һ�º�洢����  // ƥ����������
                    bool equal = gesture.Equals(PCCircleViewConst.GetGesturePassWord());
                    // ֪ͨ����
                    // ֪ͨ����
                    if (myCircleViewDelegate != null)
                    {
                        myCircleViewDelegate.CircleViewDidCompleteSetSecondGesture(this, Type, gesture,equal);
                    }

                    if (equal)
                    {
                        // һ�£��洢����
                        PCCircleViewConst.SaveGesturePassWord(gesture);
                    }
                    else
                    {
                        // ��һ�£��ػ����
                        ChangeCircleInCircleSetWithState(CircleState.CircleStateError);
                    }
                }
            }
        }

        /// <summary>�������ͣ���½ ����·���Ĵ���
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="length"></param>
        public void GestureEndByTypeLoginWithGesture(string gesture, nfloat length)
        {
            string password = PCCircleViewConst.GetGesturePassWord();
            bool equal = gesture.Equals(password);
            // ֪ͨ����
            var myCircleViewDelegate = Delegate as MyCircleViewDelegate;
            if (myCircleViewDelegate != null)
            {
                myCircleViewDelegate.CircleViewDidCompleteLoginGesture(this,Type,gesture,equal);
            }

            if (!equal)
            {
                // ��һ�£��ػ����
                ChangeCircleInCircleSetWithState(CircleState.CircleStateError);
            }
        }

        /// <summary>�������ͣ���֤ ����·���Ĵ���
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="length"></param>
        public void GestureEndByTypeVerifyWithGesture(string gesture, nfloat length)
        {
            GestureEndByTypeLoginWithGesture(gesture , length);
        }

        /// <summary>�ı�ѡ������CircleSet�ӿؼ�״̬
        /// </summary>
        /// <param name="state"></param>
        public void ChangeCircleInCircleSetWithState(CircleState state)
        {
            for (int i = 0; i < CircleSet.Count; i++)
            {
                CircleSet[i].State = state;
                // ����Ǵ���״̬���Ǿͽ����һ����ť���⴦��
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

        /// <summary>��circleSet�������������ƴ���������ַ���
        /// </summary>
        /// <param name="circleSet"></param>
        /// <returns></returns>
        public string GetGestureResultFromCircleSet(List<PCCircle> CircleSet)
        {
            StringBuilder gesture = new StringBuilder();
            // ����ȡtagƴ�ַ���
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
            // ���û���κ�ѡ�а�ť�� ֱ��retrun
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
            // ����ͼ��
            ConnectCirclesInRect(rect, color);
        }

        /// <summary> ���߻���ͼ��(���趨��ɫ����)  ��ѡ�е�Բ����color��ɫ��������
        /// </summary>
        /// <param name="rect">ͼ��������</param>
        /// <param name="color">������ɫ</param>
        public void ConnectCirclesInRect(CGRect rect, UIColor color)
        {
            //��ȡ������
            CGContext ctx = UIGraphics.GetCurrentContext();
            // ���·��
            ctx.AddRect(rect);
            //�Ƿ����
            ClipSubviewsWhenConnectInContext(ctx, Clip);
            //����������
            ctx.EOClip();
            // ���������е�circle
            for (int index = 0; index < CircleSet.Count; index++)
            {
                // ȡ��ѡ�а�ť
                PCCircle circle = CircleSet[index];
                // ��㰴ť
                if (index == 0)
                {
                    ctx.MoveTo(circle.Center.X, circle.Center.Y);
                }
                else
                {
                    // ȫ��������
                    ctx.AddLineToPoint(circle.Center.X, circle.Center.Y);
                }
            }

            // �������һ����ť����ָ��ǰ�����õ�
            if (!CurrentPoint.Equals(new CGPoint(0, 0)))
            {
                foreach (var item in Subviews)
                {
                    if (GetCircleState() == CircleState.CircleStateError || GetCircleState() == CircleState.CircleStateLastOneError)
                    {
                        // ����Ǵ����״̬�²����ӵ���ǰ��
                    }
                    else
                    {
                        ctx.AddLineToPoint(CurrentPoint.X, CurrentPoint.Y);
                    }
                }
            }
             
            //����ת����ʽ
            ctx.SetLineCap(CGLineCap.Round);
            ctx.SetLineJoin(CGLineJoin.Round);
            // ���û�ͼ������
            ctx.SetLineWidth(PCCircleViewConst.CircleConnectLineWidth);
            // ������ɫ
            color.SetColor();
            //��Ⱦ·��
            ctx.StrokePath();
        }
        
        /// <summary> �Ƿ�����ӿؼ�
        /// </summary>
        /// <param name="ctx">ͼ��������</param>
        /// <param name="clip">�Ƿ����</param>
        void ClipSubviewsWhenConnectInContext(CGContext ctx, bool clip)
        {
            if (clip)
            {
                // ���������ӿؼ�
                foreach (PCCircle circle in Subviews)
                {
                    // ȷ��"����"����״
                    ctx.AddEllipseInRect(circle.Frame);
                }
            }
        }


        /// <summary>ÿ���һ��Բ���ͼ���һ�η���
        /// </summary>
        public void CalAngleAndconnectTheJumpedCircle()
        {
            if (CircleSet == null || CircleSet.Count <= 1)
                return;
            //ȡ�����һ������
            PCCircle lastOne = CircleSet.Last();
            //�����ڶ���
            PCCircle lastTwo = CircleSet[CircleSet.Count - 2];
            //���㵹���ڶ�����λ��
            nfloat last_1_x = lastOne.Center.X;
            nfloat last_1_y = lastOne.Center.Y;
            nfloat last_2_x = lastTwo.Center.X;
            nfloat last_2_y = lastTwo.Center.Y;

            // 1.����Ƕȣ������к�����
            nfloat angle =(nfloat)(Math.Atan2(last_1_y - last_2_y, last_1_x - last_2_x) + Math.PI/2);
            lastTwo.Angle = angle;

            // 2.������Ծ����
            CGPoint center = CenterPointWithPointOne(lastOne.Center, lastTwo.Center);
            PCCircle centerCircle = EnumCircleSetToFindWhichSubviewContainTheCenterPoint(center);
            if (centerCircle != null)
            {
                // ��������Բ�ӵ������У�����λ���ǵ����ڶ���
                if (!CircleSet.Contains(centerCircle))
                {
                    CircleSet.Insert(CircleSet.Count - 1, centerCircle);
                }
            }
        }

        /// <summary>�ṩ�����㣬����һ�����ǵ��е�
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

        /// <summary> ��һ���㣬�ж�������Ƿ�Բ��������������ͷ��ص�ǰԲ��������������ص���NULL
        /// 
        /// </summary>
        /// <param name="point">ǰ��</param>
        /// <returns>�����ڵ�Բ</returns>
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
                // ���circle�ĽǶȺ͵����ڶ���circle�ĽǶ�һ��
                centerCircle.Angle = CircleSet[CircleSet.Count - 2].Angle;
            }
            return centerCircle; // ע�⣺NULL�����ǵ�ǰ�㲻��Բ��
        } 
    }

    /// <summary>�������������;����
    /// </summary>
    public enum CircleViewType
    {
        /// <summary>������������
        /// </summary>
        CircleViewTypeSetting = 1,
        /// <summary>��½��������
        /// </summary>
        CircleViewTypeLogin,
        /// <summary>��֤����������
        /// </summary>
        CircleViewTypeVerify,    
    }
}