using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
    public class FileDialog : MonoBehaviour
    {
        public string SaveFileDialog(string title, string defaultName, string filter)
        {
#if UNITY_EDITOR
            return EditorUtility.SaveFilePanel(title, "", defaultName, filter);
#elif UNITY_STANDALONE_WIN
            return SaveFileDialogWindows(title, defaultName, filter);
#elif UNITY_STANDALONE_OSX
            return SaveFileDialogMac(title, defaultName, filter);
#elif UNITY_STANDALONE_LINUX
            return SaveFileDialogLinux(title, defaultName, filter);
#else
            throw new PlatformNotSupportedException("This platform is not supported.");
#endif
        }

#if UNITY_STANDALONE_WIN
        [DllImport("Comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetSaveFileName([In, Out] OPENFILENAME ofn);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct OPENFILENAME
        {
            public int lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public string lpstrFilter;
            public string lpstrCustomFilter;
            public int nMaxCustFilter;
            public int nFilterIndex;
            public string lpstrFile;
            public int nMaxFile;
            public string lpstrFileTitle;
            public int nMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int Flags;
            public short nFileOffset;
            public short nFileExtension;
            public string lpstrDefExt;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;
            public IntPtr pvReserved;
            public int dwReserved;
            public int FlagsEx;
        }

        private string SaveFileDialogWindows(string title, string defaultName, string filter)
        {
            OPENFILENAME ofn = new OPENFILENAME();
            ofn.lStructSize = Marshal.SizeOf(ofn);
            ofn.hwndOwner = GetActiveWindow();
            ofn.lpstrFilter = filter;
            ofn.lpstrFile = new string(new char[256]);
            ofn.nMaxFile = ofn.lpstrFile.Length;
            ofn.lpstrDefExt = System.IO.Path.GetExtension(defaultName);
            ofn.lpstrFileTitle = defaultName;
            ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
            ofn.lpstrTitle = title;
            ofn.Flags = 0x00001000 | 0x00000002; // OFN_EXPLORER | OFN_OVERWRITEPROMPT

            if (GetSaveFileName(ofn))
            {
                return ofn.lpstrFile;
            }

            return null;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();
#endif

#if UNITY_STANDALONE_OSX
        [DllImport("__Internal")]
        private static extern IntPtr NSSavePanel();

        [DllImport("__Internal")]
        private static extern void NSSavePanelRelease(IntPtr panel);

        [DllImport("__Internal")]
        private static extern bool NSSavePanelRunModal(IntPtr panel);

        [DllImport("__Internal")]
        private static extern IntPtr NSSavePanelURL(IntPtr panel);

        [DllImport("__Internal")]
        private static extern void NSSavePanelSetAllowedFileTypes(IntPtr panel, IntPtr[] types);

        [DllImport("__Internal")]
        private static extern void NSSavePanelSetNameFieldStringValue(IntPtr panel, string name);

        private string SaveFileDialogMac(string title, string defaultName, string filter)
        {
            IntPtr panel = NSSavePanel();
            try
            {
                string[] filters = filter.Split('|');
                IntPtr[] types = new IntPtr[filters.Length / 2];
                for (int i = 0; i < types.Length; i++)
                {
                    types[i] = Marshal.StringToHGlobalUni(filters[i * 2 + 1].TrimStart('*').TrimEnd(';'));
                }
                NSSavePanelSetAllowedFileTypes(panel, types);
                NSSavePanelSetNameFieldStringValue(panel, defaultName);

                if (NSSavePanelRunModal(panel))
                {
                    IntPtr url = NSSavePanelURL(panel);
                    if (url != IntPtr.Zero)
                    {
                        return Marshal.PtrToStringAuto(url);
                    }
                }
            }
            finally
            {
                NSSavePanelRelease(panel);
            }
            return null;
        }
#endif

#if UNITY_STANDALONE_LINUX
        private string SaveFileDialogLinux(string title, string defaultName, string filter)
        {
            string scriptPath = "/tmp/SaveFileDialog.sh";
            string script = $"#!/bin/bash\nzenity --file-selection --save --title=\"{title}\" --filename=\"{defaultName}\" --file-filter='{filter}'";

            System.IO.File.WriteAllText(scriptPath, script);
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = scriptPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadLine();
            process.WaitForExit();
            System.IO.File.Delete(scriptPath);
            return string.IsNullOrEmpty(result) ? null : result;
        }
#endif
    }
}
