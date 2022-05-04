using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_RepeatForever : BE2_InstructionBase, I_BE2_Instruction
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
        ExecuteSection(0);
    }

    public string Generator(BE2_Generator.programmingLanguages language)
    {
        string code = "";

        if (language.Equals(BE2_Generator.programmingLanguages.Python))
            code = "while (true):\n";
        else if (language.Equals(BE2_Generator.programmingLanguages.Cpp))
            code = "...\n";

        return code;
    }
}
