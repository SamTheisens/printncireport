﻿using System;
using System.Reflection;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;


namespace Printer
{
    public class ProcessUtilities
    {
        /*** Imports ***/
        #region Imports

        [DllImport("advapi32.dll", EntryPoint = "AdjustTokenPrivileges", SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr in_hToken, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, UInt32 BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("advapi32.dll", EntryPoint = "OpenProcessToken", SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValue", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        [DllImport("userenv.dll", EntryPoint = "CreateEnvironmentBlock", SetLastError = true)]
        public static extern bool CreateEnvironmentBlock(out IntPtr out_ptrEnvironmentBlock, IntPtr in_ptrTokenHandle, bool in_bInheritProcessEnvironment);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("wtsapi32.dll", EntryPoint = "WTSQueryUserToken", SetLastError = true)]
        public static extern bool WTSQueryUserToken(UInt32 in_nSessionID, out IntPtr out_ptrTokenHandle);

        [DllImport("kernel32.dll", EntryPoint = "WTSGetActiveConsoleSessionId", SetLastError = true)]
        public static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("kernel32.dll")]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, string[] Arguments);

        [DllImport("Wtsapi32.dll", EntryPoint = "WTSQuerySessionInformation", SetLastError = true)]
        public static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport("wtsapi32.dll", EntryPoint = "WTSFreeMemory", SetLastError = false)]
        public static extern void WTSFreeMemory(IntPtr memory);

        [DllImport("userenv.dll", EntryPoint = "LoadUserProfile", SetLastError = true)]
        public static extern bool LoadUserProfile(IntPtr hToken, ref PROFILEINFO lpProfileInfo);

        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcessAsUser(IntPtr in_ptrUserTokenHandle, string in_strApplicationName, string in_strCommandLine, ref SECURITY_ATTRIBUTES in_oProcessAttributes, ref SECURITY_ATTRIBUTES in_oThreadAttributes, bool in_bInheritHandles, CreationFlags in_eCreationFlags, IntPtr in_ptrEnvironmentBlock, string in_strCurrentDirectory, ref STARTUPINFO in_oStartupInfo, ref PROCESS_INFORMATION in_oProcessInformation);

        #endregion //Imports

        /*** Delegates ***/

        /*** Structs ***/
        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint m_nLowPart;
            public uint m_nHighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public int m_nPrivilegeCount;
            public LUID m_oLUID;
            public int m_nAttributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROFILEINFO
        {
            public int dwSize;
            public int dwFlags;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpUserName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpProfilePath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpDefaultPath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpServerName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpPolicyPath;
            public IntPtr hProfile;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessID;
            public Int32 dwThreadID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        #endregion //Structs

        /*** Classes ***/

        /*** Enums ***/
        #region Enums

        public enum CreationFlags
        {
            CREATE_SUSPENDED = 0x00000004,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            CREATE_SEPARATE_WOW_VDM = 0x00000800,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
        }

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType
        }

        #endregion //Enums

        /*** Defines ***/
        #region Defines

        private const int TOKEN_QUERY = 0x08;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
        private const int SE_PRIVILEGE_ENABLED = 0x02;

        public const int ERROR_NO_TOKEN = 1008;
        public const int RPC_S_INVALID_BINDING = 1702;

        #endregion //Defines

        /*** Methods ***/
        #region Methods

