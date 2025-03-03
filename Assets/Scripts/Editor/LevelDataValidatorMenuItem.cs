using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelDataValidatorMenuItem
{
    [MenuItem("Assets/Validate All Level Files")]
    private static void ValidatorMenuItem()
    {
        LevelDataValidator.ValidateAllLevelFiles();
    }
}
