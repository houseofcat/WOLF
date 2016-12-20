using System.Windows.Forms;

namespace Wolf.WolfSpec.MainGUI
{
    public partial class Known : Form
    {
        public Known()
        {
            InitializeComponent();

            rtbKnownIssues.Text = "Last Updated: 2/20/2016\n\n" +
                "1.) Oracle is being redesigned.\n" +
                "2.) RCLI was deleted and replaced.\n" +
                "3.) General performance on Windows Server has been lacking and or buggy.\n" +
                "4.) Computer Info Reports performance can be lacking. Looking for ways of improving data binding.";
        }
    }
}
