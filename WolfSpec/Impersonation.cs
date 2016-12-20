using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace Wolf
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class Impersonation : IDisposable
    {
        private readonly SafeTokenHandle _handle;
        private readonly WindowsImpersonationContext _context;

        public Boolean ImpersonationSucceeded = false;

        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        public Impersonation(string domain, string username, string password)
        {
            try
            {
                ImpersonationSucceeded = LogonUser(username, domain, password,
                           LOGON32_LOGON_NEW_CREDENTIALS, 0, out this._handle);

                if (ImpersonationSucceeded)
                {
                    this._context = WindowsIdentity.Impersonate(this._handle.DangerousGetHandle());
                }
            }
            catch (UnauthorizedAccessException UAE)
            {
                MessageBox.Show("Impersonation: Unauthorized Access Exception Occurred.\n\nMessage: " + UAE.Message + "\n\nStack: " + UAE.StackTrace);

                ImpersonationSucceeded = false;
            }
            catch (Exception EX)
            {
                MessageBox.Show("Impersonation: Unknown Exception Occurred.\n\nMessage: " + EX.Message + "\n\nStack: " + EX.StackTrace);

                ImpersonationSucceeded = false;
            }
        }

        public void Dispose()
        {
            this._context.Dispose();
            this._handle.Dispose();
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true) { }

            [DllImport("kernel32.dll")]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [SuppressUnmanagedCodeSecurity]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool CloseHandle(IntPtr handle);

            protected override bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }
        }
    }
}
