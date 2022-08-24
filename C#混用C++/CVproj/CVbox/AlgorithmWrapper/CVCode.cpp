#pragma once
#include "pch.h"
#include "CVCode.h"

void CVCode::CVTest1() //腐蚀
{
	string filename = "lena.jpg";
	cv::Mat src = cv::imread(filename, 1);
	cv::imshow("原图", src);

	Mat element = getStructuringElement(MORPH_ERODE, Size(15, 15));
	Mat dstImage;
	erode(src, dstImage, element);
	cv::imshow("效果图", dstImage);
	cv::waitKey(0);
}
void CVCode::CVTest2() //模糊
{
	string filename = "lena.jpg";
	cv::Mat src = cv::imread(filename, 1);
	cv::imshow("原图", src);

	Mat dstImage;
	blur(src, dstImage, Size(7, 7));
	cv::imshow("效果图", dstImage);
	cv::waitKey(0);
}
void CVCode::CVTest3() //边缘识别
{
	string filename = "lena.jpg";
	cv::Mat src = cv::imread(filename, 1);
	cv::imshow("原图", src);

	Mat dstImage, edge, grayImage;
	dstImage.create(src.size(), src.type()); //创建与源文件同大小和类型的矩阵
	cvtColor(src, grayImage, COLOR_BGR2GRAY);//将RGB图像转换为灰度图

	blur(grayImage, edge, Size(3, 3)); //使用3*3内核降噪

	Canny(edge, edge, 3, 9, 3);//使用cnny算子检测边缘
	cv::imshow("效果图", edge);
	cv::waitKey(0);
}
void CVCode::CVTest4() //读取播放视频
{
	//VideoCapture capture;
	//capture.open("vtest.avi");
	VideoCapture capture("vtest.avi");
	Mat edges;

	while (1)
	{
		Mat frame;
		capture >> frame; //从视频中获取一帧
		cvtColor(frame, edges, COLOR_BGR2GRAY);//转换成灰度图
		blur(edges, edges, Size(7, 7));//图像降噪
		Canny(edges, edges, 0, 30, 3);//边缘检测

		imshow("读取视频", edges);
		if (waitKey(30) >= 0)
		{
			break;
		}
	}
}
void CVCode::CVTest5()//生成并保存图像
{
	//初始化
	Mat mat(480, 640, CV_8UC4);

	//填充图像
	for (int i = 0; i < mat.rows; i++)
	{
		for (int j = 0; j < mat.cols; j++)
		{
			Vec4b& rgba = mat.at<Vec4b>(i, j);
			rgba[0] = UCHAR_MAX;
			rgba[1] = saturate_cast<uchar>((float(mat.cols - j)) / ((float)mat.cols) * UCHAR_MAX);
			rgba[2] = saturate_cast<uchar>((float(mat.rows - i)) / ((float)mat.rows) * UCHAR_MAX);
			rgba[3] = saturate_cast<uchar>(0.5 * (rgba[1] + rgba[2]));
		}
	}

	vector<int>compression_params;
	compression_params.push_back(IMWRITE_PNG_COMPRESSION);
	compression_params.push_back(9);
	try
	{
		imwrite("透明alpha值图.png", mat, compression_params);
		imshow("生成的PNG图", mat);
		waitKey(0);
	}
	catch (Exception ex)
	{
		cout << ex.msg;
	}
}
void CVCode::CVTest6()//图像融合
{
	Mat image = imread("starry_night.jpg");
	Mat lena = imread("lena.jpg");

	namedWindow("[2]原画图");
	imshow("[2]原画图", image);

	namedWindow("[3]lena图");
	imshow("[3]lena图", lena);

	//ROI为指向原图一块区域的指针
	Mat imageROI = image(Rect(10, 10, lena.cols, lena.rows));

	//将图像附加到ROI指向的内存
	addWeighted(imageROI, 0.5, lena, 0.3, 0, imageROI);

	namedWindow("[4]原画+lena图");
	imshow("[4]原画+lena图", image);
	waitKey(0);
}

