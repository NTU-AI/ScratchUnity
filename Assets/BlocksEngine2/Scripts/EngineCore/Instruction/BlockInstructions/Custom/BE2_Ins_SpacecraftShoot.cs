using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_SpacecraftShoot : BE2_InstructionBase, I_BE2_Instruction
{
    //protected override void OnAwake()
    //{
    //
    //}

    //protected override void OnStart()
    //{
    //    
    //}

    public new void Function()
    {
        if (TargetObject is BE2_TargetObjectSpacecraft3D)
        {
            (TargetObject as BE2_TargetObjectSpacecraft3D).Shoot();
        }
        ExecuteNextInstruction();
    }

    public string Generator(BE2_Generator.programmingLanguages language)
    {
        string code = "";

        if (language.Equals(BE2_Generator.programmingLanguages.Python))
            code = "...\n";
        else if (language.Equals(BE2_Generator.programmingLanguages.Cpp))
            code = "...\n";

        return code;
    }
}
