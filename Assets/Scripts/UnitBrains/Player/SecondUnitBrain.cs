using System.Collections.Generic;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;

            if (GetTemperature() >= overheatTemperature)
            {
                return; //Выход из метода, если уровень перегрева равен или выше допустимого
            }

            ///////////////////////////////////////
            // Homework 1.3 (1st block, 3rd module)
            ///////////////////////////////////////   
            for (int i = 0; i <= GetTemperature(); i++)
            {
                var projectile = CreateProjectile(forTarget);

                AddProjectileToList(projectile, intoList);
            }

            IncreaseTemperature(); //Увеличение уровня перегрева при выстреле
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        {
            ///////////////////////////////////////
            // Homework 1.4 (1st block, 4rd module)
            ///////////////////////////////////////
            List<Vector2Int> result = GetReachableTargets();

            float minDistance = float.MaxValue; //Буферная переменная для самой короткой дистанции до базы
            Vector2Int target = Vector2Int.zero; //Буферная переменная для хранения цели, которая ближе всего к базе

            foreach (var obj in result) 
            {
                float distance = DistanceToOwnBase(obj);
                //Если дистанция цели меньше предыдущей, сохраняю дистанцию и цель
                if (minDistance < distance)
                {
                    target = obj;
                    minDistance = distance;
                }
            }
            
            //Очистка списка
            while (result.Count > 1)
            {
                result.RemoveAt(result.Count - 1);
            }

            //Проверяю, найдена ближайщая цель, через сравнение дистанции с максимальным значением типа данных float
            if (minDistance != float.MaxValue)
            {
                result.Add(target); //Добавление в список, самую ближайщую цель к базе
            }

            return result;
            ///////////////////////////////////////
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}