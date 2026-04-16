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

            // If initial paths are present, populate (compare if both set)
            var leftExists = !string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text);
            var rightExists = !string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text);
            if (leftExists && rightExists)
                PopulateBothWithComparison(txtLeftDir.Text, txtRightDir.Text);
            else
            {
                if (leftExists) PopulateListView(lvwLeftDir, txtLeftDir.Text);
                if (rightExists) PopulateListView(lvwRightDir, txtRightDir.Text);
            }

            // Add context menus for copying selected items to the opposite folder
            CreateContextMenus();
        }

        // Populate both listviews and indicate differences for items including subfolders
        private void PopulateBothWithComparison(string leftPath, string rightPath)
        {
            // Build item maps: name -> (isDir, size, lastWrite)
            var leftItems = Directory.Exists(leftPath) ?
                Directory.EnumerateFileSystemEntries(leftPath).Select(p => new FileSystemInfoWrapper(p)).ToDictionary(w => w.Name, StringComparer.OrdinalIgnoreCase)
                : new System.Collections.Generic.Dictionary<string, FileSystemInfoWrapper>(StringComparer.OrdinalIgnoreCase);
            var rightItems = Directory.Exists(rightPath) ?
                Directory.EnumerateFileSystemEntries(rightPath).Select(p => new FileSystemInfoWrapper(p)).ToDictionary(w => w.Name, StringComparer.OrdinalIgnoreCase)
                : new System.Collections.Generic.Dictionary<string, FileSystemInfoWrapper>(StringComparer.OrdinalIgnoreCase);

            // prepare lists
            lvwLeftDir.BeginUpdate(); lvwLeftDir.Items.Clear();
            lvwRightDir.BeginUpdate(); lvwRightDir.Items.Clear();

            // union of names
            var allNames = leftItems.Keys.Union(rightItems.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(n => n, StringComparer.OrdinalIgnoreCase);
            foreach (var name in allNames)
            {
                leftItems.TryGetValue(name, out var l);
                rightItems.TryGetValue(name, out var r);

                // determine state: only left, only right, both same, both different
                bool onlyLeft = r == null && l != null;
                bool onlyRight = l == null && r != null;
                bool both = l != null && r != null;

                // choose colors
                Color leftBg = Color.White, rightBg = Color.White;
                if (onlyLeft)
                {
                    leftBg = ColorFromName(name);
                    rightBg = Color.White;
                }
                else if (onlyRight)
                {
                    leftBg = Color.White;
                    rightBg = ColorFromName(name);
                }
                else if (both)
                {
                    // if both are directories, show same color; if files, compare size or timestamp
                    if (l.IsDirectory && r.IsDirectory)
                    {
                        leftBg = rightBg = ColorFromName(name);
                    }
                    else if (!l.IsDirectory && !r.IsDirectory)
                    {
                        if (l.Length == r.Length && l.LastWriteTime == r.LastWriteTime)
                        {
                            leftBg = rightBg = ColorFromName(name);
                        }
                        else
                        {
                            leftBg = Color.Yellow;
                            rightBg = Color.Yellow;
                        }
                    }
                    else
                    {
                        // one is file, one is dir -> mark difference
                        leftBg = Color.Orange;
                        rightBg = Color.Orange;
                    }
                }

                // Left item
                if (l != null)
                {
                    var li = new ListViewItem(l.Name);
                    li.SubItems.Add(l.IsDirectory ? "<DIR>" : FormatSize(l.Length));
                    li.SubItems.Add(l.LastWriteTime.ToString("g"));
                    var fg = ForegroundForBackground(leftBg);
                    li.BackColor = leftBg; li.ForeColor = fg;
                    foreach (ListViewItem.ListViewSubItem si in li.SubItems) { si.BackColor = leftBg; si.ForeColor = fg; }
                    lvwLeftDir.Items.Add(li);
                }
                else
                {
                    var li = new ListViewItem(name);
                    li.SubItems.Add(""); li.SubItems.Add("");
                    lvwLeftDir.Items.Add(li);
                }

                // Right item
                if (r != null)
                {
                    var ri = new ListViewItem(r.Name);
                    ri.SubItems.Add(r.IsDirectory ? "<DIR>" : FormatSize(r.Length));
                    ri.SubItems.Add(r.LastWriteTime.ToString("g"));
                    var fg = ForegroundForBackground(rightBg);
                    ri.BackColor = rightBg; ri.ForeColor = fg;
                    foreach (ListViewItem.ListViewSubItem si in ri.SubItems) { si.BackColor = rightBg; si.ForeColor = fg; }
                    lvwRightDir.Items.Add(ri);
                }
                else
                {
                    var ri = new ListViewItem(name);
                    ri.SubItems.Add(""); ri.SubItems.Add("");
                    lvwRightDir.Items.Add(ri);
                }
            }

            lvwLeftDir.EndUpdate(); lvwRightDir.EndUpdate();
        }

        private class FileSystemInfoWrapper
        {
            public string Name { get; }
            public bool IsDirectory { get; }
            public long Length { get; }
            public DateTime LastWriteTime { get; }

            public FileSystemInfoWrapper(string path)
            {
                if (Directory.Exists(path))
                {
                    var di = new DirectoryInfo(path);
                    Name = di.Name;
                    IsDirectory = true;
                    Length = 0;
                    LastWriteTime = di.LastWriteTime;
                }
                else
                {
                    var fi = new FileInfo(path);
                    Name = fi.Name;
                    IsDirectory = false;
                    Length = fi.Exists ? fi.Length : 0;
                    LastWriteTime = fi.Exists ? fi.LastWriteTime : DateTime.MinValue;
                }
            }
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

            // selected list contains files and/or directories (relative or immediate names)

            int success = 0, failed = 0, skipped = 0;
            bool cancelled = false;

            // Process files and directories; treat directories as single items (copy recursively)
            foreach (var it in selected)
            {
                if (cancelled) break;
                var name = it.Text;
                var isDir = it.SubItems.Count > 1 && it.SubItems[1].Text == "<DIR>";
                var srcPath = Path.Combine(sourceFolder, name);
                var dstPath = Path.Combine(destFolder, name);

                if (isDir)
                {
                    // Prevent copying folder into its own subfolder
                    try
                    {
                        var srcFull = Path.GetFullPath(srcPath).TrimEnd(Path.DirectorySeparatorChar);
                        var dstFull = Path.GetFullPath(dstPath).TrimEnd(Path.DirectorySeparatorChar);
                        if (dstFull.StartsWith(srcFull + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || dstFull.Equals(srcFull, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("대상 폴더가 원본 폴더의 하위이거나 동일합니다. 복사할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            skipped++;
                            continue;
                        }
                    }
                    catch
                    {
                        // ignore path issues
                    }

                    // If destination directory exists, ask once whether to overwrite/skip/cancel for this folder
                    if (Directory.Exists(dstPath))
                    {
                        var msg = "대상에 동일한 이름의 폴더가 이미 있습니다. 폴더를 덮어쓰시겠습니까?";
                        var res = MessageBox.Show(msg, "폴더 충돌", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
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

                        // Yes => show confirmation dialog with folder summary before copying
                        using var conf = new ConfirmCopyForm(srcPath, dstPath, isDirectory: true);
                        var cr = conf.ShowDialog(this);
                        if (cr != DialogResult.OK)
                        {
                            cancelled = true;
                            break;
                        }

                        try
                        {
                            DirectoryCopyRecursive(srcPath, dstPath, ref success, ref failed, ref skipped, ref cancelled, overwrite: true);
                        }
                        catch
                        {
                            failed++;
                        }
                    }
                    else
                    {
                        using var conf = new ConfirmCopyForm(srcPath, dstPath, isDirectory: true);
                        var cr = conf.ShowDialog(this);
                        if (cr != DialogResult.OK)
                        {
                            cancelled = true;
                            break;
                        }

                        try
                        {
                            DirectoryCopyRecursive(srcPath, dstPath, ref success, ref failed, ref skipped, ref cancelled, overwrite: true);
                        }
                        catch
                        {
                            failed++;
                        }
                    }
                }
                else
                {
                    // file
                    var src = srcPath;
                    var dst = dstPath;
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

                            // Yes -> show confirmation dialog with details before actual copy
                            using var conf = new ConfirmCopyForm(src, dst, isDirectory: false);
                            var cr = conf.ShowDialog(this);
                            if (cr != DialogResult.OK)
                            {
                                // user cancelled at confirmation
                                cancelled = true;
                                break;
                            }
                        }

                        File.Copy(src, dst, true);
                        success++;
                    }
                    catch
                    {
                        failed++;
                    }
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

            // Refresh both lists
            if (Directory.Exists(txtLeftDir.Text)) PopulateListView(lvwLeftDir, txtLeftDir.Text);
            if (Directory.Exists(txtRightDir.Text)) PopulateListView(lvwRightDir, txtRightDir.Text);
        }

        // Recursive directory copy helper
        private void DirectoryCopyRecursive(string sourceDirName, string destDirName, ref int success, ref int failed, ref int skipped, ref bool cancelled, bool overwrite)
        {
            if (cancelled) return;
            if (!Directory.Exists(sourceDirName)) return;

            Directory.CreateDirectory(destDirName);

            // copy files
            foreach (var filePath in Directory.GetFiles(sourceDirName))
            {
                if (cancelled) return;
                var fileName = Path.GetFileName(filePath);
                var destFile = Path.Combine(destDirName, fileName);
                try
                {
                    if (File.Exists(destFile) && !overwrite)
                    {
                        skipped++;
                        continue;
                    }
                    File.Copy(filePath, destFile, overwrite);
                    success++;
                }
                catch
                {
                    failed++;
                }
            }

            // recurse subdirectories
            foreach (var dirPath in Directory.GetDirectories(sourceDirName))
            {
                if (cancelled) return;
                var dirName = Path.GetFileName(dirPath);
                var destSub = Path.Combine(destDirName, dirName);
                // if destSub exists and overwrite==false, ask handled by caller; here we pass overwrite true to replace files
                DirectoryCopyRecursive(dirPath, destSub, ref success, ref failed, ref skipped, ref cancelled, overwrite);
            }
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
            // indicate directories with a darker border
            bool isDir = e.Item.SubItems.Count > 1 && e.Item.SubItems[1].Text == "<DIR>";
            using (var b = new SolidBrush(bg))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }
            if (isDir)
            {
                using (var p = new Pen(Color.Gray, 2))
                {
                    var r = e.Bounds;
                    r.Inflate(-2, -2);
                    e.Graphics.DrawRectangle(p, r);
                }
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
