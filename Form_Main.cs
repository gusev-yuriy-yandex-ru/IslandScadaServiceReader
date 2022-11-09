using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinCAT.Ads;
using System.IO;

namespace IslandScadaServiceReader
{
    public partial class Form1 : Form
    {
        MemoryMappedFile Request;
        MemoryMappedFile RequestBody;
        MemoryMappedFile Result;
        MemoryMappedFile ResultIsOk;
        byte Command = 0; // 0 - Ничего не делать
                          // 1 - Читать область
                          // 2 - Писать область
        string AmsNetID = "";
        UInt16 PortNum = 0;
        UInt16 Goup = 0;
        UInt16 Offset = 0;
        UInt16 Size = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void NIcon_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            NIcon.Visible = false;
        }
        private void tmrRepaint_Tick(object sender, EventArgs e)
        {
            // Деактивация таймера
            tmrRepaint.Enabled = false;
            // Обработка состояния формы
            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                NIcon.Visible = false;
                // Изменение размера
                this.Width = 494;
                this.Height = 273;
                // Перемещение в правый нижний угол экрана
                this.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width - this.Width;
                this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - this.Height - 40;
            }
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                NIcon.Visible = true;
            }
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                NIcon.Visible = false;
            }
            // Активация таймера
            tmrRepaint.Enabled = true;
        }
        private void tmrStart_Tick(object sender, EventArgs e)
        {
            // Деактивация таймера
            tmrStart.Enabled = false;
            // Открытие файла лога
            FileStream fs = File.Open("..\\data\\IslandScadaServiceReaderLog.txt", FileMode.Append, FileAccess.Write, FileShare.None);
            // Лог - Запуск приложения
            Byte[] info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " : Запуск приложения.\n");
            fs.Write(info, 0, info.Length);
            // Получение данных запроса
            bool IsGood = false;
            rtbResult.Text = "Получение данных запроса.\n";
            try
            {
                Request = MemoryMappedFile.CreateOrOpen("IslandScadaServiceReader_Request", 1024);
                using (MemoryMappedViewAccessor reader = Request.CreateViewAccessor(0, 1024, MemoryMappedFileAccess.Read))
                {
                    // Чтение комманды
                    reader.Read<byte>(0, out Command);
                    if (Command == 0)
                    {
                        rtbResult.Text += "     Нет команды.\n";
                        info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Нет команды.\n");
                        fs.Write(info, 0, info.Length);
                    }
                    if (Command == 1) 
                    {
                        rtbResult.Text += "     Команда прочитать область.\n";
                        info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Команда прочитать область.\n");
                        fs.Write(info, 0, info.Length);
                    }
                    if (Command == 2)
                    {
                        rtbResult.Text += "     Команда записать область.\n";
                        info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Команда записать область.\n");
                        fs.Write(info, 0, info.Length);
                    }
                    // Чтение AMS Net ID
                    byte B = 0;
                    AmsNetID = "";
                    reader.Read<byte>(1, out B);
                    if (B > 0)
                        for (int i = 0; i < B; i++)
                        {
                            char C;
                            reader.Read<char>((i + 1) * 2, out C);
                            AmsNetID += Convert.ToChar(C);
                        }
                    rtbResult.Text += "     AMS Net ID = " + AmsNetID + "\n";
                    info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      AMS Net ID = " + AmsNetID + "\n");
                    fs.Write(info, 0, info.Length);
                    // Чтение номера порта
                    reader.Read<UInt16>((AmsNetID.Length + 1) * 2, out PortNum);
                    rtbResult.Text += "     Port = " + Convert.ToString(PortNum) + "\n";
                    info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Port = " + Convert.ToString(PortNum) + "\n");
                    fs.Write(info, 0, info.Length);
                    // Чтение номера группы
                    reader.Read<UInt16>((AmsNetID.Length + 2) * 2, out Goup);
                    rtbResult.Text += "     Ggoup = " + Convert.ToString(Goup) + "\n";
                    info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Ggoup = " + Convert.ToString(Goup) + "\n");
                    fs.Write(info, 0, info.Length);
                    // Чтение смещения
                    reader.Read<UInt16>((AmsNetID.Length + 3) * 2, out Offset);
                    rtbResult.Text += "     Offset = " + Convert.ToString(Offset) + "\n";
                    info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Offset = " + Convert.ToString(Offset) + "\n");
                    fs.Write(info, 0, info.Length);
                    // Чтение длины
                    reader.Read<UInt16>((AmsNetID.Length + 4) * 2, out Size);
                    rtbResult.Text += "     Size = " + Convert.ToString(Size) + "\n";
                    info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Size = " + Convert.ToString(Size) + "\n");
                    fs.Write(info, 0, info.Length);
                    IsGood = true;
                }
            }
            catch { }
            // Закрытие придожения, если команда неизвестна
            if (Command == 0) Close();
            if (Command > 2) Close();
            if (!IsGood) Close();
            // Обработка команды чтения
            if (Command == 1)
            {
                TcAdsClient tcClient;
                tcClient = new TcAdsClient();
                var ds = new AdsStream(Size + 10);
                var br = new AdsBinaryReader(ds);
                // Выполнение запроса
                IsGood = false;
                try
                {
                    // Выпролнениезапроса устройству
                    tcClient.Connect(AmsNetID, PortNum);
                    tcClient.Timeout = 100;
                    ds = new AdsStream(Size);
                    br = new AdsBinaryReader(ds);
                    ds.Position = 0;
                    tcClient.Read(Convert.ToInt32(Goup), Convert.ToInt32(Offset), ds);
                    // Открытие области с результатами
                    Result = MemoryMappedFile.OpenExisting("IslandScadaServiceReader_Result");
                    MemoryMappedViewAccessor Result_writer = Result.CreateViewAccessor(0, Size, MemoryMappedFileAccess.ReadWrite);
                    using (MemoryMappedViewAccessor ResultWriter = Result.CreateViewAccessor(0, Size))
                    {
                        rtbResult.Text += "     Result = 0x";
                        info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Result = 0x");
                        fs.Write(info, 0, info.Length);
                        for (int i = 0; i < Size; i++)
                        {
                            ds.Position = i;
                            int B = ds.ReadByte();
                            ResultWriter.Write(i, Convert.ToByte(B));
                            string sb = B.ToString("X");
                            if (sb.Length > 2) sb = sb.Substring(0, 2);
                            if (sb.Length < 2) sb = "0" + sb;
                            rtbResult.Text += sb;
                            info = new UTF8Encoding(true).GetBytes(sb);
                            fs.Write(info, 0, info.Length);
                        }
                        rtbResult.Text += "H\n";
                        info = new UTF8Encoding(true).GetBytes("H\n");
                        fs.Write(info, 0, info.Length);
                    }
                    // Открытие области с отчетом о правильности результтов
                    ResultIsOk = MemoryMappedFile.OpenExisting("IslandScadaServiceReader_ResultIsOk");
                    using (MemoryMappedViewAccessor ResultIsOk_writer = ResultIsOk.CreateViewAccessor(0, 1, MemoryMappedFileAccess.ReadWrite))
                    {
                        int B = 255;
                        ResultIsOk_writer.Write(0, Convert.ToByte(B));
                    }
                }
                catch { }
                //Close();
            }
            // Обработка команды записи
            if (Command == 2)
            {
                try
                {
                    TcAdsClient tcClientWrite;
                    tcClientWrite = new TcAdsClient();
                    var dsw = new AdsStream(Size + 10);
                    var brw = new AdsBinaryWriter(dsw);
                    RequestBody = MemoryMappedFile.OpenExisting("IslandScadaServiceReader_RequestBody");
                    MemoryMappedViewAccessor RequestBody_reader = RequestBody.CreateViewAccessor(0, Size, MemoryMappedFileAccess.Read);
                    byte[] Buffer = new byte[Size + 4];
                    rtbResult.Text += "     Body = 0x";
                    info = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString() + " :      Body = 0x");
                    fs.Write(info, 0, info.Length);
                    if (Size > 0)
                        for (UInt16 i = 0; i < Size; i++)
                        {
                            byte B = Convert.ToByte(RequestBody_reader.ReadByte(i));
                            dsw.Position = i;
                            string sb = B.ToString("X");
                            if (sb.Length > 2) sb = sb.Substring(0, 2);
                            if (sb.Length < 2) sb = "0" + sb;
                            rtbResult.Text += sb;
                            info = new UTF8Encoding(true).GetBytes(sb);
                            fs.Write(info, 0, info.Length);
                        }
                    rtbResult.Text += "H\n";
                    info = new UTF8Encoding(true).GetBytes("H\n");
                    fs.Write(info, 0, info.Length);
                    // Выпролнениезапроса устройству
                    tcClientWrite.Connect(AmsNetID, PortNum);
                    tcClientWrite.Timeout = 100;
                    tcClientWrite.Write(Convert.ToInt32(Goup), Convert.ToInt32(Offset), dsw, 0, Size);
                    dsw.Dispose();
                    brw.Dispose();
                    tcClientWrite.Dispose();
                    // Открытие области с отчетом о правильности результтов
                    ResultIsOk = MemoryMappedFile.OpenExisting("IslandScadaServiceReader_ResultIsOk");
                    using (MemoryMappedViewAccessor ResultIsOk_writer = ResultIsOk.CreateViewAccessor(0, 1, MemoryMappedFileAccess.ReadWrite))
                    {
                        int B = 255;
                        ResultIsOk_writer.Write(0, Convert.ToByte(B));
                    }
                    //Close();
                }
                catch { }
            }
            Close();
            // Активация таймера
            tmrStart.Enabled = true;
        }

    }
}
