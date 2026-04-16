using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

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
            lvwLeftDir.OwnerDraw = true;
            lvwLeftDir.DrawItem += ListView_DrawItem;
            lvwLeftDir.DrawSubItem += ListView_DrawSubItem;
            lvwLeftDir.DrawColumnHeader += ListView_DrawColumnHeader;
            lvwLeftDir.HideSelection = false;
            lvwLeftDir.GridLines = true;

            lvwRightDir.View = View.Details;
            lvwRightDir.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvwRightDir.FullRowSelect = true;
            lvwRightDir.OwnerDraw = true;
            lvwRightDir.DrawItem += ListView_DrawItem;
            lvwRightDir.DrawSubItem += ListView_DrawSubItem;
            lvwRightDir.DrawColumnHeader += ListView_DrawColumnHeader;
            lvwRightDir.HideSelection = false;
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

            // If initial paths are present, populate
            if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                PopulateListView(lvwLeftDir, txtLeftDir.Text);
            if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                PopulateListView(lvwRightDir, txtRightDir.Text);

            // Add context menus for copying selected items to the opposite folder
            CreateContextMenus();
        }

        private void CreateContextMenus()
        {
            var ctxLeft = new ContextMenuStrip();
            var miLeftToRight = new ToolStripMenuItem("선택 항목을 오른쪽으로 복사");
            miLeftToRight.Click += (s, e) => CopySelectedItems(lvwLeftDir, txtLeftDir.Text, txtRightDir.Text);
            ctxLeft.Items.Add(miLeftToRight);
            lvwLeftDir.ContextMenuStrip = ctxLeft;

            var ctxRight = new ContextMenuStrip();
            var miRightToLeft = new ToolStripMenuItem("선택 항목을 왼쪽으로 복사");
            miRightToLeft.Click += (s, e) => CopySelectedItems(lvwRightDir, txtRightDir.Text, txtLeftDir.Text);
            ctxRight.Items.Add(miRightToLeft);
            lvwRightDir.ContextMenuStrip = ctxRight;
        }

        private void CopySelectedItems(ListView sourceList, string sourceFolder, string destFolder)
        {
            if (string.IsNullOrWhiteSpace(sourceFolder) || !Directory.Exists(sourceFolder))
            {
                MessageBox.Show("복사할 원본 폴더가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(destFolder) || !Directory.Exists(destFolder))
            {
                MessageBox.Show("대상 폴더가 없습니다. 먼저 대상 폴더를 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selected = sourceList.SelectedItems.Cast<ListViewItem>().ToList();
            if (!selected.Any())
            {
                MessageBox.Show("복사할 항목을 선택하세요.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // collect files to copy (ignore folders)
            var files = selected.Where(it => it.SubItems.Count > 1 && it.SubItems[1].Text != "<DIR>")
                                .Select(it => it.Text)
                                .ToList();

            if (!files.Any())
            {
                MessageBox.Show("선택한 항목 중 복사 가능한 파일이 없습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int success = 0, failed = 0, skipped = 0;
            bool cancelled = false;
            foreach (var file in files)
            {
                var src = Path.Combine(sourceFolder, file);
                var dst = Path.Combine(destFolder, file);
                try
                {
                    if (File.Exists(dst))
                    {
                        var msg = "대상에 동일한 이름의 파일이 이미 있습니다. 덮어쓰시겠습니까?";
                        var res = MessageBox.Show(msg, "파일 충돌", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (res == DialogResult.Cancel)
                        {
                            cancelled = true;
                            break;
                        }
                        if (res == DialogResult.No)
                        {
                            skipped++;
                            continue;
                        }
                        // Yes => overwrite
                    }

                    File.Copy(src, dst, true);
                    success++;
                }
                catch
                {
                    failed++;
                }
            }

            if (cancelled)
            {
                MessageBox.Show($"복사 취소됨: {success}개 성공, {skipped}개 건너뜀, {failed}개 실패", "취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"복사 완료: {success}개 성공, {skipped}개 건너뜀, {failed}개 실패", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Refresh destination list if visible
            if (sourceList == lvwLeftDir)
                PopulateListView(lvwRightDir, destFolder);
            else
                PopulateListView(lvwLeftDir, destFolder);
        }

        // Custom draw handlers to ensure full-row coloring is visible
        private void ListView_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString(e.Header.Text, e.Font, Brushes.Black, e.Bounds);
        }

        private void ListView_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            // do nothing — handled in DrawSubItem for details view
        }

        private void ListView_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            var bg = e.SubItem.BackColor.IsEmpty ? e.Item.BackColor : e.SubItem.BackColor;
            var fg = e.SubItem.ForeColor.IsEmpty ? e.Item.ForeColor : e.SubItem.ForeColor;
            using (var b = new SolidBrush(bg))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, fg, flags);
        }

        // Generate a deterministic pastel color from a name string
        private static readonly Color[] _palette = new[]
        {
            Color.LightSalmon,
            Color.LightSkyBlue,
            Color.LightGreen,
            Color.PaleGoldenrod,
            Color.Plum,
            Color.LightPink,
            Color.Khaki,
            Color.PaleTurquoise,
            Color.MistyRose,
            Color.LightSteelBlue
        };

        private static Color ColorFromName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Color.White;

            unchecked
            {
                int hash = 23;
                foreach (char c in name)
                    hash = hash * 31 + c;
                int idx = Math.Abs(hash) % _palette.Length;
                return _palette[idx];
            }
        }

        // Choose black or white foreground depending on background luminance
        private static Color ForegroundForBackground(Color bg)
        {
            // Perceived luminance
            double l = (0.299 * bg.R + 0.587 * bg.G + 0.114 * bg.B) / 255.0;
            return l > 0.7 ? Color.Black : Color.White;
        }

        // Convert HSL to Color
        private static Color HslToColor(int h, double s, double l)
        {
            double C = (1 - Math.Abs(2 * l - 1)) * s;
            double X = C * (1 - Math.Abs((h / 60.0) % 2 - 1));
            double m = l - C / 2;
            double r1 = 0, g1 = 0, b1 = 0;
            if (h < 60) { r1 = C; g1 = X; b1 = 0; }
            else if (h < 120) { r1 = X; g1 = C; b1 = 0; }
            else if (h < 180) { r1 = 0; g1 = C; b1 = X; }
            else if (h < 240) { r1 = 0; g1 = X; b1 = C; }
            else if (h < 300) { r1 = X; g1 = 0; b1 = C; }
            else { r1 = C; g1 = 0; b1 = X; }

            int r = (int)Math.Round((r1 + m) * 255);
            int g = (int)Math.Round((g1 + m) * 255);
            int b = (int)Math.Round((b1 + m) * 255);
            return Color.FromArgb(r, g, b);
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
            catch
            {
                // ignore layout errors
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

                // Directories first
                var dirs = Directory.EnumerateDirectories(folderPath)
                                    .Select(p => new DirectoryInfo(p))
                                    .OrderBy(d => d.Name);
                foreach (var d in dirs)
                {
                    var item = new ListViewItem(d.Name);
                    item.SubItems.Add("<DIR>");
                    item.SubItems.Add(d.LastWriteTime.ToString("g"));
                    // assign a unique background color per folder name
                    var bg = ColorFromName(d.Name);
                    var fg = ForegroundForBackground(bg);
                    item.BackColor = bg;
                    item.ForeColor = fg;
                    // apply to subitems so full row shows the same colors
                    foreach (ListViewItem.ListViewSubItem si in item.SubItems)
                    {
                        si.BackColor = bg;
                        si.ForeColor = fg;
                    }
                    lv.Items.Add(item);
                }

                // Then files (size + modified)
                var files = Directory.EnumerateFiles(folderPath)
                                     .Select(p => new FileInfo(p))
                                     .OrderBy(f => f.Name);
                foreach (var f in files)
                {
                    var item = new ListViewItem(f.Name);
                    item.SubItems.Add(FormatSize(f.Length));
                    item.SubItems.Add(f.LastWriteTime.ToString("g"));
                    // assign a unique background color per file name as well
                    var bgf = ColorFromName(f.Name);
                    var fgf = ForegroundForBackground(bgf);
                    item.BackColor = bgf;
                    item.ForeColor = fgf;
                    foreach (ListViewItem.ListViewSubItem si in item.SubItems)
                    {
                        si.BackColor = bgf;
                        si.ForeColor = fgf;
                    }
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

        private static string FormatSize(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            double kb = bytes / 1024.0;
            if (kb < 1024) return kb.ToString("0.#") + " KB";
            double mb = kb / 1024.0;
            if (mb < 1024) return mb.ToString("0.#") + " MB";
            double gb = mb / 1024.0;
            return gb.ToString("0.#") + " GB";
        }

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "폴더를 선택하세요.";
            if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                dlg.SelectedPath = txtLeftDir.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtLeftDir.Text = dlg.SelectedPath;
                PopulateListView(lvwLeftDir, txtLeftDir.Text);
            }
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "폴더를 선택하세요.";
            if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                dlg.SelectedPath = txtRightDir.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtRightDir.Text = dlg.SelectedPath;
                PopulateListView(lvwRightDir, txtRightDir.Text);
            }
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "폴더를 선택하세요.";
            if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                dlg.SelectedPath = txtLeftDir.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtLeftDir.Text = dlg.SelectedPath;
                PopulateListView(lvwLeftDir, txtLeftDir.Text);
            }
        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "폴더를 선택하세요.";
            if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                dlg.SelectedPath = txtRightDir.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtRightDir.Text = dlg.SelectedPath;
                PopulateListView(lvwRightDir, txtRightDir.Text);
            }
        }

        // Keep empty event handlers required by designer
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void panel8_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void listView2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }

}
