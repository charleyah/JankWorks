/**

https://github.com/tstavrianos/OpenGL

MIT License

Copyright (c) 2018 Theo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/



using System.Collections.Generic;

namespace OpenGL {
    public static class Features {
        internal static HashSet<string> ExtensionsGPU = new HashSet<string>();
        
        public static bool IsExtensionSupported(string ExtensionName) => ExtensionsGPU.Contains(ExtensionName);
        
        public static bool GL_VERSION_1_0 {get; internal set;} = false;
        public static bool GL_VERSION_1_1 {get; internal set;} = false;
        public static bool GL_VERSION_1_2 {get; internal set;} = false;
        public static bool GL_VERSION_1_3 {get; internal set;} = false;
        public static bool GL_VERSION_1_4 {get; internal set;} = false;
        public static bool GL_VERSION_1_5 {get; internal set;} = false;
        public static bool GL_VERSION_2_0 {get; internal set;} = false;
        public static bool GL_VERSION_2_1 {get; internal set;} = false;
        public static bool GL_VERSION_3_0 {get; internal set;} = false;
        public static bool GL_VERSION_3_1 {get; internal set;} = false;
        public static bool GL_VERSION_3_2 {get; internal set;} = false;
        public static bool GL_VERSION_3_3 {get; internal set;} = false;
        public static bool GL_VERSION_4_0 {get; internal set;} = false;
        public static bool GL_VERSION_4_1 {get; internal set;} = false;
        public static bool GL_VERSION_4_2 {get; internal set;} = false;
        public static bool GL_VERSION_4_3 {get; internal set;} = false;
        public static bool GL_VERSION_4_4 {get; internal set;} = false;
        public static bool GL_VERSION_4_5 {get; internal set;} = false;
        public static bool GL_VERSION_4_6 {get; internal set;} = false;
        public static bool GL_VERSION_ES_CM_1_0 {get; internal set;} = false;
        public static bool GL_ES_VERSION_2_0 {get; internal set;} = false;
        public static bool GL_ES_VERSION_3_0 {get; internal set;} = false;
        public static bool GL_ES_VERSION_3_1 {get; internal set;} = false;
        public static bool GL_ES_VERSION_3_2 {get; internal set;} = false;
        public static bool GL_SC_VERSION_2_0 {get; internal set;} = false;
    }
}
