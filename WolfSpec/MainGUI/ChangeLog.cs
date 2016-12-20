using System.Drawing;
using System.Windows.Forms;

namespace Wolf
{
    public partial class ChangeLog : Form
    {
        public ChangeLog()
        {
            InitializeComponent();
            loadComments();

            SetDoubleBuffered(dgvChangeLog);
            Color color = ColorTranslator.FromHtml("#404040");
            dgvChangeLog.DefaultCellStyle.BackColor = color;
            dgvChangeLog.DefaultCellStyle.ForeColor = Color.White;
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            try
            {
                //Taxes: Remote Desktop Connection and painting
                //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
                if (!(System.Windows.Forms.SystemInformation.TerminalServerSession))
                {

                    System.Reflection.PropertyInfo aProp =
                      typeof(System.Windows.Forms.Control).GetProperty(
                            "DoubleBuffered",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance);

                    aProp.SetValue(c, true, null);
                }
            }
            catch { }
        }
        public void loadComments()
        {
            //Version 0.0.1 First Release
            dgvChangeLog.Rows.Add("001", "0.0.1", "11/26/2013", "First Release");
            dgvChangeLog.Rows.Add("002", "0.0.1", "11/26/2013", "Display Basic User Information");
            dgvChangeLog.Rows.Add("003", "0.0.1", "11/26/2013", "Display Basic OS Information");
            dgvChangeLog.Rows.Add("004", "0.0.1", "11/26/2013", "Displays Active Network Connection Information");
            dgvChangeLog.Rows.Add("005", "0.0.1", "11/26/2013", "Tools: Gets and Displays External IP");

            //Version 0.0.2
            dgvChangeLog.Rows.Add("006", "0.0.2", "11/27/2013", "Backend: More Robust/Efficient Code");
            dgvChangeLog.Rows.Add("007", "0.0.2", "11/27/2013", "Backend: Use of Backgroundworkers/Multi-threading");
            dgvChangeLog.Rows.Add("008", "0.0.2", "11/27/2013", "GUI Changes");
            dgvChangeLog.Rows.Add("009", "0.0.2", "11/27/2013", "Display Active Network Connection Adapter in tree view.");
            dgvChangeLog.Rows.Add("010", "0.0.2", "11/27/2013", "Fix: Fixed an exception raised when network adapter is manually\n disabled during an External IP check is running.");

            //Version 0.0.3 - Day after Thanksgiving Release
            dgvChangeLog.Rows.Add("011", "0.0.3", "11/29/2013", "A couple of UI improvements");
            dgvChangeLog.Rows.Add("012", "0.0.3", "11/29/2013", "Getting external IP was too slow by my standards, so I increased the speed by 8 seconds.");
            dgvChangeLog.Rows.Add("013", "0.0.3", "11/29/2013", "Added CPU information gathering.");
            dgvChangeLog.Rows.Add("014", "0.0.3", "11/29/2013", "Back-end code established and some further user input error catches.");

            //Non-public releases
            dgvChangeLog.Rows.Add("015", "0.0.4", "N/A", "No public release.");
            dgvChangeLog.Rows.Add("016", "0.0.5", "N/A", "No public release.");
            dgvChangeLog.Rows.Add("017", "0.0.6", "N/A", "No public release.");

            //Version 0.0.7
            dgvChangeLog.Rows.Add("018", "0.0.7", "12/3/2013", "GUI revamped");
            dgvChangeLog.Rows.Add("019", "0.0.7", "12/3/2013", "Menu system included, but non-functional as of yet.");
            dgvChangeLog.Rows.Add("020", "0.0.7", "12/3/2013", "Moved forward with hardware information gathering.");

            //Version 0.0.8
            dgvChangeLog.Rows.Add("021", "0.0.8", "12/5/2013", "Rebuilt the whole program");
            dgvChangeLog.Rows.Add("022", "0.0.8", "12/5/2013", "Adjusted response time to certain functions, CPU info does take a hitch to collect.");
            dgvChangeLog.Rows.Add("023", "0.0.8", "12/5/2013", "Switched to MVC programming style so it’s more OOP, instead of Sequential.");
            dgvChangeLog.Rows.Add("024", "0.0.8", "12/5/2013", "We are now fully multi-threaded, and will continue this route going forward.");
            dgvChangeLog.Rows.Add("025", "0.0.8", "12/5/2013", "Added more CPU Info.");
            dgvChangeLog.Rows.Add("026", "0.0.8", "12/5/2013", "Memory Module outputs are in the Debug Tab if curious.");

            //Version 0.0.9
            dgvChangeLog.Rows.Add("027", "0.0.9", "12/5/2013", "Functionality added to Main Menu (somewhat ;))");
            dgvChangeLog.Rows.Add("028", "0.0.9", "12/5/2013", "Shortcuts added to the new tools.");
            dgvChangeLog.Rows.Add("029", "0.0.9", "12/5/2013", "Added a tool tab.");
            dgvChangeLog.Rows.Add("030", "0.0.9", "12/5/2013", "Added two tools, Launch a CMDPrompt as Admin, Refresh Network Adapter Connections (ipconfig /release/renew).");

            //Version 0.1.0 - Milestone 1
            dgvChangeLog.Rows.Add("031", "0.1.0", "12/5/2013", "MILESTONE 1 REACHED.");
            dgvChangeLog.Rows.Add("032", "0.1.0", "12/5/2013", "Functionality added to Main Menu (somewhat ;))");
            dgvChangeLog.Rows.Add("033", "0.1.0", "12/5/2013", "Shortcuts added to the new tools.");

            //Version 0.1.1
            dgvChangeLog.Rows.Add("034", "0.1.1", "12/5/2013", "Fixed a few typos.");
            dgvChangeLog.Rows.Add("035", "0.1.1", "12/5/2013", "Fixed a LocSecurity Policy error. Should not have been erroring out, giving a File Not Found exception.");
            dgvChangeLog.Rows.Add("036", "0.1.1", "12/5/2013", "Converted to the build to 64-bit.");

            //Version 0.1.2
            dgvChangeLog.Rows.Add("037", "0.1.2", "12/6/2013", "Fixed a Configuration Manager shortcut exception.");
            dgvChangeLog.Rows.Add("038", "0.1.2", "12/6/2013", "Support now for Multiple Processors, and non-CPU co-processors.");
            dgvChangeLog.Rows.Add("039", "0.1.2", "12/6/2013", "Updated the CPU tab. It is now the Processor Tab, now that it supports non-CPUs.");
            dgvChangeLog.Rows.Add("040", "0.1.2", "12/6/2013", "Memory Tab now has the RAW data output from Windows for each RAM module.");
            dgvChangeLog.Rows.Add("041", "0.1.2", "12/6/2013", "General UI / Design improvements!");

            //Version 0.1.3
            dgvChangeLog.Rows.Add("042", "0.1.3", "12/9/2013", "Minor cosmetic design changes.");
            dgvChangeLog.Rows.Add("043", "0.1.3", "12/9/2013", "Memory info gathering progress.");
            dgvChangeLog.Rows.Add("044", "0.1.3", "12/9/2013", "Unified to a single Event Handler.");
            dgvChangeLog.Rows.Add("045", "0.1.3", "12/9/2013", "Added a new tool: Windows Core Parking, Status, Enable/Disable functionality.");
            dgvChangeLog.Rows.Add("046", "0.1.3", "12/9/2013", "Began use of a debug log to aid troubleshooting potential issues in the future.");

            //Version 0.1.4
            dgvChangeLog.Rows.Add("047", "0.1.4", "12/10/2013", "Minor cosmetic design changes.");
            dgvChangeLog.Rows.Add("048", "0.1.4", "12/10/2013", "Memory info gathering completed (for the most part.)");
            dgvChangeLog.Rows.Add("049", "0.1.4", "12/10/2013", "Virtual Memory info gathered and displayed.");
            dgvChangeLog.Rows.Add("050", "0.1.4", "12/10/2013", "Began work on an embedded Command Prompt.");
            dgvChangeLog.Rows.Add("051", "0.1.4", "12/10/2013", "Added a new tool: Windows Hibernate File Size Status, Hibernate Enable and Disable.");
            dgvChangeLog.Rows.Add("052", "0.1.4", "12/10/2013", "Added a new tool: System File Check and Open SFC Log. Executes in separate CMD Prompt.");
            dgvChangeLog.Rows.Add("053", "0.1.4", "12/10/2013", "Debug still partially implemented.");
            dgvChangeLog.Rows.Add("054", "0.1.4", "12/10/2013", "WOLF ICON embedded.");
            dgvChangeLog.Rows.Add("055", "0.1.4", "12/10/2013", "Began Kernel Information, Memory, and Heap processing.");
            dgvChangeLog.Rows.Add("056", "0.1.4", "12/10/2013", "Added a direct link to Network Adapters, under Shortcuts, Windows Hardware, Network Adapters.");

            //Version 0.1.5
            dgvChangeLog.Rows.Add("057", "0.1.5", "12/11/2013", "Beginning the implementation of a Task Manager");
            dgvChangeLog.Rows.Add("058", "0.1.5", "12/11/2013", "Several code re-reinforcements to prevent old code running exceptions from occurring at JIT (unfortunately none on the new code as of yet.)");
            dgvChangeLog.Rows.Add("059", "0.1.5", "12/11/2013", "Code preparation for handling Process manipulation (change priority, kill, etc.)");
            dgvChangeLog.Rows.Add("060", "0.1.5", "12/11/2013", "Process searching framework.");
            dgvChangeLog.Rows.Add("061", "0.1.5", "12/11/2013", "Process heap dumps capable. For analyzing active processes.");

            //Version 0.1.6
            dgvChangeLog.Rows.Add("062", "0.1.6", "12/18/2013", "Made Windows 8 compatible the majority of self-built “link” functions.");
            dgvChangeLog.Rows.Add("063", "0.1.6", "12/18/2013", "Removed TaskManager code for performance gains.");

            //Version 0.1.7
            dgvChangeLog.Rows.Add("064", "0.1.7", "12/18/2013", "STEAM link seemed to be working incorrectly. I attempted to fix, but it isn’t Windows 8 compatible. This sounds so stupid but it its weird.");
            dgvChangeLog.Rows.Add("065", "0.1.7", "12/18/2013", "FINALLY have fixed the Memory bug that was detecting the correct amount of memory dimms in use, but only gathering info for one.");
            dgvChangeLog.Rows.Add("066", "0.1.7", "12/18/2013", "Nobody had noticed, but tracing over my coding steps, I made the same mistake in detecting multiple CPUs.");

            //Version 0.1.8
            dgvChangeLog.Rows.Add("067", "0.1.8", "2/11/2014", "Repaired the Memory gathering class. It now handles what to do with mis-matching memory sticks and non-paired sticks.");
            dgvChangeLog.Rows.Add("068", "0.1.8", "2/11/2014", "Unused UI placeholders removed.");
            dgvChangeLog.Rows.Add("069", "0.1.8", "2/11/2014", "Unused code sequences removed (in tandem with the UI changes.)");
            dgvChangeLog.Rows.Add("070", "0.1.8", "2/11/2014", "Tool Added: Flush DNS.");
            dgvChangeLog.Rows.Add("071", "0.1.8", "2/11/2014", "Tool Added: Repair VSS (if it is in fact repairable.)");
            dgvChangeLog.Rows.Add("072", "0.1.8", "2/11/2014", "Shortcuts added for the two new tools.");
            dgvChangeLog.Rows.Add("073", "0.1.8", "2/11/2014", "NIC gathering methods are all but finished (but not displayed to the user yet.)");
            dgvChangeLog.Rows.Add("074", "0.1.8", "2/11/2014", "Repaired the SFC Log button not working.");
            dgvChangeLog.Rows.Add("075", "0.1.8", "2/11/2014", "Known Issue: Exception occurs when querying the processor in a HyperV VM.");
            dgvChangeLog.Rows.Add("076", "0.1.8", "2/11/2014", "Known Issue: If you have north of 8 sticks of RAM, an error / exception occurs on start-up but the program is still functional.");

            //Version 0.1.9 - Valentine's Day Release
            dgvChangeLog.Rows.Add("077", "0.1.9", "2/14/2014", "Tool Added: The ability to Scan Disk/Volume.");
            dgvChangeLog.Rows.Add("078", "0.1.9", "2/14/2014", "Added specific options available to FAT32 and NTFS file systems.");
            dgvChangeLog.Rows.Add("079", "0.1.9", "2/14/2014", "Tool Added: The ability to Defrag Disk/Volume.");
            dgvChangeLog.Rows.Add("080", "0.1.9", "2/14/2014", "Added special options normally only available via command line.");
            dgvChangeLog.Rows.Add("081", "0.1.9", "2/14/2014", "Tool Added: The ability to enable/disable Local UAC.");
            dgvChangeLog.Rows.Add("082", "0.1.9", "2/14/2014", "Added a few new tool tips to accommodate some older and newer tools.");
            dgvChangeLog.Rows.Add("083", "0.1.9", "2/14/2014", "Added a new ToolTab for Registry Tweaks (basically, these require a reboot after enabling/disabling.)");
            dgvChangeLog.Rows.Add("084", "0.1.9", "2/14/2014", "Fixed a slight bug with Memory data gathering that was separate to actually querying the hardware.");
            dgvChangeLog.Rows.Add("085", "0.1.9", "2/14/2014", "Known Issue: Exception occurs when querying the processor in a HyperV VM. (Still)");
            dgvChangeLog.Rows.Add("086", "0.1.9", "2/14/2014", "Known Issue: If you have north of 8 sticks of RAM, an error / exception occurs on start-up but the program is still functional. (Still)");
            dgvChangeLog.Rows.Add("087", "0.1.9", "2/14/2014", "Known Issue: Windows 8.1 may, or may not, read two physical CPUs when only one installed. Weird. (New)");

            //Verison 0.2.0 - Milestone 2 reached!
            dgvChangeLog.Rows.Add("088", "0.2.0", "2/17/2014", "MILESTONE 2 REACHED.");
            dgvChangeLog.Rows.Add("089", "0.2.0", "2/17/2014", "Added a known issues linked.");
            dgvChangeLog.Rows.Add("090", "0.2.0", "2/17/2014", "Bundled links to websites under the same submenu of Info.");
            dgvChangeLog.Rows.Add("091", "0.2.0", "2/17/2014", "Removed the Refresh/Close buttons on the main page, Refresh hadn’t been updated since initial release.");
            dgvChangeLog.Rows.Add("092", "0.2.0", "2/17/2014", "Added a new tool: Enable Windows’ use of HPET (if it is enabled via BIOS/off by default)");
            dgvChangeLog.Rows.Add("093", "0.2.0", "2/17/2014", "Added associated Tooltips for HPET and the disclaimers.");
            dgvChangeLog.Rows.Add("094", "0.2.0", "2/17/2014", "Removed some obsolete code.");
            dgvChangeLog.Rows.Add("095", "0.2.0", "2/17/2014", "Converted the Debug Tab to just logging info now. More to come on this.");

            //Version 0.2.1
            dgvChangeLog.Rows.Add("088", "0.2.1", "2/18/2014", "This is a bigger release than version 0.2.0, simply rebuilt the logging, CPU, and MEM.");
            dgvChangeLog.Rows.Add("089", "0.2.1", "2/18/2014", "Exception catching & logging is now occurring where exceptions can be thrown from .NET framework back at the program.");
            dgvChangeLog.Rows.Add("090", "0.2.1", "2/18/2014", "Debug Log is now highly verbose. It will continue to get more and more verbal.");
            dgvChangeLog.Rows.Add("091", "0.2.1", "2/18/2014", "Entire CPU Class was rebuilt to handle any and all Nulls thrown from the environment.");
            dgvChangeLog.Rows.Add("092", "0.2.1", "2/18/2014", "Rebuilt RAM Class, was rebuilt to handle any and all Nulls thrown from the virtual environment.");
            dgvChangeLog.Rows.Add("093", "0.2.1", "2/18/2014", "Added ALL information that I can get from the OS about the CPU.");
            dgvChangeLog.Rows.Add("094", "0.2.1", "2/18/2014", "Added ALL information that I can get from the OS about each DIMM.");

            //Version 0.2.2
            dgvChangeLog.Rows.Add("095", "0.2.2", "2/20/2014", "Full NIC, both physical and virtual, information has been gathered and displayed.");
            dgvChangeLog.Rows.Add("096", "0.2.2", "2/20/2014", "Full Logical Drive information has been gathered and displayed (actual DISK info coming.)");
            dgvChangeLog.Rows.Add("097", "0.2.2", "2/20/2014", "Contributions are now located (currently testers thanked) under Info menu.");
            dgvChangeLog.Rows.Add("098", "0.2.2", "2/20/2014", "Log tweaking.");

            //Version 0.2.3
            dgvChangeLog.Rows.Add("098", "0.2.3", "2/21/2014", "Full Physical Drive information displayed.");
            dgvChangeLog.Rows.Add("099", "0.2.3", "2/21/2014", "GUI tweaking, it should allow users to re-size without getting too messy.");
            dgvChangeLog.Rows.Add("100", "0.2.3", "2/21/2014", "Active connection code has been updated and streamlined, bringing it inline with the rest of the code.");
            dgvChangeLog.Rows.Add("101", "0.2.3", "2/21/2014", "Tree view information arranged better so more useful data is on top.");
            dgvChangeLog.Rows.Add("102", "0.2.3", "2/21/2014", "Log tweaking and small bug fix.");
            dgvChangeLog.Rows.Add("103", "0.2.3", "2/21/2014", "Typo fixed in Logical Drives that said \"Seria\" instead of \"Serial.\" Impossibru!");

            //Version 0.2.4
            dgvChangeLog.Rows.Add("104", "0.2.4", "3/9/2014", "Main tab information re-arranged to make more sense.");
            dgvChangeLog.Rows.Add("105", "0.2.4", "3/9/2014", "Main tab information now displays localization.");
            dgvChangeLog.Rows.Add("106", "0.2.4", "3/9/2014", "Drive tools, previously under General, have been moved to their own tab.");

            //Version 0.2.5
            dgvChangeLog.Rows.Add("107", "0.2.5", "3/12/2014", "Fixed Memory Size under Memory Tree View from “GB GB” to “GB” following capacity number.");
            dgvChangeLog.Rows.Add("108", "0.2.5", "3/12/2014", "Fixed all shortcuts to websites.");
            dgvChangeLog.Rows.Add("109", "0.2.5", "3/12/2014", "Fixed a few buttons who lost their EventHandler while moving around.");
            dgvChangeLog.Rows.Add("110", "0.2.5", "3/12/2014", "Added Tool: Firewall – Enable\\Disable");
            dgvChangeLog.Rows.Add("111", "0.2.5", "3/12/2014", "Added Tool: UAC Remote Restrictions – Enable\\Disable");
            dgvChangeLog.Rows.Add("112", "0.2.5", "3/12/2014", "Added Tool: Advanced Disk Cleanup – Options Set\\Run");
            dgvChangeLog.Rows.Add("113", "0.2.5", "3/12/2014", "Added a new tab, Software, has OS / BIOS information");
            dgvChangeLog.Rows.Add("114", "0.2.5", "3/12/2014", "Added BIOS information to the TreeView, and the primary information next to it.  Supports multiple BIOS read outs");

            //Version 0.2.6
            dgvChangeLog.Rows.Add("115", "0.2.6", "3/14/2014", "Fixed a few errors in the BIOS Strings.");
            dgvChangeLog.Rows.Add("116", "0.2.6", "3/14/2014", "Added OS Information raw output.  Definitely needs some more work.");
            dgvChangeLog.Rows.Add("117", "0.2.6", "3/14/2014", "Added OS Product Key detection and decode.");
            dgvChangeLog.Rows.Add("118", "0.2.6", "3/14/2014", "Added the ability to Show/Hide Product Keys if you wish to keep it masked.");
            dgvChangeLog.Rows.Add("119", "0.2.6", "3/14/2014", "Added BIOS Embedded Product Key detection and decoding.");
            dgvChangeLog.Rows.Add("120", "0.2.6", "3/14/2014", "Fixed tool tips on Advanced Disk Cleanup buttons.");

            //Version 0.2.7
            dgvChangeLog.Rows.Add("121", "0.2.7", "3/18/2014", "Cleaned up the output of the BIOS info.");
            dgvChangeLog.Rows.Add("122", "0.2.7", "3/18/2014", "Cleaned up the output of the OS info.");
            dgvChangeLog.Rows.Add("123", "0.2.7", "3/18/2014", "Windows 8 / 8.1 Serial Key Decoding corrected.");
            dgvChangeLog.Rows.Add("124", "0.2.7", "3/18/2014", "Max process size under OS info tab and Hardware Memory tab has been corrected. Windows 7/8 x64 is 8TB, 128TB for Servers.");

            //Version 0.2.8
            dgvChangeLog.Rows.Add("125", "0.2.8", "4/1/2014", "Added another tab to Software information, Programs.");
            dgvChangeLog.Rows.Add("126", "0.2.8", "4/1/2014", "Gathered all installed program data and displayed to user.");
            dgvChangeLog.Rows.Add("127", "0.2.8", "4/1/2014", "Shows command line to uninstall program.");
            dgvChangeLog.Rows.Add("128", "0.2.8", "4/1/2014", "Right click context/selection with the ability to “Try To Uninstall” for each program.");

            //Version 0.2.9
            dgvChangeLog.Rows.Add("129", "0.2.9", "5/8/2014", "Added the ability to query remote machines, under Domain Tools.");
            dgvChangeLog.Rows.Add("130", "0.2.9", "5/8/2014", "Added the ability to right click uninstall the installed programs from the IPL.");

            //Vesion 0.3.0 - Milestone!
            dgvChangeLog.Rows.Add("131", "0.3.0", "5/27/2014", "MILESTONE 3 REACHED.");
            dgvChangeLog.Rows.Add("132", "0.3.0", "5/27/2014", "More robust querying remote machines, handling a variety of exceptions");
            dgvChangeLog.Rows.Add("133", "0.3.0", "5/27/2014", "Local machines are now query-able. It did give a “remote credentials” can’t be used to query local machine garbage.");
            dgvChangeLog.Rows.Add("134", "0.3.0", "5/27/2014", "Cleaned up a bit more code in the background.");
            dgvChangeLog.Rows.Add("135", "0.3.0", "5/27/2014", "Added OEM, Bios Version, and machine BIOS Serial (if exists) to remote querying information.");
            dgvChangeLog.Rows.Add("136", "0.3.0", "5/27/2014", "IP Range Query has been trickier, so I have decided to delay that functionality for now.");

            //Version 0.3.1
            dgvChangeLog.Rows.Add("137", "0.3.1", "6/21/2014", "Added GPU information to the hardware tab.");
            dgvChangeLog.Rows.Add("138", "0.3.1", "6/21/2014", "Added User Accounts to the software tab.");
            dgvChangeLog.Rows.Add("139", "0.3.1", "6/21/2014", "Added ThermalProbes in use to the software tab.");
            dgvChangeLog.Rows.Add("140", "0.3.1", "6/21/2014", "Greatly enhanced the Remote Workstation Queries.");
            dgvChangeLog.Rows.Add("141", "0.3.1", "6/21/2014", "RWQ: Added the ability to set what information to query (less info is quicker.)");
            dgvChangeLog.Rows.Add("142", "0.3.1", "6/21/2014", "RWQ: Added GPU / GPU Driver information.");
            dgvChangeLog.Rows.Add("143", "0.3.1", "6/21/2014", "RWQ: Errors are now displayed in the treeview instead of MessageBoxes");
            dgvChangeLog.Rows.Add("144", "0.3.1", "6/21/2014", "RWQ: More robust error handling.");
            dgvChangeLog.Rows.Add("145", "0.3.1", "6/21/2014", "Made SFC functions in the toolstrip menu more clearer.");
            dgvChangeLog.Rows.Add("146", "0.3.1", "6/21/2014", "GUI modifications.");

            //Version 0.3.1B - Hotfix
            dgvChangeLog.Rows.Add("147", "0.3.1B", "6/21/2014", "Hotfix: Fixed an error in the exception logging function where itself could create an unhandled exception.");

            //Version 0.3.2
            dgvChangeLog.Rows.Add("148", "0.3.2", "6/22/2014", "Added Display information.");
            dgvChangeLog.Rows.Add("149", "0.3.2", "6/22/2014", "Added SMBIOS Memory Array information to BIOS tab.");
            dgvChangeLog.Rows.Add("150", "0.3.2", "6/22/2014", "Added IRQs to the ThermalProbe tab, making it a DMA/IRQ/TP tab.");
            dgvChangeLog.Rows.Add("151", "0.3.2", "6/22/2014", "Serious attempt at securing the program from never crashing.");
            dgvChangeLog.Rows.Add("152", "0.3.2", "6/22/2014", "Updated the Known Issues and Contributions in the program.");

            //Version 0.3.2 Hotfixes
            dgvChangeLog.Rows.Add("153", "0.3.2B", "6/22/2014", "Repaired AMD GPU user’s information to show correctly.");
            dgvChangeLog.Rows.Add("154", "0.3.2C", "6/22/2014", "Repaired Intel GPU user’s information to show correctly.");
            dgvChangeLog.Rows.Add("155", "0.3.2D", "6/22/2014", "Fixed one cause of slow start-up time (User\\Accounts)");
            dgvChangeLog.Rows.Add("156", "0.3.2E", "6/22/2014", "Repaired Display Information.");
            dgvChangeLog.Rows.Add("157", "0.3.2F", "6/22/2014", "Fixed issue with users crashing in start-up.");

            //Version 0.3.3
            dgvChangeLog.Rows.Add("158", "0.3.3", "6/24/2014", "Incorporated all post 0.3.2 changes!");
            dgvChangeLog.Rows.Add("159", "0.3.3", "6/24/2014", "Rebuilt the Accounts tab display. All Domain accounts display on a domain machine, with domain credentials used.");
            dgvChangeLog.Rows.Add("160", "0.3.3", "6/24/2014", "Repaired the x64 Program List not fully displaying.");

            //Version 0.3.4
            dgvChangeLog.Rows.Add("161", "0.3.4", "7/5/2014", "Added Sound Devices, both integrated/extension cards and GPU audio devices. GPU audio is under the GPU tabs.");
            dgvChangeLog.Rows.Add("162", "0.3.4", "7/5/2014", "Fixed a crash on startup on Server 2012 / 2012 R2 VMs, possibly Server 2008 / R2 VMs too, sincerest apologies!");

            //Version 0.3.5.0
            dgvChangeLog.Rows.Add("163", "0.3.5.0", "7/23/2014", "IP Range Query functionality added/enabled.");
            dgvChangeLog.Rows.Add("164", "0.3.5.0", "7/23/2014", "Speed improvements on Remote Machine information gathering and reduced the number of connections established.");
            dgvChangeLog.Rows.Add("165", "0.3.5.0", "7/23/2014", "Incorporated all post 0.3.2 changes!");
            dgvChangeLog.Rows.Add("166", "0.3.5.0", "7/23/2014", "The road to WOLF 359 begins ^.^");

            //Version 0.3.5.1
            dgvChangeLog.Rows.Add("166", "0.3.5.1", "7/31/2014", "IP Range Query greatly enhanced.");
            dgvChangeLog.Rows.Add("167", "0.3.5.1", "7/31/2014", "IPRQ: Speed increased 1100%");
            dgvChangeLog.Rows.Add("168", "0.3.5.1", "7/31/2014", "IPRQ: Percentage displayed below IPs to let you see progress.");
            dgvChangeLog.Rows.Add("169", "0.3.5.1", "7/31/2014", "IPRQ: Total Time Elapsed also included.");
            dgvChangeLog.Rows.Add("170", "0.3.5.1", "7/31/2014", "IPRQ: Export all data to CSV!");
            dgvChangeLog.Rows.Add("171", "0.3.5.1", "7/31/2014", "Rebuilt the Programs List to load extremely fast.");
            dgvChangeLog.Rows.Add("172", "0.3.5.1", "7/31/2014", "Added installed system Driver List (with fast load.)");
            dgvChangeLog.Rows.Add("173", "0.3.5.1", "7/31/2014", "Added 17 new tools / shortcuts.");
            dgvChangeLog.Rows.Add("174", "0.3.5.1", "7/31/2014", "Tool: Restart WMI Services.");
            dgvChangeLog.Rows.Add("175", "0.3.5.1", "7/31/2014", "Tool: Reset CMD Prompt Size.");
            dgvChangeLog.Rows.Add("176", "0.3.5.1", "7/31/2014", "Tool: Driver Query / Verbose tools.");
            dgvChangeLog.Rows.Add("177", "0.3.5.1", "7/31/2014", "Tool: File Signature Verification / shortcut to log file.");
            dgvChangeLog.Rows.Add("178", "0.3.5.1", "7/31/2014", "Tool: Shortcut to Windows Memory Test menu.");
            dgvChangeLog.Rows.Add("179", "0.3.5.1", "7/31/2014", "Tool: Shortcut to Registry Editor.");
            dgvChangeLog.Rows.Add("180", "0.3.5.1", "7/31/2014", "Tool: Execute an Open Files Query, and to enable/disable Open Files query for local machine.");
            dgvChangeLog.Rows.Add("181", "0.3.5.1", "7/31/2014", "Tool: Added the ability to check DEP status, and to enable and disable.");
            dgvChangeLog.Rows.Add("182", "0.3.5.1", "7/31/2014", "Tool: Added the ability to check Windows Defender status, and attempt to Enable and Disable.");

            //Version 0.3.5.2
            dgvChangeLog.Rows.Add("183", "0.3.5.2", "8/1/2014", "CPU / RAM usage displayed in real time.");
            dgvChangeLog.Rows.Add("184", "0.3.5.2", "8/1/2014", "Some minor UI changes, to accommodate the new features.");
            dgvChangeLog.Rows.Add("185", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to MSINFO32");
            dgvChangeLog.Rows.Add("186", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to MSTSC (Button / Shortcut Menu / Alt + R as well)");
            dgvChangeLog.Rows.Add("187", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to MSCONFIG");
            dgvChangeLog.Rows.Add("188", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to DirectX Diagnostic Tools");
            dgvChangeLog.Rows.Add("189", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to DirectX Control Panel");
            dgvChangeLog.Rows.Add("190", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Local User/Group Manager");
            dgvChangeLog.Rows.Add("191", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Local Policies Applied (RSOP)");
            dgvChangeLog.Rows.Add("192", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Local Policy Editor");
            dgvChangeLog.Rows.Add("193", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Local Security Policy Editor");
            dgvChangeLog.Rows.Add("194", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to ODBC Control Panel");
            dgvChangeLog.Rows.Add("195", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Performance Monitor");
            dgvChangeLog.Rows.Add("196", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Resource Monitor");
            dgvChangeLog.Rows.Add("197", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to WMI Management");
            dgvChangeLog.Rows.Add("198", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Print Manager");
            dgvChangeLog.Rows.Add("199", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Printer Migration");
            dgvChangeLog.Rows.Add("200", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Windows Optional Features");
            dgvChangeLog.Rows.Add("201", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Remote Assistance");
            dgvChangeLog.Rows.Add("202", "0.3.5.2", "8/1/2014", "Tool: Added shortcut to Shared Folder Management");
            dgvChangeLog.Rows.Add("203", "0.3.5.2", "8/1/2014", "Rearranged Tools / Registry.");

            //Version 0.3.5.2B - Hotfix
            dgvChangeLog.Rows.Add("204", "0.3.5.2B", "8/1/2014", "Hotfix: RAM usage counter became frozen.");
            dgvChangeLog.Rows.Add("205", "0.3.5.2B", "8/1/2014", "Hotfix: Typo on tools page.");

            //Version 0.3.5.3
            dgvChangeLog.Rows.Add("206", "0.3.5.3", "8/4/2014", "Added a change log.");
            dgvChangeLog.Rows.Add("208", "0.3.5.3", "8/4/2014", "Added error catching for both SFC Log and File Signature log not existing. No more exception.");
            dgvChangeLog.Rows.Add("207", "0.3.5.3", "8/5/2014", "Added an Updates option under the Info menu that also checks for the latest version.");
            dgvChangeLog.Rows.Add("209", "0.3.5.3", "8/5/2014", "Added an online check for latest version displayed in UI.");
            dgvChangeLog.Rows.Add("210", "0.3.5.3", "8/5/2014", "Added a ThreadException code in the main thread.");
            dgvChangeLog.Rows.Add("211", "0.3.5.3", "8/5/2014", "Bug: Fixed the UnmanagedException code to catch UI exceptions.");
            dgvChangeLog.Rows.Add("212", "0.3.5.3", "8/5/2014", "Bug: Fixed the version label location when maximized.");
            dgvChangeLog.Rows.Add("213", "0.3.5.3", "8/5/2014", "Bug: Finally fixed the latest crash on start-up bug.");
            dgvChangeLog.Rows.Add("214", "0.3.5.3", "8/5/2014", "Various UI changes.");
            dgvChangeLog.Rows.Add("215", "0.3.5.3", "8/5/2014", "Gave the user the ability to disable the WindowsDefender message.");

            //Version 0.3.5.4
            dgvChangeLog.Rows.Add("216", "0.3.5.4", "8/5/2014", "Vastly improved some older code, making it more error robust.");
            dgvChangeLog.Rows.Add("217", "0.3.5.4", "8/6/2014", "Small change in the checking online for new updates. Old WOLF versions will just error out.");
            dgvChangeLog.Rows.Add("218", "0.3.5.4", "8/5/2014", "Typo: Two 214 entries in the ChangeLog.");
            dgvChangeLog.Rows.Add("219", "0.3.5.4", "8/6/2014", "Added a Message/Save Variable regarding the Windows Firewall Service status.");
            dgvChangeLog.Rows.Add("220", "0.3.5.4", "8/5/2014", "Bug: Fixed the Windows Defender message status result from not saving correctly.");
            dgvChangeLog.Rows.Add("221", "0.3.5.4", "8/5/2014", "Bug: Fixed a CLR Runtime exception that can occur getting registry statuses.");
            dgvChangeLog.Rows.Add("222", "0.3.5.4", "8/6/2014", "Bug: Fixed a CLR StackOverflow Exception that can occur when checking Firewall Status.");

            //Version 0.3.5.5
            dgvChangeLog.Rows.Add("223", "0.3.5.5", "8/6/2014", "Typo: Ellapsed => Elapsed.  Thanks /u/noob_dude_ for the catch!");
            dgvChangeLog.Rows.Add("224", "0.3.5.5", "8/6/2014", "Created the Remote Command Line Interface.");
            dgvChangeLog.Rows.Add("225", "0.3.5.5", "8/17/2014", "Feature: IRQs detection is now placed in a background thread.");
            dgvChangeLog.Rows.Add("226", "0.3.5.5", "8/17/2014", "Feature: Latest version check is now placed in a background thread.");
            dgvChangeLog.Rows.Add("227", "0.3.5.5", "8/17/2014", "Start-up time reduced approximately 300~1000+ ms for some users.");
            dgvChangeLog.Rows.Add("228", "0.3.5.5", "8/17/2014", "RCLI: Added the AD GetDCs function (lists all Domain Controllers.)");
            dgvChangeLog.Rows.Add("229", "0.3.5.5", "8/17/2014", "RCLI: Added PING command functionality.");
            dgvChangeLog.Rows.Add("230", "0.3.5.5", "8/17/2014", "RCLI: Added LISTCMDs, LISTADs, HELP, and REDRAW commands.");
            dgvChangeLog.Rows.Add("231", "0.3.5.5", "8/17/2014", "RCLI: Added EXDET to give a detailed description of the last exception that occurred in RCLI.");
            dgvChangeLog.Rows.Add("232", "0.3.5.5", "8/20/2014", "RCLI: Added REPEAT command to repeat other commands X number of times.");
            dgvChangeLog.Rows.Add("233", "0.3.5.5", "8/20/2014", "RCLI: REPEAT command supports PING.  Repeat PING displays statistics to user.");
            dgvChangeLog.Rows.Add("234", "0.3.5.5", "9/1/2014", "RCLI: Rebuilt the command line interpreter for ease of programming.");
            dgvChangeLog.Rows.Add("235", "0.3.5.5", "9/30/2014", "Created the Oracle - An all-in-one System Monitoring system.");
            dgvChangeLog.Rows.Add("236", "0.3.5.5", "9/30/2014", "Oracle: v0.0.1 includes Central Processing: CPU, System, Cache.");

            //Version 0.3.5.6
            dgvChangeLog.Rows.Add("237", "0.3.5.6", "10/1/2014", "WOLF: Modified the log for Firewall / Defender status if check is disabled.");
            dgvChangeLog.Rows.Add("238", "0.3.5.6", "10/1/2014", "WOLF: Typofix - Processor Queue Length tooltip fixed.");
            dgvChangeLog.Rows.Add("239", "0.3.5.6", "10/14/2014", "WOLF: HardwareInfo - Added Motherboard/Chipset info.");
            dgvChangeLog.Rows.Add("240", "0.3.5.6", "10/15/2014", "WOLF: General - Reduced start-up time further.");
            dgvChangeLog.Rows.Add("241", "0.3.5.6", "10/15/2014", "WOLF: General - Removed row headers and added row select on Programs.");
            dgvChangeLog.Rows.Add("242", "0.3.5.6", "10/15/2014", "WOLF: General - Removed row headers and added row select on Drivers.");
            dgvChangeLog.Rows.Add("243", "0.3.5.6", "10/15/2014", "WOLF: Copy is now working as intended on the above two DataGridViews.");
            dgvChangeLog.Rows.Add("244", "0.3.5.6", "10/15/2014", "WOLF: Minor-GUI changes under Tools tab.");
            dgvChangeLog.Rows.Add("245", "0.3.5.6", "10/15/2014", "WOLF: Oracle now has a Tool menu shortcut and key short cut (Alt + o).");
            dgvChangeLog.Rows.Add("246", "0.3.5.6", "10/15/2014", "WOLF: Feature - Repair WMI repository feature added.");
            dgvChangeLog.Rows.Add("249", "0.3.5.6", "10/20/2014", "WOLF: Bugfix - New crash occuring on Server 2008 R2 should be fixed.");
            dgvChangeLog.Rows.Add("249", "0.3.5.6", "10/20/2014", "Oracle: Bugfix - Fixed multiple CPU server execptions on monitoring.");
            dgvChangeLog.Rows.Add("247", "0.3.5.6", "10/21/2014", "WOLF: Feature - Added an export to CSV feature for Accounts.");
            dgvChangeLog.Rows.Add("248", "0.3.5.6", "10/21/2014", "WOLF: Added an Accounts loading message.");
            dgvChangeLog.Rows.Add("249", "0.3.5.6", "10/21/2014", "WOLF: Minor GUI changes.");

            //Version 0.3.5.7
            dgvChangeLog.Rows.Add("250", "0.3.5.7", "12/18/2014", "WOLF: Implemented Click-Copy feature on all treeviews.");
            dgvChangeLog.Rows.Add("251", "0.3.5.7", "12/18/2014", "WOLF: HardwareInfo - Cleaned up Motherboard/Chipset info.");
            dgvChangeLog.Rows.Add("252", "0.3.5.7", "12/22/2014", "WOLF: Minor GUI changes.");
            dgvChangeLog.Rows.Add("253", "0.3.5.7", "12/22/2014", "WOLF: Bugfix - IRQ background worker fully functioning now.");
            dgvChangeLog.Rows.Add("254", "0.3.5.7", "12/22/2014", "WOLF: Implemented contextual Click-Copy feature on all treeviews.");
            dgvChangeLog.Rows.Add("255", "0.3.5.7", "12/29/2014", "WOLF: SoftwareInfo - Find and display all Network Ports in use.");
            dgvChangeLog.Rows.Add("256", "0.3.5.7", "12/29/2014", "WOLF: SoftwareInfo - Display the process using each Network Port and added sorting.");
            dgvChangeLog.Rows.Add("257", "0.3.5.7", "12/30/2014", "WOLF: SoftwareInfo - Right-Click allow Kill Process on Network Port selection.");
            dgvChangeLog.Rows.Add("258", "0.3.5.7", "12/30/2014", "WOLF: SoftwareInfo - Added Network Port manual Refresh and Export To CSV functionality.");

            //Version 0.3580
            dgvChangeLog.Rows.Add("259", "0.3580", "1/5/2015", "WOLF: SoftwareInfo - Network ports export feature wasn't attached to the button. Doh!");
            dgvChangeLog.Rows.Add("260", "0.3580", "1/5/2015", "WOLF: Bugfix - Network ports export feature was missing State column.");
            dgvChangeLog.Rows.Add("261", "0.3580", "1/5/2015", "WOLF: Bugfix - Accounts Export button position corrected when program maximized.");
            dgvChangeLog.Rows.Add("262", "0.3580", "1/5/2015", "WOLF: GUI Change - NIC/vNIC tab has had their buttons removed, obsolete since Copy-Click.");
            dgvChangeLog.Rows.Add("263", "0.3580", "1/9/2015", "WOLF: SoftwareInfo - Network ports feature added to allow auto refresh.");
            dgvChangeLog.Rows.Add("264", "0.3580", "1/9/2015", "WOLF: SoftwareInfo - Network ports auto refresh tries to remember the user's place and selection.");
            dgvChangeLog.Rows.Add("265", "0.3580", "1/9/2015", "WOLF: Bugfix - Network ports kill process wasn't working 100%.");
            dgvChangeLog.Rows.Add("266", "0.3580", "1/9/2015", "WOLF: Bugfix - Network ports had a variety of possible exceptions that were unhandled. My bad.");
            dgvChangeLog.Rows.Add("267", "0.3580", "1/10/2015", "WOLF: GUI Change - External IP address displayed on startup, button moved.");
            dgvChangeLog.Rows.Add("268", "0.3580", "1/10/2015", "WOLF: GUI Change - Tools - Openfiles Buttons cleaned up.");
            dgvChangeLog.Rows.Add("269", "0.3580", "1/10/2015", "WOLF: GUI Change - Log now scrolls to the end.");
            dgvChangeLog.Rows.Add("270", "0.3580", "1/10/2015", "WOLF: GUI Change - Log label removed, it was unnecessary.");
            dgvChangeLog.Rows.Add("271", "0.3580", "1/11/2015", "WOLF: Shortcut Added - Update window can now be opened with Alt + U.");
            dgvChangeLog.Rows.Add("272", "0.3580", "1/11/2015", "WOLF: GUI Change - Contribution list row header changes removed.");
            dgvChangeLog.Rows.Add("273", "0.3580", "1/11/2015", "WOLF: GUI Change - Known Issues has been updated.");
            dgvChangeLog.Rows.Add("274", "0.3580", "1/12/2015", "WOLF: SoftwareInfo - Network ports context menu has a minor change, to include the index next to PID to increase user accuracy.");
            dgvChangeLog.Rows.Add("275", "0.3580", "1/12/2015", "WOLF: Versioning change.");
            dgvChangeLog.Rows.Add("276", "0.3580", "1/12/2015", "WOLF: Feature Added - The ability to detect Office 2010/2013 product keys.");
            dgvChangeLog.Rows.Add("277", "0.3580", "1/12/2015", "WOLF: GUI Change - Added x86/x64 appendage to the application title bar.");

            //Version 0.3581
            dgvChangeLog.Rows.Add("278", "0.3581", "1/12/2015", "WOLF: Bugfix - Windows OS Key showing up again on x86 build.");
            dgvChangeLog.Rows.Add("279", "0.3581", "1/13/2015", "WOLF: Bugfix - MS Product Keys showing up on x86 build.");
            dgvChangeLog.Rows.Add("280", "0.3581", "1/14/2015", "WOLF: Feature - Added Partitions to be displayed.");
            dgvChangeLog.Rows.Add("281", "0.3581", "1/15/2015", "WOLF: GUI Change - Overhauled the Drive tab under Hardware.");
            dgvChangeLog.Rows.Add("282", "0.3581", "1/18/2015", "WOLF: Tool Added - Bulk file searching, renaming, and copying, added.");
            dgvChangeLog.Rows.Add("283", "0.3581", "1/18/2015", "WOLF: Search & Rename released, v0.001");

            //Version 0.3582
            dgvChangeLog.Rows.Add("284", "0.3582", "1/19/2015", "WOLF: Bugfix - Windows OS Key showing up again on x86 build. Again.");
            dgvChangeLog.Rows.Add("285", "0.3582", "1/19/2015", "WOLF: Bugfix - Microsoft Product Keys showing up again on x86 build. Again.");
            dgvChangeLog.Rows.Add("286", "0.3582", "1/19/2015", "WOLF: Bug Discovered - The x86 build crashes frequently on Windows x64.");
            dgvChangeLog.Rows.Add("287", "0.3582", "1/21/2015", "WOLF: Bugfix - Rewrote the vast majority of the tools code to handle above bug.");
            dgvChangeLog.Rows.Add("288", "0.3582", "2/10/2015", "WOLF: Performance - Drivers, Install Programs, and Net Ports flicker less!");
            dgvChangeLog.Rows.Add("289", "0.3582", "2/10/2015", "WOLF: UI - New color scheme.");
            dgvChangeLog.Rows.Add("290", "0.3582", "2/10/2015", "WOLF: UI - Rebuilt the NICs tab.");
            dgvChangeLog.Rows.Add("291", "0.3582", "2/11/2015", "WOLF: Performance - Draw to screen performance is up when scrolling and smoother too (with less flicker).");
            dgvChangeLog.Rows.Add("292", "0.3582", "2/12/2015", "WOLF: Feature - HPET Enable status now detected at startup.");
            dgvChangeLog.Rows.Add("293", "0.3582", "2/12/2015", "WOLF: Feature - DEP Enable status now detected at startup.");

            //Version 0.3583
            dgvChangeLog.Rows.Add("294", "0.3583", "2/15/2015", "WOLF: Adjusted the context menu theme colors a bit more.");
            dgvChangeLog.Rows.Add("295", "0.3583", "3/23/2015", "Tool: Added the Computer Info Report under tools.");
            dgvChangeLog.Rows.Add("296", "0.3583", "3/22/2015", "CIR: Allows user to generate a cell/grid view of various Win32 information for copying or exporting to CSV.");
            dgvChangeLog.Rows.Add("297", "0.3583", "3/22/2015", "Tool: Removed RCLI.");
            dgvChangeLog.Rows.Add("298", "0.3583", "3/23/2015", "CIR: Added a performance counter to let a user know when reporting can slow down.");
            dgvChangeLog.Rows.Add("299", "0.3583", "3/23/2015", "CIR: Computer Info Reports was tweaked quite a bit.");
            dgvChangeLog.Rows.Add("300", "0.3583", "3/24/2015", "CIR: Added the ability to generate mini-reports (quick)");
            dgvChangeLog.Rows.Add("301", "0.3583", "3/24/2015", "Tool: POSH - Added embedded semi-enabled PowerShell console.");
            dgvChangeLog.Rows.Add("302", "0.3583", "3/25/2015", "POSH: Added the ability to launch PowerShell session.");
            dgvChangeLog.Rows.Add("303", "0.3583", "3/25/2015", "POSH: Added four PowerShell sessions styles.");
            dgvChangeLog.Rows.Add("304", "0.3583", "3/25/2015", "POSH: Added the ability to set execution policy.");
            dgvChangeLog.Rows.Add("305", "0.3583", "3/25/2015", "POSH: Added the ability to pipe FT, FL, and Out-String naturally.");
            dgvChangeLog.Rows.Add("306", "0.3583", "3/25/2015", "POSH: Basic cmdlet support has been attempted.");
            dgvChangeLog.Rows.Add("307", "0.3583", "3/25/2015", "Oracle: Color scheme modified.");
            dgvChangeLog.Rows.Add("308", "0.3583", "3/25/2015", "WOLF: Bugfix - HPET Enable/Disable work again on Windows 7.");
            dgvChangeLog.Rows.Add("309", "0.3583", "3/25/2015", "WOLF: Bugfix - DEP Enable/Disable work again on Windows 7.");
            dgvChangeLog.Rows.Add("310", "0.3583", "3/25/2015", "WOLF: Bugfix - Manage Users/Groups no longer causes an exception (x64).");
            dgvChangeLog.Rows.Add("311", "0.3583", "3/25/2015", "WOLF: Bugfix - All Windows 8/8.1 users should now properly get Firewall & WINDEF messages.");

            //Version 0.3584A
            dgvChangeLog.Rows.Add("312", "0.3584A", "3/25/2015", "Tool: Added the ability to get remote computer OS key, if user has permission.");
            dgvChangeLog.Rows.Add("313", "0.3584A", "3/25/2015", "WOLF: Remote OS Key - Enhanced the functionality.");

            //Version 0.3585
            dgvChangeLog.Rows.Add("314", "0.3585", "2/20/2016", "Tool: Added the ability to disable Windows telemetry.");
            dgvChangeLog.Rows.Add("315", "0.3585", "2/20/2016", "Tool: Added the ability to disable Windows User Interface logging.");
        }
    }
}