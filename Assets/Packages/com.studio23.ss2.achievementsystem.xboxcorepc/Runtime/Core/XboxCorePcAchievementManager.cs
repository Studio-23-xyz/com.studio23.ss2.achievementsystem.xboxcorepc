using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Studio23.SS2.AchievementSystem.Providers;
using Studio23.SS2.Authsystem.XboxCorePC.Core;

namespace Studio23.SS2.AchievementSystem.XboxCorePc.Core
{
    
    
    public class XboxCorePcAchievementManager : AchievementProvider
    {
        private Queue<(string id, float progression)> _updateQueue;
        private bool _isProcessing = false;
        
        [ContextMenu("Initialize")]
        public override void Initialize()
        {
            _updateQueue = new Queue<(string id, float progression)>();
            MSGdk.Helpers.XblAchievementsGetAchievementsForTitleIdAsync();
            OnInitializationComplete?.Invoke();
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
            string id = _achievementMapper.GetMappedID(achievementIdentifier);
            if (id != null) UpdateAchievementProgressAsync(id, progression);

        }
        private async UniTask UpdateAchievementProgressAsync(string id, float progression)
        {
                 _updateQueue.Enqueue((id, progression));
                if (!_isProcessing)
                {
                    await ProcessQueue();
                }
        }
        private async UniTask ProcessQueue()
        {
            _isProcessing = true;
            while (_updateQueue.Count > 0)
            {
                var (id, progression) = _updateQueue.Dequeue();
                Debug.Log($"Processing Achievement Id {id} with progression: {progression}");
                MSGdk.Helpers.UnlockAchievementProgression(id, (uint)progression);
                await UniTask.Delay(TimeSpan.FromSeconds(2f));
            }
            _isProcessing = false;
        }
        
       
    }

}


  