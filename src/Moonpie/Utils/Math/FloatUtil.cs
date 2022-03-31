#region Copyright
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
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
#endregion

namespace Moonpie.Utils.Math;

public static class FloatUtil
{
    public static bool IsEqual(this float a, float b, float tolerance = 0.001f) {
        return System.Math.Abs(a - b) < tolerance;
    }
    
    public static bool IsEqual(this float? a, float? b, float tolerance = 0.001f) {
        if (a == null && b == null) {
            return true;
        }
        
        if (a == null || b == null) {
            return false;
        }
        return System.Math.Abs(a.Value - b.Value) < tolerance;
    }

    public static bool IsGreater(this float a, float b) {
        return a > b;
    }

    public static bool IsLess(this float a, float b) {
        return a < b;
    }
}