using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Studio23.SS2.AchievementSystem.Providers;
using Studio23.SS2.Authsystem.XboxCorePC.Data;
using UnityEngine;
using XGamingRuntime;

public class XboxCoreAchievementManager : AchievementProvider
    {
        private UserManager.UserData _currentUserData;

        public override void Initialize()
        {
            // _currentUserData = GamingRuntimeManager.Instance.UserManager.UserDataList[0];

            SDK.XGameGetXboxTitleId(out uint titleId);
            

            SDK.XBL.XblAchievementsGetAchievementsForTitleIdAsync(
                _currentUserData.m_context,
                _currentUserData.userXUID,
                titleId,
                XblAchievementType.All,
                false,
                XblAchievementOrderBy.DefaultOrder,
                0, 100, AchievementFetchRoutineComplete);

        }


        private void AchievementFetchRoutineComplete(int hresult, XblAchievementsResultHandle result)
        {
            XblAchievement[] achievements;
            int resultCode = SDK.XBL.XblAchievementsResultGetAchievements(result, out achievements);
            if (resultCode == 0)
            {
                achievementDatas = new AchievementData[achievements.Length];

                for (int i = 0; i < achievements.Length; i++)
                {
                    AchievementData achievementData = new AchievementData();
                    achievementData.Name = achievements[i].Name;
                    achievementData.Id = achievements[i].Id;
                    achievementData.ProgressState = achievements[i].ProgressState.ToString();
                    achievementData.IsUnlocked = achievements[i].ProgressState == XblAchievementProgressState.Achieved;
                    foreach (var req in achievements[i].Progression.Requirements)
                    {
                        achievementData.Progression = float.Parse(req.CurrentProgressValue);
                    }
                    achievementDatas[i] = achievementData;
                }
            }
            else
            {
                Debug.LogError("Error retrieving achievements");
            }
            SDK.XBL.XblAchievementsResultCloseHandle(result);

        }

 
        /// <summary>
        /// Gets achievement from already initialized list of achievement data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override AchievementData GetAchievement(string id)
        {
            AchievementData achievementData = new AchievementData();

            achievementData = achievementDatas.FirstOrDefault(r => r.Id == _achievementMapper.GetMappedID(id));
            if(achievementData == null)
            {
                Debug.Log("No matching achievement found");
            }

            return achievementData;
        }


        /// <summary>
        /// Updates achievement progress
        /// </summary>
        /// <param name="achievementIdentifier"></param>
        /// <param name="progression"></param>
        public override void UpdateAchievementProgress(string achievementIdentifier, float progression)
        {

            SDK.XBL.XblAchievementsUpdateAchievementAsync(
                _currentUserData.m_context,
                _currentUserData.userXUID,
                _achievementMapper.GetMappedID(achievementIdentifier),
                (uint)progression,
                (a) => { 
                    
                    }
                );
        }


    }

 