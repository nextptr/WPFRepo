#pragma once
#include "pch.h"
#include "CVCode.h"

using namespace System;

namespace CVWrapper
{
	public ref class CVproxy
	{
	private:
		CVCode* instanceObj = nullptr;
		CVproxy()
		{
			instanceObj = CreateCVHandle();
		}
	public:
		static CVproxy^ CVproxyInstance = nullptr;
		static void CreateInstance()
		{
			if (CVproxyInstance == nullptr)
			{
				CVproxyInstance = gcnew CVproxy();
			}
		}

	public:
		void CVTest1();

	};
}

