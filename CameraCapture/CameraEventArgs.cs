namespace CameraCapture
{
    /// <summary>
    /// EventArgs for the webcam control
    /// </summary>
    public class CameraEventArgs : System.EventArgs
    {
        private System.Drawing.Image m_Image;
        private ulong m_FrameNumber = 0;

        public CameraEventArgs()
        {
        }

        /// <summary>
        ///  WebCamImage
        ///  This is the image returned by the web camera capture
        /// </summary>
        public System.Drawing.Image CamImage
        {
            get
            { return m_Image; }

            set
            { m_Image = value; }
        }

        /// <summary>
        /// FrameNumber
        /// Holds the sequence number of the frame capture
        /// </summary>
        public ulong FrameNumber
        {
            get
            { return m_FrameNumber; }

            set
            { m_FrameNumber = value; }
        }
    }
}
