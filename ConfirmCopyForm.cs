using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FileCompare
{
    public class ConfirmCopyForm : Form
    {
        private TextBox txtInfo;
        private Button btnOk;
        private Button btnCancel;

        public ConfirmCopyForm(string sourcePath, string destPath, bool isDirectory)
        {
            Text = "복사 확인";
            Width = 600;
            Height = 400;
            StartPosition = FormStartPosition.CenterParent;

            txtInfo = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9),
                WordWrap = false
            };

            btnOk = new Button { Text = "확인", DialogResult = DialogResult.OK, Anchor = AnchorStyles.Right };
            btnCancel = new Button { Text = "취소", DialogResult = DialogResult.Cancel, Anchor = AnchorStyles.Right };

            var panel = new Panel { Dock = DockStyle.Bottom, Height = 42 };
            btnOk.Location = new Point(panel.Width - 180, 6);
            btnOk.Size = new Size(80, 28);
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(panel.Width - 90, 6);
            btnCancel.Size = new Size(80, 28);
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            panel.Controls.Add(btnOk);
            panel.Controls.Add(btnCancel);

            Controls.Add(txtInfo);
            Controls.Add(panel);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            // populate info
            var sb = new StringBuilder();
            sb.AppendLine($"원본: {sourcePath}");
            sb.AppendLine($"대상: {destPath}");
            sb.AppendLine();

            if (isDirectory)
            {
                try
                {
                    var allFiles = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories);
                    long totalSize = 0;
                    int fileCount = 0;
                    int dirCount = 0;
                    foreach (var f in allFiles)
                    {
                        try
                        {
                            var fi = new FileInfo(f);
                            totalSize += fi.Length;
                            fileCount++;
                        }
                        catch { }
                    }
                    dirCount = Directory.EnumerateDirectories(sourcePath, "*", SearchOption.AllDirectories).Count();
                    sb.AppendLine($"하위 파일 수: {fileCount}");
                    sb.AppendLine($"하위 폴더 수: {dirCount}");
                    sb.AppendLine($"총 크기: {FormatSize(totalSize)}");
                    sb.AppendLine();

                    sb.AppendLine("하위 파일 일부 목록:");
                    int shown = 0;
                    foreach (var f in Directory.EnumerateFiles(sourcePath, "*", SearchOption.TopDirectoryOnly).OrderBy(Path.GetFileName))
                    {
                        sb.AppendLine("  " + Path.GetFileName(f));
                        if (++shown >= 100) break;
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine("폴더 정보를 읽는 중 오류가 발생했습니다: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    var fi = new FileInfo(sourcePath);
                    sb.AppendLine($"파일 크기: {FormatSize(fi.Length)}");
                    sb.AppendLine($"수정일: {fi.LastWriteTime}");
                }
                catch (Exception ex)
                {
                    sb.AppendLine("파일 정보를 읽는 중 오류가 발생했습니다: " + ex.Message);
                }
            }

            txtInfo.Text = sb.ToString();
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
    }
}
