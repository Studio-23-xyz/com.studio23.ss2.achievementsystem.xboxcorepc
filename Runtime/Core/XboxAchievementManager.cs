using System;
using System.Collections.Generic;
using Studio23.SS2.AchievementSystem.Providers;
 
using System.Linq;
using Studio23.SS2.AuthSystem.XboxCorePC.Core;
using Studio23.SS2.Authsystem.XboxCorePC.Data;
using UnityEngine;
using XGamingRuntime;


namespace Studio23.SS2.AchievementSystem.XboxCore.Core
{
    public class ErrorEventArgs : System.EventArgs
    {
        public string ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public ErrorEventArgs(string errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }
    }
    public class GameSaveLoadedArgs : System.EventArgs
    {
        public byte[] Data { get; private set; }

        public GameSaveLoadedArgs(byte[] data)
        {
            this.Data = data;
        }
    }
    
    public class XboxAchievementManager : AchievementProvider
    {
        private UserManager.UserData _currentUserData;

        private static Dictionary<int, string> _hresultToFriendlyErrorLookup;
        public delegate void OnErrorHandler(object sender, ErrorEventArgs e);
        public event OnErrorHandler OnError;
        
        /*XblAchievement[] xblAchievements;*/

        public override void Initialize()
        {
            _currentUserData = GamingRuntimeManager.Instance.UserManager.UserDataList[0];
            GetAllAchievements();
           
            _hresultToFriendlyErrorLookup = new Dictionary<int, string>();
            InitializeHresultToFriendlyErrorLookup();
        }
        public override bool IsAchievementUnlocked(string achievementName)
        {
           
           /*XblAchievement achievement=xblAchievements.FirstOrDefault(r=>r.Id==_achievementMapper.GetMappedID(achievementName));
           if (achievement == null) return false;
           return achievement.ProgressState == XblAchievementProgressState.Achieved;*/
           throw new NullReferenceException();
        }

        private void GetAllAchievements()
        {
            /*SDK.XBL.XblAchievementsManagerGetAchievements(_currentUserData.userXUID,XblAchievementOrderBy.DefaultOrder,XblAchievementsManagerSortOrder.Ascending, out XblAchievementsManagerResultHandle result);
            SDK.XBL.XblAchievementsManagerResultGetAchievements(result, out xblAchievements);
            SDK.XBL.XblAchievementsManagerResultCloseHandle(result);  */  
        }


        #region UpdateAchivementProgress

         public override void UpdateAchievementProgress(string achievementIdentifier, float progression)
        {
            //TODO Test In build
            //  _achievementMapper.GetMappedID(id)
           
            SDK.XBL.XblAchievementsUpdateAchievementAsync(
                _currentUserData.m_context, 
                _currentUserData.userXUID, 
                achievementIdentifier,  
                (uint)progression,
                UnlockAchievementComplete
            );
            
        }
         private void UnlockAchievementComplete(int hresult)
        {
            Succeeded(hresult, "Unlock achievement");
        }
        protected bool Succeeded(int hresult, string operationFriendlyName)
        {
            bool succeeded = false;
            if (HR.SUCCEEDED(hresult))
            {
                succeeded = true;
            }
            else
            {
                string errorCode = hresult.ToString("X8");
                string errorMessage = string.Empty;
                if (_hresultToFriendlyErrorLookup.ContainsKey(hresult))
                {
                    errorMessage = _hresultToFriendlyErrorLookup[hresult];
                }
                else
                {
                    errorMessage = operationFriendlyName + " failed.";
                }
                string formattedErrorString = string.Format("{0} Error code: hr=0x{1}", errorMessage, errorCode);
                Debug.LogError(formattedErrorString);
                if (OnError != null)
                {
                    OnError(this, new ErrorEventArgs(errorCode, errorMessage));
                }
            }

            return succeeded;
        }
        private void InitializeHresultToFriendlyErrorLookup()
        {
            if (_hresultToFriendlyErrorLookup == null)
            {
                return;
            }

            _hresultToFriendlyErrorLookup.Add(-2143330041, "IAP_UNEXPECTED: Does the player you are signed in as have a license for the game? " +
                                                           "You can get one by downloading your game from the store and purchasing it first. If you can't find your game in the store, " +
                                                           "have you published it in Partner Center?");

            _hresultToFriendlyErrorLookup.Add(-1994108656, "E_GAMEUSER_NO_PACKAGE_IDENTITY: Are you trying to call GDK APIs from the Unity editor?" +
                                                           " To call GDK APIs, you must use the GDK > Build and Run menu. You can debug your code by attaching the Unity debugger once your" +
                                                           "game is launched.");

            _hresultToFriendlyErrorLookup.Add(-1994129152, "E_GAMERUNTIME_NOT_INITIALIZED: Are you trying to call GDK APIs from the Unity editor?" +
                                                           " To call GDK APIs, you must use the GDK > Build and Run menu. You can debug your code by attaching the Unity debugger once your" +
                                                           "game is launched.");

            _hresultToFriendlyErrorLookup.Add(-2015559675, "AM_E_XAST_UNEXPECTED: Have you added the Windows 10 PC platform on the Xbox Settings page " +
                                                           "in Partner Center? Learn more: aka.ms/sandboxtroubleshootingguide");
        }
       

        #endregion
        
       
        
        public override float GetStat(string statName)
        {
            throw new System.NotImplementedException();
        }

        public override void SetStat(string statName, float value)
        {
            /*XblTitleManagedStatistic.Create(_statsMapper.GetMappedID(statName), value, out XblTitleManagedStatistic statsData);
            SDK.XBL.XblTitleManagedStatsUpdateStatsAsync(_currentUserData.m_context, new XblTitleManagedStatistic[] { statsData }, (a) =>
            {

            });*/
        }

       
    }

}
