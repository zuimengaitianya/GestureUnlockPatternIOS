using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    /// <summary>常量及帮助类
    /// </summary>
    public class PCCircleViewConst
    {
        /// <summary>手机屏幕宽高
        /// </summary>
        public static nfloat kScreenW = UIScreen.MainScreen.Bounds.Size.Width;
        public static nfloat kScreenH = UIScreen.MainScreen.Bounds.Size.Height;
        /// <summary>颜色处理
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static UIColor rgb(int r, int g, int b)
        {
           return UIColor.FromRGB( r,  g,  b);
        }
        /// <summary>单个圆背景色
        /// </summary>
        public static UIColor CircleBackgroundColor= UIColor.Clear;
        /// <summary>解锁背景色
        /// </summary>
        public static UIColor CircleViewBackgroundColor = rgb(13, 52, 89);

        /// <summary>普通状态下外空心圆颜色
        /// </summary>
        public static UIColor CircleStateNormalOutsideColor = rgb(241, 241, 241);
        /// <summary>选中状态下外空心圆颜色
        /// </summary>
        public static UIColor CircleStateSelectedOutsideColor = rgb(34, 178, 246);
        /// <summary>错误状态下外空心圆颜色
        /// </summary>
        public static UIColor CircleStateErrorOutsideColor = rgb(254, 82, 92);

        /// <summary>普通状态下内实心圆颜色
        /// </summary>
        public static UIColor CircleStateNormalInsideColor= UIColor.Clear;
        /// <summary>选中状态下内实心圆颜色
        /// </summary>
        public static UIColor CircleStateSelectedInsideColor = rgb(34, 178, 246);
        /// <summary>错误状态内实心圆颜色
        /// </summary>
        public static UIColor CircleStateErrorInsideColor = rgb(254, 82, 92);

        /// <summary>普通状态下三角形颜色
        /// </summary>
        public static UIColor CircleStateNormalTrangleColor= UIColor.Clear;
        /// <summary>选中状态下三角形颜色
        /// </summary>
        public static UIColor CircleStateSelectedTrangleColor = rgb(34, 178, 246);
        /// <summary>错误状态三角形颜色
        /// </summary>
        public static UIColor CircleStateErrorTrangleColor = rgb(254, 82, 92);
        /// <summary>三角形边长
        /// </summary>
        public static nfloat kTrangleLength = 10.0f;

        /// <summary>普通时连线颜色
        /// </summary>
        public static UIColor CircleConnectLineNormalColor = rgb(34, 178, 246);
        /// <summary>错误时连线颜色
        /// </summary>
        public static UIColor CircleConnectLineErrorColor = rgb(254, 82, 92);
        /// <summary>连线宽度
        /// </summary>
        public static nfloat CircleConnectLineWidth = 1.0f;

        /// <summary>单个圆的半径
        /// </summary>
        public static nfloat CircleRadius = 30.0f;
       
        /// <summary>空心圆圆环宽度
        /// </summary>
        public static nfloat CircleEdgeWidth = 1.0f;

        /// <summary>九宫格展示infoView 单个圆的半径
        /// </summary>
        public static nfloat CircleInfoRadius = 5;

        /// <summary>内部实心圆占空心圆的比例系数
        /// </summary>
        public static nfloat CircleRadio = 0.4f;

        /// <summary>整个解锁View居中时，距离屏幕左边和右边的距离
        /// </summary>
        public static nfloat CircleViewEdgeMargin = 30.0f;

        ///// <summary>整个解锁View的Center.y值 在当前屏幕的3/5位置
        ///// </summary>
        public static nfloat CircleViewCenterY  = kScreenH * 3 / 5;

        /// <summary>连接的圆最少的个数
        /// </summary>
        public static nint CircleSetCountLeast = 4;

        /// <summary> 错误状态下回显的时间
        /// </summary>
        public static int kdisplayTime =150;

        /// <summary>普通状态下文字提示的颜色
        /// </summary>
        public static UIColor textColorNormalState = rgb(241, 241, 241);
        /// <summary>警告状态下文字提示的颜色
        /// </summary>
        public static UIColor textColorWarningState = rgb(254, 82, 92);

        /// <summary>绘制解锁界面准备好时，提示文字
        /// </summary>
        public static string gestureTextBeforeSet ="绘制解锁图案";
        /// <summary>设置时，连线个数少，提示文字
        /// </summary>
        public static string gestureTextConnectLess = string.Format("最少连接{0}个点，请重新输入", CircleSetCountLeast);
        /// <summary>确认图案，提示再次绘制
        /// </summary>
        public static string gestureTextDrawAgain ="再次绘制解锁图案";
        /// <summary>NSString，提示文字
        /// </summary>
        public static string gestureTextDrawAgainError = "与上次绘制不一致，请重新绘制";

        /// <summary>设置成功
        /// </summary>
        public static string gestureTextSetSuccess = "设置成功";

        /// <summary>请输入原手势密码
        /// </summary>
        public static string gestureTextLoginGesture = "请输入手势密码";

        /// <summary>请输入原手势密码
        /// </summary>
        public static string gestureTextOldGesture =@"请输入原手势密码";
        /// <summary>密码错误
        /// </summary>
        public static string gestureTextGestureVerifyError = "手势密码输入错误";

        /// <summary>忘记手势密码
        /// </summary>
        public static string gestureTextForgetGesture = "忘记手势密码";

        public static Dictionary<string,string> passWordDic=new Dictionary<string, string>();
        /// <summary>第一次设置的密码
        /// </summary>
        public static string firstGesturePassWord;
        /// <summary>存字符串（手势密码）
        /// </summary>
        /// <param name="gesture">字符串对象</param>
        public static bool SaveGesturePassWord(string gesture)
        {
            bool result = false;
            if (string.IsNullOrEmpty(firstGesturePassWord))
            {
                firstGesturePassWord = gesture;
                result = true;
            }
            return result;
        }
        /// <summary取字符串手势密码
        /// </summary>
        /// <returns>字符串对象</returns>
        public static string GetGesturePassWord()
        {
            return firstGesturePassWord == null ? "": firstGesturePassWord;
        }

        /// <summary>重新绘制手势图案
        /// </summary>
        public static void ResetGesturePassWord()
        {
            firstGesturePassWord = "";
        }
    }
}