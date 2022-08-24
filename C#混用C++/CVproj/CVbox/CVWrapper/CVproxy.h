#pragma once
#include "CVCode.h"

using namespace System;

namespace CVWrapper
{
	public ref struct Mat_CPP
	{
	public:
		ImgType_CPP Type;
		IntPtr Ptr;
		int Width;
		int Height;
	public:
		Mat_CPP()
		{
		}
		Mat_CPP(IntPtr p, ImgType_CPP tp, int wid,int hig)
		{
			Ptr = p;
			Type = tp;
			Width = wid;
			Height = hig;
		}
	};

	static class TypeConvertHelper
	{
	public:
		static void CopyTo_CPP(Mat_CV& mat, Mat_CPP^% dst)
		{
			dst->Width = mat.img_w;
			dst->Height = mat.img_h;
			dst->Type = mat.img_type;
			dst->Ptr = (IntPtr)mat.data;
		}

		static Mat_CV Create(Mat_CPP^% mat)
		{
			Mat_CV ret = Mat_CV(mat->Width, mat->Height, mat->Type);
			ret.data = (uchar*)mat->Ptr.ToPointer();
			return ret;
		}
	};


	public ref class CVproxy
	{
	private:
		CVCode* instanceObj = nullptr;
		CVproxy()
		{
			instanceObj = CreateCVHandle();
		}
	public :
		static CVproxy^ CVproxyInstance = nullptr;
		static void CreateInstance()
		{
			if (CVproxyInstance == nullptr)
			{
				CVproxyInstance = gcnew CVproxy();
			}
		}

	public:
		void CVTest1(System::String^ pth);
		void ToolBGR2Gray(Mat_CPP^ src, Mat_CPP^% dst);
	};
}

