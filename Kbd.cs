﻿// =============================================================================
// MIT License
//
// Copyright (c) [2018] [Valeriya Pudova]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace VARP.Keyboard
{
    /// <summary>
    /// This class allow to convert string expression to the 
    /// keycode sequence. The sequence is the array of integers.
    /// Each character is the 32 bits word.
    /// </summary>
    public static class Kbd
    {
        /// <summary>
        /// Convert the keycode sequence to the string
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ConvertToString(int[] sequence, string separator = null)
        {
            UnityEngine.Debug.Assert(sequence != null);
            return ConvertToString(sequence, 0, sequence.Length, separator);
        }
        /// <summary>
        /// Convert the keycode sequence to the string
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="starts"></param>
        /// <param name="quantity"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ConvertToString(int[] sequence, int starts, int quantity, string separator = null)
        {
            UnityEngine.Debug.Assert(sequence != null);
            UnityEngine.Debug.Assert(starts < sequence.Length);
            UnityEngine.Debug.Assert(starts + quantity - 1 < sequence.Length);

            if (sequence.Length == 0)
                return string.Empty;

            var idx = starts;
            var cnt = 0;

            var sb = new StringBuilder();
            if (separator != null)
            {
                var addSeparator = false;
                while (cnt < quantity)
                {
                    if (addSeparator) sb.Append(separator);
                    sb.Append(Event.GetName(sequence[idx++]));
                    addSeparator = true;
                    cnt++;
                }
            }
            else
            {
                while (cnt < quantity)
                {
                    sb.Append(Event.GetName(sequence[idx++]));
                    cnt++;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Convert the keycode sequence to the string
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static string[] ConvertToStringList(int[] sequence)
        {
            UnityEngine.Debug.Assert(sequence != null);

            var result = new string[sequence.Length];
            if (sequence.Length == 0)
                return result;
            var i = 0;
            foreach (var code in sequence)
                result[i++] = Event.GetName(code);
            return result;
        }

        /// <summary>
        /// Supports multiple or single tokens
        /// When multiple tokens each one separated with ' ' space
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Event [] ParseSequence([NotNull] string expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (expression == string.Empty) throw new ArgumentException(nameof(expression));

            var sequence = new List<Event>();
            var tags = expression.Split(' ');

            foreach (var s in tags)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                var evt = Event.ParseExpression(s);

                if (evt >= 0)
                {
                    sequence.Add(evt);
                }
                else
                {
                    // This case will be translated as string
                    // "abcd" => "abcd"
                    foreach (var c in s)
                    {
                        sequence.Add(c);
                    }
                }
            }
            return sequence.ToArray();
        }
        /// <summary>
        /// Convert list of strings to list of events
        /// </summary>
        /// <param name="sequence">Array of strings</param>
        /// <returns>Array of events</returns>
        public static Event[] ParsePseudo(string[] sequence)
        {
            var idx = 0;
            var result = new Event[sequence.Length];
            foreach (var s in sequence)
                result[idx++] = Event.GetPseudoCode(s);
            return result;
        }
    }
}