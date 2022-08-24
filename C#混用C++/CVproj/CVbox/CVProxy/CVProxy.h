#pragma once
#include "dll_sift.h"
using namespace System;

namespace CVAlgorithm
{
	public ref class CVProxy
	{
	private:
		CVCode* instanceObj = nullptr;
		CVProxy()
		{
			instanceObj = CreateCVCodeHandle();
		}

	public:
		static CVProxy^ CVProxyInstance = nullptr;
		static void CreateInstance()
		{
			if (CVProxyInstance == nullptr)
			{
				CVProxyInstance = gcnew CVProxy();
			}
		}

	public:
		void Test();
	};
}