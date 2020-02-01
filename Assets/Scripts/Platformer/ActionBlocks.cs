using Assets.Scripts.FixItEditor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Platformer
{
    public static class ActionBlocks
    {
        private static float InputOneAction(float input) { return 1; }
        private static float InputLeftAction(float input) { return Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0; }
        private static float InputRightAction(float input) { return Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0; }
        private static float InputUpAction(float input) { return Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0; }
        private static float InputDownAction(float input) { return Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0; }
        private static float InputAction1Action(float input) { return Input.GetKeyDown(KeyCode.Space) ? 1 : 0; }
        private static float InputAction2Action(float input) { return Input.GetKeyDown(KeyCode.LeftAlt) ? 1 : 0; }

        private static float ModifierMulMinusOneAction(float input) { return input * -1.0f; }
        private static float ModifierMulTwoAction(float input) { return input * 2.0f; }
        private static float ModifierDivTwoAction(float input) { return input / 2.0f; }

        private static float DoNothingAction(float input) { return input;  }

        public delegate float ActionDelegate(float input);
        public static Dictionary<BoxType, ActionDelegate> BoxActions = new Dictionary<BoxType, ActionDelegate>()
        {
            { BoxType.NONE, DoNothingAction },

            { BoxType.INPUT_ONE, InputOneAction },
            { BoxType.INPUT_LEFT, InputLeftAction },
            { BoxType.INPUT_RIGHT, InputRightAction },

            { BoxType.INPUT_UP, InputUpAction },
            { BoxType.INPUT_DOWN, InputDownAction },
            { BoxType.INPUT_ACTION_1, InputAction1Action },
            { BoxType.INPUT_ACTION_2, InputAction2Action },

            { BoxType.MODIFIER_MUL_MINUS_ONE, ModifierMulMinusOneAction },
            { BoxType.MODIFIER_MUL_TWO, ModifierMulTwoAction },
            { BoxType.MODIFIER_DIV_TWO, ModifierDivTwoAction },

            { BoxType.ACTION_GO_LEFT, DoNothingAction },
            { BoxType.ACTION_GO_RIGHT, DoNothingAction },
            { BoxType.ACTION_JUMP, DoNothingAction },
            { BoxType.ACTION_DUCK, DoNothingAction },
            { BoxType.ENVIRONMENT_GRAVITY, DoNothingAction }
        };
    }
}
