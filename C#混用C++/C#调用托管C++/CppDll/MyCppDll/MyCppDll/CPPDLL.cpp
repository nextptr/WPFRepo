#include "stdafx.h"
#include "CppDll.h"

CppClass::CppClass()
{
}

CppClass::~CppClass()
{
}

double CppClass::Add(double src, double dst)
{
	return src += dst;
}
double CppClass::Min(double src, double dst)
{
	return src -= dst;
}
double CppClass::Mut(double src, double dst)
{
	return src *= dst;
}
double CppClass::Sub(double src, double dst)
{
	return src /= dst;
}