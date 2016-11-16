using GestureUnlockPatternIOS.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GestureUnlockPatternIOS.Interface
{
    public interface ICircleViewDelegate
    {
    }

    public class CircleViewDelegate: ICircleViewDelegate
    {
        #region  设置手势密码代理方法
        /// <summary>连线个数少于4个时，通知代理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">手势结果</param>
        public virtual void CircleViewConnectCirclesLessThanNeedWithGesture(PCCircleView view, CircleViewType type, string gesture)
        {

        }
        /// <summary>连线个数多于或等于4个，获取到第一个手势密码时通知代理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">第一个次保存的密码</param>
        public virtual void CircleViewDidCompleteSetFirstGesture(PCCircleView view, CircleViewType type, string gesture)
        {

        }

        /// <summary>登陆或者验证手势密码输入完成时的代理方法
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">登陆时的手势密码</param>
        /// <param name="result"></param>
        public virtual void CircleViewDidCompleteSetSecondGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {

        }

        /// <summary>登陆或者验证手势密码输入完成时的代理方法
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">登陆时的手势密码</param>
        /// <param name="result"></param>
        public virtual void CircleViewDidCompleteLoginGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {

        }
        #endregion
    }
}