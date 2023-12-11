using System;
using Studio23.SS2.AchievementSystem.Providers;
 
using System.Linq;
using Studio23.SS2.AuthSystem.XboxCorePC.Core;
using Studio23.SS2.Authsystem.XboxCorePC.Data;
using UnityEngine;
using XGamingRuntime;


namespace Studio23.SS2.AchievementSystem.XboxCore.Core
{
    public class XboxAchievementManager : AchievementProvider
    {
        private UserManager.UserData _currentUserData;

        /*XblAchievement[] xblAchievements;*/

        public override void Initialize()
        {
            _currentUserData = GamingRuntimeManager.Instance.UserManager.UserDataList[0];
            GetAllAchievements();
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


        public override void UnlockAchievement(string id)
        {

            //TODO Test In build
            /*SDK.XBL.XblAchievementsManagerUpdateAchievement(_currentUserData.userXUID, _achievementMapper.GetMappedID(id), 100);*/

            SDK.XBL.XblAchievementsUpdateAchievementAsync(_currentUserData.m_context, _currentUserData.userXUID, _achievementMapper.GetMappedID(id), 100, hresult =>
            {
                Debug.Log(hresult);
            });
        }

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
