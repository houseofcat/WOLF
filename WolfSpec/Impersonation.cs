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

        public bool Success = false;

        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        public Impersonation(string d, string u, string p)
        {
            try
            {
                Success = LogonUser(u, d, p, LOGON32_LOGON_NEW_CREDENTIALS, 0, out _handle );

                if ( Success )
                {
                    _context = WindowsIdentity.Impersonate( _handle.DangerousGetHandle());
                }
            }
            catch (UnauthorizedAccessException UAE)
            {
                MessageBox.Show("Impersonation: Unauthorized Access Exception Occurred.\n\nMessage: " + UAE.Message + "\n\nStack: " + UAE.StackTrace);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impersonation: Unknown Exception Occurred.\n\nMessage: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _handle.Dispose();
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser( string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

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