        /*
             If you need to give yourself permissions to inspect processes for their modules,
             and create tokens without worrying about what account you're running under,
             this is the method for you :) (such as the token privilege "SeDebugPrivilege")
         */
        static public bool AdjustProcessTokenPrivileges(IntPtr in_ptrProcessHandle, string in_strTokenToEnable)
        {
            IntPtr l_hToken;
            LUID l_oRestoreLUID;
            TOKEN_PRIVILEGES l_oTokenPrivileges;

            Debug.Assert(in_ptrProcessHandle != IntPtr.Zero);

            //Get the process security token
            if (false == OpenProcessToken(in_ptrProcessHandle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out l_hToken))
            {
                return false;
            }

            //Lookup the LUID for the privilege we need
            if (false == LookupPrivilegeValue(String.Empty, in_strTokenToEnable, out l_oRestoreLUID))
            {
                return false;
            }

            //Adjust the privileges of the current process to include the new privilege
            l_oTokenPrivileges.m_nPrivilegeCount = 1;
            l_oTokenPrivileges.m_oLUID = l_oRestoreLUID;
            l_oTokenPrivileges.m_nAttributes = SE_PRIVILEGE_ENABLED;

            if (false == AdjustTokenPrivileges(l_hToken, false, ref l_oTokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
            {
                return false;
            }

            return true;
        }

        /*
            Start a process the simplest way you can imagine
        */
        static public int SimpleProcessStart(string in_strTarget, string in_strArguments)
        {
            var l_oProcess = new Process();
            Debug.Assert(l_oProcess != null);

            l_oProcess.StartInfo.FileName = in_strTarget;
            l_oProcess.StartInfo.Arguments = in_strArguments;

            if (l_oProcess.Start())
            {
                return l_oProcess.Id;
            }

            return -1;
        }

        /*
            All the magic is in the call to WTSQueryUserToken, it saves you changing DACLs,
            process tokens, pulling the SID, manipulating the Windows Station and Desktop
            (and its DACLs) - if you don't know what those things are, you're lucky and should
            be on your knees thanking God at this moment.
 
            DEV NOTE:  This method currently ASSumes that it should impersonate the user
                              who is logged into session 1 (if more than one user is logged in, each
                              user will have a session of their own which means that if user switching
                              is going on, this method could start a process whose UI shows up in
                              the session of the user who is not actually using the machine at this
                              moment.)
 
            DEV NOTE 2:    If the process being started is a binary which decides, based upon
                                the user whose session it is being created in, to relaunch with a
                                different integrity level (such as Internet Explorer), the process
                                id will change immediately and the Process Manager will think
                                that the process has died (because in actuality the process it
                                launched DID in fact die only that it was due to self-termination)
                                This means beware of using this service to startup such applications
                                although it can connect to them to alarm in case of failure, just
                                make sure you don't configure it to restart it or you'll get non
                                stop process creation ;)
        */
//        static public int 
        static public int CreateUIProcessForServiceRunningAsLocalSystem(string in_strTarget, string in_strArguments)
        {
            var l_oProcessInformation = new PROCESS_INFORMATION();
            int l_nProcessID;

            try
            {
                IntPtr l_ptrEnvironment;
                SECURITY_ATTRIBUTES l_oSecurityAttributes;
                STARTUPINFO l_oStartupInfo;
                IntPtr l_ptrUserToken = LoadUserProfile(out l_oSecurityAttributes, out l_ptrEnvironment, out l_oStartupInfo);
                if (l_ptrUserToken == IntPtr.Zero)
                    return SimpleProcessStart(in_strTarget, in_strArguments);

                if (!CreateProcessAsUser(l_ptrUserToken, in_strTarget, String.Empty, ref l_oSecurityAttributes, ref l_oSecurityAttributes, false, CreationFlags.CREATE_UNICODE_ENVIRONMENT, l_ptrEnvironment, null, ref l_oStartupInfo, ref l_oProcessInformation))
                {
                    //System.Diagnostics.EventLog.WriteEntry( "CreateProcessAsUser FAILED", FormatError(Marshal.GetLastWin32Error()).ToString() );
                    throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                                        "The call to CreateProcessAsUser failed, GetLastError returns: " +
                                        FormatError(Marshal.GetLastWin32Error()));
                }

                l_nProcessID = l_oProcessInformation.dwProcessID;
            }
            catch (Exception l_oException)
            {
                throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                                    "An unhandled exception was caught spawning the process, the exception was: " +
                                    l_oException.Message);
            }
            finally
            {
                if (l_oProcessInformation.hProcess != IntPtr.Zero)
                {
                    CloseHandle(l_oProcessInformation.hProcess);
                }
                if (l_oProcessInformation.hThread != IntPtr.Zero)
                {
                    CloseHandle(l_oProcessInformation.hThread);
                }
            }

            return l_nProcessID;
        }

