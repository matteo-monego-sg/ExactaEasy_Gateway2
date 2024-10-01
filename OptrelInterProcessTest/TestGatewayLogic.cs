namespace OptrelInterProcessTest
{
    internal class TestGatewayLogic
    {
        /// <summary>
        /// 
        /// </summary>
        private FrmMain _frm;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frmMain"></param>
        public TestGatewayLogic(FrmMain frmMain)
        {
            _frm = frmMain;
        }
        /// <summary>
        /// 
        /// </summary>
        public int HmiSetSupIsAliveEx()
        {
            _frm.Log($"ExactaEasy called 'HmiSetSupIsAliveEx()', returning 1.");
            return 1;
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSetSupervisorBypassSendRecipe(bool bypass)
        {
            _frm.Log($"ExactaEasy called 'HmiSetSupervisorBypassSendRecipe({bypass})'.");
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        public string HmiGetErrorString(int errorCode)
        {
            _frm.Log($"ExactaEasy called 'HmiGetErrorString({errorCode})', returning '{errorCode}'.");
            return errorCode.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSetSupervisorMode(int hmiMode)
        {
            _frm.Log($"ExactaEasy called 'HmiSetSupervisorMode({hmiMode})'.");
        }
    }
}
