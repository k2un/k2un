using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Info
{
    #region 델리게이트
    /// <summary>
    /// 레시피의 상태가 변경되었을때 사용하는 함수 형태 정의
    /// </summary>
    /// <param name="recipe">원본 레시피 클래스</param>
    /// <param name="action">상태 변경 상황 상수</param>
    public delegate void EventHandlerProcessProgram(ProcessProgram recipe, Action action);
    #endregion

    #region Constant 클래스, 스테틱 함수 정의에 쓰인다.
    class Constant
    {
        static System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        static Constant() { }
        static public string ProcessStateToString(ProcessState state)
        {
            string bit = "0000000";
            switch (state)
            {
                case Info.ProcessState.IDLE:
                    bit = "0100000";
                    break;
                case Info.ProcessState.EXECUTE:
                    bit = "0000100";
                    break;
                case Info.ProcessState.DISABLE:
                    bit = "0000001";
                    break;
                case Info.ProcessState.PAUSE:
                    bit = "0000010";
                    break;
                case Info.ProcessState.SETUP:
                    bit = "0010000";
                    break;
                default:
                    break;
            }
            return bit;
        }
    }
    #endregion
    
    #region enum정의
    /// <summary>
    /// 이벤트의 상태를 나타낸다.
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// 생성
        /// </summary>
        CREATE,
        /// <summary>
        /// 수정
        /// </summary>
        MODIFY,
        /// <summary>
        /// 삭제
        /// </summary>
        DELETE,
        /// <summary>
        /// 초기상태
        /// </summary>
        NONE
    }

    /// <summary>
    /// 레시피 타입을 나타낸다. 두 그룹으로 나뉠수 있는데 1그룹은 타입0, 2그룹은 타입1,2 이다.
    /// </summary>
    public enum PPIDType
    {
        /// <summary>
        /// EQP PPID, 종속형에 쓰인다.
        /// </summary>
        TYPE_0,
        /// <summary>
        /// EQP PPID, 독립형(인라인)에 쓰인다. 타입2와 그룹이다.
        /// </summary>
        TYPE_1,
        /// <summary>
        /// HOST PPID, 독립형(인라인)에 쓰인다. 타입1과 그룹이다. 
        /// </summary>
        TYPE_2
    }

    public enum ProcessState
    {
        IDLE,
        SETUP,
        EXECUTE,
        PAUSE,
        DISABLE
    }

    public enum EQPState
    {
        NORMAL,
        PM,
        FAULT
    }
    #endregion
}
