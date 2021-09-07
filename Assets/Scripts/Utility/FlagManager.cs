using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utility
{
    public static class BitflagUtility 
    {


        static public int AppendFlag(ref int baseFlag, params int[] flags)
        {
            for(int i = 0;i < flags.Length;i++)
            {
                baseFlag |= flags[i];
            }


            return baseFlag;
        }

        ///<summary>第一引数からそれに続く要素を消す。
        ///</summary>
        static public int RemoveFlag(ref int baseFlag, params int[] flags)
        {
            for(int i = 0;i < flags.Length;i++)
            {
                baseFlag &= ~flags[i];
            }

            return baseFlag;
        }


    }
}