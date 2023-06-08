#include "pch.h"
#include "CVCode.h"
#include "CVproxy.h"

using namespace System;
using namespace Runtime::InteropServices;
using namespace CVWrapper;

void CVproxy::CVTest1(System::String^ pth)
{
	char* chars = (char*)(Marshal::StringToHGlobalAnsi(pth).ToPointer());
	instanceObj->CVTest1(chars);
}
void CVproxy::AttachPrint(System::String^ pth, System::String^ txt)
{
	char* chars = (char*)(Marshal::StringToHGlobalAnsi(pth).ToPointer());
	char* text = (char*)(Marshal::StringToHGlobalAnsi(txt).ToPointer());
	instanceObj->DrawMark(chars, text);
}
void CVproxy::ToolBGR2Gray(Mat_CPP^ src, Mat_CPP^% dst)
{
	/*Mat_CV matSrc = Mat_CV(src->Width, src->Height, src->Type);
	matSrc.data= (uchar*)src->Ptr.ToPointer();

	Mat_CV matDst;
	instanceObj->ToolBGR2Gray(matSrc, matDst);
	dst->Width = src->Width;
	dst->Height = src->Height;
	dst->Type = ImgType_CPP::Gray8;
	dst->Ptr = (IntPtr)matDst.data;*/

	Mat_CV matSrc = TypeConvertHelper::Create(src);

	Mat_CV matDst;
	matDst.img_type = ImgType_CPP::Gray8;

	instanceObj->ToolColorConvert(matSrc, matDst);

	TypeConvertHelper::CopyTo_CPP(matDst, dst);
}