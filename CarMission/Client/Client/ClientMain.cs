using CarMission.Client.Common;
using CarMission.Client.Peds;
using CitizenFX.Core;
using System;
using System.Threading.Tasks;

namespace CarMission.Client.Client
{
    public class ClientMain : BaseScript
    {
        #region Class Instance
        private static ClientMain _instance;
        #endregion

        public ClientMain()
        {
            _instance = this;

            ClassLoader.Init();
        }

        public static ClientMain GetInstance()
        {
            return _instance;
        }

        [Tick]
        public Task OnTick()
        {
            return Task.FromResult(0);
        }

        public void RegisterTickHandler(Func<Task> action)
        {
            try
            {
                Tick += action;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        public void DeregisterTickHandler(Func<Task> action)
        {
            try
            {
                Tick -= action;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }
    }
}