void CVCode::DrawEllipse(Mat img, double angle)
{
	int thickness = 2;
	int linetype = 8;
	ellipse(img,
		Point(WINDOW_WIDTH / 2, WINDOW_WIDTH / 2),
		Size(WINDOW_WIDTH / 4, WINDOW_WIDTH / 16),
		angle,
		0,
		360,
		Scalar(255, 129, 0),
		thickness,
		linetype);
}
void CVCode::DrawFilledCircle(Mat img, Point center)
{
	int thickness = -1;
	int linetype = 8;
	circle(img,
		center,
		WINDOW_WIDTH / 32,
		Scalar(0, 0, 255),
		thickness,
		linetype);
}
void CVCode::DrawPolygon(Mat img)
{
	int linetype = 8;
	Point rookPoints[1][20];
	rookPoints[0][0] = Point(WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8);
	rookPoints[0][1] = Point(3 * WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8);
	rookPoints[0][2] = Point(3 * WINDOW_WIDTH / 4, 13 * WINDOW_WIDTH / 16);
	rookPoints[0][3] = Point(11 * WINDOW_WIDTH / 16, 13 * WINDOW_WIDTH / 16);
	rookPoints[0][4] = Point(19 * WINDOW_WIDTH / 32, 3 * WINDOW_WIDTH / 8);
	rookPoints[0][5] = Point(3 * WINDOW_WIDTH / 4, 3 * WINDOW_WIDTH / 8);
	rookPoints[0][6] = Point(3 * WINDOW_WIDTH / 4, WINDOW_WIDTH / 8);
	rookPoints[0][7] = Point(26 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8);
	rookPoints[0][8] = Point(26 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4);
	rookPoints[0][9] = Point(22 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4);
	rookPoints[0][10] = Point(22 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8);
	rookPoints[0][11] = Point(18 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8);
	rookPoints[0][12] = Point(18 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4);
	rookPoints[0][13] = Point(14 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4);
	rookPoints[0][14] = Point(14 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8);
	rookPoints[0][15] = Point(WINDOW_WIDTH / 4, WINDOW_WIDTH / 8);
	rookPoints[0][16] = Point(WINDOW_WIDTH / 4, 3 * WINDOW_WIDTH / 8);
	rookPoints[0][17] = Point(13 * WINDOW_WIDTH / 32, 3 * WINDOW_WIDTH / 8);
	rookPoints[0][18] = Point(5 * WINDOW_WIDTH / 16, 13 * WINDOW_WIDTH / 16);
	rookPoints[0][19] = Point(WINDOW_WIDTH / 4, 13 * WINDOW_WIDTH / 16);

	const Point* ppt[1] = { rookPoints[0] };
	int npt[] = { 20 };
	fillPoly(img,
		ppt, npt,
		1,
		Scalar(255, 255, 255),
		linetype);
}
void CVCode::DrawLine(Mat img, Point start, Point end)
{
	int thickness = 2;
	int lineType = 8;
	line(img,
		start,
		end,
		Scalar(0, 0, 0),
		thickness,
		lineType);
}
void CVCode::CVTest7()//图形绘制
{
	Mat atomImage = Mat::zeros(WINDOW_WIDTH, WINDOW_WIDTH, CV_8UC3);
	Mat rookImage = Mat::zeros(WINDOW_WIDTH, WINDOW_WIDTH, CV_8UC3);

	DrawEllipse(atomImage, 90);
	DrawEllipse(atomImage, 0);
	DrawEllipse(atomImage, 45);
	DrawEllipse(atomImage, -45);

	DrawFilledCircle(atomImage, Point(WINDOW_WIDTH / 2, WINDOW_WIDTH / 2));

	DrawPolygon(rookImage);
	rectangle(rookImage,
		Point(0, 7 * WINDOW_WIDTH / 8),
		Point(WINDOW_WIDTH, WINDOW_WIDTH),
		Scalar(0, 255, 255),
		-1,
		8);
	DrawLine(rookImage, Point(0, 15 * WINDOW_WIDTH / 16), Point(WINDOW_WIDTH, 15 * WINDOW_WIDTH / 16));
	DrawLine(rookImage, Point(WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8), Point(WINDOW_WIDTH / 4, WINDOW_WIDTH));
	DrawLine(rookImage, Point(WINDOW_WIDTH / 2, 7 * WINDOW_WIDTH / 8), Point(WINDOW_WIDTH / 2, WINDOW_WIDTH));
	DrawLine(rookImage, Point(3 * WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8), Point(3 * WINDOW_WIDTH / 4, WINDOW_WIDTH));

	imshow("原子图", atomImage);
	moveWindow("原子图", 0, 200);
	imshow("rook图", rookImage);
	moveWindow("rook图", WINDOW_WIDTH, 200);
	waitKey(0);
}

void CVCode::colorReduce(Mat& inputImage, Mat& outputImage, int div)
{
	outputImage = inputImage.clone();
	int rowCount = outputImage.rows;
	int colCount = outputImage.cols * outputImage.channels();
	//列数*通道数=等于一行的元素个数

	for (int i = 0; i < rowCount; i++)
	{
		uchar* data = outputImage.ptr<uchar>(i);
		//ptr函数可以获取任意行元素的首地址
		for (int j = 0; j < colCount; j++)
		{
			data[j] = data[j] / div * div + div / 2;
		}
	}

}

void CVCode::CVTest8()//颜色空间缩减 图像元素的遍历和计算
{
	//14/10*10=1*10=10 //整数除法会自动取整 利用此原理进行颜色空间缩减

	Mat srcImg = imread("lena.jpg");
	Mat dstImg;
	dstImg.create(srcImg.rows, srcImg.cols, srcImg.type());

	double time0 = static_cast<double>(getTickCount());
	colorReduce(srcImg, dstImg, 32);

	time0 = ((double)getTickCount() - time0) / getTickFrequency();
	cout << "图像处理用时:" << time0 << endl;
	imshow("原图", srcImg);
	imshow("效果图", dstImg);
	waitKey(0);
}

CVCode* _stdcall CreateCVHandle()
{
	CVCode* newImp = new CVCode();
	return newImp;
}