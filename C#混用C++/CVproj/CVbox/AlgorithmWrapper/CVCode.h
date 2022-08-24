#pragma once
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>
#define WINDOW_WIDTH 600
using namespace std;
using namespace cv;

class CVCode
{
public:
	void CVTest1();
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
};

extern "C" CVCode * _stdcall CreateCVHandle();