using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_RepeatUntil : BE2_InstructionBase, I_BE2_Instruction
{
    //protected override void OnAwake()
    //{
    //
    //}

    //protected override void OnStart()
    //{
    //    
    //}

    I_BE2_BlockSectionHeaderInput _input0;
    string _value;

    public new void Function()
    {
        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        if (_value != "1" && _value != "true")
        {
            ExecuteSection(0);
        }
        else
        {
            ExecuteNextInstruction();
        }
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