        public static IntPtr LoadUserProfile(out SECURITY_ATTRIBUTES l_oSecurityAttributes, out IntPtr l_ptrEnvironment, out STARTUPINFO l_oStartupInfo)
        {
            PROFILEINFO l_oProfileInfo;
            string l_strActiveUserName;
            uint l_nActiveUserSessionId = GetActiveUser(out l_strActiveUserName);

            //We are supposedly running as a service so we're going to be running in session 0 so get a user token from the active user session
            IntPtr l_ptrUserToken = GetUserTokenPointer(l_nActiveUserSessionId);
            if (l_ptrUserToken == IntPtr.Zero)
            {
                l_oSecurityAttributes = new SECURITY_ATTRIBUTES();
                l_ptrEnvironment = IntPtr.Zero;
                l_oStartupInfo = new STARTUPINFO();
                return IntPtr.Zero;
            }
                

            //Create an appropriate environment block for this user token (if we have one)

            l_oSecurityAttributes = CreateEnvironmentBlock(l_ptrUserToken, out l_oStartupInfo, out l_oProfileInfo, l_strActiveUserName, out l_ptrEnvironment);

            //Remember, sometimes nobody is logged in (especially when we're set to Automatically startup)
            if (!LoadUserProfile(l_ptrUserToken, ref l_oProfileInfo))
            {
                throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                                    "The call to LoadUserProfile failed, GetLastError returns: " +
                                    FormatError(Marshal.GetLastWin32Error()));
            }
            return l_ptrUserToken;
        }

        private static uint GetActiveUser(out string l_strActiveUserName)
        {
            IntPtr l_ptrBuffer;
//The currently active user is running what session?
            uint l_nActiveUserSessionId = WTSGetActiveConsoleSessionId();
            //l_nActiveUserSessionId = 1;

            if (l_nActiveUserSessionId == 0xFFFFFFFF)
            {
                throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                                    "The call to WTSGetActiveConsoleSessionId failed,  GetLastError returns: " +
                                    FormatError(Marshal.GetLastWin32Error()));
            }

            uint l_nBytes;
            if (false == WTSQuerySessionInformation(IntPtr.Zero, (int)l_nActiveUserSessionId, WTS_INFO_CLASS.WTSUserName, out l_ptrBuffer, out l_nBytes))
            {
                int l_nLastError = Marshal.GetLastWin32Error();

                //On earlier operating systems from Vista, when no one is logged in, you get RPC_S_INVALID_BINDING which is ok, we just won't impersonate
                if (l_nLastError != RPC_S_INVALID_BINDING)
                {
                    throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                                        "The call to WTSQuerySessionInformation failed,  GetLastError returns: " +
                                        FormatError(Marshal.GetLastWin32Error()));
                }

                //No one logged in so let's just do this the simple way
                //return SimpleProcessStart(in_strTarget, in_strArguments);
            }

            l_strActiveUserName = Marshal.PtrToStringAnsi(l_ptrBuffer);
            WTSFreeMemory(l_ptrBuffer);
            return l_nActiveUserSessionId;
        }

        private static SECURITY_ATTRIBUTES CreateEnvironmentBlock(IntPtr l_ptrUserToken, out STARTUPINFO l_oStartupInfo, out PROFILEINFO l_oProfileInfo, string l_strActiveUserName, out IntPtr l_ptrEnvironment)
        {
            var l_oSecurityAttributes = new SECURITY_ATTRIBUTES();
            l_oStartupInfo = new STARTUPINFO();
            l_oProfileInfo = new PROFILEINFO();
            l_ptrEnvironment = IntPtr.Zero;
            if (l_ptrUserToken == IntPtr.Zero)
                return new SECURITY_ATTRIBUTES();

            if (!CreateEnvironmentBlock(out l_ptrEnvironment, l_ptrUserToken, false))
            {
                throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                                    "The call to CreateEnvironmentBlock failed, GetLastError returns: " +
                                    FormatError(Marshal.GetLastWin32Error()));
            }
            l_oSecurityAttributes.Length = Marshal.SizeOf(l_oSecurityAttributes);
            l_oStartupInfo.cb = Marshal.SizeOf(l_oStartupInfo);

            //DO NOT set this to "winsta0\\default" (even though many online resources say to do so)
            l_oStartupInfo.lpDesktop = String.Empty;
            l_oProfileInfo.dwSize = Marshal.SizeOf(l_oProfileInfo);
            l_oProfileInfo.lpUserName = l_strActiveUserName;
            return l_oSecurityAttributes;
        }

        private static IntPtr GetUserTokenPointer(uint l_nActiveUserSessionId)
        {
            IntPtr l_ptrUserToken;
            if (false == WTSQueryUserToken(l_nActiveUserSessionId, out l_ptrUserToken))
            {
                int l_nLastError = Marshal.GetLastWin32Error();

                //Remember, sometimes nobody is logged in (especially when we're set to Automatically startup) you should get error code 1008 (no user token available)
                if (ERROR_NO_TOKEN != l_nLastError)
                {
                    //return IntPtr.Zero;
                    //Ensure we're running under the local system account
                    WindowsIdentity l_oIdentity = WindowsIdentity.GetCurrent();

                    //if (l_oIdentity != null)
                    //    if ("NT AUTHORITY\\SYSTEM" != l_oIdentity.Name)
                    //    {
                    //        throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" +
                    //                            "The call to WTSQueryUserToken failed and querying the process' account identity results in an identity which does not match 'NT AUTHORITY\\SYSTEM' but instead returns the name:" +
                    //                            l_oIdentity.Name + "  GetLastError returns: " + l_nLastError);
                    //    }

                    //throw new Exception("ProcessUtilities" + "->" + MethodBase.GetCurrentMethod().Name + "->" + "The call to WTSQueryUserToken failed, GetLastError returns: " + l_nLastError);
                }

                //No one logged in so let's just do this the simple way
                //return SimpleProcessStart(in_strTarget, in_strArguments);
            }
            return l_ptrUserToken;
        }

        private static string FormatError(int errorCode)
        {
            const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
            const uint FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;

            // System error message

            var formattedMessage = new StringBuilder(100);
            var args = new[] { "aa" };

            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ARGUMENT_ARRAY, IntPtr.Zero, (uint)errorCode, 0,
                          formattedMessage, 100, args);
            return formattedMessage.ToString();
        }
        #endregion //Methods
    }
}