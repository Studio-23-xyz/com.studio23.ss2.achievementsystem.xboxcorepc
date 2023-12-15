using System.Linq;
using UnityEngine;
using Studio23.SS2.AchievementSystem.Providers;
using Studio23.SS2.Authsystem.XboxCorePC.Core;
using Studio23.SS2.Authsystem.XboxCorePC.Data;
using XGamingRuntime;

namespace Studio23.SS2.AchievementSystem.XboxCorePc.Core
{
    
    
    public class XboxCorePcAchievementManager : AchievementProvider
    {
        [ContextMenu("Initialize")]
        public override void Initialize()
        {
            MSGdk.Helpers.XblAchievementsGetAchievementsForTitleIdAsync();
        }
        
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
        
        public override void UpdateAchievementProgress(string achievementIdentifier, float progression)
        {

            MSGdk.Helpers.UnlockAchievementProgression(achievementIdentifier, (uint)progression);
        }
    }

}


  