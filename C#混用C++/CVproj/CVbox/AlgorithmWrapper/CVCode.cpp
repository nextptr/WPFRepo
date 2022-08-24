#pragma once
#include "pch.h"
#include "CVCode.h"

void CVCode::CVTest1() //��ʴ
{
	string filename = "lena.jpg";
	cv::Mat src = cv::imread(filename, 1);
	cv::imshow("ԭͼ", src);

	Mat element = getStructuringElement(MORPH_ERODE, Size(15, 15));
	Mat dstImage;
	erode(src, dstImage, element);
	cv::imshow("Ч��ͼ", dstImage);
	cv::waitKey(0);
}
void CVCode::CVTest2() //ģ��
{
	string filename = "lena.jpg";
	cv::Mat src = cv::imread(filename, 1);
	cv::imshow("ԭͼ", src);

	Mat dstImage;
	blur(src, dstImage, Size(7, 7));
	cv::imshow("Ч��ͼ", dstImage);
	cv::waitKey(0);
}
void CVCode::CVTest3() //��Եʶ��
{
	string filename = "lena.jpg";
	cv::Mat src = cv::imread(filename, 1);
	cv::imshow("ԭͼ", src);

	Mat dstImage, edge, grayImage;
	dstImage.create(src.size(), src.type()); //������Դ�ļ�ͬ��С�����͵ľ���
	cvtColor(src, grayImage, COLOR_BGR2GRAY);//��RGBͼ��ת��Ϊ�Ҷ�ͼ

	blur(grayImage, edge, Size(3, 3)); //ʹ��3*3�ں˽���

	Canny(edge, edge, 3, 9, 3);//ʹ��cnny���Ӽ���Ե
	cv::imshow("Ч��ͼ", edge);
	cv::waitKey(0);
}
void CVCode::CVTest4() //��ȡ������Ƶ
{
	//VideoCapture capture;
	//capture.open("vtest.avi");
	VideoCapture capture("vtest.avi");
	Mat edges;

	while (1)
	{
		Mat frame;
		capture >> frame; //����Ƶ�л�ȡһ֡
		cvtColor(frame, edges, COLOR_BGR2GRAY);//ת���ɻҶ�ͼ
		blur(edges, edges, Size(7, 7));//ͼ����
		Canny(edges, edges, 0, 30, 3);//��Ե���

		imshow("��ȡ��Ƶ", edges);
		if (waitKey(30) >= 0)
		{
			break;
		}
	}
}
void CVCode::CVTest5()//���ɲ�����ͼ��
{
	//��ʼ��
	Mat mat(480, 640, CV_8UC4);

	//���ͼ��
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
		imwrite("͸��alphaֵͼ.png", mat, compression_params);
		imshow("���ɵ�PNGͼ", mat);
		waitKey(0);
	}
	catch (Exception ex)
	{
		cout << ex.msg;
	}
}
void CVCode::CVTest6()//ͼ���ں�
{
	Mat image = imread("starry_night.jpg");
	Mat lena = imread("lena.jpg");

	namedWindow("[2]ԭ��ͼ");
	imshow("[2]ԭ��ͼ", image);

	namedWindow("[3]lenaͼ");
	imshow("[3]lenaͼ", lena);

	//ROIΪָ��ԭͼһ�������ָ��
	Mat imageROI = image(Rect(10, 10, lena.cols, lena.rows));

	//��ͼ�񸽼ӵ�ROIָ����ڴ�
	addWeighted(imageROI, 0.5, lena, 0.3, 0, imageROI);

	namedWindow("[4]ԭ��+lenaͼ");
	imshow("[4]ԭ��+lenaͼ", image);
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
void CVCode::CVTest7()//ͼ�λ���
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

	imshow("ԭ��ͼ", atomImage);
	moveWindow("ԭ��ͼ", 0, 200);
	imshow("rookͼ", rookImage);
	moveWindow("rookͼ", WINDOW_WIDTH, 200);
	waitKey(0);
}

void CVCode::colorReduce(Mat& inputImage, Mat& outputImage, int div)
{
	outputImage = inputImage.clone();
	int rowCount = outputImage.rows;
	int colCount = outputImage.cols * outputImage.channels();
	//����*ͨ����=����һ�е�Ԫ�ظ���

	for (int i = 0; i < rowCount; i++)
	{
		uchar* data = outputImage.ptr<uchar>(i);
		//ptr�������Ի�ȡ������Ԫ�ص��׵�ַ
		for (int j = 0; j < colCount; j++)
		{
			data[j] = data[j] / div * div + div / 2;
		}
	}

}

void CVCode::CVTest8()//��ɫ�ռ����� ͼ��Ԫ�صı����ͼ���
{
	//14/10*10=1*10=10 //�����������Զ�ȡ�� ���ô�ԭ�������ɫ�ռ�����

	Mat srcImg = imread("lena.jpg");
	Mat dstImg;
	dstImg.create(srcImg.rows, srcImg.cols, srcImg.type());

	double time0 = static_cast<double>(getTickCount());
	colorReduce(srcImg, dstImg, 32);

	time0 = ((double)getTickCount() - time0) / getTickFrequency();
	cout << "ͼ������ʱ:" << time0 << endl;
	imshow("ԭͼ", srcImg);
	imshow("Ч��ͼ", dstImg);
	waitKey(0);
}

CVCode* _stdcall CreateCVHandle()
{
	CVCode* newImp = new CVCode();
	return newImp;
}