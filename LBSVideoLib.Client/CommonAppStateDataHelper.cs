using LBFVideoLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public static class CommonAppStateDataHelper
    {
        static CommonAppStateDataHelper()
        {
            _applicationFormList = new Stack<Form>();
        }
        private static Stack<Form> _applicationFormList;

        public static void PushForm(Form currentForm)
        {
            _applicationFormList.Push(currentForm);
        }

        public static Form PopForm()
        {
            return _applicationFormList.Pop();
        }

        public static ClientInfo ClientInfoObject { get; set; }
    }
}
