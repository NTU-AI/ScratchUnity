using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Generator : MonoBehaviour
{
    public enum programmingLanguages
    {
        Python,
        Cpp
    };

    public programmingLanguages generatorLanguage;

    public programmingLanguages GetGeneratorLanguage()
    {
        return generatorLanguage;
    }

}
