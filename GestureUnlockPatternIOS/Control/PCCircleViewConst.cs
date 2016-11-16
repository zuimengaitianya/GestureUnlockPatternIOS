using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace GestureUnlockPatternIOS.Control
{
    /// <summary>������������
    /// </summary>
    public class PCCircleViewConst
    {
        /// <summary>�ֻ���Ļ���
        /// </summary>
        public static nfloat kScreenW = UIScreen.MainScreen.Bounds.Size.Width;
        public static nfloat kScreenH = UIScreen.MainScreen.Bounds.Size.Height;
        /// <summary>��ɫ����
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
        /// <summary>����Բ����ɫ
        /// </summary>
        public static UIColor CircleBackgroundColor= UIColor.Clear;
        /// <summary>��������ɫ
        /// </summary>
        public static UIColor CircleViewBackgroundColor = rgb(13, 52, 89);

        /// <summary>��ͨ״̬�������Բ��ɫ
        /// </summary>
        public static UIColor CircleStateNormalOutsideColor = rgb(241, 241, 241);
        /// <summary>ѡ��״̬�������Բ��ɫ
        /// </summary>
        public static UIColor CircleStateSelectedOutsideColor = rgb(34, 178, 246);
        /// <summary>����״̬�������Բ��ɫ
        /// </summary>
        public static UIColor CircleStateErrorOutsideColor = rgb(254, 82, 92);

        /// <summary>��ͨ״̬����ʵ��Բ��ɫ
        /// </summary>
        public static UIColor CircleStateNormalInsideColor= UIColor.Clear;
        /// <summary>ѡ��״̬����ʵ��Բ��ɫ
        /// </summary>
        public static UIColor CircleStateSelectedInsideColor = rgb(34, 178, 246);
        /// <summary>����״̬��ʵ��Բ��ɫ
        /// </summary>
        public static UIColor CircleStateErrorInsideColor = rgb(254, 82, 92);

        /// <summary>��ͨ״̬����������ɫ
        /// </summary>
        public static UIColor CircleStateNormalTrangleColor= UIColor.Clear;
        /// <summary>ѡ��״̬����������ɫ
        /// </summary>
        public static UIColor CircleStateSelectedTrangleColor = rgb(34, 178, 246);
        /// <summary>����״̬��������ɫ
        /// </summary>
        public static UIColor CircleStateErrorTrangleColor = rgb(254, 82, 92);
        /// <summary>�����α߳�
        /// </summary>
        public static nfloat kTrangleLength = 10.0f;

        /// <summary>��ͨʱ������ɫ
        /// </summary>
        public static UIColor CircleConnectLineNormalColor = rgb(34, 178, 246);
        /// <summary>����ʱ������ɫ
        /// </summary>
        public static UIColor CircleConnectLineErrorColor = rgb(254, 82, 92);
        /// <summary>���߿��
        /// </summary>
        public static nfloat CircleConnectLineWidth = 1.0f;

        /// <summary>����Բ�İ뾶
        /// </summary>
        public static nfloat CircleRadius = 30.0f;
       
        /// <summary>����ԲԲ�����
        /// </summary>
        public static nfloat CircleEdgeWidth = 1.0f;

        /// <summary>�Ź���չʾinfoView ����Բ�İ뾶
        /// </summary>
        public static nfloat CircleInfoRadius = 5;

        /// <summary>�ڲ�ʵ��Բռ����Բ�ı���ϵ��
        /// </summary>
        public static nfloat CircleRadio = 0.4f;

        /// <summary>��������View����ʱ��������Ļ��ߺ��ұߵľ���
        /// </summary>
        public static nfloat CircleViewEdgeMargin = 30.0f;

        ///// <summary>��������View��Center.yֵ �ڵ�ǰ��Ļ��3/5λ��
        ///// </summary>
        public static nfloat CircleViewCenterY  = kScreenH * 3 / 5;

        /// <summary>���ӵ�Բ���ٵĸ���
        /// </summary>
        public static nint CircleSetCountLeast = 4;

        /// <summary> ����״̬�»��Ե�ʱ��
        /// </summary>
        public static int kdisplayTime =150;

        /// <summary>��ͨ״̬��������ʾ����ɫ
        /// </summary>
        public static UIColor textColorNormalState = rgb(241, 241, 241);
        /// <summary>����״̬��������ʾ����ɫ
        /// </summary>
        public static UIColor textColorWarningState = rgb(254, 82, 92);

        /// <summary>���ƽ�������׼����ʱ����ʾ����
        /// </summary>
        public static string gestureTextBeforeSet ="���ƽ���ͼ��";
        /// <summary>����ʱ�����߸����٣���ʾ����
        /// </summary>
        public static string gestureTextConnectLess = string.Format("��������{0}���㣬����������", CircleSetCountLeast);
        /// <summary>ȷ��ͼ������ʾ�ٴλ���
        /// </summary>
        public static string gestureTextDrawAgain ="�ٴλ��ƽ���ͼ��";
        /// <summary>NSString����ʾ����
        /// </summary>
        public static string gestureTextDrawAgainError = "���ϴλ��Ʋ�һ�£������»���";

        /// <summary>���óɹ�
        /// </summary>
        public static string gestureTextSetSuccess = "���óɹ�";

        /// <summary>������ԭ��������
        /// </summary>
        public static string gestureTextLoginGesture = "��������������";

        /// <summary>������ԭ��������
        /// </summary>
        public static string gestureTextOldGesture =@"������ԭ��������";
        /// <summary>�������
        /// </summary>
        public static string gestureTextGestureVerifyError = "���������������";

        /// <summary>������������
        /// </summary>
        public static string gestureTextForgetGesture = "������������";

        public static Dictionary<string,string> passWordDic=new Dictionary<string, string>();
        /// <summary>��һ�����õ�����
        /// </summary>
        public static string firstGesturePassWord;
        /// <summary>���ַ������������룩
        /// </summary>
        /// <param name="gesture">�ַ�������</param>
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
        /// <summaryȡ�ַ�����������
        /// </summary>
        /// <returns>�ַ�������</returns>
        public static string GetGesturePassWord()
        {
            return firstGesturePassWord == null ? "": firstGesturePassWord;
        }

        /// <summary>���»�������ͼ��
        /// </summary>
        public static void ResetGesturePassWord()
        {
            firstGesturePassWord = "";
        }
    }
}