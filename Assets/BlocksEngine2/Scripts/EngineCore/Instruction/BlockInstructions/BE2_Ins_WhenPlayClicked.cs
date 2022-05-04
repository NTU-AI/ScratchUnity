using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BE2_Ins_WhenPlayClicked : BE2_InstructionBase, I_BE2_Instruction
{
    protected override void OnButtonPlay()
    {
        BlocksStack.IsActive = true;
    }

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

        if (language.Equals(BE2_Generator.programmingLanguages.Python)) {
            code = "" +
                "# -*- coding: utf-8 -*-\n" +
                "#!/usr/bin/env python\n" +
                "\n" +
                "import math\n" +
                "import time\n" +
                "improt rclpy\n" +
                "\n" +
                "# please, define your imports and variables here:\n"+
                "# B1 start\n"+
                "# B1 end\n\n"+

                "# please, define your definitions here:\n"+
                "# B2 start\n"+
                "# B2 end\n\n"+

                "# please, enter your code here:\n"+
                "# B3 start\n"+
                "# B3 end\n\n"+

                "# Blocks code here:\n"+
                "def main():\n" +
                "";
        }
        else if (language.Equals(BE2_Generator.programmingLanguages.Cpp))
            code = "...\n";

        return code;
    }
}
