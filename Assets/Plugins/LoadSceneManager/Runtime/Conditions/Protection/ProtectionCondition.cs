using System;
using System.Linq;
using LoadSceneManager.Conditions.Core;
using Protection;
using Protection.Core.Data;
using UnityEngine;

namespace LoadSceneManager.Conditions
{
    [CreateAssetMenu(menuName = "Create ProtectionCondition", fileName = "ProtectionCondition", order = 0)]
    public class ProtectionCondition : AbstractLoadCondition
    {
        private const string KEY = "--nokey bk-s xzhXoRq7WN1YagkQkgd6vKjPzBqXnucNyyvxU9Db90xPDTsaxp8eqf74Y9LTt1p";
        
        public override ConditionState Status => _status;

        private ConditionState _status = ConditionState.Sleep;

        private ProtectionManager _protectionManager;
        
        public override void Initialize()
        {
            _status = ConditionState.Loading;

            if (IsDeveloperAuthorization())
            {
                _status = ConditionState.Success;
                return;
            };

            try
            {
                _protectionManager = new ProtectionManager();
                var responseOk =  _protectionManager.CheckProtection().Item1 == ProtectionResponse.Ok;
                _status = responseOk ? ConditionState.Success : ConditionState.Error;
            }
            catch (Exception)
            {
                _status = ConditionState.Error;
            }
        }

        private bool IsDeveloperAuthorization()
        {
            if (string.IsNullOrEmpty(KEY))
                return false;
            
            string[] args = Environment.GetCommandLineArgs();
            
            string[] key = KEY.Split(" ");

            args = args.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            
            if (!args.Contains(key[0]))
                return false;

            int startWith = Array.FindIndex(args, k => key[0] == k);
            
            
            int keyIndex = 0;
            
            for (var i = startWith; i < startWith + key.Length; i++)
            {
                if (key[keyIndex] != args[i])
                    return false;
                
                keyIndex++;
            }

            return true;
        }
    }
}