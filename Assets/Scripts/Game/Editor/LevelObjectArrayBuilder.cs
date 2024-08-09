using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using LevelsRelated;

namespace EditorLogics
{
    [CustomEditor(typeof(LevelObjectArray))]
    public class LevelObjectArrayBuilder : ObjectArrayBuilder
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelObjectArray = (LevelObjectArray)target;

            if (GUILayout.Button("Create Levels"))
            {
                ClearObjects(levelObjectArray.transform);

                for (int i = 0; i < levelObjectArray.LevelsAmount; i++)
                {
                    var levelGo = Instantiate(levelObjectArray.LevelTemplatePrefab, levelObjectArray.transform);

                    CreateLevel(levelObjectArray.LevelStartPrefab,
                        levelObjectArray.Prefabs, levelObjectArray.Direction, levelObjectArray.Amount, levelObjectArray.Randomize, levelGo.transform,
                        levelObjectArray.FlagsPrefab,
                        levelObjectArray.GatesPrefabs);

                    AssignLevelStats(levelGo);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateLevel(GameObject startLevelPartPrefab,
            GameObject[] levelPartsPrefabs, Vector3 direction, int levelPartsAmount, bool levelRandomize, Transform levelParent,
            GameObject flagsPrefab, 
            GameObject[] gatesPartsPrefabs)
        {
            var startLevelPartPrefabArray = new GameObject[1] { startLevelPartPrefab };
            var flagsPartPrefabArray = new GameObject[1] { flagsPrefab };

            var curPartPos = CreateArrayAndGetLastPosition(startLevelPartPrefabArray, direction, 1, false, levelParent);

            CreateLevelsPartsAndFlags(levelPartsPrefabs, direction, levelPartsAmount, levelRandomize, levelParent, flagsPartPrefabArray, ref curPartPos);

            CreateArrayAndGetLastPosition(gatesPartsPrefabs, direction, gatesPartsPrefabs.Length, false, levelParent, curPartPos);
        }


        private Vector3 CreateArrayAndGetLastPosition(GameObject[] prefabs, Vector3 direction, int n, bool randomize, Transform parent)
        {
            return CreateArrayAndGetLastPosition(prefabs, direction, n, randomize, parent, Vector3.zero);
        }
        private Vector3 CreateArrayAndGetLastPosition(GameObject[] prefabs, Vector3 direction, int n, bool randomize, Transform parent, Vector3 startPos)
        {
            int prefabsN = prefabs.Length;

            var objectsInfos = GetObjectArrayInfo(prefabs, prefabsN, direction);
            return SpawnArrayObjectsAndGetLastPosition(objectsInfos, prefabsN, n, randomize, parent, startPos);
        }
        private Vector3 SpawnArrayObjectsAndGetLastPosition(ObjectArrayPrefabInfo[] objectsInfos, int prefabsN, int n, bool randomize, Transform parent, Vector3 startPos)
        {
            var curObjectInfo = objectsInfos[0];

            for (int i = 0; i < n; i++)
            {
                curObjectInfo = objectsInfos[randomize ? Random.Range(0, prefabsN) : i % prefabsN];

                var pos = curObjectInfo.ScaledDirection * i + curObjectInfo.GetPos() + startPos;
                var rot = curObjectInfo.GetRotation();

                var gO = (GameObject)PrefabUtility.InstantiatePrefab(curObjectInfo.Prefab, parent);
                gO.transform.SetPositionAndRotation(pos, rot);
            }

            return curObjectInfo.ScaledDirection + parent.GetChild(parent.childCount - 1).position;
        }

        private void CreateLevelsPartsAndFlags(GameObject[] levelPartsPrefabs, Vector3 direction, int levelPartsAmount, bool levelRandomize, Transform levelParent,
            GameObject[] flagsPartPrefabArray, ref Vector3 curPartPos)
        {
            for (int i = 0; i < levelPartsAmount / 3; i++)
            {
                curPartPos = CreateArrayAndGetLastPosition(levelPartsPrefabs, direction, 3, levelRandomize, levelParent, curPartPos);
                curPartPos = CreateArrayAndGetLastPosition(flagsPartPrefabArray, direction, 1, false, levelParent, curPartPos);
            }

            int remainingParts = levelPartsAmount % 3;
            if (remainingParts > 0)
                curPartPos = CreateArrayAndGetLastPosition(levelPartsPrefabs, direction, remainingParts, levelRandomize, levelParent, curPartPos);

        }

        private void AssignLevelStats(GameObject levelGo)
        {
            var level = levelGo.GetComponent<Level>();

            foreach (Transform childTransform in levelGo.transform.GetChild(0).transform)
            {
                if (childTransform.name == "PlayerSpawn")
                {
                    level.PlayerSpawnPoint = childTransform; break;
                }
            }

            List<float> interactablesZPositionsList = new();
            foreach (var interactable in levelGo.GetComponentsInChildren<Interactable>())
            {
                var influenceVal = interactable.GetInfluenceValue();

                if (influenceVal > 0)
                {
                    float curZPos = interactable.transform.position.z;

                    if (ZPositionIsReachable(interactablesZPositionsList, curZPos))
                    {
                        level.MaxMoney += influenceVal;

                        interactablesZPositionsList.Add(curZPos);
                    }
                }
            }
        }
        private bool ZPositionIsReachable(List<float> listZPositions, float curZPos)
        {
            foreach (var listZPos in listZPositions)
                if (Mathf.Abs(listZPos - curZPos) < 0.1f) 
                    return false;

            return true;
        }

    }
}
