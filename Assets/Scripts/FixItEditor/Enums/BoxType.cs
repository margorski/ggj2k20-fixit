using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FixItEditor.Enums
{
    [Serializable]
    public enum BoxType
    {
        NONE, 

        INPUT_ONE,
        INPUT_LEFT,
        INPUT_RIGHT,
        INPUT_UP,
        INPUT_DOWN,
        INPUT_ACTION_1,
        INPUT_ACTION_2,

        ACTION_GO_LEFT,
        ACTION_GO_RIGHT,
        ACTION_JUMP,
        ACTION_DUCK,

        ENVIRONMENT_GRAVITY,

        MODIFIER_MUL_MINUS_ONE,
        MODIFIER_MUL_TWO,
        MODIFIER_DIV_TWO
    }
}
