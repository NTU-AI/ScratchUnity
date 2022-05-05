using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Op_Or : BE2_InstructionBase, I_BE2_Instruction
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
    I_BE2_BlockSectionHeaderInput _input1;
    string _vs0;
    string _vs1;

    public new string Operation()
    {
        _input0 = Section0Inputs[0];
        _input1 = Section0Inputs[1];
        _vs0 = _input0.StringValue;
        _vs1 = _input1.StringValue;

        return (_vs0 == "1" || _vs0 == "true") || (_vs1 == "1" || _vs1 == "true")
        ? "1" : "0";
    }

    public string Generator(BE2_Generator.programmingLanguages language)
    {
        string code = "";

        I_BE2_BlockSectionHeaderInput __input0 = Section0Inputs[0];
        I_BE2_BlockSectionHeaderInput __input1 = Section0Inputs[1];
        string __vs0 = __input0.StringValue;
        string __vs1 = __input1.StringValue;

        if (language.Equals(BE2_Generator.programmingLanguages.Python))
            code = __vs0 + " or " + __vs1+"\n";
        else if (language.Equals(BE2_Generator.programmingLanguages.Cpp))
            code = "...\n";

        return code;
    }
}
