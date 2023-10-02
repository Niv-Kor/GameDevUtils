﻿using UnityEngine;

namespace GameDevUtils.Math
{
    public static class NumericUtils
    {
        #region Constants
        private static readonly int[] DECIAML_VALUES = { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };
        private static readonly string[] ROMAN_VALUES = { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
        #endregion

        /// <summary>
        /// Pad a number with characters on its left.
        /// </summary>
        /// <param name="num">The number to pad</param>
        /// <param name="places">The total amount of characters in the string, including the number's digits</param>
        /// <param name="padder">The character with which to pad the number to its left (default '0')</param>
        /// <returns>A string containing the pad on the left and the specified number on the right.</returns>
        public static string PadNumber(long num, int places, char padder = '0') {
            int numDigits = CountDigits(num);
            int shiftRightItr = places - numDigits;
            string numStr = num.ToString();

            if (shiftRightItr <= 0) return numStr;
            else {
                for (int i = 0; i < shiftRightItr; i++)
                    numStr = padder + numStr;

                return numStr;
            }
        }

        /// <summary>
        /// Convert a decimal number to its roman numerals representation.
        /// </summary>
        /// <param name="number">A decimal number to convert</param>
        /// <returns>A string representation of the specified number using roman numerals.</returns>
        public static string Romanify(long number) {
            string romanStr = "";
            int i = DECIAML_VALUES.Length - 1;

            while (number > 0) {
                long div = number / DECIAML_VALUES[i];
                number %= DECIAML_VALUES[i];

                while (div-- > 0) romanStr += ROMAN_VALUES[i];
                i--;
            }

            return romanStr;
        }

        /// <summary>
        /// Count the amount of digits in a number.
        /// </summary>
        /// <param name="num">The number to check</param>
        /// <returns>The amount of digits in the specified number</returns>
        public static int CountDigits(long num) {
            int counter = 0;

            while (Mathf.Abs(num) > 0) {
                counter++;
                num /= 10;
            }

            return counter;
        }
    }
}