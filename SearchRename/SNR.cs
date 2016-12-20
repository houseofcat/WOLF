using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Wolf.SearchRename
{
    public partial class SNR : Form
    {
        //Searching Section
        List<String> FoundFileList = new List<String>();
        List<String> PreviewFileList = new List<String>();
        List<String> BatchSearch = new List<String>();

        Int32 foundFiles = 0;
        Int32 checkedFiles = 0;

        long counter = 0;
        double dblCounter = 0.0;

        long counter2 = 0;
        double dblCounter2 = 0.0;

        //Renaming Section
        List<String> FoundFileListR = new List<String>();
        List<String> PreviewFileListR = new List<String>();

        String FilePathR = "";
        String RenamePart = "";
        String RenameTo = "";

        Int32 foundFilesR = 0;
        Int32 checkedFilesR = 0;

        long counterR = 0;
        double dblCounterR = 0.0;

        long counter2R = 0;
        double dblCounter2R = 0.0;

        //Copying Section
        List<String> FoundFileListC = new List<String>();
        List<String> PreviewFileListC = new List<String>();
        //String FilePathC = "";
        String CopyTo = "";

        Int32 foundFilesC = 0;
        Int32 checkedFilesC = 0;

        long counterC = 0;
        double dblCounterC = 0.0;

        long counter2C = 0;
        double dblCounter2C = 0.0;

        //Folder Renaming Section
        List<String> FFRen = new List<String>();
        List<String> FFRPrev = new List<String>();

        Int32 foundFolders = 0;
        Int32 checkedFolders = 0;

        long counterFR = 0;
        double dblCounterFR = 0.0;

        long counter2FR = 0;
        double dblCounter2FR = 0.0;

        //Normal Search Functions
        BackgroundWorker BW1 = new BackgroundWorker();

        //Rename Search Functions
        BackgroundWorker BW1R = new BackgroundWorker();

        //Copy Search Functions
        BackgroundWorker BW1C = new BackgroundWorker();

        //Folders Search Function
        BackgroundWorker BWFS = new BackgroundWorker();

        //Search Options
        String FileType = "";
        String FileType2 = "";
        String FileType3 = "";

        //Performance Variables
        Stopwatch PerformanceTimer = new Stopwatch();
        Stopwatch DisplayTimer = new Stopwatch();

        //AutoCompleteHistory
        AutoCompleteStringCollection SearchDirectories = new AutoCompleteStringCollection();
        AutoCompleteStringCollection SearchFiles = new AutoCompleteStringCollection();

        public SNR()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();

            //Setting the AutoCompletion Section
            tbxFilePath.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxFilePath.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxFilePath.AutoCompleteCustomSource = SearchDirectories;

            tbxFileName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxFileName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxFileName.AutoCompleteCustomSource = SearchFiles;

            tbxRenameDirectory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxRenameDirectory.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxRenameDirectory.AutoCompleteCustomSource = SearchDirectories;

            tbxRenameName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxRenameName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxRenameName.AutoCompleteCustomSource = SearchFiles;

            tbxCopyDirectory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxCopyDirectory.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxCopyDirectory.AutoCompleteCustomSource = SearchDirectories;

            tbxCopyName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxCopyName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxCopyName.AutoCompleteCustomSource = SearchFiles;

            tbxFRDirectory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxFRDirectory.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxFRDirectory.AutoCompleteCustomSource = SearchDirectories;

            tbxFRName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxFRName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxFRName.AutoCompleteCustomSource = SearchFiles;
        }

        //Multithreading
        private void InitializeBackgroundWorkers()
        {
            //Searching BW
            BW1.DoWork += new DoWorkEventHandler(BW1_DoWork);
            BW1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW1_RunWorkerCompleted);

            //Renaming BW
            BW1R.DoWork += new DoWorkEventHandler(BW1R_DoWork);
            BW1R.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW1R_RunWorkerCompleted);

            //Copying BW
            BW1C.DoWork += new DoWorkEventHandler(BW1C_DoWork);
            BW1C.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW1C_RunWorkerCompleted);

            //Folder Rename
            BWFS.DoWork += new DoWorkEventHandler(BWFS_DoWork);
            BWFS.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWFS_RunWorkerCompleted);
        }

        //Searching BW
        private void BW1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (checkBoxStrict.Checked)
                {
                    PerformanceTimer = Stopwatch.StartNew();

                    findAllFilesStrict(tbxFilePath.Text, tbxFileName.Text);

                    PerformanceTimer.Stop();
                }
                else
                {
                    PerformanceTimer = Stopwatch.StartNew();

                    findAllFiles(tbxFilePath.Text, tbxFileName.Text);

                    PerformanceTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Holy shit we are going to die!\n  " + ex.Message);
            }
        }

        private void BW1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchDirectories.Add(tbxFilePath.Text);
            SearchFiles.Add(tbxFileName.Text);

            if (FoundFileList.Any())
            {
                Fill_Tree();
            }

            TreeNode[] foundNodes = treeFOUND.Nodes.Find("Load", false);

            if (foundNodes.Any())
            {
                foundNodes[0].Text = "System has found all matches. Files Checked: " + checkedFiles.ToString() + "  Matches: " + foundFiles.ToString();
            }

            counter = PerformanceTimer.ElapsedMilliseconds;
            dblCounter = ((double)counter) / 1000;

            if (dblCounter > 1)
            {
                lblPerf.Text = "Last Search Took: " + dblCounter + " Seconds";
            }
            else
            {
                lblPerf.Text = "Last Search Took: " + PerformanceTimer.ElapsedMilliseconds + " Milliseconds";
            }

            counter2 = DisplayTimer.ElapsedMilliseconds;
            dblCounter2 = ((double)counter2) / 1000;

            if (dblCounter2 > 1)
            {
                lblDisp.Text = "Display Results: " + dblCounter2 + " Seconds";
            }
            else
            {
                lblDisp.Text = "Display Results: " + DisplayTimer.ElapsedMilliseconds + " Milliseconds";
            }

            if ((dblCounter + dblCounter2) > 1)
            {
                lblTot.Text = "Total Time: " + (dblCounter + dblCounter2).ToString() + " Seconds";
            }
            else
            {
                lblTot.Text = "Total Time: " + (PerformanceTimer.ElapsedMilliseconds + DisplayTimer.ElapsedMilliseconds) + " Milliseconds";
            }

            funcEnableAll();
        }

        //Renaming BW
        private void BW1R_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PerformanceTimer = Stopwatch.StartNew();

                findAllFilesR(tbxRenameDirectory.Text, tbxRenameName.Text);

                PerformanceTimer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Holy shit we are going to die!\n  " + ex.Message);
            }
        }

        private void BW1R_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchDirectories.Add(tbxRenameDirectory.Text);
            SearchFiles.Add(tbxRenameName.Text);

            if (FoundFileListR.Any())
            {
                Fill_TreeR();
            }

            TreeNode[] foundNodes = treeRenameFound.Nodes.Find("Load", false);

            if (foundNodes.Any())
            {
                foundNodes[0].Text = "System has found all matches. Files Checked: " + checkedFilesR.ToString() + "  Matches: " + foundFilesR.ToString();
            }

            counterR = PerformanceTimer.ElapsedMilliseconds;
            dblCounterR = ((double)counterR) / 1000;

            if (dblCounterR > 1)
            {
                lblPerf.Text = "Last Search Took: " + dblCounterR + " Seconds";
            }
            else
            {
                lblPerf.Text = "Last Search Took: " + PerformanceTimer.ElapsedMilliseconds + " Milliseconds";
            }

            counter2R = DisplayTimer.ElapsedMilliseconds;
            dblCounter2R = ((double)counter2R) / 1000;

            if (dblCounter2R > 1)
            {
                lblDisp.Text = "Display Results: " + dblCounter2 + " Seconds";
            }
            else
            {
                lblDisp.Text = "Display Results: " + DisplayTimer.ElapsedMilliseconds + " Milliseconds";
            }

            if ((dblCounterR + dblCounter2R) > 1)
            {
                lblTot.Text = "Total Time: " + (dblCounterR + dblCounter2R).ToString() + " Seconds";
            }
            else
            {
                lblTot.Text = "Total Time: " + (PerformanceTimer.ElapsedMilliseconds + DisplayTimer.ElapsedMilliseconds) + " Milliseconds";
            }

            funcEnableAllR();
        }

        //Copying BW
        private void BW1C_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PerformanceTimer = Stopwatch.StartNew();

                findAllFilesC(tbxCopyDirectory.Text, tbxCopyName.Text);

                PerformanceTimer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Holy shit we are going to die!\n  " + ex.Message);
            }
        }

        private void BW1C_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchDirectories.Add(tbxCopyDirectory.Text);
            SearchFiles.Add(tbxCopyName.Text);

            if (FoundFileListC.Any())
            {
                Fill_TreeC();
            }

            TreeNode[] foundNodes = treeCopyFound.Nodes.Find("Load", false);

            if (foundNodes.Any())
            {
                foundNodes[0].Text = "System has found all matches. Files Checked: " + checkedFilesC.ToString() + "  Matches: " + foundFilesC.ToString();
            }

            counterC = PerformanceTimer.ElapsedMilliseconds;
            dblCounterC = ((double)counterC) / 1000;

            if (dblCounterC > 1)
            {
                lblPerf.Text = "Last Search Took: " + dblCounterC + " Seconds";
            }
            else
            {
                lblPerf.Text = "Last Search Took: " + PerformanceTimer.ElapsedMilliseconds + " Milliseconds";
            }

            counter2C = DisplayTimer.ElapsedMilliseconds;
            dblCounter2C = ((double)counter2C) / 1000;

            if (dblCounter2C > 1)
            {
                lblDisp.Text = "Display Results: " + dblCounter2 + " Seconds";
            }
            else
            {
                lblDisp.Text = "Display Results: " + DisplayTimer.ElapsedMilliseconds + " Milliseconds";
            }

            if ((dblCounterC + dblCounter2C) > 1)
            {
                lblTot.Text = "Total Time: " + (dblCounterC + dblCounter2C).ToString() + " Seconds";
            }
            else
            {
                lblTot.Text = "Total Time: " + (PerformanceTimer.ElapsedMilliseconds + DisplayTimer.ElapsedMilliseconds) + " Milliseconds";
            }

            funcEnableAllC();
        }

        //Searching Folders
        private void BWFS_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PerformanceTimer = Stopwatch.StartNew();

                findFRSearch(tbxFRDirectory.Text, tbxFRName.Text);

                PerformanceTimer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Holy shit we are going to die!\n  " + ex.Message);
            }
        }

        private void BWFS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchDirectories.Add(tbxFRDirectory.Text);
            SearchFiles.Add(tbxFRName.Text);

            if (FFRen.Any())
            {
                Fill_TreeFFR();
            }

            TreeNode[] foundNodes = treeFRFound.Nodes.Find("Load", false);

            if (foundNodes.Any())
            {
                foundNodes[0].Text = "System has found all matches. Files Checked: " + checkedFolders.ToString() + "  Matches: " + foundFolders.ToString();
            }

            counterFR = PerformanceTimer.ElapsedMilliseconds;
            dblCounterFR = ((double)counterFR) / 1000;

            if (dblCounterFR > 1)
            {
                lblPerf.Text = "Last Search Took: " + dblCounterFR + " Seconds";
            }
            else
            {
                lblPerf.Text = "Last Search Took: " + PerformanceTimer.ElapsedMilliseconds + " Milliseconds";
            }

            counter2FR = DisplayTimer.ElapsedMilliseconds;
            dblCounter2FR = ((double)counter2FR) / 1000;

            if (dblCounter2FR > 1)
            {
                lblDisp.Text = "Display Results: " + dblCounter2FR + " Seconds";
            }
            else
            {
                lblDisp.Text = "Display Results: " + DisplayTimer.ElapsedMilliseconds + " Milliseconds";
            }

            if ((dblCounterFR + dblCounter2FR) > 1)
            {
                lblTot.Text = "Total Time: " + (dblCounterFR + dblCounter2FR).ToString() + " Seconds";
            }
            else
            {
                lblTot.Text = "Total Time: " + (PerformanceTimer.ElapsedMilliseconds + DisplayTimer.ElapsedMilliseconds) + " Milliseconds";
            }

            funcEnableAllFR();
        }

        //MAIN Event Handler
        private void ButtonEventHandler(object sender, EventArgs e)
        {
            if (sender.Equals(btnSearchBrowse))
            {
                tbxFilePath.Text = getFolderPath();
            }
            else if (sender.Equals(btnSearchAll))
            {
                SearchAll();
            }
            else if (sender.Equals(btnExport))
            {
                if (FoundFileList.Any())
                {
                    funcWriteToSS();
                }
                else
                {
                    MessageBox.Show("You must conduct a valid search first.");
                }
            }
        }

        private void ButtonEventHandlerRename(object sender, EventArgs e)
        {
            if (sender.Equals(btnRenameBrowse))
            {
                tbxRenameDirectory.Text = getFolderPath();
            }
            else if (sender.Equals(btnRenamePreview))
            {
                if (FoundFileListR.Any())
                {
                    PreviewFileListR.Clear();

                    try
                    {
                        funcRenamePreview();

                        if (PreviewFileListR.Any())
                        {
                            btnRenameApprove.Enabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Rename Preview Exception: " + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Conduct a search first.");
                }
            }
            else if (sender.Equals(btnRenameApprove))
            {
                funcRename();
                btnRenameApprove.Enabled = false;
            }
            if (sender.Equals(btnRenameSearch))
            {
                SearchAllRename();
            }
        }

        private void ButtonEventHandlerCopy(object sender, EventArgs e)
        {
            if (sender.Equals(btnCopyBrowse))
            {
                tbxCopyDirectory.Text = getFolderPath();
            }
            else if (sender.Equals(btnCopyToBrowse))
            {
                tbxCopyDestination.Text = getFolderPath();
            }
            else if (sender.Equals(btnCopyPreview))
            {
                if (FoundFileListC.Any())
                {
                    PreviewFileListC.Clear();

                    try
                    {
                        funcCopyPreview();

                        if (PreviewFileListC.Any())
                        {
                            btnCopyApprove.Enabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Rename Preview Exception: " + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Conduct a search first.");
                }
            }
            else if (sender.Equals(btnCopyApprove))
            {
                funcCopy();
                btnCopyApprove.Enabled = false;
            }
            else if (sender.Equals(btnCopySearch))
            {
                SearchAllCopy();
            }
        }

        private void ButtonEventHandlerFR(object sender, EventArgs e)
        {
            if (sender.Equals(btnFRBrowse))
            {
                tbxFRDirectory.Text = getFolderPath();
            }
            else if (sender.Equals(btnFRPreview))
            {
                if (FFRen.Any())
                {
                    FFRPrev.Clear();

                    try
                    {
                        funcFRPreview();

                        if (FFRPrev.Any())
                        {
                            btnFRename.Enabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Rename Preview Exception: " + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Conduct a search first.");
                }
            }
            else if (sender.Equals(btnFRename))
            {
                funcFRename();
                btnFRename.Enabled = false;
            }
            else if (sender.Equals(btnFRSearch))
            {
                try
                {
                    if (tbxFRDirectory.Text != "")
                    {
                        if (tbxFRName.Text != "")
                        {
                            if (!(tbxFRDirectory.Text.EndsWith("\\")))
                            {
                                tbxFRDirectory.Text += "\\";
                            }

                            if (doesDirExist(tbxFRDirectory.Text))
                            {
                                if (!(BWFS.IsBusy))
                                {

                                    funcDisableAllFR();
                                    clearAllFR();
                                    treeFRFound.Nodes.Clear();

                                    //MessageBox.Show("This is executing.");
                                    BWFS.RunWorkerAsync();

                                    treeFRFound.Nodes.Add("Load", "System is searching for matches...");
                                }
                                else
                                {
                                    MessageBox.Show("Chill out, program is still loading the previous data.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid filename.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid directory.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception Occured: " + ex.Message + "\n");
                }
            }
        }

        //MISC
        private Boolean doesDirExist()
        {
            Boolean doesExist = false;

            if (Directory.Exists(tbxFilePath.Text))
            {
                doesExist = true;
            }

            return doesExist;
        }

        private Boolean doesDirExist(String FilePath)
        {
            Boolean doesExist = false;

            if (Directory.Exists(FilePath))
            {
                doesExist = true;
            }

            return doesExist;
        }

        private String getFolderPath()
        {
            String temp = "";
            /*
            FolderBrowserDialog OFD = new FolderBrowserDialog();
            OFD.SelectedPath = "\\\\TOWNAS\\ARTDEPT\\";
            OFD.RootFolder = System.Environment.SpecialFolder.Desktop;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                temp = Path.GetFullPath(OFD.SelectedPath);
            }*/

            FolderBrowserDialogEx FBDX = new FolderBrowserDialogEx();
            FBDX.SelectedPath = "C:\\";

            if (FBDX.ShowDialog() == DialogResult.OK)
            {
                temp = Path.GetFullPath(FBDX.SelectedPath);
            }

            return temp;
        }

        private void EventEnterKeyDown(object sender, KeyEventArgs e)
        {
            if (sender.Equals(tbxFileName))
            {
                if (e.KeyCode == Keys.Return)
                {
                    SearchAll();
                }
            }
            else if (sender.Equals(tbxCopyName))
            {
                if (e.KeyCode == Keys.Return)
                {
                    SearchAllCopy();
                }
            }
            else if (sender.Equals(tbxRenameName))
            {
                if (e.KeyCode == Keys.Return)
                {
                    SearchAllRename();
                }
            }
            else if (sender.Equals(tbxFRName))
            {
                if (e.KeyCode == Keys.Return)
                {
                    SearchAllFR();
                }
            }
        }

        private void SearchAll()
        {
            try
            {
                if (tbxFilePath.Text != "")
                {
                    if (tbxFileName.Text != "")
                    {
                        if (!(tbxFilePath.Text.EndsWith("\\")))
                        {
                            tbxFilePath.Text += "\\";
                        }

                        if (doesDirExist())
                        {
                            if (!(BW1.IsBusy))
                            {
                                funcDisableAll();
                                clearAll();
                                treeFOUND.Nodes.Clear();

                                FileType = tbxFileExt.Text;

                                //MessageBox.Show("This is executing.");
                                BW1.RunWorkerAsync();

                                treeFOUND.Nodes.Add("Load", "System is searching for matches...");

                            }
                            else
                            {
                                MessageBox.Show("Chill out, program is still loading the previous data.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Directory could not be found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid filename.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid directory.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occured: " + ex.Message + "\n");
            }
        }

        private void SearchAllCopy()
        {
            try
            {
                if (tbxCopyDirectory.Text != "")
                {
                    if (tbxCopyName.Text != "")
                    {
                        if (!(tbxCopyDirectory.Text.EndsWith("\\")))
                        {
                            tbxCopyDirectory.Text += "\\";
                        }

                        if (doesDirExist(tbxCopyDirectory.Text))
                        {
                            if (!(BW1C.IsBusy))
                            {
                                funcDisableAllC();
                                clearAllC();
                                treeCopyFound.Nodes.Clear();

                                FileType3 = tbxFileExtC.Text;

                                BW1C.RunWorkerAsync();

                                treeCopyFound.Nodes.Add("Load", "System is searching for matches...");
                            }
                            else
                            {
                                MessageBox.Show("Chill out, program is still loading the previous data.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Directory could not be found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid filename.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid directory.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occured: " + ex.Message + "\n");
            }
        }

        private void SearchAllRename()
        {
            try
            {
                if (tbxRenameDirectory.Text != "")
                {
                    if (tbxRenameName.Text != "")
                    {
                        if (!(tbxRenameDirectory.Text.EndsWith("\\")))
                        {
                            tbxRenameDirectory.Text += "\\";
                        }

                        if (doesDirExist(tbxRenameDirectory.Text))
                        {
                            if (!(BW1R.IsBusy))
                            {
                                funcDisableAllR();
                                clearAllR();
                                treeRenameFound.Nodes.Clear();

                                FileType2 = tbxFileExtR.Text;

                                BW1R.RunWorkerAsync();

                                treeRenameFound.Nodes.Add("Load", "System is searching for matches...");

                            }
                            else
                            {
                                MessageBox.Show("Chill out, program is still loading the previous data.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Directory could not be found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid filename.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid directory.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occured: " + ex.Message + "\n");
            }
        }

        private void SearchAllFR()
        {
            try
            {
                if (tbxFRDirectory.Text != "")
                {
                    if (tbxFRName.Text != "")
                    {
                        if (!(tbxFRDirectory.Text.EndsWith("\\")))
                        {
                            tbxFRDirectory.Text += "\\";
                        }

                        if (doesDirExist(tbxFRDirectory.Text))
                        {
                            if (!(BWFS.IsBusy))
                            {
                                funcDisableAllFR();
                                clearAllFR();
                                treeFRFound.Nodes.Clear();

                                //MessageBox.Show("This is executing.");
                                BWFS.RunWorkerAsync();

                                treeFRFound.Nodes.Add("Load", "System is searching for matches...");
                            }
                            else
                            {
                                MessageBox.Show("Chill out, program is still loading the previous data.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Directory could not be found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid filename.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid directory.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occured: " + ex.Message + "\n");
            }
        }

        //Event for opening the file in the Search Results
        private void funcOpenFileNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name != "Load")
            {
                String NodeFileName = "";

                NodeFileName = e.Node.Text;

                if (File.Exists(NodeFileName))
                {
                    Process.Start(NodeFileName);
                }
                else
                {
                    MessageBox.Show("File was not found, therefore, it cannot be opened.");
                }
            }
        }

        private void funcContextMenuNode(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void funcWriteToSS()
        {
            try
            {
                String FileName = "";
                SaveFileDialog SFD = new SaveFileDialog();

                SFD.Filter = "csv files (*.csv)|*.csv";
                SFD.FilterIndex = 2;
                SFD.RestoreDirectory = true;

                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    FileName = SFD.FileName;
                    StreamWriter newStream = new StreamWriter(FileName);

                    if (FoundFileList.Any())
                    {
                        foreach (String temp in FoundFileList)
                        {
                            String FName = "";
                            String Ext = "";

                            FName = temp.Replace("\\\\", "\\");

                            String[] strArray = FName.Split('\\');

                            FName = strArray[(strArray.Length - 1)];

                            strArray = null;

                            strArray = FName.Split('.');

                            if (strArray.Length == 2)
                            {
                                Ext = strArray[1];
                            }

                            newStream.WriteLine(FName + ", " + "." + Ext + ", " + temp);
                        }
                    }

                    newStream.Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error occured writing a general CSV." + Environment.NewLine +
                            Environment.NewLine + "Exception Message: " + Ex.Message +
                            Environment.NewLine + "Message Stack: " + Ex.StackTrace);
            }
        }

        //SEARCH Section
        private void Fill_Tree()
        {
            if (cboxLimit.Checked)
            {
                if (FoundFileList.Count > 1000)
                {
                    DisplayTimer = Stopwatch.StartNew();
                    treeFOUND.BeginUpdate();
                    for (int i = 0; i < 1000; i++)
                    {
                        String item = FoundFileList.ElementAt(i);
                        treeFOUND.Nodes[0].Nodes.Add(item, item);
                    }
                    treeFOUND.EndUpdate();

                    DisplayTimer.Stop();
                }
                else
                {
                    DisplayTimer = Stopwatch.StartNew();
                    treeFOUND.BeginUpdate();
                    for (int i = 0; i < FoundFileList.Count; i++)
                    {
                        String item = FoundFileList.ElementAt(i);
                        treeFOUND.Nodes[0].Nodes.Add(item, item);
                    }
                    treeFOUND.EndUpdate();

                    DisplayTimer.Stop();
                }
            }
            else
            {
                DisplayTimer = Stopwatch.StartNew();
                treeFOUND.BeginUpdate();
                foreach (String item in FoundFileList)
                {
                    treeFOUND.Nodes[0].Nodes.Add(item, item);
                }
                treeFOUND.EndUpdate();

                DisplayTimer.Stop();
            }
        }

        private void findAllFiles(String FilePath, String FileName)
        {
            foundFiles = 0;
            checkedFiles = 0;

            if (FileName.Contains("*"))
            {
                funcFileSearch(FilePath, FileName);
            }
            else
            {
                funcFileSearch(FilePath, "*" + FileName + "*");
            }
        }

        private void findAllFilesStrict(String FilePath, String FileName)
        {
            foundFiles = 0;
            checkedFiles = 0;

            if (FileName.Contains("*"))
            {
                funcFileSearch(FilePath, FileName);
            }
            else
            {
                funcFileSearch(FilePath, FileName);
            }
        }

        private void funcFileSearch(String FilePath, String FileName)
        {
            if ((FileType == "ANY") || (FileType == ""))
            {
                foreach (String foundfile in SafeDirectoryWalk.EnumerateFiles(FilePath, FileName, SearchOption.AllDirectories).ToList())
                {

                    FoundFileList.Add(foundfile);
                    foundFiles++;

                    checkedFiles++;
                }
            }
            else if ((FileType != "ANY") && (FileType != ""))
            {
                foreach (String foundfile in SafeDirectoryWalk.EnumerateFiles(FilePath, FileName, SearchOption.AllDirectories).ToList())
                {
                    //MessageBox.Show("File " + FileName + " was found.");
                    if ((foundfile.Contains(FileType.ToLower())) || (foundfile.Contains(FileType.ToUpper())))
                    {
                        FoundFileList.Add(foundfile);
                        foundFiles++;
                    }

                    checkedFiles++;
                }
            }
        }

        public static class SafeDirectoryWalk
        {
            public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOpt)
            {
                try
                {
                    var dirFiles = Enumerable.Empty<string>();
                    
                    if (searchOpt == SearchOption.AllDirectories)
                    {
                        dirFiles = Directory.EnumerateDirectories(path)
                                            .SelectMany(x => EnumerateFiles(x, searchPattern, searchOpt));
                    }

                    return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));
                }
                catch (UnauthorizedAccessException)
                {
                    return Enumerable.Empty<string>();
                }
                catch (PathTooLongException)
                {
                    return Enumerable.Empty<string>();
                }
            }

            public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOpt)
            {
                try
                {
                    var dirFiles = Enumerable.Empty<string>();
                    
                    if (searchOpt == SearchOption.AllDirectories)
                    {
                        dirFiles = Directory.EnumerateDirectories(path)
                                            .SelectMany(x => EnumerateDirectories(x, searchPattern, searchOpt));
                    }

                    return dirFiles.Concat(Directory.EnumerateDirectories(path, searchPattern));
                }
                catch (UnauthorizedAccessException)
                {
                    return Enumerable.Empty<string>();
                }
                catch (PathTooLongException)
                {
                    return Enumerable.Empty<string>();
                }
            }
        }

        //RENAMING Section
        private void Fill_TreeR()
        {
            DisplayTimer = Stopwatch.StartNew();
            treeRenameFound.BeginUpdate();

            for (int i = 0; i < FoundFileListR.Count; i++)
            {
                String item = FoundFileListR.ElementAt(i);
                treeRenameFound.Nodes[0].Nodes.Add(item, item);
            }
            treeRenameFound.EndUpdate();

            DisplayTimer.Stop();
        }

        private void funcRename()
        {
            if (FoundFileListR.Any())
            {
                try
                {
                    for (int i = 0; i < FoundFileListR.Count; i++)
                    {
                        File.Move(FoundFileListR.ElementAt(i), PreviewFileListR.ElementAt(i));
                    }

                    if (checkedReload.Checked)
                    {
                        FoundFileListR.Clear();
                        FoundFileListR = PreviewFileListR.ToList();

                        treeRenameFound.Nodes.Clear();

                        if (FoundFileListR.Any())
                        {
                            treeRenameFound.Nodes.Add("Load", "Renamed Folders Reloaded: " + FoundFileListR.Count);

                            Fill_TreeR();
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Folder Rename Exception: " + Ex.Message + "\n\n" +
                                    Ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("You have to preview a rename first!");
            }
        }

        private void funcRenamePreview()
        {
            String Temp = "";
            Int32 copyCount = 1;

            PreviewFileListR.Clear();
            treePREV.Nodes.Clear();

            if (funcGetRenameInfo())
            {
                foreach (String item in FoundFileListR)
                {
                    Temp = getNewName(item);

                    if ((PreviewFileListR.Contains(Temp)) && (Temp != ""))
                    {
                        PreviewFileListR.Add(Temp + " Copy " + copyCount);
                        copyCount++;
                    }
                    else if (Temp != "")
                    {
                        PreviewFileListR.Add(Temp);
                    }
                }

                funcDisplayPreview();
            }
            else
            {
                MessageBox.Show("Make sure to enter valid renaming information.");
            }
        }

        private void funcDisplayPreview()
        {
            if (PreviewFileListR.Any())
            {
                treePREV.Nodes.Add("Prev", "Preview List");
                int i = 0;

                foreach (String item in PreviewFileListR)
                {
                    treePREV.Nodes[0].Nodes.Add(item, item);
                    i++;
                }

                treePREV.Nodes[0].Text = "Preview List - Total Renames Pending: " + i.ToString();
            }
        }

        private Boolean funcGetRenameInfo()
        {
            Boolean GoodToContinue = false;

            if (tbxRenamePart.Text != "")
            {
                RenamePart = tbxRenamePart.Text;
                RenameTo = tbxRenameTo.Text;
                GoodToContinue = true;
            }

            return GoodToContinue;
        }

        private String getNewName(String Input)
        {
            String NewName = "";

            try
            {
                if (!(Input.Contains("Thumbs.db")) && (!(Input.Contains(".DS_Store"))))
                {
                    //MessageBox.Show("Rename Part: " + RenamePart + "  Which is " + RenamePart.Length.ToString() + " characters long.");
                    if (RenamePart == "*_")
                    {
                        String FilePath = Path.GetDirectoryName(Input);
                        String OldName = Input.Replace(FilePath, "");
                        OldName = OldName.Replace("\\", "");

                        //Console.WriteLine("Rename Part: " + RenamePart + "  Which is " + RenamePart.Length.ToString() + " characters long.");
                        //Console.WriteLine("Rename To: " + RenameTo + "  Which is " + RenameTo.Length.ToString() + " characters long.");
                        //Console.WriteLine("RP User Entered: " + tbxRenamePart.Text + "  Which is " + tbxRenamePart.Text.Length.ToString() + " characters long.");
                        //Console.WriteLine("RT User Entered: " + tbxRenameTo.Text + "  Which is " + tbxRenameTo.Text.Length.ToString() + " characters long.");
                        //Console.WriteLine("FilePath: " + FilePath + " OldName: " + OldName);

                        NewName = RenameTo + OldName;
                        //Console.WriteLine("File Rename: " + NewName);

                        NewName = FilePath + "\\" + NewName;
                        //Console.WriteLine("File with Path Rename: " + NewName);

                        //Console.WriteLine("New Batch Name: " + NewName);
                    }
                    else if (RenamePart == "_*")
                    {
                        String FilePath = Path.GetDirectoryName(Input);
                        String OldName = Input.Replace(FilePath, "");
                        OldName = OldName.Replace("\\", "");

                        if (OldName.Contains("."))
                        {
                            String[] tempArray = OldName.Split('.');

                            if (tempArray.Any())
                            {
                                NewName = tempArray[0] + RenameTo + "." + tempArray[1];
                            }
                        }
                        else
                        {
                            NewName = OldName + RenameTo;
                        }

                        NewName = FilePath + "\\" + NewName;
                    }
                    else
                    {
                        //Console.WriteLine("Rename Part: " + RenamePart + "  Which is " + RenamePart.Length.ToString() + " characters long.");
                        //Console.WriteLine("User Entered: " + tbxRenamePart.Text + "  Which is " + tbxRenamePart.Text.Length.ToString() + " characters long.");

                        String FilePath = Path.GetDirectoryName(Input);
                        String OldName = Input.Replace(FilePath, "");
                        NewName = OldName.Replace(RenamePart, RenameTo);

                        NewName = FilePath + NewName;
                    }
                }
            }
            catch (Exception FRE)
            {
                MessageBox.Show("File Rename Exception: " + FRE.Message + "\n\n" + FRE.StackTrace);
            }

            return NewName;
        }

        private void findAllFilesR(String FilePath, String FileName)
        {
            foundFilesR = 0;
            checkedFilesR = 0;

            if (FileName.Contains("*"))
            {
                funcFileSearchR(FilePath, FileName);
            }
            else
            {
                funcFileSearchR(FilePath, "*" + FileName + "*");
            }
        }

        private void funcFileSearchR(String FilePath, String FileName)
        {
            if ((FileType2 == "ANY") || (FileType2 == ""))
            {
                foreach (String foundfile in SafeDirectoryWalk.EnumerateFiles(FilePath, FileName, SearchOption.AllDirectories).ToList())
                {
                    FoundFileList.Add(foundfile);
                    foundFiles++;

                    checkedFiles++;
                }
            }
            else if ((FileType2 != "ANY") && (FileType2 != ""))
            {
                foreach (String foundfile in Directory.EnumerateFiles(FilePath, FileName, SearchOption.AllDirectories).ToList())
                {
                    //MessageBox.Show("File " + FileName + " was found.");
                    if ((foundfile.Contains(FileType2.ToLower())) || (foundfile.Contains(FileType2.ToUpper())))
                    {
                        FoundFileList.Add(foundfile);
                        foundFiles++;
                    }

                    checkedFiles++;
                }
            }
        }

        //FOLDER RENAMING Section
        private void funcFRPreview()
        {
            String Temp = "";

            FFRPrev.Clear();
            treeFRPREV.Nodes.Clear();

            //Convert the names and add them to the list.
            foreach (String item in FFRen)
            {
                Temp = getNewNameFR(item);

                if (Temp != "")
                {
                    FFRPrev.Add(Temp);
                }
            }

            funcFRDisplay();
        }

        private void funcFRDisplay()
        {
            if (FFRPrev.Any())
            {
                treeFRPREV.Nodes.Add("Prev", "Preview List");
                int i = 0;

                foreach (String item in FFRPrev)
                {
                    treeFRPREV.Nodes[0].Nodes.Add(item, item);
                    i++;
                }

                treeFRPREV.Nodes[0].Text = "Preview List - Total Renames Pending: " + i.ToString();
            }
        }

        private void findFRSearch(String FilePath, String FolderName)
        {
            foundFolders = 0;
            checkedFolders = 0;

            if (FolderName.Contains("*"))
            {
                funcFolderSearch(FilePath, FolderName);
            }
            else
            {
                funcFolderSearch(FilePath, "*" + FolderName + "*");
            }
        }

        private void funcFolderSearch(String FilePath, String FolderName)
        {
            foreach (String foundFolder in SafeDirectoryWalk.EnumerateDirectories(FilePath, FolderName, SearchOption.AllDirectories).ToList())
            {
                FFRen.Add(foundFolder);
                foundFolders++;

                checkedFolders++;
            }
        }

        private void funcFRename()
        {
            if (FFRPrev.Any())
            {
                try
                {
                    for (int i = 0; i < FFRPrev.Count; i++)
                    {
                        //When using wildcards, you may get folders in the folder foundlist
                        //that are folders but not matching what you want to rename.
                        //Since this is a "Move" command and it goes through the entire list
                        //moving folders, folders not renamed and also moved creates exceptions.
                        if (((FFRen.ElementAt(i).Contains(tbxFRPart.Text)) || (tbxFRPart.Text == "*_")) || (tbxFRPart.Text == "_*"))
                        {
                            Directory.Move(FFRen.ElementAt(i), FFRPrev.ElementAt(i));
                        }
                    }

                    FFRen.Clear();
                    FFRen = FFRPrev.ToList();

                    treeFRFound.Nodes.Clear();

                    if (FFRen.Any())
                    {
                        treeFRFound.Nodes.Add("Load", "Renamed Folders Reloaded: " + FFRen.Count);

                        Fill_TreeFFR();
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Folder Rename Exception: " + Ex.Message + "\n\n" +
                                    Ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("You have to preview a rename first!");
            }
        }

        private void Fill_TreeFFR()
        {
            DisplayTimer = Stopwatch.StartNew();
            treeFRFound.BeginUpdate();

            for (int i = 0; i < FFRen.Count; i++)
            {
                String item = FFRen.ElementAt(i);
                treeFRFound.Nodes[0].Nodes.Add(item, item);
            }

            treeFRFound.EndUpdate();

            DisplayTimer.Stop();
        }

        private void Fill_TreeFFPreview()
        {
            DisplayTimer = Stopwatch.StartNew();
            treeFRPREV.BeginUpdate();

            for (int i = 0; i < FFRPrev.Count; i++)
            {
                String item = FFRPrev.ElementAt(i);
                treeFRPREV.Nodes[0].Nodes.Add(item, item);
            }

            treeFRPREV.EndUpdate();

            DisplayTimer.Stop();
        }

        private Boolean funcGetRenameInfoFR()
        {
            Boolean GoodToContinue = false;

            if (tbxFRPart.Text != "")
            {
                RenamePart = tbxFRPart.Text;
                RenameTo = tabControl.Text;
                GoodToContinue = true;
            }

            return GoodToContinue;
        }

        private String getNewNameFR(String Input)
        {
            String NewName = "";

            try
            {
                if (tbxFRTo.Text == "*_")
                {
                    NewName = tbxFRTo.Text + tbxFRPart.Text;
                    NewName = Input.Replace(tbxFRPart.Text, NewName);
                }
                else if (tbxFRTo.Text == "_*")
                {
                    NewName = tbxFRPart.Text + tbxFRTo.Text;
                    NewName = Input.Replace(tbxFRPart.Text, NewName);
                }
                else
                {
                    NewName = Input.Replace(tbxFRPart.Text, tbxFRTo.Text);
                }
            }
            catch (Exception FRE)
            {
                MessageBox.Show("File Rename Exception: " + FRE.Message + "\n\n" + FRE.StackTrace);
            }

            return NewName;
        }

        //COPY Section
        private void Fill_TreeC()
        {
            DisplayTimer = Stopwatch.StartNew();
            treeCopyFound.BeginUpdate();

            for (int i = 0; i < FoundFileListC.Count; i++)
            {
                String item = FoundFileListC.ElementAt(i);
                treeCopyFound.Nodes[0].Nodes.Add(item, item);
            }
            treeCopyFound.EndUpdate();

            DisplayTimer.Stop();
        }

        private void funcCopyPreview()
        {
            PreviewFileListC.Clear();
            treeCopyPreview.Nodes.Clear();

            if (tbxCopyDestination.Text != "")
            {
                CopyTo = tbxCopyDestination.Text;

                if (Directory.Exists(CopyTo))
                {
                    foreach (String item in FoundFileListC)
                    {
                        PreviewFileListC.Add(getCopyToFileNamePath(item));
                    }

                    funcDisplayCopyPreview();
                }
                else
                {
                    MessageBox.Show("Destination does not exist! Try a new folder destination to copy to.");
                }

            }
            else
            {
                MessageBox.Show("Make sure to enter a Copy To destination.");
            }
        }

        private void funcDisplayCopyPreview()
        {
            if (PreviewFileListC.Any())
            {
                treeCopyPreview.Nodes.Add("Prev", "Preview List");
                int i = 0;

                foreach (String item in PreviewFileListC)
                {
                    treeCopyPreview.Nodes[0].Nodes.Add(item, item);
                    i++;
                }

                treeCopyPreview.Nodes[0].Text = "Preview List - Total Files to Copy: " + i.ToString();
            }
        }

        private String getCopyToFileNamePath(String Input)
        {
            String NewPath = "";

            if (CopyTo.Contains(".\\"))
            {
                CopyTo = CopyTo.Replace(".\\", "");

                String FilePath = FilePathR + "\\" + CopyTo;
                String OldName = Path.GetFileName(Input);
                OldName = OldName.Replace("\\", "");

                NewPath = FilePath + "\\" + OldName;
            }
            else
            {
                String FilePath = CopyTo;
                String OldName = Path.GetFileName(Input);
                OldName = OldName.Replace("\\", "");

                NewPath = FilePath + "\\" + OldName;
            }

            return NewPath;
        }

        private void findAllFilesC(String FilePath, String FileName)
        {
            foundFilesC = 0;
            checkedFilesC = 0;

            if (FileName.Contains("*"))
            {
                funcFileSearchC(FilePath, FileName);
            }
            else
            {
                funcFileSearchC(FilePath, "*" + FileName + "*");
            }
        }

        private void funcFileSearchC(String FilePath, String FileName)
        {
            if ((FileType3 == "ANY") || (FileType3 == ""))
            {
                foreach (String foundfile in Directory.EnumerateFiles(FilePath, FileName, SearchOption.AllDirectories).ToList())
                {
                    FoundFileListC.Add(foundfile);
                    foundFilesC++;

                    checkedFilesC++;
                }
            }
            else if ((FileType3 != "ANY") && (FileType3 != ""))
            {
                foreach (String foundfile in Directory.EnumerateFiles(FilePath, FileName, SearchOption.AllDirectories).ToList())
                {
                    //MessageBox.Show("File " + FileName + " was found.");
                    if ((foundfile.Contains(FileType3.ToLower())) || (foundfile.Contains(FileType3.ToUpper())))
                    {
                        FoundFileList.Add(foundfile);
                        foundFiles++;
                    }

                    checkedFilesC++;
                }
            }
        }

        private void funcCopy()
        {
            if (PreviewFileListC.Any())
            {
                try
                {
                    for (int i = 0; i < PreviewFileListC.Count; i++)
                    {
                        File.Copy(FoundFileListC.ElementAt(i), PreviewFileListC.ElementAt(i));
                    }

                    if (checkedReload2.Checked)
                    {
                        FoundFileListC.Clear();
                        FoundFileListC = PreviewFileListC.ToList();

                        treeCopyFound.Nodes.Clear();

                        if (FoundFileListC.Any())
                        {
                            treeCopyFound.Nodes.Add("Load", "Copied Files Reloaded: " + FoundFileListC.Count);

                            Fill_TreeC();
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("File Copy To Exception: " + Ex.Message + "\n\n" +
                                    "You received this message because one of the original files probably has been moved or no longer exists!");
                }
            }
            else
            {
                MessageBox.Show("You have to preview a copy command first!");
            }
        }

        //UI Data & Component Operations
        private void funcDisableAll()
        {
            btnSearchAll.Enabled = false;
        }

        private void funcDisableAllR()
        {
            btnRenameSearch.Enabled = false;
            btnRenamePreview.Enabled = false;
            btnRenameApprove.Enabled = false;
        }

        private void funcDisableAllC()
        {
            btnCopySearch.Enabled = false;
            btnCopyPreview.Enabled = false;
            btnCopyApprove.Enabled = false;
        }

        private void funcDisableAllFR()
        {
            btnFRBrowse.Enabled = false;
            btnFRSearch.Enabled = false;
            btnFRPreview.Enabled = false;
        }

        private void funcEnableAll()
        {
            btnSearchAll.Enabled = true;
        }

        private void funcEnableAllR()
        {
            btnRenameSearch.Enabled = true;
            btnRenamePreview.Enabled = true;
        }

        private void funcEnableAllC()
        {
            btnCopySearch.Enabled = true;
            btnCopyPreview.Enabled = true;
        }

        private void funcEnableAllFR()
        {
            btnFRBrowse.Enabled = true;
            btnFRSearch.Enabled = true;
            btnFRPreview.Enabled = true;
        }

        private void clearAll()
        {
            FoundFileList.Clear();

            foundFiles = 0;
            checkedFiles = 0;
        }

        private void clearAllR()
        {
            FilePathR = "";
            FoundFileListR.Clear();

            foundFilesR = 0;
            checkedFilesR = 0;
        }

        private void clearAllC()
        {
            //FilePathC = "";
            FoundFileListC.Clear();

            foundFilesC = 0;
            checkedFilesC = 0;
        }

        private void clearAllFR()
        {
            FFRen.Clear();

            foundFolders = 0;
            checkedFolders = 0;
        }

    }
}
