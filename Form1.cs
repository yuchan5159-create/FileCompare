using System.IO;
using System.Linq;

namespace FileCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Ensure ListViews show their column headers and have expected behavior
            lvwLeftDir.View = View.Details;
            lvwLeftDir.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvwLeftDir.FullRowSelect = true;
            lvwLeftDir.GridLines = true;

            lvwRightDir.View = View.Details;
            lvwRightDir.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvwRightDir.FullRowSelect = true;
            lvwRightDir.GridLines = true;

            // Add visible header panels above each ListView so column titles are always visible
            var leftHeaderPanel = new Panel { Dock = DockStyle.Top, Height = 24 };
            var leftHeaderLabel = new Label { Dock = DockStyle.Fill, Text = "이름    크기    수정일", TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(6, 0, 0, 0) };
            leftHeaderPanel.Controls.Add(leftHeaderLabel);
            // Insert at top of panel4 controls so it appears above the ListView
            panel4.Controls.Add(leftHeaderPanel);
            panel4.Controls.SetChildIndex(leftHeaderPanel, 0);

            var rightHeaderPanel = new Panel { Dock = DockStyle.Top, Height = 24 };
            var rightHeaderLabel = new Label { Dock = DockStyle.Fill, Text = "이름    크기    수정일", TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(6, 0, 0, 0) };
            rightHeaderPanel.Controls.Add(rightHeaderLabel);
            panel10.Controls.Add(rightHeaderPanel);
            panel10.Controls.SetChildIndex(rightHeaderPanel, 0);
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            try
            {
                if (lvwLeftDir.Columns.Count >= 3)
                {
                    lvwLeftDir.Columns[0].Width = 300;
                    lvwLeftDir.Columns[1].Width = 100;
                    // make the last column fill remaining width
                    lvwLeftDir.Columns[2].Width = 160;
                }

                if (lvwRightDir.Columns.Count >= 3)
                {
                    lvwRightDir.Columns[0].Width = 300;
                    lvwRightDir.Columns[1].Width = 100;
                    lvwRightDir.Columns[2].Width = 160;
                }
            }

        private void PopulateListView(ListView lv, string folderPath)
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                    return;

                var dirs = Directory.EnumerateDirectories(folderPath)
                                    .Select(p => new DirectoryInfo(p))
                                    .OrderBy(d => d.Name);
                foreach (var d in dirs)
                {
                    var item = new ListViewItem(d.Name);
                    item.SubItems.Add("<DIR>");
                    item.SubItems.Add(d.LastWriteTime.ToString("g"));
                    lv.Items.Add(item);
                }
            }
            catch
            {
                // ignore errors
            }
            finally
            {
                lv.EndUpdate();
            }
        }
            catch
            {
                // ignore layout errors
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요."; // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                }
            }
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요."; // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                }
            }
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요."; // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                }
            }
        }
        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            private void PopulateListView(ListView lv, string folderPath)
        {
            lv.BeginUpdate(); 
lv.Items.Clear(); 
    try { // 폴더(디렉터리) 먼저 추가var dirs = Directory.EnumerateDirectories(folderPath)
          .Select(p => new DirectoryInfo(p)), .OrderBy(d => d.Name);
foreach (var d in dirs) {
var item = new ListViewItem(d.Name);
item.SubItems.Add("<DIR>");

item.SubItems.Add(d.LastWriteTime.ToString("g"));
lv.Items.Add(item);}
            }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요."; // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                }
            }
        }

    }

}
