using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runner.Utils
{
    public enum VibrateEnum
    {
        LongStrong, LongMedium, LongLight,
        ShortStrong, ShortMedium, ShortLight,
        ExtraLongStrong, ExtraLongMedium, ExtraLongLight,
        Fadein, Fadeout,
    }

    /// <summary>
    /// 手柄震动管理器: GamepadVibrateManager
    /// Author: 单元琛 2022-8-26
    /// </summary>
    public class GamepadVibrateManager : Singleton<GamepadVibrateManager>
    {
        public void Vibrate(VibrateEnum vibEnum)
        {
            switch (vibEnum)
            {
                case VibrateEnum.LongStrong:
                    StartCoroutine(SetGamepadVibration(0.75f, 1, 0.75f));
                    break;
                case VibrateEnum.LongMedium:
                    StartCoroutine(SetGamepadVibration(0.45f, 0.75f, 0.75f));
                    break;
                case VibrateEnum.LongLight:
                    StartCoroutine(SetGamepadVibration(0.1f, 0.3f, 0.75f));
                    break;
                case VibrateEnum.ShortStrong:
                    StartCoroutine(SetGamepadVibration(0.75f, 1, 0.25f));
                    break;
                case VibrateEnum.ShortMedium:
                    StartCoroutine(SetGamepadVibration(0.45f, 0.75f, 0.25f));
                    break;
                case VibrateEnum.ShortLight:
                    StartCoroutine(SetGamepadVibration(0.6f, 0.9f, 0.08f));
                    break;
                case VibrateEnum.ExtraLongStrong:
                    StartCoroutine(SetGamepadVibration(0.75f, 1, 1.25f));
                    break;
                case VibrateEnum.ExtraLongMedium:
                    StartCoroutine(SetGamepadVibration(0.45f, 0.75f, 1.25f));
                    break;
                case VibrateEnum.ExtraLongLight:
                    StartCoroutine(SetGamepadVibration(0.15f, 0.35f, 2.4f));
                    break;
                case VibrateEnum.Fadein:
                    break;
                case VibrateEnum.Fadeout:
                    break;
                default:
                    break;
            }
        }

        private IEnumerator SetGamepadVibration(float low, float high, float time)
        {
            StopVibrate();
            Gamepad.current?.SetMotorSpeeds(low, high);
            yield return new WaitForSecondsRealtime(time);
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }

        public static void StopVibrate()
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }
    }

}
