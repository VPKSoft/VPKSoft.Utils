using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKSoft.Utils;
using System.Diagnostics;

namespace VPKSoft.UtilsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = VPKSoft.Utils.SysIcons.GetSystemIconBitmap(SysIcons.SystemIconType.Shield, new Size(16, 16));
        }



        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IEnumerable<FileEnumeratorFileEntry> fileEntries = FileEnumerator.RecurseFiles(@"C:\Files\Torrents\Arrow Season 6", FileEnumerator.FiltersVideoVlcNoBinNoIso);
            foreach (FileEnumeratorFileEntry entry in fileEntries)
            {
                MessageBox.Show(entry.FileName);
            }
        }
    }
}

/*

URL Reservations: 
----------------- 

    Reserved URL            : http://+:11316/ 
        User: \Everyone
            Listen: Yes
            Delegate: No
            SDDL: D:(A;;GX;;;WD) 


*/

/*
C:\WINDOWS\system32>netsh http add urlacl url=http://+:11316/ user=Everyone

Url reservation add failed, Error: 183
Cannot create a file when that file already exists.
*/

//URL reservation successfully deleted