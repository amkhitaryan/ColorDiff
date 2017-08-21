using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CameraCapture
{
    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(CamCapture), "CAMERA.ICO")] // toolbox bitmap
    [Designer("Sytem.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(System.ComponentModel.Design.IDesigner))] // make composite
    public class CamCapture : UserControl
    {
        private IContainer components;
        private Timer _timer1;

        // property variables
        private int _mTimeToCaptureMilliseconds = 100;
        private int _mWidth = 320;
        private int _mHeight = 240;
        private int _mCapHwnd;
        private ulong _mFrameNumber = 0;

        // global variables to make the video capture go faster
        private CameraCapture.CameraEventArgs x = new CameraCapture.CameraEventArgs();
        private IDataObject _tempObj;
        private System.Drawing.Image _tempImg;
        private bool _bStopped = true;

        // event delegate
        public delegate void CamEventHandler(object source, CameraCapture.CameraEventArgs e);
        // fired when a new image is captured
        public event CamEventHandler ImageCaptured;

        #region API Declarations

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        public static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

        [DllImport("user32", EntryPoint = "OpenClipboard")]
        public static extern int OpenClipboard(int hWnd);

        [DllImport("user32", EntryPoint = "EmptyClipboard")]
        public static extern int EmptyClipboard();

        [DllImport("user32", EntryPoint = "CloseClipboard")]
        public static extern int CloseClipboard();

        #endregion

        #region API Constants

        public const int WM_USER = 1024;

        public const int WM_CAP_CONNECT = 1034;
        public const int WM_CAP_DISCONNECT = 1035;
        public const int WM_CAP_GET_FRAME = 1084;
        public const int WM_CAP_COPY = 1054;

        public const int WM_CAP_START = WM_USER;

        public const int WM_CAP_DLG_VIDEOFORMAT = WM_CAP_START + 41;
        public const int WM_CAP_DLG_VIDEOSOURCE = WM_CAP_START + 42;
        public const int WM_CAP_DLG_VIDEODISPLAY = WM_CAP_START + 43;
        public const int WM_CAP_GET_VIDEOFORMAT = WM_CAP_START + 44;
        public const int WM_CAP_SET_VIDEOFORMAT = WM_CAP_START + 45;
        public const int WM_CAP_DLG_VIDEOCOMPRESSION = WM_CAP_START + 46;
        public const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;

        #endregion

        #region NOTES

        /*
		 * If you want to allow the user to change the display size and 
		 * color format of the video capture, call:
		 * SendMessage (mCapHwnd, WM_CAP_DLG_VIDEOFORMAT, 0, 0);
		 * You will need to requery the capture device to get the new settings
		*/

        #endregion


        public CamCapture()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Override the class's finalize method, so we can stop
        /// the video capture on exit
        /// </summary>
        ~CamCapture()
        {
            this.Stop();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this._timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CamCapture
            // 
            this.Name = "CamCapture";
            this.Size = new System.Drawing.Size(342, 252);
            this.ResumeLayout(false);

        }
        #endregion

        #region Control Properties

        /// <summary>
        /// The time intervale between frame captures
        /// </summary>
        public int TimeToCapture_milliseconds
        {
            get
            { return _mTimeToCaptureMilliseconds; }

            set
            { _mTimeToCaptureMilliseconds = value; }
        }

        /// <summary>
        /// The height of the video capture image
        /// </summary>
        public int CaptureHeight
        {
            get
            { return _mHeight; }

            set
            { _mHeight = value; }
        }

        /// <summary>
        /// The width of the video capture image
        /// </summary>
        public int CaptureWidth
        {
            get
            { return _mWidth; }

            set
            { _mWidth = value; }
        }

        /// <summary>
        /// The sequence number to start at for the frame number. OPTIONAL
        /// </summary>
        public ulong FrameNumber
        {
            get
            { return _mFrameNumber; }

            set
            { _mFrameNumber = value; }
        }

        #endregion

        #region Start and Stop Capture Functions

        /// <summary>
        /// Starts the video capture
        /// </summary>
        /// <param name="FrameNum">the frame number to start at. 
        /// Set to 0 to let the control allocate the frame number</param>
        public void Start(ulong FrameNum)
        {
            try
            {
                // for safety, call stop, just in case we are already running
                this.Stop();

                // setup a capture window
                _mCapHwnd = capCreateCaptureWindowA("CamCap", 0, 0, 0, _mWidth, _mHeight, this.Handle.ToInt32(), 0);

                // connect to the capture device
                Application.DoEvents();
                SendMessage(_mCapHwnd, WM_CAP_CONNECT, 0, 0);
                SendMessage(_mCapHwnd, WM_CAP_SET_PREVIEW, 0, 0);

                // set the frame number
                _mFrameNumber = FrameNum;

                // set the timer information
                this._timer1.Interval = _mTimeToCaptureMilliseconds;
                _bStopped = false;
                this._timer1.Start();
            }

            catch (Exception excep)
            {
                MessageBox.Show("An error ocurred while starting the video capture. Check that your webcamera is connected properly and turned on.\r\n\n" + excep.Message);
                this.Stop();
            }
        }

        /// <summary>
        /// Stops the video capture
        /// </summary>
        public void Stop()
        {
            try
            {
                // stop the timer
                _bStopped = true;
                this._timer1.Stop();

                // disconnect from the video source
                Application.DoEvents();
                SendMessage(_mCapHwnd, WM_CAP_DISCONNECT, 0, 0);
            }

            catch (Exception excep)
            { // don't raise an error here.
            }

        }

        #endregion

        #region Video Capture Code

        /// <summary>
        /// Capture the next frame from the video feed
        /// </summary>
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            try
            {
                // pause the timer
                this._timer1.Stop();

                // get the next frame;
                SendMessage(_mCapHwnd, WM_CAP_GET_FRAME, 0, 0);

                // copy the frame to the clipboard
                SendMessage(_mCapHwnd, WM_CAP_COPY, 0, 0);

                // paste the frame into the event args image
                if (ImageCaptured != null)
                {
                    // get from the clipboard
                    _tempObj = Clipboard.GetDataObject();
                    if (_tempObj != null)
                        _tempImg = (System.Drawing.Bitmap)_tempObj.GetData(System.Windows.Forms.DataFormats.Bitmap);

                    /*
					* For some reason, the API is not resizing the video
					* feed to the width and height provided when the video
					* feed was started, so we must resize the image here
					*/
                    x.CamImage = _tempImg.GetThumbnailImage(_mWidth, _mHeight, null, System.IntPtr.Zero);

                    // raise the event
                    this.ImageCaptured(this, x);
                }

                // restart the timer
                Application.DoEvents();
                if (!_bStopped)
                    this._timer1.Start();
            }

            catch (Exception excep)
            {
                MessageBox.Show("An error ocurred while capturing the video image. The video capture will now be terminated.\r\n\n" + excep.Message);
                this.Stop(); // stop the process
            }
        }

        #endregion
    }
}
