using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private int nIndex = 0;
        public Form1 ()
        {
            InitializeComponent();
        }

        private void button1_Click (object sender, EventArgs e)
        {
            IntPtr hImgSmall; //the handle to the system image list
            IntPtr hImgLarge; //the handle to the system image list
            string fName; //  'the file name to get icon from
            SHFILEINFO shinfo = new SHFILEINFO();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\temp\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            listView1.SmallImageList = imageList1;
            listView1.LargeImageList = imageList1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fName = openFileDialog1.FileName;
                //Use this to get the small Icon
                //hImgSmall = Win32.SHGetFileInfo(fName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);

                //Use this to get the large Icon
                hImgLarge = Win32.SHGetFileInfo(fName, 0, 
                	ref shinfo, (uint)Marshal.SizeOf(shinfo), 
                	Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);

                //The icon is returned in the hIcon member of the shinfo struct
                System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

                imageList1.Images.Add(myIcon);

                //Add file name and icon to listview
                listView1.Items.Add(fName, nIndex++);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo (string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
    }
}
