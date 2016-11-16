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
        #region  �����������������
        /// <summary>���߸�������4��ʱ��֪ͨ����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">���ƽ��</param>
        public virtual void CircleViewConnectCirclesLessThanNeedWithGesture(PCCircleView view, CircleViewType type, string gesture)
        {

        }
        /// <summary>���߸������ڻ����4������ȡ����һ����������ʱ֪ͨ����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">��һ���α��������</param>
        public virtual void CircleViewDidCompleteSetFirstGesture(PCCircleView view, CircleViewType type, string gesture)
        {

        }

        /// <summary>��½������֤���������������ʱ�Ĵ�����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">��½ʱ����������</param>
        /// <param name="result"></param>
        public virtual void CircleViewDidCompleteSetSecondGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {

        }

        /// <summary>��½������֤���������������ʱ�Ĵ�����
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="gesture">��½ʱ����������</param>
        /// <param name="result"></param>
        public virtual void CircleViewDidCompleteLoginGesture(PCCircleView view, CircleViewType type, string gesture, bool equal)
        {

        }
        #endregion
    }
}