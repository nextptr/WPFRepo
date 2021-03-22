using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Interop;

namespace ScreenSnap
{
	unsafe public static class Tool
	{
		public static BitmapSource ScreenSnapshot
		{
			get
			{
				int width = (int)SystemParameters.PrimaryScreenWidth;
				int height = (int)SystemParameters.PrimaryScreenHeight;
				using (var tempBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
				using (var g = Graphics.FromImage(tempBitmap))
				{
					g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));

					var data = tempBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
					try
					{
						return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, data.Scan0, data.Stride * height, data.Stride);
					}
					finally
					{
						tempBitmap.UnlockBits(data);
					}
				}
			}
		}
		public static BitmapSource ScreenSnapshot1(int x,int y,int wid,int hei)
		{
			int width = (int)SystemParameters.PrimaryScreenWidth;
			int height = (int)SystemParameters.PrimaryScreenHeight;
			using (var tempBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
			using (var g = Graphics.FromImage(tempBitmap))
			{
				g.CopyFromScreen( x,  y,  wid,  hei, new System.Drawing.Size(width, height));

				var data = tempBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
				try
				{
					return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, data.Scan0, data.Stride * height, data.Stride);
				}
				finally
				{
					tempBitmap.UnlockBits(data);
				}
			}
		}

		[DllImport("user32")]
		extern static bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsProc lpEnumCallbackFunction, IntPtr lParam);

		[DllImport("user32")]
		extern static bool EnumWindows(EnumWindowsProc lpEnumCallbackFunction, IntPtr lParam);

		[DllImport("user32")]
		extern static bool GetWindowRect(IntPtr hwnd, out WinApiRect rect);

		[DllImport("user32", EntryPoint = "ClientToScreen")]
		extern static bool ClientToScreenWinApi(IntPtr hwnd, ref WinApiPoint WinApiPoint);

		[DllImport("user32")]
		public extern static bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int w, int h, int flags);

		[DllImport("user32")]
		public extern static bool ShowWindow(IntPtr hwnd, int cmd);

		[DllImport("user32")]
		extern static IntPtr GetCursor();

		[DllImport("user32")]
		extern static bool GetIconInfo(IntPtr hIcon, ref ICONINFO iconInfo);

		[DllImport("gdi32")]
		extern static int GetObject(IntPtr hObject, int cb, void* refObject);

		[DllImport("gdi32")]
		extern static bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32")]
		extern static int GetBitmapBits(IntPtr hBitmap, int cb, byte[] bits);

		[DllImport("user32", EntryPoint = "GetWindowLong")]
		public extern static int GetWindowLongWinApi(IntPtr hwnd, int index);

		[DllImport("user32")]
		public extern static long GetWindowLongPtr(IntPtr hwnd, int index);

		[DllImport("user32", EntryPoint = "SetWindowLong")]
		public extern static int SetWindowLongWinApi(IntPtr hwnd, int index, int value);

		[DllImport("user32")]
		public extern static long SetWindowLongPtr(IntPtr hwnd, int index, long value);

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lparam);

		[StructLayout(LayoutKind.Sequential)]
		struct WinApiRect
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct WinApiPoint
		{
			public int X;
			public int Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct ICONINFO
		{
			public int fIcon;
			public int xHotspot;
			public int yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct BITMAP
		{
			public int Type;
			public int Width;
			public int Height;
			public int WidthBytes;
			public short Planes;
			public short BitsPixel;
			public void* Bits;
		}

		public static IReadOnlyList<Rect> GetWindowRects()
		{
			var windows = new List<IntPtr>();
			EnumWindows((hwnd, _) => {
				windows.Add(hwnd);
				return true;
			}, IntPtr.Zero);

			double screenWidth = SystemParameters.PrimaryScreenWidth;
			double screenHeight = SystemParameters.PrimaryScreenHeight;

			return windows.Select(h => {
				var ret = GetWindowRect(h);
				var screenRect = new Rect(0, 0, screenWidth, screenHeight);
				screenRect.Intersect(ret);
				return (Hwnd: h, Rect: screenRect);
			}).Where(p => ValidWindow(p.Hwnd, p.Rect)).Select(p => p.Rect).ToArray();
		}

		private static bool ValidWindow(IntPtr hwnd, Rect rect)
		{
			if (rect.Width <= 0 || rect.Height <= 0) return false;

			const int WS_VISIBLE = 0x10000000;
			const int WS_EX_TRANSPARENT = 0x00000020;
			if ((GetWindowStyle(hwnd) & WS_VISIBLE) == 0) return false;
			if ((GetWindowExStyle(hwnd) & WS_EX_TRANSPARENT) != 0) return false;
			return true;
		}

		public static Rect GetWindowRect(IntPtr hwnd)
		{
			GetWindowRect(hwnd, out var r);
			return new Rect(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
		}

		public static System.Windows.Point ClientToScreen(MouseEventArgs e)
		{
			if (e.Source is Visual visual)
			{
				var presentationSource = PresentationSource.FromVisual(visual);
				if (!presentationSource.IsDisposed && presentationSource.RootVisual is Window window)
				{
					var hwnd = (presentationSource as HwndSource).Handle;
					var point = e.GetPosition(window);
					var pointArg = new WinApiPoint()
					{
						X = (int)point.X,
						Y = (int)point.Y
					};
					if (ClientToScreenWinApi(hwnd, ref pointArg))
					{
						return new System.Windows.Point(pointArg.X, pointArg.Y);
					}
				}
			}
			return new System.Windows.Point();
		}

		const int GWL_EXSTYLE = -20;
		const int GWL_STYLE = -16;

		public static long GetWindowLong(IntPtr hwnd, int index)
		{
			if (IntPtr.Size == 4)
			{
				return GetWindowLongWinApi(hwnd, index);
			}
			else
			{
				return GetWindowLongPtr(hwnd, index);
			}
		}

		public static void SetWindowLong(IntPtr hwnd, int index, long exStyle)
		{
			if (IntPtr.Size == 4)
			{
				SetWindowLongWinApi(hwnd, index, (int)exStyle);
			}
			else
			{
				SetWindowLongPtr(hwnd, index, exStyle);
			}
		}

		public static long GetWindowExStyle(IntPtr hwnd)
			=> GetWindowLong(hwnd, GWL_EXSTYLE);

		public static void SetWindowExStyle(IntPtr hwnd, long exStyle)
			=> SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);

		public static long GetWindowStyle(IntPtr hwnd)
			=> GetWindowLong(hwnd, GWL_STYLE);

		public static void SetWindowStyle(IntPtr hwnd, long exStyle)
			=> SetWindowLong(hwnd, GWL_STYLE, exStyle);
	}
}
