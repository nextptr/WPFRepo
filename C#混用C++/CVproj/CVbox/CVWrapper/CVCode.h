#pragma once
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>
#define WINDOW_WIDTH 600
using namespace std;
using namespace cv;


//#ifdef __DLL_EXPORTS__
//#define CV_CPP_API __declspec( dllexport )
//#else
//#define CV_CPP_API __declspec(dllimport)
//#endif
//class CV_CPP_API CVCode
//{
//public:
//	void CVTest1();
//	void CVTest2();
//	void CVTest3();
//	void CVTest4();
//	void CVTest5();
//	void CVTest6();
//	void CVTest7();
//	void CVTest8();
//
//	void DrawEllipse(Mat img, double angle);
//	void DrawFilledCircle(Mat img, Point center);
//	void DrawPolygon(Mat img);
//	void DrawLine(Mat img, Point start, Point end);
//	void colorReduce(Mat& inputImage, Mat& outputImage, int div);
//};
//
//extern "C" CV_CPP_API CVCode * _stdcall CreateCVHandle();


/*
*  public static class PixelFormats
    {
        public static PixelFormat Default => new PixelFormat(PixelFormatEnum.Default);

        public static PixelFormat Indexed1 => new PixelFormat(PixelFormatEnum.Indexed1);

        public static PixelFormat Indexed2 => new PixelFormat(PixelFormatEnum.Indexed2);

        public static PixelFormat Indexed4 => new PixelFormat(PixelFormatEnum.Indexed4);

        public static PixelFormat Indexed8 => new PixelFormat(PixelFormatEnum.Indexed8);

        public static PixelFormat BlackWhite => new PixelFormat(PixelFormatEnum.BlackWhite);

        public static PixelFormat Gray2 => new PixelFormat(PixelFormatEnum.Gray2);

        public static PixelFormat Gray4 => new PixelFormat(PixelFormatEnum.Gray4);

        public static PixelFormat Gray8 => new PixelFormat(PixelFormatEnum.Gray8);

        public static PixelFormat Bgr555 => new PixelFormat(PixelFormatEnum.Bgr555);

        public static PixelFormat Bgr565 => new PixelFormat(PixelFormatEnum.Bgr565);

        public static PixelFormat Rgb128Float => new PixelFormat(PixelFormatEnum.Rgb128Float);

        public static PixelFormat Bgr24 => new PixelFormat(PixelFormatEnum.Bgr24);

        public static PixelFormat Rgb24 => new PixelFormat(PixelFormatEnum.Rgb24);

        public static PixelFormat Bgr101010 => new PixelFormat(PixelFormatEnum.Bgr101010);

        public static PixelFormat Bgr32 => new PixelFormat(PixelFormatEnum.Bgr32);

        public static PixelFormat Bgra32 => new PixelFormat(PixelFormatEnum.Bgra32);

        public static PixelFormat Pbgra32 => new PixelFormat(PixelFormatEnum.Pbgra32);

        public static PixelFormat Rgb48 => new PixelFormat(PixelFormatEnum.Rgb48);

        public static PixelFormat Rgba64 => new PixelFormat(PixelFormatEnum.Rgba64);

        public static PixelFormat Prgba64 => new PixelFormat(PixelFormatEnum.Prgba64);

        public static PixelFormat Gray16 => new PixelFormat(PixelFormatEnum.Gray16);

        public static PixelFormat Gray32Float => new PixelFormat(PixelFormatEnum.Gray32Float);

        public static PixelFormat Rgba128Float => new PixelFormat(PixelFormatEnum.Rgba128Float);

        public static PixelFormat Prgba128Float => new PixelFormat(PixelFormatEnum.Prgba128Float);

        public static PixelFormat Cmyk32 => new PixelFormat(PixelFormatEnum.Cmyk32);
    }
*/


public enum class ImgType_CPP//  特征线 排序方式
{
	Gray2 = 0,
	Gray8,
	Bgr32,
	Bgra32
};

struct Mat_CV
{
	Mat_CV()
	{
		img_w = img_h = 0;
		data = NULL;
	}
	Mat_CV(int w, int h, ImgType_CPP tp)
	{
		img_w = w;
		img_h = h;
		img_type = tp;
	}

	~Mat_CV()
	{
	}
	int img_w;
	int img_h;
	ImgType_CPP img_type;
	uchar* data;// 引用
};

static class CVHelper
{
public:
	static int ConvertType(ImgType_CPP tp)
	{
		int colorType = 1;
		switch (tp)
		{
		case ImgType_CPP::Gray2:
			colorType = (int)CV_8UC1;
			break;
		case ImgType_CPP::Gray8:
			colorType = (int)CV_8UC1;
			break;
		case ImgType_CPP::Bgr32:
			colorType = (int)CV_8UC4;
			break;
		case ImgType_CPP::Bgra32:
			colorType = (int)CV_8UC4;
			break;
		default:
			break;
		}
		return colorType;
	}
};

class CVCode
{
public:
	void CVTest1(string pth);
	void CVTest2();
	void CVTest3();
	void CVTest4();
	void CVTest5();
	void CVTest6();
	void CVTest7();
	void CVTest8();

	void DrawEllipse(Mat img, double angle);
	void DrawFilledCircle(Mat img, Point center);
	void DrawPolygon(Mat img);
	void DrawLine(Mat img, Point start, Point end);
	void colorReduce(Mat& inputImage, Mat& outputImage, int div);
	void ToolColorConvert(const Mat_CV& src, Mat_CV& dst);
	void DrawMark(string pth,string txt);
};

extern "C" CVCode * _stdcall CreateCVHandle